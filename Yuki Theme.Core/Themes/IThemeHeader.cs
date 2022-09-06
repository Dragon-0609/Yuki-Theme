using System.Collections.Generic;
using System.Reflection;

namespace Yuki_Theme.Core.Themes
{
	public interface IThemeHeader
	{
		string GroupName { get; }
		/// <summary>
		/// Theme names. Boolean is indicator of extension whether a theme is old or new. True -> Old, False -> New
		/// </summary>
		Dictionary <string, bool> ThemeNames { get; }
		string ResourceHeader { get; }

		Assembly Location { get; }

	}
}