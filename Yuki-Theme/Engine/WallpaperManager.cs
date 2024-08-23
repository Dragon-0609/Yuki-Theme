using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using YukiTheme.Components;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class WallpaperManager : WindowManager
{
	private string _align = "center";
	private float _opacity = 0.1f;
	private WallpaperWindow _wallpaper;

	internal override void Show()
	{
		_wallpaper = new WallpaperWindow
		{
			AlignX = AlignmentX.Left,
			AlignY = AlignmentY.Top,
			BorderOutlineX = 0,
			BorderOutlineY = 0
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

		ThemeNameExtractor.Infos.Add(new ThemeLoadInfo("opacity", 8, opacity =>
		{
			try
			{
				_opacity = (float)int.Parse(opacity) / 100;
			}
			catch (Exception)
			{
				throw new InvalidDataException($"\"{opacity}\"");
			}

			UpdateOpacity();
		}));
		ThemeNameExtractor.Infos.Add(new ThemeLoadInfo("align", 6, align =>
		{
			_align = align;
			UpdateAlign();
		}));

		IDEAlterer.Instance.Form1.Focus();
	}

	internal override bool IsWindowNull()
	{
		return _wallpaper == null;
	}

	internal override void SetVisibility(Visibility visibility)
	{
		_wallpaper.Visibility = visibility;
	}

	internal void UpdateWallpaper()
	{
		if (!IsWindowNull())
		{
			if (IDEAlterer.HasWallpaper) _wallpaper.SetImage(IDEAlterer.GetWallpaperWPF);

			UpdateOpacity();
		}

		_wallpaper.ResetPosition();
		UpdateVisibility();
	}

	protected override bool CanShow()
	{
		return IDEAlterer.CanShowWallpaper;
	}

	private void UpdateOpacity()
	{
		if (!IsWindowNull()) _wallpaper.Opacity = _opacity;
	}

	private void UpdateAlign()
	{
		if (!IsWindowNull())
		{
			_wallpaper.Wallpaper.HorizontalAlignment = GetXAlign();
			IDEConsole.Log($"{_wallpaper.AlignX}, {_align}, {_wallpaper.BorderOutlineX}, {_wallpaper.BorderOutlineY}");
			_wallpaper.ResetPosition();
		}
	}

	private HorizontalAlignment GetXAlign()
	{
		switch (_align)
		{
			case "left":
				return HorizontalAlignment.Left;
			case "center":
				return HorizontalAlignment.Center;
			case "right":
				return HorizontalAlignment.Right;
			default:
				return HorizontalAlignment.Center;
		}
	}

	internal override void ReloadSettings()
	{
		UpdateVisibility();
	}
}