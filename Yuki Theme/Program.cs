using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Yuki_Theme.Core.Forms;

namespace Yuki_Theme
{
	internal static class Program
	{
		/// <summary>
		///     The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main ()
		{
			try
			{
				if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();
				Application.EnableVisualStyles ();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run (new MForm ());
			}
			catch (Exception e)
			{
				Console.WriteLine($"ERROR: {e.Message}\nStack Trace:\n{e.StackTrace}");
				File.WriteAllText("yuki.log", $"ERROR: {e.Message}\nStack Trace:\n{e.StackTrace}");
				Console.ReadLine();
			}
			// Application.Run (new ThemeDownloaderForm ());
		}
		
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool SetProcessDPIAware();
	}
}