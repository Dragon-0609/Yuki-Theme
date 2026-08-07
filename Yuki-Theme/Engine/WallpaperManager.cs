using System;
using System.IO;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Media;
using YukiTheme.Components.Windows;
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
		PluginEvents.Instance.CustomWallpaperPathChanged += UpdateWallpaper;
		PluginEvents.Instance.CustomWallpaperAlignChanged += UpdateAlign;
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

		UpdateAlign();
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
			_wallpaper.ResetPosition();
		}
	}

	private HorizontalAlignment GetXAlign()
	{
		if (DataSaver.Load(SettingsConst.USE_CUSTOM_BACKGROUND, false) && ImageLoader.Instance.HasCustomWallpaper)
		{
			int align = DataSaver.Load(SettingsConst.CUSTOM_BACKGROUND_ALIGN, 1);
			align = align > 2 ? 2 : align < 0 ? 0 : align;

			return (HorizontalAlignment)align;
		}
		
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