using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using VisualPascalABC;
using VisualPascalABCPlugins;
using YukiTheme.Style;

namespace YukiTheme.Engine;

public class EditorComponents
{
    private Form1 Fm => IDEAlterer.Instance.Form1;
    internal MenuStrip _menu;
    internal StatusStrip _statusBar;
    internal Panel _toolsPanel;
    internal ToolStrip _tools;
    internal CodeFileDocumentTextEditorControl TextEditor;
    internal TextArea TextArea;
    internal ContextMenuStrip _context;
    internal ContextMenuStrip _context2;
    internal Control _outputWindow;
    internal Panel _outputPanel2;
    internal Panel _outputPanel6;
    internal RichTextBox _outputOutput;
    internal Panel _outputInput;
    internal Panel _outputPanel4;
    internal Panel _outputPanel3;
    internal Panel _outputPanel5;
    internal Panel _outputPanel1;
    internal TextBox _outputText;
    internal ListView ErrorsList;
    internal TextBox _compilerConsole;
    internal Dictionary<ICodeFileDocument, RichTextBox> _outputTextBoxs;
    internal MenuRenderer _menuRenderer;
    internal AboutBox _aboutBox;
    internal readonly OptionsChanger _optionsChanger = new();

    public EditorComponents()
    {
    }

    public void GetComponents()
    {
        GetMainComponenets();

        GetEditorComponents();

        // _textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += UpdateMarkerBGOnCaretPositionChanged;
        // _textEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += RemoveErrorMarksOnCaretPositionChanged;

        GetBottomComponents();

        _optionsChanger.GetOptionsComponents(Fm);

        GetAboutForm();
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


        ErrorsListWindowForm erw = (ErrorsListWindowForm)IDEAlterer.Instance.Workbench.ErrorsListWindow;
        ErrorsList = (ListView)erw.Controls.Find("lvErrorsList", false)[0];
        ErrorsList.OwnerDraw = true;
        ErrorsList.DrawColumnHeader += ErrorListHeaderDrawer;
        ErrorsList.DrawItem += (sender, e) => { e.DrawDefault = true; };


        CompilerConsoleWindowForm cons = (CompilerConsoleWindowForm)IDEAlterer.Instance.Workbench.CompilerConsoleWindow;
        _compilerConsole = (TextBox)cons.Controls.Find("CompilerConsole", false)[0];

        FieldInfo fp = typeof(Form1).GetField("OutputTextBoxs", BindingFlags.Instance | BindingFlags.NonPublic);

        _outputTextBoxs = (Dictionary<ICodeFileDocument, RichTextBox>)fp.GetValue(Fm);
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
}