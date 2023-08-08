using System;
using System.Collections.Generic;
using System.IO;
using Yuki_Theme.Core.Database;
using Yuki_Theme.Core.Utils;

namespace YukiTheme.Tools
{
	public class DatabaseManager
	{
		private FileDatabase _database;

		public readonly Dictionary<int, string> _defaults = new()
		{
			{ SettingsConst.AutoUpdate, "true" },
			{ SettingsConst.BgImage, "true" },
			{ SettingsConst.Sticker, "true" },
			{ SettingsConst.StatusBar, "true" },
			{ SettingsConst.Logo, "true" },
			{ SettingsConst.Beta, "true" },
			{ SettingsConst.Login, "false" },
			{ SettingsConst.StickerPositionUnit, "1" },
			{ SettingsConst.AllowPositioning, "false" },
			{ SettingsConst.ShowGrids, "false" },
			{ SettingsConst.UseCustomSticker, "false" },
			{ SettingsConst.CustomStickerPath, "" },
			{ SettingsConst.License, "false" },
			{ SettingsConst.GoogleAnalytics, "false" },
			{ SettingsConst.DonTTrack, "false" },
			{ SettingsConst.AutoFitWidth, "true" },
			{ SettingsConst.ShowPreview, "true" },
			{ SettingsConst.Localization, "unknown" },
			{ SettingsConst.UseDimensionCap, "false" },
			{ SettingsConst.DimensionCapMax, "-1" },
			{ SettingsConst.DimensionCapUnit, "0" },
			{ SettingsConst.HideOnHover, "true" },
			{ SettingsConst.HideDelay, "750" },
			{ SettingsConst.PortableMode, "false" },
		};

		public DatabaseManager(bool setDefault)
		{
			InitDatabase();

			AddDefaults(setDefault);
		}

		private void InitDatabase()
		{
			_database = new FileDatabase();
		}

		private const string APP_NAME = "Yuki Theme";

		private void AddDefaults(bool setDefault)
		{
			if (setDefault && _database.GetValue(SettingsConst.AutoUpdate.ToString()).Length <= 2)
			{
				foreach (KeyValuePair<int, string> pair in _defaults)
				{
					SetValue(pair.Key, pair.Value);
				}
			}
		}

		private void SetValue(int name, string value)
		{
			SetValue(name.ToString(), value);
		}

		public void SetValue(string name, string value)
		{
			_database.SetValue(name, value);
		}

		private void AddToDictionary(ref Dictionary<int, string> dictionary, int name, string defValue)
		{
			dictionary.Add(name, _database.GetValue(name.ToString(), defValue));
		}

		public Dictionary<int, string> ReadData()
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();

			foreach (KeyValuePair<int, string> pair in _defaults)
			{
				AddToDictionary(ref dictionary, pair.Key, pair.Value);
			}

			return dictionary;
		}


		public string ReadData(int key, string defaultValue = "")
		{
			return GetValue(key.ToString(), defaultValue);
		}

		public void UpdateData(Dictionary<int, string> dictionary)
		{
			foreach (KeyValuePair<int, string> pair in dictionary)
			{
				SetValue(pair.Key, pair.Value);
			}
		}

		public void UpdateData(int key, string value)
		{
			SetValue(key, value);
		}

		public void DeleteData(int key)
		{
			_database.DeleteValue(key.ToString());
		}

		public string GetValue(string name) => _database.GetValue(name);
		public string GetValue(string name, string defaultValue) => _database.GetValue(name, defaultValue);

		public void DeleteValue(string name) => _database.DeleteValue(name);

		public WindowProps ReadLocation()
		{
			string sp = GetLocation();
			WindowProps props = WindowProps.Parse(sp);

			return props;
		}

		internal string GetLocation()
		{
			string sp = "0:0";
			sp = GetValue(SettingsConst.Location.ToString(), "0:0");
			if (!sp.Contains(":"))
				sp = "0:0";

			return sp;
		}

		public void SaveLocation(WindowProps loc)
		{
			SetValue(SettingsConst.Location.ToString(), loc.ToString());
		}

		public static bool IsLinux
		{
			get
			{
				int p = (int)Environment.OSVersion.Platform;
				return (p == 4) || (p == 6) || (p == 128);
			}
		}

		private static void DeletePortableSettings(bool portable, string path)
		{
			if (!portable)
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
		}

		public bool IsFileDatabase() => _database is FileDatabase;
	}
}