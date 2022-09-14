// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using VisualPascalABC;
using VisualPascalABC.OptionsContent;
using Yuki_Theme.Core;
using Yuki_Theme.Core.WPF;
using Yuki_Theme.Core.WPF.Controls;
using MessageBox = System.Windows.MessageBox;

namespace Yuki_Theme_Plugin.Controls
{
	public partial class PluginOptionsContent : UserControl, IOptionsContent
	{
		private YukiTheme_VisualPascalABCPlugin plugin;

		private string lang = Settings.localization;

		private SettingsPanel _settingsPanel;

		private bool dimensionCap = false;
		private bool customSticker = false;

		SettingsPanelUtilities utilities;
		
		public PluginOptionsContent (YukiTheme_VisualPascalABCPlugin plug)
		{
			InitializeComponent ();
			plugin = plug;
		}

		private bool alreadyShown;

		#region IOptionsContent Members

		public string ContentName => "Yuki Theme";

		public string Description => "Yuki Theme Settings";

		public UserControl Content => this;

		public void Action (OptionsContentAction action)
		{
			if (action == OptionsContentAction.Show)
				InitSettingsPanel ();
			else if (action == OptionsContentAction.Ok)
				SaveSettings ();
			else if (action == OptionsContentAction.Cancel)
				CancelChanges ();
		}

		private void InitSettingsPanel ()
		{
			WPFHelper.InitAppForWinforms ();
			WPFHelper.ConvertGUIColorsNBrushes ();
			dimensionCap = Settings.useDimensionCap;
			customSticker = Settings.useCustomSticker;
			if (!alreadyShown)
			{
				Form parentForm = ExtractOptionsParent ();
				_settingsPanel = new SettingsPanel ();
				utilities = new SettingsPanelUtilities (_settingsPanel);

				_settingsPanel.ExecuteOnLoad = utilities.PopulateToolBarList;
				_settingsPanel.ExecuteOnToolBarItemSelection = utilities.ToolBarItemSelection;
				_settingsPanel.Background = WPFHelper.bgBrush;
				_settingsPanel.Foreground = WPFHelper.fgBrush;
				_settingsPanel.Tag = WPFHelper.GenerateTag;
				_settingsPanel.popupController = plugin.popupController;
				// _settingsPanel.popupController = plugin.popupController;
				if (parentForm != null) _settingsPanel.ParentForm = parentForm;

				PanelHost.Child = _settingsPanel;
				Controls.Add (PanelHost);
				alreadyShown = true;
			}
		}

		private void SaveSettings ()
		{
			alreadyShown = false;
			utilities ??= new SettingsPanelUtilities (_settingsPanel);
			utilities.SaveSettings ();
			if (dimensionCap)
			{
				plugin.SettingsChanged (customSticker, dimensionCap);
			}
		}

		private void CancelChanges ()
		{
			utilities ??= new SettingsPanelUtilities (_settingsPanel);
			utilities.ResetToolBar ();
			alreadyShown = false;
		}


		private Form ExtractOptionsParent ()
		{
			FieldInfo field = typeof (Form1).GetField ("optionsContentEngine", BindingFlags.Instance | BindingFlags.NonPublic);

			OptionsContentEngine engine = (OptionsContentEngine)field.GetValue (plugin.ideComponents.fm);

			field = typeof (OptionsContentEngine).GetField ("optionsWindow", BindingFlags.Instance | BindingFlags.NonPublic);
			return (OptionsForm)field.GetValue (engine);
		}

		#endregion
	}
}