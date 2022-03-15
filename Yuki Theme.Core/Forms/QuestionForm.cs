using System;
using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Forms
{
	internal partial class QuestionForm : Form
	{
		
		public QuestionForm ()
		{
			InitializeComponent ();
			this.StartPosition = FormStartPosition.CenterParent;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (MForm));
			Icon = ((Icon) (resources.GetObject ("$this.Icon")));
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

		public void EditMessage (string title, string content, string buttonYes, string buttonNo, string buttonOther = null)
		{
			Text = title;
			Message.Text = content;
			button2.Text = buttonYes;
			button1.Text = buttonNo;
			if (buttonOther != null)
			{
				button3.Text = buttonOther;
			} else
			{
				button3.Visible = false;
				button2.Location = new Point (button1.Location.X - button2.Size.Width - 8, button1.Location.Y);
			}
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