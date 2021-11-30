using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using Yuki_Theme.Core.Forms;

namespace Yuki_Theme.Core
{
	public class Preparer
	{
		public void prepare ()
		{
			string path = Path.Combine (
				Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "Yuki Theme",
				"Yuki Installer.exe");
			File.Copy ("Yuki Installer.exe", path, true);
			RegistryKey ke =
				Registry.CurrentUser.CreateSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
			ke.SetValue ("mode", ((int) Helper.mode).ToString ());

			var a = Assembly.GetExecutingAssembly ();
			using (Stream str = (a.GetManifestResourceStream ($"Yuki_Theme.Core.Database.files.txt")))
			{
				using (var fstream =
					File.Create (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
					                           "Yuki Theme", "files.txt")))
				{
					str.Seek (0, SeekOrigin.Begin);
					str.CopyTo (fstream);
				}
			}

			Process.Start (path, $"Install \"{Application.ExecutablePath}\"");
			if (Helper.mode == ProductMode.Program)
				Application.Exit ();
			else if (Helper.mode == ProductMode.Plugin)
				System.Environment.Exit (1);
		}
	}
}