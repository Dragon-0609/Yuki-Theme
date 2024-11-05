using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class ImageLoader
{
	private const string ImagesFolder = "Highlighting";
	private const string PngFilter = "*.png";
	private readonly string _stickerName = "sticker";
	private readonly string _wallpaperName = "background";

	internal bool HasImage => GetWallpaper is { Height: > 10 };
	internal Image GetWallpaper { get; private set; }

	internal Image GetSticker { get; private set; }
	internal Image GetCustomSticker { get; private set; }

	internal bool HasSticker => GetSticker is { Height: > 10 };
	internal bool HasCustomSticker => GetCustomSticker != null;

	internal ImageSource GetWallpaperWPF { get; private set; }

	internal ImageSource GetStickerWPF { get; private set; }
	internal ImageSource GetCustomStickerWPF { get; private set; }

	public ImageLoader()
	{
		PluginEvents.Instance.CustomStickerPathChanged += ReLoadCustomSticker;
	}

	private void RemoveLastImage()
	{
		if (GetWallpaper != null)
		{
			GetWallpaper.Dispose();
			GetWallpaper = null;
		}

		GetWallpaperWPF = null;

		if (GetSticker != null)
		{
			GetSticker.Dispose();
			GetSticker = null;
		}

		GetStickerWPF = null;

		ReleaseCustomSticker();
	}

	internal void LoadImages()
	{
		RemoveLastImage();
		var png = Directory.GetFiles(Path.Combine(YukiTheme_VisualPascalABCPlugin.GetCurrentFolder, ImagesFolder),
			PngFilter);
		if (png.Length > 0)
		{
			var wallpaperCondition = png.Where(p => Path.GetFileNameWithoutExtension(p).ToLower() == _wallpaperName);
			string wallpaperFileName;
			string stickerFileName;
			var ordered = png.OrderByDescending(p => new FileInfo(p).Length);
			if (wallpaperCondition.Any())
				wallpaperFileName = wallpaperCondition.First();
			else
				wallpaperFileName = ordered.First();

			Console.WriteLine($"File size: {new FileInfo(wallpaperFileName).Length}");

			GetWallpaper = Image.FromFile(wallpaperFileName);

			if (png.Length > 1)
			{
				var stickerCondition = png.Where(p => Path.GetFileNameWithoutExtension(p).ToLower() == _stickerName);

				if (stickerCondition.Any())
					stickerFileName = stickerCondition.First();
				else
					stickerFileName = ordered.Skip(1).First();

				GetSticker = Image.FromFile(stickerFileName);
			}
		}

		LoadCustomSticker();
	}

	private void LoadCustomSticker()
	{
		var stickerPath = DatabaseManager.Load(SettingsConst.CUSTOM_STICKER_PATH, "");
		if (File.Exists(stickerPath))
		{
			GetCustomSticker = Image.FromFile(stickerPath);
			Console.WriteLine($"Loaded {stickerPath}");
			Console.WriteLine($"{GetCustomSticker.Width}/{GetCustomSticker.Height}");
		}
	}

	internal void ReLoadCustomSticker()
	{
		ReleaseCustomSticker();
		LoadCustomSticker();
		ApplyCustomSticker();
	}

	internal void ReleaseImages()
	{
		RemoveLastImage();
	}

	internal void ApplyImages()
	{
		if (GetWallpaper != null)
		{
			GetWallpaperWPF = GetWallpaper.ToWPFImage();
			GetStickerWPF = GetSticker.ToWPFImage();
		}

		ApplyCustomSticker();
	}

	private void ApplyCustomSticker()
	{
		if (GetCustomSticker != null)
		{
			GetCustomStickerWPF = GetCustomSticker.ToWPFImage();
		}
	}

	internal Image SetImageOpacity(Image image, float opacity)
	{
		try
		{
			var bmp = new Bitmap(image.Width, image.Height);

			opacity = opacity == 0 ? opacity : opacity / 100f;

			using (var gfx = Graphics.FromImage(bmp))
			{
				var matrix = new ColorMatrix
				{
					Matrix33 = opacity
				};


				var attributes = new ImageAttributes();


				attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

				gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height,
					GraphicsUnit.Pixel, attributes);
				image.Dispose();
			}

			return bmp;
		}
		catch
		{
			return image;
		}
	}

	private void ReleaseCustomSticker()
	{
		if (GetCustomSticker != null)
		{
			GetCustomSticker.Dispose();
			GetCustomSticker = null;
		}

		GetCustomStickerWPF = null;
	}
}