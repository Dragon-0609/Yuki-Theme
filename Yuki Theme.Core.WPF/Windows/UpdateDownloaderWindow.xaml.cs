using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Navigation;
using Yuki_Theme.Core.WPF.Controls;
using Yuki_Theme.Core.WPF.Interfaces;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class UpdateDownloaderWindow : SnapWindow, UpdateDownloader.IView
	{
		private UpdateDownloader.IModel     _model;
		private UpdateDownloader.IPresenter _presenter;

		public UpdateDownloaderWindow ()
		{
			InitializeComponent ();
			_model = new UpdateDownloaderModel ();
			_presenter = new UpdateDownloaderPresenter (_model, this);
		}

		public void SetContent (string title, string content)
		{
			HeaderBlock.Text = title;
			ContentBlock.Text = content;
		}

		public static UpdateDownloaderWindow CreateUpdateDownloader (Window parent, string title, string content)
		{
			UpdateDownloaderWindow downloaderWindow = new UpdateDownloaderWindow
			{
				target = parent,
				Owner = parent
			};
			downloaderWindow.SetContent (title, content);

			return downloaderWindow;
		}

		public static UpdateDownloaderWindow CreateUpdateDownloader (Form parentForm, string title, string content)
		{
			UpdateDownloaderWindow downloaderWindow = new UpdateDownloaderWindow
			{
				targetForm = parentForm
			};
			WindowInteropHelper helper = new WindowInteropHelper (downloaderWindow)
			{
				Owner = parentForm.Handle
			};

			downloaderWindow.SetContent (title, content);

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

		}

		public void CheckVersion (string url, PopupController controller)
		{
			_presenter.CheckVersion (url, controller);
		}
	}
}