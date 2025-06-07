using System;
using System.Drawing;
using System.Windows.Forms;
using Fluent;
using Fluent.Lists;
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
}