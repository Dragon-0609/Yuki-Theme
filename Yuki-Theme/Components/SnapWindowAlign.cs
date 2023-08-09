using System.Windows.Media;

namespace YukiTheme.Components;

public class SnapWindowAlign
{
    public AlignmentX AlignX;
    public AlignmentY AlignY;

    public SnapWindowAlign(AlignmentX x, AlignmentY y)
    {
        AlignX = x;
        AlignY = y;
    }
}