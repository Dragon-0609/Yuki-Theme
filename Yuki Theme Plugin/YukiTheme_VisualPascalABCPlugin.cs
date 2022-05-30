using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Actions;
using ICSharpCode.TextEditor.Document;
using Microsoft.Win32;
using Svg;
using VisualPascalABC;
using VisualPascalABC.OptionsContent;
using VisualPascalABCPlugins;
using WeifenLuo.WinFormsUI.Docking;
using Yuki_Theme.Core;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.Database;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Interfaces;
using Yuki_Theme.Core.WPF;
using Yuki_Theme.Core.WPF.Windows;
using Yuki_Theme_Plugin.Controls.CodeCompletion;
using Yuki_Theme_Plugin.Controls.DockStyles;
using Yuki_Theme_Plugin.Controls.Helpers;
using Application = System.Windows.Forms.Application;
using CodeCompletionHighlighter = Yuki_Theme_Plugin.Controls.DockStyles.CodeCompletionHighlighter;
using CustomList = Yuki_Theme_Plugin.Controls.DockStyles.CustomList;
using MessageBox = System.Windows.Forms.MessageBox;
using Point = System.Drawing.Point;
using Resources = Yuki_Theme_Plugin.Properties.Resources;
using Size = System.Drawing.Size;
using Timer = System.Windows.Forms.Timer;

namespace Yuki_Theme_Plugin
{
	
	public class YukiTheme_VisualPascalABCPlugin : IVisualPascalABCPlugin, IColorUpdatable
	{
		public string Name => "Yuki Theme";

		public string Version =>
			Settings.current_version.ToString ("0.0", System.Globalization.CultureInfo.InvariantCulture);

		public string Copyright => "Dragon-LV";

		#region IDE Controls

		private          AboutBox              about;
		private          Form1                 fm;
		private          MenuStrip             menu;
		private          Panel                 output_input;
		private          RichTextBox           output_output;
		private          Panel                 output_panel1;
		private          Panel                 output_panel2;
		private          Panel                 output_panel3;
		private          Panel                 output_panel4;
		private          Panel                 output_panel5;
		private          Panel                 output_panel6;
		private          TextBox               output_text;
		private          Control               outputWindow;
		private          IHighlightingStrategy highlighting;
		private          StatusStrip           statusBar;
		private          ToolStrip             tools;
		private          Panel                 toolsPanel;
		private readonly IWorkbench            workbench;
		private          TextArea              textArea;
		private          ContextMenuStrip      context;
		private          ContextMenuStrip      context2;
		private          MenuRenderer          renderer;
		private          IconBarMargin         margin;
		private          FoldMargin            foldmargin;
		private          ListView              errorsList;
		private          TextBox               compilerConsole;
		
		private IVisualEnvironmentCompiler                  compiler;
		private CodeFileDocumentTextEditorControl           textEditor;
		private Dictionary <ICodeFileDocument, RichTextBox> OutputTextBoxs;

		private RichTextBox oldInputPanel;
		private Hashtable   ht;

		#endregion

		#region Colors, Brushes and Pens

		public static Color bg;
		public static Color bgdef;
		public static Color bgClick;
		public static Color bgClick2;
		public static Color bgClick3;
		public static Color bgInactive;
		public static Color clr;
		public static Color clrHover;
		public static Color bgBorder;
		public static Color bgType;
		public static Brush bgdefBrush;
		public static Brush bgBrush;
		public static Brush selectionBrush;
		public static Brush bgClickBrush;
		public static Brush bgClick3Brush;
		public static Brush typeBrush;
		public static Brush clrBrush;
		public static Brush bgInactiveBrush;
		public static Pen   separatorPen;
		public static Pen   bgBorderPen;

		#endregion

		public static Image           img;		
		private       Alignment       wallpaperAlign;
		private       int             wallpaperOpacity;
		private       Timer           documentUpdator;
		private       Timer           menuItemsColorUpdator;
		private       Timer           tim3;
		// public        MForm           mf;
		private       ToolStripButton currentThemeName;
		private       PictureBox      logoBox;
		private       Image           sticker;
		public        CustomPicture   stickerControl;

		#region Stuff of menu

		#region Menu Items

		private ToolStripMenuItem menu_settings;
		private ToolStripMenuItem quiet;
		private ToolStripMenuItem stick;
		private ToolStripMenuItem backimage;
		private ToolStripMenuItem switchTheme;
		private ToolStripMenuItem enablePositioning;
		private ToolStripMenuItem resetStickerPosition;
		private ToolStripMenuItem updatePage;
		
		#endregion

		#region Menu Items Images

		private Image quietImage;
		private Image positioningImage;
		private Image wallpaperImage;
		private Image switchImage;
		private Image resetPositionImage;
		
		#endregion

		#endregion
		
		private Size       defaultSize;
		private Panel      panel_bg;
		private CustomList themeList;
		private Label      lbl;
		public  Image      tmpImage1;
		public  Image      tmpImage2;
		
		private       IconManager       manager;
		public static ToolBarCamouflage camouflage;

		private bool      bgImage => Settings.bgImage;
		private Rectangle oldSizeOfTextEditor = Rectangle.Empty;
		
		bool                  toggled         = false; // is toggle activated
		int                   imagesEnabled   = 0;     // Is enabled bg image and (or) sticker
		bool                  nameInStatusBar = false; // Name in status bar
		private ToolStripItem openInExplorerItem;
		const   string        yukiThemeUpdate          = "Yuki Theme Update";
		private int           lastFocused              = -1;
		private bool          needToReturnTheme        = false;
		private bool          needToFullExportTheme        = false;
		private string        oldThemeNameForPreExport = "";
		private DateTime      prevPreExportTime;
		private bool          hideBG = false;

		public PopupFormsController popupController;

        public static YukiTheme_VisualPascalABCPlugin plugin;

		/// <summary>
		///     The main entry point for the application.
		/// </summary>
		public YukiTheme_VisualPascalABCPlugin (IWorkbench workbench)
		{
			this.workbench = workbench;
		}

		public YukiTheme_VisualPascalABCPlugin (IVisualEnvironmentCompiler compiler)
		{
			this.compiler = compiler;
		}

		public void GetGUI (List <IPluginGUIItem> MenuItems, List <IPluginGUIItem> ToolBarItems)
		{
			var item1 = new PluginGUIItem ("Yuki Theme", "Yuki Theme", null, Color.Transparent, InitCore);
			//Добавляем в меню
			MenuItems.Add (item1);

			fm = (Form1) workbench.MainForm;
			Helper.mode = ProductMode.Plugin;
			Settings.connectAndGet ();

			imagesEnabled = 0;
			imagesEnabled += Settings.bgImage ? 1 : 0;
			imagesEnabled += Settings.swSticker ? 2 : 0;
			nameInStatusBar = Settings.swStatusbar;
            plugin = this;
			loadWithWaiting ();
			Initialize ();
		}


		#region Initialization

		private void Initialize ()
		{
			fm.AllowTransparency = true;
			popupController = new PopupFormsController (fm, 0, this);
			LoadColors ();
			defaultSize = new Size (32, 32);
			
			textEditor = fm.CurrentCodeFileDocument.TextEditor;
			textArea = textEditor.ActiveTextAreaControl.TextArea;
			context = textEditor.ContextMenuStrip;
			context2 = fm.MainDockPanel.ContextMenuStrip;
			openInExplorerItem = context2.Items.Add ("Open in Explorer", null, OpenInExplorer);
			
			fm.Closing += FmOnClosing;
			
			context2.Opening += EditorContextOpening;
			LoadImage ();
			
			GetFields ();

			setMargin ();
			textArea.Paint += PaintBG;
			textEditor.Parent.BackColor = bg;
			textEditor.Controls [1].Paint += CtrlOnPaint;
			textEditor.Controls [1].Invalidate();
			InspectBrackets ();

			textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += CaretPositionChangedEventHandler;
			textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += CaretOnPositionChanged;
			
			fm.CurrentCodeFileDocument.BackColor = bg;
			
			documentUpdator = new Timer () {Interval = 2};
			documentUpdator.Tick += (sender, r) =>
			{
				documentUpdator.Stop ();
				ReSetTextEditor ();
			};
			
			fm.MainDockPanel.ActiveContentChanged += (sender, e) =>
			{
				documentUpdator.Start ();
			};

			renderer = new MenuRenderer ();
			menu.Renderer = renderer;
			context.Renderer = renderer;
			context2.Renderer = renderer;
			manager = new IconManager (tools, menu, context, context2, fm);
			camouflage = new ToolBarCamouflage (tools);
			
			UpdateColors ();
			
			WaitAndUpdateMenuColors ();
			CLI_Actions.onBGIMAGEChange = RefreshEditor;
			CLI_Actions.onSTICKERChange = ReloadSticker;
			CLI_Actions.onSTATUSChange = RefreshStatusBar;

			Helper.LoadCurrent ();
			
			currentThemeName = new ToolStripButton ();
			currentThemeName.Alignment = ToolStripItemAlignment.Right;
			loadSVG ();

			currentThemeName.Padding = new Padding (2, 0, 2, 0);
			currentThemeName.Margin = Padding.Empty;
			statusBar.Items.Add (currentThemeName);
			RefreshStatusBar ();

			InitializeSticker ();
			fm.Resize += FmOnResize;
			addToSettings ();
		}

		private void InitStyles ()
		{
			Extender.SetSchema (fm.MainDockPanel);
		}
		
		private void InitCore ()
		{
			WPFHelper.InitAppForWinforms ();

			MainWindow mw = new MainWindow ();
			mw.Show ();
			/*if (mf == null || mf.IsDisposed)
			{
				mf = new MForm (1);
				mf.startSettingTheme += ReleaseResources;
				mf.setTheme += ReloadLayout;
				mf.Disposed += (sender, e) =>
				{
					GC.Collect();
					GC.WaitForPendingFinalizers();  
				};
			}
			if (mf.Visible) return;
			mf.Show();*/
		}

		private void InitializeSticker ()
		{
			stickerControl = new CustomPicture (fm);
			stickerControl.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			stickerControl.margin = new Point (10, statusBar.Size.Height);
			stickerControl.Enabled = Settings.positioning;
			CustomPanel pnl = new CustomPanel (1) { Visible = false, Name = "LayerGrids" };
			pnl.pict = stickerControl;
			fm.Controls.Add (pnl);
			fm.Controls.Add (stickerControl);
			stickerControl.pnl = pnl;
			LoadSticker ();
			stickerControl.BringToFront ();
		}
		
		private void loadWithWaiting ()
		{
			tim3 = new Timer () {Interval = 2200};
			tim3.Tick += load;
			if (Settings.swLogo)
			{
				showLogo ();
			}
			else
				InitStyles ();
			tim3.Start ();
		}

		/// <summary>
		/// Look for item in menu. After that add additional items
		/// </summary>
		private void load (object sender, EventArgs e)
		{
			tim3.Stop ();
			if (Settings.swLogo)
			{
				hideLogo ();
				InitStyles ();
			}
			menu_settings = null;
			foreach (ToolStripItem menuItem in menu.Items)
			{
				if (menuItem is ToolStripMenuItem)
				{
					foreach (ToolStripItem toolStripMenuItem in ((ToolStripMenuItem) menuItem).DropDownItems)
					{
						if (toolStripMenuItem is ToolStripMenuItem)
						{
							if (toolStripMenuItem.Text == "Yuki Theme")
							{
								menu_settings = (ToolStripMenuItem) toolStripMenuItem;
								break;
							}
						}
					}
				}
			}
			GC.Collect(); // We must clean after search
			GC.WaitForPendingFinalizers();
			
			if (menu_settings != null) // If we could find...
			{
				string add = "";
				bool isDark = Helper.IsDark (bg);
				add = isDark ? "" : "_dark";

				menu_settings.Text = "Show Settings";
				menu_settings.ShortcutKeys = Keys.Alt | Keys.S;
				menu_settings.ShortcutKeyDisplayString = "Alt + S";
				menu_settings.Image = Helper.RenderSvg (menu_settings.Size, Helper.LoadSvg (
					                                        "gearPlain"+add, Assembly.GetExecutingAssembly (),
					                                        "Yuki_Theme_Plugin.Resources.icons"), false, Size.Empty,
				                                        true, clr);
				ToolStrip ow = menu_settings.Owner;
				ow.Items.Remove (menu_settings);

				Image icon = Helper.GetYukiThemeIconImage (new Size (32, 32));
				
				ToolStripMenuItem main =
					new ToolStripMenuItem ("Yuki Theme", icon);

				quiet = new ToolStripMenuItem ("Toggle Discreet Mode", null, ToggleQuiet, Keys.Alt | Keys.A);
				quiet.ShortcutKeyDisplayString = "Alt + A";
				quiet.BackColor = menu_settings.BackColor;
				quiet.ForeColor = menu_settings.ForeColor;
				quietImage = Helper.RenderSvg (quiet.Size, Helper.LoadSvg (
					                               "quiet", Assembly.GetExecutingAssembly (),
					                               "Yuki_Theme_Plugin.Resources"));
				quiet.Image = quietImage;

				stick = new ToolStripMenuItem ("Enable Stickers",
				                               Settings.swSticker
					                               ? updateBgofImage (currentThemeName.Image)
					                               : currentThemeName.Image, ToggleSticker);
				stick.BackColor = menu_settings.BackColor;
				stick.ForeColor = menu_settings.ForeColor;

				backimage = new ToolStripMenuItem ("Enable Wallpaper",
				                                   null, ToggleWallpaper);
				backimage.Image = Settings.bgImage
					? updateBgofImage (wallpaperImage)
					: wallpaperImage;
				
				backimage.BackColor = menu_settings.BackColor;
				backimage.ForeColor = menu_settings.ForeColor;

				switchTheme = new ToolStripMenuItem ("Switch Theme",
				                                     switchImage, SwitchTheme, Keys.Control | Keys.Oemtilde);
				switchTheme.ShortcutKeyDisplayString = "Ctrl + `";
				
				switchTheme.BackColor = menu_settings.BackColor;
				switchTheme.ForeColor = menu_settings.ForeColor;

				enablePositioning = new ToolStripMenuItem ("Enable Stickers Positioning",
				                                           null, stickersPositioning);
				
				enablePositioning.BackColor = menu_settings.BackColor;
				enablePositioning.ForeColor = menu_settings.ForeColor;
				positioningImage = Helper.RenderSvg (enablePositioning.Size, Helper.LoadSvg (
					                                     "export" + add, Assembly.GetExecutingAssembly (),
					                                     "Yuki_Theme_Plugin.Resources.icons"));
				enablePositioning.Image = Settings.positioning
					? updateBgofImage (positioningImage)
					: positioningImage;

				resetStickerPosition = new ToolStripMenuItem ("Reset Sticker Margins",
				                                              resetPositionImage, ResetStickerPosition);
				
				resetStickerPosition.BackColor = menu_settings.BackColor;
				resetStickerPosition.ForeColor = menu_settings.ForeColor;

				updatePage = new ToolStripMenuItem ("Show Update Notification",
				                                    icon, ShowUpdatePage);
				
				updatePage.BackColor = menu_settings.BackColor;
				updatePage.ForeColor = menu_settings.ForeColor;

				main.DropDownItems.Add (stick);
				main.DropDownItems.Add (backimage);
				main.DropDownItems.Add (enablePositioning);
				main.DropDownItems.Add (quiet);
				main.DropDownItems.Add (switchTheme);
				main.DropDownItems.Add (menu_settings);
				main.DropDownItems.Add (resetStickerPosition);
				main.DropDownItems.Add (updatePage);

				// Move Yuki Theme to the Top
				List <ToolStripItem> coll = new List <ToolStripItem> ();
				foreach (ToolStripItem item in ow.Items)
				{
					coll.Add (item);
				}
				
				ow.Items.Clear ();
				if (coll.Last () is ToolStripSeparator)
					coll.Remove (coll.Last ());
				
				ow.Items.Add (main);
				ow.Items.Add (new ToolStripSeparator ());

				ow.Items.AddRange (coll.ToArray ());
				UpdateTranslations ();
			}

			((CompilerConsoleWindowForm) workbench.CompilerConsoleWindow).AddTextToCompilerMessages (
				"Yuki Theme: Initialization finished.\n");
			
			InjectCodeCompletion ();
			/*MForm.showLicense (bg, clr, bgClick, fm);
			MForm.showGoogleAnalytics (bg, clr, bgClick, fm);
			MForm.TrackInstall ();*/
			if (!IsUpdated () && Settings.update)
			{
				popupController.InitializeAllWindows ();
				popupController.df.CheckUpdate ();
			}
		}

		private void loadSVG ()
		{
			if (currentThemeName.Image != null)
				currentThemeName.Image.Dispose ();
			SvgDocument svg = Helper.LoadSvg ("favorite", Assembly.GetExecutingAssembly (),
			                                  "Yuki_Theme_Plugin.Resources");
			svg.Fill = new SvgColourServer (bgBorder);
			svg.Stroke = new SvgColourServer (bgBorder);
			currentThemeName.Image = svg.Draw (16, 16);

			if (stick != null)
				stick.Image = currentThemeName.Image;

			if (Helper.currentTheme.Contains (":"))
			{
				string [] spl = Helper.currentTheme.Split (':');
				currentThemeName.Text = spl [spl.Length - 1];
				spl = null;
			} else
				currentThemeName.Text = Helper.currentTheme;

			if(wallpaperImage != null)
			{
				wallpaperImage.Dispose ();
				wallpaperImage = null;
			}
			wallpaperImage = Helper.RenderSvg (defaultSize, Helper.LoadSvg (
				                                   "layoutPreview", Assembly.GetExecutingAssembly (),
				                                   "Yuki_Theme_Plugin.Resources"), false, Size.Empty, true,
			                                   bgBorder);
			if(switchImage != null)
			{
				switchImage.Dispose ();
				switchImage = null;
			}
			
			switchImage = Helper.RenderSvg (defaultSize, Helper.LoadSvg (
				                                "refresh", Assembly.GetExecutingAssembly (),
				                                "Yuki_Theme_Plugin.Resources"), false, Size.Empty, true,
			                                bgBorder);
			if(resetPositionImage != null)
			{
				resetPositionImage.Dispose ();
				resetPositionImage = null;
			}
			
			resetPositionImage = Helper.RenderSvg (defaultSize, Helper.LoadSvg (
				                                "refresh", Assembly.GetExecutingAssembly (),
				                                "Yuki_Theme_Plugin.Resources"), false, Size.Empty, true,
			                                bgBorder);
		}
		
		private void LoadColors ()
		{
			highlighting = HighlightingManager.Manager.FindHighlighterForFile ("A.pas");
			bgdef = highlighting.GetColorFor ("Default").BackgroundColor;
			bgClick = Helper.DarkerOrLighter (bgdef, 0.25f);
			bgClick2 = Helper.DarkerOrLighter (bgdef, 0.4f);
			bgClick3 = Helper.DarkerOrLighter (bgdef, 0.1f);
			bgInactive = Helper.ChangeColorBrightness (bgdef, -0.3f);
			bg = Helper.bgColor = Helper.DarkerOrLighter (bgdef, 0.05f);
			clr = Helper.DarkerOrLighter (highlighting.GetColorFor ("Default").Color, 0.2f);
			clrHover = Helper.DarkerOrLighter (highlighting.GetColorFor ("Default").Color, 0.6f);
			bgBorder = highlighting.GetColorFor ("CaretMarker").Color;
			bgType = highlighting.GetColorFor ("EOLMarkers").Color;

			ResetBrushesAndPens ();
		}

		private void LoadImage ()
		{
			bool dispI = false;
			if(img != null) { img.Dispose ();
				dispI = true;
			}
			string pth = Path.Combine (Settings.pascalPath, "Highlighting","background.png");
			if (File.Exists (pth))
			{
				Image iamg = Image.FromFile (pth);
				// Console.WriteLine("Image loaded");
				wallpaperAlign = Alignment.Center;
				wallpaperOpacity = 10;

				XmlDocument doc = new XmlDocument ();
				IHighlightingStrategy high = HighlightingManager.Manager.FindHighlighterForFile ("A.pas");

				var fls = Directory.GetFiles (Path.Combine (Settings.pascalPath, "Highlighting/"), "*.xshd");

				foreach (string fl in fls)
				{
					doc.Load (fl);
					XmlNode nod = doc.SelectSingleNode ("/SyntaxDefinition");
					if (nod.Attributes ["name"].Value == high.Name)
					{
						XmlNodeList comms = nod.SelectNodes ("//comment()");

						foreach (XmlComment comm in comms)
						{
							if (comm.Value.StartsWith ("align"))
							{
								wallpaperAlign = (Alignment) int.Parse (comm.Value.Substring (6));
							} else if (comm.Value.StartsWith ("opacity"))
							{
								wallpaperOpacity = int.Parse (comm.Value.Substring (8));
							}
						}

						break;
					}
				}
				if(wallpaperOpacity != 100)
				{
					img = Helper.SetOpacity (iamg, wallpaperOpacity);
					iamg.Dispose ();
				} else
				{
					img = iamg;
				}
				// Console.WriteLine("Image set");
			} else
			{
				img = null;
				wallpaperAlign = Alignment.Left;
			}
		}
		
		public void LoadSticker ()
		{
			if (sticker != null)
			{
				sticker.Dispose ();
				sticker = null;
			};
			if (Settings.swSticker)
			{
				if (Settings.useCustomSticker && File.Exists (Settings.customSticker))
				{
					sticker = Image.FromFile (Settings.customSticker);
				}else
				{
					string pth = Path.Combine (Settings.pascalPath, "Highlighting", "sticker.png");
					if (File.Exists (pth))
					{
						Image stckr = Image.FromFile (pth);


						if (CLI.currentTheme.StickerOpacity != 100)
						{
							sticker = Helper.SetOpacity (stckr, CLI.currentTheme.StickerOpacity);
							stckr.Dispose ();
						} else
							sticker = stckr;

						stickerControl.Visible = true;
					} else
					{
						sticker = null;
						stickerControl.Visible = false;
					}
				}

				stickerControl.img = sticker;
			} else
			{
				sticker = null;
				stickerControl.Visible = false;
			}
		}
		

		#endregion
		
		
		#region Methods_For_Menu

		private void ToggleQuiet (object sender, EventArgs e)
		{
			if (!toggled)
			{
				Settings.bgImage = false;
				Settings.swSticker = false;
				if (nameInStatusBar)
					currentThemeName.Visible = false;
			} else
			{
				Settings.bgImage = imagesEnabled == 1 || imagesEnabled == 3;
				Settings.swSticker = imagesEnabled == 2 || imagesEnabled == 3;
				if (nameInStatusBar)
					currentThemeName.Visible = true;
			}
			toggled = !toggled;
			
			textArea.Refresh ();
			LoadSticker ();
			updateQuietImage ();
			updateWallpaperImage ();
			updateStickerImage ();
		}

		private void ToggleWallpaper (object sender, EventArgs e)
		{
			Settings.bgImage = !Settings.bgImage;
			textArea.Refresh ();
			updateWallpaperImage ();
		}

		private void SwitchTheme (object sender, EventArgs e)
		{
			if(!fm.Controls.ContainsKey ("Custom Panel Switcher"))
			{
				// if (mf == null || mf.IsDisposed)
				// {
					RememberCurrentEditor ();
					 
					panel_bg = new CustomPanel (0);
					panel_bg.Name = "Custom Panel Switcher";
					needToReturnTheme = true;
					needToFullExportTheme = false;
					prevPreExportTime = DateTime.Now;
					Font fnt = new Font (FontFamily.GenericSansSerif, 10, GraphicsUnit.Point);
					
					lbl = new Label ();
					lbl.BackColor = bg;
					lbl.ForeColor = clr;
					lbl.Font = fnt;
					lbl.Text = CLI.Translate ("plugin.themes");
					lbl.TextAlign = ContentAlignment.MiddleCenter;
					lbl.Size = new Size (200, 25);

					themeList = new CustomList ();
					themeList.BackColor = bgdef;
					themeList.ForeColor = clr;
					themeList.BorderStyle = BorderStyle.None;
					themeList.list = CLI.schemes.ToArray ();
					themeList.SearchText ("");
					themeList.BorderStyle = BorderStyle.None;
					themeList.Font = fnt;
					themeList.ItemHeight = themeList.Font.Height;
					themeList.Size = new Size (200, 300);
					themeList.MouseMove += ThemeListMouseHover;
					themeList.InitSearchBar ();
					
					themeList.searchBar.Size = new Size (themeList.Size.Width, themeList.searchBar.Size.Height);
					themeList.Size = new Size (themeList.Size.Width, themeList.Size.Height - (themeList.searchBar.Size.Height + 2));

					panel_bg.Location = Point.Empty;
					panel_bg.Size = fm.ClientSize;
					themeList.DrawMode = DrawMode.OwnerDrawFixed;
					themeList.DrawItem += list_1_DrawItem;
					int x = (panel_bg.Width / 2) - (themeList.Width / 2);
					int y = (panel_bg.Height / 2) - ((themeList.Height + themeList.searchBar.Size.Height + lbl.Size.Height + 8) / 2);

					lbl.Location = new Point (x, y - 33);
					themeList.searchBar.Location = new Point (x, lbl.Location.Y + lbl.Size.Height + 2);

					themeList.Location = new Point (x, themeList.searchBar.Location.Y + themeList.searchBar.Size.Height + 4);
					
					if (themeList.Items.Contains (Helper.currentTheme))
						themeList.SelectedItem = Helper.currentTheme;
					else
						themeList.SelectedIndex = 0;
					themeList.selectionindex = themeList.SelectedIndex;
					themeList.SelectedIndexChanged += ThemeListOnSelectedIndexChanged;
					themeList.AccessibleName = themeList.SelectedItem.ToString ();
					panel_bg.Click += CloseOnClick;
					oldThemeNameForPreExport = themeList.AccessibleName;

					panel_bg.Controls.Add (themeList);
					panel_bg.Controls.Add (lbl);
					panel_bg.Controls.Add (themeList.searchBar);

					setBorder (themeList, lbl, themeList.searchBar);

					fm.Controls.Add (panel_bg);
					panel_bg.BringToFront ();
					themeList.searchBar.Focus ();
					
				// } else
				// {
				// 	MessageBox.Show (CLI.Translate ("plugin.close"));
				// }
			}
		}
		
		private void stickersPositioning (object sender, EventArgs e)
		{
			Settings.positioning = !Settings.positioning;
			stickerControl.Enabled = Settings.positioning;
			updatestickersPositioningImage ();
		}
		
		private void ResetStickerPosition (object sender, EventArgs e)
		{
			DatabaseManager.DeleteData (Settings.STICKERPOSITION);
			stickerControl.ReadData ();
			stickerControl.UpdateLocation ();
		}
		
		private void ShowUpdatePage (object sender, EventArgs e)
		{
			openUpdate ();
		}

		private void ToggleSticker (object sender, EventArgs e)
		{
			Settings.swSticker = !Settings.swSticker;
			LoadSticker ();
			updateStickerImage ();
		}

		private void EditorContextOpening (object sender, CancelEventArgs e)
		{
			if (!(fm.MainDockPanel.ActiveContent is CodeFileDocumentControl))
			{
				openInExplorerItem.Visible = false;
			}
			else
			{
				openInExplorerItem.Visible = true;
				openInExplorerItem.Enabled = CheckAvailabilityOfDocument ();
			}
		}
		
		#endregion

		
		#region Updates

		private void UpdateColors ()
		{
			fm.BackColor = menu.BackColor = statusBar.BackColor = toolsPanel.BackColor = tools.BackColor =
				fm.cmEditor.BackColor = textEditor.Parent.BackColor =
					fm.CurrentCodeFileDocument.BackColor = fm.BottomPane.Parent.BackColor = bg;
			
			output_panel2.BackColor = output_panel6.BackColor = output_input.BackColor = output_panel4.BackColor =
				output_panel3.BackColor = output_panel5.BackColor = output_panel1.BackColor = output_text.BackColor =
					output_output.BackColor = fm.ProjectPane.BackColor = errorsList.BackColor = compilerConsole.BackColor = Helper.bgColor = bgdef;

			output_output.ForeColor = output_panel2.ForeColor = output_text.ForeColor = menu.ForeColor =
				statusBar.ForeColor = toolsPanel.ForeColor = tools.ForeColor = errorsList.ForeColor =
					compilerConsole.ForeColor = Helper.fgColor = clr;

			Helper.bgClick = bgClick;
			Helper.bgBorder = bgBorder;
			Helper.fgHover = clrHover;
			
			foreach (Control o in output_panel1.Controls)
			{
				if(o is Button)
				{
					Button b = (Button) o;
					b.BackColor = bgdef;
					b.FlatAppearance.BorderColor = bgBorder;
				}
			}

			foreach (ToolStripItem item in context.Items)
			{
				item.ForeColor = clr;
			}
			
			context2.BackColor = bg;

			foreach (ToolStripItem item in context2.Items)
			{
				item.ForeColor = clr;
			}

			
			if (menu_settings != null)
			{
				string add = "";
				bool isDark = Helper.IsDark (bg);
				add = isDark ? "" : "_dark";
				menu_settings.Image = Helper.RenderSvg (menu_settings.Size, Helper.LoadSvg (
					                                        "gearPlain"+add, Assembly.GetExecutingAssembly (),
					                                        "Yuki_Theme_Plugin.Resources.icons"),
				                                        false, Size.Empty,
				                                        true, clr);
				menu_settings.BackColor = bgdef;
			}
			if (quiet != null) quiet.BackColor = bgdef;
			if (stick != null) stick.BackColor = bgdef;
			if (backimage != null) backimage.BackColor = bgdef;
			if (switchTheme != null) switchTheme.BackColor = bgdef;
			if (resetStickerPosition != null) resetStickerPosition.BackColor = bgdef;
			if (updatePage != null) updatePage.BackColor = bgdef;
			if (enablePositioning != null)
			{
				enablePositioning.BackColor = bgdef;
				bool isDark = Helper.IsDark (bg);
				string add = isDark ? "" : "_dark";
				enablePositioning.Image = Helper.RenderSvg (enablePositioning.Size,
				                                            Helper.LoadSvg ("export" + add, Assembly.GetExecutingAssembly (),
				                                                            "Yuki_Theme_Plugin.Resources.icons"), false, Size.Empty, false,
				                                            Color.Black);

			}

			try
			{
				if (textEditor.Controls.Count >= 2) textEditor.Controls [1].Invalidate ();
			} catch (ArgumentOutOfRangeException)
			{

			}
		
			if (OnColorUpdate != null)
				OnColorUpdate (bgdef, clr, bgClick);
			popupController.TryToUpdateNotificationWindow ();
			errorsList.Refresh ();
			WaitAndUpdateMenuColors ();
			manager.UpdateColors ();
			UpdateBottomTextPanel ();
		}

		private void ResetBrushesAndPens ()
		{
			ResetBrush (ref bgdefBrush, bgdef);
			ResetBrush (ref bgBrush, bg);
			ResetBrush (ref selectionBrush, highlighting.GetColorFor ("Selection").BackgroundColor);
			ResetBrush (ref bgClickBrush, bgClick);
			ResetBrush (ref bgClick3Brush, bgClick3);
			ResetBrush (ref bgInactiveBrush, bgInactive);
			ResetBrush (ref clrBrush, clr);
			ResetBrush (ref typeBrush, bgType);
			
			ResetPen (ref separatorPen, bgClick3, 1, default);
			ResetPen (ref bgBorderPen, bgBorder, 8, default);
		}

		private void ReloadSticker ()
		{
			imagesEnabled = 0;
			imagesEnabled += Settings.bgImage ? 1 : 0;
			imagesEnabled += Settings.swSticker ? 2 : 0;
			LoadSticker ();
		}
		
		private void RefreshStatusBar ()
		{
			currentThemeName.Visible = Settings.swStatusbar;
			nameInStatusBar = Settings.swStatusbar;
		}
		
		private void RefreshEditor ()
		{
			textArea.Refresh ();
			imagesEnabled = 0;
			imagesEnabled += Settings.bgImage ? 1 : 0;
			imagesEnabled += Settings.swSticker ? 2 : 0;
		}

		private void ReSetTextEditor ()
		{
			if (textEditor != fm.CurrentCodeFileDocument.TextEditor)
			{
				textArea.Paint -= PaintBG;
				StopInspectBrackets ();
				
				try
				{
					textEditor.Controls [1].Paint -= CtrlOnPaint;
				} catch (ArgumentOutOfRangeException)
				{
				}

				// textArea = textEditor.ActiveTextAreaControl.TextArea;
				textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged -= CaretPositionChangedEventHandler;
				textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged -= CaretOnPositionChanged;
				textEditor = fm.CurrentCodeFileDocument.TextEditor;
				textArea = textEditor.ActiveTextAreaControl.TextArea;
				textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += CaretPositionChangedEventHandler;
				textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += CaretOnPositionChanged;
				setMargin ();
				InspectBrackets ();
				textEditor.Parent.BackColor = bg;
				try
				{
					textEditor.Controls [1].Paint += CtrlOnPaint;
					textEditor.Controls [1].Invalidate ();
				} catch (ArgumentOutOfRangeException)
				{
				}

				fm.CurrentCodeFileDocument.BackColor = bg;
				
				textArea.Paint += PaintBG;
				textArea.Refresh ();
				UpdateBottomTextPanel ();

				if (Unsubscribe (ht [textEditor]))
				{
					SubscribeCompletion ();
				} else
				{
					// Console.WriteLine ("Couldn't Unsubscribe");
				}
			}
		}

		private void UpdateBottomTextPanel ()
		{
			// if (oldInputPanel != null)
			// {
			// 	oldInputPanel.Paint -= OldInputPanelOnPaint;
			// }
			oldInputPanel = OutputTextBoxs [fm.CurrentCodeFileDocument];
			oldInputPanel.BackColor = bgdef;
			oldInputPanel.BorderStyle = BorderStyle.None;
			// oldInputPanel.Paint += OldInputPanelOnPaint;
			// oldInputPanel.Invalidate();
		}

		public void ReloadLayout ()
		{
			ReloadLayoutAll (false);
		}
		
		public void ReloadLayoutLight ()
		{
			ReloadLayoutAll (true);
			panel_bg.Visible = false;
			
			Timer tim = new Timer ();
			tim.Interval = 5;
			tim.Tick += (sender, args) =>
			{
				tim.Stop ();
				if (panel_bg != null && !panel_bg.IsDisposed)
				{
					panel_bg.Visible = true;
					panel_bg.BringToFront ();
					themeList.searchBar.Focus ();
				}
			};
			tim.Start ();
		}
		
		public void ReloadLayoutAll (bool lightReload)
		{
			HighlightingManager.Manager.ReloadSyntaxModes ();
			if (!lightReload)
			{
				LoadImage ();
				LoadSticker ();
			}
			LoadColors ();
			UpdateColors ();
			fm.Refresh ();
			loadSVG ();
			updateQuietImage ();
			updateWallpaperImage ();
			updateStickerImage ();
			updateSwitchImage ();
			updateResetPositionImage ();
			UpdateWebBrowserTheme ();
		}
		
		private Bitmap updateBgofImage (Image oldImage)
		{
			Bitmap newImage = (Bitmap) oldImage.Clone ();
			using (var g = Graphics.FromImage(newImage))
			{
				g.Clear (Color.FromArgb (90,bgBorder));
				g.DrawImage(oldImage, new Point(0, 0));        
			}
			return newImage;
		}
		
		private void updateQuietImage ()
		{
			if (quiet != null)
			{
				quiet.Image = toggled
					? updateBgofImage (quietImage)
					: quietImage;
			}
		}
		
		private void updatestickersPositioningImage ()
		{
			if (enablePositioning != null)
			{
				enablePositioning.Image = Settings.positioning
					? updateBgofImage (positioningImage)
					: positioningImage;
			}
		}

		private void updateWallpaperImage ()
		{
			if (backimage != null)
			{
				backimage.Image = Settings.bgImage
					? updateBgofImage (wallpaperImage)
					: wallpaperImage;
			}
		}
		
		private void updateSwitchImage ()
		{
			if (switchTheme != null)
			{
				switchTheme.Image = switchImage;
			}
		}
		
		private void updateResetPositionImage ()
		{
			if (resetStickerPosition != null)
			{
				resetStickerPosition.Image = resetPositionImage;
			}
		}

		private void updateStickerImage ()
		{
			if (stick != null)
			{
				stick.Image = Settings.swSticker
					? updateBgofImage (currentThemeName.Image)
					: currentThemeName.Image;
			}
		}
		
		private void UpdateAboutForm (object senderaw, EventArgs eaw)
		{
			about.BackColor = bg;
			about.ForeColor = clr;
			Button btn = null;
			foreach (Control cont in about.Controls)
			{
				if (cont is LinkLabel)
				{
					((LinkLabel) cont).LinkColor = clr;
					((LinkLabel) cont).ActiveLinkColor = clrHover;
				}else if (cont is Button)
					btn = (Button) cont;
				else if (cont is GroupBox)
				{
					GroupBox group = (GroupBox) cont;
					group.ForeColor = clr;
					foreach (Control groupControl in group.Controls)
					{
						if (groupControl is LinkLabel)
						{
							((LinkLabel) groupControl).LinkColor = clr;
							((LinkLabel) groupControl).ActiveLinkColor = clrHover;
						} else if (groupControl is ListView)
						{
							ListView lw = (ListView) groupControl;
							lw.OwnerDraw = true;
							lw.DrawColumnHeader += errorListHeaderDrawer;
							lw.DrawItem += (sender, e) =>
							{
								e.DrawDefault = true;
							};
							lw.BackColor = bg;
							lw.ForeColor = clr;
						}
					}
				}
				else if (cont is TableLayoutPanel)
				{
					TableLayoutPanel tbl = (TableLayoutPanel) cont;
					tbl.ForeColor = clr;
					foreach (Control flowLayout in tbl.Controls)
					{
						if (flowLayout is FlowLayoutPanel)
						{
							
							foreach (Control tblControl in flowLayout.Controls)
							{
								if (tblControl is Label)
								{
									if(tblControl.Name.Contains ("Version"))
										tblControl.ForeColor = bgBorder;
									else
										tblControl.ForeColor = clr;
								}
							}
						}
					}
					
				}
			}
			

			btn.BackColor = bg;
			btn.ForeColor = clr;
			btn.FlatStyle = FlatStyle.Flat;
			btn.UseVisualStyleBackColor = false;
			btn.FlatAppearance.MouseOverBackColor = bgClick;
		}
		
		private void FmOnResize (object sender, EventArgs e)
		{
			if (panel_bg != null)
			{
				CloseOnClick (sender, e);
			}
		}
		
		private void WaitAndUpdateMenuColors ()
		{
			menuItemsColorUpdator = new Timer () {Interval = 1000};
			menuItemsColorUpdator.Tick += UpdateColorsOfMenuItems;
			menuItemsColorUpdator.Start ();
		}
		
		private void UpdateColorsOfMenuItems (object sender, EventArgs e)
		{
			foreach (ToolStripMenuItem item in menu.Items)
			{
				item.BackColor = bg;
				item.ForeColor = clr;
				foreach (ToolStripItem downItem in item.DropDownItems)
				{
					downItem.BackColor = bg;
					downItem.ForeColor = clr;
				}
			}
			menuItemsColorUpdator.Stop ();
			menuItemsColorUpdator.Dispose ();
		}
		
		private void UpdateWebBrowserTheme()
		{
			WebBrowserControl tp = null;
			FieldInfo field = typeof (Form1).GetField ("OpenBrowserDocuments", BindingFlags.Instance | BindingFlags.NonPublic);
			Dictionary <string, WebBrowserControl> OpenBrowserDocuments = (Dictionary <string, WebBrowserControl>)field.GetValue (fm);
			OpenBrowserDocuments.TryGetValue (yukiThemeUpdate, out tp); 
			if (tp is UpdatePageControl)
			{
				UpdatePageControl upage = (UpdatePageControl)tp;
				upage.UpdateTheme ();
			}
		}

		public void UpdateTranslations ()
		{
			openInExplorerItem.Text = CLI.Translate ("plugin.explorer.open");
			menu_settings.Text = CLI.Translate ("plugin.menu.settings");
			quiet.Text = CLI.Translate ("plugin.menu.discreet");
			stick.Text = CLI.Translate("plugin.menu.stickers");
			backimage.Text = CLI.Translate("plugin.menu.wallpaper");
			switchTheme.Text = CLI.Translate ("plugin.menu.switch");
			enablePositioning.Text = CLI.Translate ("plugin.menu.positioning");
			resetStickerPosition.Text = CLI.Translate ("plugin.menu.margins");
			updatePage.Text = CLI.Translate ("plugin.menu.notification");
		}
		
		#endregion

		
		#region Logo Management

		private void showLogo ()
		{
			logoBox = new PictureBox ();
			logoBox.BackColor = bgdef;
			logoBox.Image = Resources.YukiTheme;
			logoBox.Location = new Point (fm.ClientSize.Width/2 - 50, fm.ClientSize.Height/2 - 50);
			logoBox.Name = "logoBox";
			logoBox.Size = new Size (100, 100);
			logoBox.SizeMode = PictureBoxSizeMode.Zoom;
			logoBox.TabIndex = 0;
			logoBox.TabStop = false;
			fm.Controls.Add (logoBox);
		}

		private void hideLogo ()
		{
			fm.Controls.Remove (logoBox);
			logoBox.Dispose ();
		}
		

		#endregion
		

		#region Painting

		private void PaintBG (object sender, PaintEventArgs e)
		{
			if (margin != null)
			{
				e.Graphics.FillRectangle (new SolidBrush (bgdef), margin.DrawingPosition.X,
				                          margin.DrawingPosition.Y,
				                          margin.DrawingPosition.Width, margin.DrawingPosition.Height);
				var inside =
					typeof (IconBarMargin).GetMethod ("IsLineInsideRegion",
					                                  BindingFlags.Static | BindingFlags.NonPublic);
				// paint icons
				foreach (Bookmark mark in textArea.Document.BookmarkManager.Marks)
				{
					int lineNumber = textArea.Document.GetVisibleLine (mark.LineNumber);
					int lineHeight = textArea.TextView.FontHeight;
					int yPos = (int)(lineNumber * lineHeight) - textArea.VirtualTop.Y;
					if ((bool)inside.Invoke (
						    null, new object [] { yPos, yPos + lineHeight, margin.DrawingPosition.Y, margin.DrawingPosition.Height }))
					{
						if (lineNumber == textArea.Document.GetVisibleLine (mark.LineNumber - 1))
						{
							// marker is inside folded region, do not draw it
							continue;
						}

						mark.Draw (margin, e.Graphics, new Point (0, yPos));
					}
				}
			}

			if (foldmargin != null)
			{
				setMarginPosition ();
				e.Graphics.DrawLine (BrushRegistry.GetDotPen (bgdef, bgBorder),
				                     foldmargin.DrawingPosition.X,
				                     foldmargin.DrawingPosition.Y,
				                     foldmargin.DrawingPosition.X,
				                     foldmargin.DrawingPosition.Height);
			}

			if (img != null && bgImage && !hideBG)
			{
				Size vm = textEditor.ClientSize;
				// bool chnd = false;
				// if (outputWindow.Visible)
				// {
				// 	if(oldInputPanel != null)
				// 	{
				// 		vm.Height += oldInputPanel.ClientSize.Height;
				// 		chnd = true;
				// 	}
				// }
				if (oldSizeOfTextEditor.Width != vm.Width ||
				    oldSizeOfTextEditor.Height != vm.Height)
				{
					oldSizeOfTextEditor = Helper.GetSizes (img.Size, vm.Width, vm.Height,
					                                       wallpaperAlign);
				}
				// if(chnd)
				// {
				// 	Rectangle rct = new Rectangle (oldSizeOfTextEditor.X, 0, oldSizeOfTextEditor.Width, textEditor.ClientSize.Height);
				// 	float pr = oldSizeOfTextEditor.Height / 100f;
				// 	float px = img.Height / 100f;
				// 	float hg = img.Height - (px * ((oldSizeOfTextEditor.Height - rct.Height) / pr));
				// 	e.Graphics.DrawImage (img, rct, 0, 0, img.Width, hg, GraphicsUnit.Pixel);
				// }
				// else
				// {
					e.Graphics.DrawImage (img, oldSizeOfTextEditor);
				// }
			}
		}
		
		private void OldInputPanelOnPaint (object sender, PaintEventArgs e)
		{
			if (outputWindow.Visible)
			{
				Size vm = textEditor.ClientSize;
				vm.Height += oldInputPanel.ClientSize.Height;
				Rectangle rct = new Rectangle (oldSizeOfTextEditor.X, textEditor.ClientSize.Height, oldSizeOfTextEditor.Width,
				                               oldInputPanel.ClientSize.Height);
				float pr = oldSizeOfTextEditor.Height / 100f;
				float px = img.Height / 100f;
				float hg = px * ((oldSizeOfTextEditor.Height - rct.Height) / pr);
				((CompilerConsoleWindowForm) workbench.CompilerConsoleWindow).AddTextToCompilerMessages (
					"Yuki Theme: "+rct.ToString ()+ " _ " + oldSizeOfTextEditor.ToString () + " | ");
				e.Graphics.DrawImage (img, rct, 0, 0, img.Width, hg, GraphicsUnit.Pixel);
			}
		}
		
		private void CtrlOnPaint (object sender, PaintEventArgs e)
		{
			e.Graphics.FillRectangle (new SolidBrush (bgdef), e.ClipRectangle);
		}

		private void ToolStripPanelOnPaint (object sender, PaintEventArgs e)
		{
			e.Graphics.DrawLine (separatorPen, e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width,
			                     e.ClipRectangle.Y);
		}

		private void errorListHeaderDrawer (object sender, DrawListViewColumnHeaderEventArgs e)
		{
			e.Graphics.FillRectangle (bgBrush, e.Bounds);

			e.Graphics.DrawString (e.Header.Text, e.Font, clrBrush, e.Bounds);
		}
		
		#endregion

		
		#region Events For Core

		public void ifHsImage (Image img)
		{
			tmpImage1 = img;
			
		}

		public void ifHsSticker (Image img)
		{
			tmpImage2 = img;
		}
		
		public void ifDNIMG ()
		{
			tmpImage1 = null;
		}
		
		public void ifDNSTCK ()
		{
			tmpImage2 = null;
		}

		#endregion

		
		#region Methods For Theme Switcher

		private void list_1_DrawItem (object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0) return;
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				e = new DrawItemEventArgs (e.Graphics, e.Font, e.Bounds,
				                           e.Index, e.State ^ DrawItemState.Selected,
				                           e.ForeColor, bgClick2);
			} else if (e.Index == themeList.selectionindex)
			{
				e = new DrawItemEventArgs (e.Graphics, e.Font, e.Bounds,
				                           e.Index, e.State,
				                           e.ForeColor, bgClick);
			}

			e.DrawBackground ();
			e.Graphics.DrawString (((ListBox) sender).Items [e.Index].ToString (), e.Font, clrBrush, e.Bounds);

			e.DrawFocusRectangle ();
		}

		private void CloseOnClick (object sender, EventArgs e)
		{
			if (Settings.showPreview)
			{
				if (needToReturnTheme)
				{
					needToFullExportTheme = true;
					PreviewTheme (themeList.AccessibleName, oldThemeNameForPreExport);
					needToReturnTheme = false;
					needToFullExportTheme = false;
				} else
				{
					hideBG = !CLI.currentTheme.HasWallpaper;
					stickerControl.Visible = Settings.swSticker && CLI.currentTheme.HasSticker;
				}
			}

			fm.Controls.Remove (panel_bg);
			panel_bg?.Dispose ();
			themeList?.searchBar.Dispose ();
			themeList?.Dispose ();
			panel_bg = null;
			if (tmpImage1 != null)
			{
				tmpImage1.Dispose ();
				tmpImage1 = null;
			}

			if (tmpImage2 != null)
			{
				tmpImage2.Dispose ();
				tmpImage2 = null;
			}

			ReFocusCurrentEditor ();
		}
		
		private void ThemeListOnSelectedIndexChanged (object sender, EventArgs e)
		{
			if(themeList.SelectedIndex >= 0)
			{
				if(themeList.SelectedItem.ToString () != themeList.AccessibleName)
				{
					bool cnd = CLI.SelectTheme (themeList.SelectedItem.ToString ());

					if (cnd)
					{
						CLI.selectedItem = CLI.nameToLoad;
						CLI_Actions.ifHasImage2 = ifHsImage;
						CLI_Actions.ifHasSticker2 = ifHsSticker;
						CLI_Actions.ifDoesntHave2 = ifDNIMG;
						CLI_Actions.ifDoesntHaveSticker2 = ifDNSTCK;
						CLI.restore (false, null);
						CLI.export (tmpImage1, tmpImage2, ReloadLayout, ReleaseResources);

						CLI_Actions.ifHasImage2 = null;
						CLI_Actions.ifHasSticker2 = null;
						CLI_Actions.ifDoesntHave2 = null;
						CLI_Actions.ifDoesntHaveSticker2 = null;
					}
				}
				needToReturnTheme = false;
				CloseOnClick (sender, e);
			}
		}
		
		private void ThemeListMouseHover(object sender, EventArgs e)
		{
			InvalidateItem ();
			if (Settings.showPreview)
			{
				string nm = themeList.Items [themeList.selectionindex].ToString ();
				if ((DateTime.Now - prevPreExportTime).TotalMilliseconds >= 25 && nm != oldThemeNameForPreExport) // Preview Theme if delay is more than 25 milliseconds
				{
					prevPreExportTime = DateTime.Now;
					PreviewTheme (nm, oldThemeNameForPreExport);
					lbl.BackColor = bg;
					lbl.ForeColor = clr;
					themeList.BackColor = bgdef;
					themeList.ForeColor = clr;
					oldThemeNameForPreExport = themeList.Items [themeList.selectionindex].ToString ();
				}
			}
		}

		private void PreviewTheme (string name, string oldName)
		{
			if(name != oldName)
			{
				if(CLI.SelectTheme (name))
				{
					CLI.restore ();
					hideBG = !CLI.currentTheme.HasWallpaper;
					stickerControl.Visible = Settings.swSticker && CLI.currentTheme.HasSticker;
					if (needToFullExportTheme)
					{
						CLI.preview (SyntaxType.NULL, true, ReloadLayoutLight);
					} else
					{
						SyntaxType type = ShadowNames.GetSyntaxByExtension (Path.GetExtension (fm.CurrentCodeFileDocument.FileName));
						if (type != SyntaxType.Pascal)
						{
							CLI.preview (type, true, null); // Not to reload layout
							CLI.preview (SyntaxType.Pascal, false, ReloadLayoutLight); // Pascal theme is necessary for UI
						} else
						{
							CLI.preview (type, true, ReloadLayoutLight);
						}
					}
				}
			}
		}

		private void InvalidateItem ()
		{
			Point point = themeList.PointToClient (Cursor.Position);
			int index = themeList.IndexFromPoint (point);
			//Do any action with the item
			themeList.UpdateHighlighting (index);
		}

		void setBorder(Control ctl, Control ctl2, Control ctl3)
		{
			Panel pan = new Panel();
			pan.BorderStyle = BorderStyle.None;
			pan.Size = new Size (ctl.ClientRectangle.Width + 2,
			                     ctl.ClientRectangle.Height + ctl2.ClientRectangle.Height + ctl3.ClientRectangle.Height + 6);
			pan.Location = new Point(ctl.Left - 1, ctl.Top - 1);
			pan.BackColor = YukiTheme_VisualPascalABCPlugin.bgInactive;
			pan.Parent = ctl.Parent;
			ctl.Parent = pan;
			ctl2.Parent = pan;
			ctl3.Parent = pan;

			ctl3.Location = new Point (1, Math.Abs (ctl2.Top - ctl3.Top));
			ctl2.Location = new Point (1, 1);
			ctl.Location = new Point (1, ctl3.Bottom + 1);
		}
		
		#endregion

		private void RememberCurrentEditor ()
		{
			if (fm.ActiveControl is UpdatePageControl)
			{
				UpdatePageControl update = (UpdatePageControl)fm.ActiveControl;
				lastFocused = update.TabIndex;
			}else
			{
				lastFocused = fm.CurrentCodeFileDocument.TabIndex;
			}
		}

		private void ReFocusCurrentEditor ()
		{
			IDockContent [] docs = fm.MainDockPanel.DocumentsToArray ();			
			IDockContent doc = docs [lastFocused];
			doc.DockHandler.Pane.Focus ();
			if (doc.DockHandler.Content is UpdatePageControl)
			{
				UpdatePageControl update = (UpdatePageControl)doc.DockHandler.Content;
				update.Focus ();
			} else
			{
				CodeFileDocumentControl cod = (CodeFileDocumentControl)doc.DockHandler.Content;
				cod.Focus ();
			}
			docs = null;
		}
		
		private void GetFields ()
		{
			CompilerConsoleWindowForm cons = (CompilerConsoleWindowForm)workbench.CompilerConsoleWindow;
			compilerConsole = (TextBox)cons.Controls.Find ("CompilerConsole", false) [0];

			about = fm.AboutBox1;
			about.Shown += UpdateAboutForm;

			statusBar = (StatusStrip)fm.Controls.Find ("statusStrip1", false) [0];

			toolsPanel = (Panel)fm.Controls.Find ("toolStripPanel", false) [0];

			tools = (ToolStrip)toolsPanel.Controls.Find ("toolStrip1", false) [0];

			menu = (MenuStrip)fm.Controls.Find ("menuStrip1", false) [0];

			ToolRenderer toolrenderer = new ToolRenderer ();
			tools.Renderer = toolrenderer;
			tools.Paint += ToolStripPanelOnPaint;

			outputWindow = (Control)workbench.OutputWindow;
			output_panel2 = (Panel)outputWindow.Controls.Find ("panel2", false) [0];
			output_panel6 = (Panel)output_panel2.Controls.Find ("panel6", false) [0];
			output_output = (RichTextBox)output_panel6.Controls.Find ("outputTextBox", false) [0];
			output_input = (Panel)output_panel2.Controls.Find ("InputPanel", false) [0];
			output_panel4 = (Panel)output_input.Controls.Find ("panel4", false) [0];
			output_panel3 = (Panel)output_input.Controls.Find ("panel3", false) [0];
			output_panel5 = (Panel)output_panel4.Controls.Find ("panel5", false) [0];
			output_panel1 = (Panel)output_panel4.Controls.Find ("panel1", false) [0];
			output_text = (TextBox)output_panel5.Controls.Find ("InputTextBox", false) [0];
			output_text.BorderStyle = BorderStyle.FixedSingle;
			output_output.BorderStyle = BorderStyle.None;
			output_input.BorderStyle = BorderStyle.None;

			ErrorsListWindowForm erw = (ErrorsListWindowForm)workbench.ErrorsListWindow;
			errorsList = (ListView)erw.Controls.Find ("lvErrorsList", false) [0];
			errorsList.OwnerDraw = true;
			errorsList.DrawColumnHeader += errorListHeaderDrawer;
			errorsList.DrawItem += (sender, e) =>
			{
				e.DrawDefault = true;
			};

			foreach (Control o in output_panel1.Controls)
			{
				if (o is Button)
				{
					Button b = (Button)o;
					b.BackColor = bgdef;
					b.FlatStyle = FlatStyle.Flat;
					b.FlatAppearance.BorderColor = bgBorder;
				}
			}

			FieldInfo fp = typeof (Form1).GetField ("OutputTextBoxs", BindingFlags.Instance | BindingFlags.NonPublic);

			OutputTextBoxs = (Dictionary <ICodeFileDocument, RichTextBox>)fp.GetValue (fm);
		}
		
		private void OpenInExplorer (object sender, EventArgs e)
		{
			//MessageBox.Show (fm.CurrentCodeFileDocument.FileName);
			if (CheckAvailabilityOfDocument ())
				System.Diagnostics.Process.Start("explorer.exe", string.Format("/select, \"{0}\"", fm.CurrentCodeFileDocument.FileName));
		}

		private bool CheckAvailabilityOfDocument ()
		{
			return File.Exists(fm.CurrentCodeFileDocument.FileName);
		}

		private void setMargin ()
		{
			foreach (AbstractMargin margins in textArea.LeftMargins)
			{
				if (margins is IconBarMargin)
				{
					margin = (IconBarMargin) margins;
				}else if (margins is FoldMargin)
				{
					foldmargin = (FoldMargin) margins;
				}
			}
		}

		private void setMarginPosition ()
		{
			int currentXPos = 0;
			foreach (AbstractMargin margins in textArea.LeftMargins)
			{
				Rectangle marginRectangle = new Rectangle (currentXPos, 0, margins.Size.Width, textArea.Height);
				if (margins.IsVisible || margins is FoldMargin)
				{
					currentXPos += margins.DrawingPosition.Width;
				}

				if (margins is FoldMargin)
				{
					if (marginRectangle != margin.DrawingPosition)
					{
						// Be sure that the line has valid rectangle
						foldmargin.DrawingPosition = marginRectangle;
					}

					break;
				}

			}
		}

		public void ReleaseResources ()
		{
			
			if (img != null)
			{
				img.Dispose ();
				img = null;
			}
			
			if (sticker != null)
			{
				sticker.Dispose ();
				sticker = null;
				stickerControl.img = null;
			}
		}

		
		public void openUpdate ()
		{
			string version = Settings.current_version.ToString ("0.0").Replace (',', '.');
			if (Settings.current_version_add != null && Settings.current_version_add.Length > 1)
				version += "-" + Settings.current_version_add;
			AddTabWithUrl (fm.MainDockPanel, yukiThemeUpdate, $"https://dragon-0609.github.io/Yuki-Theme/updates/{version}");
		}

		private void GetWindowProperities ()
		{
			Props prop = new Props ();
			prop.root = fm;
			prop.propertyGrid1.SelectedObject = fm;
			prop.Show ();
		}
		
		private void CaretOnPositionChanged (object sender, EventArgs e)
		{
			ErrorLineBookmarkNew.Remove ();
		}
		
		private void CaretPositionChangedEventHandler (object sender, EventArgs e)
		{
			if (!VisualPABCSingleton.MainForm.UserOptions.HighlightOperatorBrackets ||
			    WorkbenchServiceFactory.DebuggerManager.IsRunning)
				return;
			CodeCompletionHighlighter.UpdateMarkers (textEditor.ActiveTextAreaControl.TextArea);
		}

		private void addToSettings ()
		{
			var getopt = fm.GetType ().GetField ("optionsContentEngine", BindingFlags.NonPublic | BindingFlags.Instance);
			OptionsContentEngine options = (OptionsContentEngine) getopt.GetValue (fm);
			options.AddContent (new Controls.PluginOptionsContent (this));
		}
		
		#region Inspection

		private void SubscribeCompletion ()
		{
			// Console.WriteLine ("Unsubscribed");
			CodeCompletionKeyHandler handler = new CodeCompletionKeyHandler (textEditor);
			ht [textEditor] = handler;

			textEditor.ActiveTextAreaControl.TextArea.KeyEventHandler += handler.TextAreaKeyEventHandler;
			textEditor.Disposed += handler.CloseCodeCompletionWindow;
			ChangeEditorShortcutForCompletition ();

			EventAdd (typeof (Form1), "TextArea_KeyEventHandler", fm, typeof (TextArea), "KeyEventHandler",
			          textEditor.ActiveTextAreaControl.TextArea);
		}
		
		private bool Unsubscribe (object target)
		{
			// Console.WriteLine (target.GetType ().Name);
			MethodInfo handler = target.GetType ().GetMethod ("TextAreaKeyEventHandler", BindingFlags.Instance | BindingFlags.NonPublic);
			if (handler == null)
			{
				return false;
			}

			handler = null;
			
			EventRemove (target.GetType (), "TextAreaKeyEventHandler", target, typeof (TextArea), "KeyEventHandler",
			             textEditor.ActiveTextAreaControl.TextArea);
			
			EventRemove (typeof (Form1), "TextArea_KeyEventHandler", fm, typeof (TextArea), "KeyEventHandler",
			             textEditor.ActiveTextAreaControl.TextArea);

			EventRemove (target.GetType (), "CloseCodeCompletionWindow", target, typeof (TextEditorControl), "Disposed",
			             textEditor.ActiveTextAreaControl.TextArea);
			
			return true;
		}
		
		private void ChangeEditorShortcutForCompletition ()
		{
			ChangeShortcut (Keys.Space | Keys.Control, new CodeCompletionAllNames ());
		}
		
		void InspectBrackets ()
		{
			textArea.Paint += InspectBracketsPaint;
		}

		void StopInspectBrackets ()
		{
			textArea.Paint -= InspectBracketsPaint;
		}

		private void InspectBracketsPaint (object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			if (textArea.TextView.Highlight != null && textArea.TextView.Highlight.OpenBrace != null &&
			    textArea.TextView.Highlight.CloseBrace != null)
			{
				int lineNumber = textArea.Caret.Line;
				LineSegment currentLine    = textArea.Document.GetLineSegment(lineNumber);
				int currentWordOffset = 0;
				int startColumn = 0;

				if (textEditor.TextEditorProperties.EnableFolding)
				{
					List<FoldMarker> starts = textArea.Document.FoldingManager.GetFoldedFoldingsWithStartAfterColumn(lineNumber, startColumn - 1);
					if (starts != null && starts.Count > 0) {
						FoldMarker firstFolding = (FoldMarker)starts[0];
						foreach (FoldMarker fm in starts) {
							if (fm.StartColumn < firstFolding.StartColumn) {
								firstFolding = fm;
							}
						}
						startColumn     = firstFolding.EndColumn;
					}
				}

				MethodInfo draw = typeof (TextView).GetMethod ("DrawDocumentWord", BindingFlags.NonPublic | BindingFlags.Instance);
				
				TextWord currentWord;
				int drawingX = textArea.TextView.DrawingPosition.X - textArea.VirtualTop.X;
				for (int wordIdx = 0; wordIdx < currentLine.Words.Count; wordIdx++)
				{
					currentWord = currentLine.Words[wordIdx];
					
					if (currentWordOffset < startColumn) {
						currentWordOffset += currentWord.Length;
						continue;
					}
					if (textArea.TextView.Highlight.OpenBrace.Y == lineNumber && textArea.TextView.Highlight.OpenBrace.X == currentWordOffset ||
					    textArea.TextView.Highlight.CloseBrace.Y == lineNumber && textArea.TextView.Highlight.CloseBrace.X == currentWordOffset)
					{
						int xpos = textArea.TextView.GetDrawingXPos (lineNumber, currentWordOffset) + drawingX;
						int liny = textArea.TextView.DrawingPosition.Top + (lineNumber - textArea.TextView.FirstVisibleLine) * textArea.TextView.FontHeight - textArea.TextView.VisibleLineDrawingRemainder;
						draw.Invoke (textArea.TextView, new object []
						{
							g,
							currentWord.Word,
							new Point (xpos, liny),
							currentWord.GetFont (textArea.TextView.TextEditorProperties.FontContainer),
							currentWord.Color,
							typeBrush
						});
					}
					currentWordOffset += currentWord.Length;
				}

			}
		}
		
		private void InjectCodeCompletion ()
		{
			var assembly = typeof (Form1).Assembly;
			Type type = assembly.GetType ("VisualPascalABC.CodeCompletionKeyHandler");
			ht = (Hashtable)type.GetField ("ht",
			                               BindingFlags.NonPublic | BindingFlags.Static).GetValue (null);
			// Console.WriteLine ("Unsubscribing: ");
			Unsubscribe (ht [textEditor]);
			SubscribeCompletion ();
		}
		
		#endregion
		
		#region Helper Methods

		private void ChangeShortcut (Keys key, IEditAction val)
		{
			var fdactions = textEditor.GetType ()
			                          .GetField ("editactions", BindingFlags.NonPublic | BindingFlags.Instance);
			Dictionary <Keys, IEditAction> actions = (Dictionary <Keys, IEditAction>)fdactions.GetValue (textEditor);
			actions [key] = val;
		}
		
		private void EventAdd (Type typeForMethod, string nameOfMethod, object objectOfMethod, Type typeForEvent, string nameOfEvent, object objectOfEvent)
		{
			MethodInfo keyEventHandler = typeForMethod.GetMethod (nameOfMethod, BindingFlags.Instance | BindingFlags.NonPublic);
			EventInfo evt = typeForEvent.GetEvent (nameOfEvent, BindingFlags.Instance | BindingFlags.Public);
			MethodInfo addMethod = evt.GetAddMethod (false);

			addMethod.Invoke (objectOfEvent, new object []
			{
				Delegate.CreateDelegate (evt.EventHandlerType, objectOfMethod, keyEventHandler)
			});
		}

		private void EventRemove (Type typeForMethod, string nameOfMethod, object objectOfMethod, Type typeForEvent, string nameOfEvent, object objectOfEvent)
		{
			MethodInfo keyEventHandler = typeForMethod.GetMethod (nameOfMethod, BindingFlags.Instance | BindingFlags.NonPublic);
			EventInfo evt = typeForEvent.GetEvent (nameOfEvent, BindingFlags.Instance | BindingFlags.Public);
			MethodInfo addMethod = evt.GetRemoveMethod (false);

			addMethod.Invoke (objectOfEvent, new object []
			{
				Delegate.CreateDelegate (evt.EventHandlerType, objectOfMethod, keyEventHandler)
			});
		}
		
		private void ResetBrush (ref Brush brush, Color color)
		{
			if (brush != null) brush.Dispose ();
			brush = new SolidBrush (color);
		}
		
		private void ResetPen (ref Pen pen, Color color, float width, PenAlignment alignment)
		{
			if (pen != null) pen.Dispose ();
			pen = new Pen (color, width) {Alignment = alignment};
		}
		
		public void AddTabWithUrl(DockPanel tabControl, string title, string url)
		{
			WebBrowserControl tp = null; //new UpdatePageControl();
			FieldInfo field = typeof (Form1).GetField ("OpenBrowserDocuments", BindingFlags.Instance | BindingFlags.NonPublic);
			Dictionary <string, WebBrowserControl> OpenBrowserDocuments = (Dictionary <string, WebBrowserControl>)field.GetValue (fm);
			if (!OpenBrowserDocuments.TryGetValue(title, out tp))
			{
				tp = new UpdatePageControl();
				tp.OpenPage(title, url);
				fm.AddWindowToDockPanel(tp, tabControl, tp.Dock, DockState.Document, tp.IsFloat, null, 0);
				OpenBrowserDocuments.Add(title, tp);
			}
			else if (tp is UpdatePageControl)
			{
				tp.Activate ();
			} else
			{
				MessageBox.Show (CLI.Translate ("plugin.browser.error"));
			}
		}
		
		
		private bool IsUpdated ()
		{
			bool updated = false;
			RegistryKey ke =
				Registry.CurrentUser.CreateSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
			
			if ((string)ke?.GetValue ("cli_update", "null") != "null")
				ke.DeleteValue ("cli_update");

			int inst = ke?.GetValue ("install") != null ? 1 : 0;
			if (inst == 1)
			{
				openUpdate ();
				ke?.DeleteValue ("install");
				updated = true;
			}

			return updated;
		}

		private void FmOnClosing (object sender, CancelEventArgs e)
		{
			if (System.Windows.Application.Current != null)
				System.Windows.Application.Current.Shutdown ();
		}
		
		#endregion
		
		public event ColorUpdate OnColorUpdate;
	}
}