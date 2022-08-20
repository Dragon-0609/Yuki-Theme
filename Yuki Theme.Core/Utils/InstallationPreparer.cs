using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Database;

namespace Yuki_Theme.Core.Utils
{
	public class InstallationPreparer
	{
		public const  string FILE_NAMESPACE = "Yuki_Theme.Core.Database.";
		private const string APP_NAME       = "Yuki Theme";

		private string GetAppDataDirectory =>
			Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), APP_NAME);
		
		public void Prepare (bool forceExit)
		{
			const string installerName = "Yuki Installer.exe";

			if (Helper.mode == null)
			{
				string translation = API.CentralAPI.Current.Translate ("theme.downloader.errors.group.wrong");
				API_Events.showError (translation, API.CentralAPI.Current.Translate (translation));
				return;
			}
			
			string path = Path.Combine ( GetAppDataDirectory, installerName);
			File.Copy (Path.Combine (SettingsConst.CurrentPath, installerName), path, true);

			Settings.database.SetValue ("mode", ((int)Helper.mode).ToString ());

			// Set false to track install again
			Settings.database.UpdateData (SettingsConst.LOGIN, "false");

			var a = Assembly.GetExecutingAssembly ();
			using (Stream str = (a.GetManifestResourceStream ($"{FILE_NAMESPACE}files.txt")))
			{
				using (FileStream fstream =
				       File.Create (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
				                                  APP_NAME, "files.txt")))
				{
					str.Seek (0, SeekOrigin.Begin);
					str.CopyTo (fstream);
					using (StreamWriter sw = new StreamWriter (fstream))
					{
						string pth = FILE_NAMESPACE + (Helper.mode == ProductMode.Program ? "files_program.txt" : "files_plugin.txt");
						using (StreamReader reader = new StreamReader (a.GetManifestResourceStream (pth)))
						{
							string cx = reader.ReadToEnd ();
							sw.WriteLine ("\n" + cx);
						}
					}
				}
			}

			if (DatabaseManager.IsLinux)
			{
				if (File.Exists (FileDatabase.GetSettingsPath ()))
				{
					File.Copy (FileDatabase.GetSettingsPath (), Path.Combine (GetAppDataDirectory, FileDatabase.YUKI_SETTINGS));
				}
			}
			
			Process.Start (path, $"Install \"{Application.ExecutablePath}\"");
			if (forceExit)
			{
				if (Helper.mode == ProductMode.Program)
					Application.Exit ();
				else if (Helper.mode == ProductMode.Plugin)
					Environment.Exit (1);
			}
		}
	}
}