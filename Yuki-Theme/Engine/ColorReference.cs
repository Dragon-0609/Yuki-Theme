using System.Drawing;

namespace YukiTheme.Engine;

public static class ColorReference
{
	public static Color ForegroundColor() => ColorChanger.Instance.GetColor(ColorChanger.FOREGROUND);
	public static Color ForegroundHoverColor() => ColorChanger.Instance.GetColor(ColorChanger.FG_HOVER);
	public static Color KeywordColor() => ColorChanger.Instance.GetColor(ColorChanger.KEYWORD);

	public static Color BackgroundColor() => ColorChanger.Instance.GetColor(ColorChanger.BG);
	public static Color BackgroundDefaultColor() => ColorChanger.Instance.GetColor(ColorChanger.BG_DEF);
	public static Color BackgroundClickColor() => ColorChanger.Instance.GetColor(ColorChanger.CLICK_BG);
	public static Color BackgroundClick2Color() => ColorChanger.Instance.GetColor(ColorChanger.CLICK_BG2);
	public static Color BackgroundClick3Color() => ColorChanger.Instance.GetColor(ColorChanger.CLICK_BG3);
	public static Color BackgroundInactiveColor() => ColorChanger.Instance.GetColor(ColorChanger.BG_INACTIVE);
	public static Color BorderColor() => ColorChanger.Instance.GetColor(ColorChanger.BORDER);
	public static Color SelectionColor() => ColorChanger.Instance.GetColor(ColorChanger.SELECTION);
	public static Color TypeColor() => ColorChanger.Instance.GetColor(ColorChanger.TYPE);

	public static readonly Color ChangedTab = Color.FromArgb(65, 229, 85);

	public static Brush ForegroundBrush() => ColorChanger.Instance.GetBrush(ColorChanger.FOREGROUND);

	public static Brush BackgroundBrush() => ColorChanger.Instance.GetBrush(ColorChanger.BG);
	public static Brush BackgroundDefaultBrush() => ColorChanger.Instance.GetBrush(ColorChanger.BG_DEF);
	public static Brush BackgroundClickBrush() => ColorChanger.Instance.GetBrush(ColorChanger.CLICK_BG);
	public static Brush BackgroundClick3Brush() => ColorChanger.Instance.GetBrush(ColorChanger.CLICK_BG3);
	public static Brush BackgroundInactiveBrush() => ColorChanger.Instance.GetBrush(ColorChanger.BG_INACTIVE);
	public static Brush SelectionBrush() => ColorChanger.Instance.GetBrush(ColorChanger.SELECTION);
	public static Brush TypeBrush() => ColorChanger.Instance.GetBrush(ColorChanger.TYPE);


	public static Pen ForegroundPen() => ColorChanger.Instance.GetPen(ColorChanger.FOREGROUND);
	public static Pen BackgroundClick3Pen() => ColorChanger.Instance.GetPen(ColorChanger.CLICK_BG3);
	public static Pen BorderPen() => ColorChanger.Instance.GetPen(ColorChanger.BORDER);
	public static Pen BorderThickPen() => ColorChanger.Instance.GetPen(ColorChanger.BORDER_THICK);
}