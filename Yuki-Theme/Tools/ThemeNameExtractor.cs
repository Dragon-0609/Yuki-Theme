using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using YukiTheme.Engine;

namespace YukiTheme.Tools;

public static class ThemeNameExtractor
{
	public static List<ThemeLoadInfo> Infos = new();

	public static void ListenToReload()
	{
		PluginEvents.Instance.Reload += LoadThemeInfo;
	}

	public static void LoadThemeInfo()
	{
		var path = GetThemeName();

		if (!File.Exists(path)) return;

		var docu = new XmlDocument();
		docu.Load(path);

		var nod = docu.SelectSingleNode("/SyntaxDefinition");
		var comms = nod.SelectNodes("//comment()");

		if (comms != null)
			foreach (XmlComment comm in comms)
			{
				var filteredInfos = Infos.Where(i => comm.Value.StartsWith(i.Key));
				if (filteredInfos.Any())
					foreach (var info in filteredInfos)
						info.Value(comm.Value.Substring(info.CutNumber));
			}
	}

	private static string GetThemeName()
	{
		return Path.Combine(YukiTheme_VisualPascalABCPlugin.GetCurrentFolder, "Highlighting", "theme.xshd");
	}
}