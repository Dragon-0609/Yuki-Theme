using System;
using System.Windows;
using System.Windows.Threading;
using Yuki_Theme.Core;
using System.Runtime.InteropServices;

namespace Yuki_Theme
{
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		public App ()
		{
			Helper.mode = ProductMode.Program;
			
			IntPtr handle = GetConsoleWindow();
			// Hide
			ShowWindow(handle, SW_HIDE);
			InitializeComponent ();

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