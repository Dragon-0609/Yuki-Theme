using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Yuki_Theme.Core.WPF.Controls;
using YukiTheme.Engine;
using YukiTheme.Tools;
using UserControl = System.Windows.Controls.UserControl;

namespace YukiTheme.Components
{
	public partial class SettingsPanel : UserControl
	{
		public SettingsPanelUtilities Utilities;
		private bool _resetStickerMargin = false;
		private readonly string[] _themes = DefaultThemeNames.Themes.Union(DokiThemeNames.Themes).ToArray();

		public SettingsPanel()
		{
			InitializeComponent();
			_resetStickerMargin = false;
			Utilities = new SettingsPanelUtilities(this);
			Utilities.LoadSettings();
			Themes.ItemsSource = _themes;
			SelectCurrentTheme();
		}

		private void SelectCurrentTheme()
		{
			string name = NameBar.ThemeName;
			// IDEConsole.Log(name);
			if (Themes.Items.Contains(name))
			{
				Themes.SelectedItem = name;
			}
			else
			{
				Themes.SelectedIndex = 0;
			}

			Themes.ScrollToCenterOfView(Themes.SelectedItem);
		}

		internal void GetColors()
		{
			Background = WpfColorContainer.Instance.BackgroundBrush;
			Foreground = WpfColorContainer.Instance.ForegroundBrush;
			Themes.Background = Background;
			Themes.Foreground = Foreground;
			Themes.Tag = Tag = WpfColorContainer.Instance;
		}

		private void ResetMargin_Click(object sender, RoutedEventArgs e)
		{
			_resetStickerMargin = true;
		}

		private void ChooseCustomSticker(object sender, RoutedEventArgs e)
		{
		}

		internal void SaveSettings()
		{
			Utilities.SaveSettings();
			if (_resetStickerMargin)
			{
				IDEConsole.Log("Resetting");
				Utilities.ResetStickerMargin();
			}

			_resetStickerMargin = false;
			if (Themes.SelectedItem.ToString() != NameBar.ThemeName)
			{
				PluginEvents.Instance.OnThemeChanged(Themes.SelectedItem.ToString());
			}
		}
	}
}