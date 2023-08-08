using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
using ICSharpCode.TextEditor.Document;
using VisualPascalABC;
using VisualPascalABCPlugins;

namespace YukiTheme.Engine
{
	// ReSharper disable once InconsistentNaming
	public class IDEAlterer
	{
		public static IDEAlterer Alterer;

		public IWorkbench Workbench;
		public Form1 Form1;
		public ColorChanger ColorChanger;
		private EditorAlterer _editorAlterer;
		private ImageChanger _imageChanger;
		private WallpaperManager _wallpaperManager;
		private StickerManager _stickerManager;
		private Timer _loadingTimer;

		public IDEAlterer(IWorkbench workbench)
		{
			Alterer = this;
			Workbench = workbench;
			Form1 = (Form1)Workbench.MainForm;
			ColorChanger = new ColorChanger();
			_editorAlterer = new EditorAlterer();
			_imageChanger = new ImageChanger();
			_wallpaperManager = new WallpaperManager();
			_stickerManager = new StickerManager();
		}

		public void Init()
		{
			StartLoadingTimer();
			_editorAlterer.GetComponents();
			/*_editorAlterer.AddMenuItems();*/
			ColorChanger.GetColors();
			_imageChanger.LoadImages();
			_imageChanger.ApplyImages();
			_editorAlterer.AlterMenu();
			_editorAlterer.SubscribeComponents();
			_editorAlterer.ChangeStyles();
			ColorChanger.UpdateColors();
			_wallpaperManager.Show();
			_stickerManager.Show();
		}

		void StartLoadingTimer()
		{
			_loadingTimer = new Timer { Interval = 100 };
			_loadingTimer.Tick += Load;
			_loadingTimer.Start();
		}

		private void Load(object sender, EventArgs e)
		{
			_loadingTimer.Stop();
			_editorAlterer.AddMenuItems();
		}

		private void Reload()
		{
			HighlightingManager.Manager.ReloadSyntaxModes();
			ColorChanger.GetColors();
			ColorChanger.UpdateColors();
			_imageChanger.LoadImages();
			_imageChanger.ApplyImages();
			_wallpaperManager.UpdateWallpaper();
			_stickerManager.UpdateSticker();
		}


		public static bool HasWallpaper => Alterer._imageChanger.HasImage;
		public static bool HasSticker => Alterer._imageChanger.HasSticker;

		public static Image GetWallpaper => Alterer._imageChanger.GetWallpaper;
		public static Image GetSticker => Alterer._imageChanger.GetSticker;

		public static ImageSource GetWallpaperWPF => Alterer._imageChanger.GetWallpaperWPF;
		public static ImageSource GetStickerWPF => Alterer._imageChanger.GetStickerWPF;
	}
}