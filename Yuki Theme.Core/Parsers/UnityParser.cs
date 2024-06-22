using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yuki_Theme.Core.Themes;
namespace Yuki_Theme.Core.Parsers;

public class UnityParser : AbstractParser
{
	public Theme ThemeToConvert;

	private Dictionary<string, (bool isBackground, string[])> _associations = new()
	{
		{
			"Default Text_darker", (true, new[]
			{
				"TabWindowBackground", "ScrollViewAlt",
				"label", "ProjectBrowserTopBarBg",
				"ProjectBrowserBottomBarBg"
			})
		},
		{
			"Default Text", (true, new[]
			{
				"ToolbarDropDownToogleRight", "ToolbarPopupLeft",
				"ToolbarPopup", "toolbarbutton",
				"PreToolbar", "AppToolbar",
				"GameViewBackground", "CN EntryInfoSmall",
				"Toolbar", "toolbarbutton",
				"toolbarbuttonRight", "ProjectBrowserIconAreaBg",
				"SceneTopBarBg", "MiniPopup",
				"TV Selection", "ExposablePopupMenu",
				"minibutton", " ToolbarSearchTextField",
				"WhiteBackground", "dockHeader", "TV LineBold"
			})
		},
		{
			"Selection_dark2", (true, new[]
			{
				"AppCommandLeft", "AppCommandMid",
				"AppCommand", "AppToolbarButtonLeft",
				"AppToolbarButtonRight", "DropDown",
				"dragtab-label"
			})
		},
	};

	public override void populateList(string path)
	{

	}
	public override void PopulateByXMLNodeTreeType(XmlNode node)
	{

	}
	public override string GetValue(XmlNode child)
	{
		return "";
	}
	public override ThemeField populateDefaultAttributes(string name)
	{
		return null;
	}
	public override bool isNecessaryAttribute(string name)
	{
		return false;
	}
	public override string[] getName(string st)
	{
		return Array.Empty<string>();
	}
	public override void finishParsing(string path)
	{
		var templateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Yuki_Theme.Core.Resources.UnityTemplate.json");
		TextReader reader = new StreamReader(templateStream);
		string template = reader.ReadToEnd();
		templateStream.Dispose();

		Console.WriteLine($"Theme {ThemeToConvert.Name}, path {path}");

		JObject json = (JObject)JsonConvert.DeserializeObject(template);
		json["Name"] = Helper.ConvertNameToPathUnity(ThemeToConvert.Name);

		// Console.WriteLine($"All keys: {string.Join(", ", ThemeToConvert.Fields.Select(s => s.Key))}");

		json["unityTheme"] = Helper.IsDark(ColorTranslator.FromHtml(ThemeToConvert.Fields["Default Text"].Background)) ? 0 : 1;

		foreach (KeyValuePair<string, (bool isBackground, string[] names)> association in _associations)
		{
			string key = association.Key;

			Console.WriteLine($"Getting {key}");
			bool darker = false;
			bool darker2 = false;
			if (key.Contains("_darker"))
			{
				darker = true;
				key = key.Replace("_darker", "");
			}
			if (key.Contains("_dark2"))
			{
				darker2 = true;
				key = key.Replace("_dark2", "");
			}

			ThemeField field = ThemeToConvert.Fields[key];
			Color color;

			color = ColorTranslator.FromHtml(association.Value.isBackground ? field.Background : field.Foreground);
			if (darker)
			{
				color = Helper.ChangeColorBrightness(color, 0.1f, 255);
			}
			if (darker2)
			{
				color = Helper.ChangeColorBrightness(color, -0.1f, 255);
			}

			foreach (string valueName in association.Value.names)
			{
				Console.WriteLine($"Finding {valueName}");
				JToken found = json["Items"].First(j => j["Name"].ToString() == valueName);
				found["Color"]["r"] = (double)color.R / 255;
				found["Color"]["g"] = (double)color.G / 255;
				found["Color"]["b"] = (double)color.B / 255;

				byte colorA = color.A;
				if (valueName == "dragtab-label")
				{
					colorA = 145;
				}
				found["Color"]["a"] = (double)colorA / 255;
			}

		}

		File.WriteAllText(path, JsonConvert.SerializeObject(json));
	}
}