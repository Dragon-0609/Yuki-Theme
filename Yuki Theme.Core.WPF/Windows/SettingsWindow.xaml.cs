using System.Windows;
using System.Windows.Controls;
using Yuki_Theme.Core.WPF.Controls;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class SettingsWindow : Window
	{
		public SettingsWindow ()
		{
			InitializeComponent ();
		}

		private void SaveButtonClick (object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		private void CancleButtonClick (object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}

		private void SettingsWindow_OnLoaded (object sender, RoutedEventArgs e)
		{
			SettingsPanelControl.ParentWindow = this;
			SettingsPanelControl.UpdateExternalTranslations = TranslateDialogButtons;
			TranslateDialogButtons ();
			IncludeToolBarItems ();
		}
		
		private void TranslateDialogButtons ()
		{
			SaveButton.Content = CLI.Translate ("messages.theme.save.short");
			CancelButton.Content = CLI.Translate ("download.cancel");
		}

		private void IncludeToolBarItems ()
		{
			SettingsPanelUtilities utilities = new SettingsPanelUtilities (SettingsPanelControl);
			SettingsPanelControl.ExecuteOnLoad = utilities.PopulateToolBarList;
			SettingsPanelControl.ExecuteOnToolBarItemSelection = utilities.ToolBarItemSelection;
			SettingsPanelControl.Background = WPFHelper.bgBrush;
			SettingsPanelControl.Foreground = WPFHelper.fgBrush;
			SettingsPanelControl.Tag = Tag;
			SettingsPanelControl.ParentWindow = this;
		}
	}
}