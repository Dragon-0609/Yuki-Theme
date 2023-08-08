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
		UpdateWallpaper();
		UpdateOpacity();
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
				if (hidden)
				{
					hidden = false;
					_wallpaper.Visibility = Visibility.Visible;
				}

				_wallpaper.SetImage(IDEAlterer.GetWallpaperWPF);
			}
			else
			{
				hidden = true;
				_wallpaper.Visibility = Visibility.Hidden;
			}

			UpdateOpacity();
		}
	}

	private void UpdateOpacity()
	{
		if (_wallpaper != null)
		{
			_wallpaper.Opacity = 0.1f;
		}
	}
}