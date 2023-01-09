using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FastColoredTextBoxNS;
using Yuki_Theme.Core.Themes;
using Brush = System.Windows.Media.Brush;
using Color = System.Drawing.Color;

namespace Yuki_Theme.Core.WPF.Controls
{
	public partial class ThemePreview : UserControl
	{

		public ThemePreview ()
		{
			InitializeComponent ();
		}

		public void SetTheme (Theme theme, HighlighterBase _highlighter)
		{
			ThemeField field = theme.GetDefaultColors();
			ThemeName.Text = theme.Name;
			SetColors(field);

			Fstb.SetReadOnly();
			Dictionary<string, TextStyle> styles = new Dictionary<string, TextStyle>();
			_highlighter.InitStyles(ref styles);
			// _highlighter.Init(preview.Fstb.box);
			Fstb.Text =
				"function Add (x, y: integer) : integer;\nbegin\n	result := x + y;\nend;\nvar a, b : integer;\nbegin\n	Readln(a);\n	Readln(b);\n	Writeln('Result: ' + Add(a,b));\nend.";
			_highlighter.UpdateColors(Fstb.box, theme.Fields, ref styles);
			_highlighter.Highlight(Fstb.box, ref styles);

			
		}

		private void SetColors(ThemeField field)
		{
			Color bgdef = ColorTranslator.FromHtml(field.Background);
			Color bg = Helper.DarkerOrLighter(bgdef, 0.05f);
			Color active = Helper.DarkerOrLighter(bgdef, 0.25f);

			ThemeName.Background = ConvertToBrush(bg, 0.95);

			ActiveTab.Background = ConvertToBrush(active, 0.95);

			Strip.Background = ConvertToBrush(bgdef, 0.95);

			Foreground = ConvertToBrush(field.Foreground);
		}

		private static Brush ConvertToBrush(string hex, double opacity = 1)
		{
			BrushConverter bc = new BrushConverter();
			Brush brush = (Brush)bc.ConvertFrom(hex);
			brush.Opacity = opacity;
			brush.Freeze();
			return brush;
		}

		private static Brush ConvertToBrush(Color color, double opacity = 1)
		{
			return new SolidColorBrush(color.ToWPFColor()){Opacity = opacity};
		}
	}
}