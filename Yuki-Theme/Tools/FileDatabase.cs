using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace YukiTheme.Tools;

internal class FileDatabase
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

			var lines = input.Split('\n');
			foreach (var line in lines)
			{
				var param = line.Split(new[] { " => " }, StringSplitOptions.None);
				if (param.Length == 2) _dictionary.Add(param[0], param[1]);
			}
		}
	}

	private void Save()
	{
		var output = "";
		var o = _dictionary.Count - 1;
		for (var i = 0; i < _dictionary.Count; i++)
		{
			output += $"{_dictionary.Keys.ElementAt(i)} => {_dictionary.Values.ElementAt(i)}";
			if (i != o) output += "\n";
		}

		File.WriteAllText(GetSettingsPath(), output, Encoding.UTF8);
	}


	internal static string GetSettingsPath()
	{
		return Path.Combine(YukiTheme_VisualPascalABCPlugin.GetCurrentFolder, YUKI_SETTINGS);
	}
}