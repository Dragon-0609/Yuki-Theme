using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using FastColoredTextBoxNS;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Svg;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.Database;
using Yuki_Theme.Core.Parsers;
using Yuki_Theme.Core.Themes;
using CommonDialog = System.Windows.Forms.CommonDialog;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace Yuki_Theme.Core.Forms
{
	public partial class MForm : Form
	{
		private readonly ColorPicker     col;
		private readonly Highlighter     highlighter;
		private          int             actionChoice;
		private          bool            askChoice;
		private          bool            update;
		private          bool            blocked;
		private          bool            blockedNumeric;
		private          bool            unblockedScrool;
		private          string          currentFile = "N|L";
		private          string          currentoFile = "N|L";
		private          DatabaseManager database;
		private          int             lastIndex = 1;

		public Dictionary <string, Dictionary <string, string>> localAttributes;

		private string           pascalPath = "empty";
		public  Populater        populater;
		private bool             selectActive;
		public  string           selectedItem = "empty";
		public  SelectionForm    selform;
		private SettingsForm     setform;
		public  int              settingMode;
		private ThemeManager     tmanagerform;
		public  DownloadForm     df;
		public  NotificationForm nf;
		private Image            img = null;
		private Image            img2 = null;

		private Rectangle        oldV    = Rectangle.Empty;
		private Alignment        align   = Alignment.Left;
		private int              opacity = 10;
		public  Color            bgClicked => Helper.bgClick;
		public  Color            bgSpecial => Helper.bgBorder;
		public  Color            fgDefault => Helper.fgColor;
		public  Color            fgHover   => Helper.fgHover;
		public  Color            fgKey     => Helper.fgKeyword;
		public  Color            bgDefault => Helper.bgColor;
		private string           getPath   => $"Themes/{currentFile}.yukitheme";
		private string           gp        => $"Yuki_Theme.Core.Themes.{currentFile}.yukitheme";
		public event ColorUpdate OnColorUpdate;
		public event SetTheme    setTheme;
		public event SetTheme    startSettingTheme;

		public  Brush   fgbrush;
		private int     textBoxHeight = 0;
		private int     notHeight = 0;

		public MForm (int mode = 0)
		{
			Helper.mode = (ProductMode) mode;
			textBoxHeight =  Helper.mode == ProductMode.Program ? 140 : 178;
			notHeight =  Helper.mode == ProductMode.Program ? 50 : 88;
			InitializeComponent ();
			connectAndGet ();
			localAttributes = new Dictionary <string, Dictionary <string, string>> ();
			highlighter = new Highlighter (sBox, this);
			populater = new Populater ();
			load_schemes ();
			if(Helper.mode == ProductMode.Plugin)
				initPlugin ();
			if (currentFile != "N|L")
			{
				col = new ColorPicker (this);
				highlighter.InitializeSyntax ();
				loadSVG ();
				save_button.Visible = !isDefault ();
				AddEvents ();
				// img = Image.FromFile (
				// 	// @"C:\Users\User\Documents\CSharp\Yuki Theme Plugin\Yuki Theme Plugin\Resources\asuna_dark.png");
				// 	@"C:\Users\User\Documents\CSharp\Yuki Theme Plugin\Yuki Theme Plugin\Resources\emilia_dark.png");
				sBox.Paint += bgImagePaint;
				if (update)
					button7_Click (this, EventArgs.Empty);
				MForm_SizeChanged (this, EventArgs.Empty);
				if (Helper.mode != ProductMode.Plugin)
				{
					Controls.Remove (bottomPanel);
				}

				isUpdated ();

				// df = new DownloadForm (this);
				// nf = new NotificationForm ();
			} else
			{
				throw new ApplicationException ("Error on loading the scheme file");
			}
		}

		private void isUpdated ()
		{
			RegistryKey ke =
				Registry.CurrentUser.CreateSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);

			int inst = ke.GetValue ("install") != null ? 1 : 0;
			if (inst == 1)
			{
				ShowNotification ("Update Complete", "The program has been successfully updated");
				ke.DeleteValue ("install");
			}
		}

		private void onSelectItem (object sender, EventArgs e)
		{
			if (lastIndex != list_1.SelectedIndex)
			{
				colorButton.Visible = false;
				bgButton.Visible = false;
				check_bold.Enabled = false;
				check_italic.Enabled = false;
				lastIndex = list_1.SelectedIndex;
				blocked = true;
				// Console.WriteLine(list_1.SelectedItem.ToString ());
				var str = list_1.SelectedItem.ToString ();
				if (!str.Contains ("Image"))
				{
					if (!colorEditor.Visible)
					{
						colorEditor.Visible = true;
						imageEditor.Visible = false;
					}

					if (highlighter.isInNames (str) && !isDefault ())
					{
						check_bold.Enabled = true;
						check_italic.Enabled = true;
					}

					var dic = localAttributes [str];
					foreach (var item in dic)
						switch (item.Key)
						{
							case "color" :
							{
								colorButton.BackColor = ColorTranslator.FromHtml (item.Value);
								colorButton.Visible = true;
							}
								break;
							case "bgcolor" :
							{
								bgButton.BackColor = ColorTranslator.FromHtml (item.Value);
								bgButton.Visible = true;
							}
								break;
							case "bold" :
							{
								// check_bold.Enabled = true;
								check_bold.Checked = bool.Parse (item.Value + "");
							}
								break;
							case "italic" :
							{
								// check_italic.Enabled = true;
								check_italic.Checked = bool.Parse (item.Value + "");
							}
								break;
						}

					blocked = false;
					highlighter.activateColors (str);
				} else
				{
					if (colorEditor.Visible)
					{
						colorEditor.Visible = false;
						imageEditor.Visible = true;
					}

					align = (Alignment) (int.Parse (localAttributes ["BackgroundImage"] ["align"]));
					opacity = int.Parse (localAttributes ["BackgroundImage"] ["opacity"]);

					updateAlignButton ();

					imageEditor.Enabled = !isDefault ();
				

				}
			}
		}

		private void saveList ()
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

				;

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

		private void populateList ()
		{
			var doc = new XmlDocument ();
			if (isDefault ())
			{
				var a = Assembly.GetExecutingAssembly ();
				Console.WriteLine(currentFile);
				// doc.Load (a.GetManifestResourceStream ($"Yuki_Theme.Core.Themes.{currentFile}.yukitheme"));
				
				
				imagePath.Text = "";
				Tuple <bool, string> content = Helper.getThemeFromMemory (gp, a);
				if (content.Item1)
				{
					doc.LoadXml (content.Item2);
					Tuple <bool, Image> iag = Helper.getImageFromMemory (gp, a);
					if (iag.Item1)
					{
						// img = iag.Item2;
						img2 = (Image) iag.Item2;
						oldV = Rectangle.Empty;
						imagePath.Text = "background.png";
					} else
					{
						img = null;
						img2 = null;
					}
				} else
				{
					img = null;
					img2 = null;
					doc.Load (a.GetManifestResourceStream (gp));
				}
			} else
			{
				imagePath.Text = "";
				Tuple <bool, string> content = Helper.getTheme (getPath);
				if (content.Item1)
				{
					doc.LoadXml (content.Item2);
					Tuple <bool, Image> iag = Helper.getImage (getPath);
					if (iag.Item1)
					{
						// img = iag.Item2;
						img2 = (Image) iag.Item2;
						oldV = Rectangle.Empty;
						imagePath.Text = "background.png";
					} else
					{
						img = null;
						img2 = null;
					}
				} else
				{
					img = null;
					img2 = null;
					try
					{
						doc.Load (getPath);
					} catch (System.Xml.XmlException)
					{
						MessageBox.Show (
							"There's problem in your theme file. Sorry, but I can't open it. The default scheme will be selected",
							"Theme file is invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
						schemes.SelectedIndex = 0;
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

			list_1.SelectedIndex = 0;
			onSelectItem (list_1, EventArgs.Empty);
		}

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

					if (highlighter.isInNames (nm))
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

				if (!list_1.Items.Contains (nm))
				{
					if (settingMode == 1)
					{
						list_1.Items.Add (nm);
						localAttributes.Add (nm, attrs);
					} else if (!localAttributes.ContainsKey (nm))
					{
						// Console.WriteLine ( $"InList: {nm}|{localAttributes.ContainsKey (nm)}");		
						localAttributes.Add (nm, attrs);
						if (!populater.isInList (nm, list_1.Items)) list_1.Items.Add (nm);
					}
					if (nm.Equals ("selection", StringComparison.OrdinalIgnoreCase) &&
					    !list_1.Items.Contains ("BackgroundImage"))
					{
						list_1.Items.Remove ("Selection");
						list_1.Items.Add ("BackgroundImage");
						list_1.Items.Add ("Selection");
					}
				}
			}
		}

		private void colorButton_Click (object sender, EventArgs e)
		{
			if (!blocked)
			{
				col.MainColor = colorButton.BackColor;
				if (col.ShowDialog (this) == DialogResult.OK) colorButton.BackColor = col.MainColor;
				updateCurrentItem ();
			}
		}

		private void bgButton_Click (object sender, EventArgs e)
		{
			if (!blocked)
			{
				col.MainColor = bgButton.BackColor;
				if (col.ShowDialog (this) == DialogResult.OK)
				{
					bgButton.BackColor = col.MainColor;
					updateCurrentItem ();
				}
			}
		}

		private void updateCurrentItem ()
		{
			var str = list_1.SelectedItem.ToString ();
			var dic = localAttributes [str];
			if (dic.ContainsKey ("color"))
				dic ["color"] = Translate (colorButton.BackColor);
			if (dic.ContainsKey ("bgcolor"))
				dic ["bgcolor"] = Translate (bgButton.BackColor);
			if (dic.ContainsKey ("bold"))
				dic ["bold"] = check_bold.Checked.ToString ().ToLower ();
			if (dic.ContainsKey ("italic"))
				dic ["italic"] = check_italic.Checked.ToString ().ToLower ();
			if (settingMode == 0)
			{
				var sttr = populater.getDependencies (str);
				if (sttr != null)
					foreach (var sr in sttr)
					{
						dic = localAttributes [sr];
						if (dic.ContainsKey ("color"))
							dic ["color"] = Translate (colorButton.BackColor);
						if (dic.ContainsKey ("bgcolor"))
							dic ["bgcolor"] = Translate (bgButton.BackColor);
						if (dic.ContainsKey ("bold"))
							dic ["bold"] = check_bold.Checked.ToString ().ToLower ();
						if (dic.ContainsKey ("italic"))
							dic ["italic"] = check_italic.Checked.ToString ().ToLower ();
					}
			}

			highlighter.updateColors ();
		}

		private string Translate (Color clr)
		{
			return ColorTranslator.ToHtml (clr);
		}
		
		private void MForm_SizeChanged (object sender, EventArgs e)
		{
			int he = ClientSize.Height - textBoxHeight;
			sBox.Height = he;
		}

		private void save_Click (object sender, EventArgs e)
		{
			if (!isDefault ())
				saveList ();
		}

		public void button7_Click (object sender, EventArgs e)
		{
			/*if (!isDefault ())
				saveList ();*/
			// showDownloader ();
			if (df == null)
				df = new DownloadForm (this);
			if (nf == null)
				nf = new NotificationForm ();
			// Thread th = new Thread (new ThreadStart(df.CheckUpdate));
			// th.Start();
			df.CheckUpdate ();
		}

		private void restore_Click (object sender, EventArgs e)
		{
			list_1.Items.Clear ();
			localAttributes.Clear ();
			populateList ();
			lastIndex = 151;
			highlighter.updateColors ();
			updateAlignButton ();
			updateBackgroundColors ();
			unblockedScrool = true;
			
			opacity_slider_Scroll (
				sender, new ScrollEventArgs (ScrollEventType.ThumbPosition, opacity));
				
			GC.Collect();
			GC.WaitForPendingFinalizers();  
			
		}

		private void check_bold_CheckedChanged (object sender, EventArgs e)
		{
			if (!blocked)
				// Console.WriteLine ("Not blocked");
				updateCurrentItem ();
			// else Console.WriteLine ("Blocked");
		}

		private void check_italic_CheckedChanged (object sender, EventArgs e)
		{
			if (!blocked)
				// Console.WriteLine ("Not blocked");
				updateCurrentItem ();
			// else Console.WriteLine ("Blocked");
		}

		private void schemes_SelectedIndexChanged (object sender, EventArgs e)
		{
			currentoFile = schemes.SelectedItem.ToString ();
			currentFile = currentoFile.Replace (": ", "__").Replace (":","");
			save_button.Visible = !isDefault ();
			restore_Click (sender, e);
			if (selectActive)
			{
				selectedItem = schemes.SelectedItem.ToString ();
				database.UpdateData (SettingsForm.ACTIVE, selectedItem);
			}
		}

		public void load_schemes ()
		{
			schemes.Items.Clear ();

			schemes.Items.AddRange (DefaultThemes.def);

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
								schemes.Items.Add (sp);
							}
						}else
						{
							schemes.Items.Add (pts);
						}
					}
				}

			if (schemes.Items.Count == 0)
			{
				var op = new OpenFileDialog ();
				op.DefaultExt = "yukitheme";
				op.Filter = "Yuki Theme(*.yukitheme)|*.yukitheme";
				op.Multiselect = false;
				if (op.ShowDialog () == DialogResult.OK)
				{
					currentFile = Path.GetFileNameWithoutExtension (op.FileName);
					currentoFile = currentFile;
					schemes.Items.Add (currentFile);
				}
			}

			if (schemes.Items.Contains (selectedItem))
				schemes.SelectedItem = selectedItem;
			else
				schemes.SelectedIndex = 0;
		}

		private void button3_Click (object sender, EventArgs e)
		{
			if (selform == null)
				selform = new SelectionForm ();
			selform.textBox1.Text = "";
			selform.comboBox1.Items.Clear ();
			foreach (var item in schemes.Items) selform.comboBox1.Items.Add (item);

			selform.comboBox1.SelectedIndex = 0;

			if (selform.ShowDialog () == DialogResult.OK)
			{
				if (!Directory.Exists ("Themes"))
					Directory.CreateDirectory ("Themes");
				string syt = selform.comboBox1.SelectedItem.ToString ();
				string patsh = $"Themes/{selform.textBox1.Text}.yukitheme";
				if (DefaultThemes.isDefault (syt))
					CopyFromMemory (syt, patsh);
				else
					File.Copy ($"Themes/{selform.comboBox1.SelectedItem}.yukitheme", patsh);
				schemes.Items.Add (selform.textBox1.Text);
				schemes.SelectedItem = selform.textBox1.Text;
			}
		}

		private void button4_Click (object sender, EventArgs e)
		{
			if (setform == null)
				setform = new SettingsForm (this);
			if(Helper.mode == ProductMode.Program)
				setform.Path = pascalPath;
			else
			{
				setform.setVisible (false);
				
			}
			setform.Active = selectActive;
			setform.askC.Checked = askChoice;
			setform.checkBox2.Checked = update;
			setform.ActionBox.SelectedIndex = actionChoice;
			setform.mode.SelectedIndex = settingMode;

			setform.schemes.Items.Clear ();
			for (var i = 0; i < schemes.Items.Count; i++) setform.schemes.Items.Add (schemes.Items [i]);

			if (setform.schemes.Items.Contains (selectedItem))
				setform.schemes.SelectedItem = selectedItem;
			else
				setform.schemes.SelectedIndex = 0;
			var st = settingMode;
			if (setform.ShowDialog () == DialogResult.OK)
			{
				pascalPath = setform.Path;
				selectActive = setform.Active;
				selectedItem = setform.schemes.SelectedItem.ToString ();
				askChoice = setform.askC.Checked;
				update = setform.checkBox2.Checked;
				actionChoice = setform.ActionBox.SelectedIndex;
				settingMode = setform.mode.SelectedIndex;
				saveData ();
				if (settingMode != st) restore_Click (this, EventArgs.Empty);
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

		private void connectAndGet ()
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

		private void export (object sender, EventArgs e)
		{
			if (!isDefault () && Helper.mode != ProductMode.Plugin)
				if (MessageBox.Show (this, "Do you want to save current scheme?", "Save",
				                     MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					save_Click (sender, e);
			if (pascalPath.Length < 6  && Helper.mode != ProductMode.Plugin)
			{
				MessageBox.Show ((IWin32Window) this, "Please, set path to the PascalABC.NET Direcory.",
				                 "Path to the PascalABC.NET Direcory", MessageBoxButtons.OK,
				                 MessageBoxIcon.Exclamation);
				button4_Click (sender, e);
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
								var quform = new QuestionForm ();
								result = quform.ShowDialog ();
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
					MessageBox.Show (this,
				                 "Your scheme has been exported to the Pascal Directory. Restart PascalABC.NET to activate this scheme",
				                 "Done");
			
				if (setTheme != null)
					setTheme ();
			} else
			{
				MessageBox.Show ((IWin32Window) this,
				                 "Export failed, because you didn't set path to the PascalABC.NET directory!",
				                 "Export failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

		public void CopyFromMemory (string file, string path, bool extract = false)
		{
			var a = Assembly.GetExecutingAssembly ();
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

		private void CleanDestination ()
		{
			var fil = Directory.GetFiles (Path.Combine (pascalPath, "Highlighting"), "*.png");
			foreach (string s in fil)
			{
				File.Delete (s);
			}
		}

		private void PrepareToUse (string path)
		{
			XmlDocument doc = new XmlDocument ();
						
			doc.Load (path);
			XmlNode node = doc.SelectSingleNode ("/SyntaxDefinition/Environment/BackgroundImage");


			if (node != null)
			{
				string al = node.Attributes ["align"].Value;
				string op = node.Attributes ["opacity"].Value;

				XmlNode parent = node.ParentNode;


				parent.RemoveChild (node);


				string newXML = doc.OuterXml;


				doc.Save (path);

				File.Move (Path.Combine (pascalPath, "Highlighting", "background.png"),
				           Path.Combine (pascalPath, "Highlighting", $"background_{al}_{op}.png"));
			}
		}
		
		private bool isPasalDirectory (string st)
		{
			return Directory.Exists (System.IO.Path.Combine (st, "Highlighting"));
		}

		private void DeleteFiles (string [] files)
		{
			foreach (var file in files) File.Delete (file);
		}

		private void button5_Click (object sender, EventArgs e)
		{
			if (tmanagerform == null)
				tmanagerform = new ThemeManager (this);

			ReItem defa = new ReItem ("Default", true);
			ReItem doki = new ReItem ("Doki Theme", true);
			ReItem custom = new ReItem ("Custom", true);
			tmanagerform.scheme.Items.Clear ();
			tmanagerform.groups.Clear ();
			tmanagerform.groups.Add (defa);
			tmanagerform.groups.Add (doki);
			tmanagerform.groups.Add (custom);

			foreach (string item in schemes.Items)
			{
				ReItem litem;
				if (DefaultThemes.isDefault (item))
				{
					ReItem cat = DefaultThemes.getCategory (item) == "Doki Theme" ? doki : defa;
					litem = new ReItem (item, false, cat);
				} else
				{
					litem = new ReItem (item, false, custom);
				}
			}

			tmanagerform.sortItems ();

			tmanagerform.ShowDialog ();
		}

		public bool isDefault ()
		{
			return DefaultThemes.isDefault (currentoFile);
		}

		private void AddEvents ()
		{
			save_button.MouseHover += (sender, args) =>
			{
				tip.Show ("Save", save_button);
			};
			save_button.MouseLeave += (sender, args) =>
			{
				if (tip.Active)
					tip.Hide (save_button);
			};

			restore_button.MouseHover += (sender, args) =>
			{
				tip.Show ("Restore", restore_button);
			};
			restore_button.MouseLeave += (sender, args) =>
			{
				if (tip.Active)
					tip.Hide (restore_button);
			};

			settings_button.MouseHover += (sender, args) =>
			{
				tip.Show ("Settings", settings_button);
			};
			settings_button.MouseLeave += (sender, args) =>
			{
				if (tip.Active)
					tip.Hide (settings_button);
			};

			export_button.MouseHover += (sender, args) =>
			{
				tip.Show ("Export", export_button);
			};
			export_button.MouseLeave += (sender, args) =>
			{
				if (tip.Active)
					tip.Hide (export_button);
			};

			import_button.MouseHover += (sender, args) =>
			{
				tip.Show ("Import", import_button);
			};
			import_button.MouseLeave += (sender, args) =>
			{
				if (tip.Active)
					tip.Hide (import_button);
			};

			add_button.MouseHover += (sender, args) =>
			{
				tip.Show ("Add New Scheme", add_button);
			};
			add_button.MouseLeave += (sender, args) =>
			{
				if (tip.Active)
					tip.Hide (add_button);
			};

			manage_button.MouseHover += (sender, args) =>
			{
				tip.Show ("Manage Themes", manage_button);
			};
			manage_button.MouseLeave += (sender, args) =>
			{
				if (tip.Active)
					tip.Hide (manage_button);
			};

			import_directory.MouseHover += (sender, args) =>
			{
				tip.Show ("Import Themes", import_directory);
			};
			import_directory.MouseLeave += (sender, args) =>
			{
				if (tip.Active)
					tip.Hide (import_directory);
			};
		}

		private void import_Click (object sender, EventArgs e)
		{
			OpenFileDialog fd = new OpenFileDialog ();
			// fd.DefaultExt = "icls";
			fd.Title = "Import";
			fd.InitialDirectory = Path.GetDirectoryName (Application.ExecutablePath);
			fd.Filter =
				"All Themes(*.icls,*.yukitheme,*.json)|*.icls;*.yukitheme;*.json|JetBrains IDE Scheme(*.icls)|*.icls|Yuki Theme(*.yukitheme)|*.yukitheme|Doki Theme(*.json)|*.json";
			fd.Multiselect = false;
			if (fd.ShowDialog () == DialogResult.OK)
			{
				MainParser.Parse (fd.FileName, this);
			}
		}

		public void showDownloader ()
		{
			if (df == null || nf.IsDisposed)
				df = new DownloadForm (this);
			df.Show (this);
			changeDownloaderLocation ();
			if (nf != null && !nf.IsDisposed && nf.Visible)
			{
				nf.Visible = false;
			}
		}

		private void panel1_Resize (object sender, EventArgs e)
		{
			changeDownloaderLocation ();
			changeNotificationLocation ();
		}

		public void changeDownloaderLocation ()
		{
			if (df != null && !df.IsDisposed && df.Visible)
			{
				df.StartPosition = FormStartPosition.Manual;
				df.Location = new Point (this.Location.X + this.ClientRectangle.Width - 284,
				                         this.Location.Y + this.ClientRectangle.Height - 73);
			}
		}

		public void changeNotificationLocation ()
		{
			if (nf != null && !nf.IsDisposed && nf.Visible)
			{
				nf.StartPosition = FormStartPosition.Manual;
				nf.Location = new Point (this.Location.X + this.ClientRectangle.Width - 306,
				                         this.Location.Y + this.ClientRectangle.Height - notHeight);
			}
		}

		private void MForm_Move (object sender, EventArgs e)
		{
			changeDownloaderLocation ();
			changeNotificationLocation ();
		}

		public void ShowNotification (string title, string content)
		{
			if (nf == null || nf.IsDisposed)
				nf = new NotificationForm ();
			nf.Visible = false;
			nf.changeContent (title, content);


			nf.Show (this);

			if (df != null && !df.IsDisposed && df.Visible)
			{
				df.Visible = false;
			}
		}

		private void MForm_FormClosing (object sender, FormClosingEventArgs e)
		{
			if (nf != null)
				nf.Dispose ();
			if (df != null)
				df.Dispose ();
			this.Dispose (true);
		}

		private void button11_Click (object sender, EventArgs e)
		{
			var op = new OpenFileDialog ();
			op.DefaultExt = "png";
			op.Filter = "PNG (*.png)|*.png";
			op.Multiselect = false;
			if (op.ShowDialog () == DialogResult.OK)
			{
				imagePath.Text = op.FileName;
				button10_Click (sender, e);
			}
		}

		private void pleft_Click (object sender, EventArgs e)
		{
			if (align != Alignment.Left)
			{
				align = Alignment.Left;
				convertAlign ();
			}

			updateAlignButton ();
		}

		private void pcenter_Click (object sender, EventArgs e)
		{
			if (align != Alignment.Center)
			{
				align = Alignment.Center;
				convertAlign ();
			}

			updateAlignButton ();
		}

		private void pright_Click (object sender, EventArgs e)
		{
			if (align != Alignment.Right)
			{
				align = Alignment.Right;
				convertAlign ();
			}

			updateAlignButton ();
		}

		private void bgImagePaint (object sender, PaintEventArgs e)
		{
			if (img != null)
			{
				if (oldV.Width != sBox.ClientRectangle.Width || oldV.Height != sBox.ClientRectangle.Height)
				{
					oldV = Helper.getSizes (img.Size, sBox.ClientRectangle.Width, sBox.ClientRectangle.Height,
					                        align);
				}

				e.Graphics.DrawImage (img, oldV);
			}
		}

		private void convertAlign ()
		{
			localAttributes ["BackgroundImage"] ["align"] = ((int) align).ToString ();
		}

		private void updateAlignButton ()
		{
				pleft.BackColor = align == Alignment.Left ? bgClicked : bgDefault;
				pcenter.BackColor = align == Alignment.Center ? bgClicked : bgDefault;
				pright.BackColor = align == Alignment.Right ? bgClicked : bgDefault;
				sBox.Refresh ();
		}

		public void updateBackgroundColors ()
		{
			this.BackColor = list_1.BackColor = save_button.BackColor = restore_button.BackColor =
				export_button.BackColor = import_button.BackColor = settings_button.BackColor = add_button.BackColor =
					manage_button.BackColor = import_directory.BackColor = panel1.BackColor = button10.BackColor =
						button11.BackColor = button7.BackColor = schemes.ListBackColor = schemes.BackColor = 
						opacity_slider.BackColor = imagePath.BackColor = numericUpDown1.BackColor = bgDefault;

			this.ForeColor = label1.ForeColor = list_1.ForeColor = label2.ForeColor = check_bold.ForeColor =
				check_italic.ForeColor = button10.FlatAppearance.BorderColor = button11.FlatAppearance.BorderColor =
					button7.FlatAppearance.BorderColor = schemes.ForeColor = schemes.ListTextColor = label3.ForeColor =
						imagePath.ForeColor = numericUpDown1.ForeColor = fgDefault;

			button10.FlatAppearance.MouseOverBackColor = save_button.FlatAppearance.MouseOverBackColor =
				export_button.FlatAppearance.MouseOverBackColor = import_button.FlatAppearance.MouseOverBackColor =
					restore_button.FlatAppearance.MouseOverBackColor =
						settings_button.FlatAppearance.MouseOverBackColor =
							add_button.FlatAppearance.MouseOverBackColor =
								manage_button.FlatAppearance.MouseOverBackColor =
									import_directory.FlatAppearance.MouseOverBackColor =
										button11.FlatAppearance.MouseOverBackColor =
											button7.FlatAppearance.MouseOverBackColor =
												opacity_slider.BarPenColorBottom =
													opacity_slider.BarPenColorTop = 
														opacity_slider.BarInnerColor = 
															numericUpDown1.ButtonHighlightColor = bgClicked;

			schemes.BorderColor = schemes.IconColor = opacity_slider.ThumbInnerColor = opacity_slider.ThumbPenColor =
				opacity_slider.ElapsedInnerColor = opacity_slider.ElapsedPenColorBottom = 
					opacity_slider.ElapsedPenColorTop = imagePath.BorderColor = numericUpDown1.BorderColor = bgSpecial;

			opacity_slider.ThumbOuterColor = fgKey;
			
			if (OnColorUpdate != null)
				OnColorUpdate (bgDefault, fgDefault, bgClicked);
			fgbrush = new SolidBrush (fgDefault);
			loadSVG ();
			if (nf != null && !nf.IsDisposed && nf.Visible)
				nf.NotificationForm_Shown (this, EventArgs.Empty);
		}

		private void button10_Click (object sender, EventArgs e)
		{
			if (imagePath.Text == "background.png")
			{
				Tuple <bool, Image> iag = isDefault ()
					? Helper.getImageFromMemory (gp, Assembly.GetExecutingAssembly ())
					: Helper.getImage (getPath);
				if (iag.Item1)
				{
					// img = iag.Item2;
					img2 = iag.Item2;
					oldV = Rectangle.Empty;
				}

				unblockedScrool = true;
				opacity_slider_Scroll (sender, new ScrollEventArgs (ScrollEventType.ThumbPosition, opacity));
				
				sBox.Refresh ();
			} else if (File.Exists (imagePath.Text))
			{
				if (imagePath.Text.ToLower ().EndsWith (".png"))
				{
					img = Image.FromFile (imagePath.Text);
					img2 = (Image) img.Clone ();
					sBox.Refresh ();
				} else
				{
					MessageBox.Show ("File format must be .png!", "Invalid format", MessageBoxButtons.OK,
					                 MessageBoxIcon.Error);
				}
			} else if (imagePath.Text.Length < 2)
			{
				img = null;
				img2 = null;
				sBox.Refresh ();
			} else
			{
				MessageBox.Show ("File isn't exist", "File not found", MessageBoxButtons.OK,
				                 MessageBoxIcon.Error);
			}
		}

		private void button7_Click_1 (object sender, EventArgs e)
		{
			imagePath.Text = "";
			button10_Click (sender, e);
		}

		private void loadSVG ()
		{
			var a = Assembly.GetExecutingAssembly ();
			Helper.renderSVG (save_button, Helper.loadsvg ("save", a));
			Helper.renderSVG (restore_button, Helper.loadsvg ("arrow-clockwise", a));
			Helper.renderSVG (export_button, Helper.loadsvg ("upload", a));
			Helper.renderSVG (import_button, Helper.loadsvg ("download", a));
			Helper.renderSVG (settings_button, Helper.loadsvg ("gear", a));
			Helper.renderSVG (add_button, Helper.loadsvg ("plus-square", a));
			Helper.renderSVG (manage_button, Helper.loadsvg ("list-task", a));
			Helper.renderSVG (import_directory, Helper.loadsvg ("box-arrow-down", a));
			Helper.renderSVG (button11, Helper.loadsvg ("three-dots", a), true, new Size (16,16));
		}

		private void list_1_DrawItem (object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0) return;
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				e = new DrawItemEventArgs (e.Graphics, e.Font, e.Bounds,
				                           e.Index, e.State ^ DrawItemState.Selected,
				                           e.ForeColor, bgClicked);
			}

			e.DrawBackground ();
			e.Graphics.DrawString (list_1.Items [e.Index].ToString (), e.Font, fgbrush, e.Bounds);

			e.DrawFocusRectangle ();
		}

		private void opacity_slider_Scroll (object sender, ScrollEventArgs e)
		{
			if (Math.Abs (Convert.ToInt32 (opacity_slider.Value) - opacity) > 3 || unblockedScrool)
			{
				if(img2 != null)
				{
					if(!unblockedScrool) 
						opacity = Convert.ToInt32 (opacity_slider.Value);
					img = Helper.setOpacity (img2, opacity);
					blockedNumeric = true;
					numericUpDown1.Value = opacity;
					blockedNumeric = false;
					sBox.Refresh ();
					localAttributes ["BackgroundImage"] ["opacity"] = opacity.ToString ();

				}
				
				if(unblockedScrool)
					unblockedScrool = false;
			}
		}

		private void select_btn_Click (object sender, EventArgs e)
		{
			export (sender, e);
		}

		private void close_btn_Click (object sender, EventArgs e)
		{
			this.Close ();
		}

		private void initPlugin ()
		{
			export_button.Visible = false;
			string [] files = Directory.GetFiles (Path.Combine (pascalPath, "Highlighting"), "*.xshd");
			if (files.Length > 0)
			{
				foreach (string s in files)
				{
					string sp = GetNameOfTheme (s);
					if (schemes.Items.Contains (sp))
					{
						// Console.WriteLine(nod.Attributes ["name"].Value);
						schemes.SelectedItem = sp;
					}
				}
			}
		}

		private void import_directory_Click (object sender, EventArgs e)
		{
			CommonOpenFileDialog co = new CommonOpenFileDialog ();
			co.IsFolderPicker = true;
			co.Multiselect = false;
			CommonFileDialogResult res = co.ShowDialog ();
			if (res == CommonFileDialogResult.Ok)
			{
				MessageBox.Show (
					"Hi. You have selected the directory, so you will have to wait until the import is done. Your current theme will be changed" +
					"on finishing import.");
				string [] fls = Directory.GetFiles (co.FileName, "*.json");
				foreach (string fl in fls)
				{
					MainParser.Parse (fl, this, false, false);
				}
				fls = Directory.GetFiles (co.FileName, "*.icls");
				foreach (string fl in fls)
				{
					MainParser.Parse (fl, this, false, false);
				}

				schemes.SelectedIndex = schemes.Items.Count - 1;
			}
		}

		private void numericUpDown1_ValueChanged (object sender, EventArgs e)
		{
			if (!blockedNumeric)
			{
				if(img2 != null)
				{
					opacity = Convert.ToInt32 (numericUpDown1.Value);
					unblockedScrool = true;
					opacity_slider.Value = numericUpDown1.Value;
					img = Helper.setOpacity (img2, opacity);
					sBox.Refresh ();
					localAttributes ["BackgroundImage"] ["opacity"] = opacity.ToString ();
				}
			}
		}

		private string GetNameOfTheme (string sps)
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
		
	}
	

	public delegate void SetTheme ();


	public delegate void ColorUpdate (Color bg, Color fg, Color bgClick);

	internal class EllipseStyle : Style
	{
		public override void Draw (Graphics gr, Point position, Range range)
		{
			//get size of rectangle
			var size = GetSizeOfRange (range);
			//create rectangle
			var rect = new Rectangle (position, size);
			//inflate it
			rect.Inflate (2, 2);
			//get rounded rectangle
			var path = GetRoundedRectangle (rect, 10);
			//draw rounded rectangle
			gr.DrawPath (Pens.Red, path);
		}
	}
}