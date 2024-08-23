using System;

namespace YukiTheme.Engine;

public class PluginEvents
{
	internal static PluginEvents Instance;

	internal PluginEvents()
	{
		Instance = this;
	}

	internal event ThemeChange ThemeChanged;
	internal event Action Reload;

	internal event Action StickerMarginReset;

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

	internal delegate void ThemeChange(string name);
}