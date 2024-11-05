using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using VisualPascalABC;
using VisualPascalABCPlugins;
using YukiTheme.Components;
using YukiTheme.Components.TempForm;
using YukiTheme.Style;

namespace YukiTheme.Engine;

public class EditorComponents
{
	private readonly OptionsChanger _optionsChanger = new();
	internal AboutBox _aboutBox;
	private IconAlterer _alterer;
	internal TextBox _compilerConsole;
	internal ContextMenuStrip _context;
	internal ContextMenuStrip _context2;
	internal MenuStrip _menu;
	internal MenuRenderer _menuRenderer;

	private NameBar _nameBar;
	internal Panel _outputInput;
	internal RichTextBox _outputOutput;
	internal Panel _outputPanel1;
	internal Panel _outputPanel2;
	internal Panel _outputPanel3;
	internal Panel _outputPanel4;
	internal Panel _outputPanel5;
	internal Panel _outputPanel6;
	internal TextBox _outputText;
	internal Dictionary<ICodeFileDocument, RichTextBox> _outputTextBoxs;
	private Control _outputWindow;
	internal ProjectExplorerForm _projectWindow;
	internal TreeView _tvProjectExplorer;
	internal Control _projectSplitter;
	internal StatusStrip _statusBar;
	internal ToolStrip _tools;
	internal Panel _toolsPanel;
	internal ListView ErrorsList;
	internal TextArea TextArea;
	internal CodeFileDocumentTextEditorControl TextEditor;

	internal ReferenceForm _referenceWindow;
	internal Control _referenceList;
	internal TabControl _referenceTabsController;

	private Form1 Fm => IDEAlterer.Instance.Form1;

	internal void GetComponents()
	{
		GetMainComponenets();

		GetEditorComponents();

		// _textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += UpdateMarkerBGOnCaretPositionChanged;
		// _textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += RemoveErrorMarksOnCaretPositionChanged;

		GetBottomComponents();

		_optionsChanger.GetOptionsComponents(Fm);

		GetAboutForm();

		_alterer = new IconAlterer(_tools, _menu, _context, _context2, Fm);
		PluginEvents.Instance.Reload += _alterer.UpdateColors;
		// TreeGridView watchList = _debugVariablesListWindow.\
	}

	private void GetMainComponenets()
	{
		_menu = (MenuStrip)Fm.Controls.Find("menuStrip1", false)[0];
		_statusBar = (StatusStrip)Fm.Controls.Find("statusStrip1", false)[0];
		_toolsPanel = (Panel)Fm.Controls.Find("toolStripPanel", false)[0];
		_tools = (ToolStrip)_toolsPanel.Controls.Find("toolStrip1", false)[0];
	}

	private void GetEditorComponents()
	{
		TextEditor = Fm.CurrentCodeFileDocument.TextEditor;
		TextArea = TextEditor.ActiveTextAreaControl.TextArea;
		_context = TextEditor.ContextMenuStrip;
		_context2 = Fm.MainDockPanel.ContextMenuStrip;
	}

	private void GetBottomComponents()
	{
		_outputWindow = (Control)IDEAlterer.Instance.Workbench.OutputWindow;
		TryToGetProjectWindow();
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


		var erw = (ErrorsListWindowForm)IDEAlterer.Instance.Workbench.ErrorsListWindow;
		ErrorsList = (ListView)erw.Controls.Find("lvErrorsList", false)[0];
		ErrorsList.OwnerDraw = true;
		ErrorsList.DrawColumnHeader += ErrorListHeaderDrawer;
		ErrorsList.DrawItem += (sender, e) =>
		{
			e.DrawDefault = true;
		};

		GetReferenceForm();

		var cons = (CompilerConsoleWindowForm)IDEAlterer.Instance.Workbench.CompilerConsoleWindow;
		_compilerConsole = (TextBox)cons.Controls.Find("CompilerConsole", false)[0];

		var fp = typeof(Form1).GetField("OutputTextBoxs", BindingFlags.Instance | BindingFlags.NonPublic);

		_outputTextBoxs = (Dictionary<ICodeFileDocument, RichTextBox>)fp.GetValue(Fm);

		_nameBar = new NameBar();

		_statusBar.Items.Add(_nameBar.GetControl());
		_statusBar.Renderer = new ToolRenderer();
	}

	private void GetReferenceForm()
	{
		_referenceWindow = ReferenceForm.Instance;
		if (ReferenceWindowClone.Get(_referenceWindow, nameof(ReferenceWindowClone.lvGac), out ListView lv))
		{
			_referenceList = lv;
		}

		if (ReferenceWindowClone.Get(_referenceWindow, nameof(ReferenceWindowClone.tabControl1),
			    out TabControl tabControl1))
		{
			_referenceTabsController = tabControl1;
		}
	}

	private void TryToGetProjectWindow()
	{
		FieldInfo field = IDEAlterer.Instance.Form1.GetType()
			.GetField("ProjectExplorerWindow", BindingFlags.NonPublic | BindingFlags.Instance);
		if (field == null) return;
		_projectWindow = (ProjectExplorerForm)field.GetValue(IDEAlterer.Instance.Form1);
		if (_projectWindow != null)
		{
			_tvProjectExplorer = (TreeView)_projectWindow.Controls.Find("tvProjectExplorer", false)[0];
			_projectSplitter = _projectWindow.Parent.Parent.Controls[0];
		}
		else
		{
			_tvProjectExplorer = null;
		}
	}

	private void GetAboutForm()
	{
		_aboutBox = Fm.AboutBox1;
	}

	internal void ErrorListHeaderDrawer(object sender, DrawListViewColumnHeaderEventArgs e)
	{
		e.Graphics.FillRectangle(ColorChanger.Instance.GetBrush(ColorChanger.BG_DEF), e.Bounds);

		e.Graphics.DrawString(e.Header.Text, e.Font, ColorChanger.Instance.GetBrush(ColorChanger.FOREGROUND), e.Bounds);
	}

	internal void UpdateIconColors()
	{
		_alterer.UpdateColors();
	}

	public Form GetSettingsParent() => _optionsChanger.GetSettingsParent();
}