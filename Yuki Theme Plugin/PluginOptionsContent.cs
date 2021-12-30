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
    public partial class PluginOptionsContent : UserControl,IOptionsContent
    {
	    private YukiTheme_VisualPascalABCPlugin plugin;
        public PluginOptionsContent(YukiTheme_VisualPascalABCPlugin plug)
        {
            InitializeComponent();
            plugin = plug;
        }

        private bool alreadyShown;
        #region IOptionsContent Members

        public string ContentName
        {
            get
            {
                return "Yuki Theme";
            }
        }
        public string Description
        {
            get
            {
                return "Yuki Theme Settings";
            }
        }

        public UserControl Content
        {
            get { return this; }
        }

        private bool oldeditor = false;
        private int  st        = -1;
        
        public void Action(OptionsContentAction action)
        {
            switch (action)
            {
                case OptionsContentAction.Show:
	                if (!alreadyShown)
	                {
		                settingsPanel.button1.BackColor =
			                settingsPanel.button4.BackColor = settingsPanel.button5.BackColor =
				                settingsPanel.button6.BackColor =
					                settingsPanel.ActionBox.ListBackColor = settingsPanel.ActionBox.BackColor =
						                settingsPanel.mode.ListBackColor = settingsPanel.mode.BackColor =
							                settingsPanel.textBox1.BackColor = settingsPanel.add_program.BackColor =
								                settingsPanel.add_plugin.BackColor = settingsPanel.tabPage1.BackColor =
									                settingsPanel.BackColor = YukiTheme_VisualPascalABCPlugin.bg;

		                settingsPanel.button1.FlatAppearance.BorderColor =
			                settingsPanel.button4.FlatAppearance.BorderColor =
				                settingsPanel.button5.FlatAppearance.BorderColor =
					                settingsPanel.button6.FlatAppearance.BorderColor =
						                settingsPanel.ActionBox.ForeColor = settingsPanel.ActionBox.ListTextColor =
							                settingsPanel.mode.ForeColor = settingsPanel.mode.ListTextColor =
								                settingsPanel.textBox1.ForeColor = settingsPanel.tabs.ForeColor =
									                settingsPanel.add_program.ForeColor =
										                settingsPanel.add_plugin.ForeColor =
											                settingsPanel.tabPage1.ForeColor =
												                YukiTheme_VisualPascalABCPlugin.clr;

		                settingsPanel.button1.FlatAppearance.MouseOverBackColor =
			                settingsPanel.button4.FlatAppearance.MouseOverBackColor =
				                settingsPanel.button5.FlatAppearance.MouseOverBackColor =
					                settingsPanel.button6.FlatAppearance.MouseOverBackColor =
						                YukiTheme_VisualPascalABCPlugin.bgClick;

		                settingsPanel.ActionBox.BorderColor = settingsPanel.ActionBox.IconColor =
			                settingsPanel.mode.BorderColor =
				                settingsPanel.mode.IconColor = settingsPanel.textBox1.BorderColor =
					                YukiTheme_VisualPascalABCPlugin.bgBorder;

		                settingsPanel.tabs.bg = YukiTheme_VisualPascalABCPlugin.bgBrush;
		                settingsPanel.tabs.bgClick = YukiTheme_VisualPascalABCPlugin.bgClickBrush;
		                if (settingsPanel.tabs.TabPages.Contains (settingsPanel.add_program))
			                settingsPanel.tabs.TabPages.Remove (settingsPanel.add_program);
		                if (!settingsPanel.tabs.TabPages.Contains (settingsPanel.add_plugin))
			                settingsPanel.tabs.TabPages.Add (settingsPanel.add_plugin);
		                if (plugin.mf != null && !plugin.mf.IsDisposed)
		                {
			                plugin.mf.Dispose ();
		                }
		                plugin.mf = null;
		                settingsPanel.mf = null;
		                
		                settingsPanel.backImage.Checked = CLI.bgImage;
		                settingsPanel.swsticker.Checked = CLI.swSticker;
		                settingsPanel.logo.Checked = CLI.swLogo;
		                settingsPanel.editor.Checked = CLI.Editor;
		                settingsPanel.checkBox1.Checked = CLI.Beta;
		                settingsPanel.swStatusbar.Checked = CLI.swStatusbar;
		                settingsPanel.askC.Checked = CLI.askChoice;
		                settingsPanel.checkBox2.Checked = CLI.update;
		                settingsPanel.ActionBox.SelectedIndex = CLI.actionChoice;
		                settingsPanel.mode.SelectedIndex = CLI.settingMode;
		                oldeditor = CLI.Editor;
		                st = CLI.settingMode;
		                
		                alreadyShown = true;
	                }

	                break;
                case OptionsContentAction.Ok:
	                CLI.bgImage = settingsPanel.backImage.Checked;
	                CLI.swSticker = settingsPanel.swsticker.Checked;
	                CLI.swLogo = settingsPanel.logo.Checked;
	                CLI.Editor = settingsPanel.editor.Checked;
	                CLI.Beta = settingsPanel.checkBox1.Checked;
	                CLI.swStatusbar = settingsPanel.swStatusbar.Checked;
	                CLI.askChoice = settingsPanel.askC.Checked;
	                CLI.update = settingsPanel.checkBox2.Checked;
	                CLI.actionChoice = settingsPanel.ActionBox.SelectedIndex;
	                CLI.settingMode = settingsPanel.mode.SelectedIndex;
	                CLI.saveData ();
	                plugin.LoadSticker ();
	                if (settingsPanel.mf != null && !settingsPanel.mf.IsDisposed)
	                {
		                settingsPanel.mf.Dispose ();
	                }
	                settingsPanel.mf = null;
	                plugin.mf = null;
	                alreadyShown = false;
                    //this.Enabled = true;           
                    break;
                case OptionsContentAction.Cancel:
	                if (settingsPanel.mf != null && !settingsPanel.mf.IsDisposed)
	                {
		                settingsPanel.mf.Dispose ();
	                }
	                settingsPanel.mf = null;
	                plugin.mf = null;
                    alreadyShown = false;
                    break;
            }

        }
        #endregion

    }
}