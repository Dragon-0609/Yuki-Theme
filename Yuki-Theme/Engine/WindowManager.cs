using System;
using System.Windows;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public abstract class WindowManager
{
	private bool _hidden = false;

	public abstract void Show();

	public abstract bool IsWindowNull();
	public abstract void SetVisibility(Visibility visibility);


	public void UpdateVisibility()
	{
		if (IsWindowNull())
		{
			HideWindow();
			return;
		}

#if LOG
		Console.WriteLine($"Can show: {CanShow()}");
#endif
		if (CanShow())
		{
			if (_hidden)
			{
				ShowWindow();
			}
		}
		else
		{
			if (!_hidden)
			{
				HideWindow();
			}
		}
	}

	protected virtual bool CanShow()
	{
		return IDEAlterer.CanShowWallpaper;
	}

	private void ShowWindow()
	{
		_hidden = false;
#if LOG
		Console.WriteLine("Showing");
#endif
		SetVisibility(Visibility.Visible);
	}

	private void HideWindow()
	{
		_hidden = true;
#if LOG
		Console.WriteLine("Hiding");
#endif
		SetVisibility(Visibility.Hidden);
	}

	public abstract void ReloadSettings();
}