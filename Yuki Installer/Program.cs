using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using Microsoft.Win32;
using Yuki_Theme.Core;
using Yuki_Theme.Core.Database;

namespace Yuki_Installer
{
	
	internal class Program
	{

		private const string INSTALL_COMMAND   = "install";
		private const string DEPENDENCIES_FILE = "files.txt";

		private const string INSTALLER_FULL_NAME = "Yuki Installer.exe";
		private const string IDE_FULL_NAME       = "PascalABCNET.exe";
		private const string APP_FULL_NAME       = "Yuki_Theme.exe";
		private const string CLI_FULL_NAME       = "yuki.exe";

		private const string APP_NAME = "Yuki Theme";

		private static DatabaseManager _databaseManager = new DatabaseManager (false);

		private static string GetPathToInstallationZip =>
			Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), APP_NAME, "yuki_theme.zip");
		
		public static void Main (string [] args)
		{
			if (args != null && args.Length == 2)
			{
				if (WantToInstall (args))
				{
					StartInstallation (args);
				} else if (IsInstallationEnd (args))
				{
					FinishInsallation (args);
				} else
				{
					ShowErrorAndWait ($"ERROR> ARG ISN'T INSTALL. ARG: {args [0]}, ARGUMENTS: {args.Length}");
				}
			} else
			{
				string output = "ERROR> ARGS ARE NULL OR LENGTH ISN'T ENOUGH.";
				if (args != null)
					output += $"ARGUMENTS: {args.Length}";
				else
					output += "IS NULL: true";
				
				ShowErrorAndWait (output);
			}
		}

		#region Installation

		
		private static void StartInstallation (string [] args)
		{
			try
			{
				if (!File.Exists (GetPathToInstallationZip))
				{
					ShowErrorAndWait ($"ERROR> ZIP ISN'T EXIST. PATH: {GetPathToInstallationZip}");
				} else
				{
					if (CheckAppPath (args, out string pathToApp))
					{
						WaitAndStopApp (pathToApp);

						string appDirectory = Path.GetDirectoryName (pathToApp);
						string tempDirectory = appDirectory + "_tmp";

						Console.WriteLine ($"DIR> {appDirectory}");
						ClearTempDirectory (tempDirectory);
						MoveToTempDirectory (appDirectory, tempDirectory);

						ZipFile.ExtractToDirectory (GetPathToInstallationZip, appDirectory);
						Console.WriteLine ($"DIR2> {appDirectory}");

						ReturnThemesToApp (appDirectory, tempDirectory);

						if (DatabaseManager.IsLinux)
						{
							if (File.Exists (FileDatabase.GetSettingsPath ()))
							{
								File.Copy (FileDatabase.GetSettingsPath (), Path.Combine (appDirectory, FileDatabase.YUKI_SETTINGS));
							}
						}
						
						string installerPath = Path.Combine (appDirectory, INSTALLER_FULL_NAME);
						Process.Start (installerPath, $"\"{appDirectory}_tmp\" \"{GetPathToInstallationZip}\"");
					}
				}
			} catch (Exception e)
			{
				Console.WriteLine ($"ERROR> {e}");
			}
		}
		
		private static void FinishInsallation (string [] args)
		{
			try
			{
				RemoveGarbage (args);
			} catch (Exception e)
			{
				ShowErrorAndWait ($"ERROR> {e}");
			}

			RecordInstallation ();
			string fileName;
			bool cli = IsUpdateFromCLI ();
			if (!cli)
			{
				ProductMode mode = GetMode ();
				fileName = mode == ProductMode.Plugin ? IDE_FULL_NAME : APP_FULL_NAME;
			} else
			{
				fileName = CLI_FULL_NAME;
			}

			Process.Start (fileName);
		}

		
		private static bool WantToInstall (string [] args)
		{
			return args [0].ToLower () == INSTALL_COMMAND;
		}
		
		private static bool IsInstallationEnd (string [] args)
		{
			return Directory.Exists (args [0]);
		}
		
		private static void RecordInstallation ()
		{
			_databaseManager.SetValue ("install", "1");
		}

		
		#endregion


		#region Checker

		private static bool IsUpdateFromCLI ()
		{
			string value = _databaseManager.GetValue ("cli_update", "false");
			if (value.Length == 0)
				value = "false";
			return bool.Parse (value);
		}
		
		private static bool CheckAppPath (string [] args, out string path)
		{
			string temp = args [1];
			bool result = File.Exists (temp) || Directory.Exists (temp);
			path = result ? temp : "";	
			return result;
		}
		
		private static bool IsYukiThemeApp (string pathToIde, Process process)
		{
			return process.ProcessName == APP_NAME && process.MainModule != null && string.Compare (process.MainModule.FileName, pathToIde, StringComparison.InvariantCultureIgnoreCase) == 0;
		}

		#endregion
		

		#region Helper Methods

		private static void ClearTempDirectory (string tempDirectory)
		{
			if (Directory.Exists (tempDirectory))
			{
				Directory.Delete (tempDirectory, true);
			}
			Directory.CreateDirectory (tempDirectory);
		}
		
		private static void MoveToTempDirectory (string ideDirectory, string tempDirectory)
		{
			string [] lines = File.ReadAllLines (
				Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), APP_NAME, DEPENDENCIES_FILE));

			foreach (string line in lines)
			{
				try
				{
					string filePath = Path.Combine (ideDirectory, line);
					string tempFilePath = Path.Combine (tempDirectory, line);
					if (File.Exists (filePath))
						File.Move (filePath, tempFilePath);
					else
					{
						ShowErrorAndWait ($"ERROR> File not exist: {line}, in {filePath}");
					}
				} catch (IOException e)
				{
					Console.WriteLine ("ERROR> {0}", e);
				}
			}

			Directory.Move (Path.Combine (ideDirectory, "Themes"), Path.Combine (tempDirectory, "Themes"));
		}

		private static void ReturnThemesToApp (string appDirectory, string tempDirectory)
		{
			string themesDirectory = Path.Combine (appDirectory, "Themes");
			foreach (FileInfo file in new DirectoryInfo (Path.Combine (tempDirectory, "Themes")).GetFiles ())
			{
				try
				{
					file.MoveTo ($@"{themesDirectory}\{file.Name}");
				} catch (IOException e)
				{
					Console.WriteLine ("ERROR FS> {0}", e);
				} catch (Exception e)
				{
					Console.WriteLine ("ERROR> {0}", e);
				}
			}
		}
		
		private static void RemoveGarbage (string [] args)
		{
			Directory.Delete (args [0], true);
			File.Delete (args [1]);
			File.Delete (Path.Combine (
				             Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
				             APP_NAME,
				             INSTALLER_FULL_NAME));
			File.Delete (Path.Combine (
				             Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
				             APP_NAME,
				             DEPENDENCIES_FILE));
		}

		
		private static void WaitAndStopApp (string pathToApp)
		{
			Thread.Sleep (5000);

			Process [] runningProcesses = Process.GetProcesses ();
			foreach (Process process in runningProcesses) // Be sure that Yuki Theme isn't working
			{
				if (IsYukiThemeApp (pathToApp, process))
				{
					process.Kill ();
				}
			}
		}


		private static void ShowErrorAndWait (string text)
		{
			Console.WriteLine (text);
			Console.ReadLine ();
		}
		
		private static ProductMode GetMode ()
		{
			string value = _databaseManager.GetValue ("cli_update", "false");
			if (value.Length == 0)
				value = "0";
			return (ProductMode) int.Parse (value);
		}
		

		#endregion
		
	}
}
