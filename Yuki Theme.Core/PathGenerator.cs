using System.IO;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core;

public class PathGenerator
{
	public static string PathToFile (string pathLoad, bool old)
	{
		return Path.Combine (API.currentPath, "Themes", $"{pathLoad}{Helper.GetExtension (old)}");
	}

	public static string PathToMemory (string name)
	{
		IThemeHeader header = DefaultThemes.headers [name];
		string file = name;
		if (file.Contains (":"))
		{
			file = Helper.ConvertNameToPath (file);
		}

		return $"{header.ResourceHeader}.{file}{Helper.GetExtension (API.themeInfos[name].isOld)}";
	}
}