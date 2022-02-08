using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Yuki_Theme.Core.Controls;

namespace Yuki_Theme.Core.Forms
{
	public partial class AboutForm : Form
	{
		public  bool          isFromPascal = false;
		private SettingsPanel sp;
		
		public AboutForm (SettingsPanel s)
		{
			InitializeComponent ();
			this.StartPosition = FormStartPosition.CenterParent;
			vers.Text =
				$"version: {SettingsForm.current_version.ToString ("0.0").Replace (',', '.')} {SettingsForm.current_version_add}";
			System.ComponentModel.ComponentResourceManager resources =
				new System.ComponentModel.ComponentResourceManager (typeof (MForm));
			Icon = ((Icon) (resources.GetObject ("$this.Icon")));
			sp = s;

			FontManager.SetAllControlsFont (panel1.Controls, 0);
			FontManager.SetControlFont (linkLabel4, 1);
			FontManager.SetControlFont (linkLabel5, 1);
			FontManager.SetControlFont (label2, 1);
			FontManager.SetControlFont (linkLabel1, 1);
			FontManager.SetControlFont (vers, 1);
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
			Color bg = Color.Empty;
			Color fg = Color.Empty;
			Color key = Color.Empty;
			Color click = Color.Empty;

			if (!isFromPascal)
			{
				bg = Helper.bgColor;
				fg = Helper.fgColor;
				key = Helper.fgKeyword;
				click = Helper.bgClick;
			} else
			{
				bg = sp.bg;
				fg = sp.fg;
				key = sp.key;
				click = sp.click;
			}
			
			button1.BackColor = panel1.BackColor = BackColor = bg;
			
			button1.ForeColor = linkLabel1.LinkColor = linkLabel2.LinkColor = linkLabel3.LinkColor =
				linkLabel4.LinkColor = linkLabel5.LinkColor = linkLabel6.LinkColor = linkLabel7.LinkColor = 
				linkLabel8.LinkColor = linkLabel10.LinkColor = linkLabel11.LinkColor = 
					linkLabel12.LinkColor = linkLabel13.LinkColor = linkLabel14.LinkColor = 
						linkLabel15.LinkColor = ForeColor = fg;
				
			linkLabel1.ActiveLinkColor = linkLabel2.ActiveLinkColor = linkLabel3.ActiveLinkColor =
				linkLabel4.ActiveLinkColor = linkLabel5.ActiveLinkColor = linkLabel6.ActiveLinkColor = 
				linkLabel7.ActiveLinkColor = linkLabel8.ActiveLinkColor = 
				linkLabel10.ActiveLinkColor = linkLabel11.ActiveLinkColor = linkLabel12.ActiveLinkColor = 
					linkLabel13.ActiveLinkColor = linkLabel14.ActiveLinkColor = linkLabel15.ActiveLinkColor = key;
				
			button1.FlatAppearance.MouseOverBackColor = click;
		}

		private void linkLabel7_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://github.com/RJCodeAdvance/Custom-ComboBox");
		}

		private void linkLabel8_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://github.com/svg-net/SVG");
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

		private void linkLabel14_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://github.com/Knagis/CommonMark.NET");
		}

		private void linkLabel15_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start ("https://jetbrains.design/intellij/resources/icons_list/");
		}
	}
}