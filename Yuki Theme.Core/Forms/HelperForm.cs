using System;
using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Forms
{
	partial class HelperForm:Form
	{
		public HelperForm ()
		{
			InitializeComponent();
			this.StartPosition = FormStartPosition.CenterParent;
		}

		public void setMessage (string title, string description, string btntext = "OK")
		{
			Text = title;
			label2.Text = description;
			button1.Text = btntext;
			Height = 100 + panel1.Height;
		}

		public void setColors (Color bg, Color fg, Color border)
		{
			BackColor = bg;
			ForeColor = fg;
			button1.BackColor = bg;
			button1.ForeColor = fg;
			button1.FlatAppearance.BorderColor = border;
		}

		private void button1_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}