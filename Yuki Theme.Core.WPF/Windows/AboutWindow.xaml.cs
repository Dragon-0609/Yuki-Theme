using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Drawing = System.Drawing;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class AboutWindow : Window
	{
		private bool     openedLicense   = false;
		private bool     openedChangelog = false;
		private bool     animating       = false;
		private Duration duration        = new Duration (TimeSpan.FromSeconds (1));

		public AboutWindow ()
		{
			InitializeComponent ();
		}

		private void OpenLink (object sender, RequestNavigateEventArgs e)
		{
			Process.Start (e.Uri.AbsoluteUri);
		}

		private void AboutWindow_OnLoaded (object sender, RoutedEventArgs e)
		{
			Version.Text =
				$"{CLI.Translate ("about.version")}: {Settings.current_version.ToString ("0.0").Replace (',', '.')} {Settings.current_version_add}";
			Logo.Source = Helper.GetYukiThemeIconImage (new Drawing.Size (Logo.Width.ToInt (), Logo.Height.ToInt ())).ToWPFImage ();
		}

		private DoubleAnimation AnimateWindow (bool positive)
		{
			DoubleAnimation widthAnimation = new DoubleAnimation (this.Width + (positive ? 300 : -300), duration);
			return widthAnimation;
		}

		private DoubleAnimation GenerateAnimation (bool opened)
		{
			DoubleAnimation widthAnimation;
			if (!opened)
			{
				widthAnimation = new DoubleAnimation (300, duration)
				{
					From = 0
				};
			} else
			{
				widthAnimation = new DoubleAnimation (0, duration)
				{
					From = 300
				};
			}

			widthAnimation.Completed += (o, args) =>
			{
				animating = false;
			};
			return widthAnimation;
		}
		
		private DoubleAnimation CalculatePosition (bool opened)
		{
			DoubleAnimation positionAnimation;

			if (!opened)
			{
				double left = Math.Max (-150, this.Left - 300);
				positionAnimation = new DoubleAnimation (left, duration);
				positionAnimation.Completed += (sender, args) => {
				this.Left = left;
				}; 
			} else
			{
				double left = this.Left + 300;
				positionAnimation = new DoubleAnimation (left, duration);
				positionAnimation.Completed += (sender, args) => {
				this.Left = left;
				}; 
			}

			return positionAnimation;
		}

		private void ToggleChangelog (object sender, RoutedEventArgs e)
		{
			if (!animating)
			{
				DoubleAnimation widthAnimation = GenerateAnimation (openedChangelog);
				DoubleAnimation animateWindow = AnimateWindow (!openedChangelog);
				DoubleAnimation positionAnimation = CalculatePosition (openedChangelog);
				openedChangelog = !openedChangelog;
				animating = true;
				if (positionAnimation != null)
					BeginAnimation (LeftProperty, positionAnimation);
				BeginAnimation (WidthProperty, animateWindow);
				ChangeLog.BeginAnimation (WidthProperty, widthAnimation);
			}
		}

		private void ToggleLicense (object sender, RoutedEventArgs e)
		{
			if (!animating)
			{
				DoubleAnimation widthAnimation = GenerateAnimation (openedLicense);
				DoubleAnimation animateWindow = AnimateWindow (!openedLicense);
				openedLicense = !openedLicense;
				animating = true;
				BeginAnimation (WidthProperty, animateWindow);
				License.BeginAnimation (WidthProperty, widthAnimation);
			}
		}
	}
}