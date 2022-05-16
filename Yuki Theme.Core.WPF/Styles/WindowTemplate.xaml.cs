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
			bool can = true;
			if (WPFHelper.checkDialog != null)
				can = WPFHelper.checkDialog ();
			
			if (can)
				WPFHelper.windowForDialogs.DialogResult = true;
		}
		
	}
}