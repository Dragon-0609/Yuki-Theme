using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using Yuki_Theme.Core.Forms;
using Yuki_Theme.Core.Parsers;
using Yuki_Theme.Core.WPF.Controls.ColorPicker;
using Yuki_Theme.Core.WPF.Interfaces;
using ComboBox = System.Windows.Controls.ComboBox;
using Drawing = System.Drawing;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using ListView = System.Windows.Controls.ListView;
using MessageBox = System.Windows.MessageBox;

namespace Yuki_Theme.Core.WPF.Windows
{
	internal class MainPresenter : Main.IPresenter
	{
		private Main.Model _model;
		private Main.IView _view;
		private Window     parent;
		
		private Thickness minMargin  = new Thickness (0);
		private Thickness normMargin = new Thickness (24, 0, 0, 0);

		public MainPresenter (Main.Model model, Main.IView view, Window parent)
		{
			_model = model;
			_view = view;
			this.parent = parent;
		}

		public void SetAPIActions ()
		{
			API_Events.ifHasImage = IfHasImage;
			API_Events.ifDoesntHave = IfDoesntHave;
			API_Events.ifHasSticker = IfHasSticker;
			API_Events.ifDoesntHaveSticker = IfDoesntHaveSticker;
			API_Events.AskChoice = AskActionChoice;
			API_Events.SaveInExport = SaveInExport;
			API_Events.showSuccess = FinishExport;
			API_Events.showError = ErrorExport;
			API_Events.hasProblem = HasProblem;
			API_Events.setPath = SetPath;
		}

		
		#region ApiEvents

		private void IfHasImage (Drawing.Image imgc)
		{
			_model.WallpaperOriginal = imgc;
			_model.CalculatedWallpaperSize = Drawing.Rectangle.Empty;
		}

		private void IfDoesntHave ()
		{
			_model.WallpaperRender = null;
			_model.WallpaperOriginal = null;
		}

		private void IfHasSticker (Drawing.Image imgc)
		{
			_model.Sticker = imgc;
		}

		private void IfDoesntHaveSticker ()
		{
			_model.Sticker = null;
		}

		private void HasProblem (string content)
		{
			MessageBox.Show (
				content, API_Base.Current.Translate ("messages.theme.invalid.short"), MessageBoxButton.OK, MessageBoxImage.Error);
			_view.SelectDefaultTheme ();
		}

		public bool SaveInExport (string content, string title)
		{
			return MessageBox.Show (content, title,
			                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
		}

		private void FinishExport (string content, string title)
		{
			MessageBox.Show (content, title);
		}

		public void ErrorExport (string content, string title)
		{
			MessageBox.Show (content, title, MessageBoxButton.OK, MessageBoxImage.Error);
		}

		public bool AskChoiceParser (string content, string title)
		{
			return MessageBox.Show (content,
			                        title,
			                        MessageBoxButton.YesNo) == MessageBoxResult.Yes;
		}
		
		private int AskActionChoice ()
		{
			return QuestionWindow.AskActionChoice (parent);
		}
		
		private void SetPath (string content, string title)
		{
			MessageBox.Show (content, title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			_view.OpenSettings ();
		}
		
		#endregion
		
		public void ChangeTopPanelMargin(WrapPanel topPanel)
		{
			if (parent.Width < 680)
			{
				if (topPanel.HorizontalAlignment != HorizontalAlignment.Center)
				{
					topPanel.HorizontalAlignment = HorizontalAlignment.Center;
					topPanel.Margin = minMargin;
				}
			} else
			{
				if (topPanel.HorizontalAlignment == HorizontalAlignment.Center)
				{
					topPanel.HorizontalAlignment = HorizontalAlignment.Left;
					topPanel.Margin = normMargin;
				}
			}
		}

		public void LoadThemesWithApi(ComboBox themeBox, ListView definitionsBox)
		{
			API_Base.Current.LoadSchemes ();
			LoadThemesToUi (themeBox, definitionsBox);
		}

		public void LoadThemesToUi(ComboBox themeBox, ListView definitionsBox)
		{
			_model.BlockedThemeSelector = true;
			themeBox.Items.Clear ();
			_model.Themes = API_Base.Current.Schemes.ToArray ();
			foreach (string theme in _model.Themes)
			{
				themeBox.Items.Add (theme);
			}

			_model.BlockedThemeSelector = false;
			// MessageBox.Show (API_Base.Current.isDefaultTheme.Count.ToString ());
			if (themeBox.Items.Contains (API_Base.Current.selectedItem))
				themeBox.SelectedItem = API_Base.Current.selectedItem;
			else
				themeBox.SelectedIndex = 0;

			API_Base.Current.Restore (false, null);
			LoadDefinitions (definitionsBox);
		}

		public void LoadDefinitions(ListView definitionsBox)
		{
			definitionsBox.Items.Clear ();
			foreach (string definition in API_Base.Current.names.ToArray ())
			{
				definitionsBox.Items.Add (definition);
			}
		}
		
		
		public void InsertTheme(ThemeAddition res, ComboBox themeBox)
		{
			if (res.result == 1)
			{
				List <string> customThemes = new List <string> ();

				foreach (string item in _model.Themes)
				{
					if (!API_Base.Current.ThemeInfos[item].isDefault)
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
					index2 = Array.IndexOf (_model.Themes, prevTheme) + 1;
				} else
				{
					int mx = customThemes.Count > 2 ? 1 : 0;
					string prevTheme = API_Base.Current.Schemes [API_Base.Current.Schemes.IndexOf (customThemes [mx]) - 1];
					index2 = Array.IndexOf (_model.Themes, prevTheme) + 1;
				}

				themeBox.Items.Insert (index2, res.to);
				themeBox.SelectedIndex = index2;
				_model.Themes = API_Base.Current.Schemes.ToArray ();
			}
		}

		public void ImportFile(Action<string> addToUiList = null, Action<string> selectAfterParse = null)
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
			{
				Filter = API_Base.Current.Translate ("main.import.extensions.all") +
				         " (*.icls,*.yukitheme,*.yuki,*.json,*.xshd)|*.icls;*.yukitheme;*.yuki;*.json;*.xshd|JetBrains IDE Scheme(*.icls)|*.icls|Yuki Theme(*.yukitheme,*.yuki)|*.yukitheme;*.yuki|Doki Theme(*.json)|*.json|Pascal syntax highlighting(*.xshd)|*.xshd"
			};
			if (openFileDialog.ShowDialog () == true)
			{
				ImportFile(openFileDialog.FileName, true, addToUiList, selectAfterParse);
			}
		}

		public void ImportFiles(string path, string extension, Action<string> addToUiList = null, Action<string> selectAfterParse = null)
		{
			string [] fls = Directory.GetFiles (path, extension, SearchOption.TopDirectoryOnly);
			foreach (string fl in fls)
			{
				ImportFile(fl, false, addToUiList, selectAfterParse);
			}
		}

		private void ImportFile(string fileName, bool select, Action<string> addToUiList = null, Action<string> selectAfterParse = null)
		{
			API_Base.Current.ImportTheme (fileName, true, select, ErrorExport, AskChoiceParser, addToUiList, selectAfterParse);
		}
		

		public Drawing.Color SelectColor(Drawing.Color defaultColor, out bool save)
		{
			Drawing.Color ncolor = Drawing.Color.Black;
			save = false;
			
			if (Settings.colorPicker == 0)
			{
				ColorPicker picker = new ColorPicker ();
				picker.allowSave = !API_Base.Current.currentTheme.isDefault;
				picker.MainColor = defaultColor;
				NativeWindow win32Parent = new NativeWindow ();
				win32Parent.AssignHandle (new WindowInteropHelper (parent).Handle);

				if (picker.ShowDialog (win32Parent) == System.Windows.Forms.DialogResult.OK)
				{
					save = true;
					ncolor = picker.MainColor;
				}
			} else
			{
				ColorPickerWindow picker = new ColorPickerWindow
				{
					allowSave = !API_Base.Current.currentTheme.isDefault,
					MainColor = defaultColor.ToWPFColor (),
					Owner = parent,
					Tag = parent.Tag
				};
				if (picker.ShowDialog () == true)
				{
					save = true;
					ncolor = picker.MainColor.ToWinformsColor ();
				}

				WPFHelper.checkDialog = null;
				WPFHelper.windowForDialogs = null;
			}
			return ncolor;
		}
	}
	
}