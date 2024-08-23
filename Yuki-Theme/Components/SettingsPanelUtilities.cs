using System.Windows.Controls;
using YukiTheme.Components;
using YukiTheme.Engine;
using YukiTheme.Tools;

namespace Yuki_Theme.Core.WPF.Controls;

public class SettingsPanelUtilities
{
	private readonly SettingsPanel _settingsPanel;

	public SettingsPanelUtilities(SettingsPanel panel)
	{
		_settingsPanel = panel;
	}

	internal void LoadSettings()
	{
		FillGeneralValues();
	}


	private void FillGeneralValues()
	{
		LoadCheckBox(SettingsConst.STICKER, _settingsPanel.ShowSticker);
		LoadCheckBox(SettingsConst.HIDE_ON_HOVER, _settingsPanel.HideHover);
		LoadCheckBox(SettingsConst.ALLOW_POSITIONING, _settingsPanel.AllowPositioning);
		LoadCheckBox(SettingsConst.USE_DIMENSION_CAP, _settingsPanel.StickerDimensionCap);
		LoadNumberBox(SettingsConst.DIMENSION_CAP_MAX, _settingsPanel.DimensionCapMax);
		LoadDropDown(SettingsConst.DIMENSION_CAP_UNIT, _settingsPanel.DimensionCapBy);
		// SCheck (_settingsPanel.EditorMode, Settings.Editor);
		// SDrop (_settingsPanel.EditorModeDropdown, (int)Settings.settingMode);
		// SRadio (_settingsPanel.WinformsPicker, firstSelected);
		// LanguageDropdown, Settings.localization
	}

	internal void SaveSettings()
	{
		SaveCheckBox(SettingsConst.STICKER, _settingsPanel.ShowSticker);
		SaveCheckBox(SettingsConst.HIDE_ON_HOVER, _settingsPanel.HideHover);
		SaveCheckBox(SettingsConst.ALLOW_POSITIONING, _settingsPanel.AllowPositioning);
		SaveCheckBox(SettingsConst.USE_DIMENSION_CAP, _settingsPanel.StickerDimensionCap);
		SaveNumberBox(SettingsConst.DIMENSION_CAP_MAX, _settingsPanel.DimensionCapMax.GetNumber());
		SaveDropDown(SettingsConst.DIMENSION_CAP_UNIT, _settingsPanel.DimensionCapBy);
	}

	internal void ResetStickerMargin()
	{
		DatabaseManager.Save(SettingsConst.STICKER_POSITION, "");
		PluginEvents.Instance.OnStickerMarginReset();
	}

	private void SaveGeneralSettings()
	{
		// KCheck(_settingsPanel.EditorMode, ref Settings.Editor);
		// KDrop(_settingsPanel.DimensionCapBy, ref Settings.dimensionCapUnit);
	}

	private void SaveProgramSettings()
	{
		// KText(_settingsPanel.PascalPath, ref Settings.pascalPath);
	}

	#region Helper Methods

	private void SaveCheckBox(int key, CheckBox checkBox)
	{
		DatabaseManager.Save(key, checkBox.IsChecked == true);
	}


	private void SaveDropDown(ComboBox dropDown, ref int value)
	{
		value = dropDown.SelectedIndex;
	}


	private void SaveNumberBox(int key, int value)
	{
		DatabaseManager.Save(key, value);
	}


	private void SaveDropDown(int key, ComboBox dropDown)
	{
		DatabaseManager.Save(key, dropDown.SelectedIndex);
	}


	private void LoadCheckBox(int key, CheckBox checkBox)
	{
		checkBox.IsChecked = DatabaseManager.Load(key, false);
	}


	private void LoadRadioButton(RadioButton radioButton, bool value)
	{
		radioButton.IsChecked = value;
	}


	private void LoadNumberBox(int key, IntegerUpDown textBox)
	{
		textBox.box.Text = DatabaseManager.Load(key, 0).ToString();
	}

	private void LoadDropDown(int key, ComboBox dropDown)
	{
		dropDown.SelectedIndex = DatabaseManager.Load(key, 0);
	}

	#endregion
}