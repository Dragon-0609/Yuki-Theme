using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using Yuki_Theme.Core.Utils;

namespace Yuki_Theme.Core.Forms
{
	public partial class SettingsForm : Form
	{
		// private MForm form;

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

		public SettingsForm (/*MForm mf*/)
		{
			InitializeComponent ();
			// form = mf;
			StartPosition = FormStartPosition.CenterParent;
			Icon = Helper.GetYukiThemeIcon (new Size (32, 32));
			settingsPanel.SettingsPanel_Load ();
			// settingsPanel.popupController = mf.popupController;
			settingsPanel.updateTranslation = UpdateTranslation;
			UpdateTranslation ();
			FontManager.SetAllControlsFont (Controls, 0);
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
					settingsPanel.lang.ListBackColor = settingsPanel.ActionBox.BackColor = settingsPanel.lang.BackColor =
						settingsPanel.mode.ListBackColor = settingsPanel.mode.BackColor = settingsPanel.textBox1.BackColor =
							settingsPanel.add_program.BackColor = settingsPanel.add_plugin.BackColor = settingsPanel.tabPage1.BackColor =
								settingsPanel.unit.ListBackColor = settingsPanel.unit.BackColor =
									settingsPanel.BackColor = settingsPanel.add_toolbar.BackColor = Helper.bgColor;

			ForeColor = settingsPanel.button1.FlatAppearance.BorderColor = button2.FlatAppearance.BorderColor =
				button3.FlatAppearance.BorderColor = settingsPanel.button4.FlatAppearance.BorderColor =
					settingsPanel.button5.FlatAppearance.BorderColor = settingsPanel.button6.FlatAppearance.BorderColor =
						settingsPanel.ActionBox.ForeColor = settingsPanel.ActionBox.ListTextColor = settingsPanel.lang.ForeColor =
							settingsPanel.lang.ListTextColor = settingsPanel.mode.ForeColor = settingsPanel.mode.ListTextColor =
								settingsPanel.unit.ForeColor = settingsPanel.unit.ListTextColor = settingsPanel.textBox1.ForeColor =
									settingsPanel.tabs.ForeColor = settingsPanel.add_program.ForeColor =
										settingsPanel.add_plugin.ForeColor = settingsPanel.add_toolbar.ForeColor =
											settingsPanel.tabPage1.ForeColor = settingsPanel.roundLabel1.ForeColor = Helper.fgColor;

			settingsPanel.button1.FlatAppearance.MouseOverBackColor = button2.FlatAppearance.MouseOverBackColor =
				button3.FlatAppearance.MouseOverBackColor = settingsPanel.button4.FlatAppearance.MouseOverBackColor =
					settingsPanel.button5.FlatAppearance.MouseOverBackColor = settingsPanel.button6.FlatAppearance.MouseOverBackColor =
						settingsPanel.roundLabel1._BackColor = Helper.bgClick;

			settingsPanel.ActionBox.BorderColor = settingsPanel.ActionBox.IconColor = settingsPanel.lang.BorderColor =
				settingsPanel.lang.IconColor = settingsPanel.mode.BorderColor = settingsPanel.mode.IconColor =
					settingsPanel.unit.BorderColor = settingsPanel.unit.IconColor = settingsPanel.textBox1.BorderColor = Helper.bgBorder;

			settingsPanel.tabs.bg = new SolidBrush (Helper.bgColor);
			settingsPanel.tabs.bgClick = new SolidBrush (Helper.bgClick);

			// settingsPanel.stickerToUpdate.Add (form.stickerControl);
			bool isProgram = Helper.mode == ProductMode.Program;
			settingsPanel.HideTabPage (isProgram, false);
		}

		public void setVisible (bool vis)
		{
			settingsPanel.setVisible (vis);
		}

		public void UpdateTranslation ()
		{
			button2.Text = API.API.Current.Translate ("main.tips.save");
			button3.Text = API.API.Current.Translate ("download.cancel");
			Text = API.API.Current.Translate ("main.tips.settings");
		}
	}
}