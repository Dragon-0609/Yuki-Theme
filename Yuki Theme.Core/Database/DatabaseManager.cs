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
         RegistryKey ke = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
         if (ke.GetValue ("1") == null)
         {
            ke.SetValue (SettingsForm.PASCALPATH.ToString(), "empty");
            ke.SetValue (SettingsForm.ACTIVE.ToString(), "empty");
            ke.SetValue (SettingsForm.ASKCHOICE.ToString(), "true");
            ke.SetValue (SettingsForm.CHOICEINDEX.ToString(), "0");
            ke.SetValue (SettingsForm.SETTINGMODE.ToString(), "0");
            ke.SetValue (SettingsForm.AUTOUPDATE.ToString(), "true");
            ke.SetValue (SettingsForm.BGIMAGE.ToString(), "true");
            ke.SetValue (SettingsForm.STICKER.ToString(), "true");
            ke.SetValue (SettingsForm.STATUSBAR.ToString(), "true");
            ke.SetValue (SettingsForm.LOGO.ToString(), "true");
            ke.SetValue (SettingsForm.EDITOR.ToString(), "false");
            ke.SetValue (SettingsForm.BETA.ToString(), "true");
            ke.SetValue (SettingsForm.LOGIN.ToString(), "false");
         }
      }
      
      public Dictionary<int, string> ReadData ()
      {
         Dictionary <int, string> dictionary = new Dictionary <int, string>();
        
         
         RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\YukiTheme");
         dictionary.Add (SettingsForm.PASCALPATH,key.GetValue (SettingsForm.PASCALPATH.ToString(), "empty").ToString ());
         dictionary.Add (SettingsForm.ACTIVE,key.GetValue (SettingsForm.ACTIVE.ToString(), "empty").ToString ());
         dictionary.Add (SettingsForm.ASKCHOICE,key.GetValue (SettingsForm.ASKCHOICE.ToString(), "true").ToString ());
         dictionary.Add (SettingsForm.CHOICEINDEX,key.GetValue (SettingsForm.CHOICEINDEX.ToString(), "0").ToString ());
         dictionary.Add (SettingsForm.SETTINGMODE,key.GetValue (SettingsForm.SETTINGMODE.ToString(), "0").ToString ());
         dictionary.Add (SettingsForm.AUTOUPDATE,key.GetValue (SettingsForm.AUTOUPDATE.ToString(), "true").ToString ());
         dictionary.Add (SettingsForm.BGIMAGE,key.GetValue (SettingsForm.BGIMAGE.ToString(), "true").ToString ());
         dictionary.Add (SettingsForm.STICKER,key.GetValue (SettingsForm.STICKER.ToString(), "true").ToString ());
         dictionary.Add (SettingsForm.STATUSBAR,key.GetValue (SettingsForm.STATUSBAR.ToString(), "true").ToString ());
         dictionary.Add (SettingsForm.LOGO,key.GetValue (SettingsForm.LOGO.ToString(), "true").ToString ());
         dictionary.Add (SettingsForm.EDITOR,key.GetValue (SettingsForm.EDITOR.ToString(), "false").ToString ());
         dictionary.Add (SettingsForm.BETA,key.GetValue (SettingsForm.BETA.ToString(), "true").ToString ());
         dictionary.Add (SettingsForm.LOGIN,key.GetValue (SettingsForm.LOGIN.ToString(), "false").ToString ());
         
         return dictionary;
      }

      public string ReadData (int key, string defaultValue = "")
      {
         RegistryKey kes = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\YukiTheme");
         return kes.GetValue (key.ToString (), defaultValue).ToString ();
      }
      
      public void UpdateData (Dictionary<int, string> dictionary)
      {
         RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
         foreach (KeyValuePair<int,string> pair in dictionary)
         {
            key.SetValue (pair.Key.ToString (), pair.Value);
         }
      }
      
      public void UpdateData (int key, string value)
      {
         RegistryKey kes = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
         kes.SetValue (key.ToString (), value);
      }

      public Point ReadLocation ()
      {
         RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\YukiTheme");
         string sp = key.GetValue (SettingsForm.LOCATION.ToString(), "0:0").ToString ();
         if (!sp.Contains (":"))
            sp = "0:0";
         string [] spp = sp.Split (':');
         Console.WriteLine (sp);
         return new Point (int.Parse (spp [0]), int.Parse (spp [1]));
      }

      public void SaveLocation (Point loc)
      {
         RegistryKey kes = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
         kes.SetValue (SettingsForm.LOCATION.ToString (), $"{loc.X}:{loc.Y}");
      }
      
	}
}