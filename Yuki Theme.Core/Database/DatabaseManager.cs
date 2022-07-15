using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Win32;
using Yuki_Theme.Core.Forms; // using System.Data.SQLite;

namespace Yuki_Theme.Core.Database
{
	public class DatabaseManager
	{
		/*
		private       SQLiteConnection connection;
		private       int              retries =0;
		private       SQLiteCommand    command;
		private const string           tablename = "settings";
		*/

		public DatabaseManager ()
		{
			// CreateConnection ();
			RegistryKey ke = Registry.CurrentUser.CreateSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
			if (ke.GetValue (Settings.PASCAL_PATH.ToString ()) == null)
			{
				SetValueToDatabase (ke, Settings.PASCAL_PATH, "empty");
				SetValueToDatabase (ke, Settings.ACTIVE, "empty");
				SetValueToDatabase (ke, Settings.ASK_CHOICE, "true");
				SetValueToDatabase (ke, Settings.CHOICE_INDEX, "0");
				SetValueToDatabase (ke, Settings.SETTING_MODE, "0");
				SetValueToDatabase (ke, Settings.AUTO_UPDATE, "true");
				SetValueToDatabase (ke, Settings.BG_IMAGE, "true");
				SetValueToDatabase (ke, Settings.STICKER, "true");
				SetValueToDatabase (ke, Settings.STATUS_BAR, "true");
				SetValueToDatabase (ke, Settings.LOGO, "true");
				SetValueToDatabase (ke, Settings.EDITOR, "false");
				SetValueToDatabase (ke, Settings.BETA, "true");
				SetValueToDatabase (ke, Settings.LOGIN, "false");
				SetValueToDatabase (ke, Settings.STICKER_POSITION_UNIT, "1");
				SetValueToDatabase (ke, Settings.ALLOW_POSITIONING, "false");
				SetValueToDatabase (ke, Settings.SHOW_GRIDS, "false");
				SetValueToDatabase (ke, Settings.USE_CUSTOM_STICKER, "false");
				SetValueToDatabase (ke, Settings.CUSTOM_STICKER_PATH, "");
				SetValueToDatabase (ke, Settings.LICENSE, "false");
				SetValueToDatabase (ke, Settings.GOOGLE_ANALYTICS, "false");
				SetValueToDatabase (ke, Settings.DON_T_TRACK, "false");
				SetValueToDatabase (ke, Settings.AUTO_FIT_WIDTH, "true");
				SetValueToDatabase (ke, Settings.ASK_TO_SAVE, "true");
				SetValueToDatabase (ke, Settings.SAVE_AS_OLD, "true");
				SetValueToDatabase (ke, Settings.SHOW_PREVIEW, "true");
				SetValueToDatabase (ke, Settings.LOCALIZATION, "unknown");
				SetValueToDatabase (ke, Settings.USE_DIMENSION_CAP, "false");
				SetValueToDatabase (ke, Settings.DIMENSION_CAP_MAX, "-1");
				SetValueToDatabase (ke, Settings.DIMENSION_CAP_UNIT, "0");
				SetValueToDatabase (ke, Settings.COLOR_PICKER, "0");
			}
		}

		private static void SetValueToDatabase (RegistryKey ke, int name, string value)
		{
			ke.SetValue (name.ToString (), value);
		}

		private void AddToDictionary (ref Dictionary <int, string> dictionary, RegistryKey key, int name, string defValue)
		{
			dictionary.Add (name, key.GetValue (name.ToString (), defValue).ToString ());
		}

		public Dictionary <int, string> ReadData ()
		{
			Dictionary <int, string> dictionary = new Dictionary <int, string> ();

			RegistryKey key = Registry.CurrentUser.OpenSubKey (@"SOFTWARE\YukiTheme");
			AddToDictionary (ref dictionary, key, Settings.PASCAL_PATH, "empty");
			AddToDictionary (ref dictionary, key, Settings.ACTIVE, "empty");
			AddToDictionary (ref dictionary, key, Settings.ASK_CHOICE, "true");
			AddToDictionary (ref dictionary, key, Settings.CHOICE_INDEX, "0");
			AddToDictionary (ref dictionary, key, Settings.SETTING_MODE, "0");
			AddToDictionary (ref dictionary, key, Settings.AUTO_UPDATE, "true");
			AddToDictionary (ref dictionary, key, Settings.BG_IMAGE, "true");
			AddToDictionary (ref dictionary, key, Settings.STICKER, "true");
			AddToDictionary (ref dictionary, key, Settings.STATUS_BAR, "true");
			AddToDictionary (ref dictionary, key, Settings.LOGO, "true");
			AddToDictionary (ref dictionary, key, Settings.EDITOR, "false");
			AddToDictionary (ref dictionary, key, Settings.BETA, "true");
			AddToDictionary (ref dictionary, key, Settings.LOGIN, "false");
			AddToDictionary (ref dictionary, key, Settings.STICKER_POSITION_UNIT, "1");
			AddToDictionary (ref dictionary, key, Settings.ALLOW_POSITIONING, "false");
			AddToDictionary (ref dictionary, key, Settings.SHOW_GRIDS, "false");
			AddToDictionary (ref dictionary, key, Settings.USE_CUSTOM_STICKER, "false");
			AddToDictionary (ref dictionary, key, Settings.CUSTOM_STICKER_PATH, "");
			AddToDictionary (ref dictionary, key, Settings.LICENSE, "false");
			AddToDictionary (ref dictionary, key, Settings.GOOGLE_ANALYTICS, "false");
			AddToDictionary (ref dictionary, key, Settings.DON_T_TRACK, "false");
			AddToDictionary (ref dictionary, key, Settings.AUTO_FIT_WIDTH, "true");
			AddToDictionary (ref dictionary, key, Settings.ASK_TO_SAVE, "true");
			AddToDictionary (ref dictionary, key, Settings.SAVE_AS_OLD, "true");
			AddToDictionary (ref dictionary, key, Settings.SHOW_PREVIEW, "true");
			AddToDictionary (ref dictionary, key, Settings.LOCALIZATION, "unknown");
			AddToDictionary (ref dictionary, key, Settings.USE_DIMENSION_CAP, "false");
			AddToDictionary (ref dictionary, key, Settings.DIMENSION_CAP_MAX, "-1");
			AddToDictionary (ref dictionary, key, Settings.DIMENSION_CAP_UNIT, "0");
			AddToDictionary (ref dictionary, key, Settings.COLOR_PICKER, "0");

			return dictionary;
		}


		public string ReadData (int key, string defaultValue = "")
		{
			RegistryKey kes = Registry.CurrentUser.OpenSubKey (@"SOFTWARE\YukiTheme");
			return kes.GetValue (key.ToString (), defaultValue).ToString ();
		}

		public void UpdateData (Dictionary <int, string> dictionary)
		{
			RegistryKey key = Registry.CurrentUser.OpenSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
			foreach (KeyValuePair <int, string> pair in dictionary)
			{
				SetValueToDatabase (key, pair.Key, pair.Value);
			}
		}

		public void UpdateData (int key, string value)
		{
			RegistryKey kes = Registry.CurrentUser.OpenSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
			SetValueToDatabase (kes, key, value);
		}

		public static void DeleteData (int key)
		{
			try
			{
				RegistryKey kes = Registry.CurrentUser.OpenSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
				kes.DeleteValue (key.ToString ());
			} catch
			{
			}
		}

		public WindowProps ReadLocation ()
		{
			string sp = GetLocation ();
			WindowProps props = WindowProps.Parse (sp);

			return props;
		}

		internal static string GetLocation ()
		{
			string sp = "0:0";
			RegistryKey key = Registry.CurrentUser.OpenSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadSubTree);
			if (key != null)
			{
				sp = key.GetValue (Settings.LOCATION.ToString (), "0:0").ToString ();
				if (!sp.Contains (":"))
					sp = "0:0";
			}

			return sp;
		}

		public void SaveLocation (WindowProps loc)
		{
			RegistryKey kes = Registry.CurrentUser.OpenSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
			kes.SetValue (Settings.LOCATION.ToString (), loc.ToString ());
		}
	}
}