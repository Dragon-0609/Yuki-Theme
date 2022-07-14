using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Navigation;
using Yuki_Theme.Core.WPF.Controls;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class NotificationWindow : SnapWindow
	{
		private Uri DEFAULT_URI = new Uri ("http://dragon-0609.github.io/Yuki-Theme/");

		private NotificationButtonData button1Data;
		private NotificationButtonData button2Data;

		public NotificationWindow ()
		{
			InitializeComponent ();
		}

		public void SetContent (string title, string content)
		{
			HeaderBlock.Text = title;
			ContentBlock.Text = content;
		}

		public void SetButtons (NotificationButtonData button1, NotificationButtonData button2)
		{
			button1Data = button1;
			button2Data = button2;
			if (button1 == null)
				Button1.Visibility = Visibility.Hidden;
			else
			{
				Button1Text.Text = button1.Text;
				
				AddUri (Button1Hyper, button1);

				AddEvents (Button1Hyper, button1);
			}

			if (button2 == null)
				Button2.Visibility = Visibility.Hidden;
			else
			{
				Button2Text.Text = button2.Text;
				AddUri (Button2Hyper, button2);
				AddEvents (Button2Hyper, button2);
			}
		}

		private void AddEvents (Hyperlink hyperlink, NotificationButtonData button1)
		{
			hyperlink.RequestNavigate += button1.NavigateEventArgs;
			if (button1.CloseAfterClick)
				hyperlink.RequestNavigate += AutoCloseButton;
		}

		private void AddUri (Hyperlink hyperlink, NotificationButtonData button1)
		{
			hyperlink.NavigateUri = button1.Link != "" ? new Uri (button1.Link) : DEFAULT_URI;
		}

		private void AutoCloseButton (object sender, RequestNavigateEventArgs e)
		{
			if (target != null)
				target.Focus ();
			else if (targetForm != null)
				targetForm.Focus ();
			Close ();
		}

		public void RemoveOldEvents ()
		{
			if (button1Data != null)
			{
				Button1Hyper.RequestNavigate -= button1Data.NavigateEventArgs;
				if (button1Data.CloseAfterClick)
					Button1Hyper.RequestNavigate -= AutoCloseButton;
			}

			if (button2Data != null)
			{
				Button2Hyper.RequestNavigate -= button2Data.NavigateEventArgs;
				if (button2Data.CloseAfterClick)
					Button2Hyper.RequestNavigate -= AutoCloseButton;
			}
		}

		public static NotificationWindow CreateNotification (Window parent, string title, string content)
		{
			NotificationWindow notificationWindow = new NotificationWindow
			{
				target = parent,
				Owner = parent
			};
			notificationWindow.SetContent (title, content);

			return notificationWindow;
		}

		public static NotificationWindow CreateNotification (Form parentForm, string title, string content)
		{
			NotificationWindow notificationWindow = new NotificationWindow
			{
				targetForm = parentForm
			};
			WindowInteropHelper helper = new WindowInteropHelper (notificationWindow)
			{
				Owner = parentForm.Handle
			};

			notificationWindow.SetContent (title, content);

			return notificationWindow;
		}

		private void OnCloseClicked (object sender, RoutedEventArgs e)
		{
			this.Close ();
		}

		public void UpdateColors ()
		{
			Foreground = WPFHelper.fgBrush;
			Background = WPFHelper.bgBrush;
			Tag = WPFHelper.GenerateTag;
			BorderBrush = WPFHelper.borderBrush;
		}
	}

	public class NotificationButtonData
	{
		public string Text;
		public string Link;
		public bool   CloseAfterClick = false;

		public RequestNavigateEventHandler NavigateEventArgs;

		public NotificationButtonData (string text, bool closeAfterClick, RequestNavigateEventHandler navigateEventArgs)
		{
			Text = text;
			Link = "";
			CloseAfterClick = closeAfterClick;
			NavigateEventArgs = navigateEventArgs;
		}

		public NotificationButtonData (string text, string link, RequestNavigateEventHandler navigateEventArgs)
		{
			Text = text;
			Link = link;
			NavigateEventArgs = navigateEventArgs;
		}

		public NotificationButtonData (string text, string link, bool closeAfterClick, RequestNavigateEventHandler navigateEventArgs)
		{
			Text = text;
			Link = link;
			CloseAfterClick = closeAfterClick;
			NavigateEventArgs = navigateEventArgs;
		}
	}
}