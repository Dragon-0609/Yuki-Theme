using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Yuki_Theme.Core.Utils;

namespace Yuki_Theme.Core.Themes
{
	public class DefaultThemes
	{

		public static List<string> names = new ();

		public static bool isDefault (string str)
		{
			return names.Contains (str);
		}

		internal static void addDefaultThemes ()
		{
			DefaultThemesHeader header = new DefaultThemesHeader ();
			addHeader (header);
		}

		internal static void addHeader (IThemeHeader header)
		{
			names.AddRange (header.ThemeNames.Keys);
			categoriesList.Add (header.GroupName);
			headersList.Add (header);
			foreach (KeyValuePair<string, bool> themeName in header.ThemeNames)
			{
				if(!API.CentralAPI.Current.ThemeInfos.ContainsKey (themeName.Key))
				{
					API.CentralAPI.Current.AddThemeInfo (themeName.Key, new ThemeInfo (true, themeName.Value, ThemeLocation.Memory, header.GroupName));
					categories.Add (themeName.Key, header.GroupName);
					headers.Add (themeName.Key, header);
				}
			}
		}

		internal static void addExternalThemes ()
		{
			ExternalThemeManager.LoadThemes ();
		}

		public static void InjectTheme (Theme theme)
		{
			
		}
		
		public static Dictionary<string, string> categories     = new ();
		public static List<string>               categoriesList = new ();

		public static Dictionary<string, IThemeHeader> headers     = new ();
		public static List<IThemeHeader>               headersList = new ();

		public static string getCategory (string st)
		{
			string res = "Custom";

			if (categories.ContainsKey (st))
				res = categories [st];
			return res;
		}

		public static void Clear ()
		{
			names.Clear ();
			categories.Clear ();
			categoriesList.Clear ();
			headers.Clear ();
			headersList.Clear ();
		}

		internal static void DistinctThemeNames ()
		{
			IEnumerable<string> distinctNames = names.Distinct ();
			names = distinctNames.ToList ();
		}
	}
}