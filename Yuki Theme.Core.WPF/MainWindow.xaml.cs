using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml;
using Yuki_Theme.Core.WPF.Controls;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Image = System.Drawing.Image;

namespace Yuki_Theme.Core.WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private bool blockedThemeSelector = true;

		private Highlighter highlighter;
		private Rectangle   oldV = Rectangle.Empty;

		private Thickness minMargin  = new Thickness (0);
		private Thickness normMargin = new Thickness (24, 0, 0, 0);
		
		private Image img  = null;
		private Image img2 = null;
		private Image img3 = null;
		private Image img4 = null;

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
					img4 = Image.FromFile (Settings.customSticker);
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

				Popup.Width = Sticker.Width = img4.Width;
				Popup.Height = Sticker.Height = img4.Height;
				Sticker.Source = img4.ToWPFImage ();
				Popup.UpdatePopupPosition ();
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

					Restore ();
					CLI.selectedItem = Themes.SelectedItem.ToString ();
					Settings.database.UpdateData (Settings.ACTIVE, CLI.selectedItem);
				}
			}
		}

		private void Restore ()
		{
			CLI.restore (false, null);
			highlighter.updateColors ();
			updateBackgroundColors ();
			SetOpacityWallpaper ();
			LoadSticker ();
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

		public void ifHasImage (Image imgc)
		{
			img2 = imgc;
			oldV = Rectangle.Empty;
			// bgtext = "wallpaper.png";
		}

		public void ifDoesntHave ()
		{
			img = null;
			img2 = null;
		}

		public void ifHasSticker (Image imgc)
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
			img = Helper.SetOpacity (img2, CLI.currentTheme.WallpaperOpacity);
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

		public void updateBackgroundColors ()
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

		
	}
}