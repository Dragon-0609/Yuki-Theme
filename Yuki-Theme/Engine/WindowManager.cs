using System.Windows;

namespace YukiTheme.Engine;

public abstract class WindowManager
{
	private bool _hidden;

	internal abstract void Show();

	internal abstract bool IsWindowNull();
	internal abstract void SetVisibility(Visibility visibility);


	internal void UpdateVisibility()
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
			if (_hidden) ShowWindow();
		}
		else
		{
			if (!_hidden) HideWindow();
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

	internal abstract void ReloadSettings();
}