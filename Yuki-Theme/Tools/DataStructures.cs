using System.Windows.Media;

namespace YukiTheme.Tools;

public class SnapWindowAlign
{
	internal AlignmentX AlignX;
	internal AlignmentY AlignY;

	public SnapWindowAlign(AlignmentX x, AlignmentY y)
	{
		AlignX = x;
		AlignY = y;
	}
}