using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class MenuReplacer
{
	private const string QUIT_KEY = "quiet";
	private const string STICKER_KEY = "heart";
	private const string WALLPAPER_KEY = "wallpaper";
	private const string SWITCH_KEY = "switch";
	private const string POSITIONING_KEY = "positioning";
	private ToolStripMenuItem _backimage;

	private readonly Dictionary<string, Image> _baseIcons = new();
	private ToolStripMenuItem _enablePositioning;

	private readonly Dictionary<string, Func<Image, Image>> _getUpdatedIcon = new();
	private Timer _loadingTimer;
	private MenuStrip _menu;
	private ToolStripMenuItem _menuSettings;
	private ToolStripMenuItem _quiet;
	private ToolStripMenuItem _resetStickerPosition;
	private ToolStripMenuItem _stick;
	private ToolStripMenuItem _switchTheme;
	private ToolStripMenuItem _updatePage;

	internal void AddMenuItemsWithDelay(MenuStrip menu)
	{
		_menu = menu;
		AddUpdateIconCallbacks();
		StartLoadingTimer();
	}

	private void AddUpdateIconCallbacks()
	{
		_getUpdatedIcon.Add(QUIT_KEY, image => GetIconWithBackground(image, IDEAlterer.IsDiscreteActive));
		_getUpdatedIcon.Add(WALLPAPER_KEY, image => GetIconWithBackground(image, IDEAlterer.IsWallpaperVisible));
		_getUpdatedIcon.Add(STICKER_KEY, image => GetIconWithBackground(image, IDEAlterer.IsStickerVisible));
	}

	private void StartLoadingTimer()
	{
		_loadingTimer = new Timer { Interval = 100 };
		_loadingTimer.Tick += Load;
		_loadingTimer.Start();
	}

	private void Load(object sender, EventArgs e)
	{
		_loadingTimer.Stop();
		ColorChanger.Instance.UpdatedColors += UpdateMenuIcons;
		AddMenuItems();
	}

	private void AddMenuItems()
	{
		FindPluginItem();

		if (_menuSettings != null) // If we could find...
		{
#if LOG
            Console.WriteLine("Adding settings");
#endif

			_menuSettings.Text = "Show Settings";
			_menuSettings.ShortcutKeys = Keys.Alt | Keys.S;
			_menuSettings.ShortcutKeyDisplayString = "Alt + S";
			_menuSettings.Click += ShowSettings;

			var ow = _menuSettings.Owner;
			ow.Items.Remove(_menuSettings);

			// Image icon = Helper.GetYukiThemeIconImage(new Size(32, 32));

			ToolStripMenuItem main = new("Yuki Theme", _menuSettings.Image);

			_quiet = CreateMenuItem("Toggle Discreet Mode", ToggleQuiet, Keys.Alt | Keys.A, "Alt + A");

			_stick = CreateMenuItem("Enable Stickers", ToggleSticker);
			_backimage = CreateMenuItem("Enable Wallpaper", ToggleWallpaper);

			_switchTheme = CreateMenuItem("Switch Theme", SwitchTheme, Keys.Control | Keys.Oemtilde, "Ctrl + `");

			_enablePositioning = CreateMenuItem("Enable Stickers Positioning", StickersPositioning);

			_resetStickerPosition = CreateMenuItem("Reset Sticker Margins", ResetStickerPosition);
			_updatePage = CreateMenuItem("Show Update Notification", ShowUpdatePage);

			UpdateMenuIcons();
			_updatePage.Image = _menuSettings.Image;

			main.DropDownItems.Add(_stick);
			main.DropDownItems.Add(_backimage);
			main.DropDownItems.Add(_enablePositioning);
			main.DropDownItems.Add(_quiet);
			main.DropDownItems.Add(_switchTheme);
			main.DropDownItems.Add(_menuSettings);
			main.DropDownItems.Add(_resetStickerPosition);
			main.DropDownItems.Add(_updatePage);
			MoveIconToTop(ow, main);
		}
	}

	private void UpdateMenuIcons()
	{
		UpdateMenuIcon(QUIT_KEY, true, _quiet);
		UpdateMenuIcon(STICKER_KEY, true, _stick);
		UpdateMenuIcon(WALLPAPER_KEY, true, _backimage);
		UpdateMenuIcon(SWITCH_KEY, false, _switchTheme, _resetStickerPosition);
		UpdateMenuIcon(POSITIONING_KEY, false, _enablePositioning);
	}

	private void UpdateMenuIcon(string icon, bool canBeUpdated, ToolStripMenuItem item1, ToolStripMenuItem item2 = null)
	{
		var image = LoadMenuIcon(icon, ColorReference.BorderColor);
		if (canBeUpdated) image = _getUpdatedIcon[icon](image);

		CleanMenuItemIcon(item1);
		CleanMenuItemIcon(item2);
		item1.Image = image;
		if (item2 != null) item2.Image = image;
	}

	private static void CleanMenuItemIcon(ToolStripMenuItem item1)
	{
		if (item1 is { Image: { } }) item1.Image.Dispose();
	}

	private void FindPluginItem()
	{
		_menuSettings = null;
		foreach (ToolStripItem menuItem in _menu.Items)
			if (menuItem is ToolStripMenuItem)
				foreach (ToolStripItem toolStripMenuItem in ((ToolStripMenuItem)menuItem).DropDownItems)
					if (toolStripMenuItem is ToolStripMenuItem)
						if (toolStripMenuItem.Text == "Yuki Theme")
						{
							_menuSettings = (ToolStripMenuItem)toolStripMenuItem;
#if LOG
                            Console.WriteLine("Found");
#endif
							break;
						}
	}

	private static void MoveIconToTop(ToolStrip ow, ToolStripMenuItem main)
	{
		List<ToolStripItem> coll = new();
		foreach (ToolStripItem item in ow.Items) coll.Add(item);

		ow.Items.Clear();
		if (coll.Last() is ToolStripSeparator)
			coll.Remove(coll.Last());

		ow.Items.Add(main);
		ow.Items.Add(new ToolStripSeparator());

		ow.Items.AddRange(coll.ToArray());
	}

	private void SwitchTheme(object sender, EventArgs e)
	{
#if LOG
        Console.WriteLine("Switching theme");
#endif
		IDEAlterer.Instance.ShowThemeSelect();
	}

	private void ToggleWallpaper(object sender, EventArgs e)
	{
		DatabaseManager.Save(SettingsConst.BG_IMAGE, !IDEAlterer.IsWallpaperVisible);
		IDEAlterer.Instance.UpdateWallpaperVisibility();
		IDEAlterer.Instance.FocusEditorWindow();
		UpdateMenuIcon(WALLPAPER_KEY, true, _backimage);
	}

	private void ToggleSticker(object sender, EventArgs e)
	{
		DatabaseManager.Save(SettingsConst.STICKER, !IDEAlterer.IsStickerVisible);
		IDEAlterer.Instance.UpdateStickerVisibility();
		IDEAlterer.Instance.FocusEditorWindow();
		UpdateMenuIcon(STICKER_KEY, true, _stick);
	}

	private void ToggleQuiet(object sender, EventArgs e)
	{
		DatabaseManager.Save(SettingsConst.DISCRETE_MODE, !IDEAlterer.IsDiscreteActive);
		IDEAlterer.Instance.UpdateWallpaperVisibility();
		IDEAlterer.Instance.UpdateStickerVisibility();
		IDEAlterer.Instance.FocusEditorWindow();
		UpdateMenuIcon(QUIT_KEY, true, _quiet);
	}

	private void StickersPositioning(object sender, EventArgs e)
	{
		DatabaseManager.Save(SettingsConst.ALLOW_POSITIONING, DatabaseManager.Load(SettingsConst.ALLOW_POSITIONING, false));
	}

	private void ResetStickerPosition(object sender, EventArgs e)
	{
		DatabaseManager.Save(SettingsConst.STICKER_POSITION, "");
		PluginEvents.Instance.OnStickerMarginReset();
	}

	private void ShowSettings(object sender, EventArgs e)
	{
		OptionsChanger.ShowSettings();
	}

	private void ShowUpdatePage(object sender, EventArgs e)
	{
#if LOG
        Console.WriteLine("Showing update");
#endif
	}

	private Image GetIconWithBackground(Image image, bool showBg)
	{
		if (!showBg) return (Image)image.Clone();
		var newImage = (Bitmap)image.Clone();
		using (var g = Graphics.FromImage(newImage))
		{
			g.Clear(Color.FromArgb(90, ColorReference.BorderColor));
			g.DrawImage(image, new Point(0, 0));
		}

		return newImage;
	}


	private ToolStripMenuItem CreateMenuItem(string text, EventHandler handler)
	{
		var item = new ToolStripMenuItem(text, null, handler)
		{
			BackColor = _menuSettings.BackColor, ForeColor = _menuSettings.ForeColor
		};

		return item;
	}

	private ToolStripMenuItem CreateMenuItem(string text, EventHandler handler, Keys shortcut,
		string shortcutDisplay)
	{
		var item = CreateMenuItem(text, handler);
		item.ShortcutKeys = shortcut;
		item.ShortcutKeyDisplayString = shortcutDisplay;
		return item;
	}

	private Image LoadMenuIcon(string icon, Color color = default)
	{
		if (_baseIcons.ContainsKey(icon) && _baseIcons[icon] != null)
		{
			_baseIcons[icon].Dispose();
			_baseIcons[icon] = null;
		}

		var document = SvgRenderer.LoadSvg(icon, "Icons.");
		var context = new SvgRenderInfo(document, false, Size.Empty, color != default, color == default ? Color.Red : color);
		var image = SvgRenderer.RenderSvg(new Size(32, 32), context);
		_baseIcons[icon] = image;
		return image;
	}
}