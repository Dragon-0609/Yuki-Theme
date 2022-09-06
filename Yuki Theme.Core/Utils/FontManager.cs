using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Utils
{
	public static class FontManager
	{
		[DllImport ("gdi32.dll")]
		private static extern IntPtr AddFontMemResourceEx (IntPtr pbFont, uint                                         cbFont,
		                                                   IntPtr pdv,    [In] ref uint pcFonts);

		private static PrivateFontCollection _fonts = new PrivateFontCollection ();

		private static bool _initialized = false;

		private static void LoadFont (string resource)
		{
			// receive resource stream
			Stream fontStream = Assembly.GetExecutingAssembly ().GetManifestResourceStream (resource);

			// create an unsafe memory block for the font data
			IntPtr data = Marshal.AllocCoTaskMem ((int) fontStream.Length);

			// create a buffer to read in to
			byte [] fontdata = new byte[fontStream.Length];

			// read the font data from the resource
			fontStream.Read (fontdata, 0, (int) fontStream.Length);

			// copy the bytes to the unsafe memory block
			Marshal.Copy (fontdata, 0, data, (int) fontStream.Length);

			// pass the font to the font collection
			_fonts.AddMemoryFont (data, (int) fontStream.Length);

			// close the resource stream
			fontStream.Close ();

			// free up the unsafe memory
			Marshal.FreeCoTaskMem (data);
		}
		
		private static void Initialize ()
		{
			LoadFont("Yuki_Theme.Core.Resources.Fonts.Montserrat-Regular.ttf");
			LoadFont ("Yuki_Theme.Core.Resources.Fonts.SAOUITT-Regular.ttf");
			_initialized = true;
		}

		public static FontFamily GetFont (int which)
		{
			if (!_initialized)
				Initialize ();
			return _fonts.Families [which];
		}
		
		public static void SetAllControlsFont(Control.ControlCollection ctrls, int which) {
			foreach(Control c in ctrls) {
				if(c.Controls != null) {
					SetAllControlsFont(c.Controls, which);
				}

				SetControlFont (c, which);
			}
		}

		public static void SetControlFont (Control c, int which)
		{
			c.Font = new Font (GetFont (which), c.Font.Size);
		}
		
	}
}