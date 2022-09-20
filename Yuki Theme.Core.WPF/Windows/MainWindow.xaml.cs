using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Media;
using Microsoft.WindowsAPICodePack.Dialogs;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Interfaces;
using Yuki_Theme.Core.Parsers;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.Utils;
using Yuki_Theme.Core.WPF.Interfaces;
using Application = System.Windows.Application;
using DataFormats = System.Windows.DataFormats;
using DragEventArgs = System.Windows.DragEventArgs;
using Drawing = System.Drawing;
using MessageBox = System.Windows.MessageBox;
using Timer = System.Windows.Forms.Timer;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class MainWindow : IColorUpdatable, Main.IView
	{

		public readonly Main.Model Model;
		private readonly Main.IPresenter _presenter;
		
		private PopupController _popupController;

		private readonly Drawing.Size _defaultSize = new Drawing.Size(20, 20);

		private bool _blockOpacity = false;

		private int _restoredTimes = 0;

		private Timer InstallTrackerTimer;

		#region Initialization

		public MainWindow ()
		{
			InitializeComponent ();
			Model = new MainModel();
			_presenter = new MainPresenter(Model, this, this);
		}

		private void Init (object sender, RoutedEventArgs e)
		{
			ChangeIcon();
			_presenter.SetAPIActions();

			Model.ChangeProductMode(ProductMode.Program, SwitchPluginMode);

			Model.InitSticker(this);

			Model.InitHighlighter(Fstb.box);
			
			_presenter.LoadThemesWithApi(Themes, Definitions);
			
			Model.InitializeSyntax ();
			
			Fstb.box.Paint += bgImagePaint;
			ToggleEditor ();
			UpdateTranslations ();
			
			InitAdditionalComponents ();
		}

		private void LoadDefinitionsWithSelection ()
		{
			int prevSelectedField = Definitions.SelectedIndex;
			_presenter.LoadDefinitions(Definitions);
			Definitions.SelectedIndex = prevSelectedField != -1 ? prevSelectedField : 0;
		}

		private void LoadSVG ()
		{
			string add = Helper.IsDark (ColorKeeper.bgColor) ? "" : "_dark";
			WPFHelper.SetSVGImage (AddButton, "add" + add);
			WPFHelper.SetSVGImage (ManageButton, "listFiles" + add);
			WPFHelper.SetSVGImage (SaveButton, "menu-saveall");
			WPFHelper.SetSVGImage (RestoreButton, "refresh" + add);
			WPFHelper.SetSVGImage (ExportButton, "export" + add);
			WPFHelper.SetSVGImage (ImportButton, "import" + add);
			WPFHelper.SetSVGImage (ImportDirectoryButton, "traceInto" + add, true, ColorKeeper.bgBorder);
			WPFHelper.SetSVGImage (SettingsButton, "gearPlain" + add, true, ColorKeeper.bgBorder);
			WPFHelper.SetSVGImage (DownloaderButton, "smartStepInto" + add, true, ColorKeeper.bgBorder);
			WPFHelper.SetSVGImage (ImagePathButton, "moreHorizontal" + add);
			WPFHelper.SetSVGImage (LAlignButton, "positionLeft" + add);
			WPFHelper.SetSVGImage (CAlignButton, "positionCenter" + add);
			WPFHelper.SetSVGImage (RAlignButton, "positionRight" + add);
		}

		
		private void UpdateTranslations ()
		{
			WPFHelper.TranslateControls (this, "ui.tooltips.", "main.labels.");
		}

		private void ReLoadCheckBoxImages ()
		{
			try
			{
				if (Application.Current.MainWindow != null)
				{
					ResourceDictionary mergedDict = null;
					if (Helper.mode == ProductMode.Program)
					{
						mergedDict =
							Application.Current.MainWindow.Resources.MergedDictionaries.FirstOrDefault (
								md => md.Source.ToString ().Contains ("CheckboxStyles.xaml"));
					} else if (Helper.mode == ProductMode.Plugin)
					{
						mergedDict =
							Application.Current.Resources.MergedDictionaries.FirstOrDefault (
								md => md.Source.ToString ().Contains ("CheckboxStyles.xaml"));
					}

					if (mergedDict != null)
					{
						Dictionary <string, Drawing.Color> idColors = WPFHelper.GenerateBGColors ();
						var disabledIdColors = WPFHelper.GenerateDisabledBGColors ();
						mergedDict.SetResourceSvg ("checkBoxDefault", "checkBox", idColors, _defaultSize);
						mergedDict.SetResourceSvg ("checkBoxDisabled", "checkBoxDisabled", disabledIdColors, _defaultSize);
						mergedDict.SetResourceSvg ("checkBoxFocused", "checkBoxFocused", idColors, _defaultSize);
						mergedDict.SetResourceSvg ("checkBoxSelected", "checkBoxSelected", idColors, _defaultSize);
						mergedDict.SetResourceSvg ("checkBoxSelectedDisabled", "checkBoxSelectedDisabled", disabledIdColors, _defaultSize);
						mergedDict.SetResourceSvg ("checkBoxSelectedFocused", "checkBoxSelectedFocused", idColors, _defaultSize);
					}
				}
			} catch (Exception e)
			{
				Console.WriteLine ($"ERORR >> {e.Message}\nTRACE >> {e.StackTrace}, {e.ToString ()}");
			}
		}

		#endregion


		#region Main Methods

		private void AddTheme ()
		{
			ThemeAddition res = WPFHelper.AddTheme (this, Themes.SelectedItem.ToString ());
			if (res.save != null && (bool)res.save)
			{
				InsertTheme (res);
			}
		}

		private void InsertTheme (ThemeAddition res)
		{
			_presenter.InsertTheme(res, Themes);
		}

		private void ManageThemes ()
		{
			ManageThemesWindow themesWindow = new ManageThemesWindow
			{
				Background = WPFHelper.bgBrush,
				Foreground = WPFHelper.fgBrush,
				Tag = Tag,
				Owner = this
			};

			themesWindow.ShowDialog ();
			_presenter.LoadThemesToUi(Themes, Definitions);
		}


		public void OpenSettings ()
		{
			SettingsWindow settingsWindow = new SettingsWindow
			{
				Background = WPFHelper.bgBrush,
				Foreground = WPFHelper.fgBrush,
				Tag = Tag,
				Icon = Icon,
				Owner = this,
				popupController = _popupController
			};

			bool? dialog = settingsWindow.ShowDialog ();
			string lang = settingsWindow.SettingsPanelControl.settings.Lang;
			if (dialog != null && (bool)dialog)
			{
				settingsWindow.utilities.SaveSettings ();
				ApplySettingsChanges (settingsWindow.SettingsPanelControl.settings);
			} else
			{
				Settings.localization = lang;
				Settings.translation.LoadLocale (lang);
				settingsWindow.utilities.ResetToolBar ();
			}
			if (lang != Settings.localization)
			{
				UpdateTranslations ();
			}

		}

		private void Restore ()
		{
			bool clean = _restoredTimes >= 3;
			_restoredTimes = _restoredTimes >= 3 ? 0 : _restoredTimes + 1;
			
			CentralAPI.Current.Restore (clean, null);
			LoadDefinitionsWithSelection ();
			Model.UpdateSyntaxColors();
			UpdateColors ();
			SetOpacityWallpaper ();
			Model.LoadSticker();
			LoadSVG ();
			ReLoadCheckBoxImages ();
		}

		private void SelectField ()
		{
			if (Definitions.SelectedItem != null)
			{
				ColorPanel.Visibility = Visibility.Collapsed;
				ImagePanel.Visibility = Visibility.Collapsed;
				if (IsColorField ())
				{
					ColorPanel.Visibility = Visibility.Visible;
					BoldCheckBox.IsEnabled = ItalicCheckBox.IsEnabled =
						HighlitherUtil.IsInColors (Definitions.SelectedItem.ToString ()) && !CentralAPI.Current.currentTheme.isDefault;

					BoldCheckBox.Visibility = ItalicCheckBox.Visibility =
						HighlitherUtil.IsInColors (Definitions.SelectedItem.ToString ()) ? Visibility.Visible : Visibility.Collapsed;

					ThemeField dic = CentralAPI.Current.currentTheme.Fields [Definitions.SelectedItem.ToString ()];

					if (dic.Foreground != null)
					{
						ColorPanel.Resources ["ForeBrush"] =
							new SolidColorBrush (Drawing.ColorTranslator.FromHtml (dic.Foreground).ToWPFColor ());
						FGColorPanel.Visibility = Visibility.Visible;
					} else
					{
						FGColorPanel.Visibility = Visibility.Hidden;
					}

					if (dic.Background != null)
					{
						ColorPanel.Resources ["BackBrush"] =
							new SolidColorBrush (Drawing.ColorTranslator.FromHtml (dic.Background).ToWPFColor ());
						BGColorPanel.Visibility = Visibility.Visible;
					} else
					{
						BGColorPanel.Visibility = Visibility.Hidden;
					}

					if (dic.Bold != null)
					{
						BoldCheckBox.IsChecked = (bool)dic.Bold;
					}

					if (dic.Italic != null)
					{
						ItalicCheckBox.IsChecked = (bool)dic.Bold;
					}

					Model.ActivateSyntaxColors(Definitions.SelectedItem.ToString());
				} else
				{
					ImagePanel.Visibility = Visibility.Visible;
					AlignPanel.Visibility = Visibility.Collapsed;
					OpacityPanel.Visibility = Visibility.Collapsed;
					ImagePath.Text = "";
					_blockOpacity = true;
					OpacitySlider.Value = 0;
					if (IsWallpaperDefinition ())
					{
						AlignPanel.Visibility = Visibility.Visible;
						OpacityPanel.Visibility = Visibility.Visible;
						ShowAlignSelection ();

						if (CentralAPI.Current.currentTheme.HasWallpaper)
						{
							ImagePath.Text = "wallpaper.png";
							OpacitySlider.Value = CentralAPI.Current.currentTheme.WallpaperOpacity;
						}
					} else
					{
						if (CentralAPI.Current.currentTheme.HasSticker)
						{
							ImagePath.Text = "sticker.png";
						}
					}

					_blockOpacity = false;
				}
			}
		}

		private void Save ()
		{
			CentralAPI.Current.Save (Model.WallpaperOriginal, Model.Sticker);
		}

		private void Export ()
		{
			CentralAPI.Current.ExportTheme (Model.WallpaperOriginal, Model.Sticker, Model.InvokeSetTheme, Model.InvokeStartSettingTheme);
		}

		private void ImportFile()
		{
			_presenter.ImportFile(ImportUIAddition, ImportThemeReset);
		}

		private void ImportFolder ()
		{
			CommonOpenFileDialog dialog = new CommonOpenFileDialog ();
			dialog.IsFolderPicker = true;
			dialog.Multiselect = false;
			_presenter.SetImportsCancel (false);
			CommonFileDialogResult res = dialog.ShowDialog ();
			if (res == CommonFileDialogResult.Ok)
			{
				// MessageBox.Show (CentralAPI.Current.Translate ("main.import.directory"));

				_presenter.PrepareProgressWindow (dialog.FileName);
				Thread thread = new Thread (() => ImportFolderWithProgress(dialog.FileName));
				thread.Start();
				// Themes.SelectedIndex = Themes.Items.Count - 1;
			}
		}
		private void ImportFolderWithProgress (string location)
		{
			_presenter.ImportFiles (location, "*.json", ImportUIAddition, ImportThemeReset);
			_presenter.ImportFiles (location, "*.icls", ImportUIAddition, ImportThemeReset);
			_presenter.ImportDirectoryDone ();
			Dispatcher.Invoke (() => Themes.SelectedIndex = Themes.Items.Count - 1);
		}

		private void OpenDownloader ()
		{
			ThemeDownloaderForm downloaderForm = new ThemeDownloaderForm ();
			downloaderForm.Show();
		}

		#endregion


		#region Core Actions
		
		private void ImportUIAddition (string fileName)
		{
			Dispatcher.Invoke (() => Themes.Items.Add (fileName));
		}

		private void ImportThemeReset (string fileName)
		{
			Dispatcher.Invoke (() =>
			{
				if (Themes.SelectedItem.ToString () != fileName)
					Themes.SelectedItem = fileName;
				else
					Restore ();
			});
		}
		
		#endregion


		#region Helper Methods

		private void SetOpacityWallpaper ()
		{
			Model.ChangeWallpaperOpacity();

			Fstb.box.Refresh ();
		}

		private bool IsColorField ()
		{
			if (Definitions.SelectedItem == null)
				return true;
			return !ShadowNames.imageNames.Contains (Definitions.SelectedItem.ToString ());
		}

		private void SetAlign (Alignment align)
		{
			CentralAPI.Current.currentTheme.WallpaperAlign = (int)align;
			ShowAlignSelection ();
			Fstb.box.Refresh ();
		}

		private void ShowAlignSelection ()
		{
			LAlignButton.IsSelected = CAlignButton.IsSelected = RAlignButton.IsSelected = false;

			if (CentralAPI.Current.currentTheme.align == Alignment.Left)
				LAlignButton.IsSelected = true;
			else if (CentralAPI.Current.currentTheme.align == Alignment.Center)
				CAlignButton.IsSelected = true;
			else
				RAlignButton.IsSelected = true;
		}

		private void ChangeImage ()
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
			{
				Filter = "PNG, GIF (*.png,*.gif)|*.png;*.gif|PNG (*.png)|*.png|GIF (*.gif)|*.gif"
			};
			if (openFileDialog.ShowDialog () == true)
			{
				if (IsWallpaperDefinition ())
				{
					if (openFileDialog.FileName != ImagePath.Text)
					{
						Model.WallpaperOriginal = Drawing.Image.FromFile (openFileDialog.FileName);
						SetOpacityWallpaper ();
					}
				} else
				{
					Model.ChangeSticker(Drawing.Image.FromFile (openFileDialog.FileName));
				}

				ImagePath.Text = openFileDialog.FileName;
			}
		}

		private bool IsWallpaperDefinition ()
		{
			return ShadowNames.imageNames [0] == Definitions.SelectedItem.ToString ();
		}

		private void ChangeColor (bool isBackground)
		{
			if (Definitions.SelectedItem != null)
			{
				string definition = Definitions.SelectedItem.ToString ();
				ThemeField field = CentralAPI.Current.currentTheme.Fields [definition];
				string hex = isBackground ? field.Background : field.Foreground;
				Drawing.Color color = Drawing.ColorTranslator.FromHtml (hex);

				Drawing.Color selectColor = _presenter.SelectColor(color, out bool save);

				if (save)
				{
					bool changed = color != selectColor;
					if (!CentralAPI.Current.isEdited) CentralAPI.Current.isEdited = changed;

					if (isBackground)
					{
						field.Background = selectColor.ToHex ();
						BGButton.Background = new SolidColorBrush (selectColor.ToWPFColor ());
					} else
					{
						field.Foreground = selectColor.ToHex ();
						FGButton.Background = new SolidColorBrush (selectColor.ToWPFColor ());
					}

					if (changed)
					{
						Model.UpdateSyntaxColors();
						UpdateColors ();
					}
				}
			}
		}

		private void UpdateColors ()
		{
			WPFHelper.ConvertGUIColorsNBrushes ();

			Background = WPFHelper.bgBrush;
			Foreground = WPFHelper.fgBrush;
			Window.Tag = WPFHelper.GenerateTag;

			if (OnColorUpdate != null)
				OnColorUpdate (ColorKeeper.bgColor, ColorKeeper.fgColor, ColorKeeper.bgClick);
		}

		private void bgImagePaint (object sender, PaintEventArgs e)
		{
			if (Model.CanDrawWallpaper())
			{
				Model.CalculateWallpaperSize(Fstb.box.ClientRectangle);
				
				e.Graphics.DrawImage (Model.WallpaperRender, Model.CalculatedWallpaperSize);
			}
		}

		private void ToggleEditor ()
		{
			Visibility visibility = Settings.Editor ? Visibility.Visible : Visibility.Collapsed;

			if (Definitions.Visibility != visibility)
			{
				Definitions.Visibility = DefSplitter.Visibility = visibility;

				if (Settings.Editor)
				{
					Grid.SetColumn (EditorSide, 2);
					Grid.SetColumnSpan (EditorSide, 1);

					EditorButtons1.Visibility = EditorButtons2.Visibility = EditorPanels.Visibility = Visibility.Visible;
				} else
				{
					Grid.SetColumn (EditorSide, 0);
					Grid.SetColumnSpan (EditorSide, 3);

					EditorButtons1.Visibility = EditorButtons2.Visibility = EditorPanels.Visibility = Visibility.Collapsed;
				}
			}
		}


		private void ChangeOpacityBySlider ()
		{
			CentralAPI.Current.currentTheme.WallpaperOpacity = OpacitySlider.Value.ToInt ();
			SetOpacityWallpaper ();
		}

		private void ApplySettingsChanges (ChangedSettings settings)
		{
			ToggleEditor ();
			if (settings.CurrentMode != Settings.settingMode)
			{
				Restore ();
			}

			if (settings.ShowSticker != Settings.swSticker)
			{
				Model.UpdateStickerVisibility ();
			}

			if (settings.CustomSticker != Settings.useCustomSticker)
			{
				if (Settings.useCustomSticker)
				{
					Model.ReloadSticker ();
				} else
				{
					Model.LoadSticker ();
				}
			}

			if (settings.ResetMargins)
			{
				Settings.database.UpdateData (SettingsConst.STICKER_POSITION, "");
				Model.ReloadStickerPositionData ();
				Model.ResetStickerPosition ();
			}

			if (settings.DimensionCap != Settings.useDimensionCap || Settings.dimensionCapMax != settings.DimensionCapMax)
			{
				Model.ResetStickerPosition ();
			}
		}

		#endregion


		#region Control Events

		private void Theme_Changed (object sender, SelectionChangedEventArgs e)
		{
			if (!Model.BlockedThemeSelector)
			{
				CentralAPI.Current.SelectTheme (Themes.SelectedItem.ToString ());

				if (CentralAPI.Current.isEdited) // Ask to save the changes
				{
					if (_presenter.SaveInExport (CentralAPI.Current.Translate ("main.theme.edited.full"),
							CentralAPI.Current.Translate ("main.theme.edited.short")))
						Save ();
				}

				Restore ();
				BoldCheckBox.IsEnabled = ItalicCheckBox.IsEnabled = ImagePanel.IsEnabled = !CentralAPI.Current.IsDefault ();
				LoadDefinitionsWithSelection ();

				SelectField ();
				CentralAPI.Current.selectedItem = Themes.SelectedItem.ToString ();
				Settings.database.UpdateData (SettingsConst.ACTIVE, CentralAPI.Current.selectedItem);
			}
		}

		private void Definitions_SelectionChanged (object sender, SelectionChangedEventArgs e)
		{
			SelectField ();
		}

		private void LAlignButton_OnClick (object sender, RoutedEventArgs e)
		{
			SetAlign (Alignment.Left);
		}

		private void CAlignButton_OnClick (object sender, RoutedEventArgs e)
		{
			SetAlign (Alignment.Center);
		}

		private void RAlignButton_OnClick (object sender, RoutedEventArgs e)
		{
			SetAlign (Alignment.Right);
		}

		private void MainWindow_OnSizeChanged (object sender, SizeChangedEventArgs e)
		{
			_presenter.ChangeTopPanelMargin(TopPanel);
		}

		private void AddButton_OnClick (object sender, RoutedEventArgs e)
		{
			AddTheme ();
		}

		private void RestoreButton_OnClick (object sender, RoutedEventArgs e)
		{
			Restore ();
		}

		private void ManageButton_OnClick (object sender, RoutedEventArgs e)
		{
			ManageThemes ();
		}

		private void SettingsButton_OnClick (object sender, RoutedEventArgs e)
		{
			OpenSettings ();
		}

		private void SaveButton_OnClick (object sender, RoutedEventArgs e)
		{
			Save ();
		}

		private void MainWindow_OnSourceInitialized (object sender, EventArgs e)
		{
			if (Helper.mode != ProductMode.Plugin)
				Settings.ConnectAndGet ();

			WindowStartupLocation = WindowStartupLocation.Manual;
			WindowProps props = Settings.database.ReadLocation ();

			Top = props.Top;
			Left = props.Left;
			Height = props.Height;
			Width = props.Width;
			if (props.Maximized)
			{
				WindowState = WindowState.Maximized;
			}
		}

		private void MainWindow_OnClosing (object sender, CancelEventArgs e)
		{
			Settings.database.SaveLocation (new WindowProps ()
			{
				Left = Left.ToInt (),
				Top = Top.ToInt (),
				Width = Width.ToInt (),
				Height = Height.ToInt (),
				Maximized = WindowState == WindowState.Maximized
			});
		}

		private void ImagePathButton_OnClick (object sender, RoutedEventArgs e)
		{
			ChangeImage ();
		}

		private void OpacitySlider_OnValueChanged (object sender, RoutedPropertyChangedEventArgs <double> e)
		{
			if (!_blockOpacity)
			{
				ChangeOpacityBySlider ();
			}
		}

		private void OpacitySlider_OnDragStarted (object sender, DragStartedEventArgs e)
		{
			if (sender is Slider)
				_blockOpacity = true;
		}

		private void OpacitySlider_OnDragCompleted (object sender, DragCompletedEventArgs e)
		{
			if (sender is Slider)
			{
				_blockOpacity = false;
				ChangeOpacityBySlider ();
			}
		}

		private void BackgroundButton_OnClick (object sender, RoutedEventArgs e)
		{
			ChangeColor (true);
		}

		private void ForegroundButton_OnClick (object sender, RoutedEventArgs e)
		{
			ChangeColor (false);
		}

		private void ExportButton_OnClick (object sender, RoutedEventArgs e)
		{
			Export ();
		}
		
		private void ImportButton_OnClick (object sender, RoutedEventArgs e)
		{
			ImportFile ();
		}

		private void ImportDirectoryButton_OnClick (object sender, RoutedEventArgs e)
		{
			ImportFolder ();
		}

		private void Close_OnClick (object sender, RoutedEventArgs e)
		{
			Close ();
		}
		
		private void DownloadButton_OnClick (object sender, RoutedEventArgs e)
		{
			OpenDownloader ();
		}
		
		#endregion


		private void InitAdditionalComponents ()
		{
			_popupController = new PopupController (this, this);
			AdditionalTools.ShowLicense (Tag, this);
			IsUpdated ();
			CheckUpdate ();
			InstallTracker ();
		}

		private void InstallTracker ()
		{
			if (AdditionalTools.ShouldTrack ())
			{
				InstallTrackerTimer = new Timer { Interval = 100 };
				InstallTrackerTimer.Tick += (_, __) =>
				{
					InstallTrackerTimer.Enabled = false;
					AdditionalTools.TrackInstall (this);
				};
				InstallTrackerTimer.Start ();
			}
		}


		private void CheckUpdate ()
		{
			if (Settings.update && Helper.mode != ProductMode.Plugin)
			{
				_popupController.CheckUpdate ();
				// _popupController.ShowNotification ("Test1", "Test1", null, null);
				// _popupController.ShowNotification ("Test2", "Test2", null, null);
			}
		}

		private void IsUpdated ()
		{
			int inst = Helper.RecognizeInstallationStatus ();
			if (inst == 1)
			{
				new ChangelogForm ().Show (this.ToNativeWindow ());
				Helper.DeleteInstallationStatus ();
			}
		}

		public event ColorUpdate OnColorUpdate;

		private void MainWindow_OnDrop (object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				if (files != null && files.Length > 0)
				{
					if (files.Length == 1)
					{
						CheckFile (files [0], true);
					}else
					{
						foreach (string file in files)
						{
							CheckFile(file, true);
						}
					}
				}
			}
		}

		private void CheckFile (string file, bool select)
		{
			if (File.Exists (file))
			{
				if (IsImage (file))
				{
					
				}else if (IsTheme (file))
				{
					CentralAPI.Current.ImportTheme (file, true, select, _presenter.ErrorExport, _presenter.AskChoiceParser, ImportUIAddition, ImportThemeReset);
				} else
				{
					CentralAPI.Current.ShowError (CentralAPI.Current.Translate ("main.dragndrop.format"), CentralAPI.Current.Translate ("main.sticker.select.invalid.short"));
				}
			}
		}

		private bool IsImage (string file)
		{
			string ext = Path.GetExtension (file).ToLower();
			return FileExtensions.ImageExtensions.Contains (ext);
		}

		private bool IsTheme (string file)
		{
			string ext = Path.GetExtension (file).ToLower();
			return FileExtensions.ThemeExtensions.Contains (ext);
		}

		public void SelectDefaultTheme ()
		{
			Themes.SelectedIndex = 0;
		}

		private void SwitchPluginMode()
		{
			PluginButtons.Visibility = Visibility.Visible;
			ExportButton.Visibility = Visibility.Hidden;
		}
		
		private void ChangeIcon()
		{
			Icon = Helper.GetYukiThemeIconImage(new Drawing.Size(24, 24)).ToWPFImage();
		}

		public Window getWindow ()
		{
			return this;
		}

	}
}