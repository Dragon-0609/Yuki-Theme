using System;
using System.Windows;
using Yuki_Theme.Core.WPF;
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
				Console.WriteLine ("Core Window initializing");
				CoreWindow = new MainWindow ();
				Console.WriteLine ("Core Window created");
				CoreWindow.Model.StartSettingTheme += ReleaseResources;
				Console.WriteLine ("Core Window added release resources");
				CoreWindow.Model.SetTheme += ReloadLayout;
				Console.WriteLine ("Core Window added reload layout");

				CoreWindow.Closed += (sender, args) =>
				{
					GC.Collect ();
					GC.WaitForPendingFinalizers ();
				};
				Console.WriteLine ("Core Window initialized");
			} else
			{
				if (CoreWindow.IsVisible)
				{
					Console.WriteLine ("Core Window is visible");
					CoreWindow.Activate ();
					return;
				}
			}

			Console.WriteLine ("Core Window showing");
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