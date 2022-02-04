using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Controls
{
	public class RoundLabel : Label
	{
		[Browsable (true)]
		public Color _BackColor { get; set; }

		[Browsable (true)]
		public int Radius { get; set; } = 15;

		public RoundLabel ()
		{
			this.DoubleBuffered = true;
		}

		protected override void OnPaint (PaintEventArgs e)
		{
			base.OnPaint (e);
			using (var graphicsPath = _getRoundRectangle (this.ClientRectangle))
			{
				e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				using (var brush = new SolidBrush (_BackColor))
					e.Graphics.FillPath (brush, graphicsPath);
				using (var pen = new Pen (_BackColor, 1.0f))
					e.Graphics.DrawPath (pen, graphicsPath);
				TextRenderer.DrawText (e.Graphics, Text, this.Font, this.ClientRectangle, this.ForeColor,
				                       TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
			}
		}

		private GraphicsPath _getRoundRectangle (Rectangle rectangle)
		{
			int diminisher = 1;
			GraphicsPath path = new GraphicsPath ();
			path.AddArc (rectangle.X, rectangle.Y, Radius, Radius, 180, 90);
			path.AddArc (rectangle.X + rectangle.Width - Radius - diminisher, rectangle.Y, Radius, Radius, 270, 90);
			path.AddArc (rectangle.X + rectangle.Width - Radius - diminisher,
			             rectangle.Y + rectangle.Height - Radius - diminisher, Radius, Radius, 0, 90);
			path.AddArc (rectangle.X, rectangle.Y + rectangle.Height - Radius - diminisher, Radius, Radius, 90, 90);
			path.CloseAllFigures ();
			return path;
		}
	}
}