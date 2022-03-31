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
			if (ke.GetValue (Settings.PASCALPATH.ToString()) == null)
			{
				SetValueToDatabase (ke, Settings.PASCALPATH, "empty");
				SetValueToDatabase (ke, Settings.ACTIVE, "empty");
				SetValueToDatabase (ke, Settings.ASKCHOICE, "true");
				SetValueToDatabase (ke, Settings.CHOICEINDEX, "0");
				SetValueToDatabase (ke, Settings.SETTINGMODE, "0");
				SetValueToDatabase (ke, Settings.AUTOUPDATE, "true");
				SetValueToDatabase (ke, Settings.BGIMAGE, "true");
				SetValueToDatabase (ke, Settings.STICKER, "true");
				SetValueToDatabase (ke, Settings.STATUSBAR, "true");
				SetValueToDatabase (ke, Settings.LOGO, "true");
				SetValueToDatabase (ke, Settings.EDITOR, "false");
				SetValueToDatabase (ke, Settings.BETA, "true");
				SetValueToDatabase (ke, Settings.LOGIN, "false");
				SetValueToDatabase (ke, Settings.STICKERPOSITIONUNIT, "1");
				SetValueToDatabase (ke, Settings.ALLOWPOSITIONING, "false");
				SetValueToDatabase (ke, Settings.SHOWGRIDS, "false");
				SetValueToDatabase (ke, Settings.USECUSTOMSTICKER, "false");
				SetValueToDatabase (ke, Settings.CUSTOMSTICKER, "");
				SetValueToDatabase (ke, Settings.LICENSE, "false");
				SetValueToDatabase (ke, Settings.GOOGLEANALYTICS, "false");
				SetValueToDatabase (ke, Settings.DONTTRACK, "false");
				SetValueToDatabase (ke, Settings.AUTOFITWIDTH, "true");
				SetValueToDatabase (ke, Settings.ASKTOSAVE, "true");
				SetValueToDatabase (ke, Settings.SAVEASOLD, "true");
				SetValueToDatabase (ke, Settings.SHOWPREVIEW, "true");
				SetValueToDatabase (ke, Settings.LOCALIZATION, "en");
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
			AddToDictionary (ref dictionary, key, Settings.PASCALPATH, "empty");
			AddToDictionary (ref dictionary, key, Settings.ACTIVE, "empty");
			AddToDictionary (ref dictionary, key, Settings.ASKCHOICE, "true");
			AddToDictionary (ref dictionary, key, Settings.CHOICEINDEX, "0");
			AddToDictionary (ref dictionary, key, Settings.SETTINGMODE, "0");
			AddToDictionary (ref dictionary, key, Settings.AUTOUPDATE, "true");
			AddToDictionary (ref dictionary, key, Settings.BGIMAGE, "true");
			AddToDictionary (ref dictionary, key, Settings.STICKER, "true");
			AddToDictionary (ref dictionary, key, Settings.STATUSBAR, "true");
			AddToDictionary (ref dictionary, key, Settings.LOGO, "true");
			AddToDictionary (ref dictionary, key, Settings.EDITOR, "false");
			AddToDictionary (ref dictionary, key, Settings.BETA, "true");
			AddToDictionary (ref dictionary, key, Settings.LOGIN, "false");
			AddToDictionary (ref dictionary, key, Settings.STICKERPOSITIONUNIT, "1");
			AddToDictionary (ref dictionary, key, Settings.ALLOWPOSITIONING, "false");
			AddToDictionary (ref dictionary, key, Settings.SHOWGRIDS, "false");
			AddToDictionary (ref dictionary, key, Settings.USECUSTOMSTICKER, "false");
			AddToDictionary (ref dictionary, key, Settings.CUSTOMSTICKER,  "");
			AddToDictionary (ref dictionary, key, Settings.LICENSE, "false");
			AddToDictionary (ref dictionary, key, Settings.GOOGLEANALYTICS, "false");
			AddToDictionary (ref dictionary, key, Settings.DONTTRACK, "false");
			AddToDictionary (ref dictionary, key, Settings.AUTOFITWIDTH, "true");
			AddToDictionary (ref dictionary, key, Settings.ASKTOSAVE, "true");
			AddToDictionary (ref dictionary, key, Settings.SAVEASOLD, "true");
			AddToDictionary (ref dictionary, key, Settings.SHOWPREVIEW, "true");
			AddToDictionary (ref dictionary, key, Settings.LOCALIZATION, "en");

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

		public Point ReadLocation ()
		{
			RegistryKey key = Registry.CurrentUser.OpenSubKey (@"SOFTWARE\YukiTheme");
			string sp = key.GetValue (Settings.LOCATION.ToString (), "0:0").ToString ();
			if (!sp.Contains (":"))
				sp = "0:0";
			string [] spp = sp.Split (':');
			Console.WriteLine (sp);
			return new Point (int.Parse (spp [0]), int.Parse (spp [1]));
		}

		public void SaveLocation (Point loc)
		{
			RegistryKey kes = Registry.CurrentUser.OpenSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
			kes.SetValue (Settings.LOCATION.ToString (), $"{loc.X}:{loc.Y}");
		}
	}
}