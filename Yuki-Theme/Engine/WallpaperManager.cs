using System.Windows;
using System.Windows.Media;
using YukiTheme.Components;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class WallpaperManager : WindowManager
{
    private WallpaperWindow _wallpaper;


    public override void Show()
    {
        _wallpaper = new WallpaperWindow()
        {
            AlignX = AlignmentX.Left,
            AlignY = AlignmentY.Top,
            BorderOutlineX = 0,
            BorderOutlineY = 0,
        };
        var targetForm = IDEAlterer.Instance.Form1;
        _wallpaper.SetOwner(targetForm);
        _wallpaper.Width = targetForm.Width;
        _wallpaper.Height = targetForm.Height;
        _wallpaper.AutoSize = true;
        _wallpaper.ResetPosition();
        _wallpaper.Show();
        _wallpaper.Focus();
        UpdateWallpaper();
        IDEAlterer.Instance.Form1.Focus();
    }

    public override bool IsWindowNull() => _wallpaper == null;

    public override void SetVisibility(Visibility visibility)
    {
        _wallpaper.Visibility = visibility;
    }

    public void UpdateWallpaper()
    {
        if (!IsWindowNull())
        {
            if (IDEAlterer.HasWallpaper)
            {
                _wallpaper.SetImage(IDEAlterer.GetWallpaperWPF);
            }

            UpdateOpacity();
        }

        UpdateVisibility();
    }

    private void UpdateOpacity()
    {
        if (!IsWindowNull())
        {
            _wallpaper.Opacity = 0.1f;
        }
    }
}