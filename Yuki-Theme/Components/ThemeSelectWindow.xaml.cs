using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YukiTheme.Engine;
using YukiTheme.Tools;

namespace YukiTheme.Components
{
	public partial class ThemeSelectWindow : SnapWindow
	{
		private const string PLACEHOLDER_TEXT = "Search...";

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

		public event Action<string> SelectedTheme;

		public ThemeSelectWindow()
		{
			InitializeComponent();
			ThemeList.ItemsSource = _themes;
			GetColors();
			Search.SilentText = PLACEHOLDER_TEXT;
			Search.GotFocus += SearchOnGotFocus;
			Search.LostFocus += SearchOnLostFocus;
			SelectCurrentTheme();
		}

		private void SelectCurrentTheme()
		{
			string name = ThemeNameExtractor.Extract();
			IDEConsole.Log(name);
			if (ThemeList.Items.Contains(name))
			{
				ThemeList.SelectedItem = name;
			}
		}

		private void SearchOnLostFocus(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(Search.Text))
				Search.SilentText = PLACEHOLDER_TEXT;
		}

		private void SearchOnGotFocus(object sender, RoutedEventArgs e)
		{
			if (Search.Text == PLACEHOLDER_TEXT)
			{
				Search.SilentText = "";
			}
		}

		public void GetColors()
		{
			Foreground = WpfColorContainer.Instance.ForegroundBrush;
			ThemeList.Background = Search.Background = WpfColorContainer.Instance.BackgroundBrush;
			ThemeList.Foreground = Search.Foreground = Foreground;
			ThemeList.Tag = Search.Tag = Tag = WpfColorContainer.Instance;
			Border.BorderBrush = WpfColorContainer.Instance.BorderBrush;
		}

		private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
		{
			// return;
			if (ThemeList == null) return;
			if (string.IsNullOrWhiteSpace(Search.Text) || Search.Text == PLACEHOLDER_TEXT)
			{
				ThemeList.ItemsSource = _themes;
			}
			else
			{
				string text = Search.Text.ToLower();
				var found = _themes.Where(t => t.ToLower().Contains(text));
				if (found.Any())
				{
					ThemeList.ItemsSource = found;
				}
				else
				{
					ThemeList.ItemsSource = Array.Empty<string>();
				}
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
			// if (ThemeList.SelectedItem == null) return;
			SelectTheme(ThemeList.SelectedItem.ToString());
		}

		public void SelectTheme(string theme)
		{
#if LOG
            Console.WriteLine($"Selected: {theme}");
#endif
			OnSelectedTheme();
			Close();
		}

		private void OnSelectedTheme()
		{
			SelectedTheme?.Invoke(ThemeList.SelectedItem.ToString());
		}

		private void ThemeSelectWindow_OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			Close();
		}

		private void Grid_OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		private void Search_OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key is Key.Down)
			{
				if (ThemeList.SelectedIndex == -1)
				{
					ThemeList.SelectedIndex = 0;
				}
				else
				{
					ThemeList.SelectedIndex = Math.Min(ThemeList.SelectedIndex + 1, ThemeList.Items.Count - 1);
				}
			}

			if (e.Key is Key.Up)
			{
				if (ThemeList.SelectedIndex == -1)
				{
					ThemeList.SelectedIndex = 0;
				}
				else
				{
					ThemeList.SelectedIndex = Math.Max(ThemeList.SelectedIndex - 1, 0);
				}
			}

			if (e.Key is Key.Escape)
				Close();
		}

		private void ThemeList_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (ThemeList.SelectedItem != null)
				SelectTheme(ThemeList.SelectedItem.ToString());
		}
	}
}