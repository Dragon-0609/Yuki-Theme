﻿using System;
using System.Windows.Interop;
using System.Windows.Media;
using YukiTheme.Style.Helpers;

namespace YukiTheme.Style.Controls
{
	public partial class WallpaperWindow : SnapWindow
	{
		public WallpaperWindow()
		{
			InitializeComponent();
		}

		public void SetImage(ImageSource source)
		{
			Wallpaper.Source = source;
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			var hwnd = new WindowInteropHelper(this).Handle;
			WindowsServices.SetWindowExTransparent(hwnd);
		}
	}
}