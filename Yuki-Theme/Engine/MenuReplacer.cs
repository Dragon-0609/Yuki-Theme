using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class MenuReplacer
{
	private ToolStripMenuItem _menuSettings;
	private MenuStrip _menu;
	private Timer _loadingTimer;

	public void AddMenuItemsWithDelay(MenuStrip menu)
	{
		_menu = menu;
		StartLoadingTimer();
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
		AddMenuItems();
	}

	private void AddMenuItems()
	{
		_menuSettings = null;
		foreach (ToolStripItem menuItem in _menu.Items)
		{
			if (menuItem is ToolStripMenuItem)
			{
				foreach (ToolStripItem toolStripMenuItem in ((ToolStripMenuItem)menuItem).DropDownItems)
				{
					if (toolStripMenuItem is ToolStripMenuItem)
					{
						if (toolStripMenuItem.Text == "Yuki Theme")
						{
							_menuSettings = (ToolStripMenuItem)toolStripMenuItem;
#if LOG
                            Console.WriteLine("Found");
#endif
							break;
						}
					}
				}
			}
		}

		if (_menuSettings != null) // If we could find...
		{
#if LOG
            Console.WriteLine("Adding settings");
#endif

			_menuSettings.Text = "Show Settings";
			_menuSettings.ShortcutKeys = Keys.Alt | Keys.S;
			_menuSettings.ShortcutKeyDisplayString = "Alt + S";
			_menuSettings.Click += ShowSettings;

			ToolStrip ow = _menuSettings.Owner;
			ow.Items.Remove(_menuSettings);

			// Image icon = Helper.GetYukiThemeIconImage(new Size(32, 32));

			ToolStripMenuItem main = new("Yuki Theme", _menuSettings.Image);

			var quiet = CreateMenuItem("Toggle Discreet Mode", ToggleQuiet, Keys.Alt | Keys.A, "Alt + A");

			var stick = CreateMenuItem("Enable Stickers", ToggleSticker);
			var backimage = CreateMenuItem("Enable Wallpaper", ToggleWallpaper);

			var switchTheme = CreateMenuItem("Switch Theme", SwitchTheme, Keys.Control | Keys.Oemtilde, "Ctrl + `");

			var enablePositioning = CreateMenuItem("Enable Stickers Positioning", StickersPositioning);

			var resetStickerPosition =
				CreateMenuItem("Reset Sticker Margins", ResetStickerPosition);
			var updatePage = CreateMenuItem("Show Update Notification", ShowUpdatePage);

			main.DropDownItems.Add(stick);
			main.DropDownItems.Add(backimage);
			main.DropDownItems.Add(enablePositioning);
			main.DropDownItems.Add(quiet);
			main.DropDownItems.Add(switchTheme);
			main.DropDownItems.Add(_menuSettings);
			main.DropDownItems.Add(resetStickerPosition);
			main.DropDownItems.Add(updatePage);
			MoveIconToTop(ow, main);
		}
	}

	private static void MoveIconToTop(ToolStrip ow, ToolStripMenuItem main)
	{
		List<ToolStripItem> coll = new();
		foreach (ToolStripItem item in ow.Items)
		{
			coll.Add(item);
		}

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
#if LOG
        Console.WriteLine("Changing wallpaper");
#endif
		DatabaseManager.Save(SettingsConst.BG_IMAGE, !IDEAlterer.IsWallpaperVisible);
		IDEAlterer.Instance.UpdateWallpaperVisibility();
		IDEAlterer.Instance.FocusEditorWindow();
	}

	private void ToggleSticker(object sender, EventArgs e)
	{
#if LOG
        Console.WriteLine("Changing sticker");
#endif
		DatabaseManager.Save(SettingsConst.STICKER, !IDEAlterer.IsStickerVisible);
		IDEAlterer.Instance.UpdateStickerVisibility();
		IDEAlterer.Instance.FocusEditorWindow();
	}

	private void ToggleQuiet(object sender, EventArgs e)
	{
#if LOG
        Console.WriteLine("Toggling discrete");
#endif
		DatabaseManager.Save(SettingsConst.DISCRETE_MODE, !IDEAlterer.IsDiscreteActive);
		IDEAlterer.Instance.UpdateWallpaperVisibility();
		IDEAlterer.Instance.UpdateStickerVisibility();
		IDEAlterer.Instance.FocusEditorWindow();
	}

	private void StickersPositioning(object sender, EventArgs e)
	{
#if LOG
        Console.WriteLine("Changing positioning");
#endif
	}

	private void ResetStickerPosition(object sender, EventArgs e)
	{
#if LOG
        Console.WriteLine("resetting position");
#endif
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
		ToolStripMenuItem item = CreateMenuItem(text, handler);
		item.ShortcutKeys = shortcut;
		item.ShortcutKeyDisplayString = shortcutDisplay;
		return item;
	}
}