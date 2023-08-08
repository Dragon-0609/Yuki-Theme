using System.Windows;
using System.Windows.Media;
using YukiTheme.Style.Controls;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class WallpaperManager
{
    private WallpaperWindow _wallpaper;

    private bool hidden = false;

    public void Show()
    {
        _wallpaper = new WallpaperWindow()
        {
            AlignX = AlignmentX.Left,
            AlignY = AlignmentY.Top,
            borderOutlineX = 0,
            borderOutlineY = 0,
        };
        var targetForm = IDEAlterer.Alterer.Form1;
        _wallpaper.SetOwner(targetForm);
        _wallpaper.Width = targetForm.Width;
        _wallpaper.Height = targetForm.Height;
        _wallpaper.AutoSize = true;
        HideWallpaper();
        UpdateWallpaper();
        _wallpaper.ResetPosition();
        _wallpaper.Show();
        _wallpaper.Focus();
        IDEAlterer.Alterer.Form1.Focus();
    }

    public void UpdateWallpaper()
    {
        if (_wallpaper != null)
        {
            if (IDEAlterer.HasWallpaper)
            {
                _wallpaper.SetImage(IDEAlterer.GetWallpaperWPF);
            }

            UpdateOpacity();
        }

        UpdateVisibility();
    }

    public void UpdateVisibility()
    {
        if (_wallpaper == null)
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
        _wallpaper.Visibility = Visibility.Visible;
    }

    private void HideWallpaper()
    {
        hidden = true;
        _wallpaper.Visibility = Visibility.Hidden;
    }

    private void UpdateOpacity()
    {
        if (_wallpaper != null)
        {
            _wallpaper.Opacity = 0.1f;
        }
    }
}