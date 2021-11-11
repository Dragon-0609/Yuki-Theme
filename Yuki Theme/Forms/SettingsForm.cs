using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace Yuki_Theme.Forms
{
	public partial class SettingsForm : Form
	{
		public const int PASCALPATH = 1;
		public const int ACTIVE = 2;
		public const int SETACTIVE = 3;
		public const int ASKCHOICE = 4;
		public const int CHOICEINDEX = 5;
		public const int SETTINGMODE = 6;
		public const int AUTOUPDATE = 7;
		
		public const string current_version = "v1.0";
		public static string next_version = "";

		private MForm form;

		public string Path
		{
			get => textBox1.Text;
			set => textBox1.Text = value;
		}

		public bool Active
		{
			get => checkBox1.Checked;
			set => checkBox1.Checked = value;
		}
		
		public SettingsForm ( MForm mf)
		{
			InitializeComponent ();
			form = mf;
			this.StartPosition = FormStartPosition.CenterParent;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (MForm));
			Icon = ((Icon) (resources.GetObject ("$this.Icon")));
		}

		private void button1_Click (object sender, EventArgs e)
		{
			FolderBrowserDialog fb = new FolderBrowserDialog ();
			fb.Description = "Select Path to the PascalABC.NET directory";
			fb.ShowNewFolderButton = false;
			selectFolder (fb);
		}

		private void selectFolder (FolderBrowserDialog fb)
		{
			if (fb.ShowDialog () == DialogResult.OK)
			{
				if (Directory.Exists (fb.SelectedPath))
				{
					if (Directory.Exists (System.IO.Path.Combine (fb.SelectedPath, "Highlighting")))
					{
						textBox1.Text = fb.SelectedPath;
					} else
					{
						fb.Description = "It isn't PascalABC.NET directory. Select Path to the PascalABC.NET directory";
						selectFolder (fb);
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
				bool has = ZipHasFile("Yuki Theme.exe", of.FileName);
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
	}
}