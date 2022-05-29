using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;
using Yuki_Theme.Core.Database;
using Yuki_Theme.Core.Formats;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Parsers;
using Yuki_Theme.Core.Themes;
using Formatting = Newtonsoft.Json.Formatting;

namespace Yuki_Theme.Core
{
	public static class CLI
	{

		#region Public Fields

		public static List <string>                  names      = new ();
		public static List <string>                  schemes    = new ();
		public static Dictionary <string, ThemeInfo> ThemeInfos = new ();

		public static Theme currentTheme = ThemeFunctions.LoadDefault ();

		#region ThemeLoading

		public static string nameToLoad;
		public static string pathToLoad;

		#endregion

		public static string selectedItem = "empty";
		public static string currentPath  = Path.GetDirectoryName (Assembly.GetEntryAssembly ()?.Location);
		public static bool   isEdited;
		public static string groupName = "";

		#endregion


		#region Main Commands

		/// <summary>
		/// Load Themes from default themes and from 'Themes' directory
		/// </summary>
		/// <param name="ifZero">If there isn't any theme, ask to set it</param>
		public static void load_schemes (Func <string> ifZero = null)
		{
			schemes.Clear ();

			ThemeInfos.Clear ();
			DefaultThemes.addDefaultThemes ();
			DefaultThemes.addExternalThemes ();
			schemes.AddRange (DefaultThemes.names);
			Helper.CreateThemeDirectory ();
			if (Directory.Exists (Path.Combine (currentPath, "Themes")))
			{
				LoadSchemesByExtension (Helper.FILE_EXTENSTION_OLD);
				LoadSchemesByExtension (Helper.FILE_EXTENSTION_NEW);
			}

			if (schemes.Count == 0)
			{
				if (ifZero != null)
				{
					string sm = ifZero ();
					if (sm != null)
					{
						nameToLoad = Path.GetFileNameWithoutExtension (sm);
						File.Copy (sm, pathToFile (pathToLoad, true));
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
		/// <returns></returns>
		public static bool add (string copyFrom, string name)
		{
			if (name.Length < 2)
			{
				if (CLI_Actions.showError != null) CLI_Actions.showError (Translate ("messages.name.short.full"), Translate ("messages.name.short.short"));
				return true;
			}

			string path = Helper.ConvertNameToPath (name);
			string destination = pathToFile (path, ThemeInfos [copyFrom].isOld);
			Helper.CreateThemeDirectory ();

			bool exist = false;

			if (File.Exists (destination))
			{
				if (!CLI_Actions.SaveInExport (Translate ("messages.file.exist.override.full"), Translate ("messages.file.exist.override.short")))
				{
					if (CLI_Actions.showError != null) CLI_Actions.showError (Translate ("messages.name.exist.full"), Translate ("messages.name.exist.short"));
					return true;
				}

				exist = true;
				File.Delete (destination);
			}

			if (!DefaultThemes.isDefault (name))
			{
				string pth = "";
				if (CopyTheme (copyFrom, name, destination, out pth, true)) return true;
				if (!exist)
				{
					schemes.Add (name);
					
					AddThemeInfo (name, false, ThemeInfos[copyFrom].isOld, ThemeLocation.File);
				}

				if (Helper.mode == ProductMode.CLI)
					if (CLI_Actions.showSuccess != null)
						CLI_Actions.showSuccess (Translate ("messages.theme.duplicate"), Translate ("messages.buttons.done"));

				return exist;
			} else
			{
				if (CLI_Actions.showError != null)
					CLI_Actions.showError (Translate ("messages.name.default.full"), Translate ("messages.name.default.short"));

				return true;
			}
		}

		public static bool CopyTheme (string copyFrom, string themeName, string destination, out string path, bool check)
		{
			if (check && ThemeInfos [copyFrom].location == ThemeLocation.Memory)
			{
				path = pathToMemory (copyFrom);
				if (path == null)
				{
					if (CLI_Actions.showError != null)
						CLI_Actions.showError (Translate ("messages.theme.memory.notfound.full"), Translate ("messages.theme.memory.notfound.short"));

					return true;
				}

				CopyFromMemory (copyFrom, copyFrom, destination);
			} else
			{
				path = pathToFile (Helper.ConvertNameToPath (copyFrom), ThemeInfos [copyFrom].isOld);
				File.Copy (path, destination);
			}

			return false;
		}

		/// <summary>
		/// Delete the theme
		/// </summary>
		/// <param name="name">Theme to be deleted</param>
		/// <param name="askD">Ask to delete</param>
		/// <param name="afterAsk">Do action after asked</param>
		/// <param name="afterDelete">Do action after deleted</param>
		public static void remove (string                  name, Func <string, string, bool> askD, Func <string, object> afterAsk = null,
		                           Action <string, object> afterDelete = null)
		{
			Helper.CreateThemeDirectory ();
			string sft = Helper.ConvertNameToPath (name);
			if (!ThemeInfos[name].isDefault)
			{
				if (File.Exists (pathToFile (sft, ThemeInfos [name].isOld)))
				{
					if (askD (Translate ("messages.delete.full", name), Translate ("messages.delete.short")))
					{
						object bn = null;
						if (afterAsk != null) bn = afterAsk (sft);

						Settings.saveData ();
						File.Delete (pathToFile (sft, ThemeInfos [name].isOld));
						schemes.Remove (sft);
						ThemeInfos.Remove (sft);
						if (afterDelete != null) afterDelete (sft, bn);
					}
				} else
				{
					if (CLI_Actions.showError != null)
						CLI_Actions.showError (Translate ("messages.theme.file.notfound.full"),
						                       Translate ("messages.theme.file.notfound.short"));
				}
			} else
			{
				if (CLI_Actions.showError != null)
					CLI_Actions.showError (Translate ("messages.theme.default.full"), Translate ("messages.theme.default.short"));
			}
		}

		/// <summary>
		/// Save current theme (currentFile string)
		/// </summary>
		/// <param name="img2">Background image</param>
		/// <param name="img3">Sticker</param>
		public static void save (Image img2 = null, Image img3 = null, bool wantToKeep = false)
		{
			Helper.CreateThemeDirectory ();
			Console.WriteLine ("{0}, {1}", nameToLoad, isDefault ());
			if (!isDefault ())
				saveList (currentTheme, img2, img3, wantToKeep);
		}

		/// <summary>
		/// Export current theme to pascal directory
		/// </summary>
		/// <param name="img2">Background image</param>
		/// <param name="img3">Sticker</param>
		/// <param name="setTheme">After theme has been set. You can use it to apply changes</param>
		/// <param name="startSettingTheme">When start to export. You can use it to release old images</param>
		public static void export (Image img2, Image img3, Action setTheme = null, Action startSettingTheme = null, bool wantToKeep = false)
		{
			AskToSaveInExport (img2, img3, wantToKeep);

			if (Settings.pascalPath.Length < 6 && Helper.mode != ProductMode.Plugin)
			{
				CLI_Actions.setPath (Translate ("messages.path.select.inexport.full"), Translate ("messages.path.select.inexport.short"));
			}

			if (Settings.pascalPath.Length > 6 || Helper.mode == ProductMode.Plugin)
			{
				if (startSettingTheme != null)
					startSettingTheme ();
				var files = Directory.GetFiles (Path.Combine (Settings.pascalPath, "Highlighting"), "*.xshd");
				var path = Path.Combine (Settings.pascalPath, "Highlighting", $"{pathToLoad}.xshd");
				if (files != null && files.Length > 0)
				{
					string [] unknownThemes = IdentifySyntaxHighlightings (files);
					// Console.WriteLine ("UNKNOWN: " + unknownThemes.Length);
					if (unknownThemes.Length == 0)
					{
						DeleteFiles (files);
					} else
					{
						var result = 2;
						if (Helper.mode != ProductMode.Plugin && Helper.mode != ProductMode.CLI)
						{
							if (Settings.askChoice)
							{
								result = CLI_Actions.AskChoice ();
							} else
							{
								switch (Settings.actionChoice)
								{
									case 0 :
									{
										result = 0;
									}
										break;
									case 1 :
									{
										result = 1;
									}
										break;
								}
							}
						} else
						{
							result = 0;
						}

						if (result != 2)
						{
							if (result == 1) CopyFiles (unknownThemes);
							DeleteFiles (unknownThemes);
						}
					}

					files = Directory.GetFiles (Path.Combine (Settings.pascalPath, "Highlighting"), "*.png");
					DeleteFiles (files);
				}

				if (currentTheme.isDefault && ThemeInfos[currentTheme.Name].location == ThemeLocation.Memory)
				{
					CopyFromMemory (currentTheme.path, currentTheme.Name, path, true);
				} else
				{
					ExportTheme (path);
				}

				PrepareToExport (path);

				if (Helper.mode != ProductMode.Plugin)
					if (CLI_Actions.showSuccess != null)
						CLI_Actions.showSuccess (Translate ("messages.export.success"), Translate ("messages.buttons.done"));

				Helper.currentTheme = currentTheme.Name;
				if (setTheme != null)
					setTheme ();
			} else
			{
				if (CLI_Actions.showError != null)
					CLI_Actions.showError (Translate ("messages.export.error.full"), Translate ("messages.export.error.short"));
			}
		}

		/// <summary>
		/// Export just .xshd file without images. It can be used for preview a theme
		/// </summary>
		/// <param name="setTheme">After theme has been set. You can use it to apply changes</param>
		public static void preview (SyntaxType syntax, bool needToDelete, Action setTheme = null)
		{
			var path = Path.Combine (Settings.pascalPath, "Highlighting", $"{pathToLoad}.xshd");
			if (needToDelete)
			{
				var files = Directory.GetFiles (Path.Combine (Settings.pascalPath, "Highlighting"), "*.xshd");
				if (files != null && files.Length > 0)
				{
					DeleteFiles (files);
				}
			}

			if (syntax != SyntaxType.NULL)
			{
				string dir = Path.GetDirectoryName (path);
				MergeSyntax (dir, syntax);
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
		public static void import (string path, Func <string, string, bool> exist)
		{
			MainParser.Parse (path, null, true, true, CLI_Actions.showError, exist);
		}

		/// <summary>
		/// Rename the theme
		/// </summary>
		/// <param name="from">From</param>
		/// <param name="to">To</param>
		public static void rename (string from, string to)
		{
			if (to.Length < 2)
			{
				if (CLI_Actions.showError != null) CLI_Actions.showError (Translate ("messages.name.short.full"), Translate ("messages.name.short.short"));
			}

			if (!ThemeInfos [from].isDefault)
			{
				string fromP = Helper.ConvertNameToPath (from);
				string toP = Helper.ConvertNameToPath (to);
				
				string fromPath = pathToFile (toP, ThemeInfos [from].isOld);
				string toPath = pathToFile (toP, ThemeInfos [from].isOld);
				
				if (!File.Exists (fromP))
				{
					if (CLI_Actions.showError != null)
						CLI_Actions.showError (Translate ("messages.theme.notexist.full"), Translate ("messages.theme.notexist.short"));
				} else {
					if (!File.Exists (toPath))
					{
						if (!DefaultThemes.isDefault (to))
						{
							File.Move (fromPath, toPath);
							WriteName (toPath, to);

							ThemeInfos.RenameKey (from, to);

							if (CLI_Actions.onRename != null) CLI_Actions.onRename (from, to);
						} else
						{
							if (CLI_Actions.showError != null)
								CLI_Actions.showError (Translate ("messages.name.default.full"), Translate ("messages.name.default.short"));
						}
					} else
					{
						if (CLI_Actions.showError != null) CLI_Actions.showError (Translate ("messages.name.exist.full"), Translate ("messages.name.exist.short"));
					}
				}
			} else
			{
				if (CLI_Actions.showError != null)
					CLI_Actions.showError (Translate ("messages.theme.default.full"), Translate ("messages.theme.default.short"));
			}
		}


		/// <summary>
		/// Restore to saved (default) state 
		/// </summary>
		/// <param name="wantClean">Do you want to clean garbage?</param>
		/// <param name="onSelect">Action, after populating list</param>
		public static void restore (bool wantClean = true, Action onSelect = null)
		{
			isEdited = false;
			if (currentTheme.Fields != null)
				currentTheme.Fields.Clear ();
			names.Clear ();
			Console.WriteLine ("\nCLI.NameToLoad: {0}", nameToLoad);
			populateList (onSelect);
			if (wantClean)
			{
				GC.Collect ();
				GC.WaitForPendingFinalizers ();
			}
		}

		/// <summary>
		/// Populate list with values. For example Default Background color, Default Foreground color and etc. 
		/// </summary>
		/// <param name="onSelect">Action, after populating list</param>
		public static void populateList (Action onSelect = null)
		{
			if (ThemeInfos[nameToLoad].isOld)
			{
				OldThemeFormat.LoadThemeToCLI ();
			} else
			{
				NewThemeFormat.LoadThemeToCLI ();
			}

			if (onSelect != null)
				onSelect ();
		}

		public static Theme GetTheme (string name)
		{
			if (ThemeInfos.ContainsKey (name))
			{
				Theme theme;
				if (ThemeInfos [name].isOld)
				{
					theme = OldThemeFormat.populateList (name, false);
				} else
				{
					theme = NewThemeFormat.populateList (name, false);
				}

				return theme;
			} else
			{
				return null;
			}
		}

		private static Stream GetStreamFromMemory (string file, string name)
		{
			IThemeHeader header = DefaultThemes.headers [name];
			Assembly a = header.Location;
			if (file.Contains (":"))
			{
				file = Helper.ConvertNameToPath (file);
			}

			string ext = Helper.GetExtension (ThemeInfos [name].isOld);
			Stream stream = a.GetManifestResourceStream ($"{header.ResourceHeader}.{file}" + ext);
			return stream;
		}

		/// <summary>
		/// Copy theme from memory. It's used to copy default themes.
		/// </summary>
		/// <param name="file">Copy from (theme name as path)</param>
		/// <param name="name">Copy from (theme name)</param>
		/// <param name="path">Copy to path</param>
		/// <param name="extract">Do you want to extract background image and sticker?</param>
		public static void CopyFromMemory (string file, string name, string path, bool extract = false)
		{
			Stream stream = GetStreamFromMemory (file, name);
			string nxp = extract ? path + "A" : path;
			using (var fs = new FileStream (nxp, FileMode.Create))
			{
				stream.Seek (0, SeekOrigin.Begin);
				stream.CopyTo (fs);
			}

			if (extract)
			{
				if (Helper.IsZip (stream))
				{
					CleanDestination ();
					Tuple <bool, Image> img = Helper.GetImage (nxp);
					Tuple <bool, Image> sticker = Helper.GetSticker (nxp);

					Helper.ExtractZip (nxp, path, img.Item1, sticker.Item1, false);
					File.Delete (nxp);
					return;
				}

				File.Move (nxp, path);
			}

			stream.Dispose ();
		}


		/// <summary>
		/// Write name of the theme to the theme file (.xshd), so Yuki Theme can show it properly (symbols like ':')
		/// </summary>
		/// <param name="path">Full path to theme</param>
		/// <param name="name">New name of the theme</param>
		public static void WriteName (string path, string name)
		{
			if (IsOldTheme (path))
			{
				OldThemeFormat.WriteName (path, name);
			} else
			{
				NewThemeFormat.WriteName (path, name);
			}
		}

		public static bool SelectTheme (string name)
		{
			nameToLoad = name;
			pathToLoad = Helper.ConvertNameToPath (name);
			Console.WriteLine ("IsDefault: {0}", ThemeInfos [name].isOld);
			return true;
		}

		#endregion

		#region Path Generators

		public static string pathToFile (string pathLoad, bool old)
		{
			return Path.Combine (currentPath, "Themes", $"{pathLoad}{Helper.GetExtension (old)}");
		}
		
		public static string pathToMemory (string name)
		{
			IThemeHeader header = DefaultThemes.headers [name];
			string file = name;
			if (file.Contains (":"))
			{
				file = Helper.ConvertNameToPath (file);
			}

			return $"{header.ResourceHeader}.{file}{Helper.GetExtension (ThemeInfos[name].isOld)}";
		}

		#endregion

		public static string GetPath (string name)
		{
			string path = Helper.ConvertNameToPath (name);
			
			
			
			return path;
		}

		/// <summary>
		/// Check if the path is Pascal Directory. To check it, I check if there is <code>Highlighting</code> directory in it.
		/// </summary>
		/// <param name="path">Path</param>
		/// <returns>True if it is Pascal Directory</returns>
		public static bool isPasalDirectory (string path)
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
			if (IsNewTheme (path))
				return NewThemeFormat.GetNameOfTheme (path);
			return OldThemeFormat.GetNameOfTheme (path);
		}

		/// <summary>
		/// Verify theme token and get group name.
		/// </summary>
		/// <param name="path">Path to the theme</param>
		/// <returns>Is Token valid and group name</returns>
		public static Tuple<bool, string> VerifyToken (string path)
		{
			if (path == "" || path.Length < 5) return new Tuple <bool, string> (false, "");
			if (IsNewTheme (path))
				return NewThemeFormat.VerifyToken (path);
			return OldThemeFormat.VerifyToken (path);
		}

		/// <summary>
		/// Save theme to file
		/// </summary>
		/// <param name="themeToSave">Theme to be saved</param>
		/// <param name="img2">Background image</param>
		/// <param name="img3">Sticker</param>
		/// <param name="wantToKeep">Want to keep old wallpaper and sticker if file is exist</param>
		public static void saveList (Theme themeToSave, Image img2 = null, Image img3 = null, bool wantToKeep = false)
		{
			if (Settings.saveAsOld)
				OldThemeFormat.saveList (themeToSave, img2, img3, wantToKeep);
			else
				NewThemeFormat.saveList (themeToSave, img2, img3, wantToKeep);
		}


		public static void PrepareToExport (string path)
		{
			string dir = Path.GetDirectoryName (path);
			// Console.WriteLine(currentTheme.Fields["Method"].ToString ());
			foreach (SyntaxType syntax in (SyntaxType [])Enum.GetValues (typeof (SyntaxType)))
			{
				if (syntax != SyntaxType.NULL)
					MergeSyntax (dir, syntax);
			}
		}

		private static void MergeSyntax (string dir, SyntaxType syntax)
		{
			string destination = Path.Combine (dir, $"{pathToLoad}_{syntax}.xshd");
			ExtractSyntaxTemplate (syntax, destination);

			Dictionary <string, ThemeField> localDic = ThemeField.GetThemeFieldsWithRealNames (syntax, CLI.currentTheme);
			Console.WriteLine (syntax.ToString ());
			MergeFiles (destination, localDic);
		}

		public static void ExtractSyntaxTemplate (SyntaxType syntax, string destination)
		{
			Assembly a = GetCore ();
			Stream stream = a.GetManifestResourceStream ($"Yuki_Theme.Core.Resources.Syntax_Templates.{syntax.ToString ()}.xshd");
			using (var fs = new FileStream (destination, FileMode.Create))
			{
				stream.Seek (0, SeekOrigin.Begin);
				stream.CopyTo (fs);
			}
		}

		public static void MergeFiles (string path, Dictionary <string, ThemeField> local)
		{
			var doc = new XmlDocument ();
			doc.Load (path);

			MergeFiles (local, currentTheme, ref doc);

			doc.Save (path);
		}

		public static void LoadFieldsAndMergeFiles (string content, string path, Theme theme)
		{
			var doc = new XmlDocument ();
			doc.LoadXml (content);

			Dictionary <string, ThemeField> localFields = ThemeField.GetThemeFieldsWithRealNames (SyntaxType.Pascal, CLI.currentTheme);

			MergeFiles (localFields, theme, ref doc);

			OldThemeFormat.SaveXML (null, null, true, theme.IsZip (), ref doc, path);
		}

		private static void MergeFiles (Dictionary <string, ThemeField> fields, Theme themeToMerge, ref XmlDocument doc)
		{
			OldThemeFormat.MergeThemeFieldsWithFile (fields, doc);

			OldThemeFormat.MergeCommentsWithFile (themeToMerge, doc);
		}

		/// <summary>
		/// Clean destination before export. Delete background image and sticker 
		/// </summary>
		private static void CleanDestination ()
		{
			var fil = Directory.GetFiles (Path.Combine (Settings.pascalPath, "Highlighting"), "*.png");
			foreach (string s in fil)
			{
				File.Delete (s);
			}
		}

		/// <summary>
		/// Copy files to <code>Themes</code> directory
		/// </summary>
		/// <param name="files">Files to be copied</param>
		private static void CopyFiles (string [] files)
		{
			foreach (var file in files)
			{
				var fs = Path.Combine (currentPath, "Themes", Path.GetFileNameWithoutExtension (file) + Helper.FILE_EXTENSTION_OLD);
				if (!File.Exists (fs))
					File.Copy (file, fs);
			}
		}

		/// <summary>
		/// Export theme to the path (pascal directory)
		/// </summary>
		/// <param name="path">Path</param>
		private static void ExportTheme (string path)
		{
			string source = currentTheme.fullPath;
			bool iszip = Helper.IsZip (source);
			if (!iszip)
			{
				// File.Copy (source, path, true);
				return;
			}

			CleanDestination ();

			Tuple <bool, Image> img = Helper.GetImage (source);
			Tuple <bool, Image> sticker = Helper.GetSticker (source);

			Helper.ExtractZip (source, path, img.Item1, sticker.Item1, false);
		}
		
		
		
		

		/// <summary>
		/// Delete files if exist
		/// </summary>
		/// <param name="files"></param>
		private static void DeleteFiles (string [] files)
		{
			foreach (var file in files)
			{
				if (File.Exists (file))
					File.Delete (file);
			}
		}

		/// <summary>
		/// Get this assembly
		/// </summary>
		/// <returns></returns>
		public static Assembly GetCore ()
		{
			// return GetAssemblyByName ("Yuki Theme.Core");
			return Assembly.GetExecutingAssembly ();
		}

		private static Assembly GetAssemblyByName (string name)
		{
			return AppDomain.CurrentDomain.GetAssemblies ()
			                .SingleOrDefault (assembly => assembly.GetName ().Name == name);
		}

		/// <summary>
		/// Is current theme in default themes
		/// </summary>
		/// <returns></returns>
		public static bool isDefault ()
		{
			return DefaultThemes.isDefault (nameToLoad);
		}


		/// <summary>
		/// Re Generate Theme to convert from old theme to new, or vice versa.
		/// </summary>
		/// <param name="path">Destination path</param>
		/// <param name="oldPath">Old path, that was copied from. It also can be path to the memory</param>
		/// <param name="name">New Name</param>
		/// <param name="oldName">Old Name</param>
		/// <returns>If it's done, it will return true</returns>
		public static bool ReGenerateTheme (string path, string oldPath, string name, string oldName, bool forceRegenerate)
		{
			if ((IsOldTheme (path) && IsOldTheme (oldPath)) || (!IsOldTheme (path) && !IsOldTheme (oldPath)))
				return false;
			if (!IsOldTheme (oldPath) && (Settings.saveAsOld || forceRegenerate))
				ReGenerateFromNew (path, oldPath, name, oldName);
			else
				ReGenerateFromOld (path, oldPath, name, oldName);
			return true;
		}

		private static void ReGenerateFromOld (string path, string oldPath, string name, string oldName)
		{
			Theme theme = new Theme ();
			theme.Fields = new Dictionary <string, ThemeField> ();
			var doc = new XmlDocument ();
			try
			{
				bool isDef = DefaultThemes.isDefault (oldName);
				OldThemeFormat.loadThemeToPopulate (ref doc, isDef ? oldName : oldPath, false, isDef, ref theme, Helper.FILE_EXTENSTION_OLD,
				                                    false, true);
			} catch
			{
				return;
			}

			List <string> namesList = new List <string> ();

			OldThemeFormat.PopulateDictionaryFromDoc (doc, ref theme, ref namesList);

			string methdoName = Settings.settingMode == SettingMode.Light ? "Method" : "MarkPrevious";
			if (!theme.Fields.ContainsKey (methdoName))
			{
				string keywordName = Settings.settingMode == SettingMode.Light ? "Keyword" : "Keywords";
				theme.Fields.Add (methdoName, new ThemeField () { Foreground = theme.Fields [keywordName].Foreground });
			}

			Dictionary <string, string> additionalInfo = OldThemeFormat.GetAdditionalInfoFromDoc (doc);
			string al = additionalInfo ["align"];
			string op = additionalInfo ["opacity"];
			string sop = additionalInfo ["stickerOpacity"];
			theme.Name = name;
			theme.Group = "";
			theme.Version = Convert.ToInt32 (Settings.current_version);
			theme.WallpaperOpacity = int.Parse (op);
			theme.StickerOpacity = int.Parse (sop);
			theme.WallpaperAlign = int.Parse (al);
			string json = JsonConvert.SerializeObject (theme, Formatting.Indented);
			bool iszip = false;

			if (DefaultThemes.isDefault (oldName))
			{
				Stream stream = GetStreamFromMemory (oldName, oldName);
				iszip = Helper.IsZip (stream);
				stream.Dispose ();
			} else
			{
				iszip = Helper.IsZip (oldPath);
			}

			if (!iszip)
				File.WriteAllText (path, json);
			else
			{
				Helper.UpdateZip (path, json, null, true, null, true, "", false);
			}
		}

		private static void ReGenerateFromNew (string path, string oldPath, string name, string oldName)
		{
			bool isDefaultTheme = DefaultThemes.isDefault (oldName);
			string json = NewThemeFormat.loadThemeToPopulate (isDefaultTheme ? oldName : oldPath, false, isDefaultTheme, Helper.FILE_EXTENSTION_NEW);
			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			theme.Name = name;
			theme.Group = "";
			var a = GetCore ();
			string str = "";
			
			using (StreamReader reader = new StreamReader (a.GetManifestResourceStream (Helper.PASCALTEMPLATE)))
			{
				str = reader.ReadToEnd ();
			}

			// Console.WriteLine ("REGENERATION...");
			LoadFieldsAndMergeFiles (str, path, theme);
		}

		public static bool IsOldTheme (string path)
		{
			return path.ToLower ().EndsWith (Helper.FILE_EXTENSTION_OLD);
		}

		private static bool IsNewTheme (string path)
		{
			return path.ToLower ().EndsWith (Helper.FILE_EXTENSTION_NEW);
		}

		private static void LoadSchemesByExtension (string extension)
		{
			foreach (string file in Directory.GetFiles (Path.Combine (currentPath, "Themes"), "*" + extension, SearchOption.TopDirectoryOnly))
			{
				string pts = Path.GetFileNameWithoutExtension (file);

				Tuple <bool, string> tokenVerification = VerifyToken (file);
				bool isValidToken = tokenVerification.Item1;
				string groupName = tokenVerification.Item2;
				
				if (!DefaultThemes.isDefault (pts) || isValidToken)
				{
					bool has = false;
					if (pts.Contains ("__"))
					{
						string stee = Path.Combine (currentPath, "Themes", $"{pts}{extension}");
						pts = GetNameOfTheme (stee);
						
						if (DefaultThemes.isDefault (pts))
						{
							has = true;
						}
					}
					if (!isValidToken)
					{
						if (!has && pts.Length > 0)
						{
							schemes.Add (pts);
							AddThemeInfo (pts, false, IsOldTheme (file), ThemeLocation.File);
						}
					} else
					{
						// Console.WriteLine ("Token valid");
						ForcelyAddToDefaults (pts, file, groupName);
					}
				}
			}
		}

		private static void ForcelyAddToDefaults (string name, string file, string group)
		{
			if (!schemes.Contains (name))
			{
				int index = GetIndexForInsert (name, group);
				if (index != -1)
					schemes.Insert (index, name);
				else
					schemes.Add (name);
				Console.WriteLine("{1} -> {0} <-> {2}", index, name, group);
				DefaultThemes.categories.Add (name, group);
				if (!DefaultThemes.categoriesList.Contains (group))
				{
					DefaultThemes.categoriesList.Add (group);
				}
			}
			
			ThemeInfo info = new ThemeInfo (true, IsOldTheme (file), ThemeLocation.File);
			if (ThemeInfos.ContainsKey (name))
				ThemeInfos [name] = info;
			else
				ThemeInfos.Add (name, info);
			
			if (!DefaultThemes.names.Contains (name))
				DefaultThemes.names.Add (name);

			
			
			// Console.WriteLine ("{0}\n\n", ThemeInfos [name]);

		}

		private static void AskToSaveInExport (Image img2, Image img3, bool wantToKeep)
		{
			if (!isDefault () && Helper.mode != ProductMode.Plugin && isEdited)
			{
				if (CLI_Actions.SaveInExport ("messages.theme.save.full", "messages.theme.save.short"))
					save (img2, img3, wantToKeep);
			} else if (!isDefault ())
			{
				save (img2, img3, wantToKeep);
			}
		}

		public static void AddThemeInfo (string name, bool isDefault, bool isOld, ThemeLocation location)
		{
			ThemeInfos.Add (name, new ThemeInfo (isDefault, isOld, location));
		}

		private static int GetIndexForInsert (string forName, string group)
		{
			int result = -1;

			string target = DefaultThemes.categories.Where (item => item.Value == group).Select (item => item.Key).FirstOrDefault();

			if (target is { Length: > 1 })
			{
				result = FindIndex (forName, target);
			}

			return result;
		}

		private static int FindIndex (string forName, string target)
		{
			int res = -1;

			if (DefaultThemes.headers.ContainsKey (target))
			{
				IThemeHeader header = DefaultThemes.headers [target];
				List <string> themes = header.ThemeNames.Keys.ToList ();
				themes.Add (forName);
				themes.Sort ();
				res = themes.FindIndex (tg => tg == forName);
				if (res > 1)
					res = res - 1;
				else
				{
					if (themes.Count >= 2)
						res = 1;
				}

				if (res != -1)
				{
					string targ2 = themes [res];
					res = schemes.FindIndex (item => item == targ2);

					if (res + 1 < schemes.Count)
					{
						res += 1;
					}
				}
			}

			return res;
		}

		private static string [] IdentifySyntaxHighlightings (string [] files)
		{
			List <string> unknowThemes = new List <string> ();
			foreach (string file in files)
			{
				string name = OldThemeFormat.GetNameOfTheme (file);
				if (name.Length > 0)
				{
					if (!schemes.Contains (name))
					{
						unknowThemes.Add (file);
					}
				}
			}

			return unknowThemes.ToArray ();
		}

		internal static string Translate (string key)
		{
			return Settings.translation.GetText (key);
		}

		internal static string Translate (string key, string p1)
		{
			return Settings.translation.GetText (key).Replace ("{0}", p1);
		}

		internal static string Translate (string key, string p1, string p2)
		{
			return Settings.translation.GetText (key).Replace ("{0}", p1).Replace ("{1}", p2);
		}
	}
}