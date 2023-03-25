using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
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
                preview.DownloaderWindow = this;
                preview.Fstb.box.VerticalScroll.Enabled = false;

                preview.Fstb.box.Scroll += BoxOnScroll;
                preview.SetTheme(theme, _highlighter);
                Previews.Children.Add(preview);
            }
        }

        private void BoxOnScroll(object sender, ScrollEventArgs e)
        {
            Viewer.ScrollToVerticalOffset(Viewer.VerticalOffset + e.NewValue - e.OldValue);
        }

        public void StartDownloading(string name)
        {
        }
    }
}