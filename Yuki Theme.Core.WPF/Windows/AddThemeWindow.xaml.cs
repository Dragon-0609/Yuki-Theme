using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class AddThemeWindow : Window
	{
		public AddThemeWindow ()
		{
			InitializeComponent ();
			WPFHelper.windowForDialogs = this;
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