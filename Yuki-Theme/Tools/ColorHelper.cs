using System;
using System.Drawing;
using System.Windows.Forms;
using Fluent;
using Fluent.Lists;
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
		Console.WriteLine($"Changing color of {control.Name}");
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
				Console.WriteLine($"Changing color of {control.Name}");
			}
			else if (control is ListView list)
			{
				control.BackColor = backColor;
				control.ForeColor = foreColor;
				Console.WriteLine($"Changing color of {control.Name}");

				/*
				list.OwnerDraw = true;
				list.View = View.Details;
				list.DrawColumnHeader -= ErrorListHeaderDrawer;
				list.DrawItem -= OnListOnDrawItem;
				list.DrawSubItem -= ListOnDrawSubItem;
				list.DrawColumnHeader += ErrorListHeaderDrawer;
				list.DrawItem += OnListOnDrawItem;
				list.DrawSubItem += ListOnDrawSubItem;*/
			}

			SetColorsToAllChildren(control.Controls, updateLabelBackground, comboBoxDrawer);
		}
	}

	private static void ListOnDrawSubItem(object sender, DrawListViewSubItemEventArgs e)
	{
		e.Graphics.FillRectangle(ColorChanger.Instance.GetBrush(ColorChanger.BG_DEF), e.Bounds);
	}

	private static void OnListOnDrawItem(object sender, DrawListViewItemEventArgs e)
	{
		e.Graphics.FillRectangle(ColorChanger.Instance.GetBrush(ColorChanger.BG_DEF), e.Bounds);
		// e.DrawDefault = true;
	}

	private static void ErrorListHeaderDrawer(object sender, DrawListViewColumnHeaderEventArgs e)
	{
		e.Graphics.FillRectangle(new SolidBrush(Color.LightBlue), e.Bounds); // first we fill the header with our color.
		e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black), 2), e.Bounds.X + 1, e.Bounds.Y + 1,
			e.Bounds.Width - 2, e.Bounds.Height - 2); //then we draw an outline
		e.Graphics.DrawString(e.Header.Text, new Font("tahoma", 12), new SolidBrush(Color.Black),
			e.Bounds); // then we draw the text, this bit could use some improvement, if you cant figure out, let me know and ill knock some more code together

		// e.Graphics.FillRectangle(ColorChanger.Instance.GetBrush(ColorChanger.BG_DEF), e.Bounds);

		// e.Graphics.DrawString(e.Header.Text, e.Font, ColorChanger.Instance.GetBrush(ColorChanger.FOREGROUND), e.Bounds);
	}
}