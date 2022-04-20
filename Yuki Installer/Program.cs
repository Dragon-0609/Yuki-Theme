using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.Win32;

namespace Yuki_Installer
{
	internal class Program
	{
		public static void Main (string [] args)
		{
			if (args != null && args.Length == 2)
			{
				if (args [0].ToLower () == "install")
				{
					try
					{
						Thread.Sleep (5000);

						string zip =
							Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
							              "Yuki Theme", "yuki_theme.zip");

						if (!File.Exists (zip))
						{
							Console.WriteLine (
								$"ERROR> ZIP ISN'T EXIST. PATH: {zip}");
							Console.ReadLine ();
						} else
						{
							string path = args [1];
							string targetProcessName = "Yuki Theme";

							Process [] runningProcesses = Process.GetProcesses ();
							foreach (Process process in runningProcesses) // Be sure that Yuki Theme isn't working
							{
								if (process.ProcessName == targetProcessName &&
								    process.MainModule != null &&
								    string.Compare (process.MainModule.FileName, path,
								                    StringComparison.InvariantCultureIgnoreCase) == 0)
								{
									process.Kill ();
								}
							}

							string dir = Path.GetDirectoryName (path);
							string ndr = dir + "_tmp";
							Console.WriteLine ($"DIR> {dir}");
							
							if (Directory.Exists (ndr))
							{
								Directory.Delete (ndr, true);
							}

							Directory.CreateDirectory (ndr);
							string [] lines = File.ReadAllLines (
								Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
								              "Yuki Theme", "files.txt"));

							foreach (string line in lines)
							{
								try
								{
									string fileName = Path.Combine (dir, line);
									if (File.Exists (fileName))
										File.Move (fileName, $@"{ndr}\{line}");
									else
									{
										Console.WriteLine ("ERROR> File not exist: {0}, in {1}", line, fileName);
										Console.ReadLine ();
									}
								} catch (IOException e)
								{
									Console.WriteLine ("ERROR> {0}", e);
								}
								// File.Delete (line);
							}

							Directory.Move (Path.Combine (dir, "Themes"), Path.Combine (ndr, "Themes"));

							System.IO.Compression.ZipFile.ExtractToDirectory (zip, dir);
							string ndir = Path.Combine (dir, "Themes");
							Console.WriteLine ($"DIR2> {dir}");

							foreach (var file in new DirectoryInfo (Path.Combine (ndr, "Themes")).GetFiles ())
							{
								try
								{
									file.MoveTo ($@"{ndir}\{file.Name}");
								} catch (IOException e)
								{
									Console.WriteLine ("ERROR FS> {0}", e);
								} catch (Exception e)
								{
									Console.WriteLine ("ERROR> {0}", e);
								}
							}
							
							ndir = Path.Combine (dir, "Yuki Installer.exe");
							Process.Start (ndir, $"\"{dir}_tmp\" \"{zip}\"");
						}
					} catch (Exception e)
					{
						Console.WriteLine (
							$"ERROR> {e.ToString ()}");
					}
				} else if (Directory.Exists (args [0]))
				{

					try
					{
						Directory.Delete (args [0], true);
						File.Delete (args [1]);
						File.Delete (Path.Combine (
							             Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
							             "Yuki Theme",
							             "Yuki Installer.exe"));
						File.Delete (Path.Combine (
							             Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
							             "Yuki Theme",
							             "files.txt"));
					} catch (Exception e)
					{
						Console.WriteLine (
							$"ERROR> {e.ToString ()}");
						Console.ReadLine ();
					}

					RegistryKey ke =
						Registry.CurrentUser.CreateSubKey (@"SOFTWARE\YukiTheme",
						                                   RegistryKeyPermissionCheck.ReadWriteSubTree);
					ke.SetValue ("install", "1");

					int mode = getMode ();
					
					bool cli = isCLIUpdate ();
					if (!cli)
					{
						switch (mode)
						{
							case 0 :
							{
								Process.Start ("Yuki_Theme.exe");
							}
								break;
							case 1 :
							{
								Process.Start ("PascalABCNET.exe");
							}
								break;
						}
					} else
					{
						Process.Start ("yuki.exe");
					}
				} else
				{
					Console.WriteLine (
						$"ERROR> ARG ISN'T INSTALL. ARG: {args [0]}, LENGTH: {args.Length}");
					Console.ReadLine ();
				}
			} else
			{
				Console.WriteLine (
					$"ERROR> ARGS ARE NULL OR LENGTH ISN'T ENOUGH. IS NULL: {args == null}, LENGTH: {args.Length}");
				Console.ReadLine ();
			}
		}

		private static bool isCLIUpdate ()
		{
			RegistryKey ke = Registry.CurrentUser.CreateSubKey (@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
			return bool.Parse (ke.GetValue ("cli_update", "false").ToString ());
		}

		private static int getMode ()
		{
			RegistryKey ke = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\YukiTheme", RegistryKeyPermissionCheck.ReadWriteSubTree);
			return int.Parse (ke.GetValue ("mode").ToString ());
		}
	}
}
