using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Yuki_Theme.Core.WPF.Styles
{
	public partial class WindowTemplate : ResourceDictionary
	{
		public WindowTemplate ()
		{
			InitializeComponent ();
		}


		private void SaveButton_OnClick (object sender, RoutedEventArgs e)
		{
			WPFHelper.windowForDialogs.DialogResult = true;
		}
		
	}
}