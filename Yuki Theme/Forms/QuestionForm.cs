using System;
using System.IO;
using System.Windows.Forms;

namespace Yuki_Theme.Forms
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
	}
}