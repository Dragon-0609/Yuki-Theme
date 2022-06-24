using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using Yuki_Theme.Core.Forms;

namespace Yuki_Theme.Core
{
	public class InstallationPreparer
	{
		public const string FileNamespace = "Yuki_Theme.Core.Database.";

		public void Prepare (bool forceExit)
		{
			string path = Path.Combine (
				Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "Yuki Theme",
				"Yuki Installer.exe");
			File.Copy (Path.Combine (API.currentPath, "Yuki Installer.exe"), path, true);
			RegistryKey ke =
				Registry.CurrentUser.CreateSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
			if (ke != null) ke.SetValue ("mode", ((int)Helper.mode).ToString ());

			// Set false to track install again
			Settings.database.UpdateData (Settings.LOGIN, "false");

			var a = Assembly.GetExecutingAssembly ();
			using (Stream str = (a.GetManifestResourceStream ($"{FileNamespace}files.txt")))
			{
				using (FileStream fstream =
				       File.Create (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
				                                  "Yuki Theme", "files.txt")))
				{
					str.Seek (0, SeekOrigin.Begin);
					str.CopyTo (fstream);
					using (StreamWriter sw = new StreamWriter (fstream))
					{
						string pth = FileNamespace + (Helper.mode == ProductMode.Program ? "files_program.txt" : "files_plugin.txt");
						using (StreamReader reader = new StreamReader (a.GetManifestResourceStream (pth)))
						{
							string cx = reader.ReadToEnd ();
							sw.WriteLine ("\n" + cx);
						}
					}
				}
			}

			Process.Start (path, $"Install \"{Application.ExecutablePath}\"");
			if (forceExit)
			{
				if (Helper.mode == ProductMode.Program)
					Application.Exit ();
				else if (Helper.mode == ProductMode.Plugin)
					System.Environment.Exit (1);
			}
		}
	}
}