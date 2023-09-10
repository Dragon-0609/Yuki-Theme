using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using YukiTheme.Engine;

namespace YukiTheme.Components
{
    public partial class ThemeSelectWindow : SnapWindow
    {
        private List<string> _themes = new()
        {
            "Azure Lane: Essex",
            "ReZero: Emily Dark",
            "ReZero: Emily Light",
            "ReZero: Ram",
            "ReZero: Rem",
            "SAO: Asuna Dark",
            "SAO: Asuna Light"
        };

        public event Action SelectedTheme;

        public ThemeSelectWindow()
        {
            InitializeComponent();
            ThemeList.ItemsSource = _themes;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            return;
            if (Search.Text.Length == 0) ThemeList.ItemsSource = _themes;
            else
            {
                string text = Search.Text.ToLower();
                ThemeList.ItemsSource = _themes.Where(t => t.ToLower().Contains(text));
            }
        }

        private void Search_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (ThemeList.SelectedItem == null) return;
            if (e.Key is Key.Enter or Key.Return)
            {
                SelectTheme(ThemeList.SelectedItem.ToString());
            }
        }

        private void ThemeList_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ThemeList.SelectedItem == null) return;
            SelectTheme(ThemeList.SelectedItem.ToString());
        }

        public void SelectTheme(string theme)
        {
#if LOG
            Console.WriteLine($"Selected: {theme}");
#endif
            OnSelectedTheme();
        }

        protected virtual void OnSelectedTheme()
        {
            SelectedTheme?.Invoke();
        }
    }
}