using System;
using System.Windows.Forms;

namespace Theme_Editor
{
	public class Program
	{

		[STAThread]
		public static void Main (string[] args)
		{
			Server server = new Server ();
			Application.Run ();
		}
	}
}