using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using YukiTheme;
using YukiTheme.Tools;

namespace Yuki_Theme.Core.Utils
{
	public class InstallationPreparer
	{
		public const  string FILE_NAMESPACE = "YukiTheme.Resources.";
		private const string APP_NAME       = "Yuki Theme";
		const string INSTALLER_NAME = "Yuki Installer.exe";

		private string GetAppDataDirectory =>
			Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), APP_NAME);
		
		public void Prepare (bool forceExit)
		{
			
			string path = Path.Combine ( GetAppDataDirectory, INSTALLER_NAME);
			// Console.WriteLine ($"Copying from {Path.Combine (YukiTheme_VisualPascalABCPlugin.GetCurrentFolder, INSTALLER_NAME)} to {path}, exists: {File.Exists(Path.Combine (YukiTheme_VisualPascalABCPlugin.GetCurrentFolder, INSTALLER_NAME))}");
			File.Copy (Path.Combine (YukiTheme_VisualPascalABCPlugin.GetCurrentFolder, INSTALLER_NAME), path, true);

			ExtractFilesToDelete();
			
			Process.Start (path, $"Install \"{Application.ExecutablePath}\"");
			CloseApp(forceExit);
		}

		private static void ExtractFilesToDelete()
		{
			Assembly a = Assembly.GetExecutingAssembly();
			using (Stream str = (a.GetManifestResourceStream($"{FILE_NAMESPACE}files.txt")))
			{
				using (FileStream fstream =
					File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
						APP_NAME, "files.txt")))
				{
					str.Seek(0, SeekOrigin.Begin);
					str.CopyTo(fstream);
				}
			}
		}

		private static void CloseApp(bool forceExit)
		{
			if (forceExit)
			{
					Environment.Exit(1);
			}
		}
	}
}