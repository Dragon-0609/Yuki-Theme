using System.Drawing;

namespace YukiTheme.Tools;

public static class ColorHelper
{
	#region Color Management

	public static Color ChangeColorBrightness(Color color, float correctionFactor)
	{
		float red = (float)color.R;
		float green = (float)color.G;
		float blue = (float)color.B;

		if (correctionFactor < 0)
		{
			correctionFactor = 1 + correctionFactor;
			red *= correctionFactor;
			green *= correctionFactor;
			blue *= correctionFactor;
		}
		else
		{
			red = (255 - red) * correctionFactor + red;
			green = (255 - green) * correctionFactor + green;
			blue = (255 - blue) * correctionFactor + blue;
		}

		return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
	}

	public static bool IsDark(Color clr)
	{
		bool dark = ((clr.R + clr.G + clr.B) / 3 < 127);
		return dark;
	}

	public static Color DarkerOrLighter(Color clr, float percent = 0)
	{
		if (IsDark(clr))
			return ChangeColorBrightness(clr, percent);
		else
			return ChangeColorBrightness(clr, -percent);
	}

	#endregion
}