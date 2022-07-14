using System;
using System.ComponentModel;
using System.Net;

namespace Yuki_Theme.Core.WPF.Interfaces
{
	public class UpdateDownloader
	{
		public interface IModel
		{
			void CheckVersion(string url, Action<bool> afterParsing);
			void StartUpdating (DownloadProgressChangedEventHandler progressChanged, AsyncCompletedEventHandler downloadingCompleted);
			string GetVersion ();
			string GetSize ();
			string GetGithubUrl ();
		}

		public interface IPresenter
		{
			void CheckVersion(string url, PopupController controller);
			void UpdateProgress();
			void StartDownloading();
		}

		public interface IView
		{
			void CheckVersion (string url, PopupController controller);

			void Show ();

			void Close ();
		}
	}
}