using System;
using System.Windows.Forms;

namespace Yuki_Theme.Forms
{
	public partial class NotificationForm : Form
	{
		public NotificationForm ()
		{
			InitializeComponent ();
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
	}
}