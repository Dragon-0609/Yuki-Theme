using System.Collections.Generic;

namespace YukiTheme.Tools;

public class DatabaseManager
{
	private static DatabaseManager _instance;

	private readonly Dictionary<int, string> _defaults = new()
	{
		{ SettingsConst.AUTO_UPDATE, true.ToInt().ToString() },
		{ SettingsConst.BG_IMAGE, true.ToInt().ToString() },
		{ SettingsConst.STICKER, true.ToInt().ToString() },
		{ SettingsConst.STATUS_BAR, true.ToInt().ToString() },
		{ SettingsConst.LOGO, true.ToInt().ToString() },
		{ SettingsConst.BETA, true.ToInt().ToString() },
		{ SettingsConst.LOGIN, false.ToInt().ToString() },
		{ SettingsConst.STICKER_POSITION_UNIT, "1" },
		{ SettingsConst.ALLOW_POSITIONING, false.ToInt().ToString() },
		{ SettingsConst.SHOW_GRIDS, false.ToInt().ToString() },
		{ SettingsConst.USE_CUSTOM_STICKER, false.ToInt().ToString() },
		{ SettingsConst.CUSTOM_STICKER_PATH, "" },
		{ SettingsConst.LICENSE, false.ToInt().ToString() },
		{ SettingsConst.GOOGLE_ANALYTICS, false.ToInt().ToString() },
		{ SettingsConst.DON_T_TRACK, false.ToInt().ToString() },
		{ SettingsConst.AUTO_FIT_WIDTH, true.ToInt().ToString() },
		{ SettingsConst.SHOW_PREVIEW, true.ToInt().ToString() },
		{ SettingsConst.LOCALIZATION, "unknown" },
		{ SettingsConst.USE_DIMENSION_CAP, false.ToInt().ToString() },
		{ SettingsConst.DIMENSION_CAP_MAX, "-1" },
		{ SettingsConst.DIMENSION_CAP_UNIT, "0" },
		{ SettingsConst.HIDE_ON_HOVER, true.ToInt().ToString() },
		{ SettingsConst.HIDE_DELAY, "750" },
		{ SettingsConst.PORTABLE_MODE, false.ToInt().ToString() },
		{ SettingsConst.DISCRETE_MODE, false.ToInt().ToString() }
	};

	private FileDatabase _database;

	public DatabaseManager()
	{
		_instance = this;
		InitDatabase();

		AddDefaults();
	}

	private void InitDatabase()
	{
		_database = new FileDatabase();
	}

	private void AddDefaults()
	{
		if (_database.GetValue(SettingsConst.BG_IMAGE.ToString()).Length == 0)
			foreach (var pair in _defaults)
				Save(pair.Key, pair.Value);
	}


	public void Delete(int key)
	{
		_database.DeleteValue(key.ToString());
	}

	#region Setter

	public static void Save(int name, int value)
	{
		_instance.Save(name.ToString(), value.ToString());
	}

	public static void Save(int name, bool value)
	{
		_instance.Save(name.ToString(), value.ToInt().ToString());
	}

	public static void Save(int name, string value)
	{
		_instance.Save(name.ToString(), value);
	}

	private void Save(string name, string value)
	{
		_database.SetValue(name, value);
	}

	#endregion

	#region Getter

	public static bool Load(int key)
	{
		return Load(key, int.Parse(_instance._defaults[key]).ToBool());
	}

	public static string Load(int key, string defaultValue)
	{
		return _instance._database.GetValue(key.ToString(), defaultValue);
	}

	public static int Load(int key, int defaultValue)
	{
		return int.Parse(_instance._database.GetValue(key.ToString(), defaultValue.ToString()));
	}

	public static bool Load(int key, bool defaultValue)
	{
		var value = _instance._database.GetValue(key.ToString(), defaultValue.ToInt().ToString());
		return int.Parse(value).ToBool();
	}

	private static string Load(string key, string defaultValue)
	{
		return _instance._database.GetValue(key, defaultValue);
	}

	#endregion
}