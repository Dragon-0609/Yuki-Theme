using System.Drawing;
using System.Windows.Forms;
using YukiTheme.Engine;

namespace YukiTheme.Style
{
	public class ToolRenderer : ToolStripProfessionalRenderer
	{
		public ToolRenderer() : base(new MyColors())
		{
		}

		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			if ((e.Item as ToolStripSeparator) == null)
			{
				base.OnRenderSeparator(e);
				return;
			}

			Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);

			using (SolidBrush brush = new SolidBrush(e.Item.BackColor))
				e.Graphics.FillRectangle(brush, bounds.X, bounds.Y + 1, bounds.Width, bounds.Height - 1);
			bounds.Y = bounds.Height / 4;
			bounds.X = bounds.Width / 2;
			bounds.Height -= bounds.Height / 4;
			e.Graphics.DrawLine(ColorReference.BackgroundClick3Pen, bounds.X, bounds.Y, bounds.X, bounds.Height);
		}
	}
}