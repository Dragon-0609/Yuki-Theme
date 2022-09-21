using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Yuki_Theme.Core.WPF.Controls
{
	public partial class InfoLabel : UserControl
	{
		
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register (
			"Text", typeof (string), typeof (InfoLabel));


		public string Text
		{
			get => (string) GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}
		
		public static readonly DependencyProperty ImageProperty = DependencyProperty.Register (
			"Image", typeof (ImageSource), typeof (InfoLabel));


		public ImageSource Image
		{
			get => (ImageSource) GetValue(ImageProperty);
			set => SetValue(ImageProperty, value);
		}

		public InfoLabel()
		{
			InitializeComponent();
		}
	}
}