using System.Windows.Forms;

namespace YukiTheme.Tools;

public static class StickerPositionConverter
{
	public static string ToString(StickerPosition position)
	{
		return $"{position.X}|{position.Y}|{(int)position.Align}";
	}

	public static StickerPosition FromString(string data)
	{
		if (data.Length < 4)
		{
			return new StickerPosition(5, 5, AnchorStyles.Bottom | AnchorStyles.Right);
		}

		string[] cc = data.Split('|');
		float x = float.Parse(cc[0]);
		float y = float.Parse(cc[1]);
		AnchorStyles align = (AnchorStyles)int.Parse(cc[2]);

		return new StickerPosition(x, y, align);
	}
}