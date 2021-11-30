using System;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Forms
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

		private void SelectionForm_Shown (object sender, EventArgs e)
		{
			BackColor = button1.BackColor = button2.BackColor =comboBox1.ListBackColor = comboBox1.BackColor = 
				textBox1.BackColor = Helper.bgColor;
				
			ForeColor = button1.FlatAppearance.BorderColor = button2.FlatAppearance.BorderColor =
				comboBox1.ForeColor = comboBox1.ListTextColor = textBox1.ForeColor = Helper.fgColor;
						
			button1.FlatAppearance.MouseOverBackColor = button2.FlatAppearance.MouseOverBackColor =
				Helper.bgClick;

			comboBox1.BorderColor = comboBox1.IconColor = textBox1.BorderColor = Helper.bgBorder;
		}
	}
}