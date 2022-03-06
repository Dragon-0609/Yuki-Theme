using System.Reflection;

namespace Yuki_Theme.Core.Themes;

public class DefaultThemesHeader : IThemeHeader
{
	public static string CoreThemeHeader = "Yuki_Theme.Core.Themes";
	
	public string GroupName => "Default";

	public string [] ThemeNames => new []
	{
		"Darcula",
		"Dracula",
		"Github Dark",
		"Github Light",
		"Monokai Dark",
		"Monokai Light",
		"Nightshade",
		"Oblivion",
		"Shades of Purple"
	};

	public string ResourceHeader => CoreThemeHeader;

	public Assembly Location => Assembly.GetExecutingAssembly ();

	public DefaultThemesHeader ()
	{
	}
}