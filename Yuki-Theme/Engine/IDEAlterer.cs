using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
using ICSharpCode.TextEditor.Document;
using VisualPascalABC;
using VisualPascalABCPlugins;
using YukiTheme.Export;
using YukiTheme.Tools;

// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace YukiTheme.Engine;

// ReSharper disable once InconsistentNaming
public class IDEAlterer
{
	public static IDEAlterer Instance;
	private ColorChanger _colorChanger;
	private EditorAlterer _editorAlterer;
	private ImageLoader _imageLoader;
	private StickerManager _stickerManager;
	private ThemeWindowManager _themeSelect;
	private WallpaperManager _wallpaperManager;
	internal Form1 Form1;

	internal IWorkbench Workbench;

	public IDEAlterer(IWorkbench workbench)
	{
		Instance = this;
		Workbench = workbench;
		Form1 = (Form1)Workbench.MainForm;
		new PluginEvents();
		_colorChanger = new ColorChanger();
		_editorAlterer = new EditorAlterer();
		_imageLoader = new ImageLoader();
		_wallpaperManager = new WallpaperManager();
		_stickerManager = new StickerManager();
		_themeSelect = new ThemeWindowManager();
		new DataSaver();
		ThemeNameExtractor.ListenToReload();
		StartExporter();
	}

	internal static bool HasWallpaper => Instance._imageLoader.HasImage;
	internal static bool HasSticker => HasCurrentSticker();

	internal static bool CanShowWallpaper => Instance._imageLoader.HasImage && IsWallpaperVisible && !IsDiscreteActive;

	internal static bool CanShowSticker => Instance._imageLoader.HasImage && IsStickerVisible && !IsDiscreteActive;

	internal static bool IsWallpaperVisible => DataSaver.Load(SettingsConst.BG_IMAGE);

	internal static bool IsStickerVisible => DataSaver.Load(SettingsConst.STICKER);

	internal static bool IsDiscreteActive => DataSaver.Load(SettingsConst.DISCRETE_MODE);

	internal static Image GetWallpaper => Instance._imageLoader.GetWallpaper;
	internal static Image GetSticker => GetCurrentSticker();

	internal static ImageSource GetWallpaperWPF => Instance._imageLoader.GetWallpaperWPF;
	internal static ImageSource GetStickerWPF => GetCurrentStickerWpf();

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
		_editorAlterer.InjectToOtherPlugins();
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
		PluginEvents.Instance.ThemeChanged += name => Reload();
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

	internal void UpdateWallpaperVisibility()
	{
		_wallpaperManager.UpdateVisibility();
	}

	internal void UpdateStickerVisibility()
	{
		_stickerManager.UpdateVisibility();
	}

	internal void RequestBottomBarUpdate()
	{
		_editorAlterer.RequestBottomBarUpdate();
	}

	internal void FocusEditorWindow()
	{
		_editorAlterer.FocusEditorWindow();
	}

	internal void ShowThemeSelect()
	{
		_themeSelect.Show();
	}

	internal static void ReleaseImages()
	{
		Instance._imageLoader.ReleaseImages();
	}

	internal static void ReloadImages()
	{
		Instance._imageLoader.LoadImages();
		Instance._imageLoader.ApplyImages();
	}

	public Form GetSettingsParent() => _editorAlterer.GetSettingsParent();

	private static Image GetCurrentSticker()
	{
		bool hasCustom = DataSaver.Load(SettingsConst.USE_CUSTOM_STICKER, false) &&
		                 Instance._imageLoader.HasCustomSticker;
		return hasCustom ? Instance._imageLoader.GetCustomSticker : Instance._imageLoader.GetSticker;
	}

	private static ImageSource GetCurrentStickerWpf()
	{
		bool hasCustom = DataSaver.Load(SettingsConst.USE_CUSTOM_STICKER, false) &&
		                 Instance._imageLoader.HasCustomSticker;
		return hasCustom
			? Instance._imageLoader.GetCustomStickerWPF
			: Instance._imageLoader.GetStickerWPF;
	}

	private static bool HasCurrentSticker()
	{
		bool hasCustom = DataSaver.Load(SettingsConst.USE_CUSTOM_STICKER, false) &&
		                 Instance._imageLoader.HasCustomSticker;
		return hasCustom ? Instance._imageLoader.HasCustomSticker : Instance._imageLoader.HasSticker;
	}

	public float EditorFontSize => _editorAlterer.EditorFontSize;
}