using System;
using System.Drawing;

namespace YukiTheme.Tools
{
	public static class ImageHelper
	{
		public static Size CalculateDimension(Size value)
		{
			Size size = value;
			int capMax = DatabaseManager.Load(SettingsConst.DIMENSION_CAP_MAX, 300);
			double aspect = 0;
			if (DatabaseManager.Load(SettingsConst.USE_DIMENSION_CAP))
			{
				if (DatabaseManager.Load(SettingsConst.DIMENSION_CAP_UNIT, 0) == 0)
				{
					if (size.Height > capMax)
					{
						aspect = (double)size.Width / size.Height;
						size.Height = capMax;
						size.Width = Convert.ToInt32(Math.Round(aspect * capMax));
					}
				}
				else
				{
					if (size.Width > capMax)
					{
						aspect = (double)size.Height / size.Width;
						size.Width = capMax;
						size.Height = Convert.ToInt32(Math.Round(aspect * capMax));
					}
				}
			}

			// MessageBox.Show ( $"Dimension: {Settings.dimensionCapUnit}, max: {capMax}, aspect: {aspect}, Size: {size}, Original: {value}");
			return size;
		}
	}
}