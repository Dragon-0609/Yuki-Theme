using System.Collections.Generic;
using System.Drawing;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.Utils;
using Yuki_Theme.Core.WPF.Controls;
using Brush = System.Windows.Media.Brush;

namespace Yuki_Theme.Core.WPF
{

    public static class WPFColorConverter
    {
        public static StyleConfig Convert(Theme theme)
        {
            Color ForeColor = default;
            Color bgdefColor = default;
            Color bgColor = default;
            Color fgColor = default;
            Color bgClick = default;
            Color fgHover = default;
            Color bgBorder = default;
            Color selectionColor = default;
            Color fgKeyword = default;
            bool isLight = Settings.settingMode == SettingMode.Light;
            foreach (KeyValuePair<string, ThemeField> style in theme.Fields)
            {
                if (!HighlitherUtil.IsInColors(style.Key))
                {
                    switch (style.Key)
                    {
                        case "Default":
                        case "Default Text":
                        {
                            ForeColor = Parse(style.Value.Foreground);
                            bgdefColor = Parse(style.Value.Background);
                            bgColor = Helper.DarkerOrLighter(bgdefColor, 0.05f);
                            fgColor = Helper.DarkerOrLighter(ForeColor, 0.2f);
                            bgClick = Helper.DarkerOrLighter(bgdefColor, 0.25f);
                            fgHover = Helper.DarkerOrLighter(ForeColor, 0.4f);
                        }
                            break;
                        case "Selection":
                        {
                            ColorKeeper.selectionColor = Parse(style.Value.Background);
                        }
                            break;
                        case "CaretMarker":
                        case "Caret":
                        {
                            bgBorder = Parse(style.Value.Foreground);
                        }
                            break;
                    }
                }
                else
                {
                    string[] key = new[] { style.Key.ToLower() };
                    if (isLight)
                        key = ShadowNames.PascalFields[style.Key];
                    foreach (string ki in key)
                    {
                        string kilow = ki.ToLower();
                        if (kilow is "keywords" or "keyword")
                        {
                            fgKeyword = Parse(style.Value.Foreground);
                        }
                    }
                }
            }


            System.Windows.Media.Color wpfbgColor = bgColor.ToWPFColor();
            System.Windows.Media.Color wpfbgdefColor = bgdefColor.ToWPFColor();
            System.Windows.Media.Color wpfbgClickColor = bgClick.ToWPFColor();
            System.Windows.Media.Color wpffgColor = fgColor.ToWPFColor();
            System.Windows.Media.Color wpfborderColor = bgBorder.ToWPFColor();
            System.Windows.Media.Color wpfselectionColor = selectionColor.ToWPFColor();
            System.Windows.Media.Color wpfkeywordColor = fgKeyword.ToWPFColor();
            Brush wpfbgBrush = wpfbgColor.ToBrush();
            Brush wpfbgdefBrush = wpfbgdefColor.ToBrush();
            Brush wpfbgClickBrush = wpfbgClickColor.ToBrush();
            Brush wpffgBrush = wpffgColor.ToBrush();
            Brush wpfborderBrush = wpfborderColor.ToBrush();
            Brush wpfselectionBrush = wpfselectionColor.ToBrush();
            Brush wpfkeywordBrush = wpfkeywordColor.ToBrush();
            StyleConfig colors = new StyleConfig()
            {
                BorderColor = wpfborderColor,
                BorderBrush = wpfborderBrush,
                SelectionColor = wpfselectionColor,
                SelectionBrush = wpfselectionBrush,
                KeywordColor = wpfkeywordColor,
                KeywordBrush = wpfkeywordBrush,
                BackgroundClickColor = wpfbgClickColor,
                BackgroundClickBrush = wpfbgClickBrush,
                BackgroundDefaultColor = wpfbgdefColor,
                BackgroundDefaultBrush = wpfbgdefBrush,
            };
            return colors;
        }

        private static Color Parse(string str)
        {
            return ColorTranslator.FromHtml(str);
        }
    }
}