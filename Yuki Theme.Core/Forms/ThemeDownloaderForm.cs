// #define PRIMARLY

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Yuki_Theme.Core.Parsers;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.Forms;

[ComVisible (true)]
public partial class ThemeDownloaderForm : Form
{
	private const string THEME_NAMESPACE = "Yuki_Theme.Core.Resources.HTML.";

	private string        branch       = "master";
	private int           searchMode   = 0;
	private List <string> branches     = new List <string> ();
	private bool          isFromBranch = false;
	private Task          loadingJSONSTask;
	private HtmlElement   themesElement;

	private double stepProgress;
	private double currentProgress;
	private bool   needToReset = false;

	private Dictionary <string, string> themesWithUrls = new ();
	private Dictionary <string, Theme>  themes         = new ();


	private const string LOCAL_SERVER = "http://localhost:8000/find.json";

	private const string REMOTE_SERVER =
		"https://api.github.com/search/code?q=extension:json+repo:doki-theme/doki-master-theme&page=1&per_page=100";

	private const string LOCAL_API_SERVER  = "http://localhost:8000/branches.json";
	private const string REMOTE_API_SERVER = "https://api.github.com/repos/doki-theme/doki-master-theme/branches?per_page=100";

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
#endif
		ShowLoading ();

		ParseBranches ();

		ParseAllThemes ();
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
		string search_api = LOCAL_API_SERVER; // to load locally
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
				JArray jresponse;
				jresponse = JArray.Parse (json);

				Console.WriteLine ("Parsing branch info...");
				foreach (JObject s in jresponse)
				{
					Console.WriteLine ("Branch: {0}", s ["name"]);
					branches.Add (s ["name"].ToString ());
				}
			}
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

	private void ParseTheme (string json)
	{
		DokiThemeParser doki = new DokiThemeParser ();
		doki.needToWrite = false;
		doki.Parse (json, "none", "none", null, false, false, false);
		themes.Add (doki.theme.Name, doki.theme);
		LoadThemeIntoPage (doki.theme);
	}

	private void ParseMode0 (string json)
	{
		JObject jresponse = JObject.Parse (json);


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
		JObject jresponse = JObject.Parse (json);


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
			return LOCAL_SERVER;
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

		foreach (string branch in branches)
		{
			result += $"<div><a href='javascript:void(0);' onclick='LoadBranch(\"{branch}\")' class='px-3'>{branch}</a></div>";
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
		try
		{
			using (var wc = new WebClient ())
			{
				return wc.DownloadString (url);
			}
		} catch (Exception e)
		{
			Console.WriteLine (e);
			return null;
		}
	}


	private void LoadJSONS ()
	{
		themesWithUrls = themesWithUrls.OrderBy (obj => obj.Key).ToDictionary ((obj) => obj.Key, (obj) => obj.Value);
		int max = 10; // themesWithUrls.Count
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

	private void LoadThemeIntoPage (Theme theme)
	{
		currentProgress += stepProgress;
		browser.Invoke ((MethodInvoker)delegate
		{
			browser.Document.InvokeScript (
				"AddTheme",
				new object []
				{
					theme.Name, currentProgress.ToString ("0.0").Replace (',', '.'), needToReset.ToStringLower (),
					(!IsEqual (theme, CLI.GetTheme (theme.Name))).ToStringLower (),
					theme.Fields ["Default"].Background, theme.Fields ["Default"].Foreground,
					theme.Fields ["KeyWords"].Foreground, theme.Fields ["CaretMarker"].Foreground, theme.Fields ["String"].Foreground,
					theme.Fields ["BeginEnd"].Foreground, theme.Fields ["Selection"].Background, theme.Fields ["MarkPrevious"].Foreground
				});
		});
		needToReset = false;
	}

	private void LoadThemePage ()
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
			browser.Refresh ();
		}
	}

	public void LoadBranch (string name)
	{
		Console.WriteLine ("Loading branch " + name + "...\n");
		branch = name;
		themesElement.InnerHtml = "";
		ParseAllThemes ();
	}

	public void DownloadTheme (string name)
	{
		Console.WriteLine ("Downloading theme " + name);
	}

	public void DownloadAll ()
	{
		Console.WriteLine ("Downloading all themes.");
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
		if (second == null)
		{
			Console.WriteLine (first.Name + " - 2nd is null");
			return false;
		}
		return first == second;
	}
}