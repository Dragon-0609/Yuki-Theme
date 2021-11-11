using System;
using System.Windows.Forms;

namespace Yuki_Theme.Forms
{
	public partial class SelectionForm : Form
	{
		public SelectionForm ()
		{
			InitializeComponent ();
			this.StartPosition = FormStartPosition.CenterParent;
		}

		private void button1_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void button2_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}