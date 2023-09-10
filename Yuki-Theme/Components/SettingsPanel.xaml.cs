using System.Collections.Generic;
using System.Windows;
using Yuki_Theme.Core.WPF.Controls;
using YukiTheme.Tools;
using UserControl = System.Windows.Controls.UserControl;

namespace YukiTheme.Components
{
	public partial class SettingsPanel : UserControl
	{
		public SettingsPanelUtilities Utilities;
		private bool _resetStickerMargin = false;

		public SettingsPanel()
		{
			InitializeComponent();
			_resetStickerMargin = false;
			Utilities = new SettingsPanelUtilities(this);
			Utilities.LoadSettings();
			Themes.ItemsSource = new List<string> { "Item 1", "Item 2", "Item 3" };
		}

		public void GetColors()
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

		public void SaveSettings()
		{
			Utilities.SaveSettings();
			if (_resetStickerMargin)
				Utilities.ResetStickerMargin();
			_resetStickerMargin = false;
		}
	}
}