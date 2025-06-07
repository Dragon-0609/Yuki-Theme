using System;
using System.Windows.Controls;
using YukiTheme.Tools;

namespace YukiTheme.Components.Setting;

public class SettingItem
{
	private Control _control;
	private int _settingsKey;
	private int _defaultValue = 0;

	public SettingItem(Control control, int key)
	{
		_control = control;
		_settingsKey = key;
	}

	public SettingItem(Control control, int key, int defaultValue)
	{
		_control = control;
		_settingsKey = key;
		_defaultValue = defaultValue;
	}

	public void Load()
	{
		if (_control is CheckBox checkBox)
		{
			checkBox.IsChecked = DataSaver.Load(_settingsKey, false);
		}

		if (_control is ComboBox comboBox)
		{
			comboBox.SelectedIndex = DataSaver.Load(_settingsKey, _defaultValue);
		}

		if (_control is IntegerUpDown integerUpDown)
		{
			integerUpDown.box.Text = DataSaver.Load(_settingsKey, 0).ToString();
		}
	}

	public void Save()
	{
		if (_control is CheckBox checkBox)
		{
			DataSaver.SaveOptimized(_settingsKey, checkBox.IsChecked == true);
		}

		if (_control is ComboBox comboBox)
		{
			DataSaver.SaveOptimized(_settingsKey, comboBox.SelectedIndex);
		}

		if (_control is IntegerUpDown integerUpDown)
		{
			DataSaver.SaveOptimized(_settingsKey, integerUpDown.GetNumber());
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

	public static SettingItem ConvertToItem<T>(this T checkBox, int key, int defaultValue) where T : Control
	{
		return new SettingItem(checkBox, key, defaultValue);
	}
}