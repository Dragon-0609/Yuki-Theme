using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using Yuki_Theme.Core.Forms;

namespace Yuki_Theme.Core.Controls
{
	public partial class SettingsPanel : UserControl
	{
		public MForm mf;
		public Color bg;
		public Color fg;
		public Color key;
		public Color click;
		public bool  isFromPascal = false;

		public SettingsPanel ()
		{
			InitializeComponent ();
			ActionBox.Items.AddRange (new string [] {"Delete", "Import and Delete", "Ignore"});
			mode.Items.AddRange (new string [] {"Light", "Advanced"});
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

		private void checkBox2_CheckedChanged (object sender, EventArgs e)
		{
			ActionBox.Enabled = !askC.Checked;
		}

		private void button4_Click (object sender, EventArgs e)
		{
			AboutForm about = new AboutForm (this);
			about.isFromPascal = isFromPascal;
			about.ShowDialog ();
		}

		private void button5_Click (object sender, EventArgs e)
		{
			if (mf == null) mf = new MForm ((int) ProductMode.Plugin, true);
			if (!mf.Visible) mf.Show ();
			mf.update_Click (sender, e);
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
							if (mf == null) mf = new MForm ((int) ProductMode.Plugin, true);
							if (!mf.Visible) mf.Show ();
							if (mf.df == null)
								mf.df = new DownloadForm (mf);
							mf.df.InstallManually ();
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

		public void setVisible (bool vis)
		{
			textBox1.Enabled = button1.Enabled = askC.Enabled = ActionBox.Enabled = vis;
			logo.Enabled = swStatusbar.Enabled = !vis;
		}

		public void loadSVG ()
		{
			var a = Assembly.GetExecutingAssembly ();
			Helper.renderSVG (button1, Helper.loadsvg ("three-dots", a), true, new Size (16,16));
		}
	}
}