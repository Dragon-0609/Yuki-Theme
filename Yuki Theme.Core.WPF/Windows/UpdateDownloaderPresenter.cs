using System;
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
		
		private int    _progress = 0;
		private double _recieved = 0;

		public UpdateDownloaderPresenter (UpdateDownloader.IModel model, UpdateDownloader.IView view)
		{
			_model = model;
			_view = view;
		}

		public void CheckVersion (string url, PopupController controller)
		{
			_controller = controller;
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			_model.CheckVersion (_controller, url, AfterCheckingVersion);
		}

		private void AfterCheckingVersion (bool hasUpdate)
		{
			string title;
			string content;
			NotificationButtonData button1Data;
			NotificationButtonData button2Data;
			if (hasUpdate)
			{
				title = API.API.Current.Translate ("download.available");
				content = GetVersionFormatted (true);
				
				button1Data = new NotificationButtonData (API.API.Current.Translate ("download.buttons.update"), true, StartUpdate);

				button2Data = new NotificationButtonData (API.API.Current.Translate ("download.buttons.github"), _model.GetGithubUrl (), true,
					(sender, args) => { Process.Start (args.Uri.AbsoluteUri); });

			} else
			{
				title = API.API.Current.Translate ("download.uptodate");
				content = API.API.Current.Translate ("download.latest");
				button1Data = button2Data = null;
			}
			
			_controller.ShowNotification (title, content, button1Data, button2Data);
		}

		private string GetVersionFormatted (bool withSize)
		{
			return $"Yuki Theme {_model.GetVersion ()}" + GetFormattedSize (withSize);
		}

		private string GetFormattedSize (bool withSize)
		{
			return !withSize ? "" : $"{Environment.NewLine}{API.API.Current.Translate ("download.size")}: {_model.GetSize ()}";
		}

		private void StartUpdate (object sender, RequestNavigateEventArgs e)
		{
			_view.UpdateColors ();
			_view.NoteAsShown ();
			_view.ChangeHeader (GetVersionFormatted (false));
			_view.UpdateProgressBar (0);
			_view.ChangeSize ();
			_view.Show ();
			_view.ResetPosition ();
			_model.StartUpdating (ProgressChanged, DownloadingCompleted);
		}

		private void ProgressChanged (object sender, DownloadProgressChangedEventArgs e)
		{
			_progress = e.ProgressPercentage;
			_recieved = e.BytesReceived / 1024.0 / 1024.0;
			UpdateProgress ();
		}

		private void DownloadingCompleted (object sender, AsyncCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				string title = API.API.Current.Translate ("download.refused.error");
				string message = e.Error.Message;
				if(e.Error.InnerException != null)
				{
					if (e.Error.InnerException is SocketException)
					{
						title = API.API.Current.Translate ("download.refused.title");
						message = API.API.Current.Translate ("download.refused.message");
					} else
					{
						message = e.Error.InnerException.Message;
					}
				}

				if (e.Error is WebException)
				{
					NotificationButtonData button1Data = new (API.API.Current.Translate ("download.buttons.github"), _model.GetGithubUrl (), true,
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
					string title = API.API.Current.Translate ("download.canceled.title");
					string content = API.API.Current.Translate ("download.canceled.message");
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
						questionWindow.SetOwner (_controller.Target);
					else
					{
						questionWindow.SetOwner (_controller.TargetForm);
					}
					
					questionWindow.Title = API.API.Current.Translate ("download.downloaded.short");
					
					questionWindow.SetTitle (API.API.Current.Translate ("download.downloaded.full"));
					questionWindow.SetButtonContents (API.API.Current.Translate ("download.buttons.install"),
						API.API.Current.Translate ("download.buttons.later"), "");
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


		private void UpdateProgress ()
		{
			_view.UpdateProgressBar (_progress);
			_view.UpdateSizeText (_recieved);
		}
	}
}