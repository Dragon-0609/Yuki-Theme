﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Yuki_Theme.Core.Forms
{
	public partial class DownloadForm : Form
	{
		private readonly MForm     form;
		public           string    downloadlink;
		public           string    size;
		private          WebClient web;

		private string user_agent =
			"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.71 Safari/537.36";

		public DownloadForm (MForm fm)
		{
			InitializeComponent ();
			form = fm;
			form.OnColorUpdate += (bg, fg, clicked) => {
				BackColor = button1.BackColor = button1.FlatAppearance.MouseDownBackColor = bg;
				ForeColor = button1.FlatAppearance.BorderColor = fg;
				button1.FlatAppearance.MouseOverBackColor = clicked;
			};
		}

		public async void CheckUpdate ()
		{
			var url = "https://api.github.com/repos/Dragon-0609/Yuki-Theme/releases/latest";
			try
			{
				using (var client = new HttpClient ())
				{
					client.DefaultRequestHeaders.Add ("User-Agent",
					                                  user_agent);
					var response = await client.GetAsync (url);
					if (response != null)
					{
						var json = await response.Content.ReadAsStringAsync ();
						// Console.WriteLine (json);

						var jresponse = JObject.Parse (json);
						string tg = jresponse ["tag_name"].ToString ();
						string nv = tg.Substring (1, tg.Length - 3);
						Console.WriteLine (nv);
						
						double ver = double.Parse (nv, CultureInfo.InvariantCulture);
						Console.WriteLine (ver);
						if (SettingsForm.current_version < ver)
						{
							int md = (int) Helper.mode;
							size = jresponse ["assets"] [md] ["size"].ToString ();
							size = string.Format ("{0:0.0} MB", double.Parse (size) / 1024 / 1024);
							downloadlink = jresponse ["assets"] [md] ["browser_download_url"].ToString ();
							form.nf.button1.Visible = true;
							form.nf.button1.Click += startUpdate;
							form.nf.button1.Text = "Update";
							form.nf.changeContent ("New version is available", $"Yuki theme {ver} \n Size: {size}");
							form.nf.Show (form);
							lock (SettingsForm.next_version)
							{
								SettingsForm.next_version = $"{ver} | {size}";								
							}
							form.changeNotificationLocation ();
							size = jresponse ["assets"] [md] ["size"].ToString ();
							size = string.Format ("{0:0.0}", double.Parse (size) / 1024 / 1024);
						} else
						{
							form.ShowNotification ("Up to date", "Your version is the latest.");
							form.nf.button1.Visible = false;
							form.changeNotificationLocation ();
						}
					}
				}
			} catch (Exception)
			{
				// Console.WriteLine (ex.Message);
			}
		}

		private void startUpdate (object sender, EventArgs e)
		{
			form.nf.button1.Click -= startUpdate;
			// Console.WriteLine ("Update is started");
			
			form.showDownloader ();
			form.df.downloadlink = downloadlink;
			form.df.size = size;
			if (!File.Exists(Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "Yuki Theme",
			                               "yuki_theme.zip")))
			{
				form.df.downl.ClickHere (e);
			} else
			{
				var fi = new FileInfo (Path.Combine (
					                       Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
					                       "Yuki Theme", "yuki_theme.zip"));
				string siz = string.Format ("{0:0.0}", fi.Length / 1024 / 1024.0);
				if (siz != size)
				{
					form.df.downl.ClickHere (e);
				} else
				{
					startUpdating ();
				}
			}
		}

		public void InstallManually ()
		{
			form.showDownloader ();
			form.df.downloadlink = downloadlink;
			form.df.size = size;
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
			// Console.WriteLine(e.Error.Message);
			if (e.Cancelled)
			{
				form.ShowNotification ("Canceled", $"Downloading is canceled");
				form.nf.button1.Visible = false;
				form.changeNotificationLocation ();
			}else
			{
				form.ShowNotification ("New version is downloaded", $"Installing...The program will be closed.");
				form.nf.button1.Visible = false;
				form.changeNotificationLocation ();
				startUpdating ();
			}
		}

		private void startUpdating ()
		{
			Preparer prep = new Preparer ();
			prep.prepare ();
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
	}
}