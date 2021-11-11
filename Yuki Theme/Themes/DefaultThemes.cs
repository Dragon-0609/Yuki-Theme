using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Yuki_Theme.Themes
{
	public class DefaultThemes
	{
		public static string [] def
		{
			get
			{
				return new string []
				{
					"Darcula", "Dracula", "Github Dark", "Github Light", "Monokai Dark", "Monokai Light", "Nightshade",
					"Oblivion", "Shades of Purple"
				};
			}
		}

		public static bool isDefault (string str)
		{
			return def.Contains (str);
		}
	}
}