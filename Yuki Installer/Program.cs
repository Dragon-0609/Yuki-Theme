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
			if(args != null && args.Length==2)
			{
				if(args[0].ToLower() == "install")
				{
					try
					{
					Thread.Sleep (5000);
					
					string zip = Path.Combine ( Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "Yuki Theme", "yuki_theme.zip");
					if (!File.Exists (zip))Console.WriteLine (
						$"ERROR> ZIP ISN'T EXIST. PATH: {zip}");
					string path = args [1];
					string targetProcessName = "Yuki Theme";

					Process [] runningProcesses = Process.GetProcesses ();
					foreach (Process process in runningProcesses)
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
					// Directory.Move (dir, dir + "_tmp");
					if (Directory.Exists (ndr))
					{
						Directory.Delete (ndr,true);
					}

					Directory.CreateDirectory (ndr);
					foreach (var file in new DirectoryInfo(dir).GetFiles())
					{
						try
						{
							file.MoveTo($@"{ndr}\{file.Name}");
						} catch (IOException e)
						{
							Console.WriteLine ("ERROR> {0}",e);
						}
					}
					Directory.Move (Path.Combine (dir, "Themes"), Path.Combine (ndr, "Themes"));
					
					System.IO.Compression.ZipFile.ExtractToDirectory(zip, dir);
					string ndir = Path.Combine (dir, "Themes");
					Console.WriteLine ($"DIR2> {dir}");
					
					foreach (var file in new DirectoryInfo(Path.Combine (ndr, "Themes")).GetFiles())
					{
						try
						{
							file.MoveTo($@"{ndir}\{file.Name}");
						} catch (IOException e)
						{
							Console.WriteLine ("ERROR> {0}",e);
						}
					}
					// Directory.Move (Path.Combine (ndr, "Themes"), Path.Combine (dir, "Themes"));
					/*foreach (var file in new DirectoryInfo(Path.Combine (dir+ "_tmp", "Themes")).GetFiles())
					{
						try
						{
							file.MoveTo($@"{ndir}\{file.Name}");
						} catch (IOException e)
						{
							Console.WriteLine (e);
						}
					}*/

					Process.Start (path, $"\"{dir}_tmp\" \"{zip}\"");
					} catch (Exception e)
					{
						Console.WriteLine (
							$"ERROR> {e.ToString ()}");
					}
				} else
				{
					Console.WriteLine (
						$"ERROR> ARG ISN'T INSTALL. ARG: {args[0]}, LENGTH: {args.Length}");
				}
			} else
			{
				Console.WriteLine (
					$"ERROR> ARGS ARE NULL OR LENGTH ISN'T ENOUGH. IS NULL: {args == null}, LENGTH: {args.Length}");
			}
		}
	}
}