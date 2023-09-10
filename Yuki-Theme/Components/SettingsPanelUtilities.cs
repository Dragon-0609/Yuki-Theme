using System.Windows.Controls;
using YukiTheme.Components;
using YukiTheme.Tools;

namespace Yuki_Theme.Core.WPF.Controls
{
	public class SettingsPanelUtilities
	{
		private SettingsPanel _settingsPanel;

		public SettingsPanelUtilities(SettingsPanel panel)
		{
			_settingsPanel = panel;
		}

		public void LoadSettings()
		{
			FillGeneralValues();
		}


		public void FillGeneralValues()
		{
			LoadCheckBox(SettingsConst.STICKER, _settingsPanel.ShowSticker);
			LoadCheckBox(SettingsConst.HIDE_ON_HOVER, _settingsPanel.HideHover);
			LoadCheckBox(SettingsConst.ALLOW_POSITIONING, _settingsPanel.AllowPositioning);
			// SCheck (_settingsPanel.EditorMode, Settings.Editor);
			// SDrop (_settingsPanel.EditorModeDropdown, (int)Settings.settingMode);
			// SText (_settingsPanel.DimensionCapMax.box, Settings.dimensionCapMax.ToString ());
			// SRadio (_settingsPanel.WinformsPicker, firstSelected);
			// LanguageDropdown, Settings.localization
		}

		public void SaveSettings()
		{
			SaveCheckBox(SettingsConst.STICKER, _settingsPanel.ShowSticker);
			SaveCheckBox(SettingsConst.HIDE_ON_HOVER, _settingsPanel.HideHover);
			SaveCheckBox(SettingsConst.ALLOW_POSITIONING, _settingsPanel.AllowPositioning);
		}

		public void ResetStickerMargin()
		{
			DatabaseManager.Save(SettingsConst.STICKER_POSITION, "");
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


		private void SaveTextBox(TextBox textBox, ref string value)
		{
			value = textBox.Text;
		}


		private void SDrop(ComboBox dropDown, int value)
		{
			dropDown.SelectedIndex = value;
		}


		private void LoadCheckBox(int key, CheckBox checkBox)
		{
			checkBox.IsChecked = DatabaseManager.Load(key, false);
		}


		private void LoadRadioButton(RadioButton radioButton, bool value)
		{
			radioButton.IsChecked = value;
		}


		private void LoadTextBox(TextBox textBox, string value)
		{
			textBox.Text = value;
		}

		#endregion
	}
}