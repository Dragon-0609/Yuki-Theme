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
		
		public System.Windows.Media.Color BackgroundClickColor { get; set; }
		
		public Brush BackgroundClickBrush { get; set; }
		
		public System.Windows.Media.Color BackgroundDefaultColor { get; set; }
		
		public Brush BackgroundDefaultBrush { get; set; }
	}
}