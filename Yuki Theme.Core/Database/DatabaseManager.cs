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
			if (ke.GetValue ("1") == null)
			{
				ke.SetValue (Settings.PASCALPATH.ToString (), "empty");
				ke.SetValue (Settings.ACTIVE.ToString (), "empty");
				ke.SetValue (Settings.ASKCHOICE.ToString (), "true");
				ke.SetValue (Settings.CHOICEINDEX.ToString (), "0");
				ke.SetValue (Settings.SETTINGMODE.ToString (), "0");
				ke.SetValue (Settings.AUTOUPDATE.ToString (), "true");
				ke.SetValue (Settings.BGIMAGE.ToString (), "true");
				ke.SetValue (Settings.STICKER.ToString (), "true");
				ke.SetValue (Settings.STATUSBAR.ToString (), "true");
				ke.SetValue (Settings.LOGO.ToString (), "true");
				ke.SetValue (Settings.EDITOR.ToString (), "false");
				ke.SetValue (Settings.BETA.ToString (), "true");
				ke.SetValue (Settings.LOGIN.ToString (), "false");
				ke.SetValue (Settings.STICKERPOSITIONUNIT.ToString (), "1");
				ke.SetValue (Settings.ALLOWPOSITIONING.ToString (), "false");
				ke.SetValue (Settings.SHOWGRIDS.ToString (), "false");
				ke.SetValue (Settings.USECUSTOMSTICKER.ToString (), "false");
				ke.SetValue (Settings.CUSTOMSTICKER.ToString (), "");
				ke.SetValue (Settings.LICENSE.ToString (), "false");
				ke.SetValue (Settings.GOOGLEANALYTICS.ToString (), "false");
				ke.SetValue (Settings.DONTTRACK.ToString (), "false");
				ke.SetValue (Settings.AUTOFITWIDTH.ToString (), "true");
				ke.SetValue (Settings.ASKTOSAVE.ToString (), "true");
				ke.SetValue (Settings.SAVEASOLD.ToString (), "true");
			}
		}

		public Dictionary <int, string> ReadData ()
		{
			Dictionary <int, string> dictionary = new Dictionary <int, string> ();


			RegistryKey key = Registry.CurrentUser.OpenSubKey (@"SOFTWARE\YukiTheme");
			dictionary.Add (Settings.PASCALPATH, key.GetValue (Settings.PASCALPATH.ToString (), "empty").ToString ());
			dictionary.Add (Settings.ACTIVE, key.GetValue (Settings.ACTIVE.ToString (), "empty").ToString ());
			dictionary.Add (Settings.ASKCHOICE, key.GetValue (Settings.ASKCHOICE.ToString (), "true").ToString ());
			dictionary.Add (Settings.CHOICEINDEX, key.GetValue (Settings.CHOICEINDEX.ToString (), "0").ToString ());
			dictionary.Add (Settings.SETTINGMODE, key.GetValue (Settings.SETTINGMODE.ToString (), "0").ToString ());
			dictionary.Add (Settings.AUTOUPDATE, key.GetValue (Settings.AUTOUPDATE.ToString (), "true").ToString ());
			dictionary.Add (Settings.BGIMAGE, key.GetValue (Settings.BGIMAGE.ToString (), "true").ToString ());
			dictionary.Add (Settings.STICKER, key.GetValue (Settings.STICKER.ToString (), "true").ToString ());
			dictionary.Add (Settings.STATUSBAR, key.GetValue (Settings.STATUSBAR.ToString (), "true").ToString ());
			dictionary.Add (Settings.LOGO, key.GetValue (Settings.LOGO.ToString (), "true").ToString ());
			dictionary.Add (Settings.EDITOR, key.GetValue (Settings.EDITOR.ToString (), "false").ToString ());
			dictionary.Add (Settings.BETA, key.GetValue (Settings.BETA.ToString (), "true").ToString ());
			dictionary.Add (Settings.LOGIN, key.GetValue (Settings.LOGIN.ToString (), "false").ToString ());
			dictionary.Add (Settings.STICKERPOSITIONUNIT, key.GetValue (Settings.STICKERPOSITIONUNIT.ToString (), "1").ToString ());
			dictionary.Add (Settings.ALLOWPOSITIONING, key.GetValue (Settings.ALLOWPOSITIONING.ToString (), "false").ToString ());
			dictionary.Add (Settings.SHOWGRIDS, key.GetValue (Settings.SHOWGRIDS.ToString (), "false").ToString ());
			dictionary.Add (Settings.USECUSTOMSTICKER, key.GetValue (Settings.USECUSTOMSTICKER.ToString (), "false").ToString ());
			dictionary.Add (Settings.CUSTOMSTICKER, key.GetValue (Settings.CUSTOMSTICKER.ToString (), "").ToString ());
			dictionary.Add (Settings.LICENSE, key.GetValue (Settings.LICENSE.ToString (), "false").ToString ());
			dictionary.Add (Settings.GOOGLEANALYTICS, key.GetValue (Settings.GOOGLEANALYTICS.ToString (), "false").ToString ());
			dictionary.Add (Settings.DONTTRACK, key.GetValue (Settings.DONTTRACK.ToString (), "false").ToString ());
			dictionary.Add (Settings.AUTOFITWIDTH, key.GetValue (Settings.AUTOFITWIDTH.ToString (), "true").ToString ());
			dictionary.Add (Settings.ASKTOSAVE, key.GetValue (Settings.ASKTOSAVE.ToString (), "true").ToString ());
			dictionary.Add (Settings.SAVEASOLD, key.GetValue (Settings.SAVEASOLD.ToString (), "true").ToString ());

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
				key.SetValue (pair.Key.ToString (), pair.Value);
			}
		}

		public void UpdateData (int key, string value)
		{
			RegistryKey kes = Registry.CurrentUser.OpenSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
			kes.SetValue (key.ToString (), value);
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