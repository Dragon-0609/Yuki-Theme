using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VisualPascalABC;
using VisualPascalABC.OptionsContent;
using YukiTheme.Components;
using YukiTheme.Tools;
using static YukiTheme.Engine.ColorChanger;

namespace YukiTheme.Engine;

public class OptionsChanger
{
	private static OptionsChanger _instance;

	private Form1 _fm;
	private Brush _lastBrush;
	private int _lastIndex = -1;
	private Rectangle _lastRectangle;
	private GroupBox _optionsContentContainer;
	private Panel _optionsContentPanel;
	private OptionsContentEngine _optionsEngine;
	private OptionsForm _optionsForm;
	private TreeView _optionsTree;
	private KeyValuePair<TreeNode, IOptionsContent> _pluginNode;
	private bool SelectYukiThemeNode;
	private PluginSettingsControl _content;

	internal void GetOptionsComponents(Form1 form1)
	{
		_instance = this;
		_fm = form1;
		_optionsEngine = _fm.GetByReflection<OptionsContentEngine>("optionsContentEngine");

		_content = new PluginSettingsControl();
		_optionsEngine.AddContent(_content);

		_optionsForm = new OptionsForm(_optionsEngine);
		_optionsForm.Shown += SelectPluginNode;
		_optionsTree = _optionsForm.GetByReflection<TreeView>("tvContentList");
		_optionsTree.AfterSelect += AfterOptionsTreeSelected;

		GetPluginNode(_content);


		_optionsContentPanel = _optionsForm.GetByReflection<Panel>("contentPanel");
		_optionsContentContainer = _optionsForm.GetByReflection<GroupBox>("contentContainer");

		_optionsEngine.SetByReflection("optionsWindow", _optionsForm);
		SubscribeToColors();
	}

	internal static void ShowSettings()
	{
		_instance.SelectYukiThemeNode = true;
		_instance._optionsForm.ShowDialog();
	}

	private void GetPluginNode(PluginSettingsControl content)
	{
		var nodes = _optionsForm.GetByReflection<Dictionary<TreeNode, IOptionsContent>>("nodes");
		_pluginNode = nodes.First(n => n.Key.Text == content.ContentName);
	}

	public Form GetSettingsParent() => _content.ExtractOptionsParent();

	private void SelectPluginNode(object sender, EventArgs e)
	{
		if (SelectYukiThemeNode)
		{
			SelectYukiThemeNode = false;
			_optionsTree.SelectedNode = _pluginNode.Key;
			_optionsTree.SetByReflection("lastSelectedNode", _pluginNode);
		}
	}

	private void AfterOptionsTreeSelected(object sender, TreeViewEventArgs e)
	{
		ColorHelper.SetColorsToAllChildren(_optionsContentPanel.Controls, false, comboBox1_DrawItem);
	}


	private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
	{
		var comboBox = (ComboBox)sender;
		if (!comboBox.DroppedDown)
		{
			if (e.Index >= 0)
				DrawComboBoxItem(e, e.Bounds, ColorReference.BackgroundBrush, comboBox.Items[e.Index].ToString());
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
				DrawComboBoxItem(e, _lastRectangle, ColorReference.BackgroundBrush,
					comboBox.Items[_lastIndex].ToString());
			_lastIndex = -1;
		}

		e.DrawBackground();
		var text = comboBox.Items[e.Index].ToString();

		_lastBrush = ColorReference.BackgroundBrush;
		if (comboBox.SelectedIndex == e.Index) _lastBrush = ColorReference.SelectionBrush;
		DrawComboBoxItem(e, e.Bounds, _lastBrush, text);

		_lastRectangle = e.Bounds;
		_lastIndex = e.Index;
	}

	private static void DrawComboBoxItem(DrawItemEventArgs e, Rectangle rect, Brush backgroundBrush, string text)
	{
		e.Graphics.FillRectangle(backgroundBrush, rect);
		e.Graphics.DrawString(text, e.Font, ColorReference.ForegroundBrush, new Point(rect.X, rect.Y));
		e.DrawFocusRectangle();
	}

	private void SubscribeToColors()
	{
		Instance.Update += UpdateColors;
	}

	private void UpdateColors(string key, Color color)
	{
		switch (key)
		{
			case BG:
				_optionsForm.BackColor = color;
				break;
			case BG_DEF:
				_optionsTree.BackColor = color;
				break;
			case FOREGROUND:
				_optionsContentContainer.ForeColor = color;
				_optionsForm.ForeColor = _optionsTree.ForeColor = color;
				break;
			case BORDER:
				EditorAlterer.ChangeButtonColorsStartingFromBorder(color, _optionsForm.Controls);
				break;
		}
	}
}