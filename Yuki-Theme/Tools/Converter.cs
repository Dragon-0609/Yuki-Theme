using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace YukiTheme.Tools
{
	public static class Converter
	{
		public static Color ToWPFColor(this System.Drawing.Color color) => Color.FromArgb(color.A, color.R, color.G, color.B);

		public static System.Drawing.Color ToWinformsColor(this Color color) =>
			System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);

		public static string ToHex(this System.Drawing.Color color)
		{
			return System.Drawing.ColorTranslator.ToHtml(color);
		}

		public static string ToHex(this Color color)
		{
			return color.ToWinformsColor().ToHex();
		}

		public static Brush ToBrush(this Color color) => new SolidColorBrush(color);

		public static BitmapImage ToWPFImage(this System.Drawing.Image img)
		{
			BitmapImage bi = new BitmapImage();
			using (MemoryStream ms = new MemoryStream())
			{
				img.Save(ms, ImageFormat.Png);
				// img.Dispose ();
				ms.Position = 0;

				bi.BeginInit();
				bi.CacheOption = BitmapCacheOption.OnLoad;
				bi.StreamSource = ms;
				bi.EndInit();
			}

			return bi;
		}

		public static BitmapImage ToWPFGIFImage(this System.Drawing.Image img)
		{
			BitmapImage bi = new BitmapImage();
			MemoryStream ms = new MemoryStream();
			img.Save(ms, ImageFormat.Gif);
			// img.Dispose ();
			ms.Position = 0;

			bi.BeginInit();
			bi.CacheOption = BitmapCacheOption.OnLoad;
			bi.StreamSource = ms;
			bi.EndInit();

			return bi;
		}

		public static Point ToWPFPoint(this System.Drawing.Point value)
		{
			return new Point(value.X, value.Y);
		}

		public static System.Drawing.Point ToDrawingPoint(this Point value)
		{
			return new System.Drawing.Point((int)value.X, (int)value.Y);
		}

		public static int ToInt(this double d)
		{
			return Convert.ToInt32(d);
		}

		public static AlignmentX ConvertToX(this AnchorStyles anchorStyles)
		{
			if (anchorStyles.HasFlag(AnchorStyles.Left))
				return AlignmentX.Left;
			if (anchorStyles.HasFlag(AnchorStyles.Right))
				return AlignmentX.Right;
			return AlignmentX.Center;
		}

		public static AlignmentY ConvertToY(this AnchorStyles anchorStyles)
		{
			if (anchorStyles.HasFlag(AnchorStyles.Top))
				return AlignmentY.Top;
			if (anchorStyles.HasFlag(AnchorStyles.Bottom))
				return AlignmentY.Bottom;
			return AlignmentY.Center;
		}

		public static AnchorStyles ConvertToAlign(this AlignmentX align)
		{
			if (align == AlignmentX.Left)
				return AnchorStyles.Left;
			if (align == AlignmentX.Right)
				return AnchorStyles.Right;
			return AnchorStyles.None;
		}

		public static AnchorStyles ConvertToAlign(this AlignmentY align)
		{
			if (align == AlignmentY.Top)
				return AnchorStyles.Top;
			if (align == AlignmentY.Bottom)
				return AnchorStyles.Bottom;
			return AnchorStyles.None;
		}

		public static Rect ToRect(this Rectangle rectangle)
		{
			return new Rect(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		public static Rect ToRect(this Rectangle rectangle, Form form)
		{
			return new Rect(form.WindowState == FormWindowState.Maximized ? rectangle.Left : form.Left + 2,
				form.WindowState == FormWindowState.Maximized ? rectangle.Top : form.Top, rectangle.Width, rectangle.Height);
		}
	}
}