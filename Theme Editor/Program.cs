using System;
using System.Windows.Forms;

namespace Theme_Editor
{
	public class Program
	{
		internal static WindowManager _manager;
		
		[STAThread]
		public static void Main (string[] args)
		{
			_manager = new WindowManager ();
			Server server = new Server ();
			Application.Run ();
		}
	}
}