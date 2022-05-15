using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.WPF.Controls;
using MessageBox = System.Windows.Forms.MessageBox;

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
			ManageableItem custom = new ManageableItem (customGroup, customGroup, true);
			groups.Add (custom);
			groupItems.Add (customGroup, custom);

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

				snd.Content = new Image ()
				{
					Source = group.IsCollapsed ? Collapsed.Source : Expanded.Source
				}; // "Collapsed" : "Expanded"; //
				// MessageBox.Show (group.IsCollapsed ? "Collapsed" : "Expanded");
			}
		}
	}
}