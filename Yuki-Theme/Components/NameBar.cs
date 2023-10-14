using System.Drawing;
using System.Windows.Forms;
using YukiTheme.Engine;
using YukiTheme.Tools;

namespace YukiTheme.Components;

public class NameBar
{
	private ToolStripButton _currentThemeName;

	public NameBar()
	{
		_currentThemeName = new ToolStripButton();
		_currentThemeName.Alignment = ToolStripItemAlignment.Right;
		_currentThemeName.Padding = new Padding(2, 0, 2, 0);
		_currentThemeName.Margin = Padding.Empty;
		_currentThemeName.Text = ThemeNameExtractor.Extract();
		PluginEvents.Instance.ThemeChanged += ThemeChanged;
	}

	private void ThemeChanged(string name)
	{
		_currentThemeName.Text = name;
	}

	public ToolStripItem GetControl() => _currentThemeName;
}