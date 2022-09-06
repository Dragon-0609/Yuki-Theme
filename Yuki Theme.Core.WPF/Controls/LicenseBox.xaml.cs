using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Yuki_Theme.Core.Utils;
using Drawing = System.Drawing;

namespace Yuki_Theme.Core.WPF.Controls
{
	public partial class LicenseBox : UserControl
	{
		private bool secondLicense = false;
		
		public LicenseBox ()
		{
			InitializeComponent ();
		}

		internal void LoadSvg ()
		{
			string add = Helper.IsDark (ColorKeeper.bgColor) ? "" : "_dark";
			Drawing.Size size = new Drawing.Size (24, 24);
			Assembly assm = Assembly.GetExecutingAssembly ();
			BackImage.Source = WPFHelper.GetSvg ("back" + add,null, false, size, "Yuki_Theme.Core.WPF.Resources.SVG", assm);
			ForwardImage.Source = WPFHelper.GetSvg ("forward" + add,null, false, size, "Yuki_Theme.Core.WPF.Resources.SVG", assm);
		}
		
		private void BackButton_OnClick (object sender, RoutedEventArgs e)
		{
			if (secondLicense)
			{
				secondLicense = !secondLicense;
				ShowLicense();
			}
		}

		private void ForwardButton_OnClick (object sender, RoutedEventArgs e)
		{
			if (!secondLicense)
			{
				secondLicense = !secondLicense;
				ShowLicense();
			}
		}


		internal void ShowLicense ()
		{
			string licensePath = secondLicense ? "JetBrainsLICENSE" : "LICENSE";
			string licenseHeader = secondLicense ? "JetBrains" : "Dragon-LV";
			LicenseTextBox.Text = Helper.ReadResource (licensePath);
			LicenseOwner.Text = licenseHeader;
			DisableLicenseButtons ();
		}
		
		
		private void DisableLicenseButtons ()
		{
			BackButton.IsEnabled = secondLicense;
			ForwardButton.IsEnabled = !secondLicense;
		}

	}
}