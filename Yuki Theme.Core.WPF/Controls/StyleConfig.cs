using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using Brush = System.Windows.Media.Brush;

namespace Yuki_Theme.Core.WPF.controls
{
	public class StyleConfig
	{
		public Brush BorderColor    { get; set; }
		public Brush SelectionColor { get; set; }
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