using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Svg;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.Database;
using Yuki_Theme.Core.Parsers;
using Yuki_Theme.Core.Themes;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace Yuki_Theme.Core.Forms
{
	public partial class MForm : Form
	{
		private readonly ColorPicker col;
		private readonly Highlighter highlighter;

		private bool blocked;
		private bool blockedNumeric;
		private bool unblockedScrool;


		private DatabaseManager database => Settings.database;
		private int             lastIndex = 1;

		#region CLI Fields

		private bool askChoice
		{
			get => Settings.askChoice;
			set => Settings.askChoice = value;
		}

		private bool update
		{
			get => Settings.update;
			set => Settings.update = value;
		}

		private string currentFile
		{
			get => CLI.nameToLoad;
			set => CLI.nameToLoad = value;
		}

		private string currentoFile
		{
			get => CLI.pathToLoad;
			set => CLI.pathToLoad = value;
		}

		public Dictionary <string, ThemeField> localAttributes
		{
			get => CLI.currentTheme.Fields;
			set => CLI.currentTheme.Fields = value;
		}

		private string pascalPath
		{
			get => Settings.pascalPath;
			set => Settings.pascalPath = value;
		}

		private bool bgImage
		{
			get => Settings.bgImage;
			set => Settings.bgImage = value;
		}

		private bool swSticker
		{
			get => Settings.swSticker;
			set => Settings.swSticker = value;
		}

		private bool useCustomSticker
		{
			get => Settings.useCustomSticker;
			set => Settings.useCustomSticker = value;
		}

		private string customSticker
		{
			get => Settings.customSticker;
			set => Settings.customSticker = value;
		}

		private bool swLogo
		{
			get => Settings.swLogo;
			set => Settings.swLogo = value;
		}

		private bool Editor
		{
			get => Settings.Editor;
			set => Settings.Editor = value;
		}

		private bool Beta
		{
			get => Settings.Beta;
			set => Settings.Beta = value;
		}

		private bool Logged
		{
			get => Settings.Logged;
			set => Settings.Logged = value;
		}

		private bool swStatusbar
		{
			get => Settings.swStatusbar;
			set => Settings.swStatusbar = value;
		}

		public string selectedItem
		{
			get => CLI.selectedItem;
			set => CLI.selectedItem = value;
		}

		private Alignment align
		{
			get => CLI.currentTheme.align;
			set => CLI.currentTheme.WallpaperAlign = (int)value;
		}

		private SettingMode settingMode
		{
			get => Settings.settingMode;
			set => Settings.settingMode = value;
		}

		#endregion

		public  SelectionForm selform;
		private SettingsForm  setform;

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
		private ImageType    imgCurrent    = ImageType.None;
		public  CustomPicture stickerControl;
		private Timer         tmr;

		public MForm (int mode = 0, bool quiet = false)
		{
			Helper.mode = (ProductMode) mode;                               // Write current type
			textBoxHeight = Helper.mode == ProductMode.Program ? 140 : 178; // This is necessary to change height properly
			notHeight = Helper.mode == ProductMode.Program ? 50 : 88;
			InitializeComponent ();
			list_1.ItemHeight = list_1.Font.Height + 2;
			// Set Actions
			CLI_Actions.AskChoice = AskChoice;
			CLI_Actions.SaveInExport = SaveInExport;
			CLI_Actions.showSuccess = FinishExport;
			CLI_Actions.showError = ErrorExport;
			CLI_Actions.setPath = setPath;
			CLI_Actions.hasProblem = hasProblem;
			CLI_Actions.ifHasImage = ifHasImage;
			CLI_Actions.ifDoesntHave = ifDoesntHave;
			CLI_Actions.ifHasSticker = ifHasSticker;
			CLI_Actions.ifDoesntHaveSticker = ifDoesntHaveSticker;
			Helper.giveMessage = GiveMessage;

			if (Helper.mode != ProductMode.Plugin)
				Settings.connectAndGet (); // Get Data
			initSticker ();

			highlighter = new Highlighter (sBox, this);
			load_schemes ();
			if (Helper.mode == ProductMode.Plugin)
				initPlugin ();

			checkEditor ();
			this.StartPosition = FormStartPosition.Manual; // Set default position for the window
			DesktopLocation = database.ReadLocation ();

			if (currentFile != "N|L") // If theme couldn't find
			{
				col = new ColorPicker (this);
				highlighter.InitializeSyntax ();
				loadSVG ();
				save_button.Visible = !isDefault ();
				AddEvents ();

				sBox.Paint += bgImagePaint;
				if (update && !quiet)
					update_Click (this, EventArgs.Empty);
				MForm_SizeChanged (this, EventArgs.Empty);
				if (Helper.mode != ProductMode.Plugin)
				{
					Controls.Remove (bottomPanel);
				}

				isUpdated ();
				tmr = new Timer ();
				tmr.Interval = 100;
				tmr.Tick += trackInstall;
				tmr.Start ();
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
				new ChangelogForm ().Show (this);
				ke.DeleteValue ("install");
			}
		}

		private void onSelectItem (object sender, EventArgs e)
		{
			if (lastIndex != list_1.SelectedIndex)
			{
				colorButton.Visible = label1.Visible = false;
				bgButton.Visible = label2.Visible = false;
				check_bold.Enabled = false;
				check_italic.Enabled = false;
				lastIndex = list_1.SelectedIndex;
				blocked = true;
				// Console.WriteLine(list_1.SelectedItem.ToString ());
				var str = list_1.SelectedItem.ToString ();
				if (!str.Contains ("Wallpaper") && !str.Contains ("Sticker"))
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

					ThemeField dic = localAttributes [str];
					if (dic.Foreground != null)
					{
						colorButton.BackColor = ColorTranslator.FromHtml (dic.Foreground);
						colorButton.Visible = label1.Visible = true;
					}

					if (dic.Background != null)
					{
						bgButton.BackColor = ColorTranslator.FromHtml (dic.Background);
						bgButton.Visible = label2.Visible = true;
					}
					if (dic.Bold != null){
						check_bold.Checked = (bool) dic.Bold;
					}
					if (dic.Italic != null){
						check_italic.Checked = (bool) dic.Italic;
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

					alignpanel.Enabled = false;
					imgCurrent = ImageType.None;


					if (!str.Contains ("Sticker"))
					{
						alignpanel.Enabled = true;
						imgCurrent = ImageType.Wallpaper;
						align = (Alignment) CLI.currentTheme.WallpaperAlign;
						imagePath.Text = bgtext;
						blockedNumeric = true;
						numericUpDown1.Value = CLI.currentTheme.WallpaperOpacity;
						blockedNumeric = false;
						unblockedScrool = true;
						opacity_slider.Value = CLI.currentTheme.WallpaperOpacity;

						updateAlignButton ();
					} else
					{
						imagePath.Text = sttext;
						blockedNumeric = true;
						numericUpDown1.Value = CLI.currentTheme.StickerOpacity;
						blockedNumeric = false;
						imgCurrent = ImageType.Sticker;
						unblockedScrool = true;
						opacity_slider.Value = CLI.currentTheme.StickerOpacity;
					}

					imageEditor.Enabled = !isDefault ();
				}
			}
		}

		private void initSticker ()
		{
			stickerControl = new CustomPicture (this);
			stickerControl.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			if (Helper.mode == ProductMode.Plugin)
				stickerControl.margin = new Point (10, bottomPanel.Size.Height);
			else
				stickerControl.margin = new Point (10, 0);
			stickerControl.Enabled = Settings.positioning;
			CustomPanel pnl = new CustomPanel (1) {Visible = false, Name = "LayerGrids"};
			stickerControl.pnl = pnl;
			pnl.pict = stickerControl;
			Controls.Add (pnl);
			Controls.Add (stickerControl);
			// stickerControl.Enabled = false;
			stickerControl.BringToFront ();
		}

		private void LoadSticker ()
		{
			if (swSticker)
			{
				// Console.WriteLine (customSticker);
				// Console.WriteLine (File.Exists (customSticker));
				if (useCustomSticker && File.Exists (customSticker))
				{
					img4 = Image.FromFile (customSticker);
				} else
				{
					if (img3 != null)
					{
						if (CLI.currentTheme.StickerOpacity != 100)
							img4 = Helper.SetOpacity (img3, CLI.currentTheme.StickerOpacity);
						else
							img4 = img3;
						stickerControl.Visible = true;
					} else
					{
						img4 = null;
						stickerControl.Visible = false;
					}
				}
				stickerControl.img = img4;
			} else
			{
				stickerControl.Visible = false;
			}
		}

		private void colorButton_Click (object sender, EventArgs e)
		{
			if (!blocked)
			{
				col.MainColor = colorButton.BackColor;
				if (col.ShowDialog (this) == DialogResult.OK)
				{
					if (!CLI.isEdited) CLI.isEdited = colorButton.BackColor != col.MainColor; 
					colorButton.BackColor = col.MainColor;
					updateCurrentItem ();
				}
			}
		}

		private void bgButton_Click (object sender, EventArgs e)
		{
			if (!blocked)
			{
				col.MainColor = bgButton.BackColor;
				if (col.ShowDialog (this) == DialogResult.OK)
				{
					if (!CLI.isEdited) CLI.isEdited = colorButton.BackColor != col.MainColor;
					bgButton.BackColor = col.MainColor;
					updateCurrentItem ();
				}
			}
		}

		private void updateCurrentItem ()
		{
			var str = list_1.SelectedItem.ToString ();
			ThemeField dic = localAttributes [str];
			SetField (ref dic);

			highlighter.updateColors ();
		}

		private void SetField (ref ThemeField dic)
		{
			if (dic.Foreground != null)
				dic.Foreground = Translate (colorButton.BackColor);
			if (dic.Background != null)
				dic.Background = Translate (bgButton.BackColor);
			if (dic.Bold != null)
				dic.Bold = check_bold.Checked;
			if (dic.Italic != null)
				dic.Italic = check_italic.Checked;
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

		public void update_Click (object sender, EventArgs e)
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
			bgtext = "wallpaper.png";
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
			sttext = "sticker.png";
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
			// Console.WriteLine(list_1.Items.Count);
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

			imgCurrent = ImageType.Wallpaper;
			opacity_slider_Scroll (
				sender, new ScrollEventArgs (ScrollEventType.ThumbPosition, CLI.currentTheme.WallpaperOpacity));
			imgCurrent = ImageType.Sticker;
			unblockedScrool = true;
			opacity_slider_Scroll (
				sender, new ScrollEventArgs (ScrollEventType.ThumbPosition, CLI.currentTheme.StickerOpacity));
			imgCurrent = ImageType.None;
			onSelectItem (sender, e);
			GC.Collect ();
			GC.WaitForPendingFinalizers ();
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
			bool cnd = CLI.SelectTheme (schemes.SelectedItem.ToString ());
			
			if (cnd)
			{
				if (CLI.isEdited) // Ask to save the changes
				{
					if (SaveInExport ("Do you want to save the theme?", "Theme is edited"))
						save_Click (sender, e); // save before restoring
				}
				restore_Click (sender, e);
				
				save_button.Visible = !isDefault ();

				selectedItem = schemes.SelectedItem.ToString ();
				database.UpdateData (Settings.ACTIVE, selectedItem);
			}
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

		private void add_Click (object sender, EventArgs e)
		{
			if (selform == null)
				selform = new SelectionForm ();
			selform.textBox1.Text = "";
			selform.comboBox1.Items.Clear ();
			foreach (var item in schemes.Items) selform.comboBox1.Items.Add (item);

			selform.comboBox1.SelectedIndex = 0;

			if (selform.ShowDialog () == DialogResult.OK)
			{
				string copyFrom = selform.comboBox1.SelectedItem.ToString ();
				string toname = selform.textBox1.Text;
				if (!CLI.add (copyFrom, toname))
				{
					schemes.Items.Add (toname);
					schemes.SelectedItem = toname;
				}
			}
		}

		private void settings_Click (object sender, EventArgs e)
		{
			if (setform == null)
				setform = new SettingsForm (this);
			if (Helper.mode == ProductMode.Program)
			{
				setform.Path = pascalPath;
				setform.setVisible (true);
			} else
			{
				setform.setVisible (false);
			}

			bool oldeditor = Editor;
			var st = settingMode;
			if (setform.ShowDialog () == DialogResult.OK)
			{
				pascalPath = setform.Path;
				bgImage = setform.bgImage;
				swSticker = setform.Sticker;
				swLogo = setform.Logo;
				Editor = setform.Editor;
				Beta = setform.Beta;
				swStatusbar = setform.StatusBar;
				askChoice = setform.settingsPanel.askC.Checked;
				update = setform.settingsPanel.checkBox2.Checked;
				settingMode = (SettingMode) setform.settingsPanel.mode.SelectedIndex;
				Settings.positioning = setform.settingsPanel.checkBox3.Checked;
				Settings.unit = (RelativeUnit) setform.settingsPanel.unit.SelectedIndex;
				Settings.showGrids = setform.settingsPanel.checkBox4.Checked;
				Settings.useCustomSticker = setform.settingsPanel.use_cstm_sticker.Checked;
				Settings.customSticker = setform.settingsPanel.customSticker;
				Settings.autoFitByWidth = setform.settingsPanel.fitWidth.Checked;
				Settings.askToSave = setform.settingsPanel.askSave.Checked;
				Settings.saveAsOld = setform.settingsPanel.saveOld.Checked;
				Settings.saveData ();
				sBox.Refresh ();
				stickerControl.Enabled = Settings.positioning;
				LoadSticker ();
				if (oldeditor != Editor) // Check if the Editor is changed
					checkEditor ();
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

		public void ErrorExport (string content, string title)
		{
			MessageBox.Show (content, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public int AskChoice ()
		{
			var quform = new QuestionForm ();
			DialogResult res = quform.ShowDialog ();
			int nm = 0;
			switch (res)
			{
				case DialogResult.Yes :
				{
					nm = 0;
				}
					break;

				case DialogResult.Ignore :
				{
					nm = 1;
				}
					break;

				case DialogResult.No :
				{
					nm = 2;
				}
					break;
			}

			return nm;
		}

		public void setPath (string content, string title)
		{
			MessageBox.Show (content, title, MessageBoxButtons.OK,
			                 MessageBoxIcon.Exclamation);
			settings_Click (this, EventArgs.Empty);
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
				if (CLI.isDefaultTheme [item])
				{
					ReItem cat = DefaultThemes.getCategory (item) == "Doki Theme" ? doki : defa;
					litem = new ReItem (item, false, CLI.oldThemeList [item], cat);
				} else
				{
					litem = new ReItem (item, false, CLI.oldThemeList [item], custom);
				}
			}

			tmanagerform.sortItems ();

			tmanagerform.ShowDialog ();
		}

		public bool isDefault ()
		{
			return CLI.currentTheme.isDefault;
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
				"All Themes(*.icls,*.yukitheme,*.yuki,*.json)|*.icls;*.yukitheme;*.yuki;*.json|JetBrains IDE Scheme(*.icls)|*.icls|Yuki Theme(*.yukitheme,*.yuki)|*.yukitheme;*.yuki|Doki Theme(*.json)|*.json";
			fd.Multiselect = false;
			if (fd.ShowDialog () == DialogResult.OK)
			{
				MainParser.Parse (fd.FileName, this, true, true, ErrorExport, AskChoiceParser);
			}
		}

		private bool AskChoiceParser (string content, string title)
		{
			return MessageBox.Show (content,
			                        title,
			                        MessageBoxButtons.YesNo) == DialogResult.Yes;
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
			database.SaveLocation (DesktopLocation);
			CLI_Actions.ifHasImage = null;
			CLI_Actions.ifDoesntHave = null;
			CLI_Actions.ifHasSticker = null;
			CLI_Actions.ifDoesntHaveSticker = null;
			this.Dispose (true);
		}

		private void button11_Click (object sender, EventArgs e)
		{
			var op = new OpenFileDialog ();
			op.DefaultExt = "png";
			// op.Filter = "PNG (*.png)|*.png|GIF (*.gif)|*.gif";
			op.Filter = "PNG (*.png)|*.png";
			op.Multiselect = false;
			if (op.ShowDialog () == DialogResult.OK)
			{
				imagePath.Text = op.FileName;
				Apply_Click (sender, e);
			}
		}

		private void pleft_Click (object sender, EventArgs e)
		{
			if (align != Alignment.Left)
			{
				align = Alignment.Left;
			}
			updateAlignButton ();
		}

		private void pcenter_Click (object sender, EventArgs e)
		{
			if (align != Alignment.Center)
			{
				align = Alignment.Center;
			}
			updateAlignButton ();
		}

		private void pright_Click (object sender, EventArgs e)
		{
			if (align != Alignment.Right)
			{
				align = Alignment.Right;
			}
			updateAlignButton ();
		}

		private void bgImagePaint (object sender, PaintEventArgs e)
		{
			if (img != null && bgImage)
			{
				if (oldV.Width != sBox.ClientRectangle.Width || oldV.Height != sBox.ClientRectangle.Height)
				{
					oldV = Helper.GetSizes (img.Size, sBox.ClientRectangle.Width, sBox.ClientRectangle.Height,
					                        align);
				}

				e.Graphics.DrawImage (img, oldV);
			}
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

		private void Apply_Click (object sender, EventArgs e)
		{
			if (imgCurrent == ImageType.Wallpaper)
			{
				bgtext = imagePath.Text;
			} else if (imgCurrent == ImageType.Sticker)
			{
				sttext = imagePath.Text;
			}

			if (imagePath.Text == "wallpaper.png" && imgCurrent == ImageType.Wallpaper)
			{
				Tuple <bool, Image> iag = isDefault ()
					? Helper.GetImageFromMemory (CLI.currentTheme.fullPath, Assembly.GetExecutingAssembly ())
					: Helper.GetImage (CLI.currentTheme.fullPath);
				if (iag.Item1)
				{
					// img = iag.Item2;
					img2 = iag.Item2;
					oldV = Rectangle.Empty;
				}

				unblockedScrool = true;
				opacity_slider_Scroll (sender, new ScrollEventArgs (ScrollEventType.ThumbPosition, CLI.currentTheme.WallpaperOpacity));
			} else if (imagePath.Text == "sticker.png" && imgCurrent == ImageType.Sticker)
			{
				Tuple <bool, Image> iag = isDefault ()
					? Helper.GetStickerFromMemory (CLI.currentTheme.fullPath, Assembly.GetExecutingAssembly ())
					: Helper.GetSticker (CLI.currentTheme.fullPath);
				if (iag.Item1)
				{
					// img = iag.Item2;
					img3 = iag.Item2;
				}

				unblockedScrool = true;
				opacity_slider_Scroll (sender, new ScrollEventArgs (ScrollEventType.ThumbPosition, CLI.currentTheme.StickerOpacity));
			} else if (File.Exists (imagePath.Text))
			{
				if (imagePath.Text.ToLower ().EndsWith (".png"))
				{
					if (imgCurrent == ImageType.Wallpaper)
					{
						img2 = Image.FromFile (imagePath.Text);

						unblockedScrool = true;
						opacity_slider_Scroll (sender, new ScrollEventArgs (ScrollEventType.ThumbPosition, CLI.currentTheme.WallpaperOpacity));
					} else if (imgCurrent == ImageType.Sticker)
					{
						img3 = Image.FromFile (imagePath.Text);

						if (img3.Width > 400 || img3.Height > 400)
						{
							if (MessageBox.Show (
								    "The image is very big. It will be displayed in original size. Are you sure?",
								    "Image is very big", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
							    DialogResult.No)
							{
								restore_Click (sender, e);
								return;
							}
						}

						unblockedScrool = true;
						opacity_slider_Scroll (sender, new ScrollEventArgs (ScrollEventType.ThumbPosition, CLI.currentTheme.StickerOpacity));
					}
				} else
				{
					MessageBox.Show ("File format must be .png!", "Invalid format", MessageBoxButtons.OK,
					                 MessageBoxIcon.Error);
				}
			} else if (imagePath.Text.Length < 2)
			{
				if (imgCurrent == ImageType.Wallpaper)
				{
					img = null;
					img2 = null;
					sBox.Refresh ();
				} else if (imgCurrent == ImageType.Sticker)
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
			Apply_Click (sender, e);
		}

		private void loadSVG ()
		{
			var a = Assembly.GetExecutingAssembly ();

			string add = Helper.IsDark (Helper.bgColor) ? "" : "_dark";
			
			Helper.RenderSvg (save_button, Helper.LoadSvg ("menu-saveall" + add, a));
			Helper.RenderSvg (restore_button, Helper.LoadSvg ("refresh" + add, a));
			Helper.RenderSvg (export_button, Helper.LoadSvg ("export" + add, a));
			Helper.RenderSvg (import_button, Helper.LoadSvg ("import" + add, a));
			Helper.RenderSvg (settings_button, Helper.LoadSvg ("gearPlain" + add, a), false, Size.Empty, true, Helper.bgBorder);
			Helper.RenderSvg (add_button, Helper.LoadSvg ("add" + add, a));
			Helper.RenderSvg (manage_button, Helper.LoadSvg ("listFiles" + add, a));
			Helper.RenderSvg (import_directory, Helper.LoadSvg ("traceInto" + add, a));
			Helper.RenderSvg (button11, Helper.LoadSvg ("moreHorizontal" + add, a), true, new Size (16, 16));
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
			int opac = imgCurrent == ImageType.Wallpaper ? CLI.currentTheme.WallpaperOpacity : CLI.currentTheme.StickerOpacity;
			if (Math.Abs (Convert.ToInt32 (opacity_slider.Value) - opac) > 3 || unblockedScrool)
			{
				if (imgCurrent == ImageType.Wallpaper)
				{
					if (img2 != null)
					{
						if (!unblockedScrool)
							CLI.currentTheme.WallpaperOpacity = Convert.ToInt32 (opacity_slider.Value);
						img = Helper.SetOpacity (img2, CLI.currentTheme.WallpaperOpacity);
						blockedNumeric = true;
						numericUpDown1.Value = CLI.currentTheme.WallpaperOpacity;
						blockedNumeric = false;
						sBox.Refresh ();
					}
				} else if (imgCurrent == ImageType.Sticker)
				{
					if (img3 != null)
					{
						if (!unblockedScrool)
							CLI.currentTheme.StickerOpacity = Convert.ToInt32 (opacity_slider.Value);
						blockedNumeric = true;
						numericUpDown1.Value = CLI.currentTheme.StickerOpacity;
						blockedNumeric = false;
					}

					LoadSticker ();
				}

				if (unblockedScrool)
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
					MainParser.Parse (fl, this, false, false, ErrorExport, AskChoiceParser);
				}

				fls = Directory.GetFiles (co.FileName, "*.icls");
				foreach (string fl in fls)
				{
					MainParser.Parse (fl, this, false, false, ErrorExport, AskChoiceParser);
				}

				schemes.SelectedIndex = schemes.Items.Count - 1;
			}
		}

		private void numericUpDown1_ValueChanged (object sender, EventArgs e)
		{
			if (!blockedNumeric)
			{
				if (imgCurrent == ImageType.Wallpaper)
				{
					if (img2 != null)
					{
						CLI.currentTheme.WallpaperOpacity = Convert.ToInt32 (numericUpDown1.Value);
						unblockedScrool = true;
						opacity_slider.Value = numericUpDown1.Value;
						// img = Helper.setOpacity (img2, opacity);
						// sBox.Refresh ();
						// localAttributes ["BackgroundImage"] ["opacity"] = opacity.ToString ();
					}
				} else if (imgCurrent == ImageType.Sticker)
				{
					if (img3 != null)
					{
						CLI.currentTheme.StickerOpacity = Convert.ToInt32 (numericUpDown1.Value);
						unblockedScrool = true;
						opacity_slider.Value = numericUpDown1.Value;
						// img = Helper.setOpacity (img2, opacity);
						// sBox.Refresh ();
						// localAttributes ["Sticker"] ["opacity"] = opacity.ToString ();
					}
				}
			}
		}

		private void checkEditor ()
		{
			import_directory.Visible = import_button.Visible =
				editorpanel.Visible = editorp2.Visible = list_1.Visible = Editor;
			if (Editor)
			{
				panel1.Size = new Size (panel1.Width, 140);
				textBoxHeight = Helper.mode == ProductMode.Program ? 140 : 178;
			} else
			{
				panel1.Size = new Size (panel1.Width, 51);
				textBoxHeight = Helper.mode == ProductMode.Program ? 51 : 89;
			}

			MForm_SizeChanged (this, EventArgs.Empty);
		}

		/// <summary>
		/// The app will track install via Google Analytics. This is necessary for me to be kept inspired. I'll switch to passive development on 1st April of 2022, but it doesn't mean that I'll close project. The installs are necessary to make me inspired for continue the development. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void trackInstall (object sender, EventArgs e)
		{
			tmr.Stop ();
			showLicense (Helper.bgColor, Helper.fgColor, Helper.bgClick, this);
			showGoogleAnalytics (Helper.bgColor, Helper.fgColor, Helper.bgClick, this);
			TrackInstall ();
			// FontManager.SetAllControlsFont (this.Controls);
			FontManager.SetControlFont (label2, 1);
			FontManager.SetControlFont (label1, 1);
			FontManager.SetControlFont (check_bold, 1);
			FontManager.SetControlFont (check_italic, 1);
		}


		public static void showLicense (Color bg, Color fg, Color bgClick, Form parent)
		{
			Console.WriteLine(Settings.license);
			if (!Settings.license)
			{
				MessageForm msgf = new MessageForm ();
				msgf.setColors (bg, fg, bgClick);
				Assembly a = Assembly.GetExecutingAssembly ();
				Stream stm = a.GetManifestResourceStream ($"Yuki_Theme.Core.Resources.LICENSE");
				string description = "";
				using (StreamReader reader = new StreamReader (stm))
				{
					description = reader.ReadToEnd ();
				}

				msgf.Text = "LICENSE";
				msgf.setMessage ("JetBrains.Icons License", description, "Accept");
				msgf.ShowDialog (parent);
				Settings.license = true;
				Settings.database.UpdateData (Settings.LICENSE, "true");
			}
		}


		public static void showGoogleAnalytics (Color bg, Color fg, Color bgClick, Form parent)
		{
			if (!Settings.googleAnalytics && !Settings.dontTrack)
			{
				QuestionForm_2 qf = new QuestionForm_2 ();
				qf.setColors (bg, fg, bgClick);
				
				if (qf.ShowDialog (parent) == DialogResult.Yes)
				{
					Settings.googleAnalytics = true;
					Settings.database.UpdateData (Settings.GOOGLEANALYTICS, "true");
				} else
				{
					Settings.dontTrack = true;
					Settings.database.UpdateData (Settings.DONTTRACK, "true");
				}
			}
		}

		public static void TrackInstall ()
		{
			if (!Settings.Logged && !Settings.dontTrack)
			{
				HttpResponseMessage result = GoogleAnalyticsHelper.TrackEvent ().Result;
				if (!result.IsSuccessStatusCode)
				{
					// Maybe internet isn't available
				} else
				{
					Settings.database.UpdateData (Settings.LOGIN, "true");
					Settings.Logged = true;
				}
			}
		}

		
	}


	public delegate void SetTheme ();


	public delegate void ColorUpdate (Color bg, Color fg, Color bgClick);
}