using System.Windows.Forms;

namespace YukiTheme.Tools;

public class StickerPosition
{
	public float X;
	public float Y;
	public AnchorStyles Align;

	public StickerPosition(float x, float y, AnchorStyles align)
	{
		X = x;
		Y = y;
		Align = align;
	}
}