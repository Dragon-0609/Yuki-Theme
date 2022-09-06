using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.WPF.Windows;
using CheckBox = System.Windows.Controls.CheckBox;
using ComboBox = System.Windows.Controls.ComboBox;
using Drawing = System.Drawing;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;
using UserControl = System.Windows.Controls.UserControl;

namespace Yuki_Theme.Core.WPF.Controls
{
	public partial class SettingsPanel : UserControl
	{
		private Drawing.Size defaultSmallSize = new Drawing.Size (16, 16);
		private string       customStickerPath;

		private Thickness groupBoxExpanded  = new Thickness (5);
		private Thickness groupBoxCollapsed = new Thickness (0, 5, 0, 5);

		public Window ParentWindow = null;
		public Form   ParentForm   = null;

		private bool blockLanguageSelection = false;

		public Action UpdateExternalTranslations;

		public Action                   ExecuteOnLoad;
		public Action <ToolBarListItem> ExecuteOnToolBarItemSelection;

		private bool _freezeToolBarBehaviour;

		public PopupController popupController;

		public SettingsPanel ()
		{
			InitializeComponent ();

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
			LoadSettings ();
			AddLanguages ();
			blockLanguageSelection = false;
			EnableRestartButton ();
			UpdateTranslations ();
			if (ExecuteOnLoad != null)
				ExecuteOnLoad ();
		}

		private void EnableRestartButton ()
		{
			RestartForUpdate.IsEnabled = DownloadForm.IsUpdateDownloaded ();
		}


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

		private void LoadSettings ()
		{
			HideFields ();
			FillGeneralValues ();
			FillProgramValues ();
			FillPluginValues ();
			StickerDimensionCapCheckedChanged (this, null);
		}

		private void HideFields ()
		{
			EditorSettingsPanel.Visibility = Settings.Editor ? Visibility.Visible : Visibility.Collapsed;
			DoActionPanel.Visibility = Settings.askChoice == true ? Visibility.Collapsed : Visibility.Visible;
		}

		private void FillGeneralValues ()
		{
			SCheck (EditorMode, Settings.Editor);
			SCheck (ShowSticker, Settings.swSticker);
			SCheck (AllowPositioning, Settings.positioning);
			SCheck (CustomSticker, Settings.useCustomSticker);
			SCheck (ShowBackgroundImage, Settings.bgImage);
			SCheck (AutoFit, Settings.autoFitByWidth);
			SCheck (AlwaysAsk, Settings.askToSave);
			SCheck (AutoUpdate, Settings.update);
			SCheck (CheckBeta, Settings.Beta);
			SCheck (StickerDimensionCap, Settings.useDimensionCap);
			SDrop (EditorModeDropdown, (int)Settings.settingMode);
			SDrop (DimensionCapBy, Settings.dimensionCapUnit);
			customStickerPath = Settings.customSticker;
			SText (DimensionCapMax.box, Settings.dimensionCapMax.ToString ());
			bool firstSelected = Settings.colorPicker == 0;
			WinformsPicker.IsChecked = firstSelected;
			WPFPicker.IsChecked = !firstSelected;
			// LanguageDropdown, Settings.localization
		}

		private void FillProgramValues ()
		{
			bool isProgram = Helper.mode == ProductMode.Program;
			HideHeader (isProgram);
			if (isProgram)
			{
				SText (PascalPath, Settings.pascalPath);
				SCheck (AskOthers, Settings.askChoice);
				SDrop (ActionDropdown, Settings.actionChoice);
			}
		}

		private void FillPluginValues ()
		{
			bool isPlugin = Helper.mode == ProductMode.Plugin;
			HideHeader (!isPlugin);
			if (isPlugin)
			{
				SCheck (LogoStart, Settings.swLogo);
				SCheck (NameStatusBar, Settings.swStatusbar);
				SCheck (Preview, Settings.showPreview);
			}
		}

		private void HideHeader (bool isProgram)
		{
			ProgramAdd.Visibility = isProgram ? Visibility.Visible : Visibility.Collapsed;
			PluginAdd.Visibility = PluginTool.Visibility = isProgram ? Visibility.Collapsed : Visibility.Visible;
		}

		public void SaveSettings ()
		{
			SaveGeneralSettings ();
			SaveProgramSettings ();
			SavePluginSettings ();
		}

		private void SaveGeneralSettings ()
		{
			KCheck (EditorMode, ref Settings.Editor);
			KCheck (ShowSticker, ref Settings.swSticker);
			KCheck (AllowPositioning, ref Settings.positioning);
			KCheck (CustomSticker, ref Settings.useCustomSticker);
			KCheck (ShowBackgroundImage, ref Settings.bgImage);
			KCheck (AutoFit, ref Settings.autoFitByWidth);
			KCheck (AlwaysAsk, ref Settings.askToSave);
			KCheck (AutoUpdate, ref Settings.update);
			KCheck (CheckBeta, ref Settings.Beta);
			KCheck (CheckBeta, ref Settings.Beta);
			KCheck (StickerDimensionCap, ref Settings.useDimensionCap);
			KDrop (DimensionCapBy, ref Settings.dimensionCapUnit);

			Settings.dimensionCapMax = DimensionCapMax.GetNumber ();
			Settings.settingMode = (SettingMode)EditorModeDropdown.SelectedIndex;
			Settings.customSticker = customStickerPath;
			Settings.colorPicker = WinformsPicker.IsChecked == true ? 0 : 1;
		}

		private void SaveProgramSettings ()
		{
			bool isProgram = Helper.mode == ProductMode.Program;
			if (isProgram)
			{
				KText (PascalPath, ref Settings.pascalPath);
				KCheck (AskOthers, ref Settings.askChoice);
				KDrop (ActionDropdown, ref Settings.actionChoice);
			}
		}

		private void SavePluginSettings ()
		{
			bool isPlugin = Helper.mode == ProductMode.Plugin;
			if (isPlugin)
			{
				KCheck (LogoStart, ref Settings.swLogo);
				KCheck (NameStatusBar, ref Settings.swStatusbar);
				KCheck (Preview, ref Settings.showPreview);
			}
		}

		#region Helper Methods

		/// <summary>
		/// Save Checkbox.IsChecked to bool
		/// </summary>
		/// <param name="checkBox">Checkbox to save from</param>
		/// <param name="target">Target to set the bool</param>
		private void KCheck (CheckBox checkBox, ref bool target)
		{
			target = checkBox.IsChecked == true;
		}

		/// <summary>
		/// Save Selected Index of combobox to int
		/// </summary>
		private void KDrop (ComboBox dropDown, ref int value)
		{
			value = dropDown.SelectedIndex;
		}

		/// <summary>
		/// Save Text of textbox to string
		/// </summary>
		private void KText (TextBox textBox, ref string value)
		{
			value = textBox.Text;
		}

		/// <summary>
		/// Set value of combobox
		/// </summary>
		private void SDrop (ComboBox dropDown, int value)
		{
			dropDown.SelectedIndex = value;
		}

		/// <summary>
		/// Set value of checkbox
		/// </summary>
		private void SCheck (CheckBox checkBox, bool value)
		{
			checkBox.IsChecked = value;
		}

		/// <summary>
		/// Set value of textbox
		/// </summary>
		private void SText (TextBox textBox, string value)
		{
			textBox.Text = value;
		}

		#endregion

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

		private void AllowPositioningCheckedChanged (object sender, RoutedEventArgs e)
		{
			ResetMargin.IsEnabled = AllowPositioning.IsChecked == true;
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
				customStickerPath = customStickerWindow.ImagePath.Text;
		}

		private void StickerDimensionCapCheckedChanged (object sender, RoutedEventArgs e)
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
			WPFHelper.TranslateControls (this, "settings.");
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

		private void IconsList_Selection (object sender, SelectionChangedEventArgs e)
		{
			if (IconsList.SelectedItem != null && IconsList.SelectedItem is ToolBarListItem item)
			{
				ToolBarItemShow.IsEnabled = ToolBarItemRight.IsEnabled = true;
				if (ExecuteOnToolBarItemSelection != null)
				{
					_freezeToolBarBehaviour = true;
					ExecuteOnToolBarItemSelection (item);
					_freezeToolBarBehaviour = false;
				}
				// ToolBarItemShow.IsChecked = 
			} else
			{
				ToolBarItemShow.IsEnabled = ToolBarItemRight.IsEnabled = false;
			}
		}

		private void ToolBarItemShow_CheckedChanged (object sender, RoutedEventArgs e)
		{
			if (IconsList.SelectedItem != null && IconsList.SelectedItem is ToolBarListItem item)
			{
				item.IsShown = ToolBarItemShow.IsChecked == true;
			}
		}

		private void ToolBarItemRight_CheckedChanged (object sender, RoutedEventArgs e)
		{
			if (IconsList.SelectedItem != null && IconsList.SelectedItem is ToolBarListItem item)
			{
				item.IsRight = ToolBarItemRight.IsChecked == true;
			}
		}

		private void ResetToolbar_OnClick (object sender, RoutedEventArgs e)
		{
			ToolBarListItem.camouflage.Reset ();
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
	}
}