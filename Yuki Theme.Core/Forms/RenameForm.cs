using System;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Forms
{
	public partial class RenameForm : Form
	{
		public RenameForm ()
		{
			InitializeComponent ();
			this.StartPosition = FormStartPosition.CenterParent;
			button1.Text = API.Translate ("main.tips.save");
			button2.Text = API.Translate ("download.cancel");
			label1.Text = API.Translate ("messages.rename.from") + ":";
			label2.Text = API.Translate ("messages.rename.to") + ":";
			Text = API.Translate ("messages.rename.title");
		}
		private void button1_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void button2_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void RenameForm_Shown (object sender, EventArgs e)
		{
			BackColor = button1.BackColor = button2.BackColor =
				toTBox.BackColor = fromTBox.BackColor = Helper.bgColor;

			ForeColor = button1.FlatAppearance.BorderColor = button2.FlatAppearance.BorderColor =
				toTBox.ForeColor = fromTBox.ForeColor = Helper.fgColor;

			button1.FlatAppearance.MouseOverBackColor = button2.FlatAppearance.MouseOverBackColor =
				Helper.bgClick;

			toTBox.BorderColor = fromTBox.BorderColor = Helper.bgBorder;
		}
	}
}