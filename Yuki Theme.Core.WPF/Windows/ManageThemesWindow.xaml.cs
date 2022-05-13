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
		private List <ManageableItem> groups = new List <ManageableItem> ();
		
		public ManageThemesWindow ()
		{
			InitializeComponent ();
			WPFHelper.windowForDialogs = this;
		}

		private void Initialize (object sender, RoutedEventArgs e)
		{
			Dictionary <string, ManageableItem> groupItems = new Dictionary <string, ManageableItem> ();
			
			foreach (string sc in DefaultThemes.categoriesList)
			{
				string nameTranslation = CLI.Translate (sc);
				ManageableItem defa = new ManageableItem (nameTranslation, true);
				groups.Add (defa);
				groupItems.Add (sc, defa);
				Schemes.Items.Add (defa);
			}

			string customgroup = CLI.Translate ("messages.theme.group.custom");
			ManageableItem custom = new ManageableItem (customgroup, true);
			groups.Add (custom);
			groupItems.Add (customgroup, custom);
			Schemes.Items.Add (customgroup);

			foreach (string item in CLI.schemes)
			{
				ManageableItem litem;
				if (CLI.isDefaultTheme [item])
				{
					ManageableItem cat = groupItems [DefaultThemes.getCategory (item)];
					litem = new ManageableItem (item, false, CLI.oldThemeList [item]);
					cat.children.Add (litem);
				} else
				{
					litem = new ManageableItem (item, false, CLI.oldThemeList [item]);
					custom.children.Add (litem);
				}
				Schemes.Items.Add (litem);
			}
			
		}
	}
}