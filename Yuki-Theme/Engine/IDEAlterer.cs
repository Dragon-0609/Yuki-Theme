using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
using ICSharpCode.TextEditor.Document;
using VisualPascalABC;
using VisualPascalABCPlugins;
using YukiTheme.Components;
using YukiTheme.Export;
using YukiTheme.Tools;

// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace YukiTheme.Engine
{
	// ReSharper disable once InconsistentNaming
	public class IDEAlterer
	{
		public static IDEAlterer Instance;

		internal IWorkbench Workbench;
		internal Form1 Form1;
		private ColorChanger _colorChanger;
		private EditorAlterer _editorAlterer;
		private ImageLoader _imageLoader;
		private WallpaperManager _wallpaperManager;
		private StickerManager _stickerManager;
		private ThemeWindowManager _themeSelect;
		
		public IDEAlterer(IWorkbench workbench)
		{
			Instance = this;
			Workbench = workbench;
			Form1 = (Form1)Workbench.MainForm;
			_colorChanger = new ColorChanger();
			_editorAlterer = new EditorAlterer();
			_imageLoader = new ImageLoader();
			_wallpaperManager = new WallpaperManager();
			_stickerManager = new StickerManager();
			_themeSelect = new ThemeWindowManager();
			new DatabaseManager();
			new PluginEvents();
			ThemeNameExtractor.ListenToReload();
			StartExporter();
		}

		internal void Init()
		{
			_editorAlterer.GetComponents();
			_colorChanger.GetColors();
			_imageLoader.LoadImages();
			_imageLoader.ApplyImages();
			_editorAlterer.AlterMenu();
			_editorAlterer.SubscribeComponents();
			_editorAlterer.ChangeStyles();
			_editorAlterer.StartMenuReplacement();
			_colorChanger.UpdateColors();
			_wallpaperManager.Show();
			_stickerManager.Show();
			ListenToThemeChange();
			ThemeNameExtractor.LoadThemeInfo();
			_editorAlterer.UpdateIconColors();
		}

		private void StartExporter()
		{
			new ExportListener();
			new DokiExporter();
		}

		private void ListenToThemeChange()
		{
			PluginEvents.Instance.ThemeChanged += (name) => Reload();
		}

		internal void Reload()
		{
			HighlightingManager.Manager.ReloadSyntaxModes();
			_colorChanger.GetColors();
			_colorChanger.UpdateColors();
			_imageLoader.LoadImages();
			_imageLoader.ApplyImages();
			_wallpaperManager.UpdateWallpaper();
			_stickerManager.UpdateSticker();
			PluginEvents.Instance.OnReload();
		}

		internal void ReloadSettings()
		{
			_wallpaperManager.ReloadSettings();
			_stickerManager.ReloadSettings();
		}

		internal void UpdateWallpaperVisibility() => _wallpaperManager.UpdateVisibility();
		internal void UpdateStickerVisibility() => _stickerManager.UpdateVisibility();

		internal void RequestBottomBarUpdate() => _editorAlterer.RequestBottomBarUpdate();

		internal void FocusEditorWindow() => _editorAlterer.FocusEditorWindow();

		internal void ShowThemeSelect() => _themeSelect.Show();

		internal static bool HasWallpaper => Instance._imageLoader.HasImage;
		internal static bool HasSticker => Instance._imageLoader.HasSticker;

		internal static bool CanShowWallpaper => Instance._imageLoader.HasImage && IsWallpaperVisible && !IsDiscreteActive;

		internal static bool CanShowSticker => Instance._imageLoader.HasImage && IsStickerVisible && !IsDiscreteActive;

		internal static bool IsWallpaperVisible => DatabaseManager.Load(SettingsConst.BG_IMAGE);

		internal static bool IsStickerVisible => DatabaseManager.Load(SettingsConst.STICKER);

		internal static bool IsDiscreteActive => DatabaseManager.Load(SettingsConst.DISCRETE_MODE);

		internal static Image GetWallpaper => Instance._imageLoader.GetWallpaper;
		internal static Image GetSticker => Instance._imageLoader.GetSticker;

		internal static ImageSource GetWallpaperWPF => Instance._imageLoader.GetWallpaperWPF;
		internal static ImageSource GetStickerWPF => Instance._imageLoader.GetStickerWPF;

		internal static void ReleaseImages() => Instance._imageLoader.ReleaseImages();

		internal static void ReloadImages()
		{
			Instance._imageLoader.LoadImages();
			Instance._imageLoader.ApplyImages();
		}
	}
}