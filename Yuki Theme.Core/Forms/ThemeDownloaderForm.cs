// #define PRIMARLY

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yuki_Theme.Core.Parsers;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.Forms;

[ComVisible (true)]
public partial class ThemeDownloaderForm : Form
{
	private const string THEME_NAMESPACE = "Yuki_Theme.Core.Resources.HTML.";

	private string        branch     = "master";
	private int           searchMode = 0;
	private List <string> branches   = new List <string> ();

	private bool        isFromBranch = false;
	private Task        loadingJSONSTask;
	private HtmlElement themesElement;

	private double stepProgress;
	private double currentProgress;
	private bool   needToReset = false;

	private bool htmlLoaded = false;

	private Dictionary <string, string> themesWithUrls    = new ();
	private Dictionary <string, string> branchCommitDates = new ();
	private Dictionary <string, bool>   newThemes         = new ();
	private Dictionary <string, Theme>  themes            = new ();

	private List <string> downloadingErrors = new List <string> ();


	private const string REMOTE_SERVER =
		"https://api.github.com/search/code?q=extension:json+repo:doki-theme/doki-master-theme&page=1&per_page=100";
	
	private const string REMOTE_BRANCH_SERVER = "https://api.github.com/repos/doki-theme/doki-master-theme/branches?per_page=100";

	private const string WALLPAPER_SERVER = "https://github.com/doki-theme/doki-theme-assets/raw/master/backgrounds/wallpapers/";
	private const string STICKER_SERVER   = "https://github.com/doki-theme/doki-theme-assets/raw/master/stickers/jetbrains/v2/";

	private const string BRANCH_COMMIT_SERVER = "https://api.github.com/repos/doki-theme/doki-master-theme/branches/";

	private const string GROUP_FILE = "https://github.com/doki-theme/doki-theme-jetbrains/raw/master/buildSrc/src/main/kotlin/Tools.kt";

	public ThemeDownloaderForm ()
	{
		InitializeComponent ();
		browser.ScrollBarsEnabled = true;
		browser.ObjectForScripting = this;
		browser.ScriptErrorsSuppressed = false;
#if PRIMARLY
		Helper.bgColor = Color.FromArgb (32, 32, 32);
		Helper.fgColor = Color.FromArgb (240, 240, 240);
		Helper.fgKeyword = Color.Green;
		Helper.bgBorder = Color.FromArgb (252, 152, 252);
		Settings.saveAsOld = true;
#endif
		ShowLoading ();

		LoadGroups ();

		ParseBranches ();
		
		// LoadThemePage ();
	}

	private void ShowLoading ()
	{
		string html = Helper.ReadHTML ("loading.html", THEME_NAMESPACE);
		html = Helper.ReplaceHTMLColors (html);
		browser.DocumentText = html;
	}

	#region Parser

	private async void ParseBranches ()
	{
		Console.WriteLine ("Requesting branch info...");
		string search_api = REMOTE_BRANCH_SERVER;
		branches.Clear ();
		using (HttpClient client = new HttpClient ())
		{
			client.DefaultRequestHeaders.Add ("User-Agent",
			                                  DownloadForm.user_agent);
			var response = await client.GetAsync (search_api);
			if (response != null)
			{
				Console.WriteLine ("Branch info loaded.");
				string json = await response.Content.ReadAsStringAsync ();
				// Damn! API rate limit exceeded for <my IP address>
				// Console.WriteLine (json); 
				JArray jresponse;
				try
				{
					jresponse = JArray.Parse (json);
				} catch (JsonReaderException e)
				{
					Console.WriteLine (json);
					MessageBox.Show ("Github API Limit exceeded for your address. Try later!", "API Limit", MessageBoxButtons.OK,
					                 MessageBoxIcon.Error);
					return;
				}


				Console.WriteLine ("Parsing branch info...");
				foreach (JObject s in jresponse)
				{
					Console.WriteLine ("Branch: {0}", s ["name"]);
					branches.Add (s ["name"].ToString ());
				}
			}
		}

		ParseBranchLastCommits ();
	}

	private async void ParseBranchLastCommits ()
	{
		Console.WriteLine ("Requesting branch commit...");
		foreach (string branchName in branches)
		{
			string search_api = BRANCH_COMMIT_SERVER + branchName;

			using (HttpClient client = new HttpClient ())
			{
				client.DefaultRequestHeaders.Add ("User-Agent",
				                                  DownloadForm.user_agent);

				var response = await client.GetAsync (search_api);
				if (response != null)
				{
					Console.WriteLine ("Branch commit loaded.");
					string json = await response.Content.ReadAsStringAsync ();
					JObject commit = JObject.Parse (json);

					Console.WriteLine ("Parsing branch commit...");
					string date = ((JToken)commit ["commit"]? ["commit"]? ["author"]? ["date"])?.Value <DateTime> ()
						.ToString ("MM.dd.yyyy");

					Console.WriteLine ("Date: {0} => {1}", branchName, date);
					lock (branchCommitDates)
					{
						branchCommitDates.Add (branchName, date);
					}
				}
			}
		}

		ParseAllThemes ();
	}

	private async void LoadGroups ()
	{
		Console.WriteLine ("Requesting groups info...");
		string search_api = GROUP_FILE;
		Dictionary <string, string> groups = new ();
		using (HttpClient client = new HttpClient ())
		{
			client.DefaultRequestHeaders.Add ("User-Agent",
			                                  DownloadForm.user_agent);
			try
			{
				var response = await client.GetAsync (search_api);
				if (response != null)
				{
					Console.WriteLine ("Groups info loaded.");
					string file = await response.Content.ReadAsStringAsync ();

					string [] lines = file.Split ('\n');

					Console.WriteLine ("Parsing groups info...");
					Dictionary <string, string> namespaces = new ();

					foreach (string line in lines)
					{
						if (line.Contains (" val "))
						{
							Tuple <string, string> res = ParseNamespace (line);
							if (res != null)
								namespaces.Add (res.Item1, res.Item2);
						} else if (line.Contains (" to "))
						{
							string cline = line.TrimStart ();
							cline = cline.Replace (",", "");
							string [] splitted = cline.Split (new [] { " to " }, StringSplitOptions.None);
							string name = splitted [0].Replace ("\"", "");
							string converted = "";
							if (!splitted [1].Contains ("\""))
							{
								if (namespaces.ContainsKey (splitted [1]))
									converted = namespaces [splitted [1]];
								else
									converted = splitted [1];
							} else
							{
								converted = splitted [1].Replace ("\"", "");
							}

							groups.Add (name, converted);
						}
					}
				}
			} catch (Exception exception)
			{
				MessageBox.Show ($"{exception.Message}\n{exception.StackTrace}");
			}
		}

		if (groups.Count > 0)
			DokiThemeParser.groups = groups;
	}

	private Tuple <string, string> ParseNamespace (string target)
	{
		string starting = "const val ";
		if (target.StartsWith (starting))
		{
			string ctarget = target.Substring (starting.Length);
			string [] splitted = ctarget.Split (new [] { " = " }, StringSplitOptions.None);
			splitted [1] = splitted [1].Replace ("\"", "");
			return new Tuple <string, string> (splitted [0], splitted [1]);
		} else
		{
			return null;
		}
	}

	private async void ParseAllThemes ()
	{
		Console.WriteLine ("Requesting themes info...");
		string search_api = GetSearchAPI ();
		themesWithUrls.Clear ();
		using (HttpClient client = new HttpClient ())
		{
			client.DefaultRequestHeaders.Add ("User-Agent",
			                                  DownloadForm.user_agent);
			var response = await client.GetAsync (search_api);
			if (response != null)
			{
				Console.WriteLine ("Themes info laoded.");
				string json = await response.Content.ReadAsStringAsync ();
				// Console.WriteLine (json);
				Console.WriteLine ("Parsing themes info...");
				ParseThemes (json);

				Console.WriteLine ("Parsing end.");
			}
		}

		LoadThemePage ();
		Console.WriteLine ("Themes parse end.");

		Thread thread = new Thread (LoadJSONS);
		thread.Start ();
		// LoadJSONS ();
	}

	private void ParseThemes (string json)
	{
		if (searchMode == 0)
			ParseMode0 (json);
		else if (searchMode == 1)
			ParseMode1 (json);
		else
			MessageBox.Show ("Error while parsing themes! Search Mode is not valid", "Parse Error", MessageBoxButtons.OK,
			                 MessageBoxIcon.Error);
	}

	private void ParseTheme (string jsonMulti)
	{
		if (!this.IsDisposed)
		{
			DokiThemeParser doki = new DokiThemeParser ();
			doki.needToWrite = false;
			doki.groupName = "Doki Theme";
			string [] multi = jsonMulti.Split (new string [] { "-|-" }, StringSplitOptions.None);
			string json = multi [1];
			doki.Parse (json, "none", "none", false, false, false);
			doki.theme.link = multi [0];
			bool isEqual = !IsEqual (doki.theme, CLI.GetTheme (doki.theme.Name));
			themes.Add (doki.theme.Name, doki.theme);
			newThemes.Add (doki.theme.Name, isEqual);
			LoadThemeIntoPage (doki.theme, isEqual);
		}
	}

	private void ParseMode0 (string json)
	{
		JObject jresponse;
		try
		{
			jresponse = JObject.Parse (json);
		} catch (JsonReaderException e)
		{
			MessageBox.Show ("Github API Limit exceeded for your address. Try later!", "API Limit", MessageBoxButtons.OK,
			                 MessageBoxIcon.Error);
			return;
		}

		JArray jarray = JArray.FromObject (jresponse.GetValue ("items"));

		foreach (JObject s in jarray)
		{
			if (s ["name"].ToString ().EndsWith (".master.definition.json"))
			{
				string url = s ["html_url"].ToString ().Replace ("/blob", "/raw");
				Console.WriteLine ("Theme 0: {0} = {1}\n", s ["name"], url);
				themesWithUrls.Add (s ["name"].ToString (), url);
			}
		}
	}

	private void ParseMode1 (string json)
	{
		JObject jresponse;
		try
		{
			jresponse = JObject.Parse (json);
		} catch (JsonReaderException e)
		{
			MessageBox.Show ("Github API Limit exceeded for your address. Try later!", "API Limit", MessageBoxButtons.OK,
			                 MessageBoxIcon.Error);
			return;
		}

		JArray jarray = JArray.FromObject (jresponse.GetValue ("tree"));

		foreach (JObject s in jarray)
		{
			if (s ["mode"].ToString () == "100644") // If object is file
			{
				if (s ["path"].ToString ().EndsWith (".master.definition.json"))
				{
					string name = GetName (s ["path"].ToString ());
					string url = CollectURL (s ["path"].ToString ());
					Console.WriteLine ("Theme 1: {0} = {1}\n", name, url);
					themesWithUrls.Add (name, url);
				}
			}
		}
	}

	#endregion

	private string GetName (string path)
	{
		string result = path;

		if (result.Contains ("/"))
		{
			result = result.Substring (result.LastIndexOf ("/") + 1);
		}

		return result;
	}

	private string GetSearchAPI ()
	{
		if (branch == "master")
		{
			searchMode = 0;
			return REMOTE_SERVER;
		} else
		{
			searchMode = 1;
			return $"https://api.github.com/repos/doki-theme/doki-master-theme/git/trees/{branch}?recursive=1";
		}
	}

	private string CollectURL (string path)
	{
		return $"https://github.com/doki-theme/doki-master-theme/raw/{branch}/{path}";
	}

	private string CollectBranchesHTML ()
	{
		string result = "";
		Console.WriteLine ("Loaded Branch HTML");
		lock (branchCommitDates)
		{
			foreach (string branchName in branches)
			{
				result +=
					$"<div class='d-flex branch_tab'><a href='javascript:void(0);' onclick='LoadBranch(\"{branchName}\")' class='px-3'>{branchName}</a> <span class='ml-auto mr-3'>{branchCommitDates[branchName]}</span></div>";
			}	
		}
		

		return result;
	}

	private string CollectThemesHTML ()
	{
		string result = "";

		foreach (KeyValuePair <string, string> theme in themesWithUrls)
		{
			result += $"<div>{theme.Key} + {theme.Value}</div>";
		}

		return result;
	}

	private string downloadFile (string url)
	{
		if (!this.IsDisposed)
		{
			try
			{
				using (var wc = new WebClient ())
				{
					return url + "-|-" + wc.DownloadString (url);
				}
			} catch (Exception e)
			{
				Console.WriteLine (e);
				return null;
			}
		}

		return null;
	}

	private Image DownloadImage (string url)
	{
		Console.WriteLine (url);
		WebClient wc = new WebClient ();
		byte [] bytes = wc.DownloadData (url);
		MemoryStream ms = new MemoryStream (bytes);
		Image img = Image.FromStream (ms);
		return img;
	}

	private void LoadJSONS ()
	{
		themesWithUrls = themesWithUrls.OrderBy (obj => obj.Key).ToDictionary ((obj) => obj.Key, (obj) => obj.Value);
		int max = themesWithUrls.Count;
		stepProgress = 100.0 / max;
		currentProgress = 0;
		needToReset = true;
		int pars = 0;
		ParallelQuery <string> downloads = themesWithUrls.AsParallel ()
		                                                 .WithDegreeOfParallelism (20)
		                                                 .Select (url => downloadFile (url.Value));

		Console.WriteLine ("Start loading jsons...");
		for (int i = 0; i < max; i++)
		{
			ParseTheme (downloads.ElementAt (i));
		}
		/*foreach (string download in downloads)
		{
			ParseTheme (download);
		}*/
	}

	private void LoadThemeIntoPage (Theme theme, bool isEqual)
	{
		if (!this.IsDisposed)
		{
			currentProgress += stepProgress;
			lock (browser)
			{
				browser.Invoke ((MethodInvoker)delegate
				{
					browser.Document.InvokeScript (
						"AddTheme",
						new object []
						{
							theme.Name, currentProgress.ToString ("0.0").Replace (',', '.'), needToReset.ToStringLower (),
							isEqual.ToStringLower (),
							theme.Fields ["Default"].Background, theme.Fields ["Default"].Foreground,
							theme.Fields ["KeyWords"].Foreground, theme.Fields ["CaretMarker"].Foreground,
							theme.Fields ["String"].Foreground,
							theme.Fields ["BeginEnd"].Foreground, theme.Fields ["Selection"].Background,
							theme.Fields ["MarkPrevious"].Foreground
						});
				});
				needToReset = false;
			}
		}
	}

	private void LoadThemePage ()
	{
		if (!this.IsDisposed)
		{
			if (isFromBranch)
			{
				themesElement.InnerHtml = CollectThemesHTML ();
				isFromBranch = false;
			} else
			{
				string html = Helper.ReadHTML ("theme_downloader.html", THEME_NAMESPACE);
				html = Helper.ReplaceHTMLColors (html);

				html = html.Replace ("__branches__", CollectBranchesHTML ());

				html = html.Replace ("__content__", "");
				html = html.Replace ("__bootstrap_css__", LoadCSS ("bootstrap.min.css"));

				browser.Navigate ("about:blank");
				browser.Document.OpenNew (false);
				browser.Document.Write (html);
				htmlLoaded = true;
				browser.Refresh ();
			}
		}
	}

	public void LoadBranch (string name)
	{
		Console.WriteLine ("Loading branch " + name + "...\n");
		branch = name;
		themesElement.InnerHtml = "";
		ParseAllThemes ();
	}

	private string GenerateStickerUrl (Theme theme)
	{
		string url = "";

		string split = theme.link.Split (new string [] { "definitions/" }, StringSplitOptions.None) [1];
		split = split.Substring (0, split.LastIndexOf ("/", StringComparison.Ordinal));
		url = STICKER_SERVER + split + "/" + theme.imagePath;
		return url;
	}

	public void DownloadTheme (string name)
	{
		currentProgress = 0;
		stepProgress = 100.0 / 3;
		Thread thread = new Thread (DownloadThemeThread);
		thread.Start (new Tuple <string, bool> (name, true));
	}

	public void DownloadAll ()
	{
		Thread thread = new Thread (DownloadAllThread);
		thread.Start ();
	}

	private void DownloadAllThread ()
	{
		lock (themes)
		{
			Console.WriteLine ("Downloading all themes.");
			stepProgress = 100.0 / themes.Count;
			currentProgress = 0;
			// Doesn't work well
			lock (downloadingErrors)
			{
				downloadingErrors.Clear ();
			}
			Parallel.ForEach (themes, keyValuePair => DownloadThemeThread (new Tuple <string, bool> (keyValuePair.Key, false)));
			MessageBox.Show ("Downloading end!");
			lock (downloadingErrors)
			{
				if (downloadingErrors.Count > 0)
				{
					string mx = "Errors:\n";
					foreach (string error in downloadingErrors)
					{
						mx += error+"\n";
						
					}

					MessageBox.Show (mx);
				}
				downloadingErrors.Clear ();
			}
			
		}
	}

	private void DownloadThemeThread (object param)
	{
		Tuple <string, bool> typle = (Tuple <string, bool>)param;
		string name = typle.Item1;
		bool eachTime = typle.Item2;

		bool notEqual = false;
		lock (newThemes)
		{
			notEqual = newThemes.ContainsKey (name) && newThemes [name];
		}

		if (notEqual)
		{
			Console.WriteLine ("Downloading theme {0}", name);
			try
			{
				if (CLI.ThemeInfos.ContainsKey (name))
				{
					if (CLI.ThemeInfos [name].location == ThemeLocation.File &&
					    File.Exists (CLI.pathToFile (Helper.ConvertNameToPath (name), CLI.ThemeInfos [name].isOld)))
					{
						File.Delete (CLI.pathToFile (Helper.ConvertNameToPath (name), CLI.ThemeInfos [name].isOld));
					}
				}

				Theme theme = themes [name];

				Image wallpaper = DownloadImage (WALLPAPER_SERVER + theme.imagePath);
				if (eachTime)
					AddNSetProgress ();
				Image sticker = DownloadImage (GenerateStickerUrl (theme));
				if (eachTime)
					AddNSetProgress ();

				theme.fullPath = CLI.pathToFile (Helper.ConvertNameToPath (name), true);
				theme.Token = Helper.EncryptString (theme.Name, DateTime.Now.ToString ("ddMMyyyy"));
				Console.WriteLine ("Token: {0}", theme.Token);
				CLI.ExtractSyntaxTemplate (SyntaxType.Pascal, theme.fullPath); // Create theme file
				CLI.SaveTheme (theme, wallpaper, sticker);
				wallpaper.Dispose ();
				sticker.Dispose ();
				AddNSetProgress ();
				Console.WriteLine ("{0} has been saved", name);
			} catch (Exception e)
			{
				Console.WriteLine ("{0} -> {1}", e.Message, e.StackTrace);
				Console.WriteLine ("{0} hasn't been saved", name);
				lock (downloadingErrors)
				{
					downloadingErrors.Add ($"{name} couldn't be saved.");
				}
			}
		} else
		{
			Console.WriteLine ("Theme: {0} is up to date ", name);
		}
	}

	private void AddNSetProgress ()
	{
		currentProgress += stepProgress;
		browser.Invoke ((MethodInvoker)delegate
		{
			browser.Document.InvokeScript ("SetProgress", new object [] { currentProgress.ToString ("0.0").Replace (',', '.') });
		});
	}

	private string Base64Encode (string plainText)
	{
		var plainTextBytes = System.Text.Encoding.UTF8.GetBytes (plainText);
		return System.Convert.ToBase64String (plainTextBytes);
	}

	private string LoadCSS (string css)
	{
		string cssf = Helper.ReadHTML ("css." + css, THEME_NAMESPACE);
		return "data:text/css;charset=utf-8;base64," + Base64Encode (cssf);
	}

	private bool IsEqual (Theme first, Theme second)
	{
		Console.WriteLine ("IsNULL: {0} = {1}", first is null ? "null" : first.Name, second is null ? "null" : second.Name);
		if (second == null)
		{
			Console.WriteLine (first.Name + " - 2nd is null");
			return false;
		}

		return first == second;
	}
}