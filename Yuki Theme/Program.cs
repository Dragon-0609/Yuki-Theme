using System;
using System.IO;
using System.Windows.Forms;
using Yuki_Theme.Forms;

namespace Yuki_Theme
{
	internal static class Program
	{
		/// <summary>
		///     The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main (string [] args)
		{
			if (args != null)
			{
				// Console.WriteLine (args.Length);
				// for (var i = 0; i < args.Length; i++) Console.WriteLine (args [i]);

				if (args.Length == 2)
					try
					{
						Directory.Delete (args [0], true);
						File.Delete (args [1]);
						File.Delete (Path.Combine (
							             Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
							             "Yuki Theme",
							             "Yuki Installer.exe"));
					} catch (Exception e)
					{
						MessageBox.Show (e.Message);
					}
			}

			Application.EnableVisualStyles ();
			Application.SetCompatibleTextRenderingDefault (false);
			Application.Run (new MForm ());
		}
	}
}