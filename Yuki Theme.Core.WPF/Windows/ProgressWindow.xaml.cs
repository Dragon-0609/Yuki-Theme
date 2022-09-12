using System;
using System.Windows;
using System.Windows.Controls;
using Yuki_Theme.Core.WPF.Interfaces;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class ProgressWindow : Window
	{
		private int max  = 0;
		private int step = 0;
		
		private Main.IPresenter _presenter;

		public ProgressWindow (Main.IPresenter presenter)
		{
			InitializeComponent ();
			_presenter = presenter;
		}
		private void Initialize (object sender, RoutedEventArgs e)
		{
			ChangeDialogButtons ();
		}

		private void ChangeDialogButtons ()
		{
			Button saveBtn = (Button)Template.FindName ("SaveButton", this);
			saveBtn.Visibility = Visibility.Collapsed;
			Button closeBtn = (Button)Template.FindName ("CancelButton", this);
			closeBtn.Content = "Close";
			closeBtn.Click += CloseBtnOnClick;
		}
		private void CloseBtnOnClick (object sender, RoutedEventArgs e)
		{
			_presenter.SetImportsCancel (true);
			// _presenter.ImportDirectoryDone ();
		}

		public void SetMax (int mx)
		{
			max = mx;
			step = 0;
			Invoke (() => Progress.Maximum = mx);
		}

		public void UpdateProgress ()
		{
			step++;
			Invoke (() =>
			{
				Status.Text = $"{step}/{max}";
				Progress.Value = step;
			});
		}

		public void Invoke (Action action)
		{
			Dispatcher.Invoke (action, null);
		}
	}
}