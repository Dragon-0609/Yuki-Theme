using System;
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
using Yuki_Theme.Core.Communication;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.Interfaces;
using Yuki_Theme.Core.Utils;
using Yuki_Theme.Core.WPF;
using Yuki_Theme.Core.WPF.Controls;
using Yuki_Theme.Core.WPF.Windows;
using Yuki_Theme_Plugin.Communication;
using Yuki_Theme_Plugin.Controls;
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
	public class YukiTheme_VisualPascalABCPlugin : Plugin.View, IVisualPascalABCPlugin, IColorUpdatable 
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
		internal IHighlightingStrategy highlighting;

		private       IconManager       manager;
		public static PluginColors      Colors = new PluginColors ();
		internal      ThemeSwitcher     switcher;
		internal      EditorInspector   inspector;


		internal Plugin.IPresenter _presenter;

		internal bool bgImage => Settings.bgImage;
		internal int  imagesEnabled;        // Is enabled Colors.bg image and (or) sticker
		internal bool StatusBarNameEnabled; // Name in status bar

		public PopupController popupController;

		public static YukiTheme_VisualPascalABCPlugin plugin;

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
			var item1 = new PluginGUIItem ("Yuki Theme", "Yuki Theme", null, Color.Transparent, InitCoreWindow);
			//Добавляем в меню
			MenuItems.Add (item1);
			plugin = this;
			
			_presenter = new PluginPresenter (this);
			
			ideComponents.fm = (Form1)ideComponents.workbench.MainForm;
			Helper.mode = ProductMode.Plugin;
			_helper = new PluginHelper (ideComponents);

			_presenter.InitAPI ();
		}


		#region Initialization

		
		internal override void StartIntegration ()
		{
			Settings.translation.TryToGetLanguage = _helper.GetDefaultLocalization;
			Settings.Get ();

			imagesEnabled = 0;
			imagesEnabled += Settings.bgImage ? 1 : 0;
			imagesEnabled += Settings.swSticker ? 2 : 0;
			StatusBarNameEnabled = Settings.swStatusbar;
			
			OutputConsoleLogger.Console = (IConsole) ideComponents;
			Logger.Write ("Initialization started.");

			loadWithWaiting ();
			Initialize ();
		}

		private static OutputConsoleLogger Logger => OutputConsoleLogger.Instance;

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
			_model.InitSticker (ideComponents.fm);
			_model.LoadSticker ();
			// InitializeSticker ();
			ideComponents.fm.Resize += FmOnResize;
			_presenter.AddToSettings ();
			ToolBarList.InitCustomController = (list, func) => 
			list._controller = new PBarController(list, func(), plugin);
		}

		private void InitStyles ()
		{
			Extender.SetSchema (ideComponents.fm.MainDockPanel);
		}

		private void InitCoreWindow ()
		{
			if (!isCommonAPI)
			{
				Logger.Write($"Sending message: {OPEN_MAIN_WINDOW}");
				_client.SendMessage(OPEN_MAIN_WINDOW);
			}
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
				CoreWindow.Model.StartSettingTheme += _presenter.ReleaseResources;
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

		private void loadWithWaiting ()
		{
			tim3 = new Timer () { Interval = 2200 };
			tim3.Tick += load;
			if (Settings.swLogo)
			{
				_presenter.ShowLogo ();
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
				_presenter.HideLogo ();
				InitStyles ();
			}

			ideComponents.AddMenuItems ();

			Logger.Write ("Initialization finished.");

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

			if (isCommonAPI)
			{
				
			}
			else
			{
				_client.SendMessage(new Message(SET_TOOLBAR_ITEMS, camouflage.ItemInfos));
			}
			
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
			if (img != null)
			{
				img.Dispose ();
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
			_model.LoadSticker ();
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

		public override void ReloadLayout ()
		{
			ReloadLayoutAll (false);
		}

		public override void ReloadLayoutLight ()
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

		public override void ReloadLayoutAll (bool lightReload)
		{
			HighlightingManager.Manager.ReloadSyntaxModes ();
			if (!lightReload)
			{
				LoadImage ();
				_model.LoadSticker ();
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
		

		internal override void Release () => _model.Release ();

		internal override void SendMessage(Message message) => _client?.SendMessage(message);

		public event ColorUpdate OnColorUpdate;
	}
}