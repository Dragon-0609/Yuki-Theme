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
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Parsers;
using Yuki_Theme.Core.Themes;
using Formatting = Newtonsoft.Json.Formatting;

namespace Yuki_Theme.Core
{
	public static class CLI
	{
		public static string getPath    => Path.Combine (currentPath, "Themes", $"{currentFile}{Helper.FILE_EXTENSTION_OLD}");
		public static string getPathNew => Path.Combine (currentPath, "Themes", $"{currentFile}{Helper.FILE_EXTENSTION_NEW}");
		public static string getPathDynamic => Path.Combine (currentPath, "Themes", $"{currentFile}{currentFileExtension}");
		public static string gp         => $"Yuki_Theme.Core.Themes.{currentFile}{Helper.FILE_EXTENSTION_OLD}";
		public static string gpNew      => $"Yuki_Theme.Core.Themes.{currentFile}{Helper.FILE_EXTENSTION_NEW}";

		#region Public Fields

		public static List <string>   names    = new List <string> ();
		public static List <string>   schemes  = new List <string> ();
		public static DatabaseManager database = new DatabaseManager ();

		public static int          actionChoice;
		public static bool         askChoice;
		public static bool         update;
		public static string       pascalPath = "empty";
		public static bool         bgImage;
		public static bool         swSticker;
		public static bool         swStatusbar;
		public static bool         swLogo;
		public static bool         Editor;
		public static bool         Beta;
		public static bool         Logged;
		public static bool         positioning;
		public static SettingMode  settingMode;
		public static string       currentFile          = "N|L";
		public static string       currentoFile         = "N|L";
		public static string       currentFileExtension = "N|L";
		public static string       selectedItem         = "empty";
		public static string       imagePath            = "";
		public static string       currentPath          = Path.GetDirectoryName (Assembly.GetEntryAssembly ().Location);
		public static Alignment    align                = Alignment.Left;
		public static RelativeUnit unit                 = RelativeUnit.Pixel;
		public static bool         showGrids;
		public static bool         useCustomSticker;
		public static string       customSticker = "";
		public static bool         license;
		public static bool         googleAnalytics;
		public static bool         dontTrack;
		public static bool         autoFitByWidth;
		public static bool         askToSave;
		public static int          opacity   = 10;
		public static int          sopacity  = 100;
		public static bool         isEdited  = false;
		public static bool         saveAsOld = false;
		public static string       groupName = "";

		public static Dictionary <string, Dictionary <string, string>> localAttributes =
			new Dictionary <string, Dictionary <string, string>> ();

		public static Func <string, string, bool> SaveInExport;
		public static Action <string, string>     setPath;
		public static Action <string, string>     showSuccess;
		public static Action <string, string>     showError;
		public static Func <int>                  AskChoice;
		public static Action <string>             hasProblem           = null;
		public static Action                      onBGIMAGEChange      = null;
		public static Action                      onSTICKERChange      = null;
		public static Action                      onSTATUSChange       = null;
		public static Action <Image>              ifHasImage           = null;
		public static Action                      ifDoesntHave         = null;
		public static Action <Image>              ifHasSticker         = null;
		public static Action                      ifDoesntHaveSticker  = null;
		public static Action <Image>              ifHasImage2          = null;
		public static Action                      ifDoesntHave2        = null;
		public static Action <Image>              ifHasSticker2        = null;
		public static Action                      ifDoesntHaveSticker2 = null;
		public static Action <string, string>     onRename;

		#endregion


		#region Main Commands

		/// <summary>
		/// Load Themes from default themes and from 'Themes' directory
		/// </summary>
		/// <param name="ifZero">If there isn't any theme, ask to set it</param>
		public static void load_schemes (Func <string> ifZero = null)
		{
			schemes.Clear ();

			schemes.AddRange (DefaultThemes.def);
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
						currentFile = Path.GetFileNameWithoutExtension (sm);
						currentFileExtension = "." + Path.GetExtension (sm);
						currentoFile = currentFile;
						File.Copy (sm, getPath);
						schemes.Add (currentFile);
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
			string syt = Helper.ConvertNameToPath (copyFrom);
			string sto = Helper.ConvertNameToPath (name);
			string patsh = Path.Combine (currentPath,
			                             $"Themes/{sto}" + (saveAsOld ? Helper.FILE_EXTENSTION_OLD : Helper.FILE_EXTENSTION_NEW));
			Helper.CreateThemeDirectory ();

			bool exist = false;

			if (File.Exists (patsh))
			{
				if (!SaveInExport ("The file is exist. Do you want to override?", "Override?"))
				{
					if (showError != null)
						showError ("The name is exist! Choose another name", "Name Exist");
					return true;
				}

				exist = true;
				File.Delete (patsh);
			}

			if (!DefaultThemes.isDefault (name))
			{
				string pth = "";
				if (DefaultThemes.isDefault (copyFrom))
				{
					pth = GetThemeFormatFromMemory (syt);
					if (pth == null)
					{
						if (showError != null)
							showError ("Couldn't find theme in memory! Try to choose another theme",
							           "Can't find theme in memory");

						return true;
					}
					
					if (saveAsOld || IsOldTheme (pth))
						CopyFromMemory (syt, patsh);
				} else
				{
					pth = Path.Combine (currentPath, $"Themes/{syt}{Helper.FILE_EXTENSTION_OLD}");
					if (!File.Exists (pth))
						pth = Path.Combine (currentPath, $"Themes/{syt}{Helper.FILE_EXTENSTION_NEW}");
					// Here I check if it's new theme. If it's new theme and we want to save as old, then don't copy the file. Because it'll be overrided later
					if (saveAsOld || IsOldTheme (pth))  
						File.Copy (pth, patsh);
				}

				bool done = ReGenerateTheme (patsh, pth, name, copyFrom);
				if (!done)
					WriteName (patsh, name);
				if (Helper.mode == ProductMode.CLI)
					if (showSuccess != null)
						showSuccess ("The theme has been duplicated!", "Done");

				return exist;
			} else
			{
				if (showError != null)
					showError ("You musn't choose default theme's name. Choose another name!",
					           "Default theme's name");

				return true;
			}
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
				if (File.Exists (Path.Combine (currentPath, "Themes", $"{sft}.yukitheme")))
				{
					if (askD ($"Do you really want to delete '{st}'?", "Delete"))
					{
						object bn = null;
						if (afterAsk != null) bn = afterAsk (sft);

						saveData ();
						File.Delete (Path.Combine (currentPath, $"Themes/{sft}.yukitheme"));
						schemes.Remove (sft);
						if (afterDelete != null) afterDelete (sft, bn);
					}
				} else
				{
					if (showError != null)
						showError ("Theme isn't exist! Choose another name", "Theme isn't exist");
				}
			} else
			{
				if (showError != null)
					showError ("You musn't choose default theme. Choose custom theme!", "Default theme");
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
			if (!isDefault () && Helper.mode != ProductMode.Plugin && isEdited)
			{
				if (SaveInExport ("Do you want to save current scheme?", "Save"))
					save (img2, img3, wantToKeep);
			} else if (!isDefault ())
			{
				save (img2, img3, wantToKeep);
			}

			if (pascalPath.Length < 6 && Helper.mode != ProductMode.Plugin)
			{
				setPath ("Please, set path to the PascalABC.NET Direcory.",
				         "Path to the PascalABC.NET Direcory");
			}

			if (pascalPath.Length > 6 || Helper.mode == ProductMode.Plugin)
			{
				if (startSettingTheme != null)
					startSettingTheme ();
				var files = Directory.GetFiles (Path.Combine (pascalPath, "Highlighting"), "*.xshd");
				var path = Path.Combine (pascalPath, "Highlighting", $"{currentFile}.xshd");
				if (files != null && files.Length > 0)
				{
					if (files [0] == path)
					{
						File.Delete (files [0]);
					}
					// Console.WriteLine ($"FILES: {files.Length}, OR: {files [0]} | MS: {path}");

					var result = 2;
					if (Helper.mode != ProductMode.Plugin && Helper.mode != ProductMode.CLI)
					{
						if (askChoice)
						{
							result = AskChoice ();
						} else
						{
							switch (actionChoice)
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
						if (result == 1) CopyFiles (files);

						DeleteFiles (files);
						files = Directory.GetFiles (Path.Combine (pascalPath, "Highlighting"), "*.png");
						DeleteFiles (files);
					}
				}

				Tuple <bool, bool> images = null;

				if (isDefault ())
				{
					images = CopyFromMemory (currentFile, path, true);
				} else
				{
					images = ExportTheme (path);
				}

				PrepareToExport (path, images.Item1, images.Item2);

				if (Helper.mode != ProductMode.Plugin)
					if (showSuccess != null)
						showSuccess (
							"Your scheme has been exported to the Pascal Directory. Restart PascalABC.NET to activate this scheme",
							"Done");

				Helper.currentTheme = currentoFile;
				if (setTheme != null)
					setTheme ();
			} else
			{
				if (showError != null)
					showError ("Export failed, because you didn't set path to the PascalABC.NET directory!",
					           "Export failed");
			}
		}

		/// <summary>
		/// Import theme
		/// </summary>
		/// <param name="path">Theme from</param>
		public static void import (string path, Func <string, string, bool> exist)
		{
			MainParser.Parse (path, null, true, true, showError, exist);
		}

		/// <summary>
		/// Rename the theme
		/// </summary>
		/// <param name="from">From</param>
		/// <param name="to">To</param>
		public static void rename (string from, string to)
		{
			string frm = Helper.ConvertNameToPath (from);
			if (File.Exists (Path.Combine (currentPath, "Themes", $"{frm}.yukitheme")))
			{
				if (!DefaultThemes.isDefault (from))
				{
					string tt = Helper.ConvertNameToPath (to);
					if (!File.Exists (Path.Combine (currentPath, "Themes", $"{tt}.yukitheme")))
					{
						if (!DefaultThemes.isDefault (to))
						{
							string tp = Path.Combine (currentPath, "Themes", $"{tt}.yukitheme");
							File.Move (Path.Combine (currentPath, "Themes", $"{frm}.yukitheme"),
							           tp);
							WriteName (tp, to);

							if (onRename != null)
								onRename (from, to);
						} else
						{
							if (showError != null)
								showError ("You musn't choose default theme's name. Choose another name!",
								           "Default theme's name");
						}
					} else
					{
						if (showError != null)
							showError ("The name is exist! Choose another name", "Name Exist");
					}
				} else
				{
					if (showError != null)
						showError ("You musn't choose default theme. Choose custom theme!", "Default theme");
				}
			} else
			{
				if (showError != null)
					showError ("The name isn't exist! Choose another name", "Name isn't exist");
			}
		}

		/// <summary>
		/// Set align 
		/// </summary>
		/// <param name="algn"></param>
		public static void palign (Alignment algn)
		{
			if (align != algn)
			{
				align = algn;
				convertAlign ();
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
			localAttributes.Clear ();
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
			if (string.Equals (currentFileExtension, Helper.FILE_EXTENSTION_OLD, StringComparison.OrdinalIgnoreCase))
			{
				populateListOld ();
			} else
			{
				populateListNew ();
			}

			if (onSelect != null)
				onSelect ();
		}

		/// <summary>
		/// Populate list with values. For example Default Background color, Default Foreground color and etc. 
		/// </summary>
		private static void populateListOld ()
		{
			var doc = new XmlDocument ();
			loadThemeToPopulateOld (ref doc, gp, getPath, false);

			PopulateDictionaryFromDocOld (doc, ref localAttributes, ref names);
			Dictionary <string, string> additionalInfo = GetAdditionalInfoFromDoc (doc);
			string al = additionalInfo ["align"];
			string op = additionalInfo ["opacity"];
			string sop = additionalInfo ["stickerOpacity"];

			localAttributes.Add ("Wallpaper",
			                     new Dictionary <string, string> () {{"align", al}, {"opacity", op}});

			localAttributes.Add ("Sticker",
			                     new Dictionary <string, string> () {{"opacity", sop}});
			/*string all = "";
			foreach (KeyValuePair <string, Dictionary <string, string>> pair in localAttributes)
			{
				all += pair.Key + "\n";
			}
			*/

			// System.Windows.Forms.Clipboard.SetText (all);

			align = (Alignment) (int.Parse (localAttributes ["Wallpaper"] ["align"]));
			opacity = int.Parse (localAttributes ["Wallpaper"] ["opacity"]);
			sopacity = int.Parse (localAttributes ["Sticker"] ["opacity"]);
		}

		private static void populateListNew ()
		{
			string json = loadThemeToPopulateNew (gpNew, getPathNew, true);

			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			PopulateDictionaryFromThemeNew (theme, ref localAttributes, ref names);
			
			align = (Alignment) theme.WallpaperAlign;
			opacity = theme.WallpaperOpacity;
			sopacity = theme.StickerOpacity;
		}

		private static void PopulateDictionaryFromThemeNew (Theme theme, ref Dictionary <string, Dictionary <string, string>> attributes, ref List<string> namesExtended)
		{
			foreach (KeyValuePair <string, ThemeField> field in theme.Fields)
			{
				if (!namesExtended.Contains (field.Key))
				{
					Dictionary <string, string> attrs = field.Value.ConvertToDictionary ();
					namesExtended.Add (field.Key);
					attributes.Add (field.Key, attrs);

					if (field.Key.Equals ("selection", StringComparison.OrdinalIgnoreCase) &&
					    !namesExtended.Contains ("Wallpaper"))
					{
						namesExtended.Remove ("Selection");
						namesExtended.Add ("Wallpaper");
						namesExtended.Add ("Selection");
					}

					if (field.Key.Equals ("selection", StringComparison.OrdinalIgnoreCase) &&
					    !namesExtended.Contains ("Sticker"))
					{
						namesExtended.Remove ("Selection");
						namesExtended.Add ("Sticker");
						namesExtended.Add ("Selection");
					}
				}
			}
			attributes.Add ("Wallpaper",
			                new Dictionary <string, string> ()
				                {{"align", theme.WallpaperAlign.ToString ()}, {"opacity", theme.WallpaperOpacity.ToString ()}});

			attributes.Add ("Sticker",
			                new Dictionary <string, string> () {{"opacity", theme.StickerOpacity.ToString ()}});
		}
		
		private static Tuple <bool, bool> loadThemeToPopulateOld (ref XmlDocument doc, string pathForMemory, string pathForFile,
		                                                          bool            needToReturn)
		{
			bool hasImage = false;
			bool hasSticker = false;
			if (isDefault ())
			{
				var a = GetCore ();


				Tuple <bool, string> content = Helper.GetThemeFromMemory (pathForMemory, a);
				if (content.Item1)
				{
					doc.LoadXml (content.Item2);
					Tuple <bool, Image> iag = Helper.GetImageFromMemory (pathForMemory, a);
					if (!needToReturn)
					{
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (ifHasImage != null)
							{
								ifHasImage ((Image) iag.Item2);
							}

							if (ifHasImage2 != null)
							{
								ifHasImage2 ((Image) iag.Item2);
							}
						} else
						{
							if (ifDoesntHave != null)
								ifDoesntHave ();
							if (ifDoesntHave2 != null)
								ifDoesntHave2 ();
						}
					} else
					{
						hasImage = iag.Item1;
					}

					iag = null;
					iag = Helper.GetStickerFromMemory (pathForMemory, a);
					if (!needToReturn)
					{
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (ifHasSticker != null)
							{
								ifHasSticker ((Image) iag.Item2);
							}

							if (ifHasSticker2 != null)
							{
								ifHasSticker2 ((Image) iag.Item2);
							}
						} else
						{
							if (ifDoesntHaveSticker != null)
								ifDoesntHaveSticker ();
							if (ifDoesntHaveSticker2 != null)
								ifDoesntHaveSticker2 ();
						}
					} else
					{
						hasSticker = iag.Item1;
					}
				} else
				{
					if (!needToReturn)
					{
						if (ifDoesntHave != null)
							ifDoesntHave ();

						if (ifDoesntHaveSticker != null)
							ifDoesntHaveSticker ();

						if (ifDoesntHave2 != null)
							ifDoesntHave2 ();

						if (ifDoesntHaveSticker2 != null)
							ifDoesntHaveSticker2 ();
					}

					doc.Load (a.GetManifestResourceStream (pathForMemory));
				}
			} else
			{
				imagePath = "";
				Tuple <bool, string> content = Helper.GetTheme (pathForFile);
				if (content.Item1)
				{
					doc.LoadXml (content.Item2);
					Tuple <bool, Image> iag = Helper.GetImage (pathForFile);
					if (!needToReturn)
					{
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (ifHasImage != null)
							{
								ifHasImage ((Image) iag.Item2);
							}

							if (ifHasImage2 != null)
							{
								ifHasImage2 ((Image) iag.Item2);
							}
						} else
						{
							if (ifDoesntHave != null)
								ifDoesntHave ();

							if (ifDoesntHave2 != null)
								ifDoesntHave2 ();
						}
					} else
					{
						hasImage = iag.Item1;
					}

					iag = Helper.GetSticker (pathForFile);
					if (!needToReturn)
					{
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (ifHasSticker != null)
							{
								ifHasSticker ((Image) iag.Item2);
							}
						} else
						{
							if (ifDoesntHaveSticker != null)
								ifDoesntHaveSticker ();

							if (ifDoesntHaveSticker2 != null)
								ifDoesntHaveSticker2 ();
						}
					} else
					{
						hasSticker = iag.Item1;
					}
				} else
				{
					if (!needToReturn)
					{
						if (ifDoesntHave != null)
							ifDoesntHave ();
						if (ifDoesntHaveSticker != null)
							ifDoesntHaveSticker ();

						if (ifDoesntHave2 != null)
							ifDoesntHave2 ();
						if (ifDoesntHaveSticker2 != null)
							ifDoesntHaveSticker2 ();
					}

					try
					{
						doc.Load (pathForFile);
					} catch (System.Xml.XmlException)
					{
						if (hasProblem != null)
							hasProblem (
								"There's problem in your theme file. Sorry, but I can't open it. The default scheme will be selected");

						return null;
					}
				}
			}

			return new Tuple <bool, bool> (hasImage, hasSticker);
		}

		private static string loadThemeToPopulateNew (string pathToMemory, string pathToFile, bool needToGetImages)
		{
			string json = "";
			if (isDefault ())
			{
				var a = GetCore ();
				
				Tuple <bool, string> content = Helper.GetThemeFromMemory (pathToMemory, a);
				if (content.Item1)
				{
					json = content.Item2;
					if (needToGetImages)
					{
						Tuple <bool, Image> iag = Helper.GetImageFromMemory (pathToMemory, a);

						if (iag.Item1)
						{
							// img = iag.Item2;
							if (ifHasImage != null)
							{
								ifHasImage ((Image) iag.Item2);
							}

							if (ifHasImage2 != null)
							{
								ifHasImage2 ((Image) iag.Item2);
							}
						} else
						{
							if (ifDoesntHave != null)
								ifDoesntHave ();
							if (ifDoesntHave2 != null)
								ifDoesntHave2 ();
						}

						iag = null;
						iag = Helper.GetStickerFromMemory (pathToMemory, a);
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (ifHasSticker != null)
							{
								ifHasSticker ((Image) iag.Item2);
							}

							if (ifHasSticker2 != null)
							{
								ifHasSticker2 ((Image) iag.Item2);
							}
						} else
						{
							if (ifDoesntHaveSticker != null)
								ifDoesntHaveSticker ();
							if (ifDoesntHaveSticker2 != null)
								ifDoesntHaveSticker2 ();
						}
					}
				} else
				{
					if (needToGetImages)
					{
						if (ifDoesntHave != null)
							ifDoesntHave ();

						if (ifDoesntHaveSticker != null)
							ifDoesntHaveSticker ();

						if (ifDoesntHave2 != null)
							ifDoesntHave2 ();

						if (ifDoesntHaveSticker2 != null)
							ifDoesntHaveSticker2 ();
					}
					StreamReader reader = new StreamReader (a.GetManifestResourceStream (pathToMemory));
					json = reader.ReadToEnd ();
				}
			} else
			{
				imagePath = "";
				Tuple <bool, string> content = Helper.GetTheme (pathToFile);
				if (content.Item1)
				{
					json = content.Item2;
					if (needToGetImages)
					{
						Tuple <bool, Image> iag = Helper.GetImage (pathToFile);
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (ifHasImage != null)
							{
								ifHasImage ((Image) iag.Item2);
							}

							if (ifHasImage2 != null)
							{
								ifHasImage2 ((Image) iag.Item2);
							}
						} else
						{
							if (ifDoesntHave != null)
								ifDoesntHave ();

							if (ifDoesntHave2 != null)
								ifDoesntHave2 ();
						}

						iag = Helper.GetSticker (pathToFile);
						if (iag.Item1)
						{
							// img = iag.Item2;
							if (ifHasSticker != null)
							{
								ifHasSticker ((Image) iag.Item2);
							}
						} else
						{
							if (ifDoesntHaveSticker != null)
								ifDoesntHaveSticker ();

							if (ifDoesntHaveSticker2 != null)
								ifDoesntHaveSticker2 ();
						}
					}
				} else
				{
					if (needToGetImages)
					{
						if (ifDoesntHave != null)
							ifDoesntHave ();
						if (ifDoesntHaveSticker != null)
							ifDoesntHaveSticker ();

						if (ifDoesntHave2 != null)
							ifDoesntHave2 ();
						if (ifDoesntHaveSticker2 != null)
							ifDoesntHaveSticker2 ();
					}
					json = File.ReadAllText (pathToFile);
				}
			}

			return json;
		}

		private static Stream GetStreamFromMemory (string file)
		{
			var a = GetCore ();
			if (file.Contains (":"))
			{
				file = file.Replace (": ", "__").Replace (":", "");
			}

			Stream stream = a.GetManifestResourceStream ($"Yuki_Theme.Core.Themes.{file}" + Helper.FILE_EXTENSTION_OLD);
			if (stream == null)
				stream = a.GetManifestResourceStream ($"Yuki_Theme.Core.Themes.{file}" + Helper.FILE_EXTENSTION_NEW);
			return stream;
		}

		/// <summary>
		/// Copy theme from memory. It's used to copy default themes.
		/// </summary>
		/// <param name="file">Copy from (theme name)</param>
		/// <param name="path">Copy to path</param>
		/// <param name="extract">Do you want to extract background image and sticker?</param>
		public static Tuple <bool, bool> CopyFromMemory (string file, string path, bool extract = false)
		{
			Stream stream = GetStreamFromMemory (file);
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
					return new Tuple <bool, bool> (img.Item1, sticker.Item1);
				} else
				{
					File.Move (nxp, path);
				}
			}

			stream.Dispose ();
			return new Tuple <bool, bool> (false, false);
		}

		/// <summary>
		/// Save current settings
		/// </summary>
		public static void saveData ()
		{
			var dict = new Dictionary <int, string> ();
			dict.Add (SettingsForm.PASCALPATH, pascalPath);
			dict.Add (SettingsForm.ACTIVE, selectedItem);
			dict.Add (SettingsForm.ASKCHOICE, askChoice.ToString ());
			dict.Add (SettingsForm.CHOICEINDEX, actionChoice.ToString ());
			dict.Add (SettingsForm.SETTINGMODE, ((int)settingMode).ToString ());
			dict.Add (SettingsForm.AUTOUPDATE, update.ToString ());
			dict.Add (SettingsForm.BGIMAGE, bgImage.ToString ());
			dict.Add (SettingsForm.STICKER, swSticker.ToString ());
			dict.Add (SettingsForm.STATUSBAR, swStatusbar.ToString ());
			dict.Add (SettingsForm.LOGO, swLogo.ToString ());
			dict.Add (SettingsForm.EDITOR, Editor.ToString ());
			dict.Add (SettingsForm.BETA, Beta.ToString ());
			dict.Add (SettingsForm.ALLOWPOSITIONING, positioning.ToString ());
			dict.Add (SettingsForm.SHOWGRIDS, showGrids.ToString ());
			dict.Add (SettingsForm.STICKERPOSITIONUNIT, ((int) unit).ToString ());
			dict.Add (SettingsForm.USECUSTOMSTICKER, useCustomSticker.ToString ());
			dict.Add (SettingsForm.CUSTOMSTICKER, customSticker.ToString ());
			dict.Add (SettingsForm.LICENSE, license.ToString ());
			dict.Add (SettingsForm.GOOGLEANALYTICS, googleAnalytics.ToString ());
			dict.Add (SettingsForm.DONTTRACK, dontTrack.ToString ());
			dict.Add (SettingsForm.AUTOFITWIDTH, autoFitByWidth.ToString ());
			dict.Add (SettingsForm.ASKTOSAVE, askToSave.ToString ());
			dict.Add (SettingsForm.SAVEASOLD, saveAsOld.ToString ());
			database.UpdateData (dict);
			if (onBGIMAGEChange != null) onBGIMAGEChange ();
			if (onSTICKERChange != null) onSTICKERChange ();
			if (onSTATUSChange != null) onSTATUSChange ();
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
				WriteNameOld (path, name);
			} else
			{
				WriteNameNew (path, name);
			}
		}

		private static void WriteNameOld (string path, string name)
		{
			var doc = new XmlDocument ();
			bool iszip = false;
			Tuple <bool, string> content = Helper.GetTheme (path);
			if (content.Item1)
			{
				doc.LoadXml (content.Item2);
				iszip = true;
			} else
			{
				doc.Load (path);
			}

			XmlNode node = doc.SelectSingleNode ("/SyntaxDefinition");

			XmlNodeList comms = node.SelectNodes ("//comment()");
			if (comms.Count >= 3)
			{
				bool hasName = false;

				string nl = "name:" + name;
				foreach (XmlComment comm in comms)
				{
					if (comm.Value.StartsWith ("name"))
					{
						comm.Value = nl;
						hasName = true;
					}
				}

				if (!hasName)
				{
					node.AppendChild (doc.CreateComment (nl));
				}
			} else
			{
				node.AppendChild (doc.CreateComment ("name:" + currentoFile));
			}

			if (!iszip)
				doc.Save (path);
			else
			{
				string txml = doc.OuterXml;

				Helper.UpdateZip (path, txml, null, true, null, true);
			}
		}

		private static void WriteNameNew (string path, string name)
		{
			string json = "";

			Tuple <bool, string> content = Helper.GetTheme (path);
			bool iszip = content.Item1;

			if (content.Item1)
				json = content.Item2;
			else
				json = File.ReadAllText (path);
			Console.WriteLine (json);
			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			theme.Name = name;
			json = JsonConvert.SerializeObject (theme, Formatting.Indented);


			if (!iszip)
				File.WriteAllText (getPathNew, json);
			else
			{
				Helper.UpdateZip (path, json, null, true, null, true);
			}
		}

		public static bool SelectTheme (string name)
		{
			currentoFile = name;
			currentFile = Helper.ConvertNameToPath (currentoFile);
			if (DefaultThemes.isDefault (currentoFile))
			{
				Console.WriteLine (gp);
				Console.WriteLine (gpNew);
				Assembly assembly = GetCore ();
				if (assembly.GetManifestResourceStream (gp) != null)
					currentFileExtension = Helper.FILE_EXTENSTION_OLD;
				else if (assembly.GetManifestResourceStream (gpNew) != null)
					currentFileExtension = Helper.FILE_EXTENSTION_NEW;
				else
				{
					showError ("The file isn't exist", "File isn't exist");
					return false;
				}
			} else
			{
				Console.WriteLine (getPath);
				Console.WriteLine (getPathNew);
				if (File.Exists (getPath))
					currentFileExtension = Helper.FILE_EXTENSTION_OLD;
				else if (File.Exists (getPathNew))
					currentFileExtension = Helper.FILE_EXTENSTION_NEW;
				else
				{
					showError ("The file isn't exist", "File isn't exist");
					return false;
				}
			}

			return true;
		}

		#endregion

		#region XML

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		private static void PopulateByXMLNodeParent (XmlNode node, ref Dictionary <string, Dictionary <string, string>> attributes,
		                                             ref List <string> namesExtra)
		{
			foreach (XmlNode xne in node.ChildNodes) PopulateByXMLNode (xne, ref attributes, ref namesExtra);
		}

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		private static void PopulateByXMLNode (XmlNode           node, ref Dictionary <string, Dictionary <string, string>> attributes,
		                                       ref List <string> namesExtra)
		{
			foreach (XmlNode xn in node.ChildNodes) PopulateByXMLNodeSingular (xn, ref attributes, ref namesExtra);
		}

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		private static void PopulateByXMLNodeSingular (XmlNode node, ref Dictionary <string, Dictionary <string, string>> attributes,
		                                               ref List <string> namesExtra)
		{
			// Console.WriteLine("TEST");
			var attrs = new Dictionary <string, string> ();
			if (node.Attributes != null && !string.Equals (node.Name, "Delimiters", StringComparison.Ordinal))
			{
				var nm = node.Name;
				if (node.Name == "Span" || node.Name == "KeyWords") nm = node.Attributes ["name"].Value;

				foreach (XmlAttribute att in node.Attributes)
				{
					if (att.Name == "color" || att.Name == "bgcolor")
					{
						// Console.WriteLine($"{nm}: {att.Name}");
						if (node.Name == "SelectedFoldLine")
						{
							if (att.Name == "color")
								attrs.Add (att.Name, att.Value);
						} else
						{
							attrs.Add (att.Name, att.Value);
						}
					}

					if (Highlighter.isInNames (nm, true))
					{
						var dsfbold = "null";
						var dsfitalic = "null";
						if (!attrs.ContainsKey ("bold")) attrs.Add ("bold", "false");

						if (att.Name == "bold")
						{
							attrs ["bold"] = att.Value;
							dsfbold = att.Value;
						}

						if (!attrs.ContainsKey ("italic")) attrs.Add ("italic", "false");

						if (att.Name == "italic")
						{
							attrs ["italic"] = att.Value;
							dsfitalic = att.Value;
						}

						/*Console.WriteLine (
							$"InNames: {nm}|{att.Name}, bold: {attrs ["bold"]}|{dsfbold}, italic: {attrs ["italic"]}|{dsfitalic}");*/
					}
				}
				string shadowName = settingMode == SettingMode.Light ? ShadowNames.GetShadowName (nm, SyntaxType.Pascal) : nm;
				if (!namesExtra.Contains (shadowName))
				{
					if (settingMode == SettingMode.Advanced)
					{
						namesExtra.Add (shadowName);
						attributes.Add (shadowName, attrs);
					} else if (!attributes.ContainsKey (shadowName))
					{
						// Console.WriteLine ( $"InList: {nm}|{attributes.ContainsKey (nm)}");		
						attributes.Add (shadowName, attrs);
						if (!Populater.isInList (shadowName, namesExtra)) namesExtra.Add (shadowName);
					}

					if (shadowName.Equals ("selection", StringComparison.OrdinalIgnoreCase) &&
					    !namesExtra.Contains ("Wallpaper"))
					{
						namesExtra.Remove ("Selection");
						namesExtra.Add ("Wallpaper");
						namesExtra.Add ("Selection");
					}

					if (shadowName.Equals ("selection", StringComparison.OrdinalIgnoreCase) &&
					    !namesExtra.Contains ("Sticker"))
					{
						namesExtra.Remove ("Selection");
						namesExtra.Add ("Sticker");
						namesExtra.Add ("Selection");
					}
				}
			}
		}

		/// <summary>
		/// Remove unnecessary fields if the setting mode is Light. Else skip.
		/// </summary>
		private static void CleanUnnecessaryFields (ref List <string> namesExtra)
		{
			if (settingMode == SettingMode.Light)
			{
				string [] nms = new string[namesExtra.Count];
				namesExtra.CopyTo (nms);
				foreach (string name in nms)
				{
					if (Populater.isInList (name, namesExtra)) namesExtra.Remove (name);
				}
			}
		}

		#endregion

		/// <summary>
		/// Check if the path is Pascal Directory. To check it, I check if there is <code>Highlighting</code> directory in it.
		/// </summary>
		/// <param name="path">Path</param>
		/// <returns>True if it is Pascal Directory</returns>
		private static bool isPasalDirectory (string path)
		{
			return Directory.Exists (System.IO.Path.Combine (path, "Highlighting"));
		}

		/// <summary>
		/// Get settings
		/// </summary>
		public static void connectAndGet ()
		{
			var data = database.ReadData ();
			pascalPath = data [SettingsForm.PASCALPATH] == "empty" ? null : data [SettingsForm.PASCALPATH];
			if (Helper.mode == ProductMode.Plugin)
			{
				pascalPath = currentPath;
			}

			if (pascalPath == null)
			{
				string defpas = "";
				if (Environment.Is64BitOperatingSystem)
				{
					defpas = Environment.GetFolderPath (Environment.SpecialFolder.ProgramFilesX86) + "PascalABC.NET";
					if (isPasalDirectory (defpas))
					{
						pascalPath = defpas;
					} else
					{
						defpas = Environment.GetFolderPath (Environment.SpecialFolder.ProgramFilesX86) +
						         "PascalABC.NET";
						if (isPasalDirectory (defpas))
						{
							pascalPath = defpas;
						}
					}
				} else
				{
					defpas = Environment.GetFolderPath (Environment.SpecialFolder.ProgramFilesX86) + "PascalABC.NET";
					if (isPasalDirectory (defpas))
					{
						pascalPath = defpas;
					}
				}
			}

			if (pascalPath == null) pascalPath = "";

			askChoice = bool.Parse (data [SettingsForm.ASKCHOICE]);
			update = bool.Parse (data [SettingsForm.AUTOUPDATE]);
			bgImage = bool.Parse (data [SettingsForm.BGIMAGE]);
			swSticker = bool.Parse (data [SettingsForm.STICKER]);
			swStatusbar = bool.Parse (data [SettingsForm.STATUSBAR]);
			swLogo = bool.Parse (data [SettingsForm.LOGO]);
			Editor = bool.Parse (data [SettingsForm.EDITOR]);
			Beta = bool.Parse (data [SettingsForm.BETA]);
			Logged = bool.Parse (data [SettingsForm.LOGIN]);
			positioning = bool.Parse (data [SettingsForm.ALLOWPOSITIONING]);
			showGrids = bool.Parse (data [SettingsForm.SHOWGRIDS]);
			useCustomSticker = bool.Parse (data [SettingsForm.USECUSTOMSTICKER]);
			customSticker = data [SettingsForm.CUSTOMSTICKER];

			license = bool.Parse (data [SettingsForm.LICENSE]);
			googleAnalytics = bool.Parse (data [SettingsForm.GOOGLEANALYTICS]);
			dontTrack = bool.Parse (data [SettingsForm.DONTTRACK]);
			autoFitByWidth = bool.Parse (data [SettingsForm.AUTOFITWIDTH]);
			askToSave = bool.Parse (data [SettingsForm.ASKTOSAVE]);
			saveAsOld = bool.Parse (data [SettingsForm.SAVEASOLD]);

			selectedItem = data [SettingsForm.ACTIVE];
			var os = 0;
			int.TryParse (data [SettingsForm.CHOICEINDEX], out os);
			actionChoice = os;
			int.TryParse (data [SettingsForm.SETTINGMODE], out os);
			settingMode = (SettingMode) os;
			int.TryParse (data [SettingsForm.STICKERPOSITIONUNIT], out os);
			unit = (RelativeUnit) os;
		}

		/// <summary>
		/// Get name of the theme.
		/// </summary>
		/// <param name="path">Path to the theme</param>
		/// <returns>Name of the theme</returns>
		public static string GetNameOfTheme (string path)
		{
			if (IsOldTheme (path))
				return GetNameOfThemeOld (path);
			else
				return GetNameOfThemeNew (path);
		}

		private static string GetNameOfThemeOld (string path)
		{
			XmlDocument docu = new XmlDocument ();

			Tuple <bool, string> content = Helper.GetTheme (path);
			if (content.Item1)
			{
				docu.LoadXml (content.Item2);
			} else
			{
				docu.Load (path);
			}

			XmlNode nod = docu.SelectSingleNode ("/SyntaxDefinition");
			XmlNodeList comms = nod.SelectNodes ("//comment()");
			string nm = "";

			foreach (XmlComment comm in comms)
			{
				if (comm.Value.StartsWith ("name"))
				{
					nm = comm.Value.Substring (5);
					break;
				}
			}

			return nm;
		}
		
		private static string GetNameOfThemeNew (string path)
		{
			string nm = "";
			string json = loadThemeToPopulateNew (path, path, false);
			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			nm = theme.Name;
			return nm;
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
				if (saveAsOld)
					saveListOld (img2, img3, wantToKeep);
				else
					saveListNew (img2, img3, wantToKeep);
			}
		}


		/// <summary>
		/// Save current theme in old format. It can be used to export to old version of Yuki Theme.
		/// </summary>
		/// <param name="img2">Background image</param>
		/// <param name="img3">Sticker</param>
		private static void saveListOld (Image img2 = null, Image img3 = null, bool wantToKeep = false)
		{
			var doc = new XmlDocument ();
			bool iszip = false;
			string themePath = File.Exists (getPath) ? getPath : File.Exists (getPathNew) ? getPathNew : null;
			Tuple <bool, string> content = Helper.GetTheme (themePath);
			if (content.Item1)
			{
				doc.LoadXml (content.Item2);
				iszip = true;
			} else
			{
				doc.Load (themePath);
			}

			#region Environment

			var node = doc.SelectSingleNode ("/SyntaxDefinition/Environment");
			bool hadSavedImage = false; // This is check for alpha version of v2.0
			foreach (XmlNode childNode in node.ChildNodes)
				if (childNode.Attributes != null &&
				    !string.Equals (childNode.Name, "Delimiters", StringComparison.Ordinal))
				{
					var nms = childNode.Name;
					if (childNode.Name == "Span" || childNode.Name == "KeyWords")
						nms = childNode.Attributes ["name"].Value;
					if (!localAttributes.ContainsKey (nms)) continue;
					if (nms == "Wallpaper")
						hadSavedImage = true;
					var attrs = localAttributes [nms];

					foreach (var att in attrs)
						childNode.Attributes [att.Key].Value = att.Value;
				}

			if (hadSavedImage)
			{
				node = doc.SelectSingleNode ("/SyntaxDefinition/Environment");
				node.RemoveChild (node.SelectSingleNode ("Wallpaper"));
			}

			#endregion

			#region Digits

			node = doc.SelectSingleNode ("/SyntaxDefinition/Digits");
			if (node.Attributes != null && !string.Equals (node.Name, "Delimiters", StringComparison.Ordinal))
			{
				var nms = node.Name;
				if (node.Name == "Span" || node.Name == "KeyWords") nms = node.Attributes ["name"].Value;
				if (localAttributes.ContainsKey (nms))
				{
					var attrs = localAttributes [nms];

					foreach (var att in attrs) node.Attributes [att.Key].Value = att.Value;
				}
			}

			#endregion

			#region Syntax

			node = doc.SelectSingleNode ("/SyntaxDefinition/RuleSets");
			foreach (XmlNode xne in node.ChildNodes)
			{
				foreach (XmlNode xn in xne.ChildNodes)
					if (xn.Attributes != null &&
					    !string.Equals (xn.Name, "Delimiters", StringComparison.Ordinal))
					{
						var nms = xn.Name;
						if (xn.Name == "Span" || xn.Name == "KeyWords")
							nms = xn.Attributes ["name"].Value;
						if (!localAttributes.ContainsKey (nms)) continue;

						var attrs = localAttributes [nms];

						foreach (var att in attrs)
							// Console.WriteLine($"2N: {xn.Attributes["name"].Value}, ATT: {att.Key},");
							xn.Attributes [att.Key].Value = att.Value;
					}
			}

			#endregion

			node = doc.SelectSingleNode ("/SyntaxDefinition");

			XmlNode nod = doc.SelectSingleNode ("/SyntaxDefinition");
			XmlNodeList comms = nod.SelectNodes ("//comment()");
			if (comms.Count >= 3)
			{
				Dictionary <string, bool> comments = new Dictionary <string, bool> ()
				{
					{"name", false}, {"align", false}, {"opacity", false}, {"sopacity", false},
					{"hasImage", false}, {"hasSticker", false}
				};

				Dictionary <string, string> commentValues = new Dictionary <string, string> ()
				{
					{"name", "name:" + currentoFile}, {"align", "align:" + ((int) align).ToString ()},
					{"opacity", "opacity:" + (opacity).ToString ()},
					{"sopacity", "sopacity:" + (sopacity).ToString ()},
					{"hasImage", "hasImage:" + (img2 != null)}, {"hasSticker", "hasSticker:" + (img3 != null)}
				};
				foreach (XmlComment comm in comms)
				{
					if (comm.Value.StartsWith ("align"))
					{
						comm.Value = commentValues ["align"];
						comments ["align"] = true;
					} else if (comm.Value.StartsWith ("opacity"))
					{
						comm.Value = commentValues ["opacity"];
						comments ["opacity"] = true;
					} else if (comm.Value.StartsWith ("sopacity"))
					{
						comm.Value = commentValues ["sopacity"];
						comments ["sopacity"] = true;
					} else if (comm.Value.StartsWith ("name"))
					{
						comm.Value = commentValues ["name"];
						comments ["name"] = true;
					} else if (comm.Value.StartsWith ("hasImage"))
					{
						comm.Value = commentValues ["hasImage"];
						comments ["hasImage"] = true;
					} else if (comm.Value.StartsWith ("hasSticker"))
					{
						comm.Value = commentValues ["hasSticker"];
						comments ["hasSticker"] = true;
					}
				}

				foreach (KeyValuePair <string, bool> comment in comments)
				{
					if (!comment.Value)
					{
						node.AppendChild (doc.CreateComment (commentValues [comment.Key]));
					}
				}
			} else
			{
				node.AppendChild (doc.CreateComment ("name:" + currentoFile));
				node.AppendChild (doc.CreateComment ("align:" + ((int) align).ToString ()));
				node.AppendChild (doc.CreateComment ("opacity:" + (opacity).ToString ()));
				node.AppendChild (doc.CreateComment ("sopacity:" + (sopacity).ToString ()));
				node.AppendChild (doc.CreateComment ("hasImage:" + (img2 != null)));
				node.AppendChild (doc.CreateComment ("hasSticker:" + (img3 != null)));
			}

			if (!iszip && img2 == null && img3 == null && !wantToKeep)
				doc.Save (themePath);
			else
			{
				string txml = doc.OuterXml;
				if (iszip)
				{
					Helper.UpdateZip (themePath, txml, img2, wantToKeep, img3, wantToKeep);
				} else
				{
					Helper.Zip (themePath, txml, img2, img3);
				}
			}
		}

		/// <summary>
		/// Save current theme in new format. It is mainly used in new versions of Yuki Theme. Smaller than version 6 won't be able to open the format
		/// </summary>
		/// <param name="img2">Background image</param>
		/// <param name="img3">Sticker</param>
		private static void saveListNew (Image img2 = null, Image img3 = null, bool wantToKeep = false)
		{
			Theme theme = PrepareToSave (img2, img3);

			string json = JsonConvert.SerializeObject (theme, Formatting.Indented);
			bool iszip = Helper.IsZip (getPathNew);


			if (!iszip && img2 == null && img3 == null && !wantToKeep)
				File.WriteAllText (getPathNew, json);
			else
			{
				if (iszip)
				{
					Helper.UpdateZip (getPathNew, json, img2, wantToKeep, img3, wantToKeep);
				} else
				{
					Helper.Zip (getPathNew, json, img2, img3);
				}
			}
		}

		public static void PrepareToExport (string path, bool hasImage, bool hasSticker)
		{
			/*foreach (KeyValuePair <string, string []> pair in ShadowNames.PascalFields)
			{
				Console.WriteLine (pair.Key);
			}*/
			string dir = Path.GetDirectoryName (path);
			foreach (SyntaxType syntax in (SyntaxType []) Enum.GetValues (typeof (SyntaxType)))
			{
				string npath = Path.Combine (dir, $"{currentFile}_{syntax}.xshd");
				var a = GetCore ();
				var stream = a.GetManifestResourceStream ($"Yuki_Theme.Core.Resources.Syntax_Templates.{syntax.ToString ()}.xshd");
				using (var fs = new FileStream (npath, FileMode.Create))
				{
					stream.Seek (0, SeekOrigin.Begin);
					stream.CopyTo (fs);
				}

				Dictionary <string, Dictionary <string, string>> localDic = new Dictionary <string, Dictionary <string, string>> ();
				List <string> shadowNames = new List <string> (); // This is necessary not to repeat fields
				foreach (KeyValuePair <string, Dictionary <string, string>> pair in localAttributes)
				{
					string shadowName = ShadowNames.GetShadowName (pair.Key, syntax, true);
					Console.WriteLine (pair.Key + " | " + shadowName);
					if (shadowName != null && !shadowNames.Contains (shadowName))
					{
						string [] realName = ShadowNames.GetRealName (shadowName, syntax);
						if (realName != null)
						{
							foreach (string st in realName)
							{
								localDic.Add (st, pair.Value);
							}
						}

						shadowNames.Add (shadowName);
					}
				}

				MergeFiles (npath, hasImage, hasSticker, localDic);
			}
		}

		public static void MergeFiles (string path, bool hasImage, bool hasSticker, Dictionary <string, Dictionary <string, string>> local)
		{
			var doc = new XmlDocument ();
			doc.Load (path);

			#region Environment

			var node = doc.SelectSingleNode ("/SyntaxDefinition/Environment");
			bool hadSavedImage = false; // This is check for alpha version of v2.0
			foreach (XmlNode childNode in node.ChildNodes)
				if (childNode.Attributes != null &&
				    !string.Equals (childNode.Name, "Delimiters", StringComparison.Ordinal))
				{
					var nms = childNode.Name;
					if (childNode.Name == "Span" || childNode.Name == "KeyWords")
						nms = childNode.Attributes ["name"].Value;
					if (!local.ContainsKey (nms)) continue;
					if (nms == "Wallpaper")
						hadSavedImage = true;
					var attrs = local [nms];

					foreach (var att in attrs)
						childNode.Attributes [att.Key].Value = att.Value;
				}

			if (hadSavedImage)
			{
				node = doc.SelectSingleNode ("/SyntaxDefinition/Environment");
				node.RemoveChild (node.SelectSingleNode ("Wallpaper"));
			}

			#endregion

			#region Digits

			node = doc.SelectSingleNode ("/SyntaxDefinition/Digits");
			if (node.Attributes != null && !string.Equals (node.Name, "Delimiters", StringComparison.Ordinal))
			{
				var nms = node.Name;
				if (node.Name == "Span" || node.Name == "KeyWords") nms = node.Attributes ["name"].Value;
				if (local.ContainsKey (nms))
				{
					var attrs = local [nms];

					foreach (var att in attrs) node.Attributes [att.Key].Value = att.Value;
				}
			}

			#endregion

			#region Syntax

			node = doc.SelectSingleNode ("/SyntaxDefinition/RuleSets");
			foreach (XmlNode xne in node.ChildNodes)
			{
				foreach (XmlNode xn in xne.ChildNodes)
					if (xn.Attributes != null &&
					    !string.Equals (xn.Name, "Delimiters", StringComparison.Ordinal))
					{
						var nms = xn.Name;
						if (xn.Name == "Span" || xn.Name == "KeyWords")
							nms = xn.Attributes ["name"].Value;
						if (!local.ContainsKey (nms)) continue;

						var attrs = local [nms];

						foreach (var att in attrs)
							// Console.WriteLine($"2N: {xn.Attributes["name"].Value}, ATT: {att.Key},");
							xn.Attributes [att.Key].Value = att.Value;
					}
			}

			#endregion

			node = doc.SelectSingleNode ("/SyntaxDefinition");

			XmlNode nod = doc.SelectSingleNode ("/SyntaxDefinition");
			XmlNodeList comms = nod.SelectNodes ("//comment()");
			if (comms.Count >= 3)
			{
				Dictionary <string, bool> comments = new Dictionary <string, bool> ()
				{
					{"name", false}, {"align", false}, {"opacity", false}, {"sopacity", false},
					{"hasImage", false}, {"hasSticker", false}
				};

				Dictionary <string, string> commentValues = new Dictionary <string, string> ()
				{
					{"name", "name:" + currentoFile}, {"align", "align:" + ((int) align).ToString ()},
					{"opacity", "opacity:" + (opacity).ToString ()},
					{"sopacity", "sopacity:" + (sopacity).ToString ()},
					{"hasImage", "hasImage:" + hasImage}, {"hasSticker", "hasSticker:" + hasSticker}
				};
				foreach (XmlComment comm in comms)
				{
					if (comm.Value.StartsWith ("align"))
					{
						comm.Value = commentValues ["align"];
						comments ["align"] = true;
					} else if (comm.Value.StartsWith ("opacity"))
					{
						comm.Value = commentValues ["opacity"];
						comments ["opacity"] = true;
					} else if (comm.Value.StartsWith ("sopacity"))
					{
						comm.Value = commentValues ["sopacity"];
						comments ["sopacity"] = true;
					} else if (comm.Value.StartsWith ("name"))
					{
						comm.Value = commentValues ["name"];
						comments ["name"] = true;
					} else if (comm.Value.StartsWith ("hasImage"))
					{
						comm.Value = commentValues ["hasImage"];
						comments ["hasImage"] = true;
					} else if (comm.Value.StartsWith ("hasSticker"))
					{
						comm.Value = commentValues ["hasSticker"];
						comments ["hasSticker"] = true;
					}
				}

				foreach (KeyValuePair <string, bool> comment in comments)
				{
					if (!comment.Value)
					{
						node.AppendChild (doc.CreateComment (commentValues [comment.Key]));
					}
				}
			} else
			{
				node.AppendChild (doc.CreateComment ("name:" + currentoFile));
				node.AppendChild (doc.CreateComment ("align:" + ((int) align).ToString ()));
				node.AppendChild (doc.CreateComment ("opacity:" + (opacity).ToString ()));
				node.AppendChild (doc.CreateComment ("sopacity:" + (sopacity).ToString ()));
				node.AppendChild (doc.CreateComment ("hasImage:" + hasImage));
				node.AppendChild (doc.CreateComment ("hasSticker:" + hasSticker));
			}

			doc.Save (path);
		}


		/// <summary>
		/// Convert align to int and add it to variables
		/// </summary>
		private static void convertAlign ()
		{
			localAttributes ["Wallpaper"] ["align"] = ((int) align).ToString ();
		}

		/// <summary>
		/// Clean destination before export. Delete background image and sticker 
		/// </summary>
		private static void CleanDestination ()
		{
			var fil = Directory.GetFiles (Path.Combine (pascalPath, "Highlighting"), "*.png");
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
				var fs = Path.Combine (currentPath, "Themes", Path.GetFileNameWithoutExtension (file) + ".yukitheme");
				if (!File.Exists (fs))
					File.Copy (file, fs);
			}
		}

		/// <summary>
		/// Export theme to the path (pascal directory)
		/// </summary>
		/// <param name="path">Path</param>
		private static Tuple <bool, bool> ExportTheme (string path)
		{
			string source = getPathDynamic;
			bool iszip = Helper.IsZip (source);
			if (!iszip)
			{
				// File.Copy (source, path, true);

				return new Tuple <bool, bool> (false, false);
			} else
			{
				CleanDestination ();

				Tuple <bool, Image> img = Helper.GetImage (source);
				Tuple <bool, Image> sticker = Helper.GetSticker (source);

				Helper.ExtractZip (source, path, img.Item1, sticker.Item1, false);

				return new Tuple <bool, bool> (img.Item1, sticker.Item1);
			}
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
			return DefaultThemes.isDefault (currentoFile);
		}

		private static Theme PrepareToSave (Image img2, Image img3)
		{
			Theme theme = new Theme ();
			theme.Name = currentoFile;
			theme.Group = groupName;
			theme.Version = Convert.ToInt32 (SettingsForm.current_version);
			theme.HasWallpaper = img2 != null;
			theme.HasSticker = img3 != null;
			theme.WallpaperOpacity = opacity;
			theme.StickerOpacity = sopacity;
			theme.WallpaperAlign = (int) align;
			theme.Fields = GetFieldsFromDictionary (localAttributes);
			return theme;
		}


		private static string GetThemeFormatFromMemory (string file)
		{
			var a = GetCore ();
			if (file.Contains (":"))
			{
				file = file.Replace (": ", "__").Replace (":", "");
			}

			string format = $"Yuki_Theme.Core.Themes.{file}" + Helper.FILE_EXTENSTION_OLD;
			Stream stream = a.GetManifestResourceStream ($"Yuki_Theme.Core.Themes.{file}" + Helper.FILE_EXTENSTION_OLD);
			if (stream == null)
			{
				stream = a.GetManifestResourceStream ($"Yuki_Theme.Core.Themes.{file}" + Helper.FILE_EXTENSTION_NEW);
				format = stream != null ? $"Yuki_Theme.Core.Themes.{file}" + Helper.FILE_EXTENSTION_NEW : null;
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
		private static bool ReGenerateTheme (string path, string oldPath, string name, string oldName)
		{
			if ((IsOldTheme (path) && IsOldTheme (oldPath)) || (!IsOldTheme (path) && !IsOldTheme (oldPath)))
				return false;
			if (!IsOldTheme (oldPath) && saveAsOld)
				ReGenerateFromNew (path, oldPath, name, oldName);
			else
				ReGenerateFromOld (path, oldPath, name, oldName);
			return true;
		}

		private static void ReGenerateFromOld (string path, string oldPath, string name, string oldName)
		{
			var doc = new XmlDocument ();
			Tuple <bool, bool> ned = loadThemeToPopulateOld (ref doc, oldPath, oldPath, true);
			Dictionary <string, Dictionary <string, string>> attributeDictionary = new Dictionary <string, Dictionary <string, string>> ();
			List <string> namesList = new List <string> ();

			PopulateDictionaryFromDocOld (doc, ref attributeDictionary, ref namesList);

			Dictionary <string, string> additionalInfo = GetAdditionalInfoFromDoc (doc);
			string al = additionalInfo ["align"];
			string op = additionalInfo ["opacity"];
			string sop = additionalInfo ["stickerOpacity"];

			Theme theme = new Theme ();
			theme.Name = name;
			theme.Group = "";
			theme.Version = Convert.ToInt32 (SettingsForm.current_version);
			theme.HasWallpaper = ned.Item1;
			theme.HasSticker = ned.Item2;
			theme.WallpaperOpacity = int.Parse (op);
			theme.StickerOpacity = int.Parse (sop);
			theme.WallpaperAlign = int.Parse (al);
			theme.Fields = GetFieldsFromDictionary (attributeDictionary);
			string json = JsonConvert.SerializeObject (theme, Formatting.Indented);
			bool iszip = false;

			if (DefaultThemes.isDefault (oldName))
			{
				Stream stream = GetStreamFromMemory (oldName);
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
				Helper.UpdateZip (path, json, null, true, null, true);
			}
		}

		private static void ReGenerateFromNew (string path, string oldPath, string name, string oldName)
		{
			string json = loadThemeToPopulateNew (oldPath, oldPath, false);
			Theme theme = JsonConvert.DeserializeObject <Theme> (json);
			theme.Name = name;
			theme.Group = "";
			Dictionary <string, Dictionary <string, string>> attribs = new Dictionary <string, Dictionary <string, string>> ();
			List <string> additionalList = new List <string> ();
			PopulateDictionaryFromThemeNew (theme, ref attribs, ref additionalList);
			var a = GetCore ();
			var stream = a.GetManifestResourceStream (Helper.PASCALTEMPLATE);
			using (var fs = new FileStream (path, FileMode.Create))
			{
				stream.Seek (0, SeekOrigin.Begin);
				stream.CopyTo (fs);
			}

			MergeFiles (path, theme.HasWallpaper, theme.HasSticker, attribs);
		}

		private static bool IsOldTheme (string path)
		{
			return path.ToLower ().EndsWith (Helper.FILE_EXTENSTION_OLD);
		}

		private static void PopulateDictionaryFromDocOld (XmlDocument doc, ref Dictionary <string, Dictionary <string, string>> attributes,
		                                                  ref List <string> namesExtra)
		{
			if (doc.SelectNodes ("/SyntaxDefinition/Environment").Count == 1)
				PopulateByXMLNode (doc.SelectNodes ("/SyntaxDefinition/Environment") [0], ref attributes, ref namesExtra);
			PopulateByXMLNodeSingular (doc.SelectNodes ("/SyntaxDefinition/Digits") [0], ref attributes, ref namesExtra);
			PopulateByXMLNodeParent (doc.SelectNodes ("/SyntaxDefinition/RuleSets") [0], ref attributes, ref namesExtra);
			CleanUnnecessaryFields (ref namesExtra);
		}

		private static Dictionary <string, string> GetAdditionalInfoFromDoc (XmlDocument doc)
		{
			XmlNode nod = doc.SelectSingleNode ("/SyntaxDefinition");
			XmlNodeList comms = nod.SelectNodes ("//comment()");
			Dictionary <string, string> dictionary = new Dictionary <string, string> ();
			dictionary.Add ("align", ((int) Alignment.Center).ToString ());
			dictionary.Add ("opacity", "15");
			dictionary.Add ("stickerOpacity", "100");
			foreach (XmlComment comm in comms)
			{
				if (comm.Value.StartsWith ("align"))
				{
					dictionary ["align"] = comm.Value.Substring (6);
				} else if (comm.Value.StartsWith ("opacity"))
				{
					dictionary ["opacity"] = comm.Value.Substring (8);
				} else if (comm.Value.StartsWith ("sopacity"))
				{
					dictionary ["stickerOpacity"] = comm.Value.Substring (9);
				}
			}

			return dictionary;
		}

		private static Dictionary <string, ThemeField> GetFieldsFromDictionary (Dictionary <string, Dictionary <string, string>> attributes)
		{
			Dictionary <string, ThemeField> fields = new Dictionary <string, ThemeField> ();
			foreach (KeyValuePair <string, Dictionary <string, string>> attribute in attributes)
			{
				if (attribute.Key != "Wallpaper" && attribute.Key != "Sticker")
				{
					ThemeField field = ThemeField.GetFieldFromDictionary (attribute.Value);
					string shadowName = ShadowNames.GetShadowName (attribute.Key, SyntaxType.Pascal); // Convert to Shadow Name
					if (shadowName == null) shadowName = attribute.Key;
					if (!fields.ContainsKey (shadowName))
						fields.Add (shadowName, field);
				}
			}

			return fields;
		}

		private static void LoadSchemesByExtension (string extension)
		{
			foreach (var file in Directory.GetFiles (Path.Combine (currentPath, "Themes/"), "*"+extension))
			{
				var pts = Path.GetFileNameWithoutExtension (file);
				if (!DefaultThemes.isDefault (pts))
				{
					if (pts.Contains ("__"))
					{
						string stee = Path.Combine (currentPath, "Themes", $"{pts}{extension}");
						string sp = GetNameOfTheme (stee);
						if (!DefaultThemes.isDefault (sp))
						{
							// Console.WriteLine(nod.Attributes ["name"].Value);
							schemes.Add (sp);
						}
					} else
					{
						schemes.Add (pts);
					}
				}
			}
		}
	}
}