using System;
using System.IO;
using System.Windows.Forms;
using Yuki_Theme.Core.Forms;

namespace Yuki_Theme.Core.Parsers
{
	public static class MainParser
	{
		private static JetBrainsParser jetparser;
		private static DokiThemeParser dokiparser;

		public static void Parse (string path, MForm form, bool ask = true, bool select = true)
		{
			string st = Path.GetFileNameWithoutExtension (path);
			string pathe = $"Themes/{st}.yukitheme";
			if (checkAvailableAndAsk (path, pathe, form, ask))
			{
				string ext = Path.GetExtension (path);
				switch (ext)
				{
					case ".yukitheme" :
					{
						File.Copy (path, pathe, true);
						form.load_schemes ();
					}
						break;
					case ".icls" :
					{
						bool has = checkAvailable (pathe);
						jetparser = null;
						GC.Collect();
						GC.WaitForPendingFinalizers();
						jetparser = new JetBrainsParser ();
						jetparser.Parse (path, st, pathe, form, has, select);
					}
						break;
					case ".json" :
					{
						bool has = checkAvailable (pathe);
						dokiparser = null;
						GC.Collect();
						GC.WaitForPendingFinalizers();
						dokiparser = new DokiThemeParser ();
						dokiparser.Parse (path, st, pathe, form, has, select);
					}
						break;

				}
			}
		}

		private static bool checkAvailable (string nxpath)
		{
			return File.Exists (nxpath);
		}

		private static bool checkAvailableAndAsk (string path, string nxpath, MForm form, bool ask = true)
		{

			bool wants = true;
			if (File.Exists (nxpath) && ask)
			{
				wants = false;
				if (MessageBox.Show ((IWin32Window) form, "Theme is already exist. Do you want to override?",
				                     "Theme is exist",
				                     MessageBoxButtons.YesNo) == DialogResult.Yes) wants = true;
			}

			return wants;
		}
	}
}