using System.Windows;
using System.Windows.Threading;
using Yuki_Theme.Core;

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
			InitializeComponent ();
			
		}
		
		
		private void ErrorRaise (object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show (e.Exception.Message);
		}
	}
}