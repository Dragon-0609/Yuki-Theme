using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Yuki_Theme.Core.Utils;
using Yuki_Theme.Core.WPF.Interfaces;

namespace Yuki_Theme.Core.WPF.Windows
{
	public class UpdateDownloaderModel : UpdateDownloader.IModel
	{
		private const string user_agent =
			"Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.71 Safari/537.36";

		private string _githubUrl      = "https://github.com/Dragon-0609/Yuki-Theme/releases/tag/";
		private string _downloadUrl    = "";
		private string _formattedSize   = "";
		private string _approximateSize = "";
		private string _version         = "";

		private WebClient _downloader;
		
		public async void CheckVersion (string url, Action<bool> afterParsing)
		{
			using (var client = new HttpClient ())
			{
				client.DefaultRequestHeaders.Add ("User-Agent", user_agent);
				var response = await client.GetAsync (url);
				if (response != null)
				{
					string json = await response.Content.ReadAsStringAsync ();
					JObject jresponse;
					if (Settings.Beta) // If can get beta, parse latest release (even pre-release)
					{
						jresponse = (JObject)JArray.Parse (json).First;
					} else
					{
						jresponse = JObject.Parse (json);
					}


					string tg = jresponse["tag_name"].ToString ();

					ChangeGithubUrl (url, tg);

					string nextVersion = "";
					bool hasBeta = tg.Contains ("-");
					nextVersion = ParseVersion (hasBeta, tg);

					double nextVer = double.Parse (nextVersion, CultureInfo.InvariantCulture);
					bool available = Settings.current_version < nextVer || (Math.Abs (Settings.current_version - nextVer) >= 0.1 &&
																		Settings.current_version_add.Length != 0 && !hasBeta);
					if (available)
					{
						PrepareForDownloading (jresponse);
					}

					afterParsing (available);
				}
			}
		}

		public string GetVersion () => _version;
		public string GetSize () => _formattedSize;
		public string GetGithubUrl () => _githubUrl;

		private static string ParseVersion (bool hasBeta, string tg)
		{
			string nv;
			if (hasBeta)
			{
				nv = tg.Split ('-')[0];
				nv = nv.Substring (1, nv.Length - 3);
			} else
			{
				nv = tg.Substring (1, tg.Length - 3);
			}

			return nv;
		}

		private void ChangeGithubUrl (string url, string tg)
		{
			if (url.EndsWith ("latest"))
			{
				_githubUrl = "https://github.com/Dragon-0609/Yuki-Theme/releases/latest";
			} else
			{
				_githubUrl = "https://github.com/Dragon-0609/Yuki-Theme/releases/tag/" + tg;
			}
		}

		private void PrepareForDownloading (JObject jresponse)
		{
			int md = (int)Helper.mode;
			_formattedSize = jresponse["assets"]?[md]?["size"]?.ToString ();
			_formattedSize = string.Format ("{0:0.0} MB", double.Parse (_formattedSize) / 1024 / 1024);
			_downloadUrl = jresponse["assets"][md]["browser_download_url"].ToString ();

			_version = jresponse["name"].ToString ();
			if (_version.StartsWith ("v")) _version = _version.Substring (1);

			_approximateSize = jresponse["assets"][md]["size"].ToString ();
			_approximateSize = $"{double.Parse (_approximateSize) / 1024 / 1024:0.0}";
		}
		
		public void StartUpdating (DownloadProgressChangedEventHandler progressChanged, AsyncCompletedEventHandler downloadingCompleted)
		{
			if (IsUpdateDownloaded () && !IsValidUpdate (null)) // update downloaded, but it isn't valid
			{
				File.Delete (GetUpdatePath ());
			}

			DownloadOrUpdate (progressChanged, downloadingCompleted);
		}

		private void DownloadOrUpdate (DownloadProgressChangedEventHandler progressChanged, AsyncCompletedEventHandler downloadingCompleted)
		{
			if (!File.Exists (GetUpdatePath ()))
			{
				StartDownloading (progressChanged, downloadingCompleted);
			} else
			{
				FileInfo fi = new FileInfo (GetUpdatePath ());
				string siz = $"{fi.Length / 1024 / 1024.0:0.0}";
				if (siz != _approximateSize)
				{
					StartDownloading (progressChanged, downloadingCompleted);
				} else
				{
					StartUpdating ();
				}
			}
		}

		private void StartUpdating ()
		{
			InstallationPreparer prep = new ();
			prep.Prepare (true);
		}

		private void StartDownloading (DownloadProgressChangedEventHandler progressChanged, AsyncCompletedEventHandler downloadingCompleted)
		{
			if (!Directory.Exists (GetUpdateDirectory ()))
				Directory.CreateDirectory (GetUpdateDirectory ());

			string destination = GetUpdatePath ();
			_downloader = new WebClient ();
			_downloader.Headers.Add ("User-Agent",
				user_agent);
			_downloader.DownloadFileCompleted += downloadingCompleted;
			_downloader.DownloadProgressChanged += progressChanged;
			_downloader.DownloadFileAsync (new Uri (_downloadUrl), destination);
		}

		
		#region Helper Methods

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
		

		private static string GetUpdateDirectory ()
		{
			return Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "Yuki Theme");
		}
		
		private static string GetUpdatePath ()
		{
			return Path.Combine (GetUpdateDirectory (), "yuki_theme.zip");
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
		
		

		#endregion
	}
}