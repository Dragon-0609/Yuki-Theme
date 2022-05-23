using System.Collections.Generic; 
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Drawing = System.Drawing;
namespace Yuki_Theme.Core.WPF.Controls
{
	public partial class SettingsPanel : UserControl
	{
		private Drawing.Size defaultSmallSize = new Drawing.Size (16, 16);
		
		public SettingsPanel ()
		{
			InitializeComponent ();
			
			LoadSVG ();
		}
		
		
		private void LoadSVG()
		{

			SetResourceSvg ("InfoImage", "balloonInformation", null, defaultSmallSize);
		}
		
		private void SetResourceSvg (string name, string source, Dictionary <string, Drawing.Color> idColor, Drawing.Size size)
		{
			MainGrid.Resources [name] = WPFHelper.GetSvg (source, idColor, size);
		}
		
		
		public Thickness InnerMargin
		{
			get { return (Thickness)GetValue (InnerMarginProperty); }
			set { SetValue (InnerMarginProperty, value); }
		}

		public static readonly DependencyProperty InnerMarginProperty =
			DependencyProperty.Register ("InnerMargin", typeof (Thickness), typeof (SettingsPanel),
			                             new PropertyMetadata (new Thickness (0)));

		private void SettingsPanel_OnLoaded (object sender, RoutedEventArgs e)
		{
		}
	}
}