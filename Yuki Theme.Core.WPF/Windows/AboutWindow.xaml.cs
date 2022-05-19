using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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
		private Duration duration        = new Duration (TimeSpan.FromSeconds (0.5));

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
			
			Assembly a = CLI.GetCore ();
			Stream stm = a.GetManifestResourceStream ($"Yuki_Theme.Core.Resources.LICENSE");
			string description = "";
			using (StreamReader reader = new StreamReader (stm))
			{
				description = reader.ReadToEnd ();
			}

			LicenseBox.Text = description;
		}

		private DoubleAnimation AnimateWindow (bool positive)
		{
			DoubleAnimation widthAnimation = new DoubleAnimation (this.Width + (positive ? 300 : -300), duration);
			widthAnimation.AccelerationRatio = 0.4; widthAnimation.DecelerationRatio = 0.6;
			return widthAnimation;
		}

		private DoubleAnimation GenerateAnimation (bool opened, DockPanel target)
		{
			DoubleAnimation widthAnimation;
			if (!opened)
			{
				widthAnimation = new DoubleAnimation (0, 300, duration);
			} else
			{
				widthAnimation = new DoubleAnimation (300, 0, duration);
			}
			widthAnimation.AccelerationRatio = 0.4; widthAnimation.DecelerationRatio = 0.6;
			widthAnimation.Completed += (o, args) =>
			{
				animating = false;
				if (opened)
					target.Visibility = Visibility.Collapsed;
			};
			return widthAnimation;
		}
		
		private DoubleAnimation CalculatePosition (bool opened)
		{
			DoubleAnimation positionAnimation;

			if (!opened)
			{
				double left = Math.Max (-150, this.Left - 300);
				positionAnimation = new DoubleAnimation (this.Left, left, duration);
				positionAnimation.Completed += (sender, args) => {
				this.Left = left;
				}; 
			} else
			{
				double left = this.Left + 300;
				positionAnimation = new DoubleAnimation (this.Left, left, duration);
				positionAnimation.Completed += (sender, args) => {
				this.Left = left;
				}; 
			}
			positionAnimation.AccelerationRatio = 0.4; positionAnimation.DecelerationRatio = 0.6;
			return positionAnimation;
		}

		private void AddAnimation (Storyboard sb, DoubleAnimation animation, DependencyObject target, DependencyProperty property)
		{
			Storyboard.SetTarget(animation, target);
			Storyboard.SetTargetProperty(animation, new PropertyPath(property));
			sb.Children.Add(animation);
		}

		private void ToggleChangelog (object sender, RoutedEventArgs e)
		{
			if (!animating)
			{
				Storyboard sb = new Storyboard();
				if (!openedChangelog){
					ChangeLogPanel.Visibility = Visibility.Visible;
				}
				DoubleAnimation widthAnimation = GenerateAnimation (openedChangelog, ChangeLogPanel);
				AddAnimation (sb, widthAnimation, ChangeLogPanel,DockPanel.WidthProperty);
				
				DoubleAnimation animateWindow = AnimateWindow (!openedChangelog);
				AddAnimation (sb, animateWindow, this,Window.WidthProperty);
				
				DoubleAnimation positionAnimation = CalculatePosition (openedChangelog);
				AddAnimation (sb, positionAnimation, this,Window.LeftProperty);				
			
				openedChangelog = !openedChangelog;
				animating = true;
				sb.Begin();
			}
		}

		private void ToggleLicense (object sender, RoutedEventArgs e)
		{
			if (!animating)
			{
				Storyboard sb = new Storyboard();
				if (!openedLicense){
					LicensePanel.Visibility = Visibility.Visible;
				}
				DoubleAnimation widthAnimation = GenerateAnimation (openedLicense, LicensePanel);
				AddAnimation (sb, widthAnimation, LicensePanel,DockPanel.WidthProperty);
				DoubleAnimation animateWindow = AnimateWindow (!openedLicense);
				AddAnimation (sb, animateWindow, this,Window.WidthProperty);
				openedLicense = !openedLicense;
				animating = true;
				sb.Begin();
			}
		}
	}
}