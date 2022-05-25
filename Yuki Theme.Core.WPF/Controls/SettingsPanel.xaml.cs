using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Yuki_Theme.Core.WPF.Windows;
using Drawing = System.Drawing;
namespace Yuki_Theme.Core.WPF.Controls
{
	public partial class SettingsPanel : UserControl
	{
		private Drawing.Size defaultSmallSize = new Drawing.Size (16, 16);

		public Window ParentWindow = null;
		
		public SettingsPanel ()
		{
			InitializeComponent ();
			
			LoadSVG ();
		}
		
		
		private void LoadSVG()
		{
			SetResourceSvg ("InfoImage", "balloonInformation", null, defaultSmallSize);
			SetResourceSvg ("HelpImage", "help", null, defaultSmallSize, "Yuki_Theme.Core.Resources.SVG", CLI.GetCore ());
		}
		
		private void SetResourceSvg (string name, string source, Dictionary <string, Drawing.Color> idColor, Drawing.Size size, string nameSpace = "Yuki_Theme.Core.WPF.Resources.SVG", Assembly asm = null)
		{
			if (asm == null)
				asm = Assembly.GetExecutingAssembly ();
			this.Resources [name] = WPFHelper.GetSvg (source, idColor, false, size, nameSpace, asm);
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
			EditorSettingsPanel.Visibility = Settings.Editor ? Visibility.Visible : Visibility.Collapsed;
		}

		private void AboutButton_OnClick (object sender, RoutedEventArgs e)
		{
			AboutWindow aboutWindow = new AboutWindow
			{
				Background = WPFHelper.bgBrush,
				Foreground = WPFHelper.fgBrush,
				Tag = Tag
			};

			if (ParentWindow != null) aboutWindow.Owner = ParentWindow;

			aboutWindow.ShowDialog ();
		}

		private void EditorModeCheckChanged (object sender, RoutedEventArgs e)
		{
			EditorSettingsPanel.Visibility = EditorMode.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
		}

		private void AskOthersCheckedChanged (object sender, RoutedEventArgs e)
		{
			DoActionPanel.Visibility = AskOthers.IsChecked == true ? Visibility.Collapsed : Visibility.Visible;
		}
	}
}