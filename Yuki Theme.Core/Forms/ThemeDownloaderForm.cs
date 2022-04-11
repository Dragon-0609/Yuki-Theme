using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Yuki_Theme.Core.Forms;

[ComVisible (true)]
public partial class ThemeDownloaderForm : Form
{
	public ThemeDownloaderForm ()
	{
		InitializeComponent ();
		browser.ScrollBarsEnabled = true;
		browser.ObjectForScripting = this;
		ShowLoading ();
		ParseAllThemes ();
	}

	private void ShowLoading ()
	{
		string html = Helper.ReadHTML ("loading.html", "Yuki_Theme.Core.Resources.HTML.");
		html = Helper.ReplaceHTMLColors (html);
		browser.DocumentText = html;
	}

	private async void ParseAllThemes ()
	{
		string search_api = "https://api.github.com/search/code?q=extension:json+repo:doki-theme/doki-master-theme&page=1&per_page=100";
		Dictionary <string, string> themes = new Dictionary <string, string> ();
		using (var client = new HttpClient ())
		{
			client.DefaultRequestHeaders.Add ("User-Agent",
			                                  DownloadForm.user_agent);
			var response = await client.GetAsync (search_api);
			if (response != null)
			{
				string json = await response.Content.ReadAsStringAsync ();
				// Console.WriteLine (json);
				JObject jresponse;
				jresponse = JObject.Parse (json);


				JArray jarray = JArray.FromObject (jresponse.GetValue ("items"));
				
				foreach (JObject s in jarray)
				{
					if (s["name"].ToString().EndsWith (".master.definition.json"))
					{
						Console.WriteLine ("{0} = {1}\n", s ["name"], s ["html_url"]);
						themes.Add (s ["name"].ToString (), s ["html_url"].ToString ());
					}
				}
			}
		}
		LoadThemePage ();
	}

	private void LoadThemePage ()
	{
		
	}
}