using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.WPF.Controls;
using Yuki_Theme.Core.WPF.Windows;

namespace Yuki_Theme.Core.WPF
{
	public static class AdditionalTools
	{

		public static void ShowLicense (object Tag, Window owner, Form form = null)
		{
			if (!Settings.license)
			{
				LicenseWindow licenseWindow = new LicenseWindow ()
				{
					Background = WPFHelper.bgBrush,
					Foreground = WPFHelper.fgBrush,
					BorderBrush = WPFHelper.borderBrush,
					Tag = Tag
				};
				if (owner != null)
					licenseWindow.Owner = owner;
				else
				{
					WindowInteropHelper helper = new WindowInteropHelper (licenseWindow);
					helper.Owner = form.Handle;
				}
				licenseWindow.Closed += (a, b) =>
				{
					GC.Collect ();
					GC.WaitForPendingFinalizers ();
				};

				licenseWindow.ShowDialog ();
				
				Settings.license = true;
				Settings.database.UpdateData (SettingsConst.LICENSE, "True");
			}
		}
		
		public static void TrackInstall ()
		{
			if (!Settings.Logged && !Settings.dontTrack)
			{
				HttpResponseMessage result = GoogleAnalyticsHelper.TrackEvent ().Result;
				if (!result.IsSuccessStatusCode)
				{
					// Maybe internet isn't available
				} else
				{
					Settings.database.UpdateData (SettingsConst.LOGIN, "true");
					Settings.Logged = true;
				}
			}
		}

	}
}