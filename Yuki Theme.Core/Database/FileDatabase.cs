using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Yuki_Theme.Core.Interfaces;

namespace Yuki_Theme.Core.Database
{
	internal class FileDatabase : IDatabase
	{
		internal const string YUKI_SETTINGS = "yuki.settings";

		private Dictionary<string, string> _dictionary;

		public FileDatabase ()
		{
			_dictionary = new Dictionary<string, string> ();
			Load ();
		}

		public void SetValue (string name, string value)
		{
			if (!_dictionary.ContainsKey (name))
				_dictionary.Add (name, value);
			else
				_dictionary[name] = value;
			Save ();
		}

		public string GetValue (string name)
		{
			if (_dictionary.ContainsKey (name))
				return _dictionary[name];
			return "";
		}

		public string GetValue (string name, string defaultValue)
		{
			if (!_dictionary.ContainsKey (name))
				return defaultValue;
			else
				return _dictionary[name];
		}

		public void DeleteValue (string name)
		{
			if (_dictionary.ContainsKey (name))
				_dictionary.Remove (name);
			Save ();
		}
		public void Wipe ()
		{
			if (File.Exists (GetSettingsPath ()))
			{
				File.Delete (GetSettingsPath ());
			}
			_dictionary.Clear ();
		}

		private void Load ()
		{
			string path = GetSettingsPath ();
			if (File.Exists (path))
			{
				string input = File.ReadAllText (path, Encoding.UTF8);

				string[] lines = input.Split ('\n');
				foreach (string line in lines)
				{
					string[] param = line.Split (new string[] { " => " }, StringSplitOptions.None);
					if (param.Length == 2)
					{
						_dictionary.Add (param[0], param[1]);
					}
				}
			}
		}

		private void Save ()
		{
			string output = "";
			int o = _dictionary.Count - 1;
			for (var i = 0; i < _dictionary.Count; i++)
			{
				output += $"{_dictionary.Keys.ElementAt (i)} => {_dictionary.Values.ElementAt (i)}";
				if (i != o) output += "\n";
			}

			File.WriteAllText (GetSettingsPath (), output, Encoding.UTF8);
		}
		

		internal static string GetSettingsPath ()
		{
			return Path.Combine (SettingsConst.CurrentPath, YUKI_SETTINGS);
		}
	}
}