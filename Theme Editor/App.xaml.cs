using Yuki_Theme.Core;

namespace Theme_Editor
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		internal static WindowManager _manager;

		public App ()
		{
			Helper.mode = ProductMode.Plugin;
			
			InitializeComponent ();
			
			_manager = new WindowManager ();
			Server server = new Server ();
		}
		
		
	}
}