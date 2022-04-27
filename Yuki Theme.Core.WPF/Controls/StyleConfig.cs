using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Brush = System.Windows.Media.Brush;

namespace Yuki_Theme.Core.WPF.Controls
{
	public class StyleConfig
	{
		public System.Windows.Media.Color BorderColor    { get; set; }
		public Brush BorderBrush    { get; set; }
		public System.Windows.Media.Color SelectionColor { get; set; }
		public Brush SelectionBrush { get; set; }
		
		public System.Windows.Media.Color KeywordColor { get; set; }
		public Brush KeywordBrush { get; set; }
		
		public Brush BackgroundClickBrush { get; set; }
	}

	public static class Extensions
	{
		public static BitmapImage ToWPFImage (this Image img)
		{
			BitmapImage bi = new BitmapImage ();
			using (var ms = new MemoryStream ())
			{
				img.Save (ms, System.Drawing.Imaging.ImageFormat.Png);
				// img.Dispose ();
				ms.Position = 0;
				
				bi.BeginInit ();
				bi.CacheOption = BitmapCacheOption.OnLoad;
				bi.StreamSource = ms;
				bi.EndInit ();
			}

			return bi;
		}
		
	}
}