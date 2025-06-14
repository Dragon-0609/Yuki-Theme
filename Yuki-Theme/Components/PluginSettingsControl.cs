// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System.Reflection;
using System.Windows.Forms;
using VisualPascalABC;
using VisualPascalABC.OptionsContent;
using YukiTheme.Engine;
using YukiTheme.Tools;

namespace YukiTheme.Components;

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
			_settingsPanel = new SettingsPanel();
			PanelHost.Child = _settingsPanel;
			Controls.Add(PanelHost);
			VerticalScroll.Enabled = true;
			alreadyShown = true;

			_settingsPanel.Width = Width;
			_settingsPanel.Height = Height;
		}
		else
		{
			_settingsPanel.UpdateThemeNames();
		}

		_settingsPanel.GetColors();
	}

	private void SaveSettings()
	{
		alreadyShown = false;
		_settingsPanel.SaveSettings();
		IDEAlterer.Instance.ReloadSettings();
	}

	private void CancelChanges()
	{
		alreadyShown = false;
	}


	public Form ExtractOptionsParent()
	{
		var field = typeof(Form1).GetField("optionsContentEngine", BindingFlags.Instance | BindingFlags.NonPublic);

		var engine = (OptionsContentEngine)field.GetValue(IDEAlterer.Instance.Form1);

		field = typeof(OptionsContentEngine).GetField("optionsWindow", BindingFlags.Instance | BindingFlags.NonPublic);
		return (OptionsForm)field.GetValue(engine);
	}

	#endregion
}