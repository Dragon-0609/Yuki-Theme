using System;
using System.Drawing;
using System.Windows;
using FastColoredTextBoxNS;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.WPF.Interfaces;
using Image = System.Drawing.Image;

namespace Yuki_Theme.Core.WPF.Windows
{
	internal class MainModel : Main.Model
	{
		
		#region Events

		public override event SetTheme SetTheme;
		public override event SetTheme StartSettingTheme;

		internal override void InvokeSetTheme()
		{
			if (SetTheme != null) SetTheme ();
		}

		internal override void InvokeStartSettingTheme()
		{
			if (StartSettingTheme != null) StartSettingTheme ();	
		}

		#endregion
		
		#region Sticker

		private StickerWindow _stickerWindow;


		internal override void InitSticker(Window window)
		{
			_stickerWindow = StickerWindow.CreateStickerControl (window);
			_stickerWindow.Show ();	
		}

		internal override void LoadSticker()
		{
			_stickerWindow.LoadImage(Sticker);
		}

		internal override void ReloadSticker()
		{
			_stickerWindow.LoadSticker();
		}

		internal override void ChangeSticker(Image image)
		{
			_stickerWindow.LoadImage(image);
		}

		internal override void ResetStickerPosition()
		{
			_stickerWindow.SetStickerSize ();
			_stickerWindow.ResetPosition ();
		}

		#endregion

		#region Wallpaper

		internal override void CalculateWallpaperSize(Rectangle rectangle)
		{
			if (CalculatedWallpaperSize.Width != rectangle.Width || CalculatedWallpaperSize.Height != rectangle.Height)
			{
				CalculatedWallpaperSize = Helper.GetSizes (WallpaperRender.Size, rectangle.Width, rectangle.Height,
					CentralAPI.Current.currentTheme.align);
			}
		}

		internal override bool CanDrawWallpaper() => WallpaperRender != null && Settings.bgImage;

		internal override void ChangeWallpaperOpacity()
		{
			WallpaperRender = WallpaperOriginal != null ? Helper.SetOpacity (WallpaperOriginal, API.CentralAPI.Current.currentTheme.WallpaperOpacity) : null;
		}
		

		#endregion
		
		#region Highlighter

		internal override void InitHighlighter(FastColoredTextBox box) => highlighter = new Highlighter(box);

		internal override void InitializeSyntax() => highlighter.InitializeSyntax ();
		internal override void UpdateSyntaxColors() => highlighter.UpdateColors();

		internal override void ActivateSyntaxColors(string item)
		{
			highlighter.activateColors (item);
		}

		#endregion

		internal override void ChangeProductMode(ProductMode mode, Action ifPlugin)
		{
			if (Helper.mode == null)
				Helper.mode = mode;
			else if (Helper.mode == ProductMode.Plugin)
				ifPlugin();
		}

	}
}