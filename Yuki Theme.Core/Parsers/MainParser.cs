using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.Parsers
{
	public static class MainParser
	{
		private static JetBrainsParser jetparser;
		private static DokiThemeParser dokiparser;

		public static void Parse (string path, MForm form = null, bool ask = true, bool select = true, Action<string, string> defaultTheme = null, Func <string, string, bool> exist = null)
		{
			string st = Path.GetFileNameWithoutExtension (path);
			if (isDefault (st))
			{
				if (defaultTheme != null) defaultTheme ("The theme is default! You can't import it!", "Default Theme");
				return;
			}

			string pathToSave =Path.Combine (CLI.currentPath,  $"Themes/{st}.yukitheme");
			string pathef =Path.Combine (CLI.currentPath,  $"Themes/{Helper.ConvertNameToPath (st)}.yukitheme");

			if (st.EndsWith (".yuki"))
			{
				pathToSave =Path.Combine (CLI.currentPath,  $"Themes/{st}.yuki");
                pathef =Path.Combine (CLI.currentPath,  $"Themes/{Helper.ConvertNameToPath (st)}.yuki");
			}
			
			if (checkAvailableAndAsk ( pathef, ask, exist))
			{
				string ext = Path.GetExtension (path);
				switch (ext)
				{
					case ".yukitheme" :
					case ".yuki" :
					{
						File.Copy (path, pathef, true);
						form.load_schemes ();
					}
						break;
					case ".icls" :
					{
						bool has = checkAvailable (pathef);
						jetparser = new JetBrainsParser ();
						jetparser.Parse (path, st, pathToSave, form, ask, has, select);
						
						jetparser = null;
						GC.Collect();
						GC.WaitForPendingFinalizers();
					}
						break;
					case ".json" :
					{
						bool has = checkAvailable (pathef);
						dokiparser = new DokiThemeParser ();
						dokiparser.defaultTheme = defaultTheme;
						dokiparser.exist = exist;
						dokiparser.Parse (path, st, pathToSave, form, ask, has, select);
						
						dokiparser = null;
						GC.Collect();
						GC.WaitForPendingFinalizers();
					}
						break;

					case ".xshd" :
					{
						string directory = Path.GetDirectoryName (path);
						bool isWallpaperExist = false;
						bool isStickerExist = false;
						
						if (File.Exists (Path.Combine (directory, "background.png")))
						{
							isWallpaperExist = true;
						}
						
						if (File.Exists (Path.Combine (directory, "sticker.png")))
						{
							isStickerExist = true;
						}
						
						if (isWallpaperExist || isStickerExist)
						{
							string content = File.ReadAllText (path);
							Image wallpaper = null;
							Image sticker = null;
							if (isWallpaperExist) wallpaper = Image.FromFile (Path.Combine (directory, "background.png"));
							if (isStickerExist) sticker = Image.FromFile (Path.Combine (directory, "sticker.png"));
							Helper.Zip (pathef, content, wallpaper, sticker, "theme.xshd", true);
						} else
						{
							File.Copy (path, pathef, true);							
						}
						form.load_schemes ();
					}
						break;
				}
			}
		}

		private static bool checkAvailable (string nxpath)
		{
			return File.Exists (nxpath);
		}

		public static bool checkAvailableAndAsk (string nxpath, bool ask = true, Func <string, string, bool> exist = null)
		{

			bool wants = true;
			if (File.Exists (nxpath) && ask)
			{
				wants = false;
				if (exist != null && exist ("Theme is already exist. Do you want to override?",
				                            "Theme is exist")) wants = true;

			}

			return wants;
		}

		private static bool isDefault (string fl)
		{
			return DefaultThemes.isDefault (fl);
		}
	}
}