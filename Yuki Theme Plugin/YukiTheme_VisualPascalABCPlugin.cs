﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Xml;
using ICSharpCode.TextEditor.Document;
using Svg;
using VisualPascalABC;
using VisualPascalABC.OptionsContent;
using VisualPascalABCPlugins;
using WeifenLuo.WinFormsUI.Docking;
using Yuki_Theme.Core;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.Interfaces;
using Yuki_Theme.Core.Utils;
using Yuki_Theme.Core.WPF;
using Yuki_Theme.Core.WPF.Controls;
using Yuki_Theme.Core.WPF.Windows;
using Yuki_Theme_Plugin.Communication;
using Yuki_Theme_Plugin.Controls.DockStyles;
using Yuki_Theme_Plugin.Controls.Helpers;
using Yuki_Theme_Plugin.Interfaces;
using MessageBox = System.Windows.Forms.MessageBox;
using Point = System.Drawing.Point;
using Resources = Yuki_Theme_Plugin.Properties.Resources;
using Size = System.Drawing.Size;
using Timer = System.Windows.Forms.Timer;
using static Yuki_Theme.Core.Communication.MessageTypes;
using Message = Yuki_Theme.Core.Communication.Message;

namespace Yuki_Theme_Plugin
{
	public class YukiTheme_VisualPascalABCPlugin : IVisualPascalABCPlugin, IColorUpdatable, Plugin.IView
	{
		public string Name => "Yuki Theme";

		public string Version =>
			SettingsConst.CURRENT_VERSION.ToString ("0.0", System.Globalization.CultureInfo.InvariantCulture);

		public string Copyright => "Dragon-LV";

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

		private       IconManager       manager;
		public static ToolBarCamouflage camouflage;
		public static PluginColors      Colors = new PluginColors ();
		internal      ThemeSwitcher     switcher;
		internal      EditorInspector   inspector;
		internal      IdeComponents     ideComponents = new IdeComponents ();

		
		internal PluginModel  _model = new PluginModel ();
		internal PluginHelper _helper;

		private bool isCommonAPI;

		internal bool   bgImage => Settings.bgImage;
		internal int    imagesEnabled;   // Is enabled Colors.bg image and (or) sticker
		internal bool   StatusBarNameEnabled; // Name in status bar
		private  int    lastFocused  = -1;

		public PopupController popupController;

		public static YukiTheme_VisualPascalABCPlugin plugin;

		internal Client _client;

		private MainWindow CoreWindow;

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
			_helper = new PluginHelper (ideComponents);
			
			InitAPI ();
			
			Settings.translation.TryToGetLanguage = GetDefaultLocalization;
			Settings.ConnectAndGet ();

			imagesEnabled = 0;
			imagesEnabled += Settings.bgImage ? 1 : 0;
			imagesEnabled += Settings.swSticker ? 2 : 0;
			StatusBarNameEnabled = Settings.swStatusbar;
			
			ideComponents.WriteToConsole ("Initialization started.");
			
			loadWithWaiting ();
			Initialize ();
		}


		#region Initialization

		private void Initialize ()
		{
			LoadColors ();
			ideComponents.fm.AllowTransparency = true;
			popupController = new PopupController (ideComponents.fm, this);
			// popupController = new PopupFormsController (ideComponents.fm, this);
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
			if (!isCommonAPI)
				_client.SendMessage (OPEN_MAIN_WINDOW);
			else
			{
				InitMainWindow ();
			}
		}

		private void InitMainWindow ()
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
			
			_helper.ShowLicense ();
			/*MForm.showLicense (Colors.bg, Colors.clr, Colors.bgClick, ideComponents.fm);
			MForm.showGoogleAnalytics (Colors.bg, Colors.clr, Colors.bgClick, ideComponents.fm);
			MForm.TrackInstall ();*/
			AdditionalTools.ShowLicense (WPFHelper.GenerateTag, null, ideComponents.fm);
			
			AdditionalTools.TrackInstall (ideComponents.fm);
			if (!_helper.IsUpdated () && Settings.update)
			{
				popupController.CheckUpdate ();
			}
			
			ToolBarListItem.camouflage = camouflage;
			ToolBarListItem.manager = manager;
			SettingsPanelUtilities.items = camouflage.items;
		}

		private void loadSVG ()
		{
			if (currentThemeName.Image != null)
				currentThemeName.Image.Dispose ();
			SvgDocument svg = Helper.LoadSvg ("favorite", Assembly.GetExecutingAssembly (),
			                                  "Yuki_Theme_Plugin.Resources");
			svg.Fill = new SvgColourServer (Colors.bgBorder);
			svg.Stroke = new SvgColourServer (Colors.bgBorder);
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
			Colors.bgdef = highlighting.GetColorFor ("Default").BackgroundColor;
			Colors.bg = Helper.DarkerOrLighter (Colors.bgdef, 0.05f);
			
			Colors.bgClick = Helper.DarkerOrLighter (Colors.bgdef, 0.25f);
			Colors.bgClick2 = Helper.DarkerOrLighter (Colors.bgdef, 0.4f);
			Colors.bgClick3 = Helper.DarkerOrLighter (Colors.bgdef, 0.1f);
			Colors.bgSelection = highlighting.GetColorFor ("Selection").BackgroundColor;
			
			Colors.bgInactive = Helper.ChangeColorBrightness (Colors.bgdef, -0.3f);
			Colors.bgBorder = highlighting.GetColorFor ("CaretMarker").Color;
			Colors.bgType = highlighting.GetColorFor ("EOLMarkers").Color;

			Color defaultForeground = highlighting.GetColorFor ("Default").Color;
			Colors.clr = Helper.DarkerOrLighter (defaultForeground, 0.2f);
			Colors.clrHover = Helper.DarkerOrLighter (defaultForeground, 0.6f);
			Colors.clrKey = highlighting.GetColorFor ("Keywords").Color;
			
			
			ColorKeeper.bgColor = Colors.bg;
			ColorKeeper.bgdefColor = Colors.bgdef;
			ColorKeeper.bgClick = Colors.bgClick;
			ColorKeeper.bgBorder = Colors.bgBorder;
			ColorKeeper.selectionColor = Colors.bgSelection;
			
			ColorKeeper.fgColor = Colors.clr;
			ColorKeeper.fgHover = Helper.DarkerOrLighter (defaultForeground, 0.4f);
			ColorKeeper.fgKeyword = Colors.clrKey;
			// Helper.selectionColor = highlighting.GetColorFor ("Selection").BackgroundColor;
			
			_helper.ResetBrushesAndPens ();
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


						if (CentralAPI.Current.currentTheme.StickerOpacity != 100)
						{
							sticker = Helper.SetOpacity (stckr, CentralAPI.Current.currentTheme.StickerOpacity);
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
		
		private void InitAPI ()
		{
			AdminTools tools = new AdminTools ();
			if (tools.IsUACEnabled && _helper.IsInProgramFiles ())
			{
				InitCommunicator ();
				if (!isCommonAPI)
					CentralAPI.Current = new ClientAPI ();
			} else
			{
				isCommonAPI = true;
				CentralAPI.Current = new CommonAPI ();
			}
		}

		private void InitCommunicator ()
		{
			if (!isCommonAPI)
			{
				_client = new Client (ideComponents, _helper);

				isCommonAPI = CentralAPI.Current is not ClientAPI;
				if (!isCommonAPI)
				{
					ClientAPI api = (ClientAPI)CentralAPI.Current;
					api.Client = _client;
					api.AddEvents ();
					api.AddEvent (RELEASE_RESOURCES, ReleaseResourcesForServer);
					api.AddEvent (APPLY_THEME, _ => ReloadLayout ());
					api.AddEvent (APPLY_THEME_LIGHT, _ => ReloadLayoutLight ());
				}
			}
		}

		#endregion


		#region Server Actions
		
		private void ReleaseResourcesForServer (Message obj)
		{
			ReleaseResources ();
			_client.SendMessage (RELEASE_RESOURCES_OK);
		}

		#endregion
		
		#region Updates

		private void UpdateColors ()
		{
			WPFHelper.ConvertGUIColorsNBrushes ();
			if (OnColorUpdate != null)
				OnColorUpdate (Colors.bgdef, Colors.clr, Colors.bgClick);
			// popupController.TryToUpdateNotificationWindow ();
			WaitAndUpdateMenuColors ();

			manager.UpdateColors ();
			ideComponents.UpdateColors ();
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
			StatusBarNameEnabled = Settings.swStatusbar;
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
			if (switcher is { panel_bg: { } })
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
				item.BackColor = Colors.bg;
				item.ForeColor = Colors.clr;
				foreach (ToolStripItem downItem in item.DropDownItems)
				{
					downItem.BackColor = Colors.bg;
					downItem.ForeColor = Colors.clr;
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
			OpenBrowserDocuments.TryGetValue (PluginHelper.UPDATE_TITLE, out tp);
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
			logoBox.BackColor = Colors.bgdef;
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

		#endregion

		public event ColorUpdate OnColorUpdate;
	}
}