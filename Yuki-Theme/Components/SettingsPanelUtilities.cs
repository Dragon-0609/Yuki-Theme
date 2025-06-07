using System.Collections.Generic;
using YukiTheme.Components.Setting;
using YukiTheme.Engine;
using YukiTheme.Tools;

namespace YukiTheme.Components;

public class SettingsPanelUtilities
{
	private readonly SettingsPanel _settingsPanel;
	private SettingItem[] _items;

	public SettingsPanelUtilities(SettingsPanel panel)
	{
		_settingsPanel = panel;
		InitComponents();
	}

	private void InitComponents()
	{
		List<SettingItem> items =
		[
			_settingsPanel.ShowBackground.ConvertToItem(SettingsConst.BG_IMAGE),
			_settingsPanel.CompletionFont.ConvertToItem(SettingsConst.COMPLETION_FONT, 8),
			_settingsPanel.CompletionFontSameAsEditor.ConvertToItem(SettingsConst.COMPLETION_FONT_AS_EDITOR),
			_settingsPanel.ShowSticker.ConvertToItem(SettingsConst.STICKER),
			_settingsPanel.HideHover.ConvertToItem(SettingsConst.HIDE_ON_HOVER),
			_settingsPanel.HideDelay.ConvertToItem(SettingsConst.HIDE_DELAY),
			_settingsPanel.AllowPositioning.ConvertToItem(SettingsConst.ALLOW_POSITIONING),
			_settingsPanel.StickerDimensionCap.ConvertToItem(SettingsConst.USE_DIMENSION_CAP),
			_settingsPanel.DimensionCapMax.ConvertToItem(SettingsConst.DIMENSION_CAP_MAX),
			_settingsPanel.DimensionCapBy.ConvertToItem(SettingsConst.DIMENSION_CAP_UNIT),
			_settingsPanel.CustomSticker.ConvertToItem(SettingsConst.USE_CUSTOM_STICKER),
		];

		_items = items.ToArray();
	}

	internal void LoadSettings()
	{
		FillGeneralValues();
	}


	private void FillGeneralValues()
	{
		foreach (SettingItem item in _items)
		{
			item.Load();
		}
		// LanguageDropdown, Settings.localization
	}

	internal void SaveSettings()
	{
		foreach (SettingItem item in _items)
		{
			item.Save();
		}

		DataSaver.SaveAll();
	}

	internal void ResetStickerMargin()
	{
		DataSaver.Save(SettingsConst.STICKER_POSITION, "");
		PluginEvents.Instance.OnStickerMarginReset();
	}

	public void ReLoadCustomSticker()
	{
		PluginEvents.Instance.OnCustomStickerPathChanged();
	}
}