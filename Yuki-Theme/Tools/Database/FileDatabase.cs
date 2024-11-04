using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YukiTheme.Tools.Database;

namespace YukiTheme.Tools;

internal class FileDatabase : IDatabase
{
	internal const string YUKI_SETTINGS = "yuki.settings";

	private readonly Dictionary<string, string> _dictionary;

	public FileDatabase()
	{
		_dictionary = new Dictionary<string, string>();
		Load();
	}

	public void SetValue(string name, string value)
	{
		if (!_dictionary.ContainsKey(name))
			_dictionary.Add(name, value);
		else
			_dictionary[name] = value;
		Save();
	}

	public string GetValue(string name)
	{
		if (_dictionary.ContainsKey(name))
			return _dictionary[name];
		return "";
	}

	public string GetValue(string name, string defaultValue)
	{
		if (!_dictionary.ContainsKey(name))
			return defaultValue;
		return _dictionary[name];
	}

	public void DeleteValue(string name)
	{
		if (_dictionary.ContainsKey(name))
			_dictionary.Remove(name);
		Save();
	}

	public void Wipe()
	{
		if (File.Exists(GetSettingsPath())) File.Delete(GetSettingsPath());

		_dictionary.Clear();
	}

	private void Load()
	{
		var path = GetSettingsPath();
		if (File.Exists(path))
		{
			var input = File.ReadAllText(path, Encoding.UTF8);

			DatabaseHelper.LoadData(input, _dictionary);
		}
	}

	private void Save()
	{
		string output = DatabaseHelper.SaveData(_dictionary);
		File.WriteAllText(GetSettingsPath(), output, Encoding.UTF8);
	}


	internal static string GetSettingsPath()
	{
		return Path.Combine(YukiTheme_VisualPascalABCPlugin.GetCurrentFolder, YUKI_SETTINGS);
	}
}