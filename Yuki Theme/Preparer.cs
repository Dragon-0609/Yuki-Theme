using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Yuki_Theme
{
	public class Preparer
	{
		public void prepare ()
		{
			string path = Path.Combine (
				Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "Yuki Theme",
				"Yuki Installer.exe");
			File.Copy ("Yuki Installer.exe", path,true);
			Process.Start (path, $"Install \"{Application.ExecutablePath}\"");
			Application.Exit();
		}
	}
}