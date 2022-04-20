#define CONSOLE_LOGS
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Yuki_Theme.Core.Forms
{
	public partial class DownloadForm : Form
	{
		private readonly PopupFormsController     popupController;
		public           string    downloadlink;
		public           string    size;
		private          WebClient web;
		private          string    github_url = "https://github.com/Dragon-0609/Yuki-Theme/releases/tag/";

		private string user_agent =
			"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.71 Safari/537.36";

		public DownloadForm (PopupFormsController controller)
		{
			InitializeComponent ();
			button1.Text = Translate ("download.cancel");
			popupController = controller;
			popupController.colorUpdatable.OnColorUpdate += (bg, fg, clicked) => {
				BackColor = button1.BackColor = button1.FlatAppearance.MouseDownBackColor = bg;
				ForeColor = button1.FlatAppearance.BorderColor = fg;
				button1.FlatAppearance.MouseOverBackColor = clicked;
			};
		}

		public async void CheckUpdate ()
		{
			if (IsValidUpdate (null))
			{
				QuestionForm quform = new QuestionForm ();
				quform.EditMessage (Translate ("download.downloaded.already"), Translate ("download.downloaded.already.restarttoinstall"), Translate ("download.buttons.install"), Translate ("download.buttons.later"));
				if (quform.ShowDialog(popupController.form) == DialogResult.Yes)
				{
					startUpdating ();
				} else
				{
					popupController.CloseDownloader ();
				}
			}else
			{
				var url = Settings.Beta
					? "https://api.github.com/repos/dragon-0609/yuki-theme/releases"
					: "https://api.github.com/repos/Dragon-0609/Yuki-Theme/releases/latest";
				try
				{
					using (var client = new HttpClient ())
					{
						client.DefaultRequestHeaders.Add ("User-Agent",
						                                  user_agent);
						var response = await client.GetAsync (url);
						if (response != null)
						{
							string json = await response.Content.ReadAsStringAsync ();
							Console.WriteLine (json);
							JObject jresponse;
							if (Settings.Beta) // If can get beta, parse latest release (even pre-release)
							{
								jresponse = (JObject)JArray.Parse (json).First;
							} else
							{
								jresponse = JObject.Parse (json);
							}
								

							string tg = jresponse ["tag_name"].ToString ();
#if CONSOLE_LOGS
							Console.WriteLine (json);
							Console.WriteLine (tg);
#endif
							if (url.EndsWith ("latest"))
							{
								github_url = "https://github.com/Dragon-0609/Yuki-Theme/releases/latest";
							} else
							{
								github_url = "https://github.com/Dragon-0609/Yuki-Theme/releases/tag/" + tg;
							}

							string nv = "";
							bool hasBeta = false;
							if (tg.Contains ("-"))
							{
								nv = tg.Split ('-') [0];
#if CONSOLE_LOGS
								Console.WriteLine (nv);
#endif
								nv = nv.Substring (1, nv.Length - 3);
								hasBeta = true;
							} else
							{
								nv = tg.Substring (1, tg.Length - 3);
							}

							double ver = double.Parse (nv, CultureInfo.InvariantCulture);

#if CONSOLE_LOGS
							Console.WriteLine (nv);
							Console.WriteLine (ver);
#endif
							if (Settings.current_version < ver ||
							    (Settings.current_version == ver && Settings.current_version_add.Length != 0 && !hasBeta))
							{
								int md = (int)Helper.mode;
								size = jresponse ["assets"] [md] ["size"].ToString ();
								size = string.Format ("{0:0.0} MB", double.Parse (size) / 1024 / 1024);
								downloadlink = jresponse ["assets"] [md] ["browser_download_url"].ToString ();
								popupController.CloseNotification ();
								popupController.InitializeAllWindows ();
								
								popupController.nf.onClick = startUpdate;
								popupController.nf.onClick2 = openInGithub;
								popupController.nf.button1.Text = Translate ("download.buttons.update");
								popupController.nf.button3.Text = Translate ("download.buttons.github");
								string sw = jresponse ["name"].ToString ();
								if (sw.StartsWith ("v")) sw = sw.Substring (1);
								popupController.nf.changeContent (Translate ("download.available"), $"Yuki theme {sw}      {Translate ("download.size")}: {size}");

								popupController.nf.button1.Visible = true;
								popupController.nf.button3.Visible = true;

								popupController.nf.Show (popupController.form);
								lock (Settings.next_version)
								{
									Settings.next_version = $"{ver} | {size}";
								}

								popupController.changeNotificationLocation ();
								size = jresponse ["assets"] [md] ["size"].ToString ();
								size = $"{double.Parse (size) / 1024 / 1024:0.0}";
							} else
							{
								popupController.ShowNotification (Translate ("download.uptodate"), Translate ("download.latest"));
								popupController.nf.button1.Visible = false;
								popupController.nf.button3.Visible = false;
								popupController.changeNotificationLocation ();
							}
						}
					}
				} catch (Exception ex)
				{
					Console.WriteLine (ex.Message + "\n" + ex.StackTrace);
				}
			}
		}

		private void openInGithub ()
		{
			popupController.nf.onClick = null;
			popupController.nf.onClick2 = null;
			Process.Start (github_url);
		}
		
		private void startUpdate ()
		{
			if (IsUpdateDownloaded () && !IsValidUpdate (null)) // update downloaded, but it isn't valid
			{
				File.Delete (GetUpdatePath ());
			}
			popupController.nf.onClick = null;
			popupController.nf.onClick2 = null;
			// Console.WriteLine ("Update is started");
			
			popupController.showDownloader ();
			popupController.df.downloadlink = downloadlink;
			popupController.df.size = size;
			if (!File.Exists(Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "Yuki Theme",
			                               "yuki_theme.zip")))
			{
				popupController.df.downl.ClickHere (EventArgs.Empty);
			} else
			{
				var fi = new FileInfo (Path.Combine (
					                       Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "Yuki Theme",
					                       "yuki_theme.zip"));
				string siz = string.Format ("{0:0.0}", fi.Length / 1024 / 1024.0);
				if (siz != size)
				{
					popupController.df.downl.ClickHere (EventArgs.Empty);
				} else
				{
					startUpdating ();
				}
			}
		}

		public void InstallManually ()
		{
			popupController.showDownloader ();
			popupController.df.downloadlink = downloadlink;
			popupController.df.size = size;
			startUpdating ();
		}
		
		private void ProgressChanged (object sender, DownloadProgressChangedEventArgs e)
		{
			// Console.WriteLine(e.ProgressPercentage);
			int per = e.ProgressPercentage;
			progressBar1.Value = per;
			double dubl = e.BytesReceived / 1024 / 1024.0;
			pr_mb.Text = string.Format ("{0:0.0} MB / {1} MB", dubl, size);
			pr.Text = $"{per}%";
		}

		private void DownloadCompleted (object sender, AsyncCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				string title = Translate ("download.refused.error");
				string message = e.Error.Message;
				
				if(e.Error.InnerException != null)
				{
					if (e.Error.InnerException is SocketException)
					{
						title = Translate ("download.refused.title");
						message = Translate ("download.refused.message");
					} else
					{
						message = e.Error.InnerException.Message;
					}
				}

				if (e.Error is WebException)
				{
					popupController.ShowNotification (title, message);
					popupController.nf.onClick = openInGithub;
					popupController.nf.onClick2 = null;
					popupController.nf.button1.Text = Translate ("download.buttons.github");
					popupController.nf.button1.Visible = true;
					
					popupController.changeNotificationLocation ();
					popupController.CloseDownloader ();
				}
				else
					throw e.Error;
			}else
			{
				// Console.WriteLine(e.Error.Message);
				if (e.Cancelled)
				{
					popupController.ShowNotification (Translate ("download.canceled.title"), Translate ("download.canceled.message"));
					popupController.nf.button1.Visible = false;
					popupController.changeNotificationLocation ();
					popupController.CloseDownloader ();
				} else
				{
					QuestionForm quform = new QuestionForm ();
					quform.EditMessage (Translate ("download.downloaded.short"), Translate ("download.downloaded.full"),
					                    Translate ("download.buttons.install"), Translate ("download.buttons.later"));
					if (quform.ShowDialog(popupController.form) == DialogResult.Yes)
					{
						startUpdating ();
					} else
					{
						popupController.CloseDownloader ();
					}
				}
			}
		}

		public void startUpdating ()
		{
			Preparer prep = new Preparer ();
			prep.prepare (true);
		}

		private void button1_Click (object sender, EventArgs e)
		{
			web.CancelAsync ();
		}

		private void downl_Click (object sender, EventArgs e)
		{
			// Download ();
			if (!Directory.Exists (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
			                                    "Yuki Theme")))
				Directory.CreateDirectory (
					Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "Yuki Theme"));
			string dest = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "Yuki Theme",
			                            "yuki_theme.zip");
			web = new WebClient ();
			web.Headers.Add ("User-Agent",
			                    user_agent);
			web.DownloadFileCompleted += new AsyncCompletedEventHandler (DownloadCompleted);
			web.DownloadProgressChanged += new DownloadProgressChangedEventHandler (ProgressChanged);
			// Console.WriteLine($"{downloadlink}, {dest}");
			web.DownloadFileAsync (new Uri (downloadlink), dest);
		}

		private void DownloadForm_FormClosing (object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
			this.Parent = null;
		}

		private void DownloadForm_Shown (object sender, EventArgs e)
		{
			ForeColor = button1.FlatAppearance.BorderColor = Helper.fgColor;
			BackColor = Helper.bgColor;
			
			button1.FlatAppearance.MouseOverBackColor = Helper.bgClick;
		}

		
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
			ControlPaint.DrawBorder (e.Graphics, ClientRectangle, Helper.bgBorder, ButtonBorderStyle.Solid);
		}
		
		public static bool IsUpdateDownloaded (string path = null)
		{
			path ??= GetUpdatePath ();
			int lng = 0;
			if (File.Exists (path))
			{
				lng = Convert.ToInt32 ((new FileInfo (path).Length / 1024) / 1024);
				Console.WriteLine (lng);
			}
			return lng > 1;
		}

		private string Translate (string key)
		{
			return CLI.Translate (key);
		}

		private static string GetUpdatePath ()
		{
			return Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "Yuki Theme",
			                     "yuki_theme.zip");
		}

		public static bool IsValidUpdate (string path)
		{
			path ??= GetUpdatePath ();
			bool valid = false;

			if (IsUpdateDownloaded (path))
			{
				bool has = ZipHasFile ("Yuki Theme.Core.dll", path);
				if (has)
				{
					has = ZipHasFile ("Newtonsoft.Json.dll", path);
					if (has)
					{
						has = ZipHasFile ("FastColoredTextBox.dll", path);
						valid = has;
					}
				}
			}
			
			return valid;
		}
		
		public static bool ZipHasFile (string fileFullName, string zipFullPath)
		{
			using (ZipArchive archive = ZipFile.OpenRead (zipFullPath))
			{
				foreach (ZipArchiveEntry entry in archive.Entries)
				{
					if (entry.FullName.EndsWith (fileFullName, StringComparison.Ordinal))
					{
						return true;
					}
				}
			}

			return false;
		}
		
	}
}