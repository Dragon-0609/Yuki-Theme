using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Fluent;
using Fluent.Lists;
using VisualPascalABCPlugins;
using WeifenLuo.WinFormsUI.Docking;
using YukiTheme.Components;
using YukiTheme.Engine.Plugins.Definitions.CodeTemplate;
using YukiTheme.Tools;

namespace YukiTheme.Engine.Processors
{
	public class LanguageConverterProcessor : InjectorProcessor
	{
		public override string Name => LanguageConverter_.StringsPrefix;
		protected override bool InjectImmediate => true;

		private object _formatter;
		private Brush _lastBrush;
		private int _lastIndex = -1;
		private Rectangle _lastRectangle;
		private Form _form;

		protected override void Process(PluginGUIItem plugin)
		{
			var execute = plugin.GetByReflection<PluginGUIItemExecuteDelegate>("executeDelegate");

			object pluginCore = execute.Target;

			_formatter = pluginCore.GetByReflection(nameof(LanguageConverter_.TextFormatterForm), false);

			_form = ((Form)_formatter);

			ColorUpdated += UpdateColors;

			// InjectListView();

			UpdateColors();
		}

		private void ClosedForm(object sender, EventArgs e)
		{
			ColorUpdated -= UpdateColors;
		}

		private void UpdateColors()
		{
			ColorHelper.SetColors(_form, true, comboBox1_DrawItem);
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

			if (e.Index != -1 && e.Index == _lastIndex)
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
			string text;
			text = e.Index == -1 ? "" : comboBox.Items[e.Index].ToString();

			_lastBrush = ColorReference.BackgroundBrush;
			bool isSelected = comboBox.SelectedIndex == e.Index;
			if (isSelected) _lastBrush = ColorReference.SelectionBrush;
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
	}
}