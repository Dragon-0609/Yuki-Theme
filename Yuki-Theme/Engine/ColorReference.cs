using System.Drawing;

namespace YukiTheme.Engine;

public static class ColorReference
{
	public static Color ForegroundColor() => ColorChanger.Changer.GetColor(ColorChanger.Foreground);
	public static Color ForegroundHoverColor() => ColorChanger.Changer.GetColor(ColorChanger.FgHover);
	public static Color KeywordColor() => ColorChanger.Changer.GetColor(ColorChanger.Keyword);

	public static Color BackgroundColor() => ColorChanger.Changer.GetColor(ColorChanger.Bg);
	public static Color BackgroundDefaultColor() => ColorChanger.Changer.GetColor(ColorChanger.BgDef);
	public static Color BackgroundClickColor() => ColorChanger.Changer.GetColor(ColorChanger.ClickBg);
	public static Color BackgroundClick2Color() => ColorChanger.Changer.GetColor(ColorChanger.ClickBg2);
	public static Color BackgroundClick3Color() => ColorChanger.Changer.GetColor(ColorChanger.ClickBg3);
	public static Color BackgroundInactiveColor() => ColorChanger.Changer.GetColor(ColorChanger.BgInactive);
	public static Color BorderColor() => ColorChanger.Changer.GetColor(ColorChanger.Border);
	public static Color SelectionColor() => ColorChanger.Changer.GetColor(ColorChanger.Selection);
	public static Color TypeColor() => ColorChanger.Changer.GetColor(ColorChanger.Type);

	public static readonly Color ChangedTab = Color.FromArgb(65, 229, 85);

	public static Brush ForegroundBrush() => ColorChanger.Changer.GetBrush(ColorChanger.Foreground);

	public static Brush BackgroundBrush() => ColorChanger.Changer.GetBrush(ColorChanger.Bg);
	public static Brush BackgroundDefaultBrush() => ColorChanger.Changer.GetBrush(ColorChanger.BgDef);
	public static Brush BackgroundClickBrush() => ColorChanger.Changer.GetBrush(ColorChanger.ClickBg);
	public static Brush BackgroundClick3Brush() => ColorChanger.Changer.GetBrush(ColorChanger.ClickBg3);
	public static Brush BackgroundInactiveBrush() => ColorChanger.Changer.GetBrush(ColorChanger.BgInactive);
	public static Brush SelectionBrush() => ColorChanger.Changer.GetBrush(ColorChanger.Selection);
	public static Brush TypeBrush() => ColorChanger.Changer.GetBrush(ColorChanger.Type);


	public static Pen ForegroundPen() => ColorChanger.Changer.GetPen(ColorChanger.Foreground);
	public static Pen BackgroundClick3Pen() => ColorChanger.Changer.GetPen(ColorChanger.ClickBg3);
	public static Pen BorderPen() => ColorChanger.Changer.GetPen(ColorChanger.Border);
	public static Pen BorderThickPen() => ColorChanger.Changer.GetPen(ColorChanger.BorderThick);
}