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
		public const int PASCALPATH          = 1;
		public const int ACTIVE              = 2;
		public const int ASKCHOICE           = 4;
		public const int CHOICEINDEX         = 5;
		public const int SETTINGMODE         = 6;
		public const int AUTOUPDATE          = 7;
		public const int BGIMAGE             = 8;
		public const int STICKER             = 9;
		public const int STATUSBAR           = 10;
		public const int LOGO                = 11;
		public const int LOCATION            = 12;
		public const int EDITOR              = 13;
		public const int BETA                = 14;
		public const int LOGIN               = 15;
		public const int CAMOUFLAGEHIDDEN    = 16;
		public const int STICKERPOSITION     = 17;
		public const int CAMOUFLAGEPOSITIONS = 18;
		public const int STICKERPOSITIONUNIT = 19;
		public const int ALLOWPOSITIONING    = 20;
		public const int SHOWGRIDS           = 21;
		public const int USECUSTOMSTICKER    = 22;
		public const int CUSTOMSTICKER       = 23;
		public const int LICENSE             = 24;
		public const int GOOGLEANALYTICS     = 25;
		public const int DONTTRACK           = 26;


		public const  double current_version     = 5.0;
		public const  string current_version_add = "beta-2";
		public static string next_version        = "";

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
			settingsPanel.SettingsPanel_Load ();
			settingsPanel.mf = mf;
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
			BackColor = settingsPanel.button1.BackColor = button2.BackColor = button3.BackColor = settingsPanel.button4.BackColor =
				settingsPanel.button5.BackColor = settingsPanel.button6.BackColor = settingsPanel.ActionBox.ListBackColor =
					settingsPanel.ActionBox.BackColor = settingsPanel.mode.ListBackColor = settingsPanel.mode.BackColor =
						settingsPanel.textBox1.BackColor = settingsPanel.add_program.BackColor = settingsPanel.add_plugin.BackColor =
							settingsPanel.tabPage1.BackColor = settingsPanel.unit.ListBackColor = settingsPanel.unit.BackColor =
								settingsPanel.BackColor = settingsPanel.add_toolbar.BackColor = Helper.bgColor;

			ForeColor = settingsPanel.button1.FlatAppearance.BorderColor = button2.FlatAppearance.BorderColor =
				button3.FlatAppearance.BorderColor = settingsPanel.button4.FlatAppearance.BorderColor =
					settingsPanel.button5.FlatAppearance.BorderColor = settingsPanel.button6.FlatAppearance.BorderColor =
						settingsPanel.ActionBox.ForeColor = settingsPanel.ActionBox.ListTextColor = settingsPanel.mode.ForeColor =
							settingsPanel.mode.ListTextColor = settingsPanel.unit.ForeColor =
								settingsPanel.unit.ListTextColor = settingsPanel.textBox1.ForeColor = settingsPanel.tabs.ForeColor =
									settingsPanel.add_program.ForeColor = settingsPanel.add_plugin.ForeColor =
										settingsPanel.add_toolbar.ForeColor = settingsPanel.tabPage1.ForeColor =
											settingsPanel.roundLabel1.ForeColor = Helper.fgColor;

			settingsPanel.button1.FlatAppearance.MouseOverBackColor = button2.FlatAppearance.MouseOverBackColor =
				button3.FlatAppearance.MouseOverBackColor = settingsPanel.button4.FlatAppearance.MouseOverBackColor =
					settingsPanel.button5.FlatAppearance.MouseOverBackColor =
						settingsPanel.button6.FlatAppearance.MouseOverBackColor = settingsPanel.roundLabel1._BackColor = Helper.bgClick;

			settingsPanel.ActionBox.BorderColor = settingsPanel.ActionBox.IconColor = settingsPanel.mode.BorderColor =
				settingsPanel.mode.IconColor = settingsPanel.unit.BorderColor =
					settingsPanel.unit.IconColor = settingsPanel.textBox1.BorderColor = Helper.bgBorder;

			settingsPanel.tabs.bg = new SolidBrush (Helper.bgColor);
			settingsPanel.tabs.bgClick = new SolidBrush (Helper.bgClick);

			settingsPanel.stickerToUpdate.Add (form.stickerControl);
			bool isProgram = Helper.mode == ProductMode.Program;
			settingsPanel.HideTabPage (isProgram, false);
		}

		public void setVisible (bool vis)
		{
			settingsPanel.setVisible (vis);
		}
	}
}