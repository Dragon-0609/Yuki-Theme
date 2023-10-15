using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace YukiTheme.Tools
{
	public static class ImageHelper
	{
		

		public static Size CalculateDimension(Size value)
		{
			Size size = value;
			int capMax = 300;
			double aspect = 0;
			if (false) // Dimension available
			{
				if (true)
				{
					if (size.Height > capMax)
					{
						aspect = (size.Width + 0.0) / size.Height;
						size.Height = capMax;
						size.Width = Convert.ToInt32(Math.Round(aspect * capMax));
					}
				}
				else
				{
					if (size.Width > capMax)
					{
						aspect = (size.Height + 0.0) / size.Width;
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