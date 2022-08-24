using System;
using System.Windows;
using System.Windows.Threading;
using Yuki_Theme.Core;
using System.Runtime.InteropServices;

namespace Theme_Editor
{
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		internal static WindowManager _manager;
		
		public App ()
		{
			Helper.mode = ProductMode.Program;
			
			// HideConsole ();
			InitializeComponent ();
			
			ShutdownMode = ShutdownMode.OnExplicitShutdown;
			
			_manager = new WindowManager ();
			Server server = new Server ();
			
		}

		private void HideConsole ()
		{
			IntPtr handle = GetConsoleWindow ();
			// Hide
			ShowWindow (handle, SW_HIDE);
		}


		private void ErrorRaise (object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show (e.Exception.Message);
		}
		
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		const int SW_HIDE = 0;
		const int SW_SHOW = 5;
		
		
		
	}
}