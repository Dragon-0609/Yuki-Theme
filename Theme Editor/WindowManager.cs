using System;
using System.Windows;
using Yuki_Theme.Core.WPF.Windows;
using static Yuki_Theme.Core.Communication.MessageTypes;

namespace Theme_Editor
{
	public class WindowManager
	{
		internal Server _server;

		private MainWindow CoreWindow;

		public void OpenMainWindow ()
		{
			// WPFHelper.InitAppForWinforms ();
			if (CoreWindow == null || PresentationSource.FromVisual (CoreWindow) == null)
			{
				CoreWindow = new MainWindow ();
				CoreWindow.Model.StartSettingTheme += ReleaseResources;
				CoreWindow.Model.SetTheme += ReloadLayout;

				CoreWindow.Closed += (sender, args) =>
				{
					GC.Collect ();
					GC.WaitForPendingFinalizers ();
				};
			} else
			{
				if (CoreWindow.IsVisible)
				{
					CoreWindow.Activate ();
					return;
				}
			}

			CoreWindow.Show ();
		}

		private void ReleaseResources ()
		{
			_server.SendMessage (RELEASE_RESOURCES);
		}

		private void ReloadLayout ()
		{
			_server.SendMessage (APPLY_THEME);
		}
	}
}