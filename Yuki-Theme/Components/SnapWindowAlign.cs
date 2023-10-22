using System.Windows.Media;

namespace YukiTheme.Components;

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