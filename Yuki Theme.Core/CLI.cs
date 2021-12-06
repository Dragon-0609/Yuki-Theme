using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Yuki_Theme.Core.Database;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Parsers;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core
{
	public static class CLI
	{
		private static string getPath => Path.Combine ( currentPath, "Themes", $"{currentFile}.yukitheme");
		private static string gp      => $"Yuki_Theme.Core.Themes.{currentFile}.yukitheme";

		#region Public Fields

		public static List <string>   names    = new List <string> ();
		public static List <string>   schemes  = new List <string> ();
		public static DatabaseManager database = new DatabaseManager ();

		public static int       actionChoice;
		public static bool      askChoice;
		public static bool      update;
		public static string    pascalPath = "empty";
		public static bool      bgImage;
		public static bool      swSticker;
		public static bool      swStatusbar;
		public static bool      swLogo;
		public static int       settingMode;
		public static string    currentFile  = "N|L";
		public static string    currentoFile = "N|L";
		public static string    selectedItem = "empty";
		public static string    imagePath    = "";
		public static string    currentPath  = Path.GetDirectoryName (Application.ExecutablePath);
		public static Alignment align        = Alignment.Left;
		public static int       opacity      = 10;
		public static int       sopacity      = 100;

		public static Dictionary <string, Dictionary <string, string>> localAttributes =
			new Dictionary <string, Dictionary <string, string>> ();

		public static Func <string, string, bool> SaveInExport;
		public static Action <string, string>     setPath;
		public static Action <string, string>     FinishExport;
		public static Action <string, string>     ErrorExport;
		public static Func <DialogResult>         AskChoice;
		public static Action <string>             hasProblem      = null;
		public static Action               onBGIMAGEChange = null;
		public static Action               onSTICKERChange = null;
		public static Action               onSTATUSChange = null;
		public static Action <Image> ifHasImage = null;
		public static Action ifDoesntHave = null;
		public static Action <Image> ifHasSticker = null;
		public static Action ifDoesntHaveSticker = null;

		#endregion


		#region Main Commands

		public static void load_schemes (Func <string> ifZero = null)
		{
			schemes.Clear ();

			schemes.AddRange (DefaultThemes.def);

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

		public static void add (string name, string copyFrom)
		{
			string syt = copyFrom.Replace (": ", "__").Replace (":", "");
			string patsh = Path.Combine (currentPath,
			                             $"Themes/{name}.yukitheme");
			if (DefaultThemes.isDefault (copyFrom))
				CopyFromMemory (syt, patsh);
			else
				File.Copy (Path.Combine (currentPath, $"Themes/{syt}.yukitheme"), patsh);

			if (copyFrom.Contains ("__"))
			{
				var doc = new XmlDocument ();
				Tuple <bool, string> content = Helper.getTheme (patsh);
				bool iszip = content.Item1;
				if (content.Item1)
				{
					doc.LoadXml (content.Item2);
				} else
				{
					try
					{
						doc.Load (patsh);
					} catch (System.Xml.XmlException)
					{
						if (hasProblem != null)
							hasProblem (
								"There's problem in your theme file. Sorry, but I can't open it. The default scheme will be selected");

						return;
					}
				}

				var node = doc.SelectSingleNode ("/SyntaxDefinition") ??
				           throw new ArgumentNullException ("doc.SelectSingleNode (\"/SyntaxDefinition\")");
				XmlNodeList comms = node.SelectNodes ("//comment()");
				if (comms.Count >= 1)
				{
					bool hasOp = false;
					foreach (XmlComment comm in comms)
					{
						if (comm.Value.StartsWith ("name"))
						{
							comm.Value = "name:" + name;
						}

						if (comm.Value.StartsWith ("sopacity"))
							hasOp = true;
					}

					if (!hasOp)
					{
						node.AppendChild (doc.CreateComment ("sopacity:" + (sopacity).ToString ()));	
					}
				} else
				{
					node.AppendChild (doc.CreateComment ("name:" + name));
					node.AppendChild (doc.CreateComment ("align:" + ((int) align).ToString ()));
					node.AppendChild (doc.CreateComment ("opacity:" + (opacity).ToString ()));
					node.AppendChild (doc.CreateComment ("sopacity:" + (sopacity).ToString ()));
				}


				string txml = doc.OuterXml;
				if (iszip)
				{
					Helper.updateZip (getPath, txml, null, true, null, true);
				} else
				{
					doc.Save (patsh);
				}
			}
		}

		public static void remove (string st, Func <string, string, bool> askD, Func <string, object> afterAsk = null,
		                           Action <string, object> afterDelete = null)
		{
			st = st.Substring (7);
			if (DefaultThemes.getCategory (st).ToLower () == "custom")
			{
				if (askD ($"Do you really want to delete '{st}'?", "Delete"))
				{
					st = Helper.ConvertNameToPath (st);
					object bn = null;
					if (afterAsk != null) bn = afterAsk (st);

					saveData ();
					File.Delete (Path.Combine (currentPath, $"Themes/{st}.yukitheme"));
					if (afterDelete != null) afterDelete (st, bn);
				}
			}
		}

		public static void save (Image img2 = null, Image img3 = null)
		{
			if (!isDefault ())
				saveList (img2, img3);
		}

		public static void export (Image img2, Image img3, Action setTheme = null, Action startSettingTheme = null)
		{
			if (!isDefault () && Helper.mode != ProductMode.Plugin)
			{
				if (SaveInExport ("Do you want to save current scheme?", "Save"))
					save (img2);
			} else if (!isDefault ())
			{
				save (img2, img3);
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

					var result = DialogResult.No;
					if (Helper.mode != ProductMode.Plugin)
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
									result = DialogResult.Yes;
								}
									break;
								case 1 :
								{
									result = DialogResult.Ignore;
								}
									break;
							}
						}
					} else
					{
						result = DialogResult.OK;
					}

					if (result != DialogResult.No)
					{
						if (result == DialogResult.Ignore) CopyFiles (files);

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
					if (FinishExport != null)
						FinishExport (
							"Your scheme has been exported to the Pascal Directory. Restart PascalABC.NET to activate this scheme",
							"Done");

				Helper.CurrentTheme = currentoFile;
				if (setTheme != null)
					setTheme ();
			} else
			{
				if (ErrorExport != null)
					ErrorExport ("Export failed, because you didn't set path to the PascalABC.NET directory!",
					             "Export failed");
			}
		}

		public static void import (string path)
		{
			MainParser.Parse (path);
		}


		public static void palign (Alignment algn)
		{
			if (align != algn)
			{
				align = algn;
				convertAlign ();
			}
		}

		public static void restore (bool wantClean = true, Action onSelect = null)
		{
			localAttributes.Clear ();
			names.Clear ();
			populateList (onSelect);
			if (wantClean)
			{
				GC.Collect ();
				GC.WaitForPendingFinalizers ();
			}
		}

		public static void populateList (Action onSelect = null)
		{
			var doc = new XmlDocument ();
			if (isDefault ())
			{
				var a = GetCore ();
				Console.WriteLine (currentFile);
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
					} else
					{
						if (ifDoesntHave != null)
							ifDoesntHave ();
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
					} else
					{
						if (ifDoesntHaveSticker != null)
							ifDoesntHaveSticker ();
					}
				} else
				{
					if (ifDoesntHave != null)
						ifDoesntHave ();
					
					if (ifDoesntHaveSticker != null)
						ifDoesntHaveSticker ();
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
					} else
					{
						if (ifDoesntHave != null)
							ifDoesntHave ();
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
					}
				} else
				{
					if (ifDoesntHave != null)
						ifDoesntHave ();
					if (ifDoesntHaveSticker != null)
						ifDoesntHaveSticker ();
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

			localAttributes.Add ("BackgroundImage",
			                     new Dictionary <string, string> () {{"align", al}, {"opacity", op}});

			localAttributes.Add ("Sticker",
			                     new Dictionary <string, string> () {{"opacity", sop}});

			align = (Alignment) (int.Parse (localAttributes ["BackgroundImage"] ["align"]));
			opacity = int.Parse (localAttributes ["BackgroundImage"] ["opacity"]);
			sopacity = int.Parse (localAttributes ["Sticker"] ["opacity"]);

				if (onSelect != null)
				onSelect ();
		}

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
			database.UpdateData (dict);
			if (onBGIMAGEChange != null) onBGIMAGEChange ();
			if (onSTICKERChange != null) onSTICKERChange ();
			if (onSTATUSChange != null) onSTATUSChange ();
		}

		#endregion

		#region XML

		private static void PopulateByXMLNodeParent (XmlNode node)
		{
			foreach (XmlNode xne in node.ChildNodes) PopulateByXMLNode (xne);
		}

		private static void PopulateByXMLNode (XmlNode node)
		{
			foreach (XmlNode xn in node.ChildNodes) PopulateByXMLNodeSingular (xn);
		}

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
					    !names.Contains ("BackgroundImage"))
					{
						names.Remove ("Selection");
						names.Add ("BackgroundImage");
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

		#endregion


		private static bool isPasalDirectory (string st)
		{
			return Directory.Exists (System.IO.Path.Combine (st, "Highlighting"));
		}

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

			selectedItem = data [SettingsForm.ACTIVE];
			var os = 0;
			int.TryParse (data [SettingsForm.CHOICEINDEX], out os);
			actionChoice = os;
			int.TryParse (data [SettingsForm.SETTINGMODE], out os);
			settingMode = os;
		}


		public static string GetNameOfTheme (string sps)
		{
			XmlDocument docu = new XmlDocument ();

			Tuple <bool, string> content = Helper.getTheme (sps);
			if (content.Item1)
			{
				docu.LoadXml (content.Item2);
			} else
			{
				docu.Load (sps);
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


		private static void saveList (Image img2 = null, Image img3 = null)
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
						if (nms == "BackgroundImage")
							hadSavedImage = true;
						var attrs = localAttributes [nms];

						foreach (var att in attrs)
							childNode.Attributes [att.Key].Value = att.Value;
					}

				if (hadSavedImage)
				{
					node = doc.SelectSingleNode ("/SyntaxDefinition/Environment");
					node.RemoveChild (node.SelectSingleNode ("BackgroundImage"));
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
					
					Dictionary <string, bool> comments = new Dictionary <string, bool> () { {"name", false},{"align", false} ,{"opacity", false} ,{"sopacity", false} ,
						{"hasImage", false} ,{"hasSticker", false}};

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
							comm.Value = commentValues["align"];
							comments ["align"] = true;
						} else if (comm.Value.StartsWith ("opacity"))
						{
							comm.Value = commentValues["opacity"];
							comments ["opacity"] = true;
						} else if (comm.Value.StartsWith ("sopacity"))
						{
							comm.Value = commentValues["sopacity"];
							comments ["sopacity"] = true;
						} else if (comm.Value.StartsWith ("name"))
						{
							comm.Value = commentValues["name"];
							comments ["name"] = true;
						} else if (comm.Value.StartsWith ("hasImage"))
						{
							comm.Value = commentValues["hasImage"];
							comments ["hasImage"] = true;
						} else if (comm.Value.StartsWith ("hasSticker"))
						{
							comm.Value = commentValues["hasSticker"];
							comments ["hasSticker"] = true;
						}
					}

					foreach (KeyValuePair<string,bool> comment in comments)
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

				if (!iszip && img2 == null && img3 == null)
					doc.Save (getPath);
				else
				{
					string txml = doc.OuterXml;
					if (iszip)
					{
						Helper.updateZip (getPath, txml, img2, false, img3);
					} else
					{
						Helper.zip (getPath, txml, img2, img3);
					}
				}
			}
		}


		private static void convertAlign ()
		{
			localAttributes ["BackgroundImage"] ["align"] = ((int) align).ToString ();
		}

		private static void CleanDestination ()
		{
			var fil = Directory.GetFiles (Path.Combine (pascalPath, "Highlighting"), "*.png");
			foreach (string s in fil)
			{
				File.Delete (s);
			}
		}

		private static void CopyFiles (string [] files)
		{
			foreach (var file in files)
			{
				var fs = Path.Combine (currentPath, "Themes", Path.GetFileNameWithoutExtension (file) + ".yukitheme");
				if (!File.Exists (fs))
					File.Copy (file, fs);
			}
		}

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

		private static void DeleteFiles (string [] files)
		{
			foreach (var file in files)
			{
				if (File.Exists (file))
					File.Delete (file);
			}
		}

		public static Assembly GetCore ()
		{
			// return GetAssemblyByName ("Yuki Theme.Core");
			return  Assembly.GetExecutingAssembly ();
		}

		private static Assembly GetAssemblyByName (string name)
		{
			return AppDomain.CurrentDomain.GetAssemblies ()
			                .SingleOrDefault (assembly => assembly.GetName ().Name == name);
		}

		public static bool isDefault ()
		{
			return DefaultThemes.isDefault (currentoFile);
		}
	}
}