// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System.Reflection;
using System.Windows.Forms;
using VisualPascalABC;
using VisualPascalABC.OptionsContent;
using YukiTheme.Components;
using YukiTheme.Engine;
using YukiTheme.Tools;

namespace Yuki_Theme_Plugin.Controls;

public partial class PluginSettingsControl : UserControl, IOptionsContent
{
	// private string lang = Settings.localization;

	private SettingsPanel _settingsPanel;

	private bool alreadyShown;

	// SettingsPanelUtilities utilities;

	public PluginSettingsControl()
	{
		InitializeComponent();
	}

	#region IOptionsContent Members

	public string ContentName => "Yuki Theme";

	public string Description => "Yuki Theme Settings";

	public UserControl Content => this;

	public void Action(OptionsContentAction action)
	{
		if (action == OptionsContentAction.Show)
			InitSettingsPanel();
		else if (action == OptionsContentAction.Ok)
			SaveSettings();
		else if (action == OptionsContentAction.Cancel)
			CancelChanges();
	}

	private void InitSettingsPanel()
	{
		WpfConverter.ConvertGuiColorsNBrushes();
		if (!alreadyShown)
		{
			WpfConverter.InitAppForWinforms();
			// Form parentForm = ExtractOptionsParent();
			// Settings.Location = SettingsPanelLocation.IDE;
			_settingsPanel = new SettingsPanel();
			var utilities = _settingsPanel.Utilities;

			// API_Events.saveToolBarData = () => plugin.camouflage.SaveData();
			// _settingsPanel.Background = WPFHelper.bgBrush;
			// _settingsPanel.Foreground = WPFHelper.fgBrush;
			// _settingsPanel.Tag = WPFHelper.GenerateTag;
			// _settingsPanel.popupController = plugin.popupController;
			// if (parentForm != null) _settingsPanel.ParentForm = parentForm;
			PanelHost.Child = _settingsPanel;
			Controls.Add(PanelHost);
			VerticalScroll.Enabled = true;
			alreadyShown = true;

			_settingsPanel.Width = Width;
			_settingsPanel.Height = Height;
		}

		_settingsPanel.GetColors();
	}

	private void SaveSettings()
	{
		alreadyShown = false;
		_settingsPanel.SaveSettings();
		IDEAlterer.Instance.ReloadSettings();
		// utilities ??= _settingsPanel._utilities;
		// utilities.SaveSettings();
		// plugin._presenter.ApplySettings (_settingsPanel.settings, true);
	}

	private void CancelChanges()
	{
		// utilities ??= _settingsPanel._utilities;
		// _settingsPanel.IconsList._controller.ReloadToolBar();
		alreadyShown = false;
	}


	private Form ExtractOptionsParent()
	{
		var field = typeof(Form1).GetField("optionsContentEngine", BindingFlags.Instance | BindingFlags.NonPublic);

		var engine = (OptionsContentEngine)field.GetValue(IDEAlterer.Instance.Form1);

		field = typeof(OptionsContentEngine).GetField("optionsWindow", BindingFlags.Instance | BindingFlags.NonPublic);
		return (OptionsForm)field.GetValue(engine);
	}

	#endregion
}