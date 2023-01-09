using System;
using System.Collections.Generic;
using System.Windows;
using FastColoredTextBoxNS;
using Yuki_Theme.Core.API;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.WPF.Controls;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class ThemeDownloaderWindow : Window
	{
		private HighlighterBase _highlighter;

		public ThemeDownloaderWindow()
		{
			InitializeComponent();
			_highlighter = new HighlighterBase();
			InitThemes();
		}

		private void InitThemes()
		{
			CentralAPI.Current.LoadSchemes();

			foreach (var scheme in CentralAPI.Current.Schemes)
			{
				Theme theme = CentralAPI.Current.GetTheme(scheme);

				ThemePreview preview = new ThemePreview();
				preview.Fstb.box.VerticalScroll.Enabled = false;
				preview.SetTheme(theme, _highlighter);
				Previews.Children.Add(preview);
			}
		}
	}
}