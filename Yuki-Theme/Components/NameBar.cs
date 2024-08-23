using System.Windows.Forms;
using YukiTheme.Engine;
using YukiTheme.Tools;

namespace YukiTheme.Components;

public class NameBar
{
	internal static string ThemeName;

	private readonly ToolStripButton _currentThemeName;

	public NameBar()
	{
		_currentThemeName = new ToolStripButton();
		_currentThemeName.Alignment = ToolStripItemAlignment.Right;
		_currentThemeName.Padding = new Padding(2, 0, 2, 0);
		_currentThemeName.Margin = Padding.Empty;

		ThemeNameExtractor.Infos.Add(new ThemeLoadInfo("name", 5, name =>
		{
			ThemeName = name;
			_currentThemeName.Text = name;
		}));

		PluginEvents.Instance.ThemeChanged += ThemeChanged;
	}

	private void ThemeChanged(string name)
	{
		ThemeName = name;
		_currentThemeName.Text = name;
	}

	internal ToolStripItem GetControl()
	{
		return _currentThemeName;
	}
}