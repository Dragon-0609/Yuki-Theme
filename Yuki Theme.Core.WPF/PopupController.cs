using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Yuki_Theme.Core.Interfaces;
using Yuki_Theme.Core.WPF.Controls;
using Yuki_Theme.Core.WPF.Windows;
using Color = System.Drawing.Color;

namespace Yuki_Theme.Core.WPF
{
	public class PopupController
	{
		public Form   TargetForm;
		public Window Target;

		private NotificationWindow     _notificationWindow;
		private UpdateDownloaderWindow _downloaderWindow;

		private IColorUpdatable _colorUpdatable;

		private Queue<NotificationPackage> _notificationPackages = new Queue<NotificationPackage> ();

		public PopupController (Window parent, IColorUpdatable updatable)
		{
			Target = parent;
			TargetForm = null;
			_colorUpdatable = updatable;
			AddUpdates ();
		}

		public PopupController (Form parent, IColorUpdatable updatable)
		{
			Target = null;
			TargetForm = parent;
			_colorUpdatable = updatable;
			AddUpdates ();
		}

		private void AddUpdates ()
		{
			_colorUpdatable.OnColorUpdate += UpdateNotificationColor;
			_colorUpdatable.OnColorUpdate += UpdateDownloaderColor;
		}


		#region Notification

		public void ShowNotification (string title, string content, NotificationButtonData button1Data, NotificationButtonData button2Data)
		{
			if (Target != null || TargetForm != null)
			{
				if (IsNotificationNull ())
				{
					CreateAndShowNotification (title, content, button1Data, button2Data);

					UpdateNotificationColor (Helper.bgColor, Helper.fgColor, Helper.bgClick);
				} else
				{
					_notificationPackages.Enqueue (new NotificationPackage (title, content, button1Data, button2Data));
					
					// SwapNotificationData (title, content, button1Data, button2Data);
				}
			} else
			{
				API_Events.showError ("Couldn't get parent", "Couldn't get parent");
			}
		}

		private bool IsNotificationNull ()
		{
			return _notificationWindow == null || PresentationSource.FromVisual (_notificationWindow) == null;
		}

		private bool IsDownloaderNull ()
		{
			return _downloaderWindow == null;
		}

		private void CreateAndShowNotification (string title,
		                                        string content, NotificationButtonData button1Data, NotificationButtonData button2Data)
		{
			if (Target != null)
				_notificationWindow = NotificationWindow.CreateNotification (Target, title, content);
			else if (TargetForm != null)
				_notificationWindow = NotificationWindow.CreateNotification (TargetForm, title, content);

			if (_notificationWindow != null)
			{
				_notificationWindow.SetAlign (new SnapWindowAlign (AlignmentX.Right, AlignmentY.Bottom));
				_notificationWindow.SetButtons (button1Data, button2Data);
				_notificationWindow.Closed += NotificationWindowOnClosed;
				_notificationWindow.Show ();
				_notificationWindow.ResetPosition ();
			}
		}

		private void CreateAndShowNotification (NotificationPackage package)
		{
			lock (_notificationWindow)
			{
				CreateAndShowNotification (package.Title, package.Content, package.Button1Data, package.Button2Data);
			}

			UpdateNotificationColor (Helper.bgColor, Helper.fgColor, Helper.bgClick);
		}

		private void NotificationWindowOnClosed (object sender, EventArgs e)
		{
			lock (_notificationPackages)
			{
				if (_notificationPackages.Count > 0)
				{
						NotificationPackage package = _notificationPackages.Dequeue ();
						CreateAndShowNotification (package);
				}
			}
		}

		private void SwapNotificationData (string title,
										   string content, NotificationButtonData button1Data, NotificationButtonData button2Data)
		{
			_notificationWindow.RemoveOldEvents ();
			_notificationWindow.SetContent (title, content);
			_notificationWindow.SetButtons (button1Data, button2Data);
		}

		private void UpdateNotificationColor (Color bg, Color fg, Color bgClick)
		{
			lock (_notificationWindow)
			{
				if (!IsNotificationNull ())
				{
					Console.WriteLine ("Updating");
					_notificationWindow.UpdateColors ();
				} else
					Console.WriteLine ("NotUpdating");

			}
		}

		private void UpdateDownloaderColor (Color bg, Color fg, Color bgClick)
		{
			if (!IsDownloaderNull () && UpdateDownloaderWindow.IsShown)
			{
				_downloaderWindow.UpdateColors ();
			}
		}

		#endregion

		public void CheckUpdate ()
		{
			if (Target != null || TargetForm != null)
			{
				if (!IsDownloaderNull () && UpdateDownloaderWindow.IsShown)
				{
					_downloaderWindow.Close ();
					_downloaderWindow = null;
				}

				UpdateDownloaderWindow.IsShown = false;
				if (Target != null)
					_downloaderWindow = UpdateDownloaderWindow.CreateUpdateDownloader (Target);
				else if (TargetForm != null)
					_downloaderWindow = UpdateDownloaderWindow.CreateUpdateDownloader (TargetForm);

				UpdateDownloaderColor (Helper.bgColor, Helper.fgColor, Helper.bgClick);
				_downloaderWindow.CheckVersion (this);
			}
		}

		public void InstallUpdate (string file)
		{
			if (Path.GetExtension (file).ToLower ().EndsWith (".zip") && UpdateDownloaderModel.IsValidUpdate (file))
			{
				if (UpdateDownloaderModel.IsUpdateDownloaded ())
					File.Delete (UpdateDownloaderModel.GetUpdatePath ());

				File.Copy (file, UpdateDownloaderModel.GetUpdatePath ());
				
				UpdateDownloaderModel.StartUpdating ();
			} else
			{
				if (API_Events.showError != null)
					API_Events.showError (API.Translate ("messages.update.invalid"), API.Translate ("messages.update.wrong"));
			}
			
		}
		
	}
}