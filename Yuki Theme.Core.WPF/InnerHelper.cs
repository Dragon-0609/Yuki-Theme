using SDColor = System.Drawing.Color;
using SWMColor = System.Windows.Media.Color;

namespace Yuki_Theme.Core.WPF
{
	public static class InnerHelper
	{
		public static SWMColor ToWPFColor (this     SDColor  color) => SWMColor.FromArgb (color.A, color.R, color.G, color.B);
		public static SDColor  ToWinformsColor(this SWMColor color) => SDColor.FromArgb(color.A, color.R, color.G, color.B);
	}
}