using System.Windows.Media;
using YukiTheme.Engine;

namespace YukiTheme.Tools;

public class WpfColorContainer
{
	public static WpfColorContainer Instance;

	public WpfColorContainer()
	{
		Instance = this;
	}

	public Color BackgroundColor { get; set; }
	public Color BackgroundDefaultColor { get; set; }
	public Color BackgroundClickColor { get; set; }
	public Color ForegroundColor { get; set; }
	public Color BorderColor { get; set; }
	public Color SelectionColor { get; set; }
	public Color KeywordColor { get; set; }
	public Brush BackgroundBrush { get; set; }
	public Brush BackgroundDefaultBrush { get; set; }
	public Brush BackgroundClickBrush { get; set; }
	public Brush ForegroundBrush { get; set; }
	public Brush BorderBrush { get; set; }
	public Brush SelectionBrush { get; set; }
	public Brush KeywordBrush { get; set; }

	public void GetColorsAndBrushes()
	{
		BackgroundColor = ColorReference.BackgroundColor.ToWPFColor();
		BackgroundDefaultColor = ColorReference.BackgroundDefaultColor.ToWPFColor();
		BackgroundClickColor = ColorReference.BackgroundClickColor.ToWPFColor();
		ForegroundColor = ColorReference.ForegroundColor.ToWPFColor();
		BorderColor = ColorReference.BorderColor.ToWPFColor();
		SelectionColor = ColorReference.SelectionColor.ToWPFColor();
		KeywordColor = ColorReference.KeywordColor.ToWPFColor();
		BackgroundBrush = BackgroundColor.ToBrush();
		BackgroundDefaultBrush = BackgroundDefaultColor.ToBrush();
		BackgroundClickBrush = BackgroundClickColor.ToBrush();
		ForegroundBrush = ForegroundColor.ToBrush();
		BorderBrush = BorderColor.ToBrush();
		SelectionBrush = SelectionColor.ToBrush();
		KeywordBrush = KeywordColor.ToBrush();
	}
}