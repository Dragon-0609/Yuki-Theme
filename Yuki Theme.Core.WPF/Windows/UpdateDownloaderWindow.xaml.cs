using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using Yuki_Theme.Core.WPF.Controls;
using Yuki_Theme.Core.WPF.Interfaces;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class UpdateDownloaderWindow : SnapWindow, UpdateDownloader.IView
	{
		private UpdateDownloader.IModel     _model;
		private UpdateDownloader.IPresenter _presenter;

		public static bool IsShown = false;

		public UpdateDownloaderWindow ()
		{
			InitializeComponent ();
			_model = new UpdateDownloaderModel ();
			_presenter = new UpdateDownloaderPresenter (_model, this);
		}

		public static UpdateDownloaderWindow CreateUpdateDownloader (Window parent)
		{
			UpdateDownloaderWindow downloaderWindow = new UpdateDownloaderWindow
			{
				target = parent,
				Owner = parent
			};
			downloaderWindow.AlignX = AlignmentX.Right;
			downloaderWindow.AlignY = AlignmentY.Bottom;

			return downloaderWindow;
		}

		public static UpdateDownloaderWindow CreateUpdateDownloader (Form parentForm)
		{
			UpdateDownloaderWindow downloaderWindow = new UpdateDownloaderWindow
			{
				targetForm = parentForm
			};
			WindowInteropHelper helper = new WindowInteropHelper (downloaderWindow)
			{
				Owner = parentForm.Handle
			};
			downloaderWindow.AlignX = AlignmentX.Right;
			downloaderWindow.AlignY = AlignmentY.Bottom;

			return downloaderWindow;
		}

		public void UpdateColors ()
		{
			Foreground = WPFHelper.fgBrush;
			Background = WPFHelper.bgBrush;
			Tag = WPFHelper.GenerateTag;
			BorderBrush = WPFHelper.borderBrush;
			Progress.Foreground = WPFHelper.borderBrush;
			Progress.Background = WPFHelper.bgBrush;
			Progress.BorderBrush = WPFHelper.bgClickBrush;

		}

		public void CheckVersion (PopupController controller)
		{
			const string stableUrl = "https://API_Base.Current.github.com/repos/dragon-0609/yuki-theme/releases";
			const string betaUrl = "https://API_Base.Current.github.com/repos/Dragon-0609/Yuki-Theme/releases/latest";
			string url = Settings.Beta ? stableUrl : betaUrl;
			_presenter.CheckVersion (url, controller);
		}

		public void NoteAsShown ()
		{
			IsShown = true;
		}

		public void UpdateProgressBar (int percent)
		{
			Progress.Value = percent;
			ProgressText.Text = $"{percent:00}%";
		}

		public void UpdateSizeText (double size)
		{
			SizeText.Text = $"{size:0.0} MB/{_model.GetSizeVar ()} MB";
		}

		public void ChangeHeader (string text)
		{
			HeaderBlock.Text = text;
		}

		public void ChangeSize ()
		{
			SizeText.Text = $"0 MB/{_model.GetSizeVar ()} MB";
		}

		private void CancelDownloading (object sender, RoutedEventArgs e)
		{
			_model.CancelDownloading ();
		}

		private void UpdateDownloaderWindow_OnClosing (object sender, CancelEventArgs e)
		{
			IsShown = false;
			_model.CancelDownloading ();
			
		}
	}
}