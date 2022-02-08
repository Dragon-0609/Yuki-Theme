using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Yuki_Theme.Core.Database;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Parsers;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core
{
	public static class CLI
	{
		public static string getPath => Path.Combine (currentPath, "Themes", $"{currentFile}.yukitheme");
		public static string gp      => $"Yuki_Theme.Core.Themes.{currentFile}.yukitheme";

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
		public static int          settingMode;
		public static string       currentFile  = "N|L";
		public static string       currentoFile = "N|L";
		public static string       selectedItem = "empty";
		public static string       imagePath    = "";
		public static string       currentPath  = Path.GetDirectoryName (Assembly.GetEntryAssembly ().Location);
		public static Alignment    align        = Alignment.Left;
		public static RelativeUnit unit         = RelativeUnit.Pixel;
		public static bool         showGrids;
		public static bool         useCustomSticker;
		public static string       customSticker = "";
		public static bool         license;
		public static bool         googleAnalytics;
		public static bool         dontTrack;
		public static bool         autoFitByWidth;
		public static bool         askToSave;
		public static int          opacity  = 10;
		public static int          sopacity = 100;
		public static bool         isEdited = false;

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
				foreach (var file in Directory.GetFiles (Path.Combine (currentPath, "Themes/"), "*.yukitheme"))
				{
					var pts = Path.GetFileNameWithoutExtension (file);
					if (!DefaultThemes.isDefault (pts))
					{
						if (pts.Contains ("__"))
						{
							string stee = Path.Combine (currentPath, "Themes", $"{pts}.yukitheme");
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

			if (schemes.Count == 0)
			{
				if (ifZero != null)
				{
					string sm = ifZero ();
					if (sm != null)
					{
						currentFile = Path.GetFileNameWithoutExtension (sm);
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
			                             $"Themes/{sto}.yukitheme");
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
				if (DefaultThemes.isDefault (copyFrom))
					CopyFromMemory (syt, patsh);
				else
					File.Copy (Path.Combine (currentPath, $"Themes/{syt}.yukitheme"), patsh);


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

				if (isDefault ())
					CopyFromMemory (currentFile, path, true);
				else
				{
					ExportTheme (path);
				}

				if (Helper.mode != ProductMode.Plugin)
					if (showSuccess != null)
						showSuccess (
							"Your scheme has been exported to the Pascal Directory. Restart PascalABC.NET to activate this scheme",
							"Done");

				Helper.CurrentTheme = currentoFile;
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
			var doc = new XmlDocument ();
			if (isDefault ())
			{
				var a = GetCore ();
				// Console.WriteLine (currentFile);
				// doc.Load (a.GetManifestResourceStream ($"Yuki_Theme.Core.Themes.{currentFile}.yukitheme"));


				Tuple <bool, string> content = Helper.getThemeFromMemory (gp, a);
				if (content.Item1)
				{
					doc.LoadXml (content.Item2);
					Tuple <bool, Image> iag = Helper.getImageFromMemory (gp, a);
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
					iag = Helper.getStickerFromMemory (gp, a);
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
					if (ifDoesntHave != null)
						ifDoesntHave ();

					if (ifDoesntHaveSticker != null)
						ifDoesntHaveSticker ();

					if (ifDoesntHave2 != null)
						ifDoesntHave2 ();

					if (ifDoesntHaveSticker2 != null)
						ifDoesntHaveSticker2 ();
					doc.Load (a.GetManifestResourceStream (gp));
				}
			} else
			{
				imagePath = "";
				Tuple <bool, string> content = Helper.getTheme (getPath);
				if (content.Item1)
				{
					doc.LoadXml (content.Item2);
					Tuple <bool, Image> iag = Helper.getImage (getPath);
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

					iag = Helper.getSticker (getPath);
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
					if (ifDoesntHave != null)
						ifDoesntHave ();
					if (ifDoesntHaveSticker != null)
						ifDoesntHaveSticker ();

					if (ifDoesntHave2 != null)
						ifDoesntHave2 ();
					if (ifDoesntHaveSticker2 != null)
						ifDoesntHaveSticker2 ();
					try
					{
						doc.Load (getPath);
					} catch (System.Xml.XmlException)
					{
						if (hasProblem != null)
							hasProblem (
								"There's problem in your theme file. Sorry, but I can't open it. The default scheme will be selected");

						return;
					}
				}
				// doc.LoadXml ();
			}

			PopulateByXMLNode (doc.SelectNodes ("/SyntaxDefinition/Environment") [0]);
			PopulateByXMLNodeSingular (doc.SelectNodes ("/SyntaxDefinition/Digits") [0]);
			PopulateByXMLNodeParent (doc.SelectNodes ("/SyntaxDefinition/RuleSets") [0]);
			CleanUnnecessaryFields ();

			XmlNode nod = doc.SelectSingleNode ("/SyntaxDefinition");
			XmlNodeList comms = nod.SelectNodes ("//comment()");
			string al = ((int) Alignment.Center).ToString ();
			string op = "15";
			string sop = "100";

			foreach (XmlComment comm in comms)
			{
				if (comm.Value.StartsWith ("align"))
				{
					al = comm.Value.Substring (6);
				} else if (comm.Value.StartsWith ("opacity"))
				{
					op = comm.Value.Substring (8);
				} else if (comm.Value.StartsWith ("sopacity"))
				{
					sop = comm.Value.Substring (9);
				}
			}

			localAttributes.Add ("Wallpaper",
			                     new Dictionary <string, string> () {{"align", al}, {"opacity", op}});

			localAttributes.Add ("Sticker",
			                     new Dictionary <string, string> () {{"opacity", sop}});

			align = (Alignment) (int.Parse (localAttributes ["Wallpaper"] ["align"]));
			opacity = int.Parse (localAttributes ["Wallpaper"] ["opacity"]);
			sopacity = int.Parse (localAttributes ["Sticker"] ["opacity"]);
			if (onSelect != null)
				onSelect ();
		}

		/// <summary>
		/// Copy theme from memory. It's used to copy default themes.
		/// </summary>
		/// <param name="file">Copy from (theme name)</param>
		/// <param name="path">Copy to path</param>
		/// <param name="extract">Do you want to extract background image and sticker?</param>
		public static void CopyFromMemory (string file, string path, bool extract = false)
		{
			var a = GetCore ();
			if (file.Contains (":"))
			{
				file = file.Replace (": ", "__").Replace (":", "");
			}

			var stream = a.GetManifestResourceStream ($"Yuki_Theme.Core.Themes.{file}.yukitheme");
			string nxp = extract ? path + "A" : path;
			using (var fs = new FileStream (nxp, FileMode.Create))
			{
				stream.Seek (0, SeekOrigin.Begin);
				stream.CopyTo (fs);
			}

			if (extract)
			{
				if (Helper.isZip (stream))
				{
					CleanDestination ();
					Tuple <bool, Image> img = Helper.getImage (nxp);
					Tuple <bool, Image> sticker = Helper.getSticker (nxp);

					Helper.extractZip (nxp, path, img.Item1, sticker.Item1);

					File.Delete (nxp);
				} else
				{
					File.Move (nxp, path);
				}
			}
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
			dict.Add (SettingsForm.SETTINGMODE, settingMode.ToString ());
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
			var doc = new XmlDocument ();
			bool iszip = false;
			Tuple <bool, string> content = Helper.getTheme (path);
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

				Helper.updateZip (path, txml, null, true, null, true);
			}
		}

		#endregion

		#region XML

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		private static void PopulateByXMLNodeParent (XmlNode node)
		{
			foreach (XmlNode xne in node.ChildNodes) PopulateByXMLNode (xne);
		}

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		private static void PopulateByXMLNode (XmlNode node)
		{
			foreach (XmlNode xn in node.ChildNodes) PopulateByXMLNodeSingular (xn);
		}

		/// <summary>
		/// Populate list by XML. Don't worry about it. It is already used in <code>populateList</code>
		/// </summary>
		/// <param name="node"></param>
		private static void PopulateByXMLNodeSingular (XmlNode node)
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

					if (Highlighter.isInNames (nm))
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

				if (!names.Contains (nm))
				{
					if (settingMode == 1)
					{
						names.Add (nm);
						localAttributes.Add (nm, attrs);
					} else if (!localAttributes.ContainsKey (nm))
					{
						// Console.WriteLine ( $"InList: {nm}|{localAttributes.ContainsKey (nm)}");		
						localAttributes.Add (nm, attrs);
						if (!Populater.isInList (nm, names)) names.Add (nm);
					}

					if (nm.Equals ("selection", StringComparison.OrdinalIgnoreCase) &&
					    !names.Contains ("Wallpaper"))
					{
						names.Remove ("Selection");
						names.Add ("Wallpaper");
						names.Add ("Selection");
					}

					if (nm.Equals ("selection", StringComparison.OrdinalIgnoreCase) &&
					    !names.Contains ("Sticker"))
					{
						names.Remove ("Selection");
						names.Add ("Sticker");
						names.Add ("Selection");
					}
				}
			}
		}

		/// <summary>
		/// Remove unnecessary fields if the setting mode is Light. Else skip.
		/// </summary>
		private static void CleanUnnecessaryFields ()
		{
			if (settingMode == 0)
			{
				string [] nms = new string[names.Count];
				names.CopyTo (nms);
				foreach (string name in nms)
				{
					if (Populater.isInList (name, names)) names.Remove (name);
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

			selectedItem = data [SettingsForm.ACTIVE];
			var os = 0;
			int.TryParse (data [SettingsForm.CHOICEINDEX], out os);
			actionChoice = os;
			int.TryParse (data [SettingsForm.SETTINGMODE], out os);
			settingMode = os;
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
			XmlDocument docu = new XmlDocument ();

			Tuple <bool, string> content = Helper.getTheme (path);
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

		/// <summary>
		/// Save current theme
		/// </summary>
		/// <param name="img2">Background image</param>
		/// <param name="img3">Sticker</param>
		private static void saveList (Image img2 = null, Image img3 = null, bool wantToKeep = false)
		{
			if (!isDefault ())
			{
				var doc = new XmlDocument ();
				bool iszip = false;
				Tuple <bool, string> content = Helper.getTheme (getPath);
				if (content.Item1)
				{
					doc.LoadXml (content.Item2);
					iszip = true;
				} else
				{
					doc.Load (getPath);
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


				Tuple <bool, Image> img = Helper.getImage (getPath);
				Tuple <bool, Image> sticker = Helper.getSticker (getPath);

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
						{"hasImage", "hasImage:" + img.Item1}, {"hasSticker", "hasSticker:" + sticker.Item1}
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
					node.AppendChild (doc.CreateComment ("hasImage:" + img.Item1));
					node.AppendChild (doc.CreateComment ("hasSticker:" + sticker.Item1));
				}

				if (!iszip && img2 == null && img3 == null && !wantToKeep)
					doc.Save (getPath);
				else
				{
					string txml = doc.OuterXml;
					if (iszip)
					{
						Helper.updateZip (getPath, txml, img2, wantToKeep, img3, wantToKeep);
					} else
					{
						Helper.zip (getPath, txml, img2, img3);
					}
				}
			}
		}

		public static void MergeFiles (string path, bool hasImage, bool hasSticker)
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
		private static void ExportTheme (string path)
		{
			string source = getPath;
			bool iszip = Helper.isZip (source);
			if (!iszip)
			{
				File.Copy (source, path, true);
			} else
			{
				CleanDestination ();

				Tuple <bool, Image> img = Helper.getImage (source);
				Tuple <bool, Image> sticker = Helper.getSticker (source);

				Helper.extractZip (source, path, img.Item1, sticker.Item1);
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
	}
}