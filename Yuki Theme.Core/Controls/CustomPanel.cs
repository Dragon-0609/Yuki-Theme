using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Controls
{
	public class CustomPanel : Panel
	{
		public  int           mode = 0;
		public  CustomPicture pict;
		private Pen           lines;
		private Point         l1, l2, l1_2, l2_2;
		private Point         b1, b2, b1_2, b2_2;
		private bool          painted = false;
		private Color         bg;
		
		protected override CreateParams CreateParams 
		{            
			get {
				if (mode == 1)
				{
					CreateParams cp = base.CreateParams;
					cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
					return cp;
				} else
				{
					return base.CreateParams;
				}
			}
		}

		public CustomPanel (int md)
		{
			mode = md;
			if(md ==1)
				SetStyle(ControlStyles.Opaque, true);
		}
		
		public void Prepare ()
		{
			lines?.Dispose ();
			lines = new Pen (Helper.bgBorder, 5);
			bg = Helper.DarkerOrLighter (Helper.bgColor, 0.4f);
			lines.Alignment = PenAlignment.Center;
			Enabled = false;
			Size = Parent.ClientSize;
			Location = new Point (0, 0);
			l1 = new Point (pict.width3, 0);
			l1_2 = new Point (pict.width3, Size.Height);
			l2 = new Point (pict.width32, 0);
			l2_2 = new Point (pict.width32, Size.Height);
			b1 = new Point (0, pict.height3);
			b1_2 = new Point (Size.Width, pict.height3);
			b2 = new Point (0, pict.height32);
			b2_2 = new Point (Size.Width, pict.height32);
			painted = false;
			Visible = true;
			this.BringToFront ();
			pict.BringToFront ();
			Invalidate();
		}

		protected override void OnPaintBackground (PaintEventArgs pevent)
		{
			
		}

		protected override void OnPaint (PaintEventArgs e)
		{
			if (mode == 0)
			{
				e.Graphics.FillRectangle (new SolidBrush (Color.FromArgb (160, 0, 0, 0)), this.ClientRectangle);
			} else if (!painted)
			{
				// painted = true;
				e.Graphics.FillRectangle (new SolidBrush (Color.FromArgb (50, bg)), this.ClientRectangle);
				e.Graphics.DrawLine (lines, l1, l1_2);
				e.Graphics.DrawLine (lines, l2, l2_2);
				e.Graphics.DrawLine (lines, b1, b1_2);
				e.Graphics.DrawLine (lines, b2, b2_2);
			}
			base.OnPaint (e);
		}
	}
}