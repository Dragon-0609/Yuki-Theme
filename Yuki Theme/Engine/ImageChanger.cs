using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class ImageChanger
{
	private const string ImagesFolder = "Highlighting";
	private const string PngFilter = "*.png";
	private Image _wallpaper;
	private Image _sticker;
	private ImageSource _wallpaperSource;
	private ImageSource _stickerSource;
	private string _wallpaperName = "background";
	private string _stickerName = "sticker";

	public bool HasImage => _wallpaper != null;
	public Image GetWallpaper => _wallpaper;
	public Image GetSticker => _sticker;

	public bool HasSticker => _sticker != null;

	public ImageSource GetWallpaperWPF => _wallpaperSource;
	public ImageSource GetStickerWPF => _stickerSource;

	private void RemoveLastImage()
	{
		if (_wallpaper != null)
		{
			_wallpaper.Dispose();
			_wallpaper = null;
		}

		_wallpaperSource = null;

		if (_sticker != null)
		{
			_sticker.Dispose();
			_sticker = null;
		}

		_stickerSource = null;
	}

	public void LoadImages()
	{
		RemoveLastImage();
		string[] png = Directory.GetFiles(Path.Combine(YukiTheme_VisualPascalABCPlugin.GetCurrentFolder, ImagesFolder), PngFilter);
		if (png.Length > 0)
		{
			var wallpaperCondition = png.Where(p => Path.GetFileNameWithoutExtension(p).ToLower() == _wallpaperName);
			string wallpaperFileName;
			string stickerFileName;
			var ordered = png.OrderByDescending(p => new FileInfo(p).Length);
			if (wallpaperCondition.Any())
			{
				wallpaperFileName = wallpaperCondition.First();
			}
			else
			{
				wallpaperFileName = ordered.First();
			}

			_wallpaper = Image.FromFile(wallpaperFileName);

			if (png.Length > 1)
			{
				var stickerCondition = png.Where(p => Path.GetFileNameWithoutExtension(p).ToLower() == _stickerName);

				if (stickerCondition.Any())
				{
					stickerFileName = stickerCondition.First();
				}
				else
				{
					stickerFileName = ordered.Skip(1).First();
				}

				_sticker = Image.FromFile(stickerFileName);
			}
		}
	}

	public void ApplyImages()
	{
		if (_wallpaper != null)
		{
			_wallpaperSource = _wallpaper.ToWPFImage();
			_stickerSource = _sticker.ToWPFImage();
		}
	}

	public Image SetImageOpacity(Image image, float opacity)
	{
		try
		{
			Bitmap bmp = new Bitmap(image.Width, image.Height);

			opacity = opacity == 0 ? opacity : opacity / 100f;

			using (Graphics gfx = Graphics.FromImage(bmp))
			{
				ColorMatrix matrix = new ColorMatrix
				{
					Matrix33 = opacity
				};


				ImageAttributes attributes = new ImageAttributes();


				attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

				gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
				image.Dispose();
			}

			return bmp;
		}
		catch (Exception ex)
		{
			return image;
		}
	}
}