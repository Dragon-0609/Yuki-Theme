using System;
using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme_Plugin
{
	public class MenuRenderer:ToolStripProfessionalRenderer
	{
		protected override void OnRenderSeparator (ToolStripSeparatorRenderEventArgs e)
		{
			if((e.Item as ToolStripSeparator) == null)
			{
				base.OnRenderSeparator (e);
				return;
			}

			Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);

			using (SolidBrush brush = new SolidBrush (e.Item.BackColor))
				e.Graphics.FillRectangle (brush, bounds);

			bounds.Y = bounds.Height / 2;

			using (Pen pen = new Pen (Color.Silver))
				e.Graphics.DrawLine (pen, 0, bounds.Y, bounds.Width, bounds.Y);
		}
	}
}