using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme_Plugin.Controls
{
	public class CustomPanel: Panel
	{

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{

		}

		protected override void OnPaint(PaintEventArgs e)
		{

			e.Graphics.FillRectangle (new SolidBrush (Color.FromArgb (160, 0, 0, 0)), this.ClientRectangle);
		}
	}
}