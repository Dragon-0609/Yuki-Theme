using System;
using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Forms
{
	public partial class NotificationForm : Form
	{
		public NotificationForm ()
		{
			InitializeComponent ();
			AllowTransparency = true;
		}
		
		public void changeContent(string title, string content){
			ttitle.Text = title;
			tcontent.Text = content;
		}

		private void label1_Click (object sender, EventArgs e)
		{
			this.Close ();
		}

		private void button1_Click (object sender, EventArgs e)
		{
			this.Close ();
		}

		private void NotificationForm_FormClosing (object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
			this.Parent = null;
		}

		public void NotificationForm_Shown (object sender, EventArgs e)
		{
			button2.FlatAppearance.MouseDownBackColor = button2.BackColor =
					button2.FlatAppearance.MouseOverBackColor = tcontent.BackColor = Color.Transparent;
			BackColor = button2.FlatAppearance.BorderColor = Helper.bgColor;
			button2.ForeColor = button1.ForeColor = button1.VisitedLinkColor = ForeColor = Helper.fgColor;
			button1.LinkColor = Helper.fgHover;
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			ControlPaint.DrawBorder (e.Graphics, ClientRectangle, Helper.bgBorder, ButtonBorderStyle.Solid);
		}

		private void button2_MouseEnter (object sender, EventArgs e)
		{
			button2.ForeColor = Helper.fgHover;
		}

		private void button2_MouseLeave (object sender, EventArgs e)
		{
			button2.ForeColor = Helper.fgColor;
		}
	}
}