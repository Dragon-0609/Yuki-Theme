using System.Windows;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class AddThemeWindow : Window
	{
		public AddThemeWindow ()
		{
			InitializeComponent ();
		}

		public void AddThemes ()
		{
			Themes.Items.Clear ();
			foreach (string theme in CLI.schemes.ToArray ())
			{
				Themes.Items.Add (theme);
			}

			Themes.SelectedIndex = 0;
		}
	}
}