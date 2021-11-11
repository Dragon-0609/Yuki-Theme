using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using FastColoredTextBoxNS;
using Yuki_Theme.Database;
using Yuki_Theme.Themes;

namespace Yuki_Theme.Forms
{
	public partial class MForm : Form
	{
		private readonly ColorPicker     col;
		private readonly Highlighter     highlighter;
		private          int             actionChoice;
		private          bool            askChoice;
		private          bool            update;
		private          bool            blocked;
		private          string          currentFile = "N|L";
		private          DatabaseManager database;
		private          int             lastIndex = 1;

		public Dictionary <string, Dictionary <string, string>> localAttributes;

		private string          pascalPath = "empty";
		public  Populater       populater;
		private bool            selectActive;
		public  string          selectedItem = "empty";
		public  SelectionForm   selform;
		private SettingsForm    setform;
		public  int             settingMode;
		private ThemeManager    tmanagerform;
		private JetBrainsParser jetparser;
		public DownloadForm            df;
		public NotificationForm        nf;

		public MForm ()
		{
			InitializeComponent ();
			connectAndGet ();
			localAttributes = new Dictionary <string, Dictionary <string, string>> ();
			highlighter = new Highlighter (sBox, this);
			populater = new Populater ();
			jetparser = new JetBrainsParser ();
			load_schemes ();
			if (currentFile != "N|L")
			{
				// ASpopulateList ();
				col = new ColorPicker (this);
				highlighter.InitializeSyntax ();
				button1.Visible = !isDefault ();
				AddEvents ();
				if(update)
					button7_Click (this, EventArgs.Empty);
				// df = new DownloadForm (this);
				// nf = new NotificationForm ();
			} else
			{
				throw new ApplicationException ("Error on loading the scheme file");
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
			}
		}

		private void saveList ()
		{
			if (!isDefault ())
			{
				var doc = new XmlDocument ();
				doc.Load ($"Themes/{currentFile}.yukitheme");

				#region Environment

				var node = doc.SelectSingleNode ("/SyntaxDefinition/Environment");

				foreach (XmlNode childNode in node.ChildNodes)
					if (childNode.Attributes != null &&
					    !string.Equals (childNode.Name, "Delimiters", StringComparison.Ordinal))
					{
						var nms = childNode.Name;
						if (childNode.Name == "Span" || childNode.Name == "KeyWords")
							nms = childNode.Attributes ["name"].Value;
						if (!localAttributes.ContainsKey (nms)) continue;

						var attrs = localAttributes [nms];

						foreach (var att in attrs)
							// Console.WriteLine($"N: {childNode.Name}, ATT: {att.Key},");
							childNode.Attributes [att.Key].Value = att.Value;
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

				doc.Save ($"Themes/{currentFile}.yukitheme");

				/*
				Dictionary <string, Dictionary <string, string>> dis =
					new Dictionary <string, Dictionary <string, string>> ();
				Dictionary <string, string> asd = new Dictionary <string, string> ();
				asd.Add ("color", "Green");
				dis.Add ("Default", asd);
				
				var doc = new XmlDocument ();
				doc.Load ("Test.xml");
	
				XmlNode sa = doc.SelectSingleNode ("/AA/Colors");
				Console.WriteLine(sa == null);
				foreach (XmlNode child in sa.ChildNodes)
				{
					foreach (KeyValuePair<string,Dictionary<string,string>> pair in dis)
					{
						foreach (KeyValuePair<string,string> valuePair in pair.Value)
						{
							if (child.Name == pair.Key && child.Attributes[valuePair.Key] != null)
							{
								child.Attributes [valuePair.Key].Value = valuePair.Value;
							}						
						}
					}
					
				}
				
				doc.Save ("Test.xml");*/
			}
		}

		private void populateList (bool inited = false)
		{
			var doc = new XmlDocument ();
			if (isDefault ())
			{
				var a = Assembly.GetExecutingAssembly ();
				doc.Load (a.GetManifestResourceStream ($"Yuki_Theme.Themes.{currentFile}.yukitheme"));
			} else
			{
				doc.Load ($"Themes/{currentFile}.yukitheme");
			}

			PopulateByXMLNode (doc.SelectNodes ("/SyntaxDefinition/Environment") [0]);
			PopulateByXMLNodeSingular (doc.SelectNodes ("/SyntaxDefinition/Digits") [0]);
			PopulateByXMLNodeParent (doc.SelectNodes ("/SyntaxDefinition/RuleSets") [0]);
			if (!inited)
			{
				list_1.SelectedIndex = 0;
				onSelectItem (list_1, EventArgs.Empty);
			}
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
			sBox.Height = ClientSize.Height - 127;
		}

		private void button1_Click (object sender, EventArgs e)
		{
			if (!isDefault ())
				saveList ();
		}
		
		public void button7_Click (object sender, EventArgs e)
		{
			/*if (!isDefault ())
				saveList ();*/
			// showDownloader ();
			if(df == null)
				df = new DownloadForm (this);
			if(nf == null)
				nf = new NotificationForm ();
			// Thread th = new Thread (new ThreadStart(df.CheckUpdate));
			// th.Start();
			df.CheckUpdate ();
		}

		private void button2_Click (object sender, EventArgs e)
		{
			list_1.Items.Clear ();
			localAttributes.Clear ();
			populateList ();
			lastIndex = 151;
			highlighter.updateColors ();
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
			currentFile = schemes.SelectedItem.ToString ();
			button1.Visible = !isDefault ();
			button2_Click (sender, e);
			if (selectActive)
			{
				selectedItem = schemes.SelectedItem.ToString ();
				database.UpdateData (SettingsForm.ACTIVE, selectedItem);
			}
		}

		private void load_schemes ()
		{
			schemes.Items.Clear ();

			schemes.Items.AddRange (DefaultThemes.def);

			if (Directory.Exists ("Themes"))
				foreach (var file in Directory.GetFiles ("Themes/", "*.yukitheme"))
				{
					var pts = Path.GetFileNameWithoutExtension (file);
					if (!DefaultThemes.isDefault (pts))
						schemes.Items.Add (pts);
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
					File.Copy ($"Themes/{selform.comboBox1.SelectedItem}.yukitheme", patsh );
				schemes.Items.Add (selform.textBox1.Text);
				schemes.SelectedItem = selform.textBox1.Text;
			}
		}

		private void button4_Click (object sender, EventArgs e)
		{
			if (setform == null)
				setform = new SettingsForm (this);

			setform.Path = pascalPath;
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
				if (settingMode != st) button2_Click (this, EventArgs.Empty);
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
						defpas = Environment.GetFolderPath (Environment.SpecialFolder.ProgramFilesX86) + "PascalABC.NET";
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

		private void button6_Click (object sender, EventArgs e)
		{
			if (!isDefault ())
				if (MessageBox.Show (this, "Do you want to save current scheme?", "Save",
				                     MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					button1_Click (sender, e);
			if(pascalPath.Length<6)
			{
				MessageBox.Show ((IWin32Window) this, "Please, set path to the PascalABC.NET Direcory.", "Path to the PascalABC.NET Direcory", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				button4_Click (sender, e);
			}
			if(pascalPath.Length>6)
			{
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

						if (result != DialogResult.No)
						{
							if (result == DialogResult.Ignore) CopyFiles (files);

							DeleteFiles (files);
						}
					}
				}

				if (isDefault ())
					CopyFromMemory (currentFile, path);
				else
					File.Copy ($"Themes/{currentFile}.yukitheme", path, true);

				MessageBox.Show (this,
				                 "Your scheme has been exported to the Pascal Directory. Restart PascalABC.NET to activate this scheme",
				                 "Done");
			} else
			{
				MessageBox.Show ((IWin32Window) this,"Export failed, because you didn't set path to the PascalABC.NET directory!", "Export failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

		public void CopyFromMemory (string file, string path)
		{
			var a = Assembly.GetExecutingAssembly ();
			var stream = a.GetManifestResourceStream ($"Yuki_Theme.Themes.{file}.yukitheme");
			using (var fs = new FileStream (path, FileMode.Create))
			{
				stream.Seek (0, SeekOrigin.Begin);
				stream.CopyTo (fs);
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
			var defs = DefaultThemes.def;
			if (tmanagerform == null)
				tmanagerform = new ThemeManager (this);

			var custom = new ListViewGroup ("Custom", HorizontalAlignment.Center);
			tmanagerform.scheme.Items.Clear ();
			tmanagerform.scheme.Groups.Clear ();
			tmanagerform.scheme.Groups.Add (custom);
			foreach (string item in schemes.Items)
			{
				ListViewItem litem;
				if (defs.Contains (item))
					litem = new ListViewItem (item);
				else
					litem = new ListViewItem (item, custom);

				tmanagerform.scheme.Items.Add (litem);
			}

			tmanagerform.ShowDialog ();
		}

		public bool isDefault ()
		{
			return DefaultThemes.isDefault (currentFile);
		}

		private void AddEvents ()
		{
			button1.MouseHover += (sender, args) =>
			{
				tip.Show ("Save", button1);
			};
			button1.MouseLeave += (sender, args) =>
			{
				if (tip.Active)
					tip.Hide (button1);
			};

			button2.MouseHover += (sender, args) =>
			{
				tip.Show ("Restore", button2);
			};
			button2.MouseLeave += (sender, args) =>
			{
				if (tip.Active)
					tip.Hide (button2);
			};

			button4.MouseHover += (sender, args) =>
			{
				tip.Show ("Settings", button4);
			};
			button4.MouseLeave += (sender, args) =>
			{
				if (tip.Active)
					tip.Hide (button4);
			};

			button6.MouseHover += (sender, args) =>
			{
				tip.Show ("Export", button6);
			};
			button6.MouseLeave += (sender, args) =>
			{
				if (tip.Active)
					tip.Hide (button6);
			};

			button3.MouseHover += (sender, args) =>
			{
				tip.Show ("Add New Scheme", button3);
			};
			button3.MouseLeave += (sender, args) =>
			{
				if (tip.Active)
					tip.Hide (button3);
			};

			button5.MouseHover += (sender, args) =>
			{
				tip.Show ("Manage Themes", button5);
			};
			button5.MouseLeave += (sender, args) =>
			{
				if (tip.Active)
					tip.Hide (button5);
			};
		}

		private void import_Click (object sender, EventArgs e)
		{
			OpenFileDialog fd = new OpenFileDialog ();
			fd.DefaultExt = "icls";
			fd.Title = "Import";
			fd.InitialDirectory = Path.GetDirectoryName (Application.ExecutablePath);
			fd.Filter = "JetBrains IDE Scheme(*.icls)|*.icls|Yuki Theme(*.yukitheme)|*.yukitheme";
			fd.Multiselect = false;
			if (fd.ShowDialog () == DialogResult.OK)
			{
				if (fd.FileName.EndsWith (".yukitheme", StringComparison.Ordinal))
				{
					string st = Path.GetFileNameWithoutExtension (fd.FileName);
					string path = $"Themes/{st}.yukitheme";;
					bool wants = true;
					if(File.Exists (path))
					{
						wants = false;
						if (MessageBox.Show ((IWin32Window) this, "Theme is already exist. Do you want to override?",
						                     "Theme is exist",
						                     MessageBoxButtons.YesNo) == DialogResult.Yes) wants = true;

					}
					if(wants)
					{
						File.Copy (fd.FileName, path, true);
						load_schemes ();
					}
				} else
				{
					jetparser.Parse (fd.FileName, this);
				}
			}
		}

		public void showDownloader ()
		{
			if (df == null || nf.IsDisposed)
				df = new DownloadForm (this);
			df.Show(this);
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
			if(df != null && !df.IsDisposed && df.Visible)
			{
				df.StartPosition = FormStartPosition.Manual;
				df.Location = new Point (this.Location.X + this.ClientRectangle.Width - 284, this.Location.Y + this.ClientRectangle.Height - 73);
			}
		}
		
		public void changeNotificationLocation ()
		{
			if(nf != null && !nf.IsDisposed && nf.Visible)
			{
				nf.StartPosition = FormStartPosition.Manual;
				nf.Location = new Point (this.Location.X + this.ClientRectangle.Width - 284, this.Location.Y + this.ClientRectangle.Height - 96);
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
			if(nf != null)
				nf.Dispose ();
			if(df != null)
				df.Dispose ();
			this.Dispose(true);
		}
	}

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