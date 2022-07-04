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

		private NotificationWindow _notificationWindow;

		private IColorUpdatable _colorUpdatable;

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
		}


		#region Notification

		public void ShowNotification (string title, string content, NotificationButtonData button1Data, NotificationButtonData button2Data)
		{
			if (Target != null || TargetForm != null)
			{
				if (IsNotificationNull ())
				{
					CreateAndShowNotification (title, content, button1Data, button2Data);
				} else
				{
					SwapNotificationData (title, content, button1Data, button2Data);
				}

				UpdateNotificationColor (Helper.bgColor, Helper.fgColor, Helper.bgClick);
			} else
			{
				API_Events.showError ("Couldn't get parent", "Couldn't get parent");
			}
		}

		private bool IsNotificationNull ()
		{
			return _notificationWindow == null || PresentationSource.FromVisual (_notificationWindow) == null;
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
				_notificationWindow.Show ();
			}
		}

		private void SwapNotificationData (string title,
		                                   string content, NotificationButtonData button1Data, NotificationButtonData button2Data)
		{
			_notificationWindow.RemoveOldEvents ();
			_notificationWindow.SetContent (title, content);
			_notificationWindow.SetButtons (button1Data, button2Data);
		}

		private void UpdateNotificationColor (Color bg, Color fg, Color bgclick)
		{
			if (!IsNotificationNull ())
			{
				_notificationWindow.UpdateColors ();
			}
		}

		#endregion

		public void ShowUpdateDownloader ()
		{
		}
	}
}