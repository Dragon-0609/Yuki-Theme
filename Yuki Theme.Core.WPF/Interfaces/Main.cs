using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FastColoredTextBoxNS;
using Yuki_Theme.Core.WPF.Windows;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;

namespace Yuki_Theme.Core.WPF.Interfaces
{
	public class Main
	{
		public abstract class Model
		{
			internal Image WallpaperRender;
			internal Image WallpaperOriginal;

			internal Image Sticker;

			internal Rectangle CalculatedWallpaperSize;
			
			public abstract event  SetTheme SetTheme;
			public abstract event  SetTheme StartSettingTheme;

			internal bool BlockedThemeSelector = true;

			internal abstract void InvokeSetTheme();
			internal abstract void InvokeStartSettingTheme();

			internal abstract void ChangeProductMode(ProductMode mode, Action ifPlugin);


			internal abstract void InitSticker(Window window);
			internal abstract void LoadSticker();
			internal abstract void ReloadSticker();
			internal abstract void ResetStickerPosition();
			internal abstract void ChangeSticker(Image image);

			internal abstract void CalculateWallpaperSize(Rectangle rectangle);

			internal abstract bool CanDrawWallpaper();

			internal abstract void ChangeWallpaperOpacity();

			internal string[] Themes;

			internal Highlighter   highlighter;

			internal abstract void InitHighlighter(FastColoredTextBox box);

			internal abstract void InitializeSyntax();
			internal abstract void UpdateSyntaxColors();
			internal abstract void ActivateSyntaxColors(string item);
		}

		public interface IView
		{
			void SelectDefaultTheme ();
			
			void OpenSettings ();

			Window getWindow ();
		}

		public interface IPresenter
		{
			void SetAPIActions ();


			void ErrorExport(string content, string title);
			bool AskChoiceParser(string content, string title);
			bool SaveInExport(string content, string title);

			void ChangeTopPanelMargin(WrapPanel topPanel);

			void LoadThemesWithApi(ComboBox themeBox, ListView definitionsBox);
			
			void LoadThemesToUi(ComboBox themeBox, ListView definitionsBox);

			void LoadDefinitions(ListView definitionsBox);

			void InsertTheme(ThemeAddition res, ComboBox themeBox);
			
			void ImportFile(Action<string> addToUiList = null, Action<string> selectAfterParse = null);
			void ImportFiles(string path, string extension, Action<string> addToUiList = null, Action<string> selectAfterParse = null);

			Color SelectColor(Color defaultColor, out bool save);

			void PrepareProgressWindow (string filename);
			
			int GetFilesCount(string path, string extension);

			bool SetImportsCancel (bool cancel);

			void ImportDirectoryDone ();
		}
	}
}