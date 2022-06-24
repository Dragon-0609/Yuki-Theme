using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Newtonsoft.Json;
using Yuki_Theme.Core.Formats;
using Yuki_Theme.Core.Parsers;
using Yuki_Theme.Core.Themes;
using Formatting = Newtonsoft.Json.Formatting;

namespace Yuki_Theme.Core
{
	public static class API
	{
		#region Public Fields

		public static List <string> names   = new ();
		public static List <string> schemes = new ();

		public static Dictionary <string, ThemeInfo> themeInfos = new ();

		public static Theme currentTheme = ThemeFunctions.LoadDefault ();

		#region ThemeLoading

		public static string nameToLoad;
		public static string pathToLoad;

		#endregion

		public static string selectedItem = "empty";
		public static string currentPath  = Path.GetDirectoryName (Assembly.GetEntryAssembly ()?.Location);
		public static bool   isEdited;

		#endregion


		#region Main Commands

		/// <summary>
		/// Load Themes from default themes and from 'Themes' directory
		/// </summary>
		/// <param name="ifZero">If there isn't any theme, ask to set it</param>
		public static void LoadSchemes (Func <string> ifZero = null)
		{
			schemes.Clear ();
			themeInfos.Clear ();
			DefaultThemes.Clear ();
			DefaultThemes.addDefaultThemes ();
			DefaultThemes.addExternalThemes ();
			schemes.AddRange (DefaultThemes.names);
			Helper.CreateThemeDirectory ();
			if (Directory.Exists (Path.Combine (currentPath, "Themes")))
			{
				API_Actions.LoadSchemesByExtension (Helper.FILE_EXTENSTION_OLD);
				API_Actions.LoadSchemesByExtension (Helper.FILE_EXTENSTION_NEW);
			}

			if (schemes.Count == 0)
			{
				if (ifZero != null)
				{
					string sm = ifZero ();
					if (sm != null)
					{
						nameToLoad = Path.GetFileNameWithoutExtension (sm);
						File.Copy (sm, PathGenerator.PathToFile (pathToLoad, true));
						schemes.Add (nameToLoad);
					}
				}
			}
		}

		/// <summary>
		/// Copy theme
		/// </summary>
		/// <param name="copyFrom">Copy from</param>
		/// <param name="name">Copy to</param>
		/// <returns>0 -> Theme isn't added cause of exceptions. 1 -> Theme is added. 2 -> Theme is overrided</returns>
		public static int Add (string copyFrom, string name)
		{
			if (name.Length < 3)
			{
				if (API_Actions.showError != null)
					API_Actions.showError (Translate ("messages.name.short.full"), Translate ("messages.name.short.short"));
				return 0;
			}

			string path = Helper.ConvertNameToPath (name);
			string destination = PathGenerator.PathToFile (path, themeInfos [copyFrom].isOld);
			Helper.CreateThemeDirectory ();

			bool exist = false;

			if (File.Exists (destination))
			{
				if (API_Actions.AskToOverrideFile (destination, ref exist, out var add)) return add;
			}

			if (!DefaultThemes.isDefault (name))
			{
				return API_Actions.AddTheme (copyFrom, name, destination, exist);
			}

			if (API_Actions.showError != null)
				API_Actions.showError (Translate ("messages.name.default.full"), Translate ("messages.name.default.short"));

			return 0;
		}

		public static bool CopyTheme (string copyFrom, string themeName, string destination, out string path, bool check)
		{
			if (check && themeInfos [copyFrom].location == ThemeLocation.Memory)
			{
				path = PathGenerator.PathToMemory (copyFrom);
				if (path == null)
				{
					if (API_Actions.showError != null)
						API_Actions.showError (Translate ("messages.theme.memory.notfound.full"),
						                       Translate ("messages.theme.memory.notfound.short"));

					return true;
				}

				API_Actions.CopyFromMemory (copyFrom, copyFrom, destination);
			} else
			{
				path = PathGenerator.PathToFile (Helper.ConvertNameToPath (copyFrom), themeInfos [copyFrom].isOld);
				File.Copy (path, destination);
			}

			return false;
		}

		/// <summary>
		/// Delete the theme
		/// </summary>
		/// <param name="name">Theme to be deleted</param>
		/// <param name="askToDelete">Ask to delete</param>
		/// <param name="afterAsk">Do action after asked</param>
		/// <param name="afterDelete">Do action after deleted</param>
		public static void Remove (string name, Func <string, string, bool> askToDelete, Func <string, object> afterAsk = null,
		                           Action <string, object> afterDelete = null)
		{
			Helper.CreateThemeDirectory ();
			string sft = Helper.ConvertNameToPath (name);
			if (!themeInfos [name].isDefault)
			{
				if (File.Exists (PathGenerator.PathToFile (sft, themeInfos [name].isOld)))
				{
					if (askToDelete (Translate ("messages.delete.full", name), Translate ("messages.delete.short")))
					{
						object bn = null;
						if (afterAsk != null) bn = afterAsk (sft);

						Settings.SaveData ();
						File.Delete (PathGenerator.PathToFile (sft, themeInfos [name].isOld));
						schemes.Remove (sft);
						themeInfos.Remove (sft);
						if (afterDelete != null) afterDelete (sft, bn);
					}
				} else
				{
					if (API_Actions.showError != null)
						API_Actions.showError (Translate ("messages.theme.file.notfound.full"),
						                       Translate ("messages.theme.file.notfound.short"));
				}
			} else
			{
				if (API_Actions.showError != null)
					API_Actions.showError (Translate ("messages.theme.default.full"), Translate ("messages.theme.default.short"));
			}
		}

		/// <summary>
		/// Save current theme (currentFile string)
		/// </summary>
		/// <param name="img2">Background image</param>
		/// <param name="img3">Sticker</param>
		/// <param name="wantToKeep">Want to keep background image and sticker</param>
		public static void Save (Image img2 = null, Image img3 = null, bool wantToKeep = false)
		{
			Helper.CreateThemeDirectory ();
			Console.WriteLine ("{0}, {1}", nameToLoad, API_Actions.IsDefault ());
			if (!API_Actions.IsDefault ())
			{
				SaveTheme (currentTheme, img2, img3, wantToKeep);
				if (isEdited) isEdited = false;
			}
		}

		/// <summary>
		/// Export current theme to pascal directory
		/// </summary>
		/// <param name="img2">Background image</param>
		/// <param name="img3">Sticker</param>
		/// <param name="setTheme">After theme has been set. You can use it to apply changes</param>
		/// <param name="startSettingTheme">When start to export. You can use it to release old images</param>
		/// <param name="wantToKeep">Want to keep background image and sticker</param>
		public static void Export (Image img2, Image img3, Action setTheme = null, Action startSettingTheme = null, bool wantToKeep = false)
		{
			API_Actions.AskToSaveInExport (img2, img3, wantToKeep);

			if (Settings.pascalPath.Length < 6 && Helper.mode != ProductMode.Plugin)
			{
				API_Actions.setPath (Translate ("messages.path.select.inexport.full"), Translate ("messages.path.select.inexport.short"));
			}

			if (Settings.pascalPath.Length > 6 || Helper.mode == ProductMode.Plugin)
			{
				if (startSettingTheme != null)
					startSettingTheme ();
				string path = Path.Combine (Settings.pascalPath, "Highlighting", $"{pathToLoad}.xshd");

				API_Actions.DeleteOldThemeIfNeed ();

				API_Actions.CopyThemeToDirectory (path);

				PrepareToExport (path);

				API_Actions.ShowEndMessage ();

				Helper.currentTheme = currentTheme.Name;
				if (setTheme != null)
					setTheme ();
			} else
			{
				if (API_Actions.showError != null)
					API_Actions.showError (Translate ("messages.export.error.full"), Translate ("messages.export.error.short"));
			}
		}

		/// <summary>
		/// Export just .xshd file without images. It can be used for preview a theme
		/// </summary>
		/// <param name="syntax">Syntax type to export</param>
		/// <param name="needToDelete">Need to delete old files</param>
		/// <param name="setTheme">After theme has been set. You can use it to apply changes</param>
		public static void Preview (SyntaxType syntax, bool needToDelete, Action setTheme = null)
		{
			string path = Path.Combine (Settings.pascalPath, "Highlighting", $"{pathToLoad}.xshd");
			if (needToDelete)
			{
				API_Actions.DeleteOldTheme ();
			}

			if (syntax != SyntaxType.NULL)
			{
				string dir = Path.GetDirectoryName (path);
				API_Actions.MergeSyntax (dir, syntax);
			} else
			{
				PrepareToExport (path);
			}

			Helper.currentTheme = currentTheme.Name;
			if (setTheme != null)
				setTheme ();
		}

		/// <summary>
		/// Import theme
		/// </summary>
		/// <param name="path">Theme from</param>
		/// <param name="exist">Do if exist</param>
		public static void Import (string path, Func <string, string, bool> exist)
		{
			MainParser.Parse (path, true, true, API_Actions.showError, exist);
		}

		/// <summary>
		/// Rename the theme
		/// </summary>
		/// <param name="from">From</param>
		/// <param name="to">To</param>
		/// <returns>0 -> error. 1 -> success</returns>
		public static int Rename (string from, string to)
		{
			bool canShowError = API_Actions.showError != null;
			if (API_Actions.CheckNameToRenameTo (to, canShowError, out int rename)) return rename;

			if (!themeInfos [from].isDefault)
			{
				string fromP = Helper.ConvertNameToPath (from);
				string toP = Helper.ConvertNameToPath (to);

				string fromPath = PathGenerator.PathToFile (fromP, themeInfos [from].isOld);
				string toPath = PathGenerator.PathToFile (toP, themeInfos [from].isOld);

				if (!File.Exists (fromPath))
				{
					API_Actions.ShowError (canShowError, "messages.theme.notexist.full", "messages.theme.notexist.short");
					return 0;
				}

				if (!File.Exists (toPath))
				{
					if (!DefaultThemes.isDefault (to))
					{
						File.Move (fromPath, toPath);
						API_Actions.WriteName (toPath, to);

						themeInfos.RenameKey (@from, to);

						if (API_Actions.onRename != null) API_Actions.onRename (@from, to);
						return 1;
					}

					API_Actions.ShowError (canShowError, "messages.name.default.full", "messages.name.default.short");
				} else
				{
					API_Actions.ShowError (canShowError, "messages.name.exist.full", "messages.name.exist.short");
				}
			} else
			{
				API_Actions.ShowError (canShowError, "messages.theme.default.full", "messages.theme.default.short");
			}

			return 0;
		}

		/// <summary>
		/// Restore to saved (default) state 
		/// </summary>
		/// <param name="wantClean">Do you want to clean garbage?</param>
		/// <param name="onSelect">Action, after populating list</param>
		public static void Restore (bool wantClean = true, Action onSelect = null)
		{
			isEdited = false;
			if (currentTheme.Fields != null)
				currentTheme.Fields.Clear ();
			names.Clear ();
			API_Actions.PopulateList (onSelect);
			if (wantClean)
			{
				GC.Collect ();
				GC.WaitForPendingFinalizers ();
			}
		}

		public static bool SelectTheme (string name)
		{
			nameToLoad = name;
			pathToLoad = Helper.ConvertNameToPath (name);
			return true;
		}

		#endregion


		#region Helper Methods

		/// <summary>
		/// Check if the path is Pascal Directory. To check it, I check if there is <code>Highlighting</code> directory in it.
		/// </summary>
		/// <param name="path">Path</param>
		/// <returns>True if it is Pascal Directory</returns>
		public static bool IsPasalDirectory (string path)
		{
			return Directory.Exists (Path.Combine (path, "Highlighting"));
		}

		/// <summary>
		/// Get name of the theme.
		/// </summary>
		/// <param name="path">Path to the theme</param>
		/// <returns>Name of the theme</returns>
		public static string GetNameOfTheme (string path)
		{
			// Console.WriteLine ("Path to get theme name: {0}", path);
			if (API_Actions.IsNewTheme (path))
				return NewThemeFormat.GetNameOfTheme (path);
			return OldThemeFormat.GetNameOfTheme (path);
		}


		/// <summary>
		/// Save theme to file
		/// </summary>
		/// <param name="themeToSave">Theme to be saved</param>
		/// <param name="img2">Background image</param>
		/// <param name="img3">Sticker</param>
		/// <param name="wantToKeep">Want to keep old wallpaper and sticker if file is exist</param>
		public static void SaveTheme (Theme themeToSave, Image img2 = null, Image img3 = null, bool wantToKeep = false)
		{
			if (!API_Actions.IsDefault ())
			{
				if (themeToSave.IsOld)
					OldThemeFormat.saveTheme (themeToSave, img2, img3, wantToKeep);
				else
					NewThemeFormat.saveTheme (themeToSave, img2, img3, wantToKeep);
			}
		}

		private static void PrepareToExport (string path)
		{
			string dir = Path.GetDirectoryName (path);
			// Console.WriteLine(currentTheme.Fields["Method"].ToString ());
			foreach (SyntaxType syntax in (SyntaxType [])Enum.GetValues (typeof (SyntaxType)))
			{
				if (syntax != SyntaxType.NULL) API_Actions.MergeSyntax (dir, syntax);
			}
		}

		public static string Translate (string key)
		{
			return Settings.translation.GetText (key);
		}

		public static string Translate (string key, string p1)
		{
			return Settings.translation.GetText (key).Replace ("{0}", p1);
		}

		public static string Translate (string key, string p1, string p2)
		{
			return Settings.translation.GetText (key).Replace ("{0}", p1).Replace ("{1}", p2);
		}

		#endregion
	}
}