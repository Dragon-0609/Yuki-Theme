using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Yuki_Theme.Core.Themes
{
	public class DefaultThemes
	{

		public static List <string> names = new ();

		public static bool isDefault (string str)
		{
			return names.Contains (str);
		}

		public static void addDefaultThemes ()
		{
			DefaultThemesHeader header = new DefaultThemesHeader ();
			addHeader (header);
		}

		public static void addHeader (IThemeHeader header)
		{
			names.AddRange (header.ThemeNames);
			categoriesList.Add (header.GroupName);
			headersList.Add (header);
			foreach (string themeName in header.ThemeNames)
			{
				if(!CLI.isDefaultTheme.ContainsKey (themeName))
				{
					CLI.isDefaultTheme.Add (themeName, true);
					categories.Add (themeName, header.GroupName);
					headers.Add (themeName, header);
				}
			}
		}

		public static void addExternalThemes ()
		{
			ExternalThemeManager.LoadThemes ();
		}

		public static void addOldNewThemeDifference (ref Dictionary <string, bool> list)
		{
			foreach (string theme in names)
			{
				ThemeFormat extension = Helper.GetThemeFormat (true, Helper.ConvertNameToPath (theme), theme);
				list.Add (theme, extension == ThemeFormat.Old);
			}
		}

		public static Dictionary <string, string> categories     = new ();
		public static List <string>               categoriesList = new ();

		public static Dictionary <string, IThemeHeader> headers     = new ();
		public static List <IThemeHeader>               headersList = new ();

		public static string getCategory (string st)
		{
			string res = "Custom";

			if (categories.ContainsKey (st))
				res = categories [st];
			return res;
		}

		public static void Clear ()
		{
			categories.Clear ();
			categoriesList.Clear ();
			headers.Clear ();
			headersList.Clear ();
			names.Clear ();
		}
	}
}