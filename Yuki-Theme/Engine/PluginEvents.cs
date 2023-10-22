using System;

namespace YukiTheme.Engine;

public class PluginEvents
{
	internal static PluginEvents Instance;

	internal event ThemeChange ThemeChanged;
	internal event Action Reload;

	internal event Action StickerMarginReset;

	internal delegate void ThemeChange(string name);

	internal PluginEvents()
	{
		Instance = this;
	}

	internal void OnThemeChanged(string name)
	{
		ThemeChanged?.Invoke(name);
	}

	internal void OnReload()
	{
		Reload?.Invoke();
	}

	internal void OnStickerMarginReset()
	{
		StickerMarginReset?.Invoke();
	}
}