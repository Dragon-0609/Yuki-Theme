using System.Windows;
using System.Windows.Threading;

namespace Yuki_Theme.Core.WPF
{
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private void ErrorRaise (object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show (e.Exception.Message);
		}
	}
}