using System;
using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Forms;

public class PopupFormsController
{
	public  DownloadForm     df;
	public  NotificationForm nf;
	public  IColorUpdatable  colorUpdatable;
	public  Form             form;
	private int              notHeight;

	public PopupFormsController (Form fm, int ntHeight, IColorUpdatable updatable)
	{
		form = fm;
		notHeight = ntHeight;
		colorUpdatable = updatable;
	}
	
	public void showDownloader ()
	{
		if (df == null || nf.IsDisposed)
			df = new DownloadForm (this);
		df.Show (form);
		changeDownloaderLocation ();
		if (nf != null && !nf.IsDisposed && nf.Visible)
		{
			nf.Visible = false;
		}
	}

	public void changeDownloaderLocation ()
	{
		if (df != null && !df.IsDisposed && df.Visible)
		{
			df.StartPosition = FormStartPosition.Manual;
			df.Location = new Point (form.Location.X + form.ClientRectangle.Width - 284,
			                         form.Location.Y + form.ClientRectangle.Height - 73);
		}
	}

	public void changeNotificationLocation ()
	{
		if (nf != null && !nf.IsDisposed && nf.Visible)
		{
			nf.StartPosition = FormStartPosition.Manual;
			nf.Location = new Point (form.Location.X + form.ClientRectangle.Width - nf.ClientRectangle.Width,
			                         form.Location.Y + form.ClientRectangle.Height - nf.ClientRectangle.Height);
		}
	}

	public void ShowNotification (string title, string content)
	{
		if (nf == null || nf.IsDisposed)
			nf = new NotificationForm ();
		nf.Visible = false;
		nf.changeContent (title, content);


		nf.Show (form);

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