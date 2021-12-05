using System.Collections.Generic;
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
         }
      }
      
      /*static void Main(string[] args)
      {
         CreateTable(sqlite_conn);
         InsertData(sqlite_conn);
         ReadData(sqlite_conn);
      }*/

      private void CreateConnection()
      {

         /*
         // Create a new database connection:
         connection = new SQLiteConnection("Data Source=database.db;Version=3;New=True;Compress=True;");
         // Open the connection:
         try
         {
            connection.Open();
            command =  connection.CreateCommand();
            CreateTable ();
            InsertData (1,"empty");
            InsertData (2,"empty");
            InsertData (3,"false");
            InsertData (4,"true");
            InsertData (5,"0");
            InsertData (6,"0");
            

            
            // Console.WriteLine("Table cars created");
            
         }
         catch (Exception ex)
         {
            retries += 1;
            if (retries < 5)
               CreateConnection ();
            else
               throw new ApplicationException (ex.Message);
         }*/
         
      }

      /*
      private void CreateTable()
      {
         // DoAction (string.Format ("DROP TABLE {0};", tablename));
         DoAction (string.Format ("CREATE TABLE IF NOT EXISTS {0} (id INTEGER (4), tvalue TEXT (120));", tablename));
      }

      private void InsertData (int id, string value)
      {
         
         DoAction (string.Format ("INSERT INTO {0}(id,tvalue) SELECT {1}, '{2}' WHERE NOT EXISTS(SELECT * FROM {0} WHERE id = {1});", tablename, id, value ));
      }

      public void UpdateData(Dictionary<int, string> dictionary)
      {
         foreach (KeyValuePair <int, string> item in dictionary)
         {
            UpdateData (item.Key, item.Value);
         }
      }

      public void UpdateData(int id, string value)
      {
            DoAction (string.Format ("UPDATE  {0} SET tvalue=\"{1}\" WHERE id={2};", tablename, value, id));
      }

      public Dictionary<int, string> ReadDatas()
      {
         var dictionary = new Dictionary <int, string>();
         SQLiteDataReader reader;
         command =  connection.CreateCommand();
         command.CommandText = $"SELECT * FROM {tablename}";

         reader = command.ExecuteReader();
         while (reader.Read())
         {
            int data_id = int.Parse (reader["id"].ToString ());
            string data_value = reader["tvalue"].ToString();
            Console.WriteLine($"{data_id}, {data_value}");
           dictionary.Add (data_id, data_value); 
         }
         reader.Dispose ();
         return dictionary;
      }

      private void DoAction (string Command)
      {
         command.CommandText = Command;
         command.ExecuteNonQuery();       
         // command.Dispose ();
      }
      */

      public Dictionary<int, string> ReadData ()
      {
         Dictionary <int, string> dictionary = new Dictionary <int, string>();
        
         
         RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\YukiTheme");
         dictionary.Add (SettingsForm.PASCALPATH,key.GetValue (SettingsForm.PASCALPATH.ToString(), "empty").ToString ());
         dictionary.Add (SettingsForm.ACTIVE,key.GetValue (SettingsForm.ACTIVE.ToString(), "empty").ToString ());
         dictionary.Add (SettingsForm.BGIMAGE,key.GetValue (SettingsForm.BGIMAGE.ToString(), "true").ToString ());
         dictionary.Add (SettingsForm.STICKER,key.GetValue (SettingsForm.STICKER.ToString(), "true").ToString ());
         dictionary.Add (SettingsForm.STATUSBAR,key.GetValue (SettingsForm.STATUSBAR.ToString(), "true").ToString ());
         dictionary.Add (SettingsForm.ASKCHOICE,key.GetValue (SettingsForm.ASKCHOICE.ToString(), "true").ToString ());
         dictionary.Add (SettingsForm.CHOICEINDEX,key.GetValue (SettingsForm.CHOICEINDEX.ToString(), "0").ToString ());
         dictionary.Add (SettingsForm.SETTINGMODE,key.GetValue (SettingsForm.SETTINGMODE.ToString(), "0").ToString ());
         dictionary.Add (SettingsForm.AUTOUPDATE,key.GetValue (SettingsForm.AUTOUPDATE.ToString(), "true").ToString ());
         
         return dictionary;
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
      
	}
}