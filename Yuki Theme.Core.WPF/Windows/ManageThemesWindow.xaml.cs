using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Yuki_Theme.Core.Controls;
using Yuki_Theme.Core.Themes;
using Yuki_Theme.Core.WPF.Controls;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class ManageThemesWindow : Window
	{
		private List <ManageableItem>               groups = new List <ManageableItem> ();
		private Dictionary <string, ManageableItem> groupItems;

		public ManageThemesWindow ()
		{
			InitializeComponent ();
			WPFHelper.windowForDialogs = this;
		}

		private void Initialize (object sender, RoutedEventArgs e)
		{
			groupItems = new Dictionary <string, ManageableItem> ();

			foreach (string sc in DefaultThemes.categoriesList)
			{
				string nameTranslation = CLI.Translate (sc);
				ManageableItem defa = new ManageableItem (nameTranslation, sc, true);
				groups.Add (defa);
				groupItems.Add (sc, defa);
			}

			string customgroup = CLI.Translate ("messages.theme.group.custom");
			ManageableItem custom = new ManageableItem (customgroup, customgroup, true);
			groups.Add (custom);
			groupItems.Add (customgroup, custom);

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
		}

		private void Expander_OnClick (object sender, RoutedEventArgs e)
		{
			Button snd = (Button)sender;
			MessageBox.Show (snd.Tag.ToString ());
			ManageableItem group = groupItems [snd.Tag.ToString ()];
			if (group != null)
			{
				group.IsCollapsed = !group.IsCollapsed;
				group.UpdateCollapse (group.IsCollapsed);
				
				MessageBox.Show (group.IsCollapsed ? "Collapsed" : "Expanded");
			}
		}
	}
}