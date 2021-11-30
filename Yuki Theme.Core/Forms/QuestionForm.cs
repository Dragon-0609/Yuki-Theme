using System;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Forms
{
	partial class QuestionForm : Form
	{
		
		public QuestionForm ()
		{
			InitializeComponent ();
			this.StartPosition = FormStartPosition.CenterParent;
		}

		private void button2_Click (object sender, EventArgs e)
		{
		DialogResult = DialogResult.Yes;
		}

		private void button3_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.Ignore;
		}

		private void button1_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.No;
		}

		private void QuestionForm_Shown (object sender, EventArgs e)
		{
			button1.BackColor = button2.BackColor = button3.BackColor = BackColor = Helper.bgColor;
			
			button1.ForeColor = button2.ForeColor = button3.ForeColor = ForeColor = Helper.fgColor;
			
			button1.FlatAppearance.MouseOverBackColor = button2.FlatAppearance.MouseOverBackColor = 
			button3.FlatAppearance.MouseOverBackColor = Helper.bgClick;
		}
	}
}