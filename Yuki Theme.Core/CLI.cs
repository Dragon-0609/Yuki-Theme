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
		public static string pathToFile      => Path.Combine (currentPath, "Themes", $"{pathToLoad}{Helper.FILE_EXTENSTION_OLD}");
		public static string pathToFileNew   => Path.Combine (currentPath, "Themes", $"{pathToLoad}{Helper.FILE_EXTENSTION_NEW}");
		public static string pathToMemory    => $"Yuki_Theme.Core.Themes.{pathToLoad}{Helper.FILE_EXTENSTION_OLD}";
		public static string pathToMemoryNew => $"Yuki_Theme.Core.Themes.{pathToLoad}{Helper.FILE_EXTENSTION_NEW}";

		#region Public Fields

		public static List <string>             names          = new List <string> ();
		public static List <string>             schemes        = new List <string> ();
		public static Dictionary <string, bool> isDefaultTheme = new Dictionary <string, bool> ();
		public static Dictionary <string, bool> oldThemeList   = new Dictionary <string, bool> ();
		public static Theme                     currentTheme   = ThemeFunctions.LoadDefault ();

		#region ThemeLoading

		public static string nameToLoad;
		public static string pathToLoad;
		public static string extensionToLoad;

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

			isDefaultTheme.Clear ();
			oldThemeList.Clear ();
			DefaultThemes.Clear ();
			DefaultThemes.addDefaultThemes ();
			DefaultThemes.addExternalThemes ();
			schemes.AddRange (DefaultThemes.names);
			DefaultThemes.addOldNewThemeDifference (ref oldThemeList);
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
						File.Copy (sm, pathToFile);
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
		public static int add (string copyFrom, string name)
		{
			if (name.Length < 3)
			{
				if (CLI_Actions.showError != null) CLI_Actions.showError (Translate ("messages.name.short.full"), Translate ("messages.name.short.short"));
				return 0;
			}

			string sto = Helper.ConvertNameToPath (name);
			string patsh = Path.Combine (currentPath,
			                             $"Themes/{sto}" + (oldThemeList [copyFrom]
				                             ? Helper.FILE_EXTENSTION_OLD
				                             : Helper.FILE_EXTENSTION_NEW));
			Helper.CreateThemeDirectory ();

			bool exist = false;

			if (patsh.Exist ())
			{
				if (!CLI_Actions.SaveInExport (Translate ("messages.file.exist.override.full"), Translate ("messages.file.exist.override.short")))
				{
					if (CLI_Actions.showError != null) CLI_Actions.showError (Translate ("messages.name.exist.full"), Translate ("messages.name.exist.short"));
					return 0;
				}

				exist = true;
				File.Delete (patsh);
			}

			if (!DefaultThemes.isDefault (name))
			{
				string pth = "";
				if (CopyTheme (copyFrom, copyFrom, patsh, out pth, true)) return 0;
				bool done = ReGenerateTheme (patsh, pth, name, copyFrom, false);
				if (!done)
					WriteName (patsh, name);
				if (!exist)
				{
					schemes.Add (name);
					isDefaultTheme.Add (name, false);
					oldThemeList.Add (name, oldThemeList [copyFrom]);
				}

				if (Helper.mode == ProductMode.CLI)
					if (CLI_Actions.showSuccess != null)
						CLI_Actions.showSuccess (Translate ("messages.theme.duplicate"), Translate ("messages.buttons.done"));

				return exist ? 2 : 1;
			} else
			{
				if (CLI_Actions.showError != null)
					CLI_Actions.showError (Translate ("messages.name.default.full"), Translate ("messages.name.default.short"));

				return 0;
			}
		}

		public static bool CopyTheme (string copyFrom, string themeName, string destination, out string path, bool check)
		{
			if (check && isDefaultTheme [copyFrom])
			{
				path = GetThemeFormatFromMemory (themeName);
				if (path == null)
				{
					if (CLI_Actions.showError != null)
						CLI_Actions.showError (Translate ("messages.theme.memory.notfound.full"), Translate ("messages.theme.memory.notfound.short"));

					return true;
				}

				CopyFromMemory (themeName, themeName, destination);
			} else
			{
				path = Path.Combine (currentPath, $"Themes/{themeName}{Helper.FILE_EXTENSTION_OLD}");
				if (!path.Exist ())
					path = Path.Combine (currentPath, $"Themes/{themeName}{Helper.FILE_EXTENSTION_NEW}");
				File.Copy (path, destination);
			}

			return false;
		}

		/// <summary>
		/// Delete the theme
		/// </summary>
		/// <param name="st">Theme to be deleted</param>
		/// <param name="askD">Ask to delete</param>
		/// <param name="afterAsk">Do action after asked</param>
		/// <param name="afterDelete">Do action after deleted</param>
		public static void remove (string                  st, Func <string, string, bool> askD, Func <string, object> afterAsk = null,
		                           Action <string, object> afterDelete = null)
		{
			Helper.CreateThemeDirectory ();
			string sft = Helper.ConvertNameToPath (st);
			if (DefaultThemes.getCategory (st).ToLower () == "custom")
			{
				string ext = oldThemeList [st] ? Helper.FILE_EXTENSTION_OLD : Helper.FILE_EXTENSTION_NEW;
				if (Path.Combine (currentPath, "Themes", $"{sft}{ext}").Exist ())
				{
					if (askD ($"Do you really want to delete '{st}'?", "Delete"))
					{
						object bn = null;
						if (afterAsk != null) bn = afterAsk (sft);

						Settings.SaveData ();
						File.Delete (Path.Combine (currentPath, $"Themes/{sft}{ext}"));
						schemes.Remove (sft);
						if (afterDelete != null) afterDelete (sft, bn);
					}
				} else
				{
					if (CLI_Actions.showError != null)
						CLI_Actions.showError (Translate ("messages.theme.file.notfound.full"), Translate ("messages.theme.file.notfound.short"));
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
			// Console.WriteLine ("{0}, {1}", nameToLoad, isDefault ());
			if (!isDefault ())
				saveList (img2, img3, wantToKeep);
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

				if (currentTheme.isDefault)
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
			MainParser.Parse (path, /*null, */true, true, CLI_Actions.showError, exist);
		}

		/// <summary>
		/// Rename the theme
		/// </summary>
		/// <param name="from">From</param>
		/// <param name="to">To</param>
		/// <returns>0 -> error. 1 -> success</returns>
		public static int rename (string from, string to)
		{
			if (to.Length < 3)
			{
				if (CLI_Actions.showError != null) CLI_Actions.showError (Translate ("messages.name.short.full"), Translate ("messages.name.short.short"));
				return 0;
			}

			if (!isDefaultTheme [from])
			{
				string frm = Helper.ConvertNameToPath (from);
				string tt = Helper.ConvertNameToPath (to);
				string tp = null;
				string frmpath = null;
				bool canOperate = false;
				if (oldThemeList [from])
				{
					tp = Path.Combine (currentPath, "Themes", $"{tt}{Helper.FILE_EXTENSTION_OLD}");
					frmpath = Path.Combine (currentPath, "Themes", $"{frm}{Helper.FILE_EXTENSTION_OLD}");
					canOperate = true;
				} else
				{
					tp = Path.Combine (currentPath, "Themes", $"{tt}{Helper.FILE_EXTENSTION_NEW}");
					frmpath = Path.Combine (currentPath, "Themes", $"{frm}{Helper.FILE_EXTENSTION_NEW}");
					canOperate = true;
				}

				if (!Path.Combine (currentPath, "Themes", $"{frm}{Helper.FILE_EXTENSTION_OLD}").Exist () &&
				    !Path.Combine (currentPath, "Themes", $"{frm}{Helper.FILE_EXTENSTION_NEW}").Exist ())
				{
					canOperate = false;
					if (CLI_Actions.showError != null)
						CLI_Actions.showError (Translate ("messages.theme.notexist.full"), Translate ("messages.theme.notexist.short"));
				}

				if (canOperate)
				{
					if (!tp.Exist ())
					{
						if (!DefaultThemes.isDefault (to))
						{
							File.Move (frmpath, tp);
							WriteName (tp, to);

							AddThemeToLists (to, false, oldThemeList [from]);
							isDefaultTheme.Remove (from);
							oldThemeList.Remove (from);

							if (CLI_Actions.onRename != null) CLI_Actions.onRename (from, to);
							return 1;
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
			return 0;
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
			if (string.Equals (extensionToLoad, Helper.FILE_EXTENSTION_OLD, StringComparison.OrdinalIgnoreCase))
			{
				OldThemeFormat.populateList ();
			} else
			{
				NewThemeFormat.populateList ();
			}

			if (onSelect != null)
				onSelect ();
		}

		private static Stream GetStreamFromMemory (string file, string name)
		{
			IThemeHeader header = DefaultThemes.headers [name];
			Assembly a = header.Location;
			if (file.Contains (":"))
			{
				file = Helper.ConvertNameToPath (file);
			}

			string ext = oldThemeList [name] ? Helper.FILE_EXTENSTION_OLD : Helper.FILE_EXTENSTION_NEW;
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
			// Console.WriteLine (isDefaultTheme [name]);
			ThemeFormat extension = Helper.GetThemeFormat (isDefaultTheme [name], pathToLoad, name);
			if (extension == ThemeFormat.Null)
			{
				CLI_Actions.showError (Translate ("messages.file.notexist.full"), Translate ("messages.file.notexist.short"));
				return false;
			} else
			{
				extensionToLoad = extension == ThemeFormat.Old ? Helper.FILE_EXTENSTION_OLD : Helper.FILE_EXTENSTION_NEW;
			}

			return true;
		}

		#endregion

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
			// Console.WriteLine (path);
			if (IsNewTheme (path))
				return NewThemeFormat.GetNameOfTheme (path);
			return OldThemeFormat.GetNameOfTheme (path);
		}

		/// <summary>
		/// Save current theme
		/// </summary>
		/// <param name="img2">Background image</param>
		/// <param name="img3">Sticker</param>
		private static void saveList (Image img2 = null, Image img3 = null, bool wantToKeep = false)
		{
			if (!isDefault ())
			{
				// Console.WriteLine ("AS OLD: " + Settings.saveAsOld);
				if (Settings.saveAsOld)
					OldThemeFormat.saveList (img2, img3, wantToKeep);
				else
					NewThemeFormat.saveList (img2, img3, wantToKeep);
			}
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
			string npath = Path.Combine (dir, $"{pathToLoad}_{syntax}.xshd");
			Assembly a = GetCore ();
			Stream stream = a.GetManifestResourceStream ($"{Helper.TEMPLATENAMESPACE}{syntax.ToString ()}.xshd");
			using (FileStream fs = new FileStream (npath, FileMode.Create))
			{
				stream.Seek (0, SeekOrigin.Begin);
				stream.CopyTo (fs);
			}

			Dictionary <string, ThemeField> localDic = ConvertToRealNames (syntax);
			// Console.WriteLine (syntax.ToString ());
			MergeFiles (npath, localDic);
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

		public static void MergeFiles (Dictionary <string, ThemeField> fields, Theme themeToMerge, ref XmlDocument doc)
		{
			OldThemeFormat.MergeThemeFieldsWithFile (fields, doc);

			OldThemeFormat.MergeCommentsWithFile (themeToMerge, doc);
		}

		
		public static Dictionary <string, ThemeField> ConvertToRealNames (SyntaxType syntax)
		{
			Dictionary <string, ThemeField> localDic = ThemeField.GetThemeFieldsWithRealNames (syntax, CLI.currentTheme);
			return localDic;
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
				if (!fs.Exist ())
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
				if (file.Exist ())
					File.Delete (file);
			}
		}

		/// <summary>
		/// Get this assembly
		/// </summary>
		/// <returns></returns>
		public static Assembly GetCore ()
		{
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


		private static string GetThemeFormatFromMemory (string file)
		{
			IThemeHeader header = DefaultThemes.headers [file];
			Assembly a = header.Location;
			if (file.Contains (":"))
			{
				file = Helper.ConvertNameToPath (file);
			}

			string format = $"{header.ResourceHeader}.{file}" + Helper.FILE_EXTENSTION_OLD;
			Stream stream = a.GetManifestResourceStream (format);
			if (stream == null)
			{
				stream = a.GetManifestResourceStream ($"{header.ResourceHeader}.{file}" + Helper.FILE_EXTENSTION_NEW);
				format = stream != null ? format : null;
			}

			stream?.Dispose ();
			return format;
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
				OldThemeFormat.loadThemeToPopulate (ref doc, oldPath, false, DefaultThemes.isDefault (oldName), ref theme, oldName,
				                                    Helper.FILE_EXTENSTION_OLD, false);
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
			string json = NewThemeFormat.loadThemeToPopulate (oldPath, false, DefaultThemes.isDefault (oldName), oldName,
			                                                  Helper.FILE_EXTENSTION_NEW);
			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			theme.Name = name;
			theme.Group = "";
			var a = GetCore ();
			string str = "";
			;
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
			foreach (var file in Directory.GetFiles (Path.Combine (currentPath, "Themes/"), "*" + extension))
			{
				var pts = Path.GetFileNameWithoutExtension (file);
				if (!DefaultThemes.isDefault (pts))
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

					if (!has && pts.Length > 0)
					{
						schemes.Add (pts);
						isDefaultTheme.Add (pts, false);
						oldThemeList.Add (pts, IsOldTheme (file));
					}
				}
			}
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

		public static void AddThemeToLists (string name, bool isDefault, bool isOld)
		{
			isDefaultTheme.Add (name, isDefault);
			oldThemeList.Add (name, isOld);
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
	}
}