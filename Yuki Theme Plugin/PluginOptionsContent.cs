// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Windows.Forms;
using VisualPascalABC;
using VisualPascalABC.OptionsContent;
using Yuki_Theme.Core;
using Yuki_Theme.Core.Forms;

namespace Yuki_Theme_Plugin
{
	public partial class PluginOptionsContent : UserControl, IOptionsContent
	{
		private YukiTheme_VisualPascalABCPlugin plugin;

		public PluginOptionsContent (YukiTheme_VisualPascalABCPlugin plug)
		{
			InitializeComponent ();
			plugin = plug;
			settingsPanel.isFromPascal = true; // To recognize
			settingsPanel.SettingsPanel_Load ();
		}

		private bool alreadyShown;

		#region IOptionsContent Members

		public string ContentName
		{
			get { return "Yuki Theme"; }
		}

		public string Description
		{
			get { return "Yuki Theme Settings"; }
		}

		public UserControl Content
		{
			get { return this; }
		}

		private bool oldeditor = false;

		public void Action (OptionsContentAction action)
		{
			switch (action)
			{
				case OptionsContentAction.Show :
					if (!alreadyShown)
					{
						settingsPanel.button1.BackColor = settingsPanel.button4.BackColor = settingsPanel.button5.BackColor =
							settingsPanel.button6.BackColor = settingsPanel.ActionBox.ListBackColor = settingsPanel.ActionBox.BackColor =
								settingsPanel.mode.ListBackColor = settingsPanel.mode.BackColor =
									settingsPanel.unit.ListBackColor = settingsPanel.unit.BackColor = settingsPanel.textBox1.BackColor =
										settingsPanel.add_program.BackColor = settingsPanel.add_plugin.BackColor =
											settingsPanel.add_toolbar.BackColor = settingsPanel.tabPage1.BackColor =
												settingsPanel.BackColor =
													settingsPanel.toolBarList.BackColor =
														settingsPanel.button2.BackColor = YukiTheme_VisualPascalABCPlugin.bg;


						settingsPanel.button1.FlatAppearance.BorderColor = settingsPanel.button4.FlatAppearance.BorderColor =
							settingsPanel.button5.FlatAppearance.BorderColor = settingsPanel.button6.FlatAppearance.BorderColor =
								settingsPanel.ActionBox.ForeColor = settingsPanel.ActionBox.ListTextColor = settingsPanel.mode.ForeColor =
									settingsPanel.mode.ListTextColor = settingsPanel.unit.ForeColor = settingsPanel.unit.ListTextColor =
										settingsPanel.textBox1.ForeColor = settingsPanel.tabs.ForeColor =
											settingsPanel.add_program.ForeColor = settingsPanel.add_plugin.ForeColor =
												settingsPanel.add_toolbar.ForeColor = settingsPanel.tabPage1.ForeColor =
													settingsPanel.toolBarList.ForeColor = settingsPanel.button2.ForeColor =
														settingsPanel.groupBox1.ForeColor = YukiTheme_VisualPascalABCPlugin.clr;

						settingsPanel.button1.FlatAppearance.MouseOverBackColor = settingsPanel.button4.FlatAppearance.MouseOverBackColor =
							settingsPanel.button5.FlatAppearance.MouseOverBackColor =
								settingsPanel.button6.FlatAppearance.MouseOverBackColor =
									settingsPanel.button2.FlatAppearance.MouseOverBackColor = YukiTheme_VisualPascalABCPlugin.bgClick;

						settingsPanel.ActionBox.BorderColor = settingsPanel.ActionBox.IconColor = settingsPanel.mode.BorderColor =
							settingsPanel.mode.IconColor = settingsPanel.unit.BorderColor = settingsPanel.unit.IconColor =
								settingsPanel.textBox1.BorderColor =
									settingsPanel.button2.FlatAppearance.BorderColor = YukiTheme_VisualPascalABCPlugin.bgBorder;

						settingsPanel.tabs.bg = YukiTheme_VisualPascalABCPlugin.bgBrush;
						settingsPanel.tabs.bgClick = YukiTheme_VisualPascalABCPlugin.bgClickBrush;
						settingsPanel.HideTabPage (false, true);

						if (plugin.mf != null && !plugin.mf.IsDisposed)
						{
							plugin.mf.Dispose ();
						}

						plugin.mf = null;
						settingsPanel.popupController = plugin.popupController;
						oldeditor = Settings.Editor;
						// Change colors for About Form
						settingsPanel.bg = YukiTheme_VisualPascalABCPlugin.bg;
						settingsPanel.bg2 = YukiTheme_VisualPascalABCPlugin.bgClick2;
						settingsPanel.fg = YukiTheme_VisualPascalABCPlugin.clr;
						settingsPanel.fgBrush = YukiTheme_VisualPascalABCPlugin.clrBrush;
						settingsPanel.key = YukiTheme_VisualPascalABCPlugin.clr;
						Helper.bgClick = settingsPanel.click = YukiTheme_VisualPascalABCPlugin.bgClick;
						settingsPanel.border = YukiTheme_VisualPascalABCPlugin.bgBorder;

						settingsPanel.toolBarImage.Image = null;
						settingsPanel.lockCheckbox = true;
						settingsPanel.toolBarVisible.Checked = false;
						settingsPanel.toolBarPosition.Checked = false;
						settingsPanel.lockCheckbox = false;
						settingsPanel.stickerToUpdate.Add (plugin.stickerControl);
						settingsPanel.PopulateList (YukiTheme_VisualPascalABCPlugin.camouflage.items,
						                            YukiTheme_VisualPascalABCPlugin.camouflage.itemsToHide,
						                            YukiTheme_VisualPascalABCPlugin.camouflage.itemsToRight);

						settingsPanel.onChange = YukiTheme_VisualPascalABCPlugin.camouflage.Update;

						alreadyShown = true;
					} else
					{
						settingsPanel.toolBarImage.Image = null;
						settingsPanel.lockCheckbox = true;
						settingsPanel.toolBarVisible.Checked = false;
						settingsPanel.toolBarPosition.Checked = false;
						settingsPanel.lockCheckbox = false;
						settingsPanel.toolBarList.SelectedIndex = -1;
					}

					break;
				case OptionsContentAction.Ok :
					Settings.bgImage = settingsPanel.backImage.Checked;
					Settings.swSticker = settingsPanel.swsticker.Checked;
					Settings.swLogo = settingsPanel.logo.Checked;
					Settings.Editor = settingsPanel.editor.Checked;
					Settings.Beta = settingsPanel.checkBox1.Checked;
					Settings.swStatusbar = settingsPanel.swStatusbar.Checked;
					Settings.askChoice = settingsPanel.askC.Checked;
					Settings.update = settingsPanel.checkBox2.Checked;
					Settings.actionChoice = settingsPanel.ActionBox.SelectedIndex;
					Settings.settingMode = (SettingMode) settingsPanel.mode.SelectedIndex;
					Settings.positioning = settingsPanel.checkBox3.Checked;
					Settings.unit = (RelativeUnit) settingsPanel.unit.SelectedIndex;
					Settings.showGrids = settingsPanel.checkBox4.Checked;
					Settings.useCustomSticker = settingsPanel.use_cstm_sticker.Checked;
					Settings.customSticker = settingsPanel.customSticker;
					plugin.stickerControl.Enabled = Settings.positioning;
					Settings.autoFitByWidth = settingsPanel.fitWidth.Checked;
					Settings.askToSave = settingsPanel.askSave.Checked;
					Settings.saveAsOld = settingsPanel.saveOld.Checked;
					Settings.showPreview = settingsPanel.preview.Checked;
					Settings.saveData ();
					plugin.LoadSticker ();
					
					settingsPanel.popupController = null;
					plugin.mf = null;
					alreadyShown = false;
					//this.Enabled = true;           
					break;
				case OptionsContentAction.Cancel :
					settingsPanel.popupController = null;
					plugin.mf = null;
					alreadyShown = false;
					break;
			}
		}

		#endregion
	}
}