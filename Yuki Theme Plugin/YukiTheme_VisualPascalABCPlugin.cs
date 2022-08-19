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
using System.Windows.Interop;
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
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.Database;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Interfaces;
using Yuki_Theme.Core.Utils;
using Yuki_Theme.Core.WPF;
using Yuki_Theme.Core.WPF.Controls;
using Yuki_Theme.Core.WPF.Windows;
using Yuki_Theme_Plugin.Communication;
using Yuki_Theme_Plugin.Controls.CodeCompletion;
using Yuki_Theme_Plugin.Controls.DockStyles;
using Yuki_Theme_Plugin.Controls.Helpers;
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
			SettingsConst.CURRENT_VERSION.ToString ("0.0", System.Globalization.CultureInfo.InvariantCulture);

		public string Copyright => "Dragon-LV";

		#region Colors, Brushes and Pens

		public static Color bg;
		public static Color bgdef;
		public static Color bgSelection;
		public static Color bgClick;
		public static Color bgClick2;
		public static Color bgClick3;
		public static Color bgInactive;
		public static Color clr;
		public static Color clrKey;
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

		public static Image     img;
		internal      Alignment wallpaperAlign;
		private       int       wallpaperOpacity;
		private       Timer     menuItemsColorUpdator;

		private Timer tim3;

		internal ToolStripButton       currentThemeName;
		private  PictureBox            logoBox;
		private  Image                 sticker;
		public   CustomPicture         stickerControl;
		internal IHighlightingStrategy highlighting;
		
		public  Image tmpImage1;
		public  Image tmpImage2;

		private       IconManager       manager;
		public static ToolBarCamouflage camouflage;
		internal      ThemeSwitcher     switcher;
		internal      EditorInspector   inspector;
		internal      IdeComponents       ideComponents = new IdeComponents ();

		internal bool   bgImage => Settings.bgImage;
		internal int    imagesEnabled   = 0;     // Is enabled bg image and (or) sticker
		internal bool   nameInStatusBar = false; // Name in status bar
		const    string yukiThemeUpdate = "Yuki Theme Update";
		private  int    lastFocused     = -1;

		public PopupController popupController;

		public static YukiTheme_VisualPascalABCPlugin plugin;

		private MainWindow CoreWindow;

		private Server _server;

		/// <summary>
		///     The main entry point for the application.
		/// </summary>
		public YukiTheme_VisualPascalABCPlugin (IWorkbench workbench)
		{
			ideComponents.workbench = workbench;
		}

		public YukiTheme_VisualPascalABCPlugin (IVisualEnvironmentCompiler compiler)
		{
			ideComponents.compiler = compiler;
		}

		public void GetGUI (List <IPluginGUIItem> MenuItems, List <IPluginGUIItem> ToolBarItems)
		{
			var item1 = new PluginGUIItem ("Yuki Theme", "Yuki Theme", null, Color.Transparent, InitCore);
			//Добавляем в меню
			MenuItems.Add (item1);
			plugin = this;
			
			ideComponents.fm = (Form1)ideComponents.workbench.MainForm;
			Helper.mode = ProductMode.Plugin;
			API.Current = new ServerAPI ();
			Settings.translation.TryToGetLanguage = GetDefaultLocalization;
			Settings.ConnectAndGet ();

			imagesEnabled = 0;
			imagesEnabled += Settings.bgImage ? 1 : 0;
			imagesEnabled += Settings.swSticker ? 2 : 0;
			nameInStatusBar = Settings.swStatusbar;
			
			ideComponents.WriteToConsole ("Initialization started.");
			
			loadWithWaiting ();
			Initialize ();
		}


		#region Initialization

		private void Initialize ()
		{
			ideComponents.fm.AllowTransparency = true;
			popupController = new PopupController (ideComponents.fm, this);
			// popupController = new PopupFormsController (ideComponents.fm, this);
			LoadColors ();
			LoadImage ();

			ideComponents.Initialize ();

			inspector = new EditorInspector (this);
			inspector.InspectBrackets ();

			manager = new IconManager (ideComponents.tools, ideComponents.menu, ideComponents.context, ideComponents.context2, ideComponents.fm);
			camouflage = new ToolBarCamouflage (ideComponents.tools);
			switcher = new ThemeSwitcher (this);

			UpdateColors ();

			WaitAndUpdateMenuColors ();
			API_Events.onBGIMAGEChange = RefreshEditor;
			API_Events.onSTICKERChange = ReloadSticker;
			API_Events.onSTATUSChange = RefreshStatusBar;

			Helper.LoadCurrent ();

			currentThemeName = new ToolStripButton ();
			currentThemeName.Alignment = ToolStripItemAlignment.Right;
			loadSVG ();

			currentThemeName.Padding = new Padding (2, 0, 2, 0);
			currentThemeName.Margin = Padding.Empty;
			ideComponents.statusBar.Items.Add (currentThemeName);
			RefreshStatusBar ();

			InitializeSticker ();
			ideComponents.fm.Resize += FmOnResize;
			addToSettings ();
		}

		private void InitStyles ()
		{
			Extender.SetSchema (ideComponents.fm.MainDockPanel);
		}

		private void InitCore ()
		{
			WPFHelper.InitAppForWinforms ();
			if (CoreWindow == null || PresentationSource.FromVisual (CoreWindow) == null)
			{
				CoreWindow = new MainWindow ();
				CoreWindow.Model.StartSettingTheme += ReleaseResources;
				CoreWindow.Model.SetTheme += ReloadLayout;
				
				CoreWindow.Closed += (_, _) =>
				{
					GC.Collect ();
					GC.WaitForPendingFinalizers ();
				};
			} else
			{
				if (CoreWindow.IsVisible)
				{
					CoreWindow.Activate ();
					return;
				}
			}

			CoreWindow.Show ();
		}

		private void InitializeSticker ()
		{
			stickerControl = new CustomPicture (ideComponents.fm);
			stickerControl.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			stickerControl.margin = new Point (10, ideComponents.statusBar.Size.Height);
			stickerControl.Enabled = Settings.positioning;
			CustomPanel pnl = new CustomPanel (1) { Visible = false, Name = "LayerGrids" };
			pnl.pict = stickerControl;
			ideComponents.fm.Controls.Add (pnl);
			ideComponents.fm.Controls.Add (stickerControl);
			stickerControl.pnl = pnl;
			LoadSticker ();
			stickerControl.BringToFront ();
		}

		private void loadWithWaiting ()
		{
			tim3 = new Timer () { Interval = 2200 };
			tim3.Tick += load;
			if (Settings.swLogo)
			{
				showLogo ();
			} else
				InitStyles ();

			tim3.Start ();
		}

		/// <summary>
		/// Look for item in ideComponents.menu. After that add additional items
		/// </summary>
		private void load (object sender, EventArgs e)
		{
			tim3.Stop ();
			if (Settings.swLogo)
			{
				hideLogo ();
				InitStyles ();
			}

			ideComponents.AddMenuItems ();

			ideComponents.WriteToConsole ("Initialization finished.");

			inspector.InjectCodeCompletion ();
			
			ShowLicense ();
			/*MForm.showLicense (bg, clr, bgClick, ideComponents.fm);
			MForm.showGoogleAnalytics (bg, clr, bgClick, ideComponents.fm);
			MForm.TrackInstall ();*/
			AdditionalTools.ShowLicense (WPFHelper.GenerateTag, null, ideComponents.fm);
			
			AdditionalTools.TrackInstall (ideComponents.fm);
			if (!IsUpdated () && Settings.update)
			{
				popupController.CheckUpdate ();
			}
			
			ToolBarListItem.camouflage = camouflage;
			ToolBarListItem.manager = manager;
			SettingsPanelUtilities.items = camouflage.items;
			InitCommunicator ();
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


			if (Helper.currentTheme.Contains (":"))
			{
				string [] spl = Helper.currentTheme.Split (':');
				currentThemeName.Text = spl [spl.Length - 1];
				spl = null;
			} else
				currentThemeName.Text = Helper.currentTheme;

			ideComponents.UpdateMenuItemIcons ();
		}

		private void LoadColors ()
		{
			highlighting = HighlightingManager.Manager.FindHighlighterForFile ("A.pas");
			bgdef = highlighting.GetColorFor ("Default").BackgroundColor;
			bg = Helper.DarkerOrLighter (bgdef, 0.05f);
			
			bgClick = Helper.DarkerOrLighter (bgdef, 0.25f);
			bgClick2 = Helper.DarkerOrLighter (bgdef, 0.4f);
			bgClick3 = Helper.DarkerOrLighter (bgdef, 0.1f);
			bgSelection = highlighting.GetColorFor ("Selection").BackgroundColor;
			
			bgInactive = Helper.ChangeColorBrightness (bgdef, -0.3f);
			bgBorder = highlighting.GetColorFor ("CaretMarker").Color;
			bgType = highlighting.GetColorFor ("EOLMarkers").Color;

			Color defaultForeground = highlighting.GetColorFor ("Default").Color;
			clr = Helper.DarkerOrLighter (defaultForeground, 0.2f);
			clrHover = Helper.DarkerOrLighter (defaultForeground, 0.6f);
			clrKey = highlighting.GetColorFor ("Keywords").Color;
			
			
			Helper.bgColor = bg;
			Helper.bgdefColor = bgdef;
			Helper.bgClick = bgClick;
			Helper.bgBorder = bgBorder;
			Helper.selectionColor = bgSelection;
			
			Helper.fgColor = clr;
			Helper.fgHover = Helper.DarkerOrLighter (defaultForeground, 0.4f);
			Helper.fgKeyword = clrKey;
			// Helper.selectionColor = highlighting.GetColorFor ("Selection").BackgroundColor;
			
			ResetBrushesAndPens ();
		}

		private void LoadImage ()
		{
			bool dispI = false;
			if (img != null)
			{
				img.Dispose ();
				dispI = true;
			}

			string pth = Path.Combine (Settings.pascalPath, "Highlighting", "background.png");
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
								wallpaperAlign = (Alignment)int.Parse (comm.Value.Substring (6));
							} else if (comm.Value.StartsWith ("opacity"))
							{
								wallpaperOpacity = int.Parse (comm.Value.Substring (8));
							}
						}

						break;
					}
				}

				if (wallpaperOpacity != 100)
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
			}
			
			if (Settings.swSticker)
			{
				if (Settings.useCustomSticker && File.Exists (Settings.customSticker))
				{
					sticker = Image.FromFile (Settings.customSticker);
				} else
				{
					string pth = Path.Combine (Settings.pascalPath, "Highlighting", "sticker.png");
					if (File.Exists (pth))
					{
						Image stckr = Image.FromFile (pth);


						if (API.Current.currentTheme.StickerOpacity != 100)
						{
							sticker = Helper.SetOpacity (stckr, API.Current.currentTheme.StickerOpacity);
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

		private void InitCommunicator ()
		{
			_server = new Server (ideComponents);
		}
		
		#endregion

		
		#region Updates

		private void UpdateColors ()
		{
			WPFHelper.ConvertGUIColorsNBrushes ();
			if (OnColorUpdate != null)
				OnColorUpdate (bgdef, clr, bgClick);
			// popupController.TryToUpdateNotificationWindow ();
			WaitAndUpdateMenuColors ();

			manager.UpdateColors ();
			ideComponents.UpdateColors ();
		}

		private void ResetBrushesAndPens ()
		{
			ResetBrush (ref bgdefBrush, bgdef);
			ResetBrush (ref bgBrush, bg);
			ResetBrush (ref selectionBrush, bgSelection);
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
			ideComponents.textArea.Refresh ();
			imagesEnabled = 0;
			imagesEnabled += Settings.bgImage ? 1 : 0;
			imagesEnabled += Settings.swSticker ? 2 : 0;
		}

		public void ReloadLayout ()
		{
			ReloadLayoutAll (false);
		}

		public void ReloadLayoutLight ()
		{
			ReloadLayoutAll (true);
			switcher.panel_bg.Visible = false;

			Timer tim = new Timer ();
			tim.Interval = 5;
			tim.Tick += (sender, args) =>
			{
				tim.Stop ();
				switcher.SetFocus ();
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
			ideComponents.fm.Refresh ();
			loadSVG ();
			ideComponents.updateQuietImage ();
			ideComponents.ReloadLayout ();
			UpdateWebBrowserTheme ();
		}


		private void FmOnResize (object sender, EventArgs e)
		{
			switcher.CloseOnClick (sender, e);
		}

		private void WaitAndUpdateMenuColors ()
		{
			menuItemsColorUpdator = new Timer () { Interval = 1000 };
			menuItemsColorUpdator.Tick += UpdateColorsOfMenuItems;
			menuItemsColorUpdator.Start ();
		}

		private void UpdateColorsOfMenuItems (object sender, EventArgs e)
		{
			foreach (ToolStripMenuItem item in ideComponents.menu.Items)
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

		private void UpdateWebBrowserTheme ()
		{
			WebBrowserControl tp = null;
			FieldInfo field = typeof (Form1).GetField ("OpenBrowserDocuments", BindingFlags.Instance | BindingFlags.NonPublic);
			Dictionary <string, WebBrowserControl> OpenBrowserDocuments =
				(Dictionary <string, WebBrowserControl>)field.GetValue (ideComponents.fm);
			OpenBrowserDocuments.TryGetValue (yukiThemeUpdate, out tp);
			if (tp is UpdatePageControl)
			{
				UpdatePageControl upage = (UpdatePageControl)tp;
				upage.UpdateTheme ();
			}
		}

		#endregion


		#region Logo Management

		private void showLogo ()
		{
			logoBox = new PictureBox ();
			logoBox.BackColor = bgdef;
			logoBox.Image = Resources.YukiTheme;
			logoBox.Location = new Point (ideComponents.fm.ClientSize.Width / 2 - 50, ideComponents.fm.ClientSize.Height / 2 - 50);
			logoBox.Name = "logoBox";
			logoBox.Size = new Size (100, 100);
			logoBox.SizeMode = PictureBoxSizeMode.Zoom;
			logoBox.TabIndex = 0;
			logoBox.TabStop = false;
			ideComponents.fm.Controls.Add (logoBox);
		}

		private void hideLogo ()
		{
			ideComponents.fm.Controls.Remove (logoBox);
			logoBox.Dispose ();
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


		internal void RememberCurrentEditor ()
		{
			if (ideComponents.fm.ActiveControl is UpdatePageControl)
			{
				UpdatePageControl update = (UpdatePageControl)ideComponents.fm.ActiveControl;
				lastFocused = update.TabIndex;
			} else
			{
				lastFocused = ideComponents.fm.CurrentCodeFileDocument.TabIndex;
			}
		}

		internal void ReFocusCurrentEditor ()
		{
			IDockContent [] docs = ideComponents.fm.MainDockPanel.DocumentsToArray ();
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

		private string GetDefaultLocalization ()
		{
			return PascalABCCompiler.StringResourcesLanguage.CurrentLanguageName;
		}

		public void openUpdate ()
		{
			string version = SettingsConst.CURRENT_VERSION.ToString ("0.0").Replace (',', '.');
			if (SettingsConst.CURRENT_VERSION_ADD != null && SettingsConst.CURRENT_VERSION_ADD.Length > 1)
				version += "-" + SettingsConst.CURRENT_VERSION_ADD;
			AddTabWithUrl (ideComponents.fm.MainDockPanel, yukiThemeUpdate, $"https://dragon-0609.github.io/Yuki-Theme/updates/{version}");
		}

		private void GetWindowProperities ()
		{
			Props prop = new Props ();
			prop.root = ideComponents.fm;
			prop.propertyGrid1.SelectedObject = ideComponents.fm;
			prop.Show ();
		}
		
		private void addToSettings ()
		{
			var getopt = ideComponents.fm.GetType ().GetField ("optionsContentEngine", BindingFlags.NonPublic | BindingFlags.Instance);
			OptionsContentEngine options = (OptionsContentEngine)getopt.GetValue (ideComponents.fm);
			options.AddContent (new Controls.PluginOptionsContent (this));
		}

		internal void SettingsChanged (bool customStickerChanged, bool dimensionChanged)
		{
			if (customStickerChanged)
			{
				LoadSticker ();
			} else if (dimensionChanged)
			{
				if (stickerControl.Image != null)
					stickerControl.ReCalculatePositionNSize ();
			}
		}


		#region Helper Methods

		private void ResetBrush (ref Brush brush, Color color)
		{
			if (brush != null) brush.Dispose ();
			brush = new SolidBrush (color);
		}

		private void ResetPen (ref Pen pen, Color color, float width, PenAlignment alignment)
		{
			if (pen != null) pen.Dispose ();
			pen = new Pen (color, width) { Alignment = alignment };
		}

		public void AddTabWithUrl (DockPanel tabControl, string title, string url)
		{
			WebBrowserControl tp = null; //new UpdatePageControl();
			FieldInfo field = typeof (Form1).GetField ("OpenBrowserDocuments", BindingFlags.Instance | BindingFlags.NonPublic);
			Dictionary <string, WebBrowserControl> OpenBrowserDocuments =
				(Dictionary <string, WebBrowserControl>)field.GetValue (ideComponents.fm);
			if (!OpenBrowserDocuments.TryGetValue (title, out tp))
			{
				tp = new UpdatePageControl ();
				tp.OpenPage (title, url);
				ideComponents.fm.AddWindowToDockPanel (tp, tabControl, tp.Dock, DockState.Document, tp.IsFloat, null, 0);
				OpenBrowserDocuments.Add (title, tp);
			} else if (tp is UpdatePageControl)
			{
				tp.Activate ();
			} else
			{
				MessageBox.Show (API.Current.Translate ("plugin.browser.error"));
			}
		}


		private bool IsUpdated ()
		{
			bool updated = false;
			int inst = Helper.RecognizeInstallationStatus ();
			if (inst == 1)
			{
				openUpdate ();
				updated = true;
				Helper.DeleteInstallationStatus ();
			}

			return updated;
		}

		private void ShowLicense ()
		{
			if (!Settings.license)
			{
				WPFHelper.InitAppForWinforms ();
				
				LicenseWindow licenseWindow = new LicenseWindow ()
				{
					Background = WPFHelper.bgBrush,
					Foreground = WPFHelper.fgBrush,
					BorderBrush = WPFHelper.borderBrush,
					Tag = WPFHelper.GenerateTag
				};
				WindowInteropHelper helper = new WindowInteropHelper (licenseWindow);
				helper.Owner = ideComponents.fm.Handle;
				
				licenseWindow.Closed += (_, _) =>
				{
					GC.Collect ();
					GC.WaitForPendingFinalizers ();
				};

				licenseWindow.ShowDialog ();
				
				Settings.license = true;
				Settings.database.UpdateData (SettingsConst.LICENSE, "True");
			}
		}

		#endregion

		public event ColorUpdate OnColorUpdate;
	}
}