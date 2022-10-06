using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using Yuki_Theme.Core.Interfaces;
using Yuki_Theme.Core.Utils;

namespace Yuki_Theme.Core.Database
{
	public class DatabaseManager
	{
		private IDatabase _database;

		public readonly Dictionary <int, string> _defaults = new Dictionary <int, string> ()
		{
			{ SettingsConst.PASCAL_PATH, "empty" },
			{ SettingsConst.ACTIVE, "empty" },
			{ SettingsConst.ASK_CHOICE, "true" },
			{ SettingsConst.CHOICE_INDEX, "0" },
			{ SettingsConst.SETTING_MODE, "0" },
			{ SettingsConst.AUTO_UPDATE, "true" },
			{ SettingsConst.BG_IMAGE, "true" },
			{ SettingsConst.STICKER, "true" },
			{ SettingsConst.STATUS_BAR, "true" },
			{ SettingsConst.LOGO, "true" },
			{ SettingsConst.EDITOR, "false" },
			{ SettingsConst.BETA, "true" },
			{ SettingsConst.LOGIN, "false" },
			{ SettingsConst.STICKER_POSITION_UNIT, "1" },
			{ SettingsConst.ALLOW_POSITIONING, "false" },
			{ SettingsConst.SHOW_GRIDS, "false" },
			{ SettingsConst.USE_CUSTOM_STICKER, "false" },
			{ SettingsConst.CUSTOM_STICKER_PATH, "" },
			{ SettingsConst.LICENSE, "false" },
			{ SettingsConst.GOOGLE_ANALYTICS, "false" },
			{ SettingsConst.DON_T_TRACK, "false" },
			{ SettingsConst.AUTO_FIT_WIDTH, "true" },
			{ SettingsConst.ASK_TO_SAVE, "true" },
			{ SettingsConst.SAVE_AS_OLD, "true" },
			{ SettingsConst.SHOW_PREVIEW, "true" },
			{ SettingsConst.LOCALIZATION, "unknown" },
			{ SettingsConst.USE_DIMENSION_CAP, "false" },
			{ SettingsConst.DIMENSION_CAP_MAX, "-1" },
			{ SettingsConst.DIMENSION_CAP_UNIT, "0" },
			{ SettingsConst.COLOR_PICKER, "0" },
			{ SettingsConst.HIDE_ON_HOVER, "true" },
			{ SettingsConst.HIDE_DELAY, "750" },
			{ SettingsConst.PORTABLE_MODE, "false" },
			{ SettingsConst.EDITOR_READ_ONLY, "true" },
			{ SettingsConst.EDITOR_SAVED_FILE, "null" },
		};

		public DatabaseManager (bool setDefault)
		{
			InitDatabase ();

			AddDefaults (setDefault);
		}

		private void InitDatabase ()
		{
			if (IsLinux || IsPortable ())
				_database = new FileDatabase ();
			else
				_database = new WindowsRegistryDatabase ();
		}

		private const string APP_NAME = "Yuki Theme";

		internal bool IsPortable ()
		{
			return File.Exists (FileDatabase.YUKI_SETTINGS);
		}

		private void AddDefaults (bool setDefault)
		{
			if (setDefault && _database.GetValue (SettingsConst.PASCAL_PATH.ToString ()).Length <= 2)
			{
				foreach (KeyValuePair <int, string> pair in _defaults)
				{
					SetValue (pair.Key, pair.Value);
				}
			}
		}

		private void SetValue (int name, string value)
		{
			SetValue (name.ToString (), value);
		}

		public void SetValue (string name, string value)
		{
			_database.SetValue (name, value);
		}

		private void AddToDictionary (ref Dictionary <int, string> dictionary, int name, string defValue)
		{
			dictionary.Add (name, _database.GetValue (name.ToString (), defValue));
		}

		public Dictionary <int, string> ReadData ()
		{
			Dictionary <int, string> dictionary = new Dictionary <int, string> ();

			foreach (KeyValuePair <int, string> pair in _defaults)
			{
				AddToDictionary (ref dictionary, pair.Key, pair.Value);
			}

			return dictionary;
		}


		public string ReadData (int key, string defaultValue = "")
		{
			return GetValue (key.ToString (), defaultValue);
		}

		public void UpdateData (Dictionary <int, string> dictionary)
		{
			foreach (KeyValuePair <int, string> pair in dictionary)
			{
				SetValue (pair.Key, pair.Value);
			}
		}

		public void UpdateData (int key, string value)
		{
			SetValue (key, value);
		}

		public void DeleteData (int key)
		{
			_database.DeleteValue (key.ToString ());
		}

		public string GetValue (string name) => _database.GetValue (name);
		public string GetValue (string name, string defaultValue) => _database.GetValue (name, defaultValue);

		public void DeleteValue (string name) => _database.DeleteValue (name);

		public WindowProps ReadLocation ()
		{
			string sp = GetLocation ();
			WindowProps props = WindowProps.Parse (sp);

			return props;
		}

		internal string GetLocation ()
		{
			string sp = "0:0";
			sp = GetValue (SettingsConst.LOCATION.ToString (), "0:0");
			if (!sp.Contains (":"))
				sp = "0:0";

			return sp;
		}

		public void SaveLocation (WindowProps loc)
		{
			SetValue (SettingsConst.LOCATION.ToString (), loc.ToString ());
		}

		public static bool IsLinux
		{
			get
			{
				int p = (int)Environment.OSVersion.Platform;
				return (p == 4) || (p == 6) || (p == 128);
			}
		}

		public void SwapDatabase ()
		{
			bool portable = Settings.portableMode;
			_database.Wipe ();
			_database = portable ? new FileDatabase () : new WindowsRegistryDatabase ();
			Settings.SaveData ();
			string path = FileDatabase.YUKI_SETTINGS;
			DeletePortableSettings (portable, path);
		}

		private static void DeletePortableSettings (bool portable, string path)
		{
			if (!portable)
			{
				if (File.Exists (path))
				{
					File.Delete (path);
				}
			}
		}

		public bool IsFileDatabase() => _database is FileDatabase;

	}
}