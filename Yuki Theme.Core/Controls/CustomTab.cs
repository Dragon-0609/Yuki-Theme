
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Controls
{
	public class CustomTab : TabControl
	{
		public Brush bg = SystemBrushes.Control;
		public Brush bgClick = SystemBrushes.ControlDark;

		protected override bool ShowFocusCues => false;

		public CustomTab () : base ()
		{
			// this.SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
			DoubleBuffered = ResizeRedraw = true;
			Click += _Click;
			// DrawMode = TabDrawMode.OwnerDrawFixed;
			// DrawItem += DrawingItem;
		}
		
		public override Color BackColor {
			// Override TabControl.BackColor, we need transparency
			get { return Color.Transparent; }
			set { base.BackColor = Color.Transparent; }
		}

		protected virtual void DrawTabRectangle (Graphics g, int index, Rectangle r)
		{
			if (index == 0) r = new Rectangle (r.Left - 2, r.Top, r.Width + 2, r.Height);
			if (index != SelectedIndex) r = new Rectangle (r.Left, r.Top + 2, r.Width, r.Height - 2);
			if (index == SelectedIndex)
			{
				g.FillRectangle (bgClick, r);
			} else
			{
				g.FillRectangle (bg, r);
			}
		}

		protected virtual void DrawTab(Graphics g, int index, Rectangle r) {
			r.Inflate(-1, -1);
			TextRenderer.DrawText(g, TabPages[index].Text, Font,
			                      r, ForeColor, 
			                      TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
		}

		protected override void OnPaint(PaintEventArgs e) {
			if (TabCount <= 0) return;
			// Draw tabpage area
			Rectangle r = ClientRectangle;
			var top = GetTabRect(0).Bottom;
				e.Graphics.FillRectangle(bg, new Rectangle(r.Left, top, r.Width, r.Height - top));
			// Draw tabs
			for (int index = 0; index < TabCount; index++) {
				r = GetTabRect(index);
				DrawTabRectangle(e.Graphics, index, r);
				DrawTab(e.Graphics, index, r);
				if (index == SelectedIndex) {
					r.Inflate(-1, -1);
					ControlPaint.DrawFocusRectangle(e.Graphics, r);
				}
			}
		}/*

		protected override void OnPaintBackground (PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			Rectangle lasttabrect = GetTabRect(TabPages.Count - 1);
			RectangleF emptyspacerect = new RectangleF(
				lasttabrect.X + lasttabrect.Width + Left,
				Top + lasttabrect.Y, 
				Width - (lasttabrect.X + lasttabrect.Width), 
				lasttabrect.Height);

			Brush b = Brushes.BlueViolet; // the color you want
			e.Graphics.FillRectangle(b, emptyspacerect );
		}
		*/

		private void DrawingItem (object sender, DrawItemEventArgs e)
		{
			TabPage page = TabPages [e.Index];
			if (e.Index == SelectedIndex)
			{
				e.Graphics.FillRectangle (bgClick, e.Bounds);
			} else
			{
				e.Graphics.FillRectangle (bg, e.Bounds);
			}


			Rectangle paddedBounds = e.Bounds;
			int yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
			paddedBounds.Offset (1, yOffset);
			TextRenderer.DrawText (e.Graphics, page.Text, e.Font, paddedBounds, ForeColor);
		}/*
		
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 0x0005) // WM_SIZE
			{
				int Width = unchecked((short)m.LParam);
				int Height = unchecked((short)((uint)m.LParam >> 16));

				// Remove the annoying white pixels on the outside of the tab control
				// by adjusting the control's clipping region to exclude the 2 pixels
				// on the right and one pixel on the bottom.
				Region = new Region(new Rectangle(1, 0, Width - 3, Height - 2));
			}

			base.WndProc(ref m);
		}*/

		private void _Click (object sender, EventArgs e)
		{
			(sender as TabControl).SelectedTab.Focus ();
		}
	}
}