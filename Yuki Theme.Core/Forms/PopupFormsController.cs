using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Yuki_Theme.Core.Interfaces;
using IWin32Window = System.Windows.Forms.IWin32Window;
using Point = System.Drawing.Point;

namespace Yuki_Theme.Core.Forms
{
	public class PopupFormsController
	{
		public  DownloadForm     df;
		public  NotificationForm nf;
		public  IColorUpdatable  colorUpdatable;
		public  Form             form;
		public  Window           window;
		private NativeWindow     _win32Parent;

		public PopupFormsController (Form fm, IColorUpdatable updatable)
		{
			form = fm;
			window = null;
			_win32Parent = null;
			colorUpdatable = updatable;
		}

		public PopupFormsController (Window wn, IColorUpdatable updatable)
		{
			window = wn;
			form = null;
			_win32Parent = wn.ToNativeWindow ();
			colorUpdatable = updatable;
		}

		private IWin32Window GetParentWindow ()
		{
			if (form != null) return form;
			else if (_win32Parent != null) return _win32Parent;
			else return null;
		}
	
		public void showDownloader ()
		{
			if (df == null || nf.IsDisposed)
				df = new DownloadForm (this);

			IWin32Window parent = GetParentWindow ();
			if (parent != null)
				df.Show(parent);
		
			changeDownloaderLocation ();
			if (nf is { IsDisposed: false, Visible: true })
			{
				nf.Visible = false;
			}
		}

		public void changeDownloaderLocation ()
		{
			if (df != null && !df.IsDisposed && df.Visible)
			{
				df.StartPosition = FormStartPosition.Manual;
				Point destination;
				destination = GetParentLocation ();

				destination.X -= 284;
				destination.Y -= 73;
				df.Location = destination;
			}
		}

		public void changeNotificationLocation ()
		{
			if (nf != null && !nf.IsDisposed && nf.Visible)
			{
				nf.StartPosition = FormStartPosition.Manual;
				Point destination;
				destination = GetParentLocation ();
			
			
				destination.X -=  nf.ClientRectangle.Width;
				destination.Y -= nf.ClientRectangle.Height;
				nf.Location = destination;
			}
		}

		private Point GetParentLocation ()
		{
			return form != null ? new Point (GetFormLocationX (), GetFormLocationY ()) : new Point (GetWindowLocationX (), GetWindowLocationY ());
		}

		private int GetWindowLocationY ()
		{
			return Convert.ToInt32 (window.Top + window.RenderSize.Height);
		}

		private int GetWindowLocationX ()
		{
			return Convert.ToInt32 (window.Left + window.RenderSize.Width);
		}

		private int GetFormLocationY ()
		{
			return form.Location.Y + form.ClientRectangle.Height;
		}

		private int GetFormLocationX ()
		{
			return form.Location.X + form.ClientRectangle.Width;
		}

		public void ShowNotification (string title, string content)
		{
			if (nf == null || nf.IsDisposed)
				nf = new NotificationForm ();
			nf.Visible = false;
			nf.changeContent (title, content);
			IWin32Window parent = GetParentWindow ();
			if (parent != null)
				nf.Show(parent);

			if (df != null && !df.IsDisposed && df.Visible)
			{
				df.Visible = false;
			}
		}

		public void InitializeAllWindows ()
		{
			if (df == null || df.IsDisposed)
				df = new DownloadForm (this);
			if (nf == null || nf.IsDisposed)
				nf = new NotificationForm ();
		}
	
		public void CloseAllWindows ()
		{
			if (nf != null)
				nf.Dispose ();
			if (df != null)
				df.Dispose ();
		}

		public void TryToUpdateNotificationWindow ()
		{
			if (nf is { IsDisposed: false, Visible: true })
				nf.NotificationForm_Shown (this, EventArgs.Empty);
		}

		public void CloseDownloader ()
		{
			if (df != null && !df.IsDisposed)
			{
				df.Close ();
			}
		}

		public void CloseNotification ()
		{
			if (nf != null && !nf.IsDisposed)
				nf.Close ();
		}
	
	}
}