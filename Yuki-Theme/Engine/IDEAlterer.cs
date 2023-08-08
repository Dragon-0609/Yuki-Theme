using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
using ICSharpCode.TextEditor.Document;
using VisualPascalABC;
using VisualPascalABCPlugins;
using YukiTheme.Tools;

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
            new DatabaseManager();
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

        public void UpdateWallpaperVisibility() => _wallpaperManager.UpdateVisibility();
        public void UpdateStickerVisibility() => _stickerManager.UpdateVisibility();


        public static bool HasWallpaper => Alterer._imageChanger.HasImage;
        public static bool HasSticker => Alterer._imageChanger.HasSticker;

        public static bool CanShowWallpaper => Alterer._imageChanger.HasImage && GetWallpaperVisibility();

        public static bool CanShowSticker => Alterer._imageChanger.HasImage && GetStickerVisibility();

        public static bool GetWallpaperVisibility() => DatabaseManager.Instance.Load(SettingsConst.BgImage, true);

        public static bool GetStickerVisibility() => DatabaseManager.Instance.Load(SettingsConst.Sticker, true);

        public static Image GetWallpaper => Alterer._imageChanger.GetWallpaper;
        public static Image GetSticker => Alterer._imageChanger.GetSticker;

        public static ImageSource GetWallpaperWPF => Alterer._imageChanger.GetWallpaperWPF;
        public static ImageSource GetStickerWPF => Alterer._imageChanger.GetStickerWPF;
    }
}