using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.WPF.Controls;
using Application = System.Windows.Application;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
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

		private Drawing.Size defaultSize = new Drawing.Size (20, 20);
		
		#region Initialization

		public MainWindow ()
		{
			InitializeComponent ();
		}

		private void Init (object sender, RoutedEventArgs e)
		{
			if (Helper.mode != ProductMode.Plugin)
				Settings.connectAndGet ();

			CLI_Actions.ifHasImage = ifHasImage;
			CLI_Actions.ifDoesntHave = ifDoesntHave;
			CLI_Actions.ifHasSticker = ifHasSticker;
			CLI_Actions.ifDoesntHaveSticker = ifDoesntHaveSticker;
			CLI_Actions.SaveInExport = SaveInExport;
			CLI_Actions.showSuccess = FinishExport;
			CLI_Actions.showError = ErrorExport;
			CLI_Actions.hasProblem = hasProblem;
			
			highlighter = new Highlighter (Fstb.box);
			load_schemes ();
			highlighter.InitializeSyntax ();
			Fstb.box.Paint += bgImagePaint;
		}

		private void load_schemes ()
		{
			CLI.load_schemes ();
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
						if (CLI.currentTheme.StickerOpacity != 100)
							img4 = Helper.SetOpacity (img3, CLI.currentTheme.StickerOpacity);
						else
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
					Popup.Width = Sticker.Width = img4.Width;
					Popup.Height = Sticker.Height = img4.Height;
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
			WPFHelper.SetSVGImage (ImportDirectoryButton, "traceInto" + add);
			WPFHelper.SetSVGImage (SettingsButton, "gearPlain" + add, true, Helper.bgBorder);
			WPFHelper.SetSVGImage (ImagePathButton, "moreHorizontal" + add);
			WPFHelper.SetSVGImage (LAlignButton, "positionLeft" + add);
			WPFHelper.SetSVGImage (CAlignButton, "positionCenter" + add);
			WPFHelper.SetSVGImage (RAlignButton, "positionRight" + add);
		}

		private void ReLoadCheckBoxImages ()
		{
			if (Application.Current.MainWindow != null)
			{
				ResourceDictionary mergedDict = Application.Current.MainWindow.Resources.MergedDictionaries.FirstOrDefault (md => md.Source.ToString ().Contains ("CheckboxStyles.xaml"));
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
		}
		
		#endregion

		private void AddTheme ()
		{
			ThemeAddition res = WPFHelper.AddTheme (this, Themes.SelectedItem.ToString ());
			if (res.save != null && (bool)res.save)
			{
				if (res.result == 1)
				{
					List <string> customThemes = new List <string> (); 
					
					foreach (string item in themes)
					{
						if (!CLI.isDefaultTheme [item])
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
			
			bool? dialog = themesWindow.ShowDialog ();
			if (dialog != null && (bool)dialog)
			{
				// MessageBox.Show (string.Format("Saved: {0} -> {1}", themesWindow.Themes.SelectedItem.ToString (), themesWindow.TName.Text));
			} else
			{
				// MessageBox.Show ("Canceled");
			}
		}

		
		private void OpenSettings ()
		{
			SettingsWindow settingsWindow = new SettingsWindow
			{
				Background = WPFHelper.bgBrush,
				Foreground = WPFHelper.fgBrush,
				Tag = Tag,
				Owner = this
			};
			
			settingsWindow.ShowDialog ();
		}

		private void Restore ()
		{
			CLI.restore (false, null);
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

					if (ShadowNames.imageNames [0] == Definitions.SelectedItem.ToString ())
					{
						AlignPanel.Visibility = Visibility.Visible;
						ShowAlignSelection ();
					}
				}
			}
		}


		public void updateColors ()
		{
			WPFHelper.bgColor = Helper.bgColor.ToWPFColor ();
			WPFHelper.bgdefColor = Helper.bgdefColor.ToWPFColor ();
			WPFHelper.bgClickColor = Helper.bgClick.ToWPFColor ();
			WPFHelper.fgColor = Helper.fgColor.ToWPFColor ();
			WPFHelper.borderColor = Helper.bgBorder.ToWPFColor ();
			WPFHelper.selectionColor = Helper.selectionColor.ToWPFColor ();
			WPFHelper.keywordColor = Helper.fgKeyword.ToWPFColor ();
			WPFHelper.bgBrush = WPFHelper.bgColor.ToBrush ();
			WPFHelper.bgdefBrush = WPFHelper.bgdefColor.ToBrush ();
			WPFHelper.bgClickBrush = WPFHelper.bgClickColor.ToBrush ();
			WPFHelper.fgBrush = WPFHelper.fgColor.ToBrush ();
			WPFHelper.borderBrush = WPFHelper.borderColor.ToBrush ();
			WPFHelper.selectionBrush = WPFHelper.selectionColor.ToBrush ();
			WPFHelper.keywordBrush = WPFHelper.keywordColor.ToBrush ();

			Background = WPFHelper.bgBrush;
			Foreground = WPFHelper.fgBrush;
			StyleConfig config = WPFHelper.GenerateTag;
			Window.Tag = config;
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

		#endregion


		#region Control Events

		private void Theme_Changed (object sender, SelectionChangedEventArgs e)
		{
			if (!blockedThemeSelector)
			{
				bool cnd = CLI.SelectTheme (Themes.SelectedItem.ToString ());

				if (cnd)
				{
					// if (CLI.isEdited) // Ask to save the changes
					// {
					// 	if (SaveInExport (Translate ("main.theme.edited.full"), Translate ("main.theme.edited.short")))
					// 		save_Click (sender, e); // save before restoring
					// }
					int prevSelectedField = Definitions.SelectedIndex;
					Restore ();
					LoadDefinitions ();

					if (prevSelectedField != -1)
						Definitions.SelectedIndex = prevSelectedField;
					else
						Definitions.SelectedIndex = 0;

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
		#endregion

		private void SaveButton_OnClick (object sender, RoutedEventArgs e)
		{
			CustomStickerWindow customStickerWindow = new CustomStickerWindow
			{
				Background = WPFHelper.bgBrush,
				Foreground = WPFHelper.fgBrush,
				Tag = Tag,
				Owner = this
			};
			bool? dialog = customStickerWindow.ShowDialog ();
		}
	}
}