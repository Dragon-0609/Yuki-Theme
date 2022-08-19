using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Windows.Forms;
using CustomControls.RJControls;
using Microsoft.WindowsAPICodePack.Dialogs;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Database;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Utils;

namespace Yuki_Theme.Core.Controls
{
	public partial class SettingsPanel : UserControl
	{
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
		public  bool                 lockLang     = false;
		public  List <ToolStripItem> items;
		public  List <string>        itemsToHide;
		public  List <string>        itemsToRight;
		public  List <CustomPicture> stickerToUpdate;
		public  string               customSticker;
		public  PopupFormsController popupController;

		private RJComboBox [] _comboBoxes;

		public Action <List <ToolStripItem>, List <string>, List <string>> onChange;

		public Action updateTranslation;

		public SettingsPanel ()
		{
			InitializeComponent ();
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
					if (Directory.Exists (Path.Combine (co.FileName, "Highlighting")))
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
			popupController.InitializeAllWindows ();
			popupController.df.CheckUpdate ();
		}

		private void button6_Click (object sender, EventArgs e)
		{
			OpenFileDialog of = new OpenFileDialog ();
			of.DefaultExt = "zip";
			of.Filter = "Yuki Theme(*.zip)|*.zip";
			of.Multiselect = false;
			if (of.ShowDialog () == DialogResult.OK)
			{
				bool has = DownloadForm.IsValidUpdate (of.FileName);

				if (has)
				{
					File.Copy (of.FileName, Path.Combine (
						           Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
						           "Yuki Theme",
						           "yuki_theme.zip"), true);
					popupController.InitializeAllWindows ();
					popupController.df.InstallManually ();
				} else
				{
					MessageBox.Show (API.API.Current.Translate ("messages.update.invalid"),
					                 API.API.Current.Translate ("messages.update.wrong"), MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		public void setVisible (bool vis)
		{
			textBox1.Enabled = button1.Enabled = askC.Enabled = ActionBox.Enabled = vis;
			logo.Enabled = swStatusbar.Enabled = !vis;
		}

		public void loadSVG ()
		{
			var a = Assembly.GetExecutingAssembly ();
			string add = "";
			Color fr = Color.White;
			if (isFromPascal)
			{
				add = Helper.IsDark (bg) ? "" : "_dark";
				fr = fg;
			} else
			{
				add = Helper.IsDark (Helper.bgColor) ? "" : "_dark";
				fr = Helper.fgColor;
			}

			Helper.RenderSvg (showHelp, Helper.LoadSvg ("help", a), false, Size.Empty, true, fr);
			Helper.RenderSvg (button1, Helper.LoadSvg ("moreHorizontal" + add, a), true, new Size (16, 16), true, fr);
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
			e.Graphics.DrawString (((ListBox)sender).Items [e.Index].ToString (), e.Font, fgBrush, e.Bounds);

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
						string nm = (string)toolBarList.SelectedItem;
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
					string nm = (string)toolBarList.SelectedItem;
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
			itemsToRight.Clear ();
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
					string nm = (string)toolBarList.SelectedItem;
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
			Settings.database.DeleteData (SettingsConst.STICKER_POSITION);
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

		public void SettingsPanel_Load ()
		{
			ActionBox.Items.AddRange (new ExtendedDropItem [] { new ("settings.additional.action.delete"), new ("settings.additional.action.import"), new ("settings.additional.action.ignore") });
			mode.Items.AddRange (new ExtendedDropItem [] { new ("settings.general.mode.light"), new ("settings.general.mode.advanced") });
			_comboBoxes = new [] { ActionBox, mode };
			stickerToUpdate = new List <CustomPicture> ();
			foreach (string name in Enum.GetNames (typeof (RelativeUnit)))
			{
				unit.Items.Add (name);
			}

			AddLanguages ();
			
			toolBarList.ItemHeight = Font.Height + 3;
			backImage.Checked = Settings.bgImage;
			swsticker.Checked = Settings.swSticker;
			logo.Checked = Settings.swLogo;
			editor.Checked = Settings.Editor;
			checkBox1.Checked = Settings.Beta;
			swStatusbar.Checked = Settings.swStatusbar;
			askC.Checked = Settings.askChoice;
			checkBox2.Checked = Settings.update;
			ActionBox.SelectedIndex = Settings.actionChoice;
			mode.SelectedIndex = (int)Settings.settingMode;
			unit.SelectedIndex = (int)Settings.unit;
			checkBox3.Checked = Settings.positioning;
			unit.Enabled = checkBox4.Enabled = reset_margin.Enabled = Settings.positioning && Settings.swSticker;
			checkBox4.Checked = Settings.showGrids;
			use_cstm_sticker.Checked = Settings.useCustomSticker;
			customSticker = Settings.customSticker;
			fitWidth.Checked = Settings.autoFitByWidth;
			fitWidth.Enabled = Settings.bgImage;
			askSave.Checked = Settings.askToSave;
			saveOld.Checked = Settings.saveAsOld;
			restartUpdate.Enabled = DownloadForm.IsUpdateDownloaded ();
			preview.Checked = Settings.showPreview;
			lockLang = true;
			UpdateLanguageSelection ();
			lockLang = false;
			loadSVG ();
			UpdateTranslations ();
		}

		public void UpdateLanguageSelection ()
		{
			lang.SelectedIndex = Settings.translation.GetIndexOfLangShort (Settings.localization);
		}

		private void backImage_CheckedChanged (object sender, EventArgs e)
		{
			fitWidth.Enabled = backImage.Checked;
		}

		private void showHelp_Click (object sender, EventArgs e)
		{
			Color back = Color.White;
			Color fore = Color.White;
			Color brdr = Color.White;
			if (isFromPascal)
			{
				back = bg;
				fore = fg;
				brdr = border;
			} else
			{
				back = Helper.bgColor;
				fore = Helper.fgColor;
				brdr = Helper.bgBorder;
			}

			HelperForm hf = new HelperForm ();
			hf.setMessage ("Old New Formats", SmallDocumentation.Documentation [SmallDocumentation.OLD_NEW_HELP]);
			hf.setColors (back, fore, brdr);
			hf.ShowDialog (ParentForm);
		}

		private void restartUpdate_Click (object sender, EventArgs e)
		{
			if (DownloadForm.IsUpdateDownloaded ())
			{
				popupController.InitializeAllWindows ();
				popupController.df.startUpdating ();
			} else
			{
				API_Events.showError ("Update isn't downloaded!", "Update isn't downloaded");
			}
		}

		private void AddLanguages ()
		{
			lang.Items.AddRange (Settings.translation.GetLanguages ());
		}

		public void UpdateTranslations ()
		{
			TranslateControls (tabs);
			if (updateTranslation != null)
				updateTranslation ();
			ReSize ();
			ReAddItems ();
		}

		private void ReSize ()
		{
			flowLayoutPanel2.Size = new Size (flowLayoutPanel2.Width, 50);
			flowLayoutPanel1.Size = new Size (flowLayoutPanel1.Width, 50);
		}

		private void ReAddItems ()
		{
			foreach (RJComboBox comboBox in _comboBoxes)
			{
				for (var i = 0; i < comboBox.Items.Count; i++)
				{
					if (comboBox.Items [i] is ExtendedDropItem)
					{
						ExtendedDropItem item = (ExtendedDropItem) comboBox.Items [i];
						comboBox.Items [i] = item;
					}
				}
			}
		}

		private void TranslateControls (Control parent)
		{
			foreach (Control control in parent.Controls)
			{
				if (control.AccessibleName != null)
					control.Text = API.API.Current.Translate (control.AccessibleName);
				if (control.Controls.Count > 0)
				{
					TranslateControls (control);
				}
			}
		}

		private void lang_OnSelectedIndexChanged (object sender, EventArgs e)
		{
			if (!lockLang)
			{
				if (Settings.localization != Settings.translation.GetLanguageISO2 ((string)lang.SelectedItem))
				{
					string language = Settings.translation.GetLanguageISO2 ((string)lang.SelectedItem);
					Settings.localization = language;
					Settings.translation.LoadLocale (language);
					UpdateTranslations ();
				}
			}
		}
	}
}