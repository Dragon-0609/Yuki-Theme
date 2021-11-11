using System;
using System.Diagnostics;
using System.Deployment;
using System.Deployment.Application;
using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme.Forms
{
	public partial class AboutForm : Form
	{
		public AboutForm ()
		{
			InitializeComponent ();
			this.StartPosition = FormStartPosition.CenterParent;
			string sr =	Application.ProductVersion.ToString ();

			vers.Text = $"version: {sr}";
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (MForm));
			Icon = ((Icon) (resources.GetObject ("$this.Icon")));
		}

		private void button1_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void linkLabel1_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://github.com/doki-theme/doki-theme-jetbrains");
		}

		private void linkLabel2_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://github.com/PavelTorgashov/FastColoredTextBox");
		}

		private void linkLabel3_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://github.com/cyotek/Cyotek.Windows.Forms.ColorPicker");
		}

		private void linkLabel4_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://github.com/Dragon-0609/Yuki-Theme");
		}
	}
}