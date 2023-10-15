using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
using ICSharpCode.TextEditor.Document;
using VisualPascalABC;
using VisualPascalABCPlugins;
using YukiTheme.Components;
using YukiTheme.Tools;

// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace YukiTheme.Engine
{
    // ReSharper disable once InconsistentNaming
    public class IDEAlterer
    {
        public static IDEAlterer Instance;

        public IWorkbench Workbench;
        public Form1 Form1;
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
            new DokiExporter();
        }

        public void Init()
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
        }

        private void ListenToThemeChange()
        {
            PluginEvents.Instance.ThemeChanged += (name) => Reload();
        }

        public void Reload()
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

        public void ReloadSettings()
        {
            _wallpaperManager.ReloadSettings();
            _stickerManager.ReloadSettings();
        }

        public void UpdateWallpaperVisibility() => _wallpaperManager.UpdateVisibility();
        public void UpdateStickerVisibility() => _stickerManager.UpdateVisibility();

        public void RequestBottomBarUpdate() => _editorAlterer.RequestBottomBarUpdate();

        public void FocusEditorWindow() => _editorAlterer.FocusEditorWindow();

        public void ShowThemeSelect() => _themeSelect.Show();

        public static bool HasWallpaper => Instance._imageLoader.HasImage;
        public static bool HasSticker => Instance._imageLoader.HasSticker;

        public static bool CanShowWallpaper => Instance._imageLoader.HasImage && IsWallpaperVisible && !IsDiscreteActive;

        public static bool CanShowSticker => Instance._imageLoader.HasImage && IsStickerVisible && !IsDiscreteActive;

        public static bool IsWallpaperVisible => DatabaseManager.Load(SettingsConst.BG_IMAGE);

        public static bool IsStickerVisible => DatabaseManager.Load(SettingsConst.STICKER);

        public static bool IsDiscreteActive => DatabaseManager.Load(SettingsConst.DISCRETE_MODE);

        public static Image GetWallpaper => Instance._imageLoader.GetWallpaper;
        public static Image GetSticker => Instance._imageLoader.GetSticker;

        public static ImageSource GetWallpaperWPF => Instance._imageLoader.GetWallpaperWPF;
        public static ImageSource GetStickerWPF => Instance._imageLoader.GetStickerWPF;
    }
}