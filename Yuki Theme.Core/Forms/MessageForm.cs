using System;
using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Forms
{
	partial class MessageForm:Form
	{
		public MessageForm ()
		{
			InitializeComponent();
			StartPosition = FormStartPosition.CenterParent;
		}

		public void setMessage (string title, string description, string btntext = "OK")
		{
			label1.Text = title;
			label2.Text = description;
			button1.Text = btntext;
		}

		public void setColors (Color bg, Color fg, Color border)
		{
			BackColor = bg;
			ForeColor = fg;
			button1.BackColor = bg;
			button1.ForeColor = fg;
			button1.FlatAppearance.BorderColor = border;
		}

	}
}