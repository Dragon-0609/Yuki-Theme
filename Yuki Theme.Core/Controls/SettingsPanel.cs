using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using Yuki_Theme.Core.Database;
using Yuki_Theme.Core.Forms;

namespace Yuki_Theme.Core.Controls
{
	public partial class SettingsPanel : UserControl
	{
		public  MForm                mf;
		public  Color                bg;
		public  Color                bg2;
		public  Color                fg;
		public  Brush                fgBrush;
		public  Color                key;
		public  Color                click;
		public  Color                border;
		public  bool                 isFromPascal = false;
		private int                  selectionindex;
		public  bool                 lockCheckbox = false;
		public  bool                 lockList     = false;
		public  List <ToolStripItem> items;
		public  List <string>        itemsToHide;
		public  List <string>        itemsToRight;
		public  List <CustomPicture> stickerToUpdate;
		public  string               customSticker;

		public Action <List <ToolStripItem>, List <string>, List <string>> onChange;

		public SettingsPanel ()
		{
			InitializeComponent ();
			ActionBox.Items.AddRange (new string [] {"Delete", "Import and Delete", "Ignore"});
			mode.Items.AddRange (new string [] {"Light", "Advanced"});
			stickerToUpdate = new List <CustomPicture> ();
			foreach (string name in Enum.GetNames (typeof (RelativeUnit)))
			{
				unit.Items.Add (name);
			}

			toolBarList.ItemHeight = Font.Height + 3;
			backImage.Checked = CLI.bgImage;
			swsticker.Checked = CLI.swSticker;
			logo.Checked = CLI.swLogo;
			editor.Checked = CLI.Editor;
			checkBox1.Checked = CLI.Beta;
			swStatusbar.Checked = CLI.swStatusbar;
			askC.Checked = CLI.askChoice;
			checkBox2.Checked = CLI.update;
			ActionBox.SelectedIndex = CLI.actionChoice;
			mode.SelectedIndex = CLI.settingMode;
			unit.SelectedIndex = (int) CLI.unit;
			checkBox3.Checked = CLI.positioning;
			unit.Enabled = checkBox4.Enabled = reset_margin.Enabled = CLI.positioning && CLI.swSticker;
			checkBox4.Checked = CLI.showGrids;
			use_cstm_sticker.Checked = CLI.useCustomSticker;
			customSticker = CLI.customSticker;
			loadSVG ();
		}

		public void HideTabPage (bool isProgram, bool needToolBarPage)
		{
			if (isProgram)
			{
				if (tabs.TabPages.Contains (add_plugin))
					tabs.TabPages.Remove (add_plugin);
				if (!tabs.TabPages.Contains (add_program))
					tabs.TabPages.Add (add_program);
			} else
			{
				if (tabs.TabPages.Contains (add_program))
					tabs.TabPages.Remove (add_program);
				if (!tabs.TabPages.Contains (add_plugin))
					tabs.TabPages.Add (add_plugin);
			}

			if (needToolBarPage)
			{
				if (!tabs.TabPages.Contains (add_toolbar))
					tabs.TabPages.Add (add_toolbar);
			} else
			{
				if (tabs.TabPages.Contains (add_toolbar))
					tabs.TabPages.Remove (add_toolbar);
			}
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
				bool has = ZipHasFile ("Yuki Theme.Core.dll", of.FileName);
				if (has)
				{
					has = ZipHasFile ("Newtonsoft.Json.dll", of.FileName);
					if (has)
					{
						has = ZipHasFile ("FastColoredTextBox.dll", of.FileName);
						if (has)
						{
							File.Copy (of.FileName, System.IO.Path.Combine (
								           Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
								           "Yuki Theme",
								           "yuki_theme.zip"), true);
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
			Helper.renderSVG (button1, Helper.loadsvg ("three-dots", a), true, new Size (16, 16));
		}

		private void list_1_DrawItem (object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0) return;
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				e = new DrawItemEventArgs (e.Graphics, e.Font, e.Bounds,
				                           e.Index, e.State ^ DrawItemState.Selected,
				                           e.ForeColor, bg2);
			} else if (e.Index == selectionindex)
			{
				e = new DrawItemEventArgs (e.Graphics, e.Font, e.Bounds,
				                           e.Index, e.State,
				                           e.ForeColor, click);
			}

			e.DrawBackground ();
			e.Graphics.DrawString (((ListBox) sender).Items [e.Index].ToString (), e.Font, fgBrush, e.Bounds);

			e.DrawFocusRectangle ();
		}

		private void Lst_MouseHover (object sender, MouseEventArgs e)
		{
			if (!lockList)
			{
				int index = toolBarList.IndexFromPoint (e.Location);
				if (index < 0) return;
				//Do any action with the item
				if (selectionindex != index)
				{
					int oldindex = selectionindex;
					selectionindex = index;
					toolBarList.Invalidate (toolBarList.GetItemRectangle (oldindex));
					toolBarList.Invalidate (toolBarList.GetItemRectangle (index));
				}
			}
		}

		private void toolBarList_SelectedIndexChanged (object sender, EventArgs e)
		{
			if (!lockList)
			{
				if (toolBarList.SelectedIndex >= 0)
				{
					if (items != null && itemsToHide != null)
					{
						string nm = (string) toolBarList.SelectedItem;
						ToolStripItem item = getByText (nm);

						if (item != null)
						{
							toolBarImage.Image = item.Image;
							lockCheckbox = true;
							toolBarVisible.Checked = !itemsToHide.Contains (item.Name);
							toolBarPosition.Checked = itemsToRight.Contains (item.Name);
							lockCheckbox = false;
						}
					}
				}
			}
		}

		private ToolStripItem getByText (string nm)
		{
			ToolStripItem item = null;
			foreach (ToolStripItem toolStripItem in items)
			{
				if (toolStripItem.ToolTipText == nm)
				{
					item = toolStripItem;
				}
			}

			return item;
		}

		private void toolBarVisible_CheckedChanged (object sender, EventArgs e)
		{
			if (!lockCheckbox)
			{
				if (toolBarList.SelectedIndex >= 0)
				{
					string nm = (string) toolBarList.SelectedItem;
					ToolStripItem item = getByText (nm);

					if (toolBarVisible.Checked)
					{
						if (itemsToHide.Contains (item.Name))
						{
							itemsToHide.Remove (item.Name);
						}
					} else
					{
						if (!itemsToHide.Contains (item.Name))
						{
							itemsToHide.Add (item.Name);
						}
					}

					if (onChange != null)
					{
						onChange (items, itemsToHide, itemsToRight);
					}
				}
			}
		}

		public void PopulateList (List <ToolStripItem> pitems, List <string> pitemsToHide, List <string> pitemsToRight)
		{
			items = pitems;
			itemsToHide = pitemsToHide;
			itemsToRight = pitemsToRight;

			toolBarList.Items.Clear ();

			for (var i = 0; i < items.Count; i++)
			{
				if (items [i].ToolTipText != null)
					toolBarList.Items.Add (items [i].ToolTipText);
			}
		}

		private void button2_Click (object sender, EventArgs e)
		{
			itemsToHide.Clear ();
			if (onChange != null)
			{
				onChange (items, itemsToHide, itemsToRight);
			}
		}

		private void toolBarPosition_CheckedChanged (object sender, EventArgs e)
		{
			if (!lockCheckbox)
			{
				if (toolBarList.SelectedIndex >= 0)
				{
					string nm = (string) toolBarList.SelectedItem;
					ToolStripItem item = getByText (nm);

					if (toolBarPosition.Checked)
					{
						if (!itemsToRight.Contains (item.Name))
						{
							itemsToRight.Add (item.Name);
						}
					} else
					{
						if (itemsToRight.Contains (item.Name))
						{
							itemsToRight.Remove (item.Name);
						}
					}

					if (onChange != null)
					{
						onChange (items, itemsToHide, itemsToRight);
					}
				}
			}
		}

		private void checkBox3_CheckedChanged (object sender, EventArgs e)
		{
			unit.Enabled = checkBox3.Checked;
			checkBox4.Enabled = checkBox3.Checked;
			reset_margin.Enabled = checkBox3.Checked;
		}

		private void reset_margin_Click (object sender, EventArgs e)
		{
			DatabaseManager.DeleteData (SettingsForm.STICKERPOSITION);
			for (int i = 0; i < stickerToUpdate.Count; i++)
			{
				try
				{
					stickerToUpdate [i].ReadData ();
					stickerToUpdate [i].UpdateLocation ();
				} catch
				{
				}
			}

		}

		private void swsticker_CheckedChanged (object sender, EventArgs e)
		{
			unit.Enabled = checkBox4.Enabled = reset_margin.Enabled = reset_margin.Enabled = checkBox3.Enabled = swsticker.Checked;
		}

		private void cstm_sticker_Click (object sender, EventArgs e)
		{
			PathForm pf = new PathForm (this);
			pf.path.Text = customSticker;
			pf.isFromPascal = isFromPascal;
			if (pf.ShowDialog () == DialogResult.OK)
			{
				customSticker = pf.path.Text;
			}
		}
	}
}