using System;
using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Forms
{
	partial class QuestionForm_2 : Form
	{
		
		public QuestionForm_2 ()
		{
			InitializeComponent ();
			this.StartPosition = FormStartPosition.CenterParent;
		}

		private void button2_Click (object sender, EventArgs e)
		{
		DialogResult = DialogResult.Yes;
		}
		
		private void button1_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.No;
		}

		public void setColors (Color bg, Color fg, Color bgclick)
		{
			button1.BackColor = button2.BackColor = BackColor = bg;
			
			button1.ForeColor = button2.ForeColor = ForeColor = fg;
			
			button1.FlatAppearance.MouseOverBackColor = button2.FlatAppearance.MouseOverBackColor = bgclick;
		}
	}
}