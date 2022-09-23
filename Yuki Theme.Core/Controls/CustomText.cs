using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Controls
{
	public class CustomText : TextBox
	{
		Color borderColor = Color.Blue;
		public Color BorderColor {
			get { return borderColor; }
			set { borderColor = value;
				RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero,
				             RDW_FRAME | RDW_IUPDATENOW | RDW_INVALIDATE);
			}
		}
	
		[DllImport("user32")]
		private static extern IntPtr GetWindowDC(IntPtr hwnd);
		
		[DllImport("user32.dll")]
		static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
		
		[DllImport("user32.dll")]
        static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprc, IntPtr hrgn, uint flags);
		
		private const int  WM_NCPAINT     = 0x85;
		const         uint RDW_INVALIDATE = 0x1;
		const         uint RDW_IUPDATENOW = 0x100;
		const         uint RDW_FRAME      = 0x400;
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (m.Msg == WM_NCPAINT)
			{
				var dc = GetWindowDC(Handle);
				using (Graphics g = Graphics.FromHdc(dc))
				{
					using (var p = new Pen(BorderColor,2))
						g.DrawRectangle(p, new Rectangle(1, 1, Width - 2, Height - 2));
				}
				ReleaseDC(Handle, dc);
			}
		}

		
		
		public void CallKeyUp (KeyEventArgs e)
		{
			OnKeyUp (e);
			e.Handled = true;
		}
		
		public void CallKeyDown (KeyEventArgs e)
		{
			OnKeyDown (e);
			e.Handled = true;
		}
		
		public void CallKeyPress (KeyPressEventArgs e)
		{
			OnKeyPress (e);
			e.Handled = true;
		}
	}
}