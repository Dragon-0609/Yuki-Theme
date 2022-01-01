using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Yuki_Theme.Core.Forms
{
	public partial class SettingsForm : Form
	{
		public const int PASCALPATH = 1;
		public const int ACTIVE = 2;
		public const int ASKCHOICE = 4;
		public const int CHOICEINDEX = 5;
		public const int SETTINGMODE = 6;
		public const int AUTOUPDATE = 7;
		public const int BGIMAGE = 8;
		public const int STICKER = 9;
		public const int STATUSBAR = 10;
		public const int LOGO = 11;
		public const int LOCATION = 12;
		public const int EDITOR = 13;
		public const int BETA = 14;
		public const int LOGIN = 15;
		
		public const double current_version = 5.0;
		public const string current_version_add = "beta";
		public static string next_version = "";

		private MForm form;

		public string Path
		{
			get => settingsPanel.textBox1.Text;
			set => settingsPanel.textBox1.Text = value;
		}

		public bool bgImage
		{
			get => settingsPanel.backImage.Checked;
			set => settingsPanel.backImage.Checked = value;
		}

		public bool Sticker
		{
			get => settingsPanel.swsticker.Checked;
			set => settingsPanel.swsticker.Checked = value;
		}

		public bool StatusBar
		{
			get => settingsPanel.swStatusbar.Checked;
			set => settingsPanel.swStatusbar.Checked = value;
		}
		
		public bool Logo
		{
			get => settingsPanel.logo.Checked;
			set => settingsPanel.logo.Checked = value;
		}
		
		public bool Editor
		{
			get => settingsPanel.editor.Checked;
			set => settingsPanel.editor.Checked = value;
		}
		
		public bool Beta
		{
			get => settingsPanel.checkBox1.Checked;
			set => settingsPanel.checkBox1.Checked = value;
		}
		
		public SettingsForm (MForm mf)
		{
			InitializeComponent ();
			form = mf;
			this.StartPosition = FormStartPosition.CenterParent;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (MForm));
			Icon = ((Icon) (resources.GetObject ("$this.Icon")));
		}

		private void button2_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void button3_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
		
		private bool ZipHasFile (string fileFullName, string zipFullPath)
		{
			using (ZipArchive archive = ZipFile.OpenRead (zipFullPath))
			{
				foreach (ZipArchiveEntry entry in archive.Entries)
				{
					if (entry.FullName.EndsWith (fileFullName, StringComparison.Ordinal))
					{
						return true;
					}
				}
				
			}

			return false;
		}

		private void SettingsForm_Shown (object sender, EventArgs e)
		{
			BackColor = settingsPanel.button1.BackColor = button2.BackColor = button3.BackColor =
				settingsPanel.button4.BackColor = settingsPanel.button5.BackColor = settingsPanel.button6.BackColor =
					settingsPanel.ActionBox.ListBackColor = settingsPanel.ActionBox.BackColor =
						settingsPanel.mode.ListBackColor = settingsPanel.mode.BackColor =
							settingsPanel.textBox1.BackColor = settingsPanel.add_program.BackColor =
								settingsPanel.add_plugin.BackColor = settingsPanel.tabPage1.BackColor =
									settingsPanel.BackColor = Helper.bgColor;

			ForeColor = settingsPanel.button1.FlatAppearance.BorderColor = button2.FlatAppearance.BorderColor =
				button3.FlatAppearance.BorderColor = settingsPanel.button4.FlatAppearance.BorderColor =
					settingsPanel.button5.FlatAppearance.BorderColor =
						settingsPanel.button6.FlatAppearance.BorderColor =
							settingsPanel.ActionBox.ForeColor = settingsPanel.ActionBox.ListTextColor =
								settingsPanel.mode.ForeColor = settingsPanel.mode.ListTextColor =
									settingsPanel.textBox1.ForeColor = settingsPanel.tabs.ForeColor =
										settingsPanel.add_program.ForeColor =
											settingsPanel.add_plugin.ForeColor =
												settingsPanel.tabPage1.ForeColor = Helper.fgColor;

			settingsPanel.button1.FlatAppearance.MouseOverBackColor = button2.FlatAppearance.MouseOverBackColor =
				button3.FlatAppearance.MouseOverBackColor = settingsPanel.button4.FlatAppearance.MouseOverBackColor =
					settingsPanel.button5.FlatAppearance.MouseOverBackColor =
						settingsPanel.button6.FlatAppearance.MouseOverBackColor = Helper.bgClick;

			settingsPanel.ActionBox.BorderColor = settingsPanel.ActionBox.IconColor =
				settingsPanel.mode.BorderColor =
					settingsPanel.mode.IconColor = settingsPanel.textBox1.BorderColor = Helper.bgBorder;

			settingsPanel.tabs.bg = new SolidBrush (Helper.bgColor);
			settingsPanel.tabs.bgClick = new SolidBrush (Helper.bgClick);
			bool isProgram = Helper.mode == ProductMode.Program;
			if (isProgram)
			{
				if (settingsPanel.tabs.TabPages.Contains (settingsPanel.add_plugin))
					settingsPanel.tabs.TabPages.Remove (settingsPanel.add_plugin);
				if (!settingsPanel.tabs.TabPages.Contains (settingsPanel.add_program))
					settingsPanel.tabs.TabPages.Add (settingsPanel.add_program);
			} else
			{
				if (settingsPanel.tabs.TabPages.Contains (settingsPanel.add_program))
					settingsPanel.tabs.TabPages.Remove (settingsPanel.add_program);
				if (!settingsPanel.tabs.TabPages.Contains (settingsPanel.add_plugin))
					settingsPanel.tabs.TabPages.Add (settingsPanel.add_plugin);
			}
		}

		public void setVisible (bool vis)
		{
			settingsPanel.setVisible (vis);
		}
		
	}
}