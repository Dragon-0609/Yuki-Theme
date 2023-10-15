using System;
using System.IO;
using System.Xml;
using YukiTheme.Engine;

namespace YukiTheme.Tools;

public static class ThemeNameExtractor
{
	public static string Extract()
	{
		string name = "";
		
		string path = GetThemeName();

		if (!File.Exists(path))
		{
			return "Unknown";
		}
		
		XmlDocument docu = new XmlDocument();
		docu.Load(path);

		XmlNode nod = docu.SelectSingleNode("/SyntaxDefinition");
		XmlNodeList comms = nod.SelectNodes("//comment()");

		if (comms != null)
			foreach (XmlComment comm in comms)
			{
				if (comm.Value.StartsWith("name"))
				{
					name = comm.Value.Substring(5);
					break;
				}
			}

		if (name == "")
		{
			if (nod.Attributes != null) name = nod.Attributes["name"].Value;
		}

		return name;
	}

	private static string GetThemeName()
	{
		return Path.Combine(YukiTheme_VisualPascalABCPlugin.GetCurrentFolder, "Highlighting", "theme.xshd");
	}
}