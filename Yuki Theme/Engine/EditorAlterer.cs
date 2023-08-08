using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using AdvancedDataGridView;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using VisualPascalABC;
using VisualPascalABC.OptionsContent;
using VisualPascalABCPlugins;
using YukiTheme.Style;
using YukiTheme.Tools;
using static YukiTheme.Engine.ColorChanger;

namespace YukiTheme.Engine;

public class EditorAlterer
{
	private MenuStrip _menu;
	private StatusStrip _statusBar;
	private Panel _toolsPanel;
	private ToolStrip _tools;
	internal CodeFileDocumentTextEditorControl TextEditor;
	internal TextArea TextArea;
	private ContextMenuStrip _context;
	private ContextMenuStrip _context2;
	private Control _outputWindow;
	private Panel _outputPanel2;
	private Panel _outputPanel6;
	private RichTextBox _outputOutput;
	private Panel _outputInput;
	private Panel _outputPanel4;
	private Panel _outputPanel3;
	private Panel _outputPanel5;
	private Panel _outputPanel1;
	private TextBox _outputText;
	private ListView _errorsList;
	private TextBox _compilerConsole;
	private Dictionary<ICodeFileDocument, RichTextBox> _outputTextBoxs;
	private MenuRenderer _menuRenderer;

	private AboutBox _aboutBox;

	private readonly OptionsChanger _optionsChanger = new();

	public Form1 Fm => IDEAlterer.Alterer.Form1;

	private TextEditorChanger _editorChanger = new();
	private ToolStripMenuItem _menuSettings;

	public void GetComponents()
	{
		_menu = (MenuStrip)Fm.Controls.Find("menuStrip1", false)[0];
		_statusBar = (StatusStrip)Fm.Controls.Find("statusStrip1", false)[0];
		_toolsPanel = (Panel)Fm.Controls.Find("toolStripPanel", false)[0];
		_tools = (ToolStrip)_toolsPanel.Controls.Find("toolStrip1", false)[0];

		TextEditor = Fm.CurrentCodeFileDocument.TextEditor;
		TextArea = TextEditor.ActiveTextAreaControl.TextArea;
		_context = TextEditor.ContextMenuStrip;
		_context2 = Fm.MainDockPanel.ContextMenuStrip;

		_editorChanger.Init(this);

		// _textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += UpdateMarkerBGOnCaretPositionChanged;
		// _textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += RemoveErrorMarksOnCaretPositionChanged;

		_outputWindow = (Control)IDEAlterer.Alterer.Workbench.OutputWindow;
		_outputPanel2 = (Panel)_outputWindow.Controls.Find("panel2", false)[0];
		_outputPanel6 = (Panel)_outputPanel2.Controls.Find("panel6", false)[0];
		_outputOutput = (RichTextBox)_outputPanel6.Controls.Find("outputTextBox", false)[0];
		_outputInput = (Panel)_outputPanel2.Controls.Find("InputPanel", false)[0];
		_outputPanel4 = (Panel)_outputInput.Controls.Find("panel4", false)[0];
		_outputPanel3 = (Panel)_outputInput.Controls.Find("panel3", false)[0];
		_outputPanel5 = (Panel)_outputPanel4.Controls.Find("panel5", false)[0];
		_outputPanel1 = (Panel)_outputPanel4.Controls.Find("panel1", false)[0];
		_outputText = (TextBox)_outputPanel5.Controls.Find("InputTextBox", false)[0];
		_outputText.BorderStyle = BorderStyle.FixedSingle;
		_outputOutput.BorderStyle = BorderStyle.None;
		_outputInput.BorderStyle = BorderStyle.None;


		ErrorsListWindowForm erw = (ErrorsListWindowForm)IDEAlterer.Alterer.Workbench.ErrorsListWindow;
		_errorsList = (ListView)erw.Controls.Find("lvErrorsList", false)[0];
		_errorsList.OwnerDraw = true;
		_errorsList.DrawColumnHeader += ErrorListHeaderDrawer;
		_errorsList.DrawItem += (sender, e) => { e.DrawDefault = true; };


		CompilerConsoleWindowForm cons = (CompilerConsoleWindowForm)IDEAlterer.Alterer.Workbench.CompilerConsoleWindow;
		_compilerConsole = (TextBox)cons.Controls.Find("CompilerConsole", false)[0];

		FieldInfo fp = typeof(Form1).GetField("OutputTextBoxs", BindingFlags.Instance | BindingFlags.NonPublic);

		_outputTextBoxs = (Dictionary<ICodeFileDocument, RichTextBox>)fp.GetValue(Fm);

		_optionsChanger.GetOptionsComponents(Fm);

		_aboutBox = Fm.AboutBox1;
		_aboutBox.Shown += UpdateAboutForm;
		// TreeGridView watchList = _debugVariablesListWindow.\
	}

	public void SubscribeComponents()
	{
		SubscribeUpdate();
		Changer.UpdatedColors += () =>
		{
			RefreshEditorColors();
			_errorsList.Refresh();
			UpdateBottomTextPanel();
		};
	}

	private void SubscribeUpdate()
	{
		Changer.Update += (key, color) =>
		{
			switch (key)
			{
				case Bg:
					UpdateBackground(color);
					break;
				case BgDef:
					UpdateBackgroundDefault(color);
					break;
				case Foreground:
					UpdateForeground(color);
					break;
				case Border:
					UpdateBorder(color);
					break;
				case Selection:
					// _optionsTree. = color;
					break;
			}
		};
	}


	private void UpdateBackground(Color color)
	{
		Fm.BackColor = _menu.BackColor = _statusBar.BackColor = _toolsPanel.BackColor = _tools.BackColor = color;
		Fm.cmEditor.BackColor = TextEditor.Parent.BackColor = Fm.CurrentCodeFileDocument.BackColor = Fm.BottomPane.Parent.BackColor = color;
		_optionsChanger._optionsForm.BackColor = _context2.BackColor = color;
	}

	private void UpdateBackgroundDefault(Color color)
	{
		_outputPanel2.BackColor = _outputPanel6.BackColor = _outputInput.BackColor = _outputPanel4.BackColor =
			_outputPanel3.BackColor = _outputPanel5.BackColor = _outputPanel1.BackColor = _outputText.BackColor =
				_outputOutput.BackColor = Fm.ProjectPane.BackColor = _errorsList.BackColor = _compilerConsole.BackColor = color;
		_optionsChanger._optionsTree.BackColor = color;
	}

	private void UpdateForeground(Color color)
	{
		_outputOutput.ForeColor = _outputPanel2.ForeColor = _outputText.ForeColor = _menu.ForeColor =
			_statusBar.ForeColor = _toolsPanel.ForeColor = _tools.ForeColor = _errorsList.ForeColor =
				_compilerConsole.ForeColor = color;
		_optionsChanger._optionsForm.ForeColor = _optionsChanger._optionsTree.ForeColor = color;

		_optionsChanger._optionsContentContainer.ForeColor = color;
		foreach (ToolStripItem item in _context.Items)
		{
			item.ForeColor = color;
		}

		foreach (ToolStripItem item in _context2.Items)
		{
			item.ForeColor = color;
		}
	}

	private void UpdateBorder(Color color)
	{
		ChangeButtonColorsStartingFromBorder(color, _outputPanel1.Controls);
		ChangeButtonColorsStartingFromBorder(color, _optionsChanger._optionsForm.Controls);
	}


	private void ChangeButtonColorsStartingFromBorder(Color color, Control.ControlCollection collection)
	{
		foreach (Control o in collection)
		{
			if (o is Button)
			{
				Button b = (Button)o;
				UpdateButton(color, b);
			}
		}
	}

	internal static void UpdateButton(Color color, Button b)
	{
		b.BackColor = ColorReference.BackgroundDefaultColor();
		b.ForeColor = ColorReference.ForegroundColor();
		b.FlatAppearance.BorderColor = color;
		b.FlatStyle = FlatStyle.Flat;
	}

	public void ChangeStyles()
	{
		Extender.SetSchema(Fm.MainDockPanel);
	}

	public void AlterMenu()
	{
		_menuRenderer = new MenuRenderer();
		_menu.Renderer = _menuRenderer;
		_context.Renderer = _menuRenderer;
		_context2.Renderer = _menuRenderer;
		ToolRenderer toolrenderer = new ToolRenderer();
		_tools.Renderer = toolrenderer;
		_tools.Paint += ToolStripPanelOnPaint;
	}

	private void ErrorListHeaderDrawer(object sender, DrawListViewColumnHeaderEventArgs e)
	{
		e.Graphics.FillRectangle(Changer.GetBrush(BgDef), e.Bounds);

		e.Graphics.DrawString(e.Header.Text, e.Font, Changer.GetBrush(Foreground), e.Bounds);
	}


	private void RefreshEditorColors()
	{
		try
		{
			if (TextEditor.Controls.Count >= 2) TextEditor.Controls[1].Invalidate();
		}
		catch (ArgumentOutOfRangeException)
		{
		}
	}

	internal void UpdateBottomTextPanel()
	{
		RichTextBox oldInputPanel = _outputTextBoxs[Fm.CurrentCodeFileDocument];
		oldInputPanel.BackColor = Changer.GetColor(BgDef);
		oldInputPanel.BorderStyle = BorderStyle.None;
	}

	internal void UpdateAboutForm(object senderaw, EventArgs eaw)
	{
		_aboutBox.BackColor = ColorReference.BackgroundColor();
		_aboutBox.ForeColor = ColorReference.ForegroundColor();
		Button btn = null;
		foreach (Control cont in _aboutBox.Controls)
		{
			if (cont is LinkLabel)
			{
				((LinkLabel)cont).LinkColor = ColorReference.ForegroundColor();
				((LinkLabel)cont).ActiveLinkColor = ColorReference.ForegroundHoverColor();
			}
			else if (cont is Button)
				btn = (Button)cont;
			else if (cont is GroupBox)
			{
				GroupBox group = (GroupBox)cont;
				group.ForeColor = ColorReference.ForegroundColor();
				foreach (Control groupControl in group.Controls)
				{
					if (groupControl is LinkLabel label)
					{
						label.LinkColor = ColorReference.ForegroundColor();
						label.ActiveLinkColor = ColorReference.TypeColor();
					}
					else if (groupControl is ListView view)
					{
						view.OwnerDraw = true;
						view.DrawColumnHeader += ErrorListHeaderDrawer;
						view.DrawItem += (_, e) => { e.DrawDefault = true; };
						view.BackColor = ColorReference.BackgroundColor();
						view.ForeColor = ColorReference.ForegroundColor();
					}
				}
			}
			else if (cont is TableLayoutPanel)
			{
				TableLayoutPanel tbl = (TableLayoutPanel)cont;
				tbl.ForeColor = ColorReference.ForegroundColor();
				foreach (Control flowLayout in tbl.Controls)
				{
					if (flowLayout is FlowLayoutPanel)
					{
						foreach (Control tblControl in flowLayout.Controls)
						{
							if (tblControl is Label)
							{
								if (tblControl.Name.Contains("Version"))
									tblControl.ForeColor = ColorReference.BorderColor();
								else
									tblControl.ForeColor = ColorReference.ForegroundColor();
							}
						}
					}
				}
			}
		}


		btn.BackColor = ColorReference.BackgroundColor();
		btn.ForeColor = ColorReference.ForegroundColor();
		btn.FlatStyle = FlatStyle.Flat;
		btn.UseVisualStyleBackColor = false;
		btn.FlatAppearance.MouseOverBackColor = ColorReference.BackgroundClickColor();
	}


	private void ToolStripPanelOnPaint(object sender, PaintEventArgs e)
	{
		e.Graphics.DrawLine(ColorReference.BackgroundClick3Pen(), e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width,
			e.ClipRectangle.Y);
	}


	internal void AddMenuItems()
	{
		_menuSettings = null;
		foreach (ToolStripItem menuItem in _menu.Items)
		{
			if (menuItem is ToolStripMenuItem)
			{
				foreach (ToolStripItem toolStripMenuItem in ((ToolStripMenuItem)menuItem).DropDownItems)
				{
					if (toolStripMenuItem is ToolStripMenuItem)
					{
						if (toolStripMenuItem.Text == "Yuki Theme")
						{
							_menuSettings = (ToolStripMenuItem)toolStripMenuItem;
							Console.WriteLine("Found");
							break;
						}
					}
				}
			}
		}

		if (_menuSettings != null) // If we could find...
		{
			Console.WriteLine("Adding settings");

			_menuSettings.Text = "Show Settings";
			_menuSettings.ShortcutKeys = Keys.Alt | Keys.S;
			_menuSettings.ShortcutKeyDisplayString = "Alt + S";

			ToolStrip ow = _menuSettings.Owner;
			ow.Items.Remove(_menuSettings);

			// Image icon = Helper.GetYukiThemeIconImage(new Size(32, 32));

			ToolStripMenuItem main = new("Yuki Theme", null);

			var quiet = CreateMenuItem("Toggle Discreet Mode", ToggleQuiet, Keys.Alt | Keys.A, "Alt + A");

			var stick = CreateMenuItem("Enable Stickers", ToggleSticker);
			var backimage = CreateMenuItem("Enable Wallpaper", ToggleWallpaper);

			var switchTheme = CreateMenuItem("Switch Theme", SwitchTheme, Keys.Control | Keys.Oemtilde, "Ctrl + `");

			var enablePositioning = CreateMenuItem("Enable Stickers Positioning", StickersPositioning);

			var resetStickerPosition =
				CreateMenuItem("Reset Sticker Margins", ResetStickerPosition);
			var updatePage = CreateMenuItem("Show Update Notification", ShowUpdatePage);

			main.DropDownItems.Add(stick);
			main.DropDownItems.Add(backimage);
			main.DropDownItems.Add(enablePositioning);
			main.DropDownItems.Add(quiet);
			main.DropDownItems.Add(switchTheme);
			main.DropDownItems.Add(_menuSettings);
			main.DropDownItems.Add(resetStickerPosition);
			main.DropDownItems.Add(updatePage);
			MoveIconToTop(ow, main);
		}
	}

	private static void MoveIconToTop(ToolStrip ow, ToolStripMenuItem main)
	{
		List<ToolStripItem> coll = new();
		foreach (ToolStripItem item in ow.Items)
		{
			coll.Add(item);
		}

		ow.Items.Clear();
		if (coll.Last() is ToolStripSeparator)
			coll.Remove(coll.Last());

		ow.Items.Add(main);
		ow.Items.Add(new ToolStripSeparator());

		ow.Items.AddRange(coll.ToArray());
	}

	private void SwitchTheme(object sender, EventArgs e)
	{
	}

	private void ToggleSticker(object sender, EventArgs e)
	{
	}

	private void ToggleQuiet(object sender, EventArgs e)
	{
	}

	private void ToggleWallpaper(object sender, EventArgs e)
	{
	}

	private void StickersPositioning(object sender, EventArgs e)
	{
	}

	private void ResetStickerPosition(object sender, EventArgs e)
	{
	}

	private void ShowUpdatePage(object sender, EventArgs e)
	{
	}

	private ToolStripMenuItem CreateMenuItem(string text, EventHandler handler)
	{
		var item = new ToolStripMenuItem(text, null, handler)
		{
			BackColor = _menuSettings.BackColor, ForeColor = _menuSettings.ForeColor
		};

		return item;
	}

	private ToolStripMenuItem CreateMenuItem(string text, EventHandler handler, Keys shortcut,
		string shortcutDisplay)
	{
		ToolStripMenuItem item = CreateMenuItem(text, handler);
		item.ShortcutKeys = shortcut;
		item.ShortcutKeyDisplayString = shortcutDisplay;
		return item;
	}
}