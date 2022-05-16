using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.WPF.Controls;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class ManageThemesWindow : Window
	{
		private List <ManageableItem>               groups = new List <ManageableItem> ();
		private Dictionary <string, ManageableItem> groupItems;

		private Image Expanded;
		private Image Collapsed;

		public ManageThemesWindow ()
		{
			InitializeComponent ();
			WPFHelper.windowForDialogs = this;
		}

		private void Initialize (object sender, RoutedEventArgs e)
		{
			ChangeDialogButtons ();
			LoadSVG ();

			groupItems = new Dictionary <string, ManageableItem> ();

			foreach (string sc in DefaultThemes.categoriesList)
			{
				string nameTranslation = CLI.Translate (sc);
				ManageableItem defa = new ManageableItem (nameTranslation, sc, true);
				groups.Add (defa);
				groupItems.Add (sc, defa);
			}

			string customGroup = CLI.Translate ("messages.theme.group.custom");
			ManageableItem custom = new ManageableItem (customGroup, "custom", true);
			groups.Add (custom);
			groupItems.Add ("custom", custom);

			foreach (string item in CLI.schemes)
			{
				if (CLI.isDefaultTheme [item])
				{
					ManageableItem cat = groupItems [DefaultThemes.getCategory (item)];
					new ManageableItem (item, item, false, CLI.oldThemeList [item], cat);
				} else
				{
					new ManageableItem (item, item, false, CLI.oldThemeList [item], custom);
				}
			}

			foreach (ManageableItem group in groups)
			{
				Schemes.Items.Add (group);
				foreach (ManageableItem child in group.children)
				{
					Schemes.Items.Add (child);
				}
			}

			Dispatcher.BeginInvoke(new Action(UpdateAllCollapseButtons), DispatcherPriority.Normal);
			
		}

		private void ChangeDialogButtons ()
		{
			Button saveBtn = (Button)Template.FindName ("SaveButton", this);
			saveBtn.Visibility = Visibility.Collapsed;
			Button closeBtn = (Button)Template.FindName ("Cancel", this);
			saveBtn.Content = "Close";
		}

		private void LoadSVG ()
		{
			string add = Helper.IsDark (Helper.bgColor) ? "" : "_dark";
			Size size = new Size (24, 24);
			Collapsed = WPFHelper.GetSVGImage ("findAndShowNextMatches" + add, size);
			Expanded = WPFHelper.GetSVGImage ("findAndShowPrevMatches" + add, size);
			WPFHelper.SetSVGImage (AddButton, "add" + add);
			WPFHelper.SetSVGImage (RemoveButton, "remove" + add);
			WPFHelper.SetSVGImage (RenameButton, "edit" + add);
		}

		private void UpdateAllCollapseButtons ()
		{
			foreach (ManageableItem item in groups)
			{
				Button btn = (Button)item.Template.FindName ("Expander", item);
				if (btn != null)
					btn.Content = new Image { Source = Expanded.Source };
			}
		}
		
		private void Expander_OnClick (object sender, RoutedEventArgs e)
		{
			Button snd = (Button)sender;
			ManageableItem group = groupItems [snd.Tag.ToString ()];
			if (group != null)
			{
				group.IsCollapsed = !group.IsCollapsed;
				group.UpdateCollapse ();

				snd.Content = new Image
				{
					Source = group.IsCollapsed ? Collapsed.Source : Expanded.Source
				}; // "Collapsed" : "Expanded"; //
				// MessageBox.Show (group.IsCollapsed ? "Collapsed" : "Expanded");
			}
		}

		private void AddButton_OnClick (object sender, RoutedEventArgs e)
		{
			ThemeAddition res = WPFHelper.AddTheme (this);
			if (res.save != null && (bool)res.save)
			{
				if (res.result == 1)
				{
					AddTheme (res);
				}
			}
		}

		private void RemoveButton_OnClick (object sender, RoutedEventArgs e)
		{
			if (Schemes.SelectedItem != null && Schemes.SelectedItem is ManageableItem item)
			{
				CLI.remove (item.Content.ToString (), askDelete, afterAsk, afterDelete);
			}
		}

		private void RenameButton_OnClick (object sender, RoutedEventArgs e)
		{
			if (Schemes.SelectedItem != null && Schemes.SelectedItem is ManageableItem item)
			{
				ThemeAddition result = ShowRenameDialog ();
				if (result.save != null && (bool)result.save)
				{
					RenameTheme (result);
				}
			}
		}

		private ThemeAddition ShowRenameDialog ()
		{
			RenameThemeWindow rename = new RenameThemeWindow ()
			{
				Tag = Tag,
				Owner = this
			};
			rename.SetColors (WPFHelper.bgBrush, WPFHelper.fgBrush);
			bool? dialog = rename.ShowDialog ();
			WPFHelper.windowForDialogs = null;
			WPFHelper.checkDialog = null;
			return new ThemeAddition (rename.FName.Text, rename.TName.Text, dialog, rename.result);
		}

		private void Schemes_OnSelectionChanged (object sender, SelectionChangedEventArgs e)
		{
			if (Schemes.SelectedItem != null && Schemes.SelectedItem is ManageableItem item && item.IsGroup)
			{
				RemoveButton.Visibility = RenameButton.Visibility = Visibility.Hidden;
			} else
			{
				RemoveButton.Visibility = RenameButton.Visibility = Visibility.Visible;
			}
		}

		#region Small Methods

		private void AddTheme (ThemeAddition res)
		{
			ManageableItem group = groupItems ["custom"];
			ManageableItem theme = new ManageableItem (res.to, res.to, false, true, group);
			int index = group.children.FindIndex (op => op.Content.ToString () == theme.Content.ToString ());
			ManageableItem prevTheme;
			prevTheme = index > 0 ? group.children [index - 1] : group;
			int indx = Schemes.Items.IndexOf (prevTheme);
			if (indx == -1)
			{
				MessageBox.Show ($"Index wasn't found. PrevIndx: {index}");
			} else
			{
				Schemes.Items.Insert (indx + 1, theme);
			}
		}

		private void RenameTheme (ThemeAddition res)
		{
			/*ManageableItem group = groupItems ["custom"];
			ManageableItem theme = new ManageableItem (res.to, res.to, false, true, group);
			int index = group.children.FindIndex (op => op.Content.ToString () == theme.Content.ToString ());
			ManageableItem prevTheme;
			prevTheme = index > 0 ? group.children [index - 1] : group;
			int indx = Schemes.Items.IndexOf (prevTheme);
			if (indx == -1)
			{
				MessageBox.Show ($"Index wasn't found. PrevIndx: {index}");
			} else
			{
				Schemes.Items.Insert (indx + 1, theme);
			}*/
		}

		public bool askDelete (string content, string title)
		{
			return MessageBox.Show (content, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
		}

		public object afterAsk (string sel)
		{
			ManageableItem sifr = (ManageableItem) Schemes.SelectedItem;
			/*if (form.selectedItem == sel || form.schemes.SelectedItem.ToString () == sel)
			{
				form.schemes.SelectedIndex = 0;
			}*/

			return sifr;
		}

		public void afterDelete (string sel, object sifr)
		{
			Schemes.Items.Remove ((ManageableItem)sifr);
			// form.schemes.Items.Remove (sel);
		}

		#endregion
	}
}