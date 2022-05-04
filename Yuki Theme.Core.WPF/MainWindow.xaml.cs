using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.WPF.Controls;
using Brush = System.Windows.Media.Brush;
using Button = System.Windows.Controls.Button;
using Color = System.Windows.Media.Color;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Drawing = System.Drawing;
using Image = System.Windows.Controls.Image;

namespace Yuki_Theme.Core.WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
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

		public MainWindow ()
		{
			InitializeComponent ();
		}

		private void Init (object sender, RoutedEventArgs e)
		{
			if (Helper.mode != ProductMode.Plugin)
				Settings.connectAndGet ();

			Definitions.ItemContainerStyle = Definitions.FindResource ("Win10") as Style;

			CLI_Actions.ifHasImage = ifHasImage;
			CLI_Actions.ifDoesntHave = ifDoesntHave;
			CLI_Actions.ifHasSticker = ifHasSticker;
			CLI_Actions.ifDoesntHaveSticker = ifDoesntHaveSticker;

			highlighter = new Highlighter (Fstb.box);
			load_schemes ();
			highlighter.InitializeSyntax ();
			Fstb.box.Paint += bgImagePaint;
		}

		private void load_schemes ()
		{
			CLI.load_schemes ();
			Themes.Items.Clear ();
			foreach (string theme in CLI.schemes.ToArray ())
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

					if (prevSelectedField != -1)
						Definitions.SelectedIndex = prevSelectedField;
					else
						Definitions.SelectedIndex = 0;

					CLI.selectedItem = Themes.SelectedItem.ToString ();
					Settings.database.UpdateData (Settings.ACTIVE, CLI.selectedItem);
				}
			}
		}

		private void Restore ()
		{
			CLI.restore (false, null);
			highlighter.updateColors ();
			updateColors ();
			SetOpacityWallpaper ();
			LoadSticker ();
			LoadSVG ();
		}

		private void SelectField ()
		{
			ColorPanel.Visibility = Visibility.Collapsed;
			ImagePanel.Visibility = Visibility.Collapsed;
			if (IsColorField ())
			{
				ColorPanel.Visibility = Visibility.Visible;
				ItalicPanel.IsEnabled = BoldPanel.IsEnabled =
					Highlighter.isInNames (Definitions.SelectedItem.ToString ()) && !CLI.currentTheme.isDefault;
				
				ItalicPanel.Visibility = BoldPanel.Visibility =
					Highlighter.isInNames (Definitions.SelectedItem.ToString ()) ? Visibility.Visible : Visibility.Collapsed;
				
				ThemeField dic = CLI.currentTheme.Fields [Definitions.SelectedItem.ToString ()];
				
				if (dic.Foreground != null)
				{
					ColorPanel.Resources ["ForeBrush"] =
						new SolidColorBrush (Drawing.ColorTranslator.FromHtml (dic.Foreground).ToWPFColor ());
					FGColorPanel.Visibility = Visibility.Visible;
				} else
				{
					FGColorPanel.Visibility = Visibility.Collapsed;
				}

				if (dic.Background != null)
				{
					ColorPanel.Resources ["BackBrush"] =
						new SolidColorBrush (Drawing.ColorTranslator.FromHtml (dic.Background).ToWPFColor ());
					BGColorPanel.Visibility = Visibility.Visible;
				} else
				{
					BGColorPanel.Visibility = Visibility.Collapsed;
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
			}
		}

		private bool IsColorField ()
		{
			return !ShadowNames.imageNames.Contains (Definitions.SelectedItem.ToString ());
		}

		private void LoadSVG ()
		{
			string add = Helper.IsDark (Helper.bgColor) ? "" : "_dark";
			SetSVGImage (AddButton, "add" + add);
			SetSVGImage (ManageButton, "listFiles" + add);
			SetSVGImage (SaveButton, "menu-saveall");
			SetSVGImage (RestoreButton, "refresh" + add);
			SetSVGImage (ExportButton, "export" + add);
			SetSVGImage (ImportButton, "import" + add);
			SetSVGImage (ImportDirectoryButton, "traceInto" + add);
			SetSVGImage (SettingsButton, "gearPlain" + add, true, Helper.bgBorder);
			SetSVGImage (ImagePathButton, "moreHorizontal" + add);
			SetSVGImage (LAlignButton, "positionLeft" + add);
			SetSVGImage (CAlignButton, "positionCenter" + add);
			SetSVGImage (RAlignButton, "positionRight" + add);
		}

		private void SetSVGImage (Button btn, string source, bool customColor = false, Drawing.Color color = default)
		{
			btn.Content = new Image ()
			{
				Source = (Helper.RenderSvg (new Drawing.Size (System.Convert.ToInt32 (btn.Width), System.Convert.ToInt32 (btn.Height)),
				                            Helper.LoadSvg (source, CLI.GetCore ()), false, Drawing.Size.Empty, customColor, color))
					.ToWPFImage ()
			};
		}

		public void updateColors ()
		{
			Color bgColor = Helper.bgColor.ToWPFColor ();
			Color bgClickColor = Helper.bgClick.ToWPFColor ();
			Color fgColor = Helper.fgColor.ToWPFColor ();
			Color borderColor = Helper.bgBorder.ToWPFColor ();
			Color selectionColor = Helper.selectionColor.ToWPFColor ();
			Color KeywordColor = Helper.fgKeyword.ToWPFColor ();
			Brush bgBrush = new SolidColorBrush (bgColor);
			Brush bgClickBrush = new SolidColorBrush (bgClickColor);
			Brush fgBrush = new SolidColorBrush (fgColor);
			Brush borderBrush = new SolidColorBrush (borderColor);
			Brush selectionBrush = new SolidColorBrush (selectionColor);
			Brush KeywordBrush = new SolidColorBrush (KeywordColor);

			Background = bgBrush;
			Foreground = fgBrush;
			StyleConfig config = new StyleConfig
			{
				BorderColor = borderColor,
				SelectionColor = selectionColor,
				KeywordColor = KeywordColor,
				BorderBrush = borderBrush,
				SelectionBrush = selectionBrush,
				KeywordBrush = KeywordBrush,
				BackgroundClickBrush = bgClickBrush
			};
			Window.Tag = config;
			// MessageBox.Show (Themes.Tag.GetType ().ToString ());
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

		#endregion

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

		private void Definitions_SelectionChanged (object sender, SelectionChangedEventArgs e)
		{
			SelectField ();
		}
	}
}