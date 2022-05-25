using System.Windows;
using System.Windows.Controls;
using Brush = System.Windows.Media.Brush;

namespace Yuki_Theme.Core.WPF.Windows
{
	public partial class RenameThemeWindow : Window
	{
		public int result;
		
		public RenameThemeWindow ()
		{
			InitializeComponent ();
			WPFHelper.windowForDialogs = this;
			WPFHelper.checkDialog = TryToRename;
		}

		private void Initialization (object sender, RoutedEventArgs e)
		{
			ChangeDialogButtons ();
		}
		
		private void ChangeDialogButtons ()
		{
			Button saveBtn = (Button)Template.FindName ("SaveButton", this);
			saveBtn.Content = "Rename";
		}
		
		private bool TryToRename ()
		{
			bool canReturn = false;

			string from = FName.Text, to = TName.Text;
			
			if (from != to)
			{
				result = CLI.rename (from, to);

				canReturn = result != 0;
			} else
			{
				CLI_Actions.showError (CLI.Translate ("messages.name.equal.message"), CLI.Translate ("messages.name.equal.title"));
			}
			
			return canReturn;
		}
	}
}