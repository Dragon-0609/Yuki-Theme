using System;

namespace YukiTheme.Engine;

public class PluginEvents
{
	public static PluginEvents Instance;

	public event ThemeChange ThemeChanged;
	public event Action Reload;

	public delegate void ThemeChange(string name);

	public PluginEvents()
	{
		Instance = this;
	}

	public void OnThemeChanged(string name)
	{
		ThemeChanged?.Invoke(name);
	}
	public void OnReload()
	{
		Reload?.Invoke();
	}
}