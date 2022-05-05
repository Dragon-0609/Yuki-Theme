using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Yuki_Theme.Core.WPF.Controls;
using SDColor = System.Drawing.Color;
using SWMColor = System.Windows.Media.Color;
using Drawing = System.Drawing;
using WBrush = System.Windows.Media.Brush;

namespace Yuki_Theme.Core.WPF
{
	public static class WPFHelper
	{
		public static SWMColor ToWPFColor (this     SDColor  color) => SWMColor.FromArgb (color.A, color.R, color.G, color.B);
		public static SDColor  ToWinformsColor(this SWMColor color) => SDColor.FromArgb(color.A, color.R, color.G, color.B);

		public static WBrush ToBrush (this SWMColor color) => new SolidColorBrush (color);
		
		public static void SetSVGImage (Button btn, string source, bool customColor = false, Drawing.Color color = default)
		{
			btn.Content = new Image ()
			{
				Source = (Helper.RenderSvg (new Drawing.Size (System.Convert.ToInt32 (btn.Width), System.Convert.ToInt32 (btn.Height)),
				                            Helper.LoadSvg (source, CLI.GetCore ()), false, Drawing.Size.Empty, customColor, color))
					.ToWPFImage ()
			};
		}
		
		public static BitmapImage ToWPFImage (this Drawing.Image img)
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