using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Yuki_Theme.Core.Localization;

public class Localization
{
	private const string MemoryNamespace = "Yuki_Theme.Core.Localization.";

	private Dictionary <string, string> translation;
	private Dictionary <string, bool>   externalLang;

	private string [] languages;
	private string [] languagesdisplay;
	private int       recursionCalls = 0;

	public void SearchLocals ()
	{
		List <string> locales = new List <string> () { "en", "ru" };
		List <string> languagesdisplay = new List <string> () { "English", "Русский" };
		Dictionary <string, bool> external = new Dictionary <string, bool> () { { "en", false }, { "ru", false } };
		string [] files = Directory.GetFiles (Path.Combine (CLI.currentPath, "Langs"), "*.lang");
		foreach (string file in files)
		{
			try
			{
				JObject jObject = JObject.Parse (File.ReadAllText (file));
				string lang = jObject ["language"].ToString ().ToLower();
				if (!locales.Contains (lang))
				{
					locales.Add (lang);
					external.Add (lang, true);
					languagesdisplay.Add (jObject ["display"].ToString ());
				}
			} catch (Exception e)
			{
				CLI_Actions.showError (e.Message, "Something went wrong");
			}
		}
		
		languages = locales.ToArray ();
		externalLang = external;
	}
	
	public void LoadLocalization ()
	{
		SearchLocals ();
		string locale = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName.ToLower();
		if (!externalLang.ContainsKey (locale))
		{
			locale = languages [0];
		}

		LoadLocale (locale);
	}

	public void LoadLocale (string lang)
	{
		recursionCalls = 0;
		if (externalLang [lang])
			LoadLocaleFromFile (lang);
		else
			LoadLocaleFromMemory (lang);
	}

	public void LoadLocaleFromFile (string lang)
	{
		string file = Path.Combine (CLI.currentPath, "Langs", lang + ".lang");
		if (File.Exists (file))
			LoadTranslation (File.ReadAllText (file));
		else
		{
			CLI_Actions.showError ("Couldn't read language file! Retrieving to the default language...", "Couldn't read language file");
			LoadLocaleFromMemory (languages [0]);
		}
	}

	private void LoadTranslation (string json)
	{
		JObject jObject = JObject.Parse (json);
		translation = jObject ["translations"].ToObject <Dictionary <string, string>> ();
	}

	public void LoadLocaleFromMemory (string lang)
	{
		string path = Path.Combine (MemoryNamespace, lang + ".lang");
		Stream str = Assembly.GetExecutingAssembly ().GetManifestResourceStream (path);
		if (str != null)
		{
			string result = "";
			using (StreamReader streamReader = new StreamReader (str))
			{
				result = streamReader.ReadToEnd ();
			}

			str.Dispose ();
			LoadTranslation (result);
		} else if (recursionCalls < 5)
		{
			recursionCalls++;
			CLI_Actions.showError ("Couldn't read language file! Retrieving to the default language...", "Couldn't read language file");
			string fallback = languages [0];
			if (lang == fallback) fallback = languages [1];
			LoadLocaleFromMemory (fallback);
		} else
		{
			CLI_Actions.showError ("Infinite Recursion happened... Damn! I won't be able to translate.", "Infinite Recursion");
			translation.Clear ();
		}
	}

	public string GetText (string definition)
	{
		if (translation.ContainsKey (definition))
			return translation [definition];
		else
			return definition;
	}
}