using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using YukiTheme.Components;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class ThemeWindowManager : WindowManager
{
	private ThemeSelectWindow _themeSelect;

	internal override void Show()
	{
		WpfConverter.ConvertGuiColorsNBrushes();

		_themeSelect = new ThemeSelectWindow
		{
			AlignX = AlignmentX.Left,
			AlignY = AlignmentY.Top,
			BorderOutlineX = 0,
			BorderOutlineY = 0
		};
		var targetForm = IDEAlterer.Instance.Form1;
		_themeSelect.SetOwner(targetForm);
		_themeSelect.Width = targetForm.Width;
		_themeSelect.Height = targetForm.Height;
		_themeSelect.AutoSize = true;
		_themeSelect.ResetPosition();
		ElementHost.EnableModelessKeyboardInterop(_themeSelect);
		_themeSelect.Show();
		_themeSelect.Focus();

		_themeSelect.SelectedTheme += ThemeSelected;

		IDEAlterer.Instance.Form1.Focus();
		_themeSelect.Search.Focus();
	}

	private void ThemeSelected(string name)
	{
		PluginEvents.Instance.OnThemeChanged(name);
	}

	internal override bool IsWindowNull()
	{
		return _themeSelect == null;
	}

	internal override void SetVisibility(Visibility visibility)
	{
		_themeSelect.Visibility = visibility;
	}

	internal override void ReloadSettings()
	{
	}
}