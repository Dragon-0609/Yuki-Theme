using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using VisualPascalABC;
using VisualPascalABCPlugins;
using WeifenLuo.WinFormsUI.Docking;
using Yuki_Theme.Core;
using Yuki_Theme.Core.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Yuki_Theme_Plugin
{
	
	internal class YukiTheme_VisualPascalABCPlugin : IVisualPascalABCPlugin
	{
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

		private Image         img;
		private Color         bg;
		private Color         bgdef;
		private Color         clr;
		private Color         bgBorder;
		private Alignment     align;
		private int           opacity;
		private Timer         tim;
		private Timer         tim2;
		private Timer         tim3;
		private IconBarMargin margin;
		private MForm         mf;
		private ListView      cr;
		private TextBox       con;
		private PictureBox    logoBox;

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
			var item1 = new PluginGUIItem ("Yuki Theme", "Yuki Theme", icon.ToBitmap (), Color.Transparent, Click1);
			//Добавляем в меню
			MenuItems.Add (item1);
			//Добавляем на панель
			ToolBarItems.Add (item1);
			
			fm = (Form1) workbench.MainForm;
			showLogo ();
			Initialize ();
		}

		private void Initialize ()
		{
			fm.AllowTransparency = true;
			
			LoadColors ();
			
			textEditor = fm.CurrentCodeFileDocument.TextEditor;
			textArea = textEditor.ActiveTextAreaControl.TextArea;
			LoadImage ();
			
			/*
			textArea.InsertString (@"var a:string;
begin
  Readln(a);
end.");*/

			about = fm.AboutBox1;
			about.BackColor = Color.Red;

			statusBar = (StatusStrip) fm.Controls.Find ("statusStrip1", false) [0];

			toolsPanel = (Panel) fm.Controls.Find ("toolStripPanel", false) [0];

			tools = (ToolStrip) toolsPanel.Controls.Find ("toolStrip1", false) [0];

			menu = (MenuStrip) fm.Controls.Find ("menuStrip1", false) [0];
			
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

			output_output.BorderStyle = BorderStyle.None;
			output_input.BorderStyle = BorderStyle.FixedSingle;

			ErrorsListWindowForm erw = (ErrorsListWindowForm) workbench.ErrorsListWindow;
			cr = (ListView) erw.Controls.Find ("lvErrorsList", false) [0];
			cr.OwnerDraw = true;
			cr.DrawColumnHeader += errorListHeaderDrawer;
			cr.DrawItem += (sender, e) =>
			{
				e.DrawDefault = true;
			};

			CompilerConsoleWindowForm cons = (CompilerConsoleWindowForm) workbench.CompilerConsoleWindow;
			con = (TextBox) cons.Controls.Find ("CompilerConsole", false) [0];
			
			foreach (Control o in output_panel1.Controls)
			{
				if(o is Button)
				{
					Button b = (Button) o;
					b.BackColor = bg;
					b.FlatStyle = FlatStyle.Flat;
					b.FlatAppearance.BorderColor = bgBorder;
				}
			}

			setMargin ();
			textArea.Paint += PaintBG;
			
			tim = new Timer () {Interval = 1};
			tim.Tick += (sender, r) =>
			{
				if(textEditor != fm.CurrentCodeFileDocument.TextEditor)
				{
					textArea.Paint -= PaintBG;
					// textArea = textEditor.ActiveTextAreaControl.TextArea;
					textEditor = fm.CurrentCodeFileDocument.TextEditor;
					textArea = textEditor.ActiveTextAreaControl.TextArea;
					setMargin ();
					textArea.Paint += PaintBG;
					textArea.Refresh ();
					tim.Stop ();
				}
			};
			
			
			fm.MainDockPanel.ActiveContentChanged += (sender, e) =>
			{
				tim.Start ();
			};

			UpdateColors ();

			MenuRenderer renderer = new MenuRenderer ();
			menu.Renderer = renderer;
			setTim2 ();
		}

		private void errorListHeaderDrawer (object sender, DrawListViewColumnHeaderEventArgs e)
		{
			using (SolidBrush brush = new SolidBrush (bg)) { e.Graphics.FillRectangle (brush, e.Bounds); }

			using (SolidBrush forebrush = new SolidBrush (clr)) { e.Graphics.DrawString (e.Header.Text, e.Font, forebrush, e.Bounds); }
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
				}
			}
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
		
		private void PaintBG (object sender, PaintEventArgs e)
		{
			if(margin != null)
					e.Graphics.FillRectangle (new SolidBrush (bgdef), margin.DrawingPosition.X,
					                          margin.DrawingPosition.Y,
					                          margin.DrawingPosition.Width, margin.DrawingPosition.Height);

			if(img != null)
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

		public string Name => "Yuki Theme";

		public string Version => "2.0";

		public string Copyright => "Dragon-LV";

		private void ReleaseResources ()
		{
			if (img != null)
			{
				img.Dispose ();
				img = null;
			}
		}
		
		private void UpdateColors ()
		{
			output_panel2.BackColor = output_panel6.BackColor = output_input.BackColor = output_panel4.BackColor =
				output_panel3.BackColor = output_panel5.BackColor =
					output_panel1.BackColor = output_text.BackColor = output_output.BackColor = fm.BackColor = 
						menu.BackColor = statusBar.BackColor = toolsPanel.BackColor = tools.BackColor = 
							fm.ProjectPane.BackColor = cr.BackColor = con.BackColor = fm.cmEditor.BackColor = bg;

			output_output.ForeColor = output_panel2.ForeColor = output_text.ForeColor = menu.ForeColor = 
				statusBar.ForeColor = toolsPanel.ForeColor = tools.ForeColor = cr.ForeColor = 
					con.ForeColor = clr;
			
			foreach (Control o in output_panel1.Controls)
			{
				if(o is Button)
				{
					Button b = (Button) o;
					b.BackColor = bg;
					b.FlatAppearance.BorderColor = bgBorder;
				}
			}
			
			cr.Refresh ();
			setTim2 ();
		}
		
		private void LoadColors ()
		{
			highlighting = HighlightingManager.Manager.FindHighlighterForFile ("A.pas");
			bgdef = highlighting.GetColorFor ("Default").BackgroundColor;
			bg = Helper.bgColor = Helper.DarkerOrLighter (bgdef, 0.05f);
			clr = Helper.DarkerOrLighter (highlighting.GetColorFor ("Default").Color, 0.2f);
			bgBorder = highlighting.GetColorFor ("CaretMarker").Color;
		}
		
		private void LoadImage ()
		{
			var fo = Directory.GetFiles ("Highlighting/", "bac.png");
			if(File.Exists ("Highlighting/background.png"))
			{
				string pth = "Highlighting/background.png";
				img = Image.FromFile (pth);
				// @"C:\Users\User\Documents\CSharp\Yuki Theme Plugin\Yuki Theme Plugin\Resources\asuna_dark.png");
				// @"C:\Users\User\Documents\CSharp\Yuki Theme Plugin\Yuki Theme Plugin\Resources\emilia_dark.png");

				align = Alignment.Center;
				opacity = 10;

				XmlDocument doc = new XmlDocument ();
				IHighlightingStrategy high = HighlightingManager.Manager.FindHighlighterForFile ("A.pas");
				
				var fls = Directory.GetFiles ("Highlighting/", "*.xshd");

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
				
				img = Helper.setOpacity (img, opacity);
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
			LoadColors ();
			UpdateColors ();
			fm.Refresh ();
		}
		
		private void Click1 ()
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

		private void showLogo ()
		{
			tim3 = new Timer () {Interval = 2200};
			tim3.Tick += hideLogo;
			logoBox = new PictureBox (); 
			logoBox.Image = global::Yuki_Theme_Plugin.Properties.Resources.YukiTheme;
			logoBox.Location = new System.Drawing.Point (fm.ClientSize.Width/2 - 50, fm.ClientSize.Height/2 - 50);
			logoBox.Name = "logoBox";
			logoBox.Size = new System.Drawing.Size (100, 100);
			logoBox.SizeMode = PictureBoxSizeMode.Zoom;
			logoBox.TabIndex = 0;
			logoBox.TabStop = false;
			fm.Controls.Add (logoBox);
			tim3.Start ();
		}

		private void hideLogo (object sender, EventArgs e)
		{
			fm.Controls.Remove (logoBox);
			logoBox.Dispose ();
			tim3.Stop ();
		}
	}
}