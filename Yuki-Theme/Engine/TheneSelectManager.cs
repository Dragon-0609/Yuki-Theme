using System.Windows;
using System.Windows.Media;
using YukiTheme.Components;

namespace YukiTheme.Engine;

public class ThemeWindowManager : WindowManager
{
    private ThemeSelectWindow _themeSelect;

    public override void Show()
    {
        _themeSelect = new ThemeSelectWindow()
        {
            AlignX = AlignmentX.Left,
            AlignY = AlignmentY.Top,
            BorderOutlineX = 0,
            BorderOutlineY = 0,
        };
        var targetForm = IDEAlterer.Instance.Form1;
        _themeSelect.SetOwner(targetForm);
        _themeSelect.Width = targetForm.Width;
        _themeSelect.Height = targetForm.Height;
        _themeSelect.AutoSize = true;
        _themeSelect.ResetPosition();
        _themeSelect.Show();
        _themeSelect.Focus();
        _themeSelect.SelectedTheme += () =>
        {
            IDEConsole.Log("Selected theme");
        };
        IDEAlterer.Instance.Form1.Focus();
    }

    public override bool IsWindowNull() => _themeSelect == null;

    public override void SetVisibility(Visibility visibility)
    {
        _themeSelect.Visibility = visibility;
    }
}