using System.Windows;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public abstract class WindowManager
{
    private bool hidden = false;

    public abstract void Show();

    public abstract bool IsWindowNull();
    public abstract void SetVisibility(Visibility visibility);


    public void UpdateVisibility()
    {
        if (IsWindowNull())
        {
            HideWallpaper();
            return;
        }

        if (IDEAlterer.CanShowWallpaper)
        {
            if (hidden)
            {
                ShowWallpaper();
            }
        }
        else
        {
            if (!hidden)
            {
                HideWallpaper();
            }
        }
    }

    private void ShowWallpaper()
    {
        hidden = false;
        SetVisibility(Visibility.Visible);
    }

    private void HideWallpaper()
    {
        hidden = true;
        SetVisibility(Visibility.Visible);
    }
}