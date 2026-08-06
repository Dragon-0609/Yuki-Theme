using YukiTheme.Engine;
using YukiTheme.Tools;

namespace YukiTheme.Components.StatusBar;

public class NameBar : StatusBarItem
{
	internal static string ThemeName;

	public NameBar()
	{
		ColorChanger.Instance.UpdatedColors += UpdatedColors;
		LoadSvg();

		ThemeNameExtractor.Infos.Add(new ThemeLoadInfo("name", 5, name =>
		{
			ThemeName = name;
			Text = name;
		}));

		PluginEvents.Instance.ThemeChanged += ThemeChanged;
	}

	private void UpdatedColors()
	{
		LoadSvg();
	}

	private void LoadSvg()
	{
		SetIcon("heart.svg", ColorReference.BorderColor);
	}

	private void ThemeChanged(string name)
	{
		ThemeName = name;
		Text = name;
	}
}