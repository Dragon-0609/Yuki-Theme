using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using AdvancedDataGridView;
using Fluent;
using Fluent.Lists;
using ICSharpCode.TextEditor;
using VisualPascalABC;
using YukiTheme.Components;
using YukiTheme.Engine;

namespace YukiTheme.Tools;

public static class ColorHelper
{
	#region Color Management

	public static Color ChangeColorBrightness(Color color, float correctionFactor)
	{
		float red = color.R;
		float green = color.G;
		float blue = color.B;

		if (correctionFactor < 0)
		{
			correctionFactor = 1 + correctionFactor;
			red *= correctionFactor;
			green *= correctionFactor;
			blue *= correctionFactor;
		}
		else
		{
			red = (255 - red) * correctionFactor + red;
			green = (255 - green) * correctionFactor + green;
			blue = (255 - blue) * correctionFactor + blue;
		}

		return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
	}

	public static bool IsDark(Color clr)
	{
		var dark = (clr.R + clr.G + clr.B) / 3 < 127;
		return dark;
	}

	public static Color DarkerOrLighter(Color clr, float percent = 0)
	{
		if (IsDark(clr))
			return ChangeColorBrightness(clr, percent);
		return ChangeColorBrightness(clr, -percent);
	}

	#endregion

	private static List<TreeGridView> _processedTrees = new();

	private static List<CodeFileDocumentTextEditorControl> _processedEditors = new();
	private static FoldMargin _foldmargin;
	private static IconBarMargin _margin;

	internal static void SetColorsToAllChildren(Control.ControlCollection collection, bool updateLabelBackground,
		DrawItemEventHandler comboBoxDrawer)
	{
		foreach (Control controlControl in collection)
		{
			SetColors(controlControl, updateLabelBackground, comboBoxDrawer);
		}
	}

	public static void SetColors(Control control, bool updateLabelBackground, DrawItemEventHandler comboBoxDrawer)
	{
		Color backColor = ColorReference.BackgroundColor;
		Color foreColor = ColorReference.ForegroundColor;

		/*
		backColor = Color.Red;
		foreColor = Color.Yellow;*/

		if (control.ContextMenuStrip != null)
		{
			ContextMenuStrip strip = control.ContextMenuStrip;
			strip.BackColor = backColor;
			strip.ForeColor = foreColor;
			if (strip.Renderer != EditorComponents._menuRenderer)
			{
				strip.Renderer = EditorComponents._menuRenderer;
			}

			foreach (ToolStripItem item in strip.Items)
			{
				item.BackColor = backColor;
				item.ForeColor = foreColor;
			}
		}

		if (control is Label)
		{
			control.ForeColor = foreColor;
			if (updateLabelBackground)
				control.BackColor = backColor;
		}
		else if (control is Button button)
		{
			button.ForeColor = foreColor;
			button.BackColor = backColor;
			EditorAlterer.UpdateButton(ColorReference.BorderColor, button);
		}
		else if (control is CheckBox checkBox)
		{
			checkBox.BackColor = backColor;
			checkBox.ForeColor = foreColor;
		}
		else if (control is ComboBox comboBox)
		{
			comboBox.BackColor = backColor;
			comboBox.ForeColor = foreColor;
			if (comboBoxDrawer != null)
			{
				comboBox.DrawMode = DrawMode.OwnerDrawFixed;
				comboBox.DrawItem += comboBoxDrawer;
			}
		}
		else if (control is TextBox textBox)
		{
			textBox.BorderStyle = BorderStyle.FixedSingle;
			textBox.BackColor = backColor;
			textBox.ForeColor = foreColor;
		}
		else if (control is TreeGridView tree)
		{
			tree.BackColor = backColor;
			tree.ForeColor = foreColor;
			tree.BackgroundColor = backColor;
			tree.GridColor = ColorReference.BorderColor;
			if (!_processedTrees.Contains(tree))
			{
				DataGridViewCellStyle style = new DataGridViewCellStyle();
				style.BackColor = backColor;
				style.ForeColor = foreColor;

				tree.DefaultCellStyle = style;
				tree.RowsDefaultCellStyle = style;
				tree.ColumnHeadersDefaultCellStyle = style;
				tree.RowHeadersDefaultCellStyle = style;

				_processedTrees.Add(tree);
			}
		}
		else if (control is CodeFileDocumentTextEditorControl code)
		{
			code.BackColor = backColor;
			code.ForeColor = foreColor;
			code.Parent.BackColor = backColor;
			if (!_processedEditors.Contains(code))
			{
				SetMargin(code.ActiveTextAreaControl.TextArea);
				code.ActiveTextAreaControl.TextArea.Paint += PaintBg;
				_processedEditors.Add(code);
			}
		}
		else
		{
			if (control is Form)
			{
				control.BackColor = backColor;
				control.ForeColor = foreColor;
			}
			else if (control is ListView list)
			{
				control.BackColor = backColor;
				control.ForeColor = foreColor;

				if (control is ListViewExt listEx)
				{
					listEx.GroupHeadingBackColor = backColor;
					listEx.GroupHeadingForeColor = ColorReference.BorderColor;
					listEx.SeparatorColor = ColorReference.BorderColor;
					listEx.ListViewSelectionColor = ColorReference.SelectionColor;
				}
			}

			SetColorsToAllChildren(control.Controls, updateLabelBackground, comboBoxDrawer);
		}
	}


	private static void PaintBg(object sender, PaintEventArgs e)
	{
		TextArea area = (TextArea)sender;
		if (_margin != null)
		{
			e.Graphics.FillRectangle(ColorReference.BackgroundDefaultBrush, _margin.DrawingPosition.X,
				_margin.DrawingPosition.Y,
				_margin.DrawingPosition.Width, _margin.DrawingPosition.Height);
			var inside =
				typeof(IconBarMargin).GetMethod("IsLineInsideRegion",
					BindingFlags.Static | BindingFlags.NonPublic);
			// paint icons
			foreach (var mark in area.Document.BookmarkManager.Marks)
			{
				var lineNumber = area.Document.GetVisibleLine(mark.LineNumber);
				var lineHeight = area.TextView.FontHeight;
				var yPos = lineNumber * lineHeight - area.VirtualTop.Y;
				if ((bool)inside.Invoke(
					    null,
					    new object[]
						    { yPos, yPos + lineHeight, _margin.DrawingPosition.Y, _margin.DrawingPosition.Height }))
				{
					if (lineNumber == area.Document.GetVisibleLine(mark.LineNumber - 1))
						// marker is inside folded region, do not draw it
						continue;

					mark.Draw(_margin, e.Graphics, new Point(0, yPos));
				}
			}
		}

		if (_foldmargin != null)
		{
			SetMarginPosition(area);
			e.Graphics.DrawLine(
				BrushRegistry.GetDotPen(ColorReference.BackgroundDefaultColor, ColorReference.BorderColor),
				_foldmargin.DrawingPosition.X,
				_foldmargin.DrawingPosition.Y,
				_foldmargin.DrawingPosition.X,
				_foldmargin.DrawingPosition.Height);
		}
	}


	#region Margins

	internal static void SetMargin(TextArea area)
	{
		foreach (var margins in area.LeftMargins)
			if (margins is IconBarMargin margin)
				_margin = margin;
			else if (margins is FoldMargin foldMargin) _foldmargin = foldMargin;
	}

	internal static void SetMarginPosition(TextArea area)
	{
		var currentXPos = 0;
		foreach (var margins in area.LeftMargins)
		{
			var marginRectangle = new Rectangle(currentXPos, 0, margins.Size.Width, area.Height);
			if (margins.IsVisible || margins is FoldMargin) currentXPos += margins.DrawingPosition.Width;

			if (margins is FoldMargin)
			{
				if (marginRectangle != _margin.DrawingPosition)
					// Be sure that the line has valid rectangle
					_foldmargin.DrawingPosition = marginRectangle;

				break;
			}
		}
	}

	#endregion
}