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
		private bool     secondLicense   = false;
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
			UpdateTranslation ();
			versionNumber.Text = GenerateVersionText ();
			Logo.Source = Helper.GetYukiThemeIconImage (new Drawing.Size (Logo.Width.ToInt (), Logo.Height.ToInt ())).ToWPFImage ();
			LicensePanel.ShowLicense ();
			PrepareChangelog ();
			LoadSvg ();
		}

		private void PrepareChangelog ()
		{
			string htmlText = Helper.ReadNConvertMDToHTML ("CHANGELOG.md");
			htmlText = htmlText.Replace ("__expand__", "none");
			Browser.NavigateToString (htmlText);
		}

		private void LoadSvg ()
		{
			LicensePanel.LoadSvg();
			Icon = Helper.GetYukiThemeIconImage (new Drawing.Size (24, 24)).ToWPFImage ();
		}

		private static string GenerateVersionText ()
		{
			return $"{SettingsConst.CURRENT_VERSION.ToString ("0.0").Replace (',', '.')} {SettingsConst.CURRENT_VERSION_ADD}";
		}

		private DoubleAnimation AnimateWindow (bool positive)
		{
			DoubleAnimation widthAnimation = new DoubleAnimation (this.Width + (positive ? 300 : -300), duration);
			widthAnimation.AccelerationRatio = 0.4;
			widthAnimation.DecelerationRatio = 0.6;
			return widthAnimation;
		}

		private DoubleAnimation GenerateAnimation (bool opened, FrameworkElement target)
		{
			DoubleAnimation widthAnimation;
			if (!opened)
			{
				widthAnimation = new DoubleAnimation (0, 300, duration);
			} else
			{
				widthAnimation = new DoubleAnimation (300, 0, duration);
			}

			widthAnimation.AccelerationRatio = 0.4;
			widthAnimation.DecelerationRatio = 0.6;
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
			double windowLeft = PointToScreen (new Point (0, 0)).X;
			if (!opened)
			{
				double left = Math.Max (-150, windowLeft - 300);
				positionAnimation = new DoubleAnimation (windowLeft, left, duration);
				positionAnimation.Completed += (sender, args) =>
				{
					this.Left = left;
				};
			} else
			{
				double left = windowLeft + 300;
				positionAnimation = new DoubleAnimation (windowLeft, left, duration);
				positionAnimation.Completed += (sender, args) =>
				{
					this.Left = left;
				};
			}

			positionAnimation.AccelerationRatio = 0.4;
			positionAnimation.DecelerationRatio = 0.6;
			return positionAnimation;
		}

		private void AddAnimation (Storyboard sb, DoubleAnimation animation, DependencyObject target, DependencyProperty property)
		{
			Storyboard.SetTarget (animation, target);
			Storyboard.SetTargetProperty (animation, new PropertyPath (property));
			sb.Children.Add (animation);
		}

		private void ToggleChangelog (object sender, RoutedEventArgs e)
		{
			if (!animating)
			{
				Storyboard sb = new Storyboard ();
				if (!openedChangelog)
				{
					ChangeLogPanel.Visibility = Visibility.Visible;
				}

				DoubleAnimation panelWidthAnimation = GenerateAnimation (openedChangelog, ChangeLogPanel);
				AddAnimation (sb, panelWidthAnimation, ChangeLogPanel, DockPanel.WidthProperty);

				DoubleAnimation windowWidthAnimation = AnimateWindow (!openedChangelog);
				AddAnimation (sb, windowWidthAnimation, this, Window.WidthProperty);

				DoubleAnimation windowPositionAnimation = CalculatePosition (openedChangelog);
				AddAnimation (sb, windowPositionAnimation, this, Window.LeftProperty);

				openedChangelog = !openedChangelog;
				animating = true;
				sb.Begin ();
			}
		}

		private void ToggleLicense (object sender, RoutedEventArgs e)
		{
			if (!animating)
			{
				Storyboard sb = new Storyboard ();
				if (!openedLicense)
				{
					LicensePanel.Visibility = Visibility.Visible;
				}

				DoubleAnimation panelWidthAnimation = GenerateAnimation (openedLicense, LicensePanel);
				AddAnimation (sb, panelWidthAnimation, LicensePanel, DockPanel.WidthProperty);
				DoubleAnimation windowWidthAnimation = AnimateWindow (!openedLicense);
				AddAnimation (sb, windowWidthAnimation, this, Window.WidthProperty);
				openedLicense = !openedLicense;
				animating = true;
				sb.Begin ();
			}
		}

		private void UpdateTranslation ()
		{
			WPFHelper.TranslateControls (this, "about.", "messages.", "main.");
		}
		
	}
}