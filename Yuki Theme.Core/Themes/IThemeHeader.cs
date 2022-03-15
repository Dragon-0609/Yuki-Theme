using System.Reflection;

namespace Yuki_Theme.Core.Themes;

public interface IThemeHeader
{
	string    GroupName      { get; }
	string [] ThemeNames     { get; }
	string    ResourceHeader { get; }

	Assembly Location { get; }

}