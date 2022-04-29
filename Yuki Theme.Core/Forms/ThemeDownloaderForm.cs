using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Yuki_Theme.Core.Forms;

[PermissionSet(SecurityAction.Demand, Name="FullTrust")]
[System.Runtime.InteropServices.ComVisibleAttribute(true)]
public partial class ThemeDownloaderForm : Form
{
	private const string THEME_NAMESPACE = "Yuki_Theme.Core.Resources.HTML.";
	
	private string        branch       = "master";
	private int           searchMode   = 0;
	private List <string> branches     = new List <string> ();
	private bool          isFromBranch = false;

	private Dictionary <string, string> themes = new ();

	public ThemeDownloaderForm ()
	{
		InitializeComponent ();
		browser.ScrollBarsEnabled = true;
		browser.ObjectForScripting = this;
		browser.ScriptErrorsSuppressed = false;
		
		Helper.bgColor = Color.FromArgb (32, 32, 32);
		Helper.fgColor = Color.FromArgb (240, 240, 240);
		Helper.fgKeyword = Color.Green;
		
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

	private async void ParseBranches ()
	{
		Console.WriteLine("Requesting branch info...");
		string search_api = "https://api.github.com/repos/doki-theme/doki-master-theme/branches?per_page=100";
		branches.Clear ();
		using (HttpClient client = new HttpClient ())
		{
			client.DefaultRequestHeaders.Add ("User-Agent",
			                                  DownloadForm.user_agent);
			var response = await client.GetAsync (search_api);
			if (response != null)
			{
				Console.WriteLine("Branch info loaded.");
				string json = await response.Content.ReadAsStringAsync ();
				JArray jresponse;
				jresponse = JArray.Parse (json);

				Console.WriteLine("Parsing branch info...");
				foreach (JObject s in jresponse)
				{
					Console.WriteLine ("Branch: {0}\n", s ["name"]);
					branches.Add (s ["name"].ToString ());
				}
			}
		}
	}

	private async void ParseAllThemes ()
	{
		Console.WriteLine("Requesting themes info...");
		string search_api = GetSearchAPI ();
		themes.Clear ();
		using (HttpClient client = new HttpClient ())
		{
			client.DefaultRequestHeaders.Add ("User-Agent",
			                                  DownloadForm.user_agent);
			var response = await client.GetAsync (search_api);
			if (response != null)
			{
				Console.WriteLine("Themes info laoded.");
				string json = await response.Content.ReadAsStringAsync ();
				// Console.WriteLine (json);
				Console.WriteLine("Parsing themes info...");
				ParseThemes (json);
				
				Console.WriteLine("Parsing end.");
				
			}
		}
		LoadThemePage ();
		Console.WriteLine("Themes parse end.");
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
				themes.Add (s ["name"].ToString (), url);
			}
		}
	}

	private void ParseMode1 (string json)
	{
		JObject jresponse = JObject.Parse (json);


		JArray jarray = JArray.FromObject (jresponse.GetValue ("tree"));
		
		foreach (JObject s in jarray)
		{
			if (s["mode"].ToString() == "100644") // If object is file
			{
				if (s ["path"].ToString ().EndsWith (".master.definition.json"))
				{
					string name = GetName (s ["path"].ToString ());
					string url = CollectURL (s ["path"].ToString ());
					Console.WriteLine ("Theme 1: {0} = {1}\n", name, url);
					themes.Add (name, url);
				}
			}
		}
	}

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
			return "https://api.github.com/search/code?q=extension:json+repo:doki-theme/doki-master-theme&page=1&per_page=100";
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

		foreach (KeyValuePair <string, string> theme in themes)
		{
			result += $"<div>{theme.Key} + {theme.Value}</div>";
		}
		
		return result;
	}
	
	private void LoadThemePage ()
	{
		if (isFromBranch)
		{
			browser.Document.GetElementById ("themes").InnerHtml = CollectThemesHTML ();
			isFromBranch = false;
		}else
		{
			string html = Helper.ReadHTML ("theme_downloader.html", THEME_NAMESPACE);
			html = Helper.ReplaceHTMLColors (html);

			html = html.Replace ("__branches__", CollectBranchesHTML ());

			html = html.Replace ("__content__", CollectThemesHTML ());

			browser.Navigate ("about:blank");
			browser.Document.OpenNew (false);
			browser.Document.Write (html);
			browser.Refresh ();
		}
	}

	public void LoadBranch (string name)
	{
		Console.WriteLine("Loading branch...\n");
		branch = name;
		isFromBranch = true;
		ParseAllThemes ();
	}
}