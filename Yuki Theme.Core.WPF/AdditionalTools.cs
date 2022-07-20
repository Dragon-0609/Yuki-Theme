using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.WPF.Controls;
using Yuki_Theme.Core.WPF.Windows;
using HorizontalAlignment = System.Windows.HorizontalAlignment;

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
		
		public static void TrackInstall (Window parent)
		{
			if (ShouldTrack ())
			{
				QuestionWindow questionWindow = new QuestionWindow ()
				{
					Background = WPFHelper.bgBrush,
					Foreground = WPFHelper.fgBrush,
					Tag = parent.Tag
				};

				questionWindow.SetOwner (parent);

				SetContents (questionWindow);

				Ask (questionWindow);
			}
		}

		public static void TrackInstall (Form parent)
		{
			if (ShouldTrack ())
			{
				QuestionWindow questionWindow = new QuestionWindow ()
				{
					Background = WPFHelper.bgBrush,
					Foreground = WPFHelper.fgBrush,
					Tag = WPFHelper.GenerateTag
				};

				questionWindow.SetOwner (parent);

				SetContents (questionWindow);
				
				Ask (questionWindow);
			}
		}

		private static void SetContents (QuestionWindow questionWindow)
		{
			questionWindow.SetTitle (API.Translate ("messages.google.analytics"));

			questionWindow.SetButtonContents (API.Translate ("messages.buttons.decline"), API.Translate ("messages.buttons.accept"), "");

			questionWindow.SetButtonVisibilities (true, true, false);
			questionWindow.SetButtonAlign (HorizontalAlignment.Center);
		}

		private static void Ask (QuestionWindow questionWindow)
		{
			int shouldTrack = 0;

			if (questionWindow.ShowDialog () == true)
			{
				shouldTrack = questionWindow.result;
			}

			if (shouldTrack == 1)
				Track ();
			else
				DontTrack ();
		}


		public static bool ShouldTrack ()
		{
			return !Settings.Logged && !Settings.dontTrack;
		}

		private static void Track ()
		{
			HttpResponseMessage result = GoogleAnalyticsHelper.TrackEvent ().Result;
			if (!result.IsSuccessStatusCode)
			{
				// Maybe internet isn't available
			} else
			{
				SetTrackStatus (true);
			}
		}
		

		private static void DontTrack ()
		{
			SetTrackStatus (false);
		}

		private static void SetTrackStatus (bool tracked)
		{
			Settings.database.UpdateData (SettingsConst.LOGIN, "true");
			Settings.Logged = true;
			Settings.googleAnalytics = tracked;
			Settings.database.UpdateData (SettingsConst.GOOGLE_ANALYTICS, tracked.ToString());
			Settings.dontTrack = !tracked;
			Settings.database.UpdateData (SettingsConst.DON_T_TRACK, (!tracked).ToString ());
		}
	}
}