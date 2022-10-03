using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using Microsoft.WindowsAPICodePack.Dialogs;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.WPF.Windows;
using Drawing = System.Drawing;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace Yuki_Theme.Core.WPF.Controls
{
	public partial class SettingsPanel : UserControl
	{
		private Drawing.Size defaultSmallSize = new Drawing.Size (16, 16);
		internal string       customStickerPath;

		private Thickness groupBoxExpanded  = new Thickness (5);
		private Thickness groupBoxCollapsed = new Thickness (0, 5, 0, 5);

		public Window ParentWindow = null;
		public Form   ParentForm   = null;

		private bool blockLanguageSelection;
		public int lockToolBarCheckboxes = 0;

		public Action UpdateExternalTranslations;

		public PopupController popupController;

		internal bool portable;

		public ChangedSettings settings = new ChangedSettings ();
		public SettingsPanelUtilities _utilities;

		public SettingsPanel ()
		{
			InitializeComponent ();
			_utilities = new SettingsPanelUtilities(this);
			LoadSVG ();
		}

		public Thickness InnerMargin
		{
			get { return (Thickness)GetValue (InnerMarginProperty); }
			set { SetValue (InnerMarginProperty, value); }
		}

		public static readonly DependencyProperty InnerMarginProperty =
			DependencyProperty.Register ("InnerMargin", typeof (Thickness), typeof (SettingsPanel),
			                             new PropertyMetadata (new Thickness (0)));

		private void SettingsPanel_OnLoaded (object sender, RoutedEventArgs e)
		{
			blockLanguageSelection = true;
			CheckGridSize ();
			FillLanguageField ();
			_utilities.LoadSettings (this);
			AddLanguages ();
			blockLanguageSelection = false;
			EnableRestartButton ();
			UpdateTranslations ();
			IconsList._panel = this;
			IconsList.Init(this);
		}

		private void EnableRestartButton ()
		{
			RestartForUpdate.IsEnabled = DownloadForm.IsUpdateDownloaded ();
		}

		internal bool IsCommonAPI => Helper.mode == ProductMode.Plugin && CentralAPI.Current is not CommonAPI;


		private void FillLanguageField ()
		{
			LanguageDropdown.Items.Clear ();
		}

		private void LoadSVG ()
		{
			SetResourceSvg ("InfoImage", "balloonInformation", null, defaultSmallSize);
			SetResourceSvg ("HelpImage", "help", null, defaultSmallSize, "Yuki_Theme.Core.Resources.SVG", CentralAPI.Current.GetCore ());
		}


		private void SetResourceSvg (string name, string source, Dictionary <string, Drawing.Color> idColor, Drawing.Size size,
		                             string nameSpace = "Yuki_Theme.Core.WPF.Resources.SVG", Assembly asm = null)
		{
			if (asm == null)
				asm = Assembly.GetExecutingAssembly ();
			Resources [name] = WPFHelper.GetSvg (source, idColor, false, size, nameSpace, asm);
		}

		public void HideFields ()
		{
			EditorSettingsPanel.Visibility = Settings.Editor ? Visibility.Visible : Visibility.Collapsed;
			DoActionPanel.Visibility = Settings.askChoice == true ? Visibility.Collapsed : Visibility.Visible;
		}

		private void AboutButton_OnClick (object sender, RoutedEventArgs e)
		{
			AboutWindow aboutWindow = new AboutWindow
			{
				Background = WPFHelper.bgBrush,
				Foreground = WPFHelper.fgBrush,
				Tag = Tag
			};

			if (ParentWindow != null) aboutWindow.Owner = ParentWindow;
			else if (ParentForm != null)
			{
				WindowInteropHelper helper = new WindowInteropHelper (aboutWindow);
				helper.Owner = ParentForm.Handle;
			}

			aboutWindow.ShowDialog ();
		}

		private void EditorModeCheckChanged (object sender, RoutedEventArgs e)
		{
			EditorSettingsPanel.Visibility = EditorMode.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
		}

		private void AskOthersCheckedChanged (object sender, RoutedEventArgs e)
		{
			DoActionPanel.Visibility = AskOthers.IsChecked == true ? Visibility.Collapsed : Visibility.Visible;
		}

		public void AllowPositioningCheckedChanged (object sender, RoutedEventArgs e)
		{
			ResetMargin.IsEnabled =  PositioningUnit.IsEnabled = AllowPositioning.IsChecked == true;
			if (HideHover.IsChecked != null && (bool)HideHover.IsChecked && AllowPositioning.IsChecked != null && (bool)AllowPositioning.IsChecked)
				HideHover.IsChecked = false;
		}

		private void ChooseCustomSticker (object sender, RoutedEventArgs e)
		{
			CustomStickerWindow customStickerWindow = new CustomStickerWindow
			{
				Background = WPFHelper.bgBrush,
				Foreground = WPFHelper.fgBrush,
				Tag = Tag
			};
			customStickerWindow.ImagePath.Text = customStickerPath;
			if (ParentWindow != null) customStickerWindow.Owner = ParentWindow;
			else if (ParentForm != null)
			{
				WindowInteropHelper helper = new WindowInteropHelper (customStickerWindow);
				helper.Owner = ParentForm.Handle;
			}

			bool? dialog = customStickerWindow.ShowDialog ();
			if (dialog != null && (bool)dialog)
			{
				customStickerPath = customStickerWindow.ImagePath.Text;
				if (CustomSticker.IsChecked != null && (bool)CustomSticker.IsChecked && customStickerPath.Length == 0)
				{
					CustomSticker.IsChecked = false;
				}

			}
		}

		public void StickerDimensionCap_CheckedChanged (object sender, RoutedEventArgs e)
		{
			DimensionCapMax.IsEnabled = DimensionCapBy.IsEnabled = StickerDimensionCap.IsChecked == true;
		}

		private void SettingsPanel_OnSizeChanged (object sender, SizeChangedEventArgs e)
		{
			CheckGridSize ();
		}

		private void CheckGridSize ()
		{
			const int additionalMargins = 125;
			bool canExpand = RenderSize.Width > PositioningPanel.ActualWidth + DimensionCapPanel.ActualWidth + additionalMargins;
			CheckNSetGridLayouts (DimensionCapGroup, canExpand);
			CheckNSetMargin (DimensionCapGroup, canExpand, groupBoxExpanded, groupBoxCollapsed);
		}

		private void CheckNSetGridLayouts (UIElement element, bool toOne)
		{
			if (Grid.GetColumn (element) == (toOne ? 0 : 1))
			{
				Grid.SetColumn (element, toOne ? 1 : 0);
				Grid.SetRow (element, toOne ? 0 : 1);
			}
		}

		private void CheckNSetMargin (FrameworkElement element, bool expand, Thickness expandedMargin, Thickness collapsedMargin)
		{
			element.Margin = expand ? expandedMargin : collapsedMargin;
		}


		private void AddLanguages ()
		{
			foreach (string language in Settings.translation.GetLanguages ())
			{
				LanguageDropdown.Items.Add (language);
			}

			LanguageDropdown.SelectedIndex = Settings.translation.GetIndexOfLangShort (Settings.localization);
		}

		private void UpdateTranslations ()
		{
			WPFHelper.TranslateControls (this, "settings.", "plugin.settings.");
		}

		private void Language_Selection (object sender, SelectionChangedEventArgs e)
		{
			if (!blockLanguageSelection)
			{
				if (Settings.localization != Settings.translation.GetLanguageISO2 ((string)LanguageDropdown.SelectedItem))
				{
					string language = Settings.translation.GetLanguageISO2 ((string)LanguageDropdown.SelectedItem);
					Settings.localization = language;
					Settings.translation.LoadLocale (language);
					UpdateTranslations ();
					if (UpdateExternalTranslations != null)
						UpdateExternalTranslations ();
				}
			}
		}

		private void IconsList_Selection(object sender, SelectionChangedEventArgs e)
		{
			if (IconsList.SelectedItem is ToolBarListItem item)
			{
				ToolBarItemShow.IsEnabled = ToolBarItemRight.IsEnabled = true;
				IconsList._controller.UpdateInfo(item);
			}
			else
			{
				ToolBarItemShow.IsEnabled = ToolBarItemRight.IsEnabled = false;
			}
		}

		private void ToolBarItemShow_CheckedChanged (object sender, RoutedEventArgs e)
		{
			if (IconsList.SelectedItem is ToolBarListItem item)
			{
				DoIfLockIs0(() => item.IsShown = ToolBarItemShow.IsChecked == true);
			}
		}

		private void ToolBarItemRight_CheckedChanged (object sender, RoutedEventArgs e)
		{
			if (IconsList.SelectedItem is ToolBarListItem item)
			{
				DoIfLockIs0(() => item.IsRight = ToolBarItemRight.IsChecked == true);
			}
		}

		private void DoIfLockIs0(Action action)
		{
			if (lockToolBarCheckboxes <= 0)
			{
				action();
			}
			else
			{
				lockToolBarCheckboxes--;
			}
		}

		private void ResetToolbar_OnClick (object sender, RoutedEventArgs e)
		{
			IconsList._controller.ResetToolBar();
		}

		private void SetToolBarHeight ()
		{

		}

		private void RestartForUpdate_OnClick (object sender, RoutedEventArgs e)
		{
			if (DownloadForm.IsUpdateDownloaded ())
			{

			} else
			{
				API_Events.showError ("Update isn't downloaded!", "Update isn't downloaded");
			}
		}

		private void CheckUpdate (object sender, RoutedEventArgs e)
		{

			popupController.CheckUpdate ();
			// popupController.InitializeAllWindows ();
			// popupController.df.CheckUpdate ();			
		}

		private void UpdateManually (object sender, RoutedEventArgs e)
		{
			OpenFileDialog of = new OpenFileDialog ();
			of.DefaultExt = "zip";
			of.Filter = "Yuki Theme(*.zip)|*.zip";
			of.Multiselect = false;
			if (of.ShowDialog () == DialogResult.OK)
			{
				bool has = DownloadForm.IsValidUpdate (of.FileName);

				if (has)
				{
					File.Copy (of.FileName, Path.Combine (
						           Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData),
						           "Yuki Theme",
						           "yuki_theme.zip"), true);
					// popupController.InitializeAllWindows ();
					// popupController.df.InstallManually ();
				} else
				{
					MessageBox.Show (CentralAPI.Current.Translate ("messages.update.invalid"), CentralAPI.Current.Translate ("messages.update.wrong"),
					                 MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		public void HideHover_CheckedChanged (object sender, RoutedEventArgs e)
		{
			HideDelay.IsEnabled = HideHover.IsChecked == true;
			if (HideHover.IsChecked != null && AllowPositioning.IsChecked != null && (bool)AllowPositioning.IsChecked && (bool)HideHover.IsChecked)
				AllowPositioning.IsChecked = false;
		}


		private void SetPascalPath (object sender, RoutedEventArgs e)
		{
			CommonOpenFileDialog co = new CommonOpenFileDialog ();
			co.IsFolderPicker = true;
			co.Multiselect = false;
			co.Title = CentralAPI.Current.Translate ("messages.path.select");
			SelectFolder (co);
		}

		private void SelectFolder (CommonOpenFileDialog co)
		{
			CommonFileDialogResult res = co.ShowDialog ();
			if (res == CommonFileDialogResult.Ok)
			{
				if (Directory.Exists (co.FileName))
				{
					if (IsPascalDirectory (co.FileName))
					{
						PascalPath.Text = co.FileName;
					} else
					{
						if (System.Windows.Forms.MessageBox.Show (CentralAPI.Current.Translate ("messages.path.wrong"), CentralAPI.Current.Translate ("messages.path.select"), MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
						{
							co.Title = CentralAPI.Current.Translate ("messages.path.wrong");
							SelectFolder (co);
						}
					}
				} else
				{
					throw new FileLoadException (CentralAPI.Current.Translate ("messages.directory.notexist"));
				}
			}
		}

		internal bool IsPascalDirectory (string path)
		{

			return File.Exists (Path.Combine (path, "PascalABCNET.exe")) && File.Exists (Path.Combine (path, "pabcnetc.exe"));
		}
		private void ResetMargin_Click (object sender, RoutedEventArgs e)
		{
			settings.ResetMargins = true;
		}
	}
}