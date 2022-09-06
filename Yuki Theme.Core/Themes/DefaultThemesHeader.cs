using System.Collections.Generic;
using System.Reflection;

namespace Yuki_Theme.Core.Themes
{
	public class DefaultThemesHeader : IThemeHeader
	{
		public static string CoreThemeHeader = "Yuki_Theme.Core.Resources.Themes";
	
		public string GroupName => "Default";

		public Dictionary <string, bool> ThemeNames => new ()
		{
			{ "Darcula", true },
			{ "Dracula", true },
			{ "Github Dark", true },
			{ "Github Light", true },
			{ "Monokai Dark", true },
			{ "Monokai Light", true },
			{ "Nightshade", true },
			{ "Oblivion", true },
			{ "Shades of Purple", true }
		};

		public string ResourceHeader => CoreThemeHeader;

		public Assembly Location => Assembly.GetExecutingAssembly ();

		public DefaultThemesHeader ()
		{
		}
	}
}