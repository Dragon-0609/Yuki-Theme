using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Yuki_Theme.Core.Themes;

public static class ExternalThemeManager
{


	public static void LoadThemes ()
	{
		string [] files = Directory.GetFiles (CLI.currentPath, "*Themes.dll");
		if (files.Length > 0)
			AddThemesToDB (files);
		files = Directory.GetFiles (Path.Combine (CLI.currentPath, "Themes"), "*.dll");
		if (files.Length > 0)
			AddThemesToDB (files);	
	}

	private static void AddThemesToDB (string [] files)
	{
		foreach (string file in files)
		{
			Assembly assembly = Assembly.LoadFile (file);
			Type [] types = assembly.GetTypes ();
			Type themeHeader = types.FirstOrDefault (i => typeof (IThemeHeader).IsAssignableFrom (i) && i.IsClass);
			if (themeHeader != null)
			{
				IThemeHeader header = (IThemeHeader)Activator.CreateInstance (themeHeader);
				DefaultThemes.addHeader (header);
			}
		}
	}
}