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
			names.AddRange (header.ThemeNames.Keys);
			categoriesList.Add (header.GroupName);
			headersList.Add (header);
			foreach (KeyValuePair <string, bool> themeName in header.ThemeNames)
			{
				if(!CLI.ThemeInfos.ContainsKey (themeName.Key))
				{
					CLI.AddThemeInfo (themeName.Key, true, themeName.Value, ThemeLocation.Memory);
					categories.Add (themeName.Key, header.GroupName);
					headers.Add (themeName.Key, header);
				}
			}
		}

		public static void addExternalThemes ()
		{
			ExternalThemeManager.LoadThemes ();
		}

		public static void InjectTheme (Theme theme)
		{
			
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
	}
}