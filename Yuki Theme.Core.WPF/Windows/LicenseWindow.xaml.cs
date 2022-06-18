using System.Windows;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class LicenseWindow : Window
	{
		public LicenseWindow ()
		{
			InitializeComponent ();
		}

		private void LicenseWindow_OnLoaded (object sender, RoutedEventArgs e)
		{
			License.LoadSvg();
			License.ShowLicense ();
			TranslateControls ();
		}

		private void TranslateControls ()
		{
			WPFHelper.TranslateControls (this, "messages.");
		}
	}
}