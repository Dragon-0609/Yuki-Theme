using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Yuki_Theme.Themes;

namespace Yuki_Theme.Forms
{
	public partial class ThemeManager : Form
	{
		private MForm form;
		
		public ThemeManager (MForm fm)
		{
			InitializeComponent ();
			this.StartPosition = FormStartPosition.CenterParent;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (MForm));
			Icon = ((Icon) (resources.GetObject ("$this.Icon")));
			form = fm;
		}

		private void button2_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void button3_Click (object sender, EventArgs e)
		{
			if (form.selform == null)
				form.selform = new SelectionForm ();
			form.selform.textBox1.Text = "";
			form.selform.comboBox1.Items.Clear ();
			foreach (object item in form.schemes.Items)
			{
				form.selform.comboBox1.Items.Add (item);
			}

			form.selform.comboBox1.SelectedIndex = 0;

			if (form.selform.ShowDialog () == DialogResult.OK)
			{
				string syt = form.selform.comboBox1.SelectedItem.ToString ();
				string patsh = $"Themes/{form.selform.textBox1.Text}.yukitheme";
				if (DefaultThemes.isDefault (syt))
					form.CopyFromMemory (syt, patsh);
				else
					File.Copy ($"Themes/{syt}.yukitheme", patsh );
				form.schemes.Items.Add (form.selform.textBox1.Text);
				form.schemes.SelectedItem = form.selform.textBox1.Text;
				DialogResult = DialogResult.OK;
			}
		}

		private void remove_Click (object sender, EventArgs e)
		{
			if (scheme.SelectedItems.Count > 0)
			{
				if (scheme.SelectedItems [0].Group != null)
				{
					string sel = scheme.SelectedItems [0].Text;
					DialogResult sr = MessageBox.Show ((IWin32Window) this, $"Do you really want to delete '{sel}'?", "Delete",
					                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (sr == DialogResult.Yes)
					{
						ListViewItem sifr = scheme.SelectedItems [0];
						if (form.selectedItem == sel || form.schemes.SelectedItem.ToString () == sel)
						{
							form.schemes.SelectedIndex = 0;
						}

						form.saveData ();
						File.Delete ($"Themes/{sel}.yukitheme");
						scheme.Items.Remove (sifr);
						form.schemes.Items.Remove (sel);
					}
				}
			}	
		}

		private void scheme_SelectedIndexChanged (object sender, EventArgs e)
		{
			remove.Enabled = false;
			remove.BackColor = SystemColors.ControlDark;
			if (scheme.SelectedItems.Count > 0)
			{
				if (scheme.SelectedItems [0].Group != null)
				{
					remove.Enabled = true;
					remove.BackColor = SystemColors.Control;
				}
			}
		}
	}
}