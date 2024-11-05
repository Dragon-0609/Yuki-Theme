using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using VisualPascalABC;
using YukiTheme.Components.TempForm;
using YukiTheme.Style;
using static YukiTheme.Engine.ColorChanger;

namespace YukiTheme.Engine;

public class EditorAlterer
{
	private readonly TextEditorChanger _editorChanger = new();

	private readonly MenuReplacer _menuReplacer = new();

	public EditorAlterer()
	{
		EditorComponents = new EditorComponents();
	}

	private Form1 Fm => IDEAlterer.Instance.Form1;

	private EditorComponents EditorComponents { get; }

	internal void GetComponents()
	{
		EditorComponents.GetComponents();
		_editorChanger.Init(EditorComponents);
		EditorComponents._aboutBox.Shown += UpdateAboutForm;
	}

	internal void SubscribeComponents()
	{
		SubscribeUpdate();
		Instance.UpdatedColors += () =>
		{
			RefreshEditorColors();
			EditorComponents.ErrorsList.Refresh();
			UpdateBottomTextPanel();
		};
		_editorChanger.InjectCompletion();
		_editorChanger.StartInspectingBrackets();
	}

	private void SubscribeUpdate()
	{
		Instance.Update += (key, color) =>
		{
			switch (key)
			{
				case BG:
					UpdateBackground(color);
					break;
				case BG_DEF:
					UpdateBackgroundDefault(color);
					break;
				case FOREGROUND:
					UpdateForeground(color);
					break;
				case BORDER:
					UpdateBorder(color);
					break;
			}
		};
	}

	internal void UpdateIconColors()
	{
		EditorComponents.UpdateIconColors();
	}

	internal void StartMenuReplacement()
	{
		_menuReplacer.AddMenuItemsWithDelay(EditorComponents._menu);
	}

	internal void FocusEditorWindow()
	{
		Fm.Focus();
		Fm.SetFocusToEditor();
		var timer = new Timer();
		timer.Interval = 10;
		timer.Tick += (sender, args) =>
		{
			timer.Stop();
			Fm.Focus();
			Fm.SetFocusToEditor();
		};
		timer.Start();
	}


	private void UpdateBackground(Color color)
	{
		Fm.BackColor = EditorComponents._menu.BackColor =
			EditorComponents._statusBar.BackColor =
				EditorComponents._toolsPanel.BackColor = EditorComponents._tools.BackColor = color;
		Fm.cmEditor.BackColor = EditorComponents.TextEditor.Parent.BackColor =
			Fm.CurrentCodeFileDocument.BackColor = Fm.BottomPane.Parent.BackColor = color;
		EditorComponents._context2.BackColor = color;
		if (EditorComponents._projectWindow != null)
		{
			EditorComponents._projectSplitter.BackColor = color;
		}

		// TODO: Later change colors for references window

		if (EditorComponents._referenceList != null)
		{
			EditorComponents._referenceList.BackColor = color;
		}

		if (EditorComponents._referenceTabsController != null)
		{
			EditorComponents._referenceTabsController.BackColor = color;
		}
	}

	private void UpdateBackgroundDefault(Color color)
	{
		EditorComponents._outputPanel2.BackColor =
			EditorComponents._outputPanel6.BackColor = EditorComponents._outputInput.BackColor =
				EditorComponents._outputPanel4.BackColor = color;
		EditorComponents._outputPanel3.BackColor =
			EditorComponents._outputPanel5.BackColor = EditorComponents._outputPanel1.BackColor =
				EditorComponents._outputText.BackColor = color;
		EditorComponents._outputOutput.BackColor = Fm.ProjectPane.BackColor =
			EditorComponents.ErrorsList.BackColor = EditorComponents._compilerConsole.BackColor = color;

		if (EditorComponents._projectWindow != null)
		{
			EditorComponents._tvProjectExplorer.BackColor = color;
		}

		EditorComponents._referenceWindow.BackColor = color;
	}

	private void UpdateForeground(Color color)
	{
		EditorComponents._outputOutput.ForeColor = EditorComponents._outputPanel2.ForeColor =
			EditorComponents._outputText.ForeColor = EditorComponents._menu.ForeColor = color;
		EditorComponents._statusBar.ForeColor = EditorComponents._toolsPanel.ForeColor =
			EditorComponents._tools.ForeColor = EditorComponents.ErrorsList.ForeColor = color;
		EditorComponents._compilerConsole.ForeColor = color;

		foreach (ToolStripItem item in EditorComponents._context.Items) item.ForeColor = color;

		foreach (ToolStripItem item in EditorComponents._context2.Items) item.ForeColor = color;

		if (EditorComponents._projectWindow != null)
		{
			EditorComponents._tvProjectExplorer.ForeColor = color;
		}

		if (EditorComponents._referenceList != null)
		{
			EditorComponents._referenceList.ForeColor = color;
		}

		if (EditorComponents._referenceTabsController != null)
		{
			EditorComponents._referenceTabsController.ForeColor = color;
		}
	}

	private void UpdateBorder(Color color)
	{
		ChangeButtonColorsStartingFromBorder(color, EditorComponents._outputPanel1.Controls);
		ChangeButtonColorsStartingFromBorder(color, EditorComponents._referenceWindow.Controls);
	}


	internal static void ChangeButtonColorsStartingFromBorder(Color color, Control.ControlCollection collection)
	{
		foreach (Control o in collection)
			if (o is Button)
			{
				var b = (Button)o;
				UpdateButton(color, b);
			}
	}

	internal static void UpdateButton(Color color, Button b)
	{
		b.BackColor = ColorReference.BackgroundDefaultColor;
		b.ForeColor = ColorReference.ForegroundColor;
		b.FlatAppearance.BorderColor = color;
		b.FlatStyle = FlatStyle.Flat;
	}

	internal void ChangeStyles()
	{
		Extender.SetSchema(Fm.MainDockPanel);
	}

	internal void AlterMenu()
	{
		EditorComponents._menuRenderer = new MenuRenderer();
		EditorComponents._menu.Renderer = EditorComponents._menuRenderer;
		EditorComponents._context.Renderer = EditorComponents._menuRenderer;
		EditorComponents._context2.Renderer = EditorComponents._menuRenderer;
		var toolrenderer = new ToolRenderer();
		EditorComponents._tools.Renderer = toolrenderer;
		EditorComponents._tools.Paint += PaintOnToolBar;
	}

	internal void RequestBottomBarUpdate()
	{
		UpdateBottomTextPanel();
	}


	private void RefreshEditorColors()
	{
		try
		{
			if (EditorComponents.TextEditor.Controls.Count >= 2) EditorComponents.TextEditor.Controls[1].Invalidate();
		}
		catch (ArgumentOutOfRangeException)
		{
		}
	}

	private void UpdateBottomTextPanel()
	{
		var oldInputPanel = EditorComponents._outputTextBoxs[Fm.CurrentCodeFileDocument];
		oldInputPanel.BackColor = Instance.GetColor(BG_DEF);
		oldInputPanel.BorderStyle = BorderStyle.None;
	}

	private void UpdateAboutForm(object senderaw, EventArgs eaw)
	{
		EditorComponents._aboutBox.BackColor = ColorReference.BackgroundColor;
		EditorComponents._aboutBox.ForeColor = ColorReference.ForegroundColor;
		Button btn = null;
		foreach (Control cont in EditorComponents._aboutBox.Controls)
			if (cont is LinkLabel link)
			{
				link.LinkColor = ColorReference.ForegroundColor;
				link.ActiveLinkColor = ColorReference.ForegroundHoverColor;
			}
			else if (cont is Button button)
			{
				btn = button;
			}
			else if (cont is GroupBox group)
			{
				group.ForeColor = ColorReference.ForegroundColor;
				foreach (Control groupControl in group.Controls)
					if (groupControl is LinkLabel label)
					{
						label.LinkColor = ColorReference.ForegroundColor;
						label.ActiveLinkColor = ColorReference.TypeColor;
					}
					else if (groupControl is ListView view)
					{
						view.OwnerDraw = true;
						view.DrawColumnHeader += EditorComponents.ErrorListHeaderDrawer;
						view.DrawItem += (_, e) =>
						{
							e.DrawDefault = true;
						};
						view.BackColor = ColorReference.BackgroundColor;
						view.ForeColor = ColorReference.ForegroundColor;
					}
			}
			else if (cont is TableLayoutPanel table)
			{
				table.ForeColor = ColorReference.ForegroundColor;
				foreach (Control flowLayout in table.Controls)
					if (flowLayout is FlowLayoutPanel)
						foreach (Control tblControl in flowLayout.Controls)
							if (tblControl is Label)
								tblControl.ForeColor = tblControl.Name.Contains("Version")
									? ColorReference.BorderColor
									: ColorReference.ForegroundColor;
			}


		btn.BackColor = ColorReference.BackgroundColor;
		btn.ForeColor = ColorReference.ForegroundColor;
		btn.FlatStyle = FlatStyle.Flat;
		btn.UseVisualStyleBackColor = false;
		btn.FlatAppearance.MouseOverBackColor = ColorReference.BackgroundClickColor;
	}


	private void PaintOnToolBar(object sender, PaintEventArgs e)
	{
		e.Graphics.DrawLine(ColorReference.BackgroundClick3Pen, e.ClipRectangle.X, e.ClipRectangle.Y,
			e.ClipRectangle.Width,
			e.ClipRectangle.Y);
	}

	public Form GetSettingsParent() => EditorComponents.GetSettingsParent();
}