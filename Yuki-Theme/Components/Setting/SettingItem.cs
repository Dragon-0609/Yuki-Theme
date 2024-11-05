using System;
using System.Windows.Controls;
using YukiTheme.Tools;

namespace YukiTheme.Components.Setting;

public class SettingItem
{
	private Control _control;
	private int _settingsKey;

	public SettingItem(Control control, int key)
	{
		_control = control;
		_settingsKey = key;
	}

	public void Load()
	{
		if (_control is CheckBox checkBox)
		{
			checkBox.IsChecked = DatabaseManager.Load(_settingsKey, false);
		}

		if (_control is ComboBox comboBox)
		{
			comboBox.SelectedIndex = DatabaseManager.Load(_settingsKey, 0);
		}

		if (_control is IntegerUpDown integerUpDown)
		{
			integerUpDown.box.Text = DatabaseManager.Load(_settingsKey, 0).ToString();
		}
	}

	public void Save()
	{
		if (_control is CheckBox checkBox)
		{
			DatabaseManager.SaveOptimized(_settingsKey, checkBox.IsChecked == true);
		}

		if (_control is ComboBox comboBox)
		{
			DatabaseManager.SaveOptimized(_settingsKey, comboBox.SelectedIndex);
		}

		if (_control is IntegerUpDown integerUpDown)
		{
			DatabaseManager.SaveOptimized(_settingsKey, integerUpDown.GetNumber());
		}
	}

	private void GetLoading()
	{
	}
}

public static class SettingItemHelper
{
	public static SettingItem ConvertToItem<T>(this T checkBox, int key) where T : Control
	{
		return new SettingItem(checkBox, key);
	}
}