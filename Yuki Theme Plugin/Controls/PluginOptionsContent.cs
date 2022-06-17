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
			/*settingsPanel.isFromPascal = true; // To recognize
			settingsPanel.SettingsPanel_Load ();*/
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

		public static StyleConfig GenerateTag => new StyleConfig
		{
			BorderColor = YukiTheme_VisualPascalABCPlugin.bgBorder.ToWPFColor (),
			SelectionColor = YukiTheme_VisualPascalABCPlugin.bgSelection.ToWPFColor (),
			KeywordColor = YukiTheme_VisualPascalABCPlugin.clrKey.ToWPFColor (),
			BorderBrush = YukiTheme_VisualPascalABCPlugin.bgBorder.ToWPFColor ().ToBrush (),
			SelectionBrush = YukiTheme_VisualPascalABCPlugin.bgSelection.ToWPFColor ().ToBrush (),
			KeywordBrush = YukiTheme_VisualPascalABCPlugin.clrKey.ToWPFColor ().ToBrush (),
			BackgroundClickColor = YukiTheme_VisualPascalABCPlugin.bgClick.ToWPFColor (),
			BackgroundClickBrush = YukiTheme_VisualPascalABCPlugin.bgClick.ToWPFColor ().ToBrush (),
			BackgroundDefaultColor = YukiTheme_VisualPascalABCPlugin.bgdef.ToWPFColor (),
			BackgroundDefaultBrush = YukiTheme_VisualPascalABCPlugin.bgdef.ToWPFColor ().ToBrush ()
		};

		public void Action (OptionsContentAction action)
		{
			switch (action)
			{
				case OptionsContentAction.Show :
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
						_settingsPanel.Tag = GenerateTag;
						if (parentForm != null) _settingsPanel.ParentForm = parentForm;

						PanelHost.Child = _settingsPanel;
						Controls.Add (PanelHost);
						alreadyShown = true;
					}

					break;
				case OptionsContentAction.Ok :
					alreadyShown = false;
					utilities ??= new SettingsPanelUtilities (_settingsPanel);
					utilities.SaveSettings ();
					if (dimensionCap)
					{
						plugin.SettingsChanged (customSticker, dimensionCap);
					}
					break;
				case OptionsContentAction.Cancel :
					utilities ??= new SettingsPanelUtilities (_settingsPanel);
					utilities.ResetToolBar ();
					alreadyShown = false;
					break;
			}
		}

		
		
		private Form ExtractOptionsParent ()
		{
			FieldInfo field = typeof (Form1).GetField ("optionsContentEngine", BindingFlags.Instance | BindingFlags.NonPublic);

			OptionsContentEngine engine = (OptionsContentEngine)field.GetValue (plugin.fm);

			field = typeof (OptionsContentEngine).GetField ("optionsWindow", BindingFlags.Instance | BindingFlags.NonPublic);
			return (OptionsForm)field.GetValue (engine);
		}

		#endregion
	}
}