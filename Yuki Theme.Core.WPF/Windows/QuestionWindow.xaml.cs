using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Button = System.Windows.Controls.Button;
using HorizontalAlignment = System.Windows.HorizontalAlignment;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class QuestionWindow : Window
	{
		public int result = 0;

		private int _visibleButtonsCount = 3;
		
		public QuestionWindow ()
		{
			InitializeComponent ();
		}

		internal void SetTitle (string text)
		{
			Question.Text = text;
		}

		internal void SetButtonContents (string t1, string t2, string t3)
		{
			B1.Content = t1;
			B2.Content = t2;
			B3.Content = t3;
		}

		public void SetButtonVisibilities (bool b1, bool b2, bool b3)
		{
			_visibleButtonsCount = 3;
			HideButton (b1, B1);
			HideButton (b2, B2);
			HideButton (b3, B3);
			ButtonsGrid.Columns = _visibleButtonsCount;
		}

		public void SetButtonAlign (HorizontalAlignment alignment)
		{
			B1.HorizontalAlignment = B2.HorizontalAlignment = B3.HorizontalAlignment = alignment;
		}

		private void HideButton (bool b1, Button button)
		{
			_visibleButtonsCount--;
			button.Visibility = b1 ? Visibility.Visible : Visibility.Collapsed;
		}

		private void B1Clicked (object sender, RoutedEventArgs e)
		{
			result = 0;
			DialogResult = true;
		}

		private void B2Clicked (object sender, RoutedEventArgs e)
		{
			result = 1;
			DialogResult = true;
		}

		private void B3Clicked (object sender, RoutedEventArgs e)
		{
			result = 2;
			DialogResult = true;
		}

		public static int AskActionChoice (Window parent)
		{
			int choice = 0;
			QuestionWindow questionWindow = new QuestionWindow
			{
				Background = parent.Background,
				Foreground = parent.Foreground,
				Tag = parent.Tag,
				Icon = parent.Icon,
				Owner = parent
			};
			questionWindow.SetTitle (API.API.Current.Translate ("messages.theme.others.found"));

			questionWindow.SetButtonContents (API.API.Current.Translate ("messages.buttons.yes"),
			                                  API.API.Current.Translate ("settings.additional.action.import"),
			                                  API.API.Current.Translate ("messages.buttons.no"));
			bool? dialog = questionWindow.ShowDialog ();
			if (dialog == true)
			{
				choice = questionWindow.result;
			}
			return choice;
		}

		public void SetOwner (Window target)
		{
				Owner = target;
		}

		public void SetOwner (Form target)
		{
			if (target != null)
			{
				WindowInteropHelper helper = new (this)
				{
					Owner = target.Handle
				};
			}
		}
		
	}
}