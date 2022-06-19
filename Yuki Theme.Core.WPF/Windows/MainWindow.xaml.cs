using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using Microsoft.WindowsAPICodePack.Dialogs;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Parsers;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.WPF.Controls;
using Yuki_Theme.Core.WPF.Controls.ColorPicker;
using Application = System.Windows.Application;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Drawing = System.Drawing;
using MessageBox = System.Windows.MessageBox;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class MainWindow
	{
		private bool blockedThemeSelector = true;

		private Highlighter       highlighter;
		private Drawing.Rectangle oldV = Drawing.Rectangle.Empty;

		private Thickness minMargin  = new Thickness (0);
		private Thickness normMargin = new Thickness (24, 0, 0, 0);

		private Drawing.Image img  = null;
		private Drawing.Image img2 = null;
		private Drawing.Image img3 = null;
		private Drawing.Image img4 = null;
		private string []     themes;

		public event SetTheme setTheme;
		public event SetTheme startSettingTheme;

		private Drawing.Size defaultSize = new Drawing.Size (20, 20);

		private bool BlockOpacity = false;

		#region Initialization

		public MainWindow ()
		{
			InitializeComponent ();
		}

		private void Init (object sender, RoutedEventArgs e)
		{
			Icon = Helper.GetYukiThemeIconImage (new Drawing.Size (24, 24)).ToWPFImage ();
			CLI_Actions.ifHasImage = ifHasImage;
			CLI_Actions.ifDoesntHave = ifDoesntHave;
			CLI_Actions.ifHasSticker = ifHasSticker;
			CLI_Actions.ifDoesntHaveSticker = ifDoesntHaveSticker;
			CLI_Actions.SaveInExport = SaveInExport;
			CLI_Actions.showSuccess = FinishExport;
			CLI_Actions.showError = ErrorExport;
			CLI_Actions.hasProblem = hasProblem;

			if (Helper.mode == null)
				Helper.mode = ProductMode.Program;
			else if (Helper.mode == ProductMode.Plugin)
			{
				PluginButtons.Visibility = Visibility.Visible;
				ExportButton.Visibility = Visibility.Hidden;
			}

			highlighter = new Highlighter (Fstb.box);
			load_schemes ();
			highlighter.InitializeSyntax ();
			Fstb.box.Paint += bgImagePaint;
			ToggleEditor ();
			ShowLicense ();
			UpdateTranslations ();
		}

		private void load_schemes ()
		{
			CLI.load_schemes ();
			LoadThemes ();
		}

		public void LoadThemes ()
		{
			blockedThemeSelector = true;
			Themes.Items.Clear ();
			themes = CLI.schemes.ToArray ();
			foreach (string theme in themes)
			{
				Themes.Items.Add (theme);
			}

			blockedThemeSelector = false;
			// MessageBox.Show (CLI.isDefaultTheme.Count.ToString ());
			if (Themes.Items.Contains (CLI.selectedItem))
				Themes.SelectedItem = CLI.selectedItem;
			else
				Themes.SelectedIndex = 0;

			CLI.restore (false, null);
			LoadDefinitions ();
		}

		private void LoadDefinitions ()
		{
			Definitions.Items.Clear ();
			foreach (string definition in CLI.names.ToArray ())
			{
				Definitions.Items.Add (definition);
			}
		}

		private void LoadDefinitionsWithSelection ()
		{
			int prevSelectedField = Definitions.SelectedIndex;
			LoadDefinitions ();
			Definitions.SelectedIndex = prevSelectedField != -1 ? prevSelectedField : 0;
		}

		private void LoadSticker ()
		{
			if (Settings.swSticker)
			{
				if (Settings.useCustomSticker && Settings.customSticker.Exist ())
				{
					img4 = Drawing.Image.FromFile (Settings.customSticker);
				} else
				{
					if (img3 != null)
					{
						img4 = img3;
						Sticker.Visibility = Visibility.Visible;
					} else
					{
						img4 = null;
						Sticker.Visibility = Visibility.Hidden;
					}
				}

				if (img4 != null)
				{
					SetStickerSize ();
					Sticker.Source = img4.ToWPFImage ();
					Popup.UpdatePopupPosition ();
				} else
				{
					Popup.Visibility = Visibility.Hidden;
				}
			} else
			{
				Popup.Visibility = Visibility.Hidden;
			}
		}

		private void LoadSVG ()
		{
			string add = Helper.IsDark (Helper.bgColor) ? "" : "_dark";
			WPFHelper.SetSVGImage (AddButton, "add" + add);
			WPFHelper.SetSVGImage (ManageButton, "listFiles" + add);
			WPFHelper.SetSVGImage (SaveButton, "menu-saveall");
			WPFHelper.SetSVGImage (RestoreButton, "refresh" + add);
			WPFHelper.SetSVGImage (ExportButton, "export" + add);
			WPFHelper.SetSVGImage (ImportButton, "import" + add);
			WPFHelper.SetSVGImage (ImportDirectoryButton, "traceInto" + add, true, Helper.bgBorder);
			WPFHelper.SetSVGImage (SettingsButton, "gearPlain" + add, true, Helper.bgBorder);
			WPFHelper.SetSVGImage (DownloaderButton, "smartStepInto" + add, true, Helper.bgBorder);
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
						mergedDict.SetResourceSvg ("checkBoxDefault", "checkBox", idColors, defaultSize);
						mergedDict.SetResourceSvg ("checkBoxDisabled", "checkBoxDisabled", disabledIdColors, defaultSize);
						mergedDict.SetResourceSvg ("checkBoxFocused", "checkBoxFocused", idColors, defaultSize);
						mergedDict.SetResourceSvg ("checkBoxSelected", "checkBoxSelected", idColors, defaultSize);
						mergedDict.SetResourceSvg ("checkBoxSelectedDisabled", "checkBoxSelectedDisabled", disabledIdColors, defaultSize);
						mergedDict.SetResourceSvg ("checkBoxSelectedFocused", "checkBoxSelectedFocused", idColors, defaultSize);
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
			if (res.result == 1)
			{
				List <string> customThemes = new List <string> ();

				foreach (string item in themes)
				{
					if (!CLI.ThemeInfos[item].isDefault)
					{
						customThemes.Add (item);
					}
				}

				customThemes.Add (res.to);
				customThemes.Sort ();
				int index = customThemes.IndexOf (res.to);
				int index2 = 0;

				if (index >= 1)
				{
					string prevTheme = customThemes [index - 1];
					index2 = Array.IndexOf (themes, prevTheme) + 1;
				} else
				{
					int mx = customThemes.Count > 2 ? 1 : 0;
					string prevTheme = CLI.schemes [CLI.schemes.IndexOf (customThemes [mx]) - 1];
					index2 = Array.IndexOf (themes, prevTheme) + 1;
				}

				Themes.Items.Insert (index2, res.to);
				Themes.SelectedIndex = index2;
				themes = CLI.schemes.ToArray ();
			}
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
			LoadThemes ();
		}


		private void OpenSettings ()
		{
			SettingsWindow settingsWindow = new SettingsWindow
			{
				Background = WPFHelper.bgBrush,
				Foreground = WPFHelper.fgBrush,
				Tag = Tag,
				Icon = Icon,
				Owner = this
			};
			string lang = Settings.localization;
			bool? dialog = settingsWindow.ShowDialog ();
			SettingMode currentMode = Settings.settingMode;
			if (dialog != null && (bool)dialog)
			{
				settingsWindow.utilities.SaveSettings ();
				ToggleEditor ();
				if (currentMode != Settings.settingMode)
				{
					Restore ();
				}

				
				if (settingsWindow.customSticker)
				{
					LoadSticker ();
				}else if (settingsWindow.dimensionCap)
				{
					SetStickerSize ();
				}
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
			CLI.restore (false, null);
			LoadDefinitionsWithSelection ();
			highlighter.updateColors ();
			updateColors ();
			SetOpacityWallpaper ();
			LoadSticker ();
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
						Highlighter.isInNames (Definitions.SelectedItem.ToString ()) && !CLI.currentTheme.isDefault;

					BoldCheckBox.Visibility = ItalicCheckBox.Visibility =
						Highlighter.isInNames (Definitions.SelectedItem.ToString ()) ? Visibility.Visible : Visibility.Collapsed;

					ThemeField dic = CLI.currentTheme.Fields [Definitions.SelectedItem.ToString ()];

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

					highlighter.activateColors (Definitions.SelectedItem.ToString ());
				} else
				{
					ImagePanel.Visibility = Visibility.Visible;
					AlignPanel.Visibility = Visibility.Collapsed;
					OpacityPanel.Visibility = Visibility.Collapsed;
					ImagePath.Text = "";
					BlockOpacity = true;
					OpacitySlider.Value = 0;
					if (IsWallpaperDefinition ())
					{
						AlignPanel.Visibility = Visibility.Visible;
						OpacityPanel.Visibility = Visibility.Visible;
						ShowAlignSelection ();

						if (CLI.currentTheme.HasWallpaper)
						{
							ImagePath.Text = "wallpaper.png";
							OpacitySlider.Value = CLI.currentTheme.WallpaperOpacity;
						}
					} else
					{
						if (CLI.currentTheme.HasSticker)
						{
							ImagePath.Text = "sticker.png";
						}
					}

					BlockOpacity = false;
				}
			}
		}

		private void Save ()
		{
			CLI.save (img2, img3);
		}

		private void Export ()
		{
			CLI.export (img2, img3, setThemeDelegate, startSettingThemeDelegate);
		}

		private void ImportFile ()
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
			{
				Filter = CLI.Translate ("main.import.extensions.all") +
				         " (*.icls,*.yukitheme,*.yuki,*.json,*.xshd)|*.icls;*.yukitheme;*.yuki;*.json;*.xshd|JetBrains IDE Scheme(*.icls)|*.icls|Yuki Theme(*.yukitheme,*.yuki)|*.yukitheme;*.yuki|Doki Theme(*.json)|*.json|Pascal syntax highlighting(*.xshd)|*.xshd"
			};
			if (openFileDialog.ShowDialog () == true)
			{
				MainParser.Parse (openFileDialog.FileName, true, true, ErrorExport, AskChoiceParser, ImportUIAddition, ImportThemeReset);
			}
		}

		private void ImportFolder ()
		{
			CommonOpenFileDialog co = new CommonOpenFileDialog ();
			co.IsFolderPicker = true;
			co.Multiselect = false;
			CommonFileDialogResult res = co.ShowDialog ();
			if (res == CommonFileDialogResult.Ok)
			{
				MessageBox.Show (CLI.Translate ("main.import.directory"));
				string [] fls = Directory.GetFiles (co.FileName, "*.json", SearchOption.TopDirectoryOnly);
				foreach (string fl in fls)
				{
					MainParser.Parse (fl, true, false, ErrorExport, AskChoiceParser, ImportUIAddition, ImportThemeReset);
				}

				fls = Directory.GetFiles (co.FileName, "*.icls", SearchOption.TopDirectoryOnly);
				foreach (string fl in fls)
				{
					MainParser.Parse (fl, true, false, ErrorExport, AskChoiceParser, ImportUIAddition, ImportThemeReset);
				}

				Themes.SelectedIndex = Themes.Items.Count - 1;
			}
		}

		private void OpenDownloader ()
		{
			ThemeDownloaderForm downloaderForm = new ThemeDownloaderForm ();
			downloaderForm.Show();
		}

		#endregion


		#region Core Actions

		public void ifHasImage (Drawing.Image imgc)
		{
			img2 = imgc;
			oldV = Drawing.Rectangle.Empty;
			// bgtext = "wallpaper.png";
		}

		public void ifDoesntHave ()
		{
			img = null;
			img2 = null;
		}

		public void ifHasSticker (Drawing.Image imgc)
		{
			img3 = imgc;
			// sttext = "sticker.png";
		}

		public void ifDoesntHaveSticker ()
		{
			img3 = null;
			img4 = null;
			// sttext = "";
		}

		public void hasProblem (string content)
		{
			MessageBox.Show (
				content, CLI.Translate ("messages.theme.invalid.short"), MessageBoxButton.OK, MessageBoxImage.Error);
			Themes.SelectedIndex = 0;
		}

		public bool SaveInExport (string content, string title)
		{
			return MessageBox.Show (content, title,
			                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
		}

		public void FinishExport (string content, string title)
		{
			MessageBox.Show (content, title);
		}

		public void ErrorExport (string content, string title)
		{
			MessageBox.Show (content, title, MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private bool AskChoiceParser (string content, string title)
		{
			return MessageBox.Show (content,
			                        title,
			                        MessageBoxButton.YesNo) == MessageBoxResult.Yes;
		}

		private void ImportUIAddition (string fileName)
		{
			Themes.Items.Add (fileName);
		}

		private void ImportThemeReset (string fileName)
		{
			if (Themes.SelectedItem.ToString () != fileName)
				Themes.SelectedItem = fileName;
			else
				Restore ();
		}

		#endregion


		#region Helper Methods

		private void SetOpacityWallpaper ()
		{
			if (img2 != null)
			{
				img = Helper.SetOpacity (img2, CLI.currentTheme.WallpaperOpacity);
			} else
			{
				img = null;
			}

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
			CLI.currentTheme.WallpaperAlign = (int)align;
			ShowAlignSelection ();
			Fstb.box.Refresh ();
		}

		private void ShowAlignSelection ()
		{
			LAlignButton.IsSelected = CAlignButton.IsSelected = RAlignButton.IsSelected = false;

			if (CLI.currentTheme.align == Alignment.Left)
				LAlignButton.IsSelected = true;
			else if (CLI.currentTheme.align == Alignment.Center)
				CAlignButton.IsSelected = true;
			else
				RAlignButton.IsSelected = true;
		}

		private void ChangeImage ()
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
			{
				Filter = "PNG (*.png)|*.png"
			};
			if (openFileDialog.ShowDialog () == true)
			{
				if (IsWallpaperDefinition ())
				{
					if (openFileDialog.FileName != ImagePath.Text)
					{
						img2 = Drawing.Image.FromFile (openFileDialog.FileName);
						SetOpacityWallpaper ();
					}
				} else
				{
					img3 = Drawing.Image.FromFile (openFileDialog.FileName);
					LoadSticker ();
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
				ThemeField field = CLI.currentTheme.Fields [definition];
				string hex = isBackground ? field.Background : field.Foreground;
				Drawing.Color color = Drawing.ColorTranslator.FromHtml (hex);
				Drawing.Color ncolor = Drawing.Color.Empty;
				bool save = false;
				if (Settings.colorPicker == 0)
				{
					ColorPicker picker = new ColorPicker ();
					picker.allowSave = !CLI.currentTheme.isDefault;
					picker.MainColor = color;
					NativeWindow win32Parent = new NativeWindow ();
					win32Parent.AssignHandle (new WindowInteropHelper (this).Handle);

					if (picker.ShowDialog (win32Parent) == System.Windows.Forms.DialogResult.OK)
					{
						save = true;
						ncolor = picker.MainColor;
					}
				} else
				{
					ColorPickerWindow picker = new ColorPickerWindow
					{
						allowSave = !CLI.currentTheme.isDefault,
						MainColor = color.ToWPFColor (),
						Owner = this,
						Tag = Tag
					};
					if (picker.ShowDialog () == true)
					{
						save = true;
						ncolor = picker.MainColor.ToWinformsColor ();
					}

					WPFHelper.checkDialog = null;
					WPFHelper.windowForDialogs = null;
				}

				if (save)
				{
					bool changed = color != ncolor;
					if (!CLI.isEdited) CLI.isEdited = changed;

					if (isBackground)
					{
						field.Background = ncolor.ToHex ();
						BGButton.Background = new SolidColorBrush (ncolor.ToWPFColor ());
					} else
					{
						field.Foreground = ncolor.ToHex ();
						FGButton.Background = new SolidColorBrush (ncolor.ToWPFColor ());
					}

					if (changed)
					{
						highlighter.updateColors ();
						updateColors ();
					}
				}
			}
		}


		public void startSettingThemeDelegate ()
		{
			if (startSettingTheme != null) startSettingTheme ();
		}

		public void setThemeDelegate ()
		{
			if (setTheme != null) setTheme ();
		}

		public void updateColors ()
		{
			WPFHelper.ConvertGUIColorsNBrushes ();

			Background = WPFHelper.bgBrush;
			Foreground = WPFHelper.fgBrush;
			Window.Tag = WPFHelper.GenerateTag;
		}

		private void bgImagePaint (object sender, PaintEventArgs e)
		{
			if (img != null && Settings.bgImage)
			{
				if (oldV.Width != Fstb.box.ClientRectangle.Width || oldV.Height != Fstb.box.ClientRectangle.Height)
				{
					oldV = Helper.GetSizes (img.Size, Fstb.box.ClientRectangle.Width, Fstb.box.ClientRectangle.Height,
					                        CLI.currentTheme.align);
				}

				e.Graphics.DrawImage (img, oldV);
			}
		}

		private void ToggleEditor ()
		{
			Visibility visibility;
			if (Settings.Editor)
			{
				visibility = Visibility.Visible;
			} else
			{
				visibility = Visibility.Collapsed;
			}

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
			CLI.currentTheme.WallpaperOpacity = OpacitySlider.Value.ToInt ();
			SetOpacityWallpaper ();
		}

		private void SetStickerSize ()
		{
			Drawing.Size dimensionSize = Helper.CalculateDimension (img4.Size);
			Popup.Width = Sticker.Width = dimensionSize.Width;
			Popup.Height = Sticker.Height = dimensionSize.Height;
		}
		
		#endregion


		#region Control Events

		private void Theme_Changed (object sender, SelectionChangedEventArgs e)
		{
			if (!blockedThemeSelector)
			{
				bool cnd = CLI.SelectTheme (Themes.SelectedItem.ToString ());

				if (cnd)
				{
					if (CLI.isEdited) // Ask to save the changes
					{
						if (SaveInExport (CLI.Translate ("main.theme.edited.full"), CLI.Translate ("main.theme.edited.short")))
							Save ();
					}
					Restore ();
					BoldCheckBox.IsEnabled = ItalicCheckBox.IsEnabled = ImagePanel.IsEnabled = !CLI.isDefault ();
					LoadDefinitionsWithSelection ();

					SelectField ();
					CLI.selectedItem = Themes.SelectedItem.ToString ();
					Settings.database.UpdateData (Settings.ACTIVE, CLI.selectedItem);
				}
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
			if (Width < 680)
			{
				if (TopPanel.HorizontalAlignment != HorizontalAlignment.Center)
				{
					TopPanel.HorizontalAlignment = HorizontalAlignment.Center;
					TopPanel.Margin = minMargin;
				}
			} else
			{
				if (TopPanel.HorizontalAlignment == HorizontalAlignment.Center)
				{
					TopPanel.HorizontalAlignment = HorizontalAlignment.Left;
					TopPanel.Margin = normMargin;
				}
			}
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
				Settings.connectAndGet ();

			WindowStartupLocation = WindowStartupLocation.Manual;
			WindowProps props = Settings.database.ReadLocation ();

			this.Top = props.Top;
			this.Left = props.Left;
			if (props.Height != null)
			{
				this.Height = (int)props.Height;
				if (props.Width != null)
				{
					this.Width = (int)props.Width;
					if (props.Maximized != null && (bool)props.Maximized)
					{
						WindowState = WindowState.Maximized;
					}
				}
			}
		}

		private void MainWindow_OnClosing (object sender, CancelEventArgs e)
		{
			Settings.database.SaveLocation (new WindowProps ()
			{
				Left = this.Left.ToInt (),
				Top = this.Top.ToInt (),
				Width = this.Width.ToInt (),
				Height = this.Height.ToInt (),
				Maximized = WindowState == WindowState.Maximized
			});
		}

		private void ImagePathButton_OnClick (object sender, RoutedEventArgs e)
		{
			ChangeImage ();
		}

		private void OpacitySlider_OnValueChanged (object sender, RoutedPropertyChangedEventArgs <double> e)
		{
			if (!BlockOpacity)
			{
				ChangeOpacityBySlider ();
			}
		}

		private void OpacitySlider_OnDragStarted (object sender, DragStartedEventArgs e)
		{
			if (sender is Slider)
				BlockOpacity = true;
		}

		private void OpacitySlider_OnDragCompleted (object sender, DragCompletedEventArgs e)
		{
			if (sender is Slider)
			{
				BlockOpacity = false;
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
			this.Close ();
		}
		
		private void DownloadButton_OnClick (object sender, RoutedEventArgs e)
		{
			OpenDownloader ();
		}
		
		#endregion

		private void ShowLicense ()
		{
			if (!Settings.license)
			{
				LicenseWindow licenseWindow = new LicenseWindow ()
				{
					Background = WPFHelper.bgBrush,
					Foreground = WPFHelper.fgBrush,
					BorderBrush = WPFHelper.borderBrush,
					Tag = Tag
				};
				licenseWindow.Owner = this;
				licenseWindow.Closed += (a, b) =>
				{
					GC.Collect ();
					GC.WaitForPendingFinalizers ();
				};

				licenseWindow.ShowDialog ();
				
				Settings.license = true;
				Settings.database.UpdateData (Settings.LICENSE, "True");
			}
		}
	}
}