using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Windows.Interop;
using System.Windows.Navigation;
using Yuki_Theme.Core.WPF.Interfaces;

namespace Yuki_Theme.Core.WPF.Windows
{
	public class UpdateDownloaderPresenter : UpdateDownloader.IPresenter
	{
		private UpdateDownloader.IModel _model;
		private UpdateDownloader.IView  _view;
		private PopupController         _controller;

		public UpdateDownloaderPresenter (UpdateDownloader.IModel model, UpdateDownloader.IView view)
		{
			_model = model;
			_view = view;
		}

		public void CheckVersion (string url, PopupController controller)
		{
			_controller = controller;
			_model.CheckVersion (url, AfterCheckingVersion);
		}

		private void AfterCheckingVersion (bool hasUpdate)
		{
			string title;
			string content;
			NotificationButtonData button1Data;
			NotificationButtonData button2Data;
			if (hasUpdate)
			{
				title = API.Translate ("download.available");
				content = $"Yuki Theme {_model.GetVersion ()}      {API.Translate ("download.size")}: {_model.GetSize ()}";
				
				button1Data = new NotificationButtonData (API.Translate ("download.buttons.update"), true, StartUpdate);

				button2Data = new NotificationButtonData (API.Translate ("download.buttons.github"), _model.GetGithubUrl (), true,
					(sender, args) => { Process.Start (args.Uri.AbsoluteUri); });

			} else
			{
				title = "";
				content = "";
				button1Data = button2Data = null;
			}
			
			_controller.ShowNotification (title, content, button1Data, button2Data);
		}

		private void StartUpdate (object sender, RequestNavigateEventArgs e)
		{
			_view.Show ();
			_model.StartUpdating (ProgressChanged, DownloadingCompleted);
		}

		private void ProgressChanged (object sender, DownloadProgressChangedEventArgs e)
		{
			
		}

		private void DownloadingCompleted (object sender, AsyncCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				string title = API.Translate ("download.refused.error");
				string message = e.Error.Message;
				if(e.Error.InnerException != null)
				{
					if (e.Error.InnerException is SocketException)
					{
						title = API.Translate ("download.refused.title");
						message = API.Translate ("download.refused.message");
					} else
					{
						message = e.Error.InnerException.Message;
					}
				}

				if (e.Error is WebException)
				{
					NotificationButtonData button1Data = new (API.Translate ("download.buttons.github"), _model.GetGithubUrl (), true,
						(sender, args) => { Process.Start (args.Uri.AbsoluteUri); });
					
					_controller.ShowNotification (title, message, button1Data, null);
					_view.Close ();
				}
				else
					throw e.Error;
			} else
			{
				if (e.Cancelled)
				{
					string title = API.Translate ("download.canceled.title");
					string content = API.Translate ("download.canceled.message");
					_controller.ShowNotification (title, content, null, null);
					_view.Close ();
				} else
				{
					QuestionWindow questionWindow = new()
					{
						Background = WPFHelper.bgBrush,
						Foreground = WPFHelper.fgBrush,
						Tag = WPFHelper.GenerateTag
					};

					if (_controller.Target != null)
						questionWindow.Owner = _controller.Target;
					else
					{
						WindowInteropHelper helper = new (questionWindow)
						{
							Owner = _controller.TargetForm.Handle
						};
					}
					
					questionWindow.Title = API.Translate ("download.downloaded.short");
					
					questionWindow.SetTitle (API.Translate ("download.downloaded.full"));
					questionWindow.SetButtonContents (API.Translate ("download.buttons.install"),
						API.Translate ("download.buttons.later"), "");
					questionWindow.SetButtonVisibilities (true, true, false);
					bool needToClose = true;
					if (questionWindow.ShowDialog () == true)
					{
						needToClose = questionWindow.result == 0;
					}
					if (needToClose)
						_view.Close ();
					else
						StartUpdate (this, null);
				}
			}
		}

		
		public void UpdateProgress ()
		{
			
		}

		public void StartDownloading ()
		{
			
		}
	}
}