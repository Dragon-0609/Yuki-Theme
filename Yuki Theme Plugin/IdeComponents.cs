using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using VisualPascalABC;
using VisualPascalABCPlugins;
using Yuki_Theme.Core;
using Yuki_Theme.Core.Database;
using Yuki_Theme_Plugin.Controls;
using Yuki_Theme_Plugin.Controls.DockStyles;
using CodeCompletionHighlighter = Yuki_Theme_Plugin.Controls.DockStyles.CodeCompletionHighlighter;

namespace Yuki_Theme_Plugin
{
	public class IdeComponents
	{
		#region IDE Controls

		internal AboutBox              about;
		internal Form1                 fm;
		internal MenuStrip             menu;
		internal Panel                 output_input;
		internal RichTextBox           output_output;
		internal Panel                 output_panel1;
		internal Panel                 output_panel2;
		internal Panel                 output_panel3;
		internal Panel                 output_panel4;
		internal Panel                 output_panel5;
		internal Panel                 output_panel6;
		internal TextBox               output_text;
		internal Control               outputWindow;
		internal StatusStrip           statusBar;
		internal ToolStrip             tools;
		internal Panel                 toolsPanel;
		internal IWorkbench            workbench;
		internal TextArea              textArea;
		internal ContextMenuStrip      context;
		internal ContextMenuStrip      context2;
		internal MenuRenderer          renderer;
		internal IconBarMargin         margin;
		internal FoldMargin            foldmargin;
		internal ListView              errorsList;
		internal TextBox               compilerConsole;

		internal ProjectExplorerForm projectExplorer;
		internal ToolBoxForm         toolbox;
		
		internal IVisualEnvironmentCompiler                  compiler;
		internal CodeFileDocumentTextEditorControl           textEditor;
		internal Dictionary <ICodeFileDocument, RichTextBox> OutputTextBoxs;

		internal RichTextBox   oldInputPanel;
		internal ToolStripItem openInExplorerItem;
		
		#endregion
		
		internal Rectangle oldSizeOfTextEditor = Rectangle.Empty;

		private bool toggled = false; // is toggle activated

		internal bool supportProject = false; 

		private Size  defaultSize;
		private Timer documentUpdator;

		#region Colors, Brushes, Pens

		private Color bg       => YukiTheme_VisualPascalABCPlugin.bg;
		private Color bgdef    => YukiTheme_VisualPascalABCPlugin.bgdef;
		private Color bgBorder => YukiTheme_VisualPascalABCPlugin.bgBorder;
		private Color bgClick  => YukiTheme_VisualPascalABCPlugin.bgClick;
		private Color clr      => YukiTheme_VisualPascalABCPlugin.clr;
		private Color clrHover => YukiTheme_VisualPascalABCPlugin.clrHover;

		private Pen separatorPen => YukiTheme_VisualPascalABCPlugin.separatorPen;

		private Brush bgBrush  => YukiTheme_VisualPascalABCPlugin.bgBrush;
		private Brush clrBrush => YukiTheme_VisualPascalABCPlugin.clrBrush;

		#endregion


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

		private Image img => YukiTheme_VisualPascalABCPlugin.img;

		private YukiTheme_VisualPascalABCPlugin plugin => YukiTheme_VisualPascalABCPlugin.plugin;

		
		#region Initialization

		internal void Initialize ()
		{
			defaultSize = new Size (32, 32);
			textEditor = fm.CurrentCodeFileDocument.TextEditor;
			textArea = textEditor.ActiveTextAreaControl.TextArea;
			context = textEditor.ContextMenuStrip;
			context2 = fm.MainDockPanel.ContextMenuStrip;

			fm.Closing += FmOnClosing;
			context2.Opening += EditorContextOpening;

			openInExplorerItem = context2.Items.Add ("Open in Explorer", null, OpenInExplorer);

			GetFields ();
			SetMargin ();
			textArea.Paint += PaintBG;
			textEditor.Parent.BackColor = bg;
			textEditor.Controls [1].Paint += CtrlOnPaint;
			textEditor.Controls [1].Invalidate ();

			textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += UpdateMarkerBGOnCaretPositionChanged;
			textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += RemoveErrorMarksOnCaretPositionChanged;

			fm.CurrentCodeFileDocument.BackColor = bg;
			
			
			documentUpdator = new Timer () { Interval = 2 };
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
		}
		
		internal void GetFields ()
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

			try
			{
				FieldInfo field = typeof (Form1).GetField ("ProjectExplorerWindow", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null)
				{
					projectExplorer = (ProjectExplorerForm)field.GetValue (fm);
					if (projectExplorer != null)
					{
						supportProject = true;
						field = typeof (ProjectExplorerForm).GetField ("tvProjectExplorer", BindingFlags.Instance | BindingFlags.NonPublic);
						CustomTreeView view = new CustomTreeView ();
						if (field != null) view.AddCustomTreeViewEvents ((TreeView)field.GetValue (projectExplorer));

						toolbox = fm.ToolBoxWindow;
					}
				}
			} catch (Exception) {
				projectExplorer = null;	
				supportProject = false;
			}
			
		}
		
		internal void AddMenuItems ()
		{
			menu_settings = null;
			foreach (ToolStripItem menuItem in menu.Items)
			{
				if (menuItem is ToolStripMenuItem)
				{
					foreach (ToolStripItem toolStripMenuItem in ((ToolStripMenuItem)menuItem).DropDownItems)
					{
						if (toolStripMenuItem is ToolStripMenuItem)
						{
							if (toolStripMenuItem.Text == "Yuki Theme")
							{
								menu_settings = (ToolStripMenuItem)toolStripMenuItem;
								break;
							}
						}
					}
				}
			}

			GC.Collect (); // We must clean after search
			GC.WaitForPendingFinalizers ();

			if (menu_settings != null) // If we could find...
			{
				string add = "";
				bool isDark = Helper.IsDark (bg);
				add = isDark ? "" : "_dark";

				menu_settings.Text = "Show Settings";
				menu_settings.ShortcutKeys = Keys.Alt | Keys.S;
				menu_settings.ShortcutKeyDisplayString = "Alt + S";
				menu_settings.Image = Helper.RenderSvg (menu_settings.Size, Helper.LoadSvg (
					                                        "gearPlain" + add, Assembly.GetExecutingAssembly (),
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
					                               ? updateBgofImage (plugin.currentThemeName.Image)
					                               : plugin.currentThemeName.Image, ToggleSticker);
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
		}

		#endregion

		
		#region Update

		internal void UpdateColors ()
		{
			fm.BackColor = menu.BackColor = statusBar.BackColor = toolsPanel.BackColor = tools.BackColor =
				fm.cmEditor.BackColor = textEditor.Parent.BackColor =
					fm.CurrentCodeFileDocument.BackColor = fm.BottomPane.Parent.BackColor = bg;

			output_panel2.BackColor = output_panel6.BackColor = output_input.BackColor = output_panel4.BackColor =
				output_panel3.BackColor = output_panel5.BackColor = output_panel1.BackColor = output_text.BackColor =
					output_output.BackColor = fm.ProjectPane.BackColor = errorsList.BackColor = compilerConsole.BackColor = bgdef;

			output_output.ForeColor = output_panel2.ForeColor = output_text.ForeColor = menu.ForeColor =
				statusBar.ForeColor = toolsPanel.ForeColor = tools.ForeColor = errorsList.ForeColor =
					compilerConsole.ForeColor = clr;
			foreach (Control o in output_panel1.Controls)
			{
				if (o is Button)
				{
					Button b = (Button)o;
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

			try
			{
				if (textEditor.Controls.Count >= 2) textEditor.Controls [1].Invalidate ();
			} catch (ArgumentOutOfRangeException)
			{
			}

			UpdateMenuColors ();
			errorsList.Refresh ();
			UpdateBottomTextPanel ();
			UpdateProjectColors ();
		}

		private void UpdateMenuColors ()
		{
			if (menu_settings != null)
			{
				string add = "";
				bool isDark = Helper.IsDark (bg);
				add = isDark ? "" : "_dark";
				menu_settings.Image = Helper.RenderSvg (menu_settings.Size, Helper.LoadSvg (
					                                        "gearPlain" + add, Assembly.GetExecutingAssembly (),
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

		}
		
		internal void UpdateAboutForm (object senderaw, EventArgs eaw)
		{
			about.BackColor = bg;
			about.ForeColor = clr;
			Button btn = null;
			foreach (Control cont in about.Controls)
			{
				if (cont is LinkLabel)
				{
					((LinkLabel)cont).LinkColor = clr;
					((LinkLabel)cont).ActiveLinkColor = clrHover;
				} else if (cont is Button)
					btn = (Button)cont;
				else if (cont is GroupBox)
				{
					GroupBox group = (GroupBox)cont;
					group.ForeColor = clr;
					foreach (Control groupControl in group.Controls)
					{
						if (groupControl is LinkLabel)
						{
							((LinkLabel)groupControl).LinkColor = clr;
							((LinkLabel)groupControl).ActiveLinkColor = clrHover;
						} else if (groupControl is ListView)
						{
							ListView lw = (ListView)groupControl;
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
				} else if (cont is TableLayoutPanel)
				{
					TableLayoutPanel tbl = (TableLayoutPanel)cont;
					tbl.ForeColor = clr;
					foreach (Control flowLayout in tbl.Controls)
					{
						if (flowLayout is FlowLayoutPanel)
						{
							foreach (Control tblControl in flowLayout.Controls)
							{
								if (tblControl is Label)
								{
									if (tblControl.Name.Contains ("Version"))
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
		
		internal void UpdateMenuItemIcons ()
		{
			if (stick != null)
				stick.Image = plugin.currentThemeName.Image;
			
			if (wallpaperImage != null)
			{
				wallpaperImage.Dispose ();
				wallpaperImage = null;
			}

			wallpaperImage = Helper.RenderSvg (defaultSize, Helper.LoadSvg (
				                                   "layoutPreview", Assembly.GetExecutingAssembly (),
				                                   "Yuki_Theme_Plugin.Resources"), false, Size.Empty, true,
			                                   bgBorder);
			if (switchImage != null)
			{
				switchImage.Dispose ();
				switchImage = null;
			}

			switchImage = Helper.RenderSvg (defaultSize, Helper.LoadSvg (
				                                "refresh", Assembly.GetExecutingAssembly (),
				                                "Yuki_Theme_Plugin.Resources"), false, Size.Empty, true,
			                                bgBorder);
			if (resetPositionImage != null)
			{
				resetPositionImage.Dispose ();
				resetPositionImage = null;
			}

			resetPositionImage = Helper.RenderSvg (defaultSize, Helper.LoadSvg (
				                                       "refresh", Assembly.GetExecutingAssembly (),
				                                       "Yuki_Theme_Plugin.Resources"), false, Size.Empty, true,
			                                       bgBorder);
		}
		
		public void UpdateTranslations ()
		{
			openInExplorerItem.Text = CLI.Translate ("plugin.explorer.open");
			menu_settings.Text = CLI.Translate ("plugin.menu.settings");
			quiet.Text = CLI.Translate ("plugin.menu.discreet");
			stick.Text = CLI.Translate ("plugin.menu.stickers");
			backimage.Text = CLI.Translate ("plugin.menu.wallpaper");
			switchTheme.Text = CLI.Translate ("plugin.menu.switch");
			enablePositioning.Text = CLI.Translate ("plugin.menu.positioning");
			resetStickerPosition.Text = CLI.Translate ("plugin.menu.margins");
			updatePage.Text = CLI.Translate ("plugin.menu.notification");
		}
		
		internal void UpdateMenuStickersPositioningImage ()
		{
			if (enablePositioning != null)
			{
				enablePositioning.Image = Settings.positioning
					? updateBgofImage (positioningImage)
					: positioningImage;
			}
		}

		internal void UpdateMenuWallpaperImage ()
		{
			if (backimage != null)
			{
				backimage.Image = Settings.bgImage
					? updateBgofImage (wallpaperImage)
					: wallpaperImage;
			}
		}

		private void UpdateMenuSwitchImage ()
		{
			if (switchTheme != null)
			{
				switchTheme.Image = switchImage;
			}
		}

		private void UpdateMenuResetPositionImage ()
		{
			if (resetStickerPosition != null)
			{
				resetStickerPosition.Image = resetPositionImage;
			}
		}

		internal void UpdateMenuStickerImage ()
		{
			if (stick != null)
			{
				stick.Image = Settings.swSticker
					? updateBgofImage (plugin.currentThemeName.Image)
					: plugin.currentThemeName.Image;
			}
		}
		
		private void UpdateBottomTextPanel ()
		{
			oldInputPanel = OutputTextBoxs [fm.CurrentCodeFileDocument];
			oldInputPanel.BackColor = bgdef;
			oldInputPanel.BorderStyle = BorderStyle.None;
		}

		private void UpdateMarkerBGOnCaretPositionChanged (object sender, EventArgs e)
		{
			if (!VisualPABCSingleton.MainForm.UserOptions.HighlightOperatorBrackets ||
			    WorkbenchServiceFactory.DebuggerManager.IsRunning)
				return;
			CodeCompletionHighlighter.UpdateMarkers (textEditor.ActiveTextAreaControl.TextArea);
		}

		private void UpdateProjectColors ()
		{
			if (supportProject && projectExplorer != null)
			{
				SetColors (projectExplorer);
				// SetColors (toolbox);

				UpdateDesignerPageColors ();
			}
		}

		private void SetColors (Form target)
		{
			target.BackColor = bg;
			target.ForeColor = clr;
			foreach (Control targetControl in target.Controls)
			{
				targetControl.BackColor = bg;
				targetControl.ForeColor = clr;
			}
		}

		private void UpdateDesignerPageColors ()
		{
			if (supportProject && fm.CurrentCodeFileDocument.DesignerAndCodeTabs != null)
			{
				foreach (TabPage tabPage in fm.CurrentCodeFileDocument.DesignerAndCodeTabs.TabPages)
				{
					tabPage.BackColor = bgdef;
					tabPage.ForeColor = clr;
					foreach (Control pageControl in tabPage.Controls)
					{
						pageControl.BackColor = bgdef;
						pageControl.ForeColor = clr;
					}
				}

				GC.Collect ();
				GC.WaitForPendingFinalizers ();
			}
		}

		#endregion

		
		#region Margins

		internal void SetMargin ()
		{
			foreach (AbstractMargin margins in textArea.LeftMargins)
			{
				if (margins is IconBarMargin)
				{
					margin = (IconBarMargin)margins;
				} else if (margins is FoldMargin)
				{
					foldmargin = (FoldMargin)margins;
				}
			}
		}

		internal void SetMarginPosition ()
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
		
		#endregion
		
		
		internal void ReSetTextEditor ()
		{
			if (textEditor != fm.CurrentCodeFileDocument.TextEditor)
			{
				textArea.Paint -= PaintBG;
				plugin.inspector.StopInspectBrackets ();

				try
				{
					textEditor.Controls [1].Paint -= CtrlOnPaint;
				} catch (ArgumentOutOfRangeException)
				{
				}

				// textArea = textEditor.ActiveTextAreaControl.TextArea;
				textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged -= UpdateMarkerBGOnCaretPositionChanged;
				textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged -= RemoveErrorMarksOnCaretPositionChanged;
				textEditor = fm.CurrentCodeFileDocument.TextEditor;
				textArea = textEditor.ActiveTextAreaControl.TextArea;
				textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += UpdateMarkerBGOnCaretPositionChanged;
				textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += RemoveErrorMarksOnCaretPositionChanged;
				SetMargin ();
				plugin.inspector.InspectBrackets ();
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

				plugin.inspector.Unsubscribe ();
				
				UpdateDesignerPageColors ();
			}
		}
		
		internal void ReloadLayout ()
		{
			UpdateMenuWallpaperImage ();
			UpdateMenuStickerImage ();
			UpdateMenuSwitchImage ();
			UpdateMenuResetPositionImage ();
		}

		private void EditorContextOpening (object sender, CancelEventArgs e)
		{
			if (!(fm.MainDockPanel.ActiveContent is CodeFileDocumentControl))
			{
				openInExplorerItem.Visible = false;
			} else
			{
				openInExplorerItem.Visible = true;
				openInExplorerItem.Enabled = CheckAvailabilityOfDocument ();
			}
		}

		private void OpenInExplorer (object sender, EventArgs e)
		{
			if (CheckAvailabilityOfDocument ())
				System.Diagnostics.Process.Start ("explorer.exe", string.Format ("/select, \"{0}\"", fm.CurrentCodeFileDocument.FileName));
		}

		private bool CheckAvailabilityOfDocument ()
		{
			return File.Exists (fm.CurrentCodeFileDocument.FileName);
		}

		private void FmOnClosing (object sender, CancelEventArgs e)
		{
			if (System.Windows.Application.Current != null)
				System.Windows.Application.Current.Shutdown ();
		}

		private void RemoveErrorMarksOnCaretPositionChanged (object sender, EventArgs e)
		{
			ErrorLineBookmarkNew.Remove ();
		}

		public void WriteToConsole (string text)
		{
			fm.AddTextToCompilerMessages (text + Environment.NewLine);
		}

		#region Methods_For_Menu

		internal void updateQuietImage ()
		{
			if (quiet != null)
			{
				quiet.Image = toggled
					? updateBgofImage (quietImage)
					: quietImage;
			}
		}
		
		private void ToggleQuiet (object sender, EventArgs e)
		{
			if (!toggled)
			{
				Settings.bgImage = false;
				Settings.swSticker = false;
				if (plugin.nameInStatusBar)
					plugin.currentThemeName.Visible = false;
			} else
			{
				Settings.bgImage = plugin.imagesEnabled == 1 || plugin.imagesEnabled == 3;
				Settings.swSticker = plugin.imagesEnabled == 2 || plugin.imagesEnabled == 3;
				if (plugin.nameInStatusBar)
					plugin.currentThemeName.Visible = true;
			}

			toggled = !toggled;

			textArea.Refresh ();
			plugin.LoadSticker ();
			updateQuietImage ();
			UpdateMenuWallpaperImage ();
			UpdateMenuStickerImage ();
		}

		private void ToggleWallpaper (object sender, EventArgs e)
		{
			Settings.bgImage = !Settings.bgImage;
			textArea.Refresh ();
			UpdateMenuWallpaperImage ();
		}

		private void SwitchTheme (object sender, EventArgs e)
		{
			plugin.switcher.SwitchTheme ();
		}

		private void stickersPositioning (object sender, EventArgs e)
		{
			Settings.positioning = !Settings.positioning;
			plugin.stickerControl.Enabled = Settings.positioning;
			UpdateMenuStickersPositioningImage ();
		}

		private void ResetStickerPosition (object sender, EventArgs e)
		{
			DatabaseManager.DeleteData (Settings.STICKERPOSITION);
			plugin.stickerControl.ReadData ();
			plugin.stickerControl.UpdateLocation ();
		}

		private void ShowUpdatePage (object sender, EventArgs e)
		{
			plugin.openUpdate ();
		}

		private void ToggleSticker (object sender, EventArgs e)
		{
			Settings.swSticker = !Settings.swSticker;
			plugin.LoadSticker ();
			UpdateMenuStickerImage ();
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
				SetMarginPosition ();
				e.Graphics.DrawLine (BrushRegistry.GetDotPen (bgdef, bgBorder),
				                     foldmargin.DrawingPosition.X,
				                     foldmargin.DrawingPosition.Y,
				                     foldmargin.DrawingPosition.X,
				                     foldmargin.DrawingPosition.Height);
			}

			if (img != null && plugin.bgImage && !plugin.switcher.hideBG)
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
					oldSizeOfTextEditor = Helper.GetSizes (img.Size, vm.Width, vm.Height, plugin.wallpaperAlign);
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

		private Bitmap updateBgofImage (Image oldImage)
		{
			Bitmap newImage = (Bitmap)oldImage.Clone ();
			using (var g = Graphics.FromImage (newImage))
			{
				g.Clear (Color.FromArgb (90, bgBorder));
				g.DrawImage (oldImage, new Point (0, 0));
			}

			return newImage;
		}
		
		#endregion
		
	}
}