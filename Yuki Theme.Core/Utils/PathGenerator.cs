using System.IO;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.Utils
{
	public static class PathGenerator
	{
		public static string PathToFile (string pathLoad, bool old)
		{
			return Path.Combine (SettingsConst.CurrentPath, "Themes", $"{pathLoad}{Helper.GetExtension (old)}");
		}

		public static string PathToMemory (string name)
		{
			IThemeHeader header = DefaultThemes.headers [name];
			string file = name;
			if (file.Contains (":"))
			{
				file = Helper.ConvertNameToPath (file);
			}

			return $"{header.ResourceHeader}.{file}{Helper.GetExtension (API.CentralAPI.Current.ThemeInfos[name].isOld)}";
		}
	}
}