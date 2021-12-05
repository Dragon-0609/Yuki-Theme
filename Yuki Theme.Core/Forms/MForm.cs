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
		private readonly ColorPicker col;
		private readonly Highlighter highlighter;

		private bool            blocked;
		private bool            blockedNumeric;
		private bool            unblockedScrool;
		

		private DatabaseManager database => CLI.database;
		private int             lastIndex = 1;

		#region CLI Fields
		
		private int actionChoice
		{
			get { return CLI.actionChoice; }
			set { CLI.actionChoice = value; }
		}

		private bool askChoice
		{
			get => CLI.askChoice;
			set => CLI.askChoice = value;
		}

		private bool update
		{
			get => CLI.update;
			set => CLI.update = value;
		}
		private string currentFile
		{
			get => CLI.currentFile;
			set => CLI.currentFile = value;
		}

		private string currentoFile
		{
			get => CLI.currentoFile;
			set => CLI.currentoFile = value;
		}
		public Dictionary <string, Dictionary <string, string>> localAttributes
		{
			get => CLI.localAttributes;
			set => CLI.localAttributes = value;
		}

		private string pascalPath
		{
			get => CLI.pascalPath;
			set => CLI.pascalPath = value;
		}

		private bool bgImage
		{
			get => CLI.bgImage;
			set => CLI.bgImage = value;
		}
		
		private bool swSticker
		{
			get => CLI.swSticker;
			set => CLI.swSticker = value;
		}
		
		private bool swStatusbar
		{
			get => CLI.swStatusbar;
			set => CLI.swStatusbar = value;
		}

		public string selectedItem
		{
			get => CLI.selectedItem;
			set => CLI.selectedItem = value;
		}
		
		private Alignment align
		{
			get => CLI.align;
			set => CLI.align = value;
		}

		private int opacity
		{
			get => CLI.opacity;
			set => CLI.opacity = value;
		}
		
		private int sopacity
		{
			get => CLI.sopacity;
			set => CLI.sopacity = value;
		}

		#endregion

		public  SelectionForm    selform;
		private SettingsForm     setform;
		public  int              settingMode;
		private ThemeManager     tmanagerform;
		public  DownloadForm     df;
		public  NotificationForm nf;
		private Image            img  = null;
		private Image            img2 = null;
		private Image            img3 = null;
		private Image            img4 = null;

		private string bgtext;
		private string sttext;

		private Rectangle        oldV = Rectangle.Empty;
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

		public  Brush         fgbrush;
		private int           textBoxHeight = 0;
		private int           notHeight     = 0;
		private int           imgCurrent    = 0;
		private CustomPicture stickerControl;


		public MForm (int mode = 0)
		{
			Helper.mode = (ProductMode) mode;
			textBoxHeight =  Helper.mode == ProductMode.Program ? 140 : 178;
			notHeight =  Helper.mode == ProductMode.Program ? 50 : 88;
			InitializeComponent ();
			
			CLI.AskChoice = AskChoice;
			CLI.SaveInExport = SaveInExport;
			CLI.FinishExport = FinishExport;
			CLI.ErrorExport = ErrorExport;
			CLI.setPath = setPath;
			CLI.hasProblem = hasProblem;
			CLI.ifHasImage = ifHasImage;
			CLI.ifDoesntHave = ifDoesntHave;
			CLI.ifHasSticker = ifHasSticker;
			CLI.ifDoesntHaveSticker = ifDoesntHaveSticker;
			Helper.GiveMessage = GiveMessage;
			
			if(Helper.mode != ProductMode.Plugin)
				CLI.connectAndGet ();
			initSticker ();
			
			highlighter = new Highlighter (sBox, this);
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
				if (!str.Contains ("Image") && !str.Contains ("Sticker"))
				{
					if (!colorEditor.Visible)
					{
						colorEditor.Visible = true;
						imageEditor.Visible = false;
					}

					if (Highlighter.isInNames (str) && !isDefault ())
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

					imgCurrent = 2;
					alignpanel.Enabled = false;
					imgCurrent = 0;

					
					if(!str.Contains ("Sticker"))
					{
						alignpanel.Enabled = true;
						imgCurrent = 1;
						align = (Alignment) (int.Parse (localAttributes [str] ["align"]));
						imagePath.Text = bgtext;
						opacity = int.Parse (localAttributes [str] ["opacity"]);
						blockedNumeric = true;
						numericUpDown1.Value = opacity;
						blockedNumeric = false;
						imgCurrent = 1;
						unblockedScrool = true;
						opacity_slider.Value = opacity;
						
						updateAlignButton ();
					} else
					{
						imagePath.Text = sttext;
						sopacity = int.Parse (localAttributes [str] ["opacity"]);
						blockedNumeric = true;
						numericUpDown1.Value = sopacity;
						blockedNumeric = false;
						imgCurrent = 2;
						unblockedScrool = true;
						opacity_slider.Value = sopacity;
						
					}

					imageEditor.Enabled = !isDefault ();


				}
			}
		}

		private void initSticker ()
		{
			stickerControl = new CustomPicture ();
			stickerControl.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			if (Helper.mode == ProductMode.Plugin)
				stickerControl.margin = new Point (10, bottomPanel.Size.Height);
			else
				stickerControl.margin = new Point (10, 0);
			Controls.Add (stickerControl);
			stickerControl.Enabled = false;
			stickerControl.BringToFront ();
		}
		
		private void LoadSticker ()
		{
			if (img3 != null && swSticker)
			{
				if (sopacity != 100)
					img4 = Helper.setOpacity (img3, sopacity);
				else
					img4 = img3;
				stickerControl.Visible = true;
			}
			else
			{
				img4 = null;
				stickerControl.Visible = false;
			}
			
			stickerControl.img = img4;
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
				var sttr = Populater.getDependencies (str);
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
			CLI.save (img2, img3);
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

		public void ifHasImage (Image imgc)
		{
			img2 = imgc;
			oldV = Rectangle.Empty;
			bgtext = "background.png";
		}
		public void ifDoesntHave ()
		{
			img = null;
			img2 = null;
			bgtext = "";
		}
		
		public void ifHasSticker (Image imgc)
		{
			img3 = imgc;
			sttext = "background.png";
		}
		public void ifDoesntHaveSticker ()
		{
			img3 = null;
			img4 = null;
			sttext = "";
		}
		
		public void onSelect ()
		{
			list_1.Items.AddRange (CLI.names.ToArray ());
			Console.WriteLine(list_1.Items.Count);
			list_1.SelectedIndex = 0;
			onSelectItem (list_1, EventArgs.Empty);
		}

		public void hasProblem (string content)
		{
			MessageBox.Show (
				content,
				"Theme file is invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
			schemes.SelectedIndex = 0;
		}
		
		public void GiveMessage (string content)
		{
			MessageBox.Show (content);
		}

		private void restore_Click (object sender, EventArgs e)
		{
			list_1.Items.Clear ();
			CLI.restore (false, onSelect);
			lastIndex = 151;
			highlighter.updateColors ();
			updateAlignButton ();
			updateBackgroundColors ();
			unblockedScrool = true;

			imgCurrent = 1;
			opacity_slider_Scroll (
				sender, new ScrollEventArgs (ScrollEventType.ThumbPosition, opacity));
			imgCurrent = 2;
			unblockedScrool = true;
			opacity_slider_Scroll (
				sender, new ScrollEventArgs (ScrollEventType.ThumbPosition, sopacity));
			imgCurrent = 0;
			
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
			Console.WriteLine (currentoFile);
			currentFile = currentoFile.Replace (": ", "__").Replace (":","");
			save_button.Visible = !isDefault ();
			restore_Click (sender, e);
			
			selectedItem = schemes.SelectedItem.ToString ();
			database.UpdateData (SettingsForm.ACTIVE, selectedItem);
			
		}

		public string ifZero ()
		{
			string res = null;
			var op = new OpenFileDialog ();
			op.DefaultExt = "yukitheme";
			op.Filter = "Yuki Theme(*.yukitheme)|*.yukitheme";
			op.Multiselect = false;
			if (op.ShowDialog () == DialogResult.OK)
				res = op.FileName;
			return res;
		}

		public void load_schemes ()
		{
			schemes.Items.Clear ();

			CLI.load_schemes (ifZero);

			schemes.Items.AddRange (CLI.schemes.ToArray ());

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
					CLI.CopyFromMemory (syt, patsh);
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
			setform.bgImage = bgImage;
			setform.Sticker = swSticker;
			setform.StatusBar = swStatusbar;
			setform.askC.Checked = askChoice;
			setform.checkBox2.Checked = update;
			setform.ActionBox.SelectedIndex = actionChoice;
			setform.mode.SelectedIndex = settingMode;
			setform.swStatusbar.Enabled = Helper.mode == ProductMode.Plugin;

			var st = settingMode;
			if (setform.ShowDialog () == DialogResult.OK)
			{
				pascalPath = setform.Path;
				bgImage = setform.bgImage;
				swSticker = setform.Sticker;
				swStatusbar = setform.StatusBar;
				askChoice = setform.askC.Checked;
				update = setform.checkBox2.Checked;
				actionChoice = setform.ActionBox.SelectedIndex;
				settingMode = setform.mode.SelectedIndex;
				CLI.saveData ();
				sBox.Refresh ();
				LoadSticker ();
				if (settingMode != st) restore_Click (this, EventArgs.Empty);
			}
		}

		public bool SaveInExport (string content, string title)
		{
			return MessageBox.Show (content, title,
			                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
		}
		
		public void FinishExport (string content, string title)
		{
			MessageBox.Show (content, title);
		}
		
		public void ActionInExport (string content, string title)
		{
			MessageBox.Show (content, title);
		}
		
		public void ErrorExport (string content, string title)
		{
			MessageBox.Show (content, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		
		public DialogResult AskChoice ()
		{
			var quform = new QuestionForm ();
			return quform.ShowDialog ();
		}

		public void setPath (string content, string title)
		{
			MessageBox.Show (content, title, MessageBoxButtons.OK,
			                 MessageBoxIcon.Exclamation);
			button4_Click (this, EventArgs.Empty);
		}

		public void startSettingThemeDelegate ()
		{
			if (startSettingTheme != null) startSettingTheme ();
		}
		
		public void setThemeDelegate ()
		{
			if (setTheme != null) setTheme ();
		}

		private void export (object sender, EventArgs e)
		{
			CLI.export (img2, img3, setThemeDelegate, startSettingThemeDelegate);
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
			if (img != null && bgImage)
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
			if (imgCurrent == 1)
			{
				bgtext = imagePath.Text;
			}else if (imgCurrent == 2)
			{
				sttext = imagePath.Text;
			}
			
			if (imagePath.Text == "background.png")
			{
				if(imgCurrent == 1)
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
				} else if (imgCurrent == 2)
				{
					Tuple <bool, Image> iag = isDefault ()
						? Helper.getStickerFromMemory (gp, Assembly.GetExecutingAssembly ())
						: Helper.getSticker (getPath);
					if (iag.Item1)
					{
						// img = iag.Item2;
						img3 = iag.Item2;
					}

					unblockedScrool = true;
					opacity_slider_Scroll (sender, new ScrollEventArgs (ScrollEventType.ThumbPosition, sopacity));
				}
			} else if (File.Exists (imagePath.Text))
			{
				if (imagePath.Text.ToLower ().EndsWith (".png"))
				{
					if(imgCurrent == 1)
					{
						img2 = Image.FromFile (imagePath.Text);

						unblockedScrool = true;
						opacity_slider_Scroll (sender, new ScrollEventArgs (ScrollEventType.ThumbPosition, opacity));
					} else if (imgCurrent == 2)
					{
						img3 = Image.FromFile (imagePath.Text);

						if (img3.Width > 400 || img3.Height > 400)
						{
							if (MessageBox.Show (
								    "The image is very big. It will be displayed in original size. Are you sure?",
								    "Image is very big", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
							    DialogResult.No)
							{
								button7_Click (sender, e);
								return;
							}
						}
						
						unblockedScrool = true;
						opacity_slider_Scroll (sender, new ScrollEventArgs (ScrollEventType.ThumbPosition, sopacity));
					}
				} else
				{
					MessageBox.Show ("File format must be .png!", "Invalid format", MessageBoxButtons.OK,
					                 MessageBoxIcon.Error);
				}
			} else if (imagePath.Text.Length < 2)
			{
				if(imgCurrent == 1)
				{
					img = null;
					img2 = null;
					sBox.Refresh ();
				} else if (imgCurrent == 2)
				{
					img3 = null;
					LoadSticker ();
				}
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
				if(imgCurrent == 1)
				{
					if (img2 != null)
					{
						if (!unblockedScrool)
							opacity = Convert.ToInt32 (opacity_slider.Value);
						img = Helper.setOpacity (img2, opacity);
						blockedNumeric = true;
						numericUpDown1.Value = opacity;
						blockedNumeric = false;
						sBox.Refresh ();
						localAttributes ["BackgroundImage"] ["opacity"] = opacity.ToString ();

					}
				} else if (imgCurrent == 2)
				{
					if (img3 != null)
					{
						if (!unblockedScrool)
							sopacity = Convert.ToInt32 (opacity_slider.Value);
						blockedNumeric = true;
						numericUpDown1.Value = sopacity;
						blockedNumeric = false;
						localAttributes ["Sticker"] ["opacity"] = sopacity.ToString ();
					}
					LoadSticker ();
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
				
				if(imgCurrent == 1)
				{
					if(img2 != null)
					{
						opacity = Convert.ToInt32 (numericUpDown1.Value);
						unblockedScrool = true;
						opacity_slider.Value = numericUpDown1.Value;
						// img = Helper.setOpacity (img2, opacity);
						// sBox.Refresh ();
						// localAttributes ["BackgroundImage"] ["opacity"] = opacity.ToString ();
					}
				} else if (imgCurrent == 2)
				{
					if(img3 != null)
					{
						sopacity = Convert.ToInt32 (numericUpDown1.Value);
						unblockedScrool = true;
						opacity_slider.Value = numericUpDown1.Value;
						// img = Helper.setOpacity (img2, opacity);
						// sBox.Refresh ();
						// localAttributes ["Sticker"] ["opacity"] = opacity.ToString ();
					}
				}
				
				
			}
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