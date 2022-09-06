using System;
using System.Drawing;
using System.IO;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.Utils;

namespace Yuki_Theme.Core.Parsers
{
	public static class MainParser
	{
		private static JetBrainsParser jetparser;
		private static DokiThemeParser dokiparser;

		public static void Parse (string path, bool ask = true, bool select = true, Action <string, string> defaultTheme = null,
		                          Func <string, string, bool> exist = null, Action <string> addToUIList = null,
		                          Action <string> selectAfterParse = null)
		{
			string fileName = Path.GetFileNameWithoutExtension (path);
			if (IsDefault (fileName))
			{
				if (defaultTheme != null)
					defaultTheme (API.CentralAPI.Current.Translate ("parser.theme.default"), API.CentralAPI.Current.Translate ("messages.theme.default.short"));
				return;
			}

			string pathToSave = Path.Combine (SettingsConst.CurrentPath, $"Themes/{fileName}.yukitheme");
			string pathToSaveEncoded = Path.Combine (SettingsConst.CurrentPath, $"Themes/{Helper.ConvertNameToPath (fileName)}.yukitheme");

			if (fileName.EndsWith (".yuki"))
			{
				pathToSave = Path.Combine (SettingsConst.CurrentPath, $"Themes/{fileName}.yuki");
				pathToSaveEncoded = Path.Combine (SettingsConst.CurrentPath, $"Themes/{Helper.ConvertNameToPath (fileName)}.yuki");
			}

			if (CheckAvailableAndAsk (pathToSaveEncoded, ask, exist))
			{
				string ext = Path.GetExtension (path).ToLower();
				switch (ext)
				{
					case ".yukitheme" :
					case ".yuki" :
					{
						CopyYukiTheme (path, select, addToUIList, selectAfterParse, pathToSaveEncoded, fileName);
					}
						break;
					case ".icls" :
					{
						ParseJetBrainsTheme (path, ask, select, addToUIList, selectAfterParse, pathToSaveEncoded, fileName, pathToSave);
					}
						break;
					case ".json" :
					{
						ParseDokiTheme (path, ask, select, defaultTheme, exist, addToUIList, selectAfterParse, pathToSaveEncoded, fileName,
						                pathToSave);
					}
						break;

					case ".xshd" :
					{
						ParsePascalScheme (path, pathToSaveEncoded);
					}
						break;
				}
			}
		}

		private static void CopyYukiTheme (string path, bool select, Action <string> addToUIList, Action <string> selectAfterParse,
		                                   string pathToSaveEncoded, string fileName)
		{
			File.Copy (path, pathToSaveEncoded, true);
			// form.LoadSchemes ();
			if (addToUIList != null)
				addToUIList (fileName);
			if (select && selectAfterParse != null)
				selectAfterParse (fileName);
		}

		private static void ParseJetBrainsTheme (string          path,             bool   ask, bool select, Action <string> addToUIList,
		                                         Action <string> selectAfterParse, string pathToSaveEncoded,
		                                         string          fileName,         string pathToSave)
		{
			bool has = CheckAvailable (pathToSaveEncoded);
			jetparser = new JetBrainsParser ();
			jetparser.needToWrite = true;
			jetparser.Parse (path, fileName, pathToSave, ask, has, select, addToUIList, selectAfterParse);

			jetparser = null;
			GC.Collect ();
			GC.WaitForPendingFinalizers ();
		}

		private static void ParseDokiTheme (string path, bool ask, bool select, Action <string, string> defaultTheme,
		                                    Func <string, string, bool> exist, Action <string> addToUIList,
		                                    Action <string> selectAfterParse, string pathToSaveEncoded, string fileName, string pathToSave)
		{
			bool has = CheckAvailable (pathToSaveEncoded);
			dokiparser = new DokiThemeParser ();
			dokiparser.needToWrite = true;
			dokiparser.defaultTheme = defaultTheme;
			dokiparser.exist = exist;
			dokiparser.Parse (path, fileName, pathToSave, ask, has, select, addToUIList, selectAfterParse);

			dokiparser = null;
			GC.Collect ();
			GC.WaitForPendingFinalizers ();
		}

		private static void ParsePascalScheme (string path, string pathef)
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
				ZipManager.Zip (pathef, content, wallpaper, sticker, "theme.xshd");
			} else
			{
				File.Copy (path, pathef, true);
			}
		}

		private static bool CheckAvailable (string pathToCheck)
		{
			return File.Exists (pathToCheck);
		}

		public static bool CheckAvailableAndAsk (string filePath, bool ask = true, Func <string, string, bool> exist = null)
		{
			bool wants = true;
			if (File.Exists (filePath) && ask)
			{
				wants = false;
				if (exist != null && exist ("Theme is already exist. Do you want to override?",
				                            "Theme is exist")) wants = true;
			}

			return wants;
		}

		private static bool IsDefault (string fl)
		{
			return DefaultThemes.isDefault (fl);
		}
	}
}