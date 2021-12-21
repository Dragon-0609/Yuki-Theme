using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Yuki_Theme_Plugin.Controls
{
	public class CustomList : ListBox
	{
		public CustomList ()
		{
			BorderStyle = BorderStyle.None;
		}
		
		/*
		protected override void WndProc(ref Message m)
		{
			var switchExpr = m.Msg;
			switch (switchExpr)
			{
				case 0xF:
				{
					Graphics g = this.CreateGraphics();
					g.SmoothingMode = SmoothingMode.Default;
					Rectangle rect = ClientRectangle;
					g.DrawRectangle (YukiTheme_VisualPascalABCPlugin.bgPen, rect);

					break;
				}

				default:
				{
					break;
				}
			}
			base.WndProc(ref m);
		}*/
	}
}