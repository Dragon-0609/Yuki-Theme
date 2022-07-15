using System;
using System.ComponentModel;
using System.Net;

namespace Yuki_Theme.Core.WPF.Interfaces
{
	public class UpdateDownloader
	{
		public interface IModel
		{
			void CheckVersion(PopupController controller, string url, Action<bool> afterParsing);
			void StartUpdating (DownloadProgressChangedEventHandler progressChanged, AsyncCompletedEventHandler downloadingCompleted);
			string GetVersion ();
			string GetSize ();
			string GetSizeVar ();
			string GetGithubUrl ();
			void CancelDownloading ();
		}

		public interface IPresenter
		{
			void CheckVersion(string url, PopupController controller);
		}

		public interface IView
		{
			void CheckVersion (PopupController controller);

			void NoteAsShown ();
			void Show ();

			void UpdateColors ();
			
			void Close ();

			void UpdateProgressBar (int percent);

			void UpdateSizeText (double size);

			void ResetPosition ();
			void ChangeHeader (string text);
		}
	}
}