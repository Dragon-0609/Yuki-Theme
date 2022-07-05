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
			using (MemoryStream ms = new MemoryStream ())
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

		public static BitmapImage ToWPFGIFImage (this System.Drawing.Image img)
		{
			BitmapImage bi = new BitmapImage ();
			MemoryStream ms = new MemoryStream ();
			img.Save (ms, ImageFormat.Gif);
			// img.Dispose ();
			ms.Position = 0;

			bi.BeginInit ();
			bi.CacheOption = BitmapCacheOption.OnLoad;
			bi.StreamSource = ms;
			bi.EndInit ();

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

		public static AlignmentX ConvertToX (this AnchorStyles anchorStyles)
		{
			if (anchorStyles.HasFlag (AnchorStyles.Left))
				return AlignmentX.Left;
			if (anchorStyles.HasFlag (AnchorStyles.Right))
				return AlignmentX.Right;
			return AlignmentX.Center;
		}

		public static AlignmentY ConvertToY (this AnchorStyles anchorStyles)
		{
			if (anchorStyles.HasFlag (AnchorStyles.Top))
				return AlignmentY.Top;
			if (anchorStyles.HasFlag (AnchorStyles.Bottom))
				return AlignmentY.Bottom;
			return AlignmentY.Center;
		}
	}
}