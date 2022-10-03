using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using Yuki_Theme.Core.API;

namespace Yuki_Theme.Core.WPF.Controls
{
	public class SettingsPanelUtilities
	{
		private SettingsPanel _settingsPanel;
		
		public SettingsPanelUtilities (SettingsPanel panel)
		{
			_settingsPanel = panel;
		}

		public void LoadSettings (SettingsPanel settingsPanel)
		{
			settingsPanel.HideFields ();
			FillGeneralValues ();
			FillProgramValues ();
			FillPluginValues ();
			settingsPanel.StickerDimensionCap_CheckedChanged (settingsPanel, null);
			settingsPanel.AllowPositioningCheckedChanged(this, null);
			settingsPanel.HideHover_CheckedChanged (this, null);
		}
		

		public void FillGeneralValues ()
		{
			SCheck (_settingsPanel.EditorMode, Settings.Editor);
			SCheck (_settingsPanel.ShowSticker, Settings.swSticker);
			SCheck (_settingsPanel.AllowPositioning, Settings.positioning);
			SCheck (_settingsPanel.CustomSticker, Settings.useCustomSticker);
			SCheck (_settingsPanel.ShowBackgroundImage, Settings.bgImage);
			SCheck (_settingsPanel.AutoFit, Settings.autoFitByWidth);
			SCheck (_settingsPanel.AlwaysAsk, Settings.askToSave);
			SCheck (_settingsPanel.AutoUpdate, Settings.update);
			SCheck (_settingsPanel.CheckBeta, Settings.Beta);
			SCheck (_settingsPanel.StickerDimensionCap, Settings.useDimensionCap);
			SCheck (_settingsPanel.HideHover, Settings.hideOnHover);
			SCheck (_settingsPanel.PortableMode, Settings.portableMode);
			SCheck (_settingsPanel.EditorReadOnly, Settings.editorReadOnly);
			SDrop (_settingsPanel.EditorModeDropdown, (int)Settings.settingMode);
			SDrop (_settingsPanel.DimensionCapBy, Settings.dimensionCapUnit);
			SDrop(_settingsPanel.PositioningUnit, (int) Settings.unit);
			SText (_settingsPanel.DimensionCapMax.box, Settings.dimensionCapMax.ToString ());
			SText (_settingsPanel.HideDelay.box, Settings.hideDelay.ToString ());
			_settingsPanel.customStickerPath = Settings.customSticker;
			bool firstSelected = Settings.colorPicker == 0;
			SRadio (_settingsPanel.WinformsPicker, firstSelected);
			SRadio (_settingsPanel.WPFPicker, !firstSelected);
			_settingsPanel.portable = Settings.portableMode;
			// LanguageDropdown, Settings.localization
		}
		
		
		public void FillProgramValues ()
		{
			bool isProgram = Helper.mode == ProductMode.Program;
			HideHeader (isProgram);
			if (isProgram)
			{
				SText (_settingsPanel.PascalPath, Settings.pascalPath);
				SCheck (_settingsPanel.AskOthers, Settings.askChoice);
				SDrop (_settingsPanel.ActionDropdown, Settings.actionChoice);
			}
		}

		public void FillPluginValues ()
		{
			bool isPlugin = Helper.mode == ProductMode.Plugin;
			HideHeader (!isPlugin);
			if (isPlugin)
			{
				SCheck (_settingsPanel.LogoStart, Settings.swLogo);
				SCheck (_settingsPanel.NameStatusBar, Settings.swStatusbar);
				SCheck (_settingsPanel.Preview, Settings.showPreview);
			}
		}

		private void HideHeader (bool isProgram)
		{
			_settingsPanel.ProgramAdd.Visibility = isProgram ? Visibility.Visible : Visibility.Collapsed;
			_settingsPanel.PluginAdd.Visibility = _settingsPanel.PluginTool.Visibility = isProgram ? Visibility.Collapsed : Visibility.Visible;
		}

		public void SaveSettings ()
		{
			SaveGeneralSettings ();
			SaveProgramSettings ();
			SavePluginSettings ();
			
			Settings.SaveData ();
			if (_settingsPanel.IsCommonAPI)
			{
				
			}else if (Helper.mode == ProductMode.Plugin && API_Events.saveToolBarData != null)
				API_Events.saveToolBarData();
		}

		private void SaveGeneralSettings ()
		{
			KCheck (_settingsPanel.EditorMode, ref Settings.Editor);
			KCheck (_settingsPanel.ShowSticker, ref Settings.swSticker);
			KCheck (_settingsPanel.AllowPositioning, ref Settings.positioning);
			KCheck (_settingsPanel.CustomSticker, ref Settings.useCustomSticker);
			KCheck (_settingsPanel.ShowBackgroundImage, ref Settings.bgImage);
			KCheck (_settingsPanel.AutoFit, ref Settings.autoFitByWidth);
			KCheck (_settingsPanel.AlwaysAsk, ref Settings.askToSave);
			KCheck (_settingsPanel.AutoUpdate, ref Settings.update);
			KCheck (_settingsPanel.CheckBeta, ref Settings.Beta);
			KCheck (_settingsPanel.CheckBeta, ref Settings.Beta);
			KCheck (_settingsPanel.StickerDimensionCap, ref Settings.useDimensionCap);
			KCheck (_settingsPanel.HideHover, ref Settings.hideOnHover);
			KCheck (_settingsPanel.PortableMode, ref Settings.portableMode);
			KCheck (_settingsPanel.EditorReadOnly, ref Settings.editorReadOnly);
			KDrop (_settingsPanel.DimensionCapBy, ref Settings.dimensionCapUnit);
			KDrop (_settingsPanel.PositioningUnit, ref Settings.unit);

			Settings.dimensionCapMax = _settingsPanel.DimensionCapMax.GetNumber ();
			Settings.settingMode = (SettingMode)_settingsPanel.EditorModeDropdown.SelectedIndex;
			Settings.customSticker = _settingsPanel.customStickerPath;
			Settings.colorPicker = _settingsPanel.WinformsPicker.IsChecked == true ? 0 : 1;
			Settings.hideDelay = _settingsPanel.HideDelay.GetNumber ();

			if (_settingsPanel.portable != _settingsPanel.PortableMode.IsChecked)
				Settings.database.SwapDatabase ();
		}

		private void SaveProgramSettings ()
		{
			bool isProgram = Helper.mode == ProductMode.Program;
			if (isProgram)
			{
				KCheck (_settingsPanel.AskOthers, ref Settings.askChoice);
				KDrop (_settingsPanel.ActionDropdown, ref Settings.actionChoice);
				if (_settingsPanel.IsPascalDirectory (_settingsPanel.PascalPath.Text))
					KText (_settingsPanel.PascalPath, ref Settings.pascalPath);
				else
				{
					MessageBox.Show (CentralAPI.Current.Translate ("messages.path.wrong.restore", _settingsPanel.PascalPath.Text));
					if (!Directory.Exists (Settings.pascalPath))
						Settings.pascalPath = "";
				}
			}
		}

		private void SavePluginSettings ()
		{
			bool isPlugin = Helper.mode == ProductMode.Plugin;
			if (isPlugin)
			{
				KCheck (_settingsPanel.LogoStart, ref Settings.swLogo);
				KCheck (_settingsPanel.NameStatusBar, ref Settings.swStatusbar);
				KCheck (_settingsPanel.Preview, ref Settings.showPreview);
			}
		}

		
		
		#region Helper Methods

		/// <summary>
		/// Save Checkbox.IsChecked to bool
		/// </summary>
		/// <param name="checkBox">Checkbox to save from</param>
		/// <param name="target">Target to set the bool</param>
		private void KCheck (CheckBox checkBox, ref bool target)
		{
			target = checkBox.IsChecked == true;
		}

		/// <summary>
		/// Save Selected Index of combobox to int
		/// </summary>
		private void KDrop (ComboBox dropDown, ref int value)
		{
			value = dropDown.SelectedIndex;
		}

		/// <summary>
		/// Save Selected Index of combobox to int
		/// </summary>
		private void KDrop (ComboBox dropDown, ref RelativeUnit value)
		{
			value = (RelativeUnit) dropDown.SelectedIndex;
		}

		/// <summary>
		/// Save Text of textbox to string
		/// </summary>
		private void KText (TextBox textBox, ref string value)
		{
			value = textBox.Text;
		}

		/// <summary>
		/// Set value of combobox
		/// </summary>
		private void SDrop (ComboBox dropDown, int value)
		{
			dropDown.SelectedIndex = value;
		}

		/// <summary>
		/// Set value of checkbox
		/// </summary>
		private void SCheck (CheckBox checkBox, bool value)
		{
			checkBox.IsChecked = value;
		}

		/// <summary>
		/// Set value of radiobutton
		/// </summary>
		private void SRadio (RadioButton radioButton, bool value)
		{
			radioButton.IsChecked = value;
		}

		/// <summary>
		/// Set value of textbox
		/// </summary>
		private void SText (TextBox textBox, string value)
		{
			textBox.Text = value;
		}

		#endregion

	}
}