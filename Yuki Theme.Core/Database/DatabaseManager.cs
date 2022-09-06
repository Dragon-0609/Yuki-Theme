using System;
using System.Collections.Generic;
using Microsoft.Win32;
using Yuki_Theme.Core.Interfaces;
using Yuki_Theme.Core.Utils;

namespace Yuki_Theme.Core.Database
{
	public class DatabaseManager
	{
		private IDatabase _database;

		public DatabaseManager (bool setDefault)
		{
			InitDatabase ();

			AddDefaults (setDefault);
		}

		private void InitDatabase ()
		{
			if (IsLinux)
				_database = new FileDatabase ();
			else
				_database = new WindowsRegistryDatabase ();
		}

		private void AddDefaults (bool setDefault)
		{
			if (setDefault && _database.GetValue (SettingsConst.PASCAL_PATH.ToString ()).Length <= 2)
			{
				SetValue (SettingsConst.PASCAL_PATH, "empty");
				SetValue (SettingsConst.ACTIVE, "empty");
				SetValue (SettingsConst.ASK_CHOICE, "true");
				SetValue (SettingsConst.CHOICE_INDEX, "0");
				SetValue (SettingsConst.SETTING_MODE, "0");
				SetValue (SettingsConst.AUTO_UPDATE, "true");
				SetValue (SettingsConst.BG_IMAGE, "true");
				SetValue (SettingsConst.STICKER, "true");
				SetValue (SettingsConst.STATUS_BAR, "true");
				SetValue (SettingsConst.LOGO, "true");
				SetValue (SettingsConst.EDITOR, "false");
				SetValue (SettingsConst.BETA, "true");
				SetValue (SettingsConst.LOGIN, "false");
				SetValue (SettingsConst.STICKER_POSITION_UNIT, "1");
				SetValue (SettingsConst.ALLOW_POSITIONING, "false");
				SetValue (SettingsConst.SHOW_GRIDS, "false");
				SetValue (SettingsConst.USE_CUSTOM_STICKER, "false");
				SetValue (SettingsConst.CUSTOM_STICKER_PATH, "");
				SetValue (SettingsConst.LICENSE, "false");
				SetValue (SettingsConst.GOOGLE_ANALYTICS, "false");
				SetValue (SettingsConst.DON_T_TRACK, "false");
				SetValue (SettingsConst.AUTO_FIT_WIDTH, "true");
				SetValue (SettingsConst.ASK_TO_SAVE, "true");
				SetValue (SettingsConst.SAVE_AS_OLD, "true");
				SetValue (SettingsConst.SHOW_PREVIEW, "true");
				SetValue (SettingsConst.LOCALIZATION, "unknown");
				SetValue (SettingsConst.USE_DIMENSION_CAP, "false");
				SetValue (SettingsConst.DIMENSION_CAP_MAX, "-1");
				SetValue (SettingsConst.DIMENSION_CAP_UNIT, "0");
				SetValue (SettingsConst.COLOR_PICKER, "0");
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
			
			AddToDictionary (ref dictionary, SettingsConst.PASCAL_PATH, "empty");
			AddToDictionary (ref dictionary, SettingsConst.ACTIVE, "empty");
			AddToDictionary (ref dictionary, SettingsConst.ASK_CHOICE, "true");
			AddToDictionary (ref dictionary, SettingsConst.CHOICE_INDEX, "0");
			AddToDictionary (ref dictionary, SettingsConst.SETTING_MODE, "0");
			AddToDictionary (ref dictionary, SettingsConst.AUTO_UPDATE, "true");
			AddToDictionary (ref dictionary, SettingsConst.BG_IMAGE, "true");
			AddToDictionary (ref dictionary, SettingsConst.STICKER, "true");
			AddToDictionary (ref dictionary, SettingsConst.STATUS_BAR, "true");
			AddToDictionary (ref dictionary, SettingsConst.LOGO, "true");
			AddToDictionary (ref dictionary, SettingsConst.EDITOR, "false");
			AddToDictionary (ref dictionary, SettingsConst.BETA, "true");
			AddToDictionary (ref dictionary, SettingsConst.LOGIN, "false");
			AddToDictionary (ref dictionary, SettingsConst.STICKER_POSITION_UNIT, "1");
			AddToDictionary (ref dictionary, SettingsConst.ALLOW_POSITIONING, "false");
			AddToDictionary (ref dictionary, SettingsConst.SHOW_GRIDS, "false");
			AddToDictionary (ref dictionary, SettingsConst.USE_CUSTOM_STICKER, "false");
			AddToDictionary (ref dictionary, SettingsConst.CUSTOM_STICKER_PATH, "");
			AddToDictionary (ref dictionary, SettingsConst.LICENSE, "false");
			AddToDictionary (ref dictionary, SettingsConst.GOOGLE_ANALYTICS, "false");
			AddToDictionary (ref dictionary, SettingsConst.DON_T_TRACK, "false");
			AddToDictionary (ref dictionary, SettingsConst.AUTO_FIT_WIDTH, "true");
			AddToDictionary (ref dictionary, SettingsConst.ASK_TO_SAVE, "true");
			AddToDictionary (ref dictionary, SettingsConst.SAVE_AS_OLD, "true");
			AddToDictionary (ref dictionary, SettingsConst.SHOW_PREVIEW, "true");
			AddToDictionary (ref dictionary, SettingsConst.LOCALIZATION, "unknown");
			AddToDictionary (ref dictionary, SettingsConst.USE_DIMENSION_CAP, "false");
			AddToDictionary (ref dictionary, SettingsConst.DIMENSION_CAP_MAX, "-1");
			AddToDictionary (ref dictionary, SettingsConst.DIMENSION_CAP_UNIT, "0");
			AddToDictionary (ref dictionary, SettingsConst.COLOR_PICKER, "0");

			return dictionary;
		}


		public string ReadData (int key, string defaultValue = "")
		{
			return GetValue (key.ToString (), defaultValue).ToString ();
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
				int p = (int) Environment.OSVersion.Platform;
				return (p == 4) || (p == 6) || (p == 128);
			}
		}
		
	}
}