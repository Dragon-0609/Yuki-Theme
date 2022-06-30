using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Yuki_Theme.Core.WPF
{
	public static class Converter
	{
		public static Color ToWPFColor (this System.Drawing.Color color) => Color.FromArgb (color.A, color.R, color.G, color.B);

		public static System.Drawing.Color ToWinformsColor (this Color color) =>
			System.Drawing.Color.FromArgb (color.A, color.R, color.G, color.B);

		public static string ToHex (this System.Drawing.Color color)
		{
			return System.Drawing.ColorTranslator.ToHtml (color);
		}

		public static string ToHex (this Color color)
		{
			return color.ToWinformsColor ().ToHex ();
		}
		
		public static Brush ToBrush (this Color color) => new SolidColorBrush (color);

		public static BitmapImage ToWPFImage (this System.Drawing.Image img)
		{
			BitmapImage bi = new BitmapImage ();
			using (var ms = new MemoryStream ())
			{
				img.Save (ms, ImageFormat.Png);
				// img.Dispose ();
				ms.Position = 0;

				bi.BeginInit ();
				bi.CacheOption = BitmapCacheOption.OnLoad;
				bi.StreamSource = ms;
				bi.EndInit ();
			}

			return bi;
		}

		public static Point ToWPFPoint (this System.Drawing.Point value)
		{
			return new Point (value.X, value.Y);
		}

		public static System.Drawing.Point ToDrawingPoint (this Point value)
		{
			return new System.Drawing.Point ((int)value.X, (int)value.Y);
		}

		public static int ToInt (this double d)
		{
			return Convert.ToInt32 (d);
		}
	}
}