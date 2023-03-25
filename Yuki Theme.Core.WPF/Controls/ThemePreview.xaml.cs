using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using FastColoredTextBoxNS;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.Utils;
using Yuki_Theme.Core.WPF.Windows;
using Brush = System.Windows.Media.Brush;
using Color = System.Drawing.Color;
using UserControl = System.Windows.Controls.UserControl;
using Drawing = System.Drawing;

namespace Yuki_Theme.Core.WPF.Controls
{
    public partial class ThemePreview : UserControl
    {
        private Drawing.Size defaultSmallSize = new Drawing.Size(16, 16);
        public ThemeDownloaderWindow DownloaderWindow;

        public ThemePreview()
        {
            InitializeComponent();
        }

        public void SetTheme(Theme theme, HighlighterBase _highlighter)
        {
            ThemeField field = theme.GetDefaultColors();
            ThemeName.Text = theme.Name;
            SetColors(field);
            Fstb.SetReadOnly();
            Fstb.SetVerticalOff();

            Dictionary<string, TextStyle> styles = new Dictionary<string, TextStyle>();
            _highlighter.InitStyles(ref styles);
            // _highlighter.Init(preview.Fstb.box);
            Fstb.Text =
                "function Add (x, y: integer) : integer;\nbegin\n	result := x + y;\nend;\nvar a, b : integer;\nbegin\n	Readln(a);\n	Readln(b);\n	Writeln('Result: ' + Add(a,b));\nend.";
            _highlighter.UpdateColors(Fstb.box, theme.Fields, ref styles);
            _highlighter.Highlight(Fstb.box, ref styles);

            string add = Helper.IsDark(ColorTranslator.FromHtml(field.Background)) ? "" : "_dark";
            ColorKeeper.fgColor = ColorTranslator.FromHtml(field.Foreground);

            // WPFHelper.GenerateTag
            Tag = WPFHelper.GenerateTagfromTheme(theme);
            LoadSVG(add);
        }

        private void LoadSVG(string add)
        {
            SetResourceSvg("DownloadImage", "traceInto" + add, null, defaultSmallSize, "Yuki_Theme.Core.Resources.SVG",
                CentralAPI.Current.GetCore());
        }


        private void SetResourceSvg(string name, string source, Dictionary<string, Drawing.Color> idColor, Drawing.Size size,
            string nameSpace = "Yuki_Theme.Core.WPF.Resources.SVG", Assembly asm = null)
        {
            if (asm == null)
                asm = Assembly.GetExecutingAssembly();
            Resources[name] = WPFHelper.GetSvg(source, idColor, true, size, nameSpace, asm);
        }

        private void SetColors(ThemeField field)
        {
            Color bgdef = ColorTranslator.FromHtml(field.Background);
            Color bg = Helper.DarkerOrLighter(bgdef, 0.05f);
            Color active = Helper.DarkerOrLighter(bgdef, 0.25f);

            ThemeName.Background = ConvertToBrush(bg, 0.95);

            ActiveTab.Background = ConvertToBrush(active, 0.95);

            BottomStrip.Background = Strip.Background = ConvertToBrush(bgdef, 0.95);

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
            return new SolidColorBrush(color.ToWPFColor()) { Opacity = opacity };
        }

        private void StartDownloading(object sender, RoutedEventArgs e)
        {
            DownloaderWindow.StartDownloading(ThemeName.Text);
        }
    }
}