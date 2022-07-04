using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Navigation;
using Yuki_Theme.Core.WPF.Controls;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class NotificationWindow : SnapWindow
	{
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
				Button1Hyper.NavigateUri = new Uri (button1.Link);
				Button1Hyper.RequestNavigate += button1.NavigateEventArgs;
			}
			
			if (button2 == null)
				Button2.Visibility = Visibility.Hidden;
			else
			{
				Button2Text.Text = button2.Text;
				Button2Hyper.NavigateUri = new Uri (button2.Link);
				Button2Hyper.RequestNavigate += button2.NavigateEventArgs;
			}
		}

		public void RemoveOldEvents ()
		{
			if (button1Data != null)
				Button1Hyper.RequestNavigate -= button1Data.NavigateEventArgs;
			
			if (button2Data != null)
				Button2Hyper.RequestNavigate -= button2Data.NavigateEventArgs;
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
		public string Text = "";
		public string Link = "";
		
		public RequestNavigateEventHandler NavigateEventArgs;
	}
}