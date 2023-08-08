using System.Drawing;
using System.Windows.Forms;
using VisualPascalABC;
using VisualPascalABC.OptionsContent;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class OptionsChanger
{
	private OptionsContentEngine _optionsEngine;
	internal OptionsForm _optionsForm;
	internal TreeView _optionsTree;
	private Panel _optionsContentPanel;
	internal GroupBox _optionsContentContainer;
	private int _lastIndex = -1;
	private Rectangle _lastRectangle;
	private Brush _lastBrush;
	private Form1 fm;

	public void GetOptionsComponents(Form1 form1)
	{
		fm = form1;
		_optionsEngine = fm.GetByReflection<Form1, OptionsContentEngine>("optionsContentEngine");

		_optionsForm = new OptionsForm(_optionsEngine);
		_optionsTree = _optionsForm.GetByReflection<OptionsForm, TreeView>("tvContentList");
		_optionsTree.AfterSelect += AfterOptionsTreeSelected;

		_optionsContentPanel = _optionsForm.GetByReflection<OptionsForm, Panel>("contentPanel");
		_optionsContentContainer = _optionsForm.GetByReflection<OptionsForm, GroupBox>("contentContainer");

		_optionsEngine.SetByReflection("optionsWindow", _optionsForm);
	}

	private void AfterOptionsTreeSelected(object sender, TreeViewEventArgs e)
	{
		SetColorsToAllChildren(_optionsContentPanel.Controls);
	}

	internal void SetColorsToAllChildren(Control.ControlCollection collection)
	{
		foreach (Control controlControl in collection)
		{
			if (controlControl is Label)
			{
				controlControl.ForeColor = ColorReference.ForegroundColor();
			}
			else if (controlControl is Button button)
			{
				button.ForeColor = ColorReference.ForegroundColor();
				button.BackColor = ColorReference.BackgroundColor();
				EditorAlterer.UpdateButton(ColorReference.BorderColor(), button);
			}
			else if (controlControl is CheckBox checkBox)
			{
				checkBox.BackColor = ColorReference.BackgroundColor();
				checkBox.ForeColor = ColorReference.ForegroundColor();
			}
			else if (controlControl is ComboBox comboBox)
			{
				comboBox.BackColor = ColorReference.BackgroundColor();
				comboBox.ForeColor = ColorReference.ForegroundColor();
				comboBox.DrawMode = DrawMode.OwnerDrawFixed;
				comboBox.DrawItem += comboBox1_DrawItem;
			}
			else if (controlControl is TextBox textBox)
			{
				textBox.BorderStyle = BorderStyle.FixedSingle;
				textBox.BackColor = ColorReference.BackgroundColor();
				textBox.ForeColor = ColorReference.ForegroundColor();
			}
			else
			{
				SetColorsToAllChildren(controlControl.Controls);
			}
		}
	}

	private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
	{
		ComboBox comboBox = (ComboBox)sender;
		if (!comboBox.DroppedDown)
		{
			if (e.Index >= 0)
				DrawComboBoxItem(e, e.Bounds, ColorReference.BackgroundBrush(), comboBox.Items[e.Index].ToString());
			return;
		}

		if (e.Index == _lastIndex)
		{
			DrawComboBoxItem(e, e.Bounds, _lastBrush, comboBox.Items[e.Index].ToString());
			return;
		}

		if (_lastIndex >= 0)
		{
			if (_lastIndex < comboBox.Items.Count)
				DrawComboBoxItem(e, _lastRectangle, ColorReference.BackgroundBrush(), comboBox.Items[_lastIndex].ToString());
			_lastIndex = -1;
		}

		e.DrawBackground();
		string text = comboBox.Items[e.Index].ToString();

		_lastBrush = ColorReference.BackgroundBrush();
		if (comboBox.SelectedIndex == e.Index) _lastBrush = ColorReference.SelectionBrush();
		DrawComboBoxItem(e, e.Bounds, _lastBrush, text);

		_lastRectangle = e.Bounds;
		_lastIndex = e.Index;
	}

	private static void DrawComboBoxItem(DrawItemEventArgs e, Rectangle rect, Brush backgroundBrush, string text)
	{
		e.Graphics.FillRectangle(backgroundBrush, rect);
		e.Graphics.DrawString(text, e.Font, ColorReference.ForegroundBrush(), new Point(rect.X, rect.Y));
		e.DrawFocusRectangle();
	}
}