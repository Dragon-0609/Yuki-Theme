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
	public class CLI
	{
		private string          getPath => $"Themes/{currentFile}.yukitheme";
		private string          gp      => $"Yuki_Theme.Core.Themes.{currentFile}.yukitheme";

		#region Singleton

		public static CLI cli;

		#endregion
		
		#region  Public Fields

		public List <string>   names;
		public List <string>   schemes;
		public DatabaseManager database;
		
		public int       actionChoice;
		public bool      askChoice;
		public bool      update;
		public string    pascalPath = "empty";
		public bool      selectActive;
		public int       settingMode;
		public string    currentFile  = "N|L";
		public string    currentoFile = "N|L";
		public string    selectedItem = "empty";
		public string    imagePath    = "";
		public Alignment align        = Alignment.Left;
		public int       opacity      = 10;
		
		public Dictionary <string, Dictionary <string, string>> localAttributes;

		public Func <string, string, bool> SaveInExport;
		public Action <string, string>     setPath;
		public Action <string, string>     FinishExport;
		public Action <string, string>     ErrorExport;
		public Func <DialogResult>         AskChoice;
		public Action <string>             hasProblem = null;
		
		#endregion


		public CLI ()
		{
			cli = this;
			names = new List <string> ();
			schemes = new List <string> ();
			localAttributes = new Dictionary <string, Dictionary <string, string>> ();
		}

		#region Main Commands

		public void load_schemes (Func <string> ifZero =null)
		{
			schemes.Clear ();

			schemes.AddRange (DefaultThemes.def);

			if (Directory.Exists ("Themes"))
				foreach (var file in Directory.GetFiles ("Themes/", "*.yukitheme"))
				{
					var pts = Path.GetFileNameWithoutExtension (file);
					if (!DefaultThemes.isDefault (pts))
					{
						if (pts.Contains ("__"))
						{
							string stee = $"Themes/{pts}.yukitheme";
							string sp = GetNameOfTheme (stee);
							if (!DefaultThemes.isDefault (sp))
							{
								// Console.WriteLine(nod.Attributes ["name"].Value);
								schemes.Add (sp);
							}
						}else
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
						File.Copy (sm,getPath);
						schemes.Add (currentFile);
					}
				}
			}
		}

		public void add (string name, string copyFrom)
		{
			string syt = copyFrom.Replace (": ", "__").Replace (":", "");
			string patsh = $"Themes/{name}.yukitheme";
			if (DefaultThemes.isDefault (copyFrom))
				CLI.cli.CopyFromMemory (syt, patsh);
			else
				File.Copy ($"Themes/{syt}.yukitheme", patsh );

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
				if(comms.Count >= 1)
				{
					foreach (XmlComment comm in comms)
					{
						if (comm.Value.StartsWith ("name"))
						{
							comm.Value = "name:" + name;
						}
					}
				} else
				{
					node.AppendChild (doc.CreateComment ("name:" + name));
					node.AppendChild (doc.CreateComment ("align:" + ((int) align).ToString ()));
					node.AppendChild (doc.CreateComment ("opacity:" + (opacity).ToString ()));
				}


				string txml = doc.OuterXml;
				if (iszip)
				{
					Helper.updateZip (getPath, txml, null, true);
				} else
				{
					doc.Save (patsh);
				}
			}
		}
		
		public void remove (string st, Func<string, string, bool> askD, Func<string, object> afterAsk = null, Action<string, object> afterDelete = null)
		{
			st = st.Substring (7);
			if(DefaultThemes.getCategory(st).ToLower() == "custom")
			{
				if (askD ($"Do you really want to delete '{st}'?", "Delete"))
				{
					object bn = null;
					if (afterAsk != null) bn = afterAsk (st);

					saveData ();
					File.Delete ($"Themes/{st}.yukitheme");
					if (afterDelete != null) afterDelete (st, bn);
				}
			}
		}
		
		public void save (Image img2 = null)
		{
			if (!isDefault ())
				saveList (img2);
		}
		
		public void export (Image img2, Action setTheme = null, Action startSettingTheme = null)
		{
			if (!isDefault () && Helper.mode != ProductMode.Plugin)
				if (SaveInExport ("Do you want to save current scheme?", "Save"))
					save (img2);
			if (pascalPath.Length < 6  && Helper.mode != ProductMode.Plugin)
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
				var can = true;
				if (files != null && files.Length > 0)
				{
					// Console.WriteLine ($"FILES: {files.Length}, OR: {files [0]} | MS: {path}");
					if (files.Length == 1 && files [0] == path)
						can = false;
					if (can)
					{
						var result = DialogResult.No;
						if(Helper.mode != ProductMode.Plugin)
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
				}

				if (isDefault ())
					CopyFromMemory (currentFile, path, true);
				else
				{
					ExportTheme (path);
				}
				if(Helper.mode != ProductMode.Plugin)
					if (FinishExport != null)
						FinishExport (
							"Your scheme has been exported to the Pascal Directory. Restart PascalABC.NET to activate this scheme",
							"Done");

				if (setTheme != null)
					setTheme ();
			} else
			{
				if (ErrorExport != null)
					ErrorExport ("Export failed, because you didn't set path to the PascalABC.NET directory!",
					             "Export failed");
			}
		}

		public void import (string path)
		{
			MainParser.Parse (path);
		}
		
		
		public void palign (Alignment algn)
		{
			if (align != algn)
			{
				align = algn;
				convertAlign ();
			}
		}

		public void restore (bool wantClean = true, Action<Image> ifHasImage = null, Action ifDoesntHave =null, Action onSelect = null)
		{
			localAttributes.Clear ();
			names.Clear ();
			populateList (ifHasImage, ifDoesntHave, onSelect);
			if(wantClean)
			{
				GC.Collect ();
				GC.WaitForPendingFinalizers ();
			}
		}
		
		public void populateList (Action<Image> ifHasImage = null, Action ifDoesntHave=null, Action onSelect = null)
		{
			var doc = new XmlDocument ();
			if (isDefault ())
			{
				var a = GetCore ();
				Console.WriteLine(currentFile);
				// doc.Load (a.GetManifestResourceStream ($"Yuki_Theme.Core.Themes.{currentFile}.yukitheme"));
				
				
				Tuple <bool, string> content = Helper.getThemeFromMemory (gp, a);
				if (content.Item1)
				{
					doc.LoadXml (content.Item2);
					Tuple <bool, Image> iag = Helper.getImageFromMemory (gp, a);
					if (iag.Item1)
					{
						// img = iag.Item2;
						if(ifHasImage != null)
						{
							ifHasImage ((Image) iag.Item2);
						}
					} else
					{
						if (ifDoesntHave != null)
							ifDoesntHave ();
					}
				} else
				{
					if (ifDoesntHave != null)
						ifDoesntHave ();
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
						if(ifHasImage != null)
						{
							ifHasImage ((Image) iag.Item2);
						}
					} else
					{
						if (ifDoesntHave != null)
							ifDoesntHave ();
					}
				} else
				{
					if (ifDoesntHave != null)
						ifDoesntHave ();
					try
					{
						doc.Load (getPath);
					} catch (System.Xml.XmlException)
					{
						if (hasProblem != null)
							hasProblem ("There's problem in your theme file. Sorry, but I can't open it. The default scheme will be selected");

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
			string al = ((int) Alignment.Center).ToString();
			string op = "15";
			
			foreach (XmlComment comm in comms)
			{
				if (comm.Value.StartsWith ("align"))
				{
					al = comm.Value.Substring (6);
				}else if (comm.Value.StartsWith ("opacity"))
				{
					op = comm.Value.Substring (8);
				}
			}

			localAttributes.Add ("BackgroundImage",
			                     new Dictionary <string, string> () {{"align", al}, {"opacity", op}});
			
			if (localAttributes.ContainsKey ("BackgroundImage"))
			{
				align = (Alignment) (int.Parse (localAttributes ["BackgroundImage"] ["align"]));
				opacity = int.Parse (localAttributes ["BackgroundImage"] ["opacity"]);
			}

			if (onSelect != null)
				onSelect ();
		}

		public void CopyFromMemory (string file, string path, bool extract = false)
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

					Helper.extractZip (nxp, path);

					File.Delete (nxp);
				} else
				{
					File.Move (nxp, path);
				}
			}
		}

		public void saveData ()
		{
			var dict = new Dictionary <int, string> ();
			dict.Add (SettingsForm.PASCALPATH, pascalPath);
			dict.Add (SettingsForm.SETACTIVE, selectActive.ToString ());
			dict.Add (SettingsForm.ACTIVE, selectedItem);
			dict.Add (SettingsForm.ASKCHOICE, askChoice.ToString ());
			dict.Add (SettingsForm.CHOICEINDEX, actionChoice.ToString ());
			dict.Add (SettingsForm.SETTINGMODE, settingMode.ToString ());
			dict.Add (SettingsForm.AUTOUPDATE, update.ToString ());
			database.UpdateData (dict);
		}

		#endregion

		#region XML

		
		private void PopulateByXMLNodeParent (XmlNode node)
		{
			foreach (XmlNode xne in node.ChildNodes) PopulateByXMLNode (xne);
		}

		private void PopulateByXMLNode (XmlNode node)
		{
			foreach (XmlNode xn in node.ChildNodes) PopulateByXMLNodeSingular (xn);
		}

		private void PopulateByXMLNodeSingular (XmlNode node)
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
				}
			}
		}


		#endregion
		

		private bool isPasalDirectory (string st)
		{
			return Directory.Exists (System.IO.Path.Combine (st, "Highlighting"));
		}

		public void connectAndGet ()
		{
			database = new DatabaseManager ();
			var data = database.ReadData ();
			pascalPath = data [SettingsForm.PASCALPATH] == "empty" ? null : data [SettingsForm.PASCALPATH];
			if (Helper.mode == ProductMode.Plugin)
			{
				pascalPath = Path.GetDirectoryName (Application.ExecutablePath);
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

			selectActive = bool.Parse (data [SettingsForm.SETACTIVE]);
			askChoice = bool.Parse (data [SettingsForm.ASKCHOICE]);
			update = bool.Parse (data [SettingsForm.AUTOUPDATE]);

			selectedItem = data [SettingsForm.ACTIVE];
			var os = 0;
			int.TryParse (data [SettingsForm.CHOICEINDEX], out os);
			actionChoice = os;
			int.TryParse (data [SettingsForm.SETTINGMODE], out os);
			settingMode = os;
		}
		
		

		public string GetNameOfTheme (string sps)
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
		

		private void saveList (Image img2 = null)
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
				bool hadSavedImage = false;
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
							// Console.WriteLine($"N: {childNode.Name}, ATT: {att.Key},");
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


				node = doc.SelectSingleNode ("/SyntaxDefinition");
				
				XmlNode nod = doc.SelectSingleNode ("/SyntaxDefinition");
				XmlNodeList comms = nod.SelectNodes ("//comment()");
				if(comms.Count >= 3)
				{
					foreach (XmlComment comm in comms)
					{
						if (comm.Value.StartsWith ("align"))
						{
							comm.Value = "align:" + ((int) align).ToString ();
						} else if (comm.Value.StartsWith ("opacity"))
						{
							comm.Value = "opacity:" + (opacity).ToString ();
						}
					}
				}else
				{
					node.AppendChild (doc.CreateComment ("name:" + currentoFile));
					node.AppendChild (doc.CreateComment ("align:" + ((int) align).ToString ()));
					node.AppendChild (doc.CreateComment ("opacity:" + (opacity).ToString ()));
				}
				
				if (!iszip && img2 == null)
					doc.Save (getPath);
				else
				{
					string txml = doc.OuterXml;
					if (iszip)
					{
						Helper.updateZip (getPath, txml, img2);
					} else
					{
						Helper.zip (getPath, txml, img2);
					}
				}
			}
		}


		private void convertAlign ()
		{
			localAttributes ["BackgroundImage"] ["align"] = ((int) align).ToString ();
		}
		
		private void CleanDestination ()
		{
			var fil = Directory.GetFiles (Path.Combine (pascalPath, "Highlighting"), "*.png");
			foreach (string s in fil)
			{
				File.Delete (s);
			}
		}
		
		private void CopyFiles (string [] files)
		{
			foreach (var file in files)
			{
				var fs = Path.GetFileNameWithoutExtension (file) + ".yukitheme";
				if (!File.Exists (fs))
					File.Copy (file, fs);
			}
		}
		
		private void ExportTheme (string path)
		{
			string source = getPath;
			bool iszip = Helper.isZip (source);
			if (!iszip)
			{
				File.Copy (source, path, true);
			} else
			{
				CleanDestination ();

				Helper.extractZip (source, path);
			}
		}
		
		private void DeleteFiles (string [] files)
		{
			foreach (var file in files) File.Delete (file);
		}

		public Assembly GetCore ()
		{
			return GetAssemblyByName ("Yuki Theme.Core");
		}
		
		private Assembly GetAssemblyByName(string name)
		{
			return AppDomain.CurrentDomain.GetAssemblies().
			                 SingleOrDefault(assembly => assembly.GetName().Name == name);
		}
		
		public bool isDefault ()
		{
			return DefaultThemes.isDefault (currentoFile);
		}
	}
}