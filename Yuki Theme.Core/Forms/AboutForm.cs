using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Forms
{
	public partial class AboutForm : Form
	{
		public AboutForm ()
		{
			InitializeComponent ();
			this.StartPosition = FormStartPosition.CenterParent;
			vers.Text =
				$"version: {SettingsForm.current_version.ToString ("0.0").Replace (',', '.')} {SettingsForm.current_version_add}";
			System.ComponentModel.ComponentResourceManager resources =
				new System.ComponentModel.ComponentResourceManager (typeof (MForm));
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

		private void linkLabel5_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://github.com/Dragon-0609");
		}

		private void linkLabel6_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://github.com/JamesNK/Newtonsoft.Json");
		}

		private void AboutForm_Shown (object sender, EventArgs e)
		{
			button1.BackColor = panel1.BackColor = BackColor = Helper.bgColor;
			
			button1.ForeColor = linkLabel1.LinkColor = linkLabel2.LinkColor = linkLabel3.LinkColor =
				linkLabel4.LinkColor = linkLabel5.LinkColor = linkLabel6.LinkColor = linkLabel7.LinkColor = 
				linkLabel8.LinkColor = linkLabel9.LinkColor = linkLabel10.LinkColor = linkLabel11.LinkColor = 
					linkLabel12.LinkColor = ForeColor = Helper.fgColor;
				
			linkLabel1.ActiveLinkColor = linkLabel2.ActiveLinkColor = linkLabel3.ActiveLinkColor =
				linkLabel4.ActiveLinkColor = linkLabel5.ActiveLinkColor = linkLabel6.ActiveLinkColor = 
				linkLabel7.ActiveLinkColor = linkLabel8.ActiveLinkColor = linkLabel9.ActiveLinkColor = 
				linkLabel10.ActiveLinkColor = linkLabel11.ActiveLinkColor = linkLabel12.ActiveLinkColor = Helper.fgKeyword;
				
			button1.FlatAppearance.MouseOverBackColor = Helper.bgClick;
		}

		private void linkLabel7_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://github.com/RJCodeAdvance/Custom-ComboBox");
		}

		private void linkLabel8_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://github.com/svg-net/SVG");
		}

		private void linkLabel9_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://icons.getbootstrap.com/");
		}

		private void linkLabel10_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://github.com/fabricelacharme/ColorSlider");
		}

		private void linkLabel11_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://www.nuget.org/packages/WindowsAPICodePack");
		}

		private void linkLabel12_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://github.com/r-aghaei/FlatNumericUpDownExample");
		}

		private void linkLabel13_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://www.mechanikadesign.com/software/colorpicker-controls-for-windows-forms/");
		}
	}
}