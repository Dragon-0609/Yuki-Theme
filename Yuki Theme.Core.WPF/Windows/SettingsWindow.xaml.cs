using System.Windows;
using System.Windows.Controls;
using Yuki_Theme.Core.WPF.Controls;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class SettingsWindow : Window
	{
		public  SettingsPanelUtilities utilities;
		
		internal bool dimensionCap  = false;
		internal bool customSticker = false;
		
		public SettingsWindow ()
		{
			InitializeComponent ();
			dimensionCap = Settings.useDimensionCap;
			customSticker = Settings.useCustomSticker;
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
			utilities = new SettingsPanelUtilities (SettingsPanelControl);
			if (Helper.mode == ProductMode.Plugin)
			{
				SettingsPanelControl.ExecuteOnLoad = utilities.PopulateToolBarList;
				SettingsPanelControl.ExecuteOnToolBarItemSelection = utilities.ToolBarItemSelection;
			}

			SettingsPanelControl.Background = WPFHelper.bgBrush;
			SettingsPanelControl.Foreground = WPFHelper.fgBrush;
			SettingsPanelControl.Tag = Tag;
			SettingsPanelControl.ParentWindow = this;
		}
	}
}