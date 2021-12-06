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
		
		public const double current_version = 3.0;
		public const string current_version_add = "beta 2";
		public static string next_version = "";

		private MForm form;

		public string Path
		{
			get => textBox1.Text;
			set => textBox1.Text = value;
		}

		public bool bgImage
		{
			get => backImage.Checked;
			set => backImage.Checked = value;
		}

		public bool Sticker
		{
			get => swsticker.Checked;
			set => swsticker.Checked = value;
		}

		public bool StatusBar
		{
			get => swStatusbar.Checked;
			set => swStatusbar.Checked = value;
		}
		
		public bool Logo
		{
			get => logo.Checked;
			set => logo.Checked = value;
		}
		
		public SettingsForm ( MForm mf)
		{
			InitializeComponent ();
			form = mf;
			this.StartPosition = FormStartPosition.CenterParent;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (MForm));
			Icon = ((Icon) (resources.GetObject ("$this.Icon")));
			ActionBox.Items.AddRange (new string[]{"Delete", "Import and Delete", "Ignore"});
			mode.Items.AddRange (new string[]{"Light", "Advanced"});
			loadSVG ();
		}

		private void button1_Click (object sender, EventArgs e)
		{
			CommonOpenFileDialog co = new CommonOpenFileDialog ();
			co.IsFolderPicker = true;
			co.Multiselect = false;
			co.Title = "Select Path to the PascalABC.NET directory";
			selectFolder (co);
		}

		private void selectFolder (CommonOpenFileDialog co)
		{
			CommonFileDialogResult res = co.ShowDialog ();
			if (res == CommonFileDialogResult.Ok)
			{
				if (Directory.Exists (co.FileName))
				{
					if (Directory.Exists (System.IO.Path.Combine (co.FileName, "Highlighting")))
					{
						textBox1.Text = co.FileName;
					} else
					{
						co.Title = "It isn't PascalABC.NET directory. Select Path to the PascalABC.NET directory";
						selectFolder (co);
					}
				} else
				{
					throw new FileLoadException ("Directory isn't exist");
				}
			}
		}

		private void button2_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void button3_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void checkBox2_CheckedChanged (object sender, EventArgs e)
		{
			ActionBox.Enabled = !askC.Checked;
		}

		private void button4_Click (object sender, EventArgs e)
		{
			AboutForm about = new AboutForm ();
			about.ShowDialog ();
		}

		private void button5_Click (object sender, EventArgs e)
		{
			form.button7_Click (sender, e);
		}

		public void updateStats ()
		{
			label5.Text = next_version;
		}

		private void button6_Click (object sender, EventArgs e)
		{
			OpenFileDialog of = new OpenFileDialog ();
			of.DefaultExt = "zip";
			of.Filter = "Yuki Theme(*.zip)|*.zip";
			of.Multiselect = false;
			if (of.ShowDialog () == DialogResult.OK)
			{
				bool has = ZipHasFile("Yuki Theme.Core.dll", of.FileName);
				if(has)
				{
					has = ZipHasFile ("Newtonsoft.Json.dll", of.FileName);
					if(has)
					{
						has = ZipHasFile ("FastColoredTextBox.dll", of.FileName);
						if (has)
						{
							File.Copy (of.FileName, System.IO.Path.Combine (
								           Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
								           "Yuki Theme",
								           "yuki_theme.zip"),true);
							if (form.df == null)
								form.df = new DownloadForm (form);
							form.df.InstallManually ();
						}
					}
				}

				if (!has)
				{
					MessageBox.Show ("The zip isn't Yuki Theme. Please, go to github and download from there",
					                 "The wrong zip", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			
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
			BackColor = button1.BackColor = button2.BackColor = button3.BackColor = button4.BackColor =
				button5.BackColor = button6.BackColor = 
				ActionBox.ListBackColor = ActionBox.BackColor = mode.ListBackColor = mode.BackColor = textBox1.BackColor = Helper.bgColor;
				
			ForeColor = button1.FlatAppearance.BorderColor = button2.FlatAppearance.BorderColor =
				button3.FlatAppearance.BorderColor = button4.FlatAppearance.BorderColor =
					button5.FlatAppearance.BorderColor = button6.FlatAppearance.BorderColor =
						ActionBox.ForeColor = ActionBox.ListTextColor = 
							mode.ForeColor = mode.ListTextColor = textBox1.ForeColor = Helper.fgColor;
						
			button1.FlatAppearance.MouseOverBackColor = button2.FlatAppearance.MouseOverBackColor =
				button3.FlatAppearance.MouseOverBackColor = button4.FlatAppearance.MouseOverBackColor =
					button5.FlatAppearance.MouseOverBackColor =
						button6.FlatAppearance.MouseOverBackColor = Helper.bgClick;

			ActionBox.BorderColor = ActionBox.IconColor = 
			mode.BorderColor = mode.IconColor = textBox1.BorderColor = Helper.bgBorder;
		}

		public void setVisible (bool vis)
		{
			textBox1.Enabled = button1.Enabled = askC.Enabled = ActionBox.Enabled = vis;
		}

		private void loadSVG ()
		{
			var a = Assembly.GetExecutingAssembly ();
			Helper.renderSVG (button1, Helper.loadsvg ("three-dots", a), true, new Size (16,16));
		}
		
	}
}