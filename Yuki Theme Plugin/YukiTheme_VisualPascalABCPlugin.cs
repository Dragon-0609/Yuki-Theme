using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using Svg;
using VisualPascalABC;
using VisualPascalABC.OptionsContent;
using VisualPascalABCPlugins;
using WeifenLuo.WinFormsUI.Docking;
using Yuki_Theme.Core;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.Forms;
using Yuki_Theme_Plugin.Controls;
using Timer = System.Windows.Forms.Timer;

namespace Yuki_Theme_Plugin
{
	
	public class YukiTheme_VisualPascalABCPlugin : IVisualPascalABCPlugin
	{
		public string Name => "Yuki Theme";

		public string Version =>
			SettingsForm.current_version.ToString ("0.0", System.Globalization.CultureInfo.InvariantCulture);

		public string Copyright => "Dragon-LV";
		
		private AboutBox                   about;
		private IVisualEnvironmentCompiler compiler;
		private Form1                      fm;
		private MenuStrip                  menu;
		private Panel                      output_input;
		private RichTextBox                output_output;
		private Panel                      output_panel1;
		private Panel                      output_panel2;
		private Panel                      output_panel3;
		private Panel                      output_panel4;
		private Panel                      output_panel5;
		private Panel                      output_panel6;
		private TextBox                    output_text;
		private Control                    outputWindow;
		private IHighlightingStrategy      highlighting;

		private          StatusStrip statusBar;
		private          ToolStrip   tools;
		private          Panel       toolsPanel;
		private readonly IWorkbench  workbench;

		private TextArea                          textArea;
		private CodeFileDocumentTextEditorControl textEditor;
		private ContextMenuStrip                  context;
		private ContextMenuStrip                  context2;
		private MenuRenderer                  renderer;

		private       Image           img;
		public static Color           bg;
		public static Color           bgdef;
		public static Color           bgClick;
		public static Color           bgClick2;
		public static Color           bgClick3;
		public static Color           bgInactive;
		public static Color           clr;
		public static Color           clrHover;
		public static Color           bgBorder;
		public static Color           bgType;
		public static Color           bgvruler;
		public static Brush           bgdefBrush;
		public static Brush           bgBrush;
		public static Brush           bgClickBrush;
		public static Brush           bgClick3Brush;
		public static Brush           clrBrush;
		public static Brush           bgInactiveBrush;
		public static Pen             bgPen;
		public static Pen             clrPen;
		public static Pen             bgBorderPen;
		private       Alignment       align;
		private       int             opacity;
		private       Timer           tim;
		private       Timer           tim2;
		private       Timer           tim3;
		private       IconBarMargin   margin;
		private       FoldMargin      foldmargin;
		public        MForm           mf;
		private       ListView        cr;
		private       TextBox         con;
		private       PictureBox      logoBox;
		private       ToolStripButton currentTheme;
		private       Image           sticker;
		public        CustomPicture   stickerControl;

		private ToolStripMenuItem menu_settings;
		private ToolStripMenuItem quiet;
		private ToolStripMenuItem stick;
		private ToolStripMenuItem backimage;
		private ToolStripMenuItem switchTheme;
		private ToolStripMenuItem enablePositioning;
		private Image             quietImage;
		private Image             positioningImage;
		private Image             wallpaperImage;
		private Image             switchImage;
		private Size              defaultSize;
		private Panel             panel_bg;
		private CustomList        lst;
		private Image             tmpImage1;
		private Image             tmpImage2;
		
		private       int               selectionindex;
		private       IconManager       manager;
		public static ToolBarCamouflage camouflage;
		
		private bool bgImage => CLI.bgImage;
		
		bool tg = false; // is toggle activated
		int enbld = 0; // Is enabled bg image and (or) sticker
		int nminst = 0; // Name in status bar

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
			
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (MForm));
			Icon icon = ((Icon) (resources.GetObject ($"$this.Icon")));
			var item1 = new PluginGUIItem ("Yuki Theme", "Yuki Theme", icon.ToBitmap (), Color.Transparent, InitCore);
			//Добавляем в меню
			MenuItems.Add (item1);

			fm = (Form1) workbench.MainForm;
			Helper.mode = ProductMode.Plugin;
			CLI.connectAndGet ();

			enbld = 0;
			enbld += CLI.bgImage ? 1 : 0;
			enbld += CLI.swSticker ? 2 : 0;
			nminst = CLI.swStatusbar ? 1 : 0;

			loadWithWaiting ();
			Initialize ();
		}

		private void Initialize ()
		{
			fm.AllowTransparency = true;
			
			LoadColors ();
			defaultSize = new Size (32, 32);
			
			textEditor = fm.CurrentCodeFileDocument.TextEditor;
			textArea = textEditor.ActiveTextAreaControl.TextArea;
			context = textEditor.ContextMenuStrip;
			context2 = fm.MainDockPanel.ContextMenuStrip;
			
			LoadImage ();
			

			CompilerConsoleWindowForm cons = (CompilerConsoleWindowForm) workbench.CompilerConsoleWindow;
			con = (TextBox) cons.Controls.Find ("CompilerConsole", false) [0];

			about = fm.AboutBox1;
			about.Shown += UpdateAboutForm;

			statusBar = (StatusStrip) fm.Controls.Find ("statusStrip1", false) [0];

			toolsPanel = (Panel) fm.Controls.Find ("toolStripPanel", false) [0];

			tools = (ToolStrip) toolsPanel.Controls.Find ("toolStrip1", false) [0];

			menu = (MenuStrip) fm.Controls.Find ("menuStrip1", false) [0];
			
			ToolRenderer toolrenderer = new ToolRenderer ();
			tools.Renderer = toolrenderer;
			tools.Paint += ToolStripPanelOnPaint;
			
			outputWindow = (Control) workbench.OutputWindow;
			output_panel2 = (Panel) outputWindow.Controls.Find ("panel2", false) [0];
			output_panel6 = (Panel) output_panel2.Controls.Find ("panel6", false) [0];
			output_output = (RichTextBox) output_panel6.Controls.Find ("outputTextBox", false) [0];
			output_input = (Panel) output_panel2.Controls.Find ("InputPanel", false) [0];
			output_panel4 = (Panel) output_input.Controls.Find ("panel4", false) [0];
			output_panel3 = (Panel) output_input.Controls.Find ("panel3", false) [0];
			output_panel5 = (Panel) output_panel4.Controls.Find ("panel5", false) [0];
			output_panel1 = (Panel) output_panel4.Controls.Find ("panel1", false) [0];
			output_text = (TextBox) output_panel5.Controls.Find ("InputTextBox", false) [0];
			output_text.BorderStyle = BorderStyle.FixedSingle;
			output_output.BorderStyle = BorderStyle.None;
			output_input.BorderStyle = BorderStyle.None;

			ErrorsListWindowForm erw = (ErrorsListWindowForm) workbench.ErrorsListWindow;
			cr = (ListView) erw.Controls.Find ("lvErrorsList", false) [0];
			// cr.GridLines = false;
			cr.OwnerDraw = true;
			cr.DrawColumnHeader += errorListHeaderDrawer;
			cr.DrawItem += (sender, e) =>
			{
				/*e.Graphics.DrawRectangle (clrPen, e.Bounds);
				e.Item.ImageList.Draw (e.Graphics, Point.Empty, e.Item.ImageIndex);
				StringFormat format = new StringFormat ();
				format.Trimming = StringTrimming.Character;
				e.Graphics.DrawString (e.Item.Text, e.Item.Font, clrBrush, e.Bounds, format);*/
				e.DrawDefault = true;
			};/*
			cr.DrawSubItem += (sender, e) =>
			{
				e.Graphics.DrawRectangle (clrPen, e.Bounds);
				e.Item.ImageList.Draw (e.Graphics, Point.Empty, e.Item.ImageIndex);
				
				StringFormat format = new StringFormat ();
				format.Trimming = StringTrimming.Character;
				e.Graphics.DrawString (e.SubItem.Text, e.SubItem.Font, clrBrush, e.Bounds, format);
				e.DrawDefault = false;
			};*/
			
			foreach (Control o in output_panel1.Controls)
			{
				if(o is Button)
				{
					Button b = (Button) o;
					b.BackColor = bgdef;
					b.FlatStyle = FlatStyle.Flat;
					b.FlatAppearance.BorderColor = bgBorder;
				}
			}

			setMargin ();
			textArea.Paint += PaintBG;
			textEditor.Parent.BackColor = bg;
			textEditor.Controls [1].Paint += CtrlOnPaint;
			textEditor.Controls [1].Invalidate();


			textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += CaretPositionChangedEventHandler;
			textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += CaretOnPositionChanged;
			
			fm.CurrentCodeFileDocument.BackColor = bg;
			
			tim = new Timer () {Interval = 1};
			tim.Tick += (sender, r) =>
			{
				if (textEditor != fm.CurrentCodeFileDocument.TextEditor)
				{
					textArea.Paint -= PaintBG;
					try
					{
						textEditor.Controls [1].Paint -= CtrlOnPaint;
					} catch (ArgumentOutOfRangeException)
					{

					}

					// textArea = textEditor.ActiveTextAreaControl.TextArea;
					textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged -= CaretPositionChangedEventHandler;
					textEditor = fm.CurrentCodeFileDocument.TextEditor;
					textArea = textEditor.ActiveTextAreaControl.TextArea;
					textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += CaretPositionChangedEventHandler;
					setMargin ();
					textArea.Paint += PaintBG;
					textArea.Refresh ();
					tim.Stop ();
					textEditor.Parent.BackColor = bg;
					try
					{
						textEditor.Controls [1].Paint += CtrlOnPaint;
						textEditor.Controls [1].Invalidate ();
						textEditor.Controls [1].Invalidate ();
					} catch (ArgumentOutOfRangeException)
					{

					}

					fm.CurrentCodeFileDocument.BackColor = bg;

					// MessageBox.Show (fm.CurrentCodeFileDocument.TabIndex.ToString ());
					try
					{
						if (output_panel6.Controls [fm.CurrentCodeFileDocument.TabIndex] is RichTextBox)
						{
							((RichTextBox) output_panel6.Controls [fm.CurrentCodeFileDocument.TabIndex]).BorderStyle =
								BorderStyle.None;
							((RichTextBox) output_panel6.Controls [fm.CurrentCodeFileDocument.TabIndex]).BackColor =
								bgdef;
						}
						
					} catch (ArgumentOutOfRangeException)
					{

					}
				}
			};
			
			fm.MainDockPanel.ActiveContentChanged += (sender, e) =>
			{
				tim.Start ();
			};

			renderer = new MenuRenderer ();
			menu.Renderer = renderer;
			context.Renderer = renderer;
			context2.Renderer = renderer;
			manager = new IconManager (tools, menu, fm);
			camouflage = new ToolBarCamouflage (tools);
			
			UpdateColors ();
			
			setTim2 ();
			CLI.onBGIMAGEChange = RefreshEditor;
			CLI.onSTICKERChange = ReloadSticker;
			CLI.onSTATUSChange = RefreshStatusBar;

			Helper.LoadCurrent ();
			
			currentTheme = new ToolStripButton ();
			currentTheme.Alignment = ToolStripItemAlignment.Right;
			loadSVG ();

			// statusBar.Items.Add (logo);
			currentTheme.Padding = new Padding (2, 0, 2, 0);
			currentTheme.Margin = Padding.Empty;
			statusBar.Items.Add (currentTheme);
			RefreshStatusBar ();

			stickerControl = new CustomPicture (fm);
			stickerControl.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			stickerControl.margin = new Point (10, statusBar.Size.Height);
			stickerControl.Enabled = CLI.positioning;
			// stickerControl.MouseDown += new MouseEventHandler (stickerControl_MouseDown);
			// stickerControl.MouseMove += new MouseEventHandler (stickerControl_MouseMove);
			// stickerControl.MouseUp += new MouseEventHandler (stickerControl_MouseUp);
			CustomPanel pnl = new CustomPanel(1) {Visible = false, Name = "LayerGrids"};
			pnl.pict = stickerControl;
			fm.Controls.Add (pnl);
			fm.Controls.Add (stickerControl);
			stickerControl.pnl = pnl;
			LoadSticker ();
			// stickerControl.Enabled = true;
			stickerControl.BringToFront ();
			fm.Resize += FmOnResize;
			addSettings ();
		}
		private void ReloadSticker ()
		{
			enbld = 0;
			enbld += CLI.bgImage ? 1 : 0;
			enbld += CLI.swSticker ? 2 : 0;
			LoadSticker ();
		}

		public void LoadSticker ()
		{
			if (sticker != null)
			{
				sticker.Dispose ();
				sticker = null;
			};
			if (CLI.swSticker)
			{
				if (CLI.useCustomSticker && File.Exists (CLI.customSticker))
				{
					sticker = Image.FromFile (CLI.customSticker);
				}else
				{
					string pth = Path.Combine (CLI.pascalPath, "Highlighting", "sticker.png");
					if (File.Exists (pth))
					{
						Image stckr = Image.FromFile (pth);


						if (CLI.sopacity != 100)
						{
							sticker = Helper.setOpacity (stckr, CLI.sopacity);
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

		private void RefreshStatusBar ()
		{
			currentTheme.Visible = CLI.swStatusbar;
			nminst = CLI.swStatusbar ? 1 : 0;
		}

		private void loadSVG ()
		{
			if (currentTheme.Image != null)
				currentTheme.Image.Dispose ();
			SvgDocument svg = Helper.loadsvg ("favorite", Assembly.GetExecutingAssembly (),
			                                  "Yuki_Theme_Plugin.Resources");
			svg.Fill = new SvgColourServer (bgBorder);
			svg.Stroke = new SvgColourServer (bgBorder);
			currentTheme.Image = svg.Draw (16, 16);

			if (stick != null)
				stick.Image = currentTheme.Image;

			if (Helper.CurrentTheme.Contains (":"))
			{
				string [] spl = Helper.CurrentTheme.Split (':');
				currentTheme.Text = spl [spl.Length - 1];
				spl = null;
			} else
				currentTheme.Text = Helper.CurrentTheme;

			if(wallpaperImage != null)
			{
				wallpaperImage.Dispose ();
				wallpaperImage = null;
			}
			wallpaperImage = Helper.renderSVG (defaultSize, Helper.loadsvg (
				                                   "layoutPreview", Assembly.GetExecutingAssembly (),
				                                   "Yuki_Theme_Plugin.Resources"), false, Size.Empty, true,
			                                   bgBorder);
			if(switchImage != null)
			{
				switchImage.Dispose ();
				switchImage = null;
			}
			
			switchImage = Helper.renderSVG (defaultSize, Helper.loadsvg (
				                                   "refresh", Assembly.GetExecutingAssembly (),
				                                   "Yuki_Theme_Plugin.Resources"), false, Size.Empty, true,
			                                   bgBorder);

		}

		private void errorListHeaderDrawer (object sender, DrawListViewColumnHeaderEventArgs e)
		{
			e.Graphics.FillRectangle (bgBrush, e.Bounds);

			e.Graphics.DrawString (e.Header.Text, e.Font, clrBrush, e.Bounds);
			// e.DrawText ();
		}

		private void setMargin ()
		{
			foreach (AbstractMargin margins in textArea.LeftMargins)
			{
				// MessageBox.Show (margin.Size.ToString());

				if (margins is IconBarMargin)
				{
					margin = (IconBarMargin) margins;
				}else if (margins is FoldMargin)
				{
					foldmargin = (FoldMargin) margins;
				}
			}
		}

		private void InitAdditions ()
		{
			Extender.SetSchema (fm.MainDockPanel);
		}

		private void setTim2 ()
		{
			tim2 = new Timer () {Interval = 1000};
			tim2.Tick += Tim2OnTick;
			tim2.Start ();
		}
		
		private void Tim2OnTick (object sender, EventArgs e)
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
			tim2.Stop ();
			tim2.Dispose ();
		}

		private void RefreshEditor ()
		{
			textArea.Refresh ();
			enbld = 0;
			enbld += CLI.bgImage ? 1 : 0;
			enbld += CLI.swSticker ? 2 : 0;
		}
		
		private void PaintBG (object sender, PaintEventArgs e)
		{
			if(margin != null)
			{
				e.Graphics.FillRectangle (new SolidBrush (bgdef), margin.DrawingPosition.X,
				                          margin.DrawingPosition.Y,
				                          margin.DrawingPosition.Width, margin.DrawingPosition.Height);
				var inside =
					typeof (IconBarMargin).GetMethod ("IsLineInsideRegion",
					                                  BindingFlags.Static | BindingFlags.NonPublic);
				// paint icons
				foreach (Bookmark mark in textArea.Document.BookmarkManager.Marks) {
					int lineNumber = textArea.Document.GetVisibleLine(mark.LineNumber);
					int lineHeight = textArea.TextView.FontHeight;
					int yPos = (int)(lineNumber * lineHeight) - textArea.VirtualTop.Y;
					if ((bool) inside.Invoke(null,new object[] {yPos, yPos + lineHeight, margin.DrawingPosition.Y, margin.DrawingPosition.Height})) {
						if (lineNumber == textArea.Document.GetVisibleLine(mark.LineNumber - 1)) {
							// marker is inside folded region, do not draw it
							continue;
						}
						mark.Draw(margin, e.Graphics, new Point(0, yPos));
					}
				}
			}

			if (foldmargin != null)
			{
				e.Graphics.DrawLine(BrushRegistry.GetDotPen(bgdef, bgvruler),
				                    foldmargin.DrawingPosition.X,
				                    foldmargin.DrawingPosition.Y,
				                    foldmargin.DrawingPosition.X,
				                    foldmargin.DrawingPosition.Height);
			}

			if(img != null && bgImage)
			{
				if (oldV.Width != textArea.ClientRectangle.Width || oldV.Height != textArea.ClientRectangle.Height)
				{
					oldV = Helper.getSizes (img.Size, textArea.ClientRectangle.Width, textArea.ClientRectangle.Height,
					                        align);
				}

				e.Graphics.DrawImage (img, oldV);
			}
		}

		private Rectangle oldV = Rectangle.Empty;

		private void ReleaseResources ()
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
		
		private void UpdateColors ()
		{
			fm.BackColor = menu.BackColor = statusBar.BackColor = toolsPanel.BackColor = tools.BackColor =
				fm.cmEditor.BackColor = textEditor.Parent.BackColor =
					fm.CurrentCodeFileDocument.BackColor = bg;
			
			output_panel2.BackColor = output_panel6.BackColor = output_input.BackColor = output_panel4.BackColor =
				output_panel3.BackColor = output_panel5.BackColor = output_panel1.BackColor = output_text.BackColor =
					output_output.BackColor = fm.ProjectPane.BackColor = cr.BackColor = con.BackColor = bgdef;

			output_output.ForeColor = output_panel2.ForeColor = output_text.ForeColor = menu.ForeColor =
				statusBar.ForeColor = toolsPanel.ForeColor = tools.ForeColor = cr.ForeColor =
					con.ForeColor = clr;
			
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
				bool isDark = Helper.isDark (bg);
				add = isDark ? "" : "_dark";
				menu_settings.Image = Helper.renderSVG (menu_settings.Size, Helper.loadsvg (
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
			if (enablePositioning != null)
			{
				enablePositioning.BackColor = bgdef;
				bool isDark = Helper.isDark (bg);
				string add = isDark ? "" : "_dark";
				enablePositioning.Image = Helper.renderSVG (enablePositioning.Size,
				                                            Helper.loadsvg ("export" + add, Assembly.GetExecutingAssembly (),
				                                                            "Yuki_Theme_Plugin.Resources.icons"), false, Size.Empty, false,
				                                            Color.Black);

			}

			try
			{
				if (textEditor.Controls.Count >= 2) textEditor.Controls [1].Invalidate ();
			} catch (ArgumentOutOfRangeException)
			{

			}

			cr.Refresh ();
			setTim2 ();
			manager.UpdateColors ();
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
			bgvruler = highlighting.GetColorFor ("VRuler").Color;

			if(bgdefBrush != null) bgdefBrush.Dispose ();
			bgdefBrush = new SolidBrush (bgdef);
			
			if(bgBrush != null) bgBrush.Dispose ();
			bgBrush = new SolidBrush (bg);
			
			if(bgClickBrush != null) bgClickBrush.Dispose ();
			bgClickBrush = new SolidBrush (bgClick);
			
			if(bgClick3Brush != null) bgClick3Brush.Dispose ();
			bgClick3Brush = new SolidBrush (bgClick3);
			
			if(bgInactiveBrush != null) bgInactiveBrush.Dispose ();
			bgInactiveBrush = new SolidBrush (bgInactive);
			
			if(clrBrush != null) clrBrush.Dispose ();
			clrBrush = new SolidBrush (clr);
			
			if(bgPen != null) bgPen.Dispose ();
			bgPen = new Pen (bgBorder, 1){ Alignment = PenAlignment.Center };
			
			if(clrPen != null) clrPen.Dispose ();
			clrPen = new Pen (clrHover, 1);
			
			if(bgBorderPen != null) bgBorderPen.Dispose ();
			bgBorderPen = new Pen (bgBorder, 8);
		}

		private void LoadImage ()
		{
			if(img != null) img.Dispose ();
			string pth = Path.Combine (CLI.pascalPath, "Highlighting","background.png");
			if (File.Exists (pth))
			{
				Image iamg = Image.FromFile (pth);

				align = Alignment.Center;
				opacity = 10;

				XmlDocument doc = new XmlDocument ();
				IHighlightingStrategy high = HighlightingManager.Manager.FindHighlighterForFile ("A.pas");

				var fls = Directory.GetFiles (Path.Combine (CLI.pascalPath, "Highlighting/"), "*.xshd");

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
								align = (Alignment) int.Parse (comm.Value.Substring (6));
							} else if (comm.Value.StartsWith ("opacity"))
							{
								opacity = int.Parse (comm.Value.Substring (8));
							}
						}

						break;
					}
				}
				if(opacity != 100)
				{
					img = Helper.setOpacity (iamg, opacity);
					iamg.Dispose ();
				} else
				{
					img = iamg;
				}
			} else
			{
				img = null;
				align = Alignment.Left;
			}
		}
		
		private void ReloadLayout ()
		{
			HighlightingManager.Manager.ReloadSyntaxModes ();
			LoadImage ();
			LoadSticker ();
			LoadColors ();
			UpdateColors ();
			fm.Refresh ();
			loadSVG ();
			updateQuietImage ();
			updateWallpaperImage ();
			updateStickerImage ();
			updateSwitchImage ();
		}
		
		private void InitCore ()
		{
			// if (MessageBox.Show ("The program needs restart", "Restart", MessageBoxButtons.OKCancel) ==
			    // DialogResult.OK)
			    // IHighlightingStrategy tmphighligh = HighlightingManager.Manager.FindHighlighterForFile ("A.pas");
			    ReloadLayout ();
			    if (mf == null || mf.IsDisposed)
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
			    mf.Show();
		}


		private void ToggleQuiet (object sender, EventArgs e)
		{
			if (!tg)
			{
				CLI.bgImage = false;
				CLI.swSticker = false;
				if (nminst == 1)
					currentTheme.Visible = false;
			} else
			{
				CLI.bgImage = enbld == 1 || enbld == 3;
				CLI.swSticker = enbld == 2 || enbld == 3;
				if (nminst == 1)
					currentTheme.Visible = true;
			}
			tg = !tg;
			
			textArea.Refresh ();
			LoadSticker ();
			updateQuietImage ();
			updateWallpaperImage ();
			updateStickerImage ();
		}

		private void ToggleWallpaper (object sender, EventArgs e)
		{
			CLI.bgImage = !CLI.bgImage;
			textArea.Refresh ();
			updateWallpaperImage ();
		}

		private void SwitchTheme (object sender, EventArgs e)
		{
			if(!fm.Controls.ContainsKey ("Custom Panel Switcher"))
			{
				if (mf == null || mf.IsDisposed)
				{
					panel_bg = new CustomPanel (0);
					panel_bg.Name = "Custom Panel Switcher";

					Font fnt = new Font (FontFamily.GenericSansSerif, 10, GraphicsUnit.Point);

					Label lbl = new Label ();
					lbl.BackColor = bg;
					lbl.ForeColor = clr;
					lbl.Font = fnt;
					lbl.Text = "Themes";
					lbl.TextAlign = ContentAlignment.MiddleCenter;
					lbl.Size = new Size (200, 25);

					lst = new CustomList ();
					lst.BackColor = bgdef;
					lst.ForeColor = clr;
					lst.BorderStyle = BorderStyle.None;
					lst.Items.AddRange (CLI.schemes.ToArray ());
					lst.BorderStyle = BorderStyle.None;
					lst.Font = fnt;
					lst.ItemHeight = lst.Font.Height;
					lst.Size = new Size (200, 300);
					lst.MouseMove += Lst_MouseHover;


					panel_bg.Location = Point.Empty;
					panel_bg.Size = fm.ClientSize;
					lst.DrawMode = DrawMode.OwnerDrawFixed;
					lst.DrawItem += list_1_DrawItem;
					int x = (panel_bg.Width / 2) - (lst.Width / 2);
					int y = (panel_bg.Height / 2) - (lst.Height / 2);

					lbl.Location = new Point (x, y - 13);
					lst.Location = new Point (x, y + 12);

					if (lst.Items.Contains (Helper.CurrentTheme))
						lst.SelectedItem = Helper.CurrentTheme;
					else
						lst.SelectedIndex = 0;
					lst.SelectedIndexChanged += LstOnSelectedIndexChanged;
					lst.AccessibleName = lst.SelectedItem.ToString ();
					panel_bg.Click += CloseOnClick;

					panel_bg.Controls.Add (lst);
					panel_bg.Controls.Add (lbl);

					setBorder (lst, lbl);

					fm.Controls.Add (panel_bg);
					panel_bg.BringToFront ();
					lst.Focus ();
				} else
				{
					MessageBox.Show ("Please, close Yuki Theme window to activate 'Switch theme'");
				}
			}
		}
		
		private void stickersPositioning (object sender, EventArgs e)
		{
			CLI.positioning = !CLI.positioning;
			stickerControl.Enabled = CLI.positioning;
			updatestickersPositioningImage ();
		}

		private void ToggleSticker (object sender, EventArgs e)
		{
			CLI.swSticker = !CLI.swSticker;
			LoadSticker ();
			updateStickerImage ();
		}

		private void updateQuietImage ()
		{
			if (quiet != null)
			{
				quiet.Image = tg
					? updateBgofImage (quietImage)
					: quietImage;
			}
		}
		
		private void updatestickersPositioningImage ()
		{
			if (enablePositioning != null)
			{
				enablePositioning.Image = CLI.positioning
					? updateBgofImage (positioningImage)
					: positioningImage;
			}
		}

		private void updateWallpaperImage ()
		{
			if (backimage != null)
			{
				backimage.Image = CLI.bgImage
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

		private void updateStickerImage ()
		{
			if (stick != null)
			{
				stick.Image = CLI.swSticker
					? updateBgofImage (currentTheme.Image)
					: currentTheme.Image;
			}
		}

		private void loadWithWaiting ()
		{
			tim3 = new Timer () {Interval = 2200};
			tim3.Tick += load;
			if (CLI.swLogo)
			{
				showLogo ();
			}
			else
				InitAdditions ();
			tim3.Start ();
		}

		/// <summary>
		/// Look for item in menu. After that, add additional items
		/// </summary>
		private void load (object sender, EventArgs e)
		{
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
				bool isDark = Helper.isDark (bg);
				add = isDark ? "" : "_dark";

				menu_settings.Text = "Show Settings";
				menu_settings.ShortcutKeys = Keys.Alt | Keys.S;
				menu_settings.ShortcutKeyDisplayString = "Alt + S";
				menu_settings.Image = Helper.renderSVG (menu_settings.Size, Helper.loadsvg (
					                                        "gearPlain"+add, Assembly.GetExecutingAssembly (),
					                                        "Yuki_Theme_Plugin.Resources.icons"), false, Size.Empty,
				                                        true, clr);
				menu_settings.ImageScaling = ToolStripItemImageScaling.SizeToFit;
				ToolStrip ow = menu_settings.Owner;
				ow.Items.Remove (menu_settings);
				System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (MForm));	
				Icon icon = ((Icon) (resources.GetObject ($"$this.Icon")));

				ToolStripMenuItem main =
					new ToolStripMenuItem ("Yuki Theme", icon.ToBitmap ());

				quiet = new ToolStripMenuItem ("Toggle Discreet Mode", null, ToggleQuiet, Keys.Alt | Keys.A);
				quiet.ShortcutKeyDisplayString = "Alt + A";
				quiet.BackColor = menu_settings.BackColor;
				quiet.ForeColor = menu_settings.ForeColor;
				quietImage = Helper.renderSVG (quiet.Size, Helper.loadsvg (
					                               "quiet", Assembly.GetExecutingAssembly (),
					                               "Yuki_Theme_Plugin.Resources"));
				quiet.Image = quietImage;

				stick = new ToolStripMenuItem ("Enable Stickers",
				                               CLI.swSticker
					                               ? updateBgofImage (currentTheme.Image)
					                               : currentTheme.Image, ToggleSticker);
				stick.BackColor = menu_settings.BackColor;
				stick.ForeColor = menu_settings.ForeColor;

				backimage = new ToolStripMenuItem ("Enable Wallpaper",
				                                   null, ToggleWallpaper);
				backimage.Image = CLI.bgImage
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
				positioningImage = Helper.renderSVG (enablePositioning.Size, Helper.loadsvg (
					                                     "export" + add, Assembly.GetExecutingAssembly (),
					                                     "Yuki_Theme_Plugin.Resources.icons"));
				enablePositioning.Image = CLI.positioning
					? updateBgofImage (positioningImage)
					: positioningImage;

				main.DropDownItems.Add (stick);
				main.DropDownItems.Add (backimage);
				main.DropDownItems.Add (enablePositioning);
				main.DropDownItems.Add (quiet);
				main.DropDownItems.Add (switchTheme);
				main.DropDownItems.Add (menu_settings);

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
			}

			((CompilerConsoleWindowForm) workbench.CompilerConsoleWindow).AddTextToCompilerMessages (
				"Yuki Theme: Initialization finished.\n");
			if (CLI.swLogo)
			{
				hideLogo ();
				InitAdditions ();
			}
			tim3.Stop ();
			MForm.showLicense (bg, clr, bgClick, fm);
			MForm.showGoogleAnalytics (bg, clr, bgClick, fm);
			MForm.TrackInstall ();
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
		
		private void showLogo ()
		{
			logoBox = new PictureBox ();
			logoBox.BackColor = bgdef;
			logoBox.Image = global::Yuki_Theme_Plugin.Properties.Resources.YukiTheme;
			logoBox.Location = new System.Drawing.Point (fm.ClientSize.Width/2 - 50, fm.ClientSize.Height/2 - 50);
			logoBox.Name = "logoBox";
			logoBox.Size = new System.Drawing.Size (100, 100);
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

		private void CtrlOnPaint (object sender, PaintEventArgs e)
		{
			e.Graphics.FillRectangle (new SolidBrush (bgdef), e.ClipRectangle);
		}
		
		private void FmOnResize (object sender, EventArgs e)
		{
			if (panel_bg != null)
			{
				CloseOnClick (sender, e);
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

		private void ToolStripPanelOnPaint (object sender, PaintEventArgs e)
		{
			e.Graphics.DrawLine (clrPen, e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width,
			                     e.ClipRectangle.Y);
		}


		private void list_1_DrawItem (object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0) return;
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				e = new DrawItemEventArgs (e.Graphics, e.Font, e.Bounds,
				                           e.Index, e.State ^ DrawItemState.Selected,
				                           e.ForeColor, bgClick2);
			} else if (e.Index == selectionindex)
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
			fm.Controls.Remove (panel_bg);
			panel_bg.Dispose ();
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
		}

		private void ifHsImage (Image img)
		{
			tmpImage1 = img;
		}

		private void ifHsSticker (Image img)
		{
			tmpImage2 = img;
		}
		
		private void ifDNIMG ()
		{
			tmpImage1 = null;
		}
		
		private void ifDNSTCK ()
		{
			tmpImage2 = null;
		}

		private void LstOnSelectedIndexChanged (object sender, EventArgs e)
		{
			if(lst.SelectedIndex >= 0)
			{
				if(lst.SelectedItem.ToString () != lst.AccessibleName)
				{
					CLI.currentoFile = lst.SelectedItem.ToString ();
					CLI.currentFile = Helper.ConvertNameToPath (CLI.currentoFile);
					CLI.selectedItem = CLI.currentoFile;
					CLI.ifHasImage2 = ifHsImage;
					CLI.ifHasSticker2 = ifHsSticker;
					CLI.ifDoesntHave2 = ifDNIMG;
					CLI.ifDoesntHaveSticker2 = ifDNSTCK;
					CLI.restore (false, null);
					CLI.export (tmpImage1, tmpImage2, ReloadLayout, ReleaseResources);

					CLI.ifHasImage2 = null;
					CLI.ifHasSticker2 = null;
					CLI.ifDoesntHave2 = null;
					CLI.ifDoesntHaveSticker2 = null;
				}
				
				CloseOnClick (sender, e);
			}
		}
		
		private void Lst_MouseHover(object sender, EventArgs e)
		{
			Point point = lst.PointToClient(Cursor.Position);
			int index = lst.IndexFromPoint(point);
			if (index < 0) return;
			//Do any action with the item
			if(selectionindex != index)
			{
				int oldindex = selectionindex;
				selectionindex = index;
				lst.Invalidate (lst.GetItemRectangle (oldindex));
				lst.Invalidate (lst.GetItemRectangle (index));
			}
		}
		
		void setBorder(Control ctl, Control ctl2)
		{
				Panel pan = new Panel();
				pan.BorderStyle = BorderStyle.None;
				pan.Size = new Size (ctl.ClientRectangle.Width + 2,
				                     ctl.ClientRectangle.Height + ctl2.ClientRectangle.Height - 10);
				pan.Location = new Point(ctl.Left - 1, ctl.Top - 1);
				pan.BackColor = bgBorder;
				pan.Parent = ctl.Parent;
				ctl.Parent = pan;
				ctl2.Parent = pan;
				ctl.Location = new Point (1, Math.Abs (ctl.Top - ctl2.Top));
				ctl2.Location = new Point (1, 1);
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

		private void addSettings ()
		{
			var getopt = fm.GetType ().GetField ("optionsContentEngine", BindingFlags.NonPublic | BindingFlags.Instance);
			OptionsContentEngine options = (OptionsContentEngine) getopt.GetValue (fm);
			options.AddContent (new PluginOptionsContent (this));
		}
		
	}
}