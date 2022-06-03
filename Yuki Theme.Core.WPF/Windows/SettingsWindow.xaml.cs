using System.Windows;
using System.Windows.Controls;

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
		}
		
		private void TranslateDialogButtons ()
		{
			SaveButton.Content = CLI.Translate ("messages.theme.save.short");
			CancelButton.Content = CLI.Translate ("download.cancel");
		}
	}
}