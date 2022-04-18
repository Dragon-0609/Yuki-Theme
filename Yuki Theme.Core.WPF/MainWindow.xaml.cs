using System.Windows;
using System.Windows.Controls;
using Yuki_Theme.Core.Themes;

namespace Yuki_Theme.Core.WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private bool blockedThemeSelector = true;

		public MainWindow ()
		{
			InitializeComponent ();
		}

		private void Init (object sender, RoutedEventArgs e)
		{
			if (Helper.mode != ProductMode.Plugin)
				Settings.connectAndGet ();
			load_schemes ();
		}

		private void load_schemes ()
		{
			CLI.load_schemes ();
			Themes.Items.Clear ();
			foreach (string theme in CLI.schemes.ToArray ())
			{
				Themes.Items.Add (theme);
			}

			blockedThemeSelector = false;
			// MessageBox.Show (CLI.isDefaultTheme.Count.ToString ());
			if (Themes.Items.Contains (CLI.selectedItem))
				Themes.SelectedItem = CLI.selectedItem;
			else
				Themes.SelectedIndex = 0;

			CLI.restore (false, null);
			foreach (string definition in CLI.names.ToArray ())
			{
				Definitions.Items.Add (definition);
			}
		}

		private void Theme_Changed (object sender, SelectionChangedEventArgs e)
		{
			if (!blockedThemeSelector)
			{
				bool cnd = CLI.SelectTheme (Themes.SelectedItem.ToString ());

				if (cnd)
				{
					// if (CLI.isEdited) // Ask to save the changes
					// {
					// 	if (SaveInExport (Translate ("main.theme.edited.full"), Translate ("main.theme.edited.short")))
					// 		save_Click (sender, e); // save before restoring
					// }

					restore ();
					CLI.selectedItem = Themes.SelectedItem.ToString ();
					Settings.database.UpdateData (Settings.ACTIVE, CLI.selectedItem);
				}
			}
		}

		private void restore ()
		{
			CLI.restore (false, null);
		}
	}
}