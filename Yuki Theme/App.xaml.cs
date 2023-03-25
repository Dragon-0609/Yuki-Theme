using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Yuki_Theme.Core;
using System.Runtime.InteropServices;
using log4net;

namespace Yuki_Theme
{
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		public readonly ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

		public App()
		{
			try
			{
				Helper.mode = ProductMode.Program;


				// IntPtr handle = GetConsoleWindow();
				// Hide
				// ShowWindow(handle, SW_HIDE);
				InitializeComponent();
			}
			catch (Exception e)
			{
				Console.WriteLine($"ERROR: {e.Message}\nStack Trace:\n{e.StackTrace}");
				Log.Error("Error on initialization", e);
				Console.ReadLine();
			}
		}


		private void ErrorRaise(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.Message);
		}

		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		const int SW_HIDE = 0;
		const int SW_SHOW = 5;


		private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			Console.WriteLine($"ERROR: {e.Exception.Message}\nStack Trace:\n{e.Exception.StackTrace}");
			Log.Error("Error on runtime", e.Exception);
		}
	}
}