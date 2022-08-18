using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using Yuki_Theme.Core.Parsers;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.Utils;

namespace Yuki_Theme.Core
{
	public static class API
	{
		#region Public Fields

		public static  List <string> names   = new ();
		private static List <string> _schemes = new ();


		private static Dictionary <string, ThemeInfo> _themeInfos = new ();

		public static Theme currentTheme = ThemeFunctions.LoadDefault ();

		#region ThemeLoading

		public static string nameToLoad;
		public static string pathToLoad;

		#endregion

		public static string selectedItem = "empty";
		public static bool   isEdited;


		internal static ThemeFormatBase _newThemeFormat = new NewThemeFormat ();
		internal static ThemeFormatBase _oldThemeFormat = new OldThemeFormat ();
		internal static ThemeManager    _themeManager   = new ();
		private static  API_Actions     _actions        = new ();

		public static List <string> Schemes => _schemes;

		public static Dictionary <string, ThemeInfo> ThemeInfos => _themeInfos;

		#endregion


		#region Main Commands

		/// <summary>
		/// Load Themes from default themes and from 'Themes' directory
		/// </summary>
		/// <param name="ifZero">If there isn't any theme, ask to set it</param>
		public static void LoadSchemes (Func <string> ifZero = null)
		{
			ClearThemeInfos ();
			DefaultThemes.Clear ();
			DefaultThemes.addDefaultThemes ();
			DefaultThemes.addExternalThemes ();
			DefaultThemes.DistinctThemeNames ();
			Schemes.AddRange (DefaultThemes.names);
			Helper.CreateThemeDirectory ();
			LoadExternalThemes ();
			IfThemesNotFound (ifZero);
		}

		/// <summary>
		/// Copy theme
		/// </summary>
		/// <param name="copyFrom">Copy from</param>
		/// <param name="name">Copy to</param>
		/// <returns>0 -> Theme isn't added cause of exceptions. 1 -> Theme is added. 2 -> Theme is overriden</returns>
		public static int AddTheme (string copyFrom, string name)
		{
			if (name.Length < 3)
			{
				if (API_Events.showError != null)
					API_Events.showError (Translate ("messages.name.short.full"), Translate ("messages.name.short.short"));
				return 0;
			}

			string path = Helper.ConvertNameToPath (name);
			string destination = PathGenerator.PathToFile (path, _themeInfos [copyFrom].isOld);
			Helper.CreateThemeDirectory ();

			bool exist = false;

			if (File.Exists (destination))
			{
				if (_actions.AskToOverrideFile (destination, ref exist, out var add)) return add;
			}

			if (!DefaultThemes.isDefault (name))
			{
				return _themeManager.AddTheme (copyFrom, name, destination, exist);
			}

			if (API_Events.showError != null)
				API_Events.showError (Translate ("messages.name.default.full"), Translate ("messages.name.default.short"));

			return 0;
		}

		/// <summary>
		/// Delete the theme
		/// </summary>
		/// <param name="name">Theme to be deleted</param>
		/// <param name="askToDelete">Ask to delete</param>
		/// <param name="afterAsk">Do action after asked</param>
		/// <param name="afterDelete">Do action after deleted</param>
		public static void RemoveTheme (string                  name, Func <string, string, bool> askToDelete, Func <string, object> afterAsk = null,
		                           Action <string, object> afterDelete = null)
		{
			Helper.CreateThemeDirectory ();
			string sft = Helper.ConvertNameToPath (name);
			if (!_themeInfos [name].isDefault)
			{
				if (File.Exists (PathGenerator.PathToFile (sft, _themeInfos [name].isOld)))
				{
					if (askToDelete (Translate ("messages.delete.full", name), Translate ("messages.delete.short")))
					{
						object bn = null;
						if (afterAsk != null) bn = afterAsk (sft);

						Settings.SaveData ();
						File.Delete (PathGenerator.PathToFile (sft, _themeInfos [name].isOld));
						_schemes.Remove (sft);
						_themeInfos.Remove (sft);
						if (afterDelete != null) afterDelete (sft, bn);
					}
				} else
				{
					if (API_Events.showError != null)
						API_Events.showError (Translate ("messages.theme.file.notfound.full"),
						                      Translate ("messages.theme.file.notfound.short"));
				}
			} else
			{
				if (API_Events.showError != null)
					API_Events.showError (Translate ("messages.theme.default.full"), Translate ("messages.theme.default.short"));
			}
		}

		/// <summary>
		/// Save current theme (currentFile string)
		/// </summary>
		/// <param name="wallpaper">Background image</param>
		/// <param name="sticker">Sticker</param>
		/// <param name="wantToKeep">Want to keep background image and sticker</param>
		public static void Save (Image wallpaper = null, Image sticker = null, bool wantToKeep = false)
		{
			Helper.CreateThemeDirectory ();
			if (!IsDefault ())
			{
				SaveTheme (currentTheme, wallpaper, sticker, wantToKeep);
				if (isEdited) isEdited = false;
			}
		}

		/// <summary>
		/// Export current theme to pascal directory
		/// </summary>
		/// <param name="wallpaper">Background image</param>
		/// <param name="sticker">Sticker</param>
		/// <param name="setTheme">After theme has been set. You can use it to apply changes</param>
		/// <param name="startSettingTheme">When start to export. You can use it to release old images</param>
		/// <param name="wantToKeep">Want to keep background image and sticker</param>
		public static void ExportTheme (Image wallpaper, Image sticker, Action setTheme = null, Action startSettingTheme = null, bool wantToKeep = false)
		{
			_actions.AskToSaveInExport (wallpaper, sticker, wantToKeep);

			if (Settings.pascalPath.Length < 6 && Helper.mode != ProductMode.Plugin)
			{
				API_Events.setPath (Translate ("messages.path.select.inexport.full"), Translate ("messages.path.select.inexport.short"));
			}

			if (Settings.pascalPath.Length > 6 || Helper.mode == ProductMode.Plugin)
			{
				if (startSettingTheme != null)
					startSettingTheme ();
				string path = Path.Combine (Settings.pascalPath, "Highlighting", $"{pathToLoad}.xshd");

				_themeManager.DeleteOldThemeIfNeed ();

				_themeManager.CopyThemeToDirectory (path);

				PrepareToExport (path);

				_actions.ShowEndMessage (currentTheme.Name);

				Helper.currentTheme = currentTheme.Name;
				if (setTheme != null)
					setTheme ();
			} else
			{
				if (API_Events.showError != null)
					API_Events.showError (Translate ("messages.export.error.full"), Translate ("messages.export.error.short"));
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
				_themeManager.DeleteOldTheme ();
			}

			if (syntax != SyntaxType.NULL)
			{
				string dir = Path.GetDirectoryName (path);
				_actions.MergeSyntax (dir, syntax);
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
		public static void ImportTheme (string path, Func <string, string, bool> exist)
		{
			MainParser.Parse (path, true, true, API_Events.showError, exist);
		}
		
		/// <summary>
		/// Import theme
		/// </summary>
		/// <param name="path">Theme from</param>
		/// <param name="exist">Do if exist</param>
		public static void ImportTheme (string path, bool ask = true, bool select = true, Action <string, string> defaultTheme = null, Func <string, string, bool> exist = null, Action <string> addToUIList = null, Action <string> selectAfterParse = null)
		{
			MainParser.Parse (path, ask, select, defaultTheme, exist, addToUIList, selectAfterParse);
		}

		/// <summary>
		/// Rename the theme
		/// </summary>
		/// <param name="from">From</param>
		/// <param name="to">To</param>
		/// <returns>0 -> error. 1 -> success</returns>
		public static int RenameTheme (string from, string to)
		{
			bool canShowError = API_Events.showError != null;
			if (_actions.CheckNameToRenameTo (to, canShowError, out int rename)) return rename;

			if (!_themeInfos [@from].isDefault)
			{
				string fromP = Helper.ConvertNameToPath (@from);
				string toP = Helper.ConvertNameToPath (to);

				string fromPath = PathGenerator.PathToFile (fromP, _themeInfos [@from].isOld);
				string toPath = PathGenerator.PathToFile (toP, _themeInfos [@from].isOld);

				if (!File.Exists (fromPath))
				{
					ShowError (canShowError, "messages.theme.notexist.full", "messages.theme.notexist.short");
					return 0;
				}

				if (!File.Exists (toPath))
				{
					if (!DefaultThemes.isDefault (to))
					{
						File.Move (fromPath, toPath);
						_themeManager.WriteName (toPath, to);

						_themeInfos.RenameKey (@from, to);

						if (API_Events.onRename != null) API_Events.onRename (@from, to);
						return 1;
					}

					ShowError (canShowError, "messages.name.default.full", "messages.name.default.short");
				} else
				{
					ShowError (canShowError, "messages.name.exist.full", "messages.name.exist.short");
				}
			} else
			{
				ShowError (canShowError, "messages.theme.default.full", "messages.theme.default.short");
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
			_actions.PopulateList (onSelect);
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
		public static bool IsPascalDirectory (string path)
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
			if (_actions.IsNewTheme (path))
				return _newThemeFormat.GetNameOfTheme (path);
			return _oldThemeFormat.GetNameOfTheme (path);
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
			if (!IsDefault ())
			{
				if (themeToSave.IsOld)
					_oldThemeFormat.SaveTheme (themeToSave, img2, img3, wantToKeep);
				else
					_newThemeFormat.SaveTheme (themeToSave, img2, img3, wantToKeep);
			}
		}

		private static void PrepareToExport (string path)
		{
			string dir = Path.GetDirectoryName (path);
			foreach (SyntaxType syntax in (SyntaxType [])Enum.GetValues (typeof (SyntaxType)))
			{
				if (syntax != SyntaxType.NULL) _actions.MergeSyntax (dir, syntax);
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

		/// <summary>
		/// Get this assembly
		/// </summary>
		/// <returns></returns>
		public static Assembly GetCore ()
		{
			return Assembly.GetExecutingAssembly ();
		}

		#region Referenced Methods

		public static Theme GetTheme (string name) => _themeManager.GetTheme (name);
		
		public static bool IsDefault () => _actions.IsDefault ();

		public static bool ReGenerateTheme (string path, string oldPath, string name, string oldName, bool forceRegenerate) =>
			_actions.ReGenerateTheme (path, oldPath, name, oldName, forceRegenerate);

		public static bool IsOldTheme (string path) => _actions.IsOldTheme (path);

		public static void ExtractSyntaxTemplate (SyntaxType syntax, string destination) =>
			_actions.ExtractSyntaxTemplate (syntax, destination);

		public static void CopyFromMemory (string file, string name, string path, bool extract = false) =>
			_themeManager.CopyFromMemory (file, name, path, extract);

		public static void AddThemeInfo (string name, ThemeInfo themeInfo) => _actions.AddThemeInfo (name, themeInfo);

		#endregion
		
		private static void ClearThemeInfos ()
		{
			_schemes.Clear ();
			_themeInfos.Clear ();
		}
		
		private static void LoadExternalThemes ()
		{
			if (Directory.Exists (Path.Combine (SettingsConst.CurrentPath, "Themes")))
			{
				_actions.LoadSchemesByExtension (Helper.FILE_EXTENSTION_OLD);
				_actions.LoadSchemesByExtension (Helper.FILE_EXTENSTION_NEW);
			}
		}

		private static void IfThemesNotFound (Func<string> ifZero)
		{
			if (_schemes.Count == 0)
			{
				if (ifZero != null)
				{
					string sm = ifZero ();
					if (sm != null)
					{
						nameToLoad = Path.GetFileNameWithoutExtension (sm);
						File.Copy (sm, PathGenerator.PathToFile (pathToLoad, true));
						_schemes.Add (nameToLoad);
					}
				}
			}
		}
		
		public static void ShowError (string message, string title)
		{
			_actions.ShowError (true, message, title);
		}
		
		public static void ShowError (bool canShowError, string message, string title)
		{
			_actions.ShowError (canShowError, message, title);
		}
		
		#endregion

	}
}