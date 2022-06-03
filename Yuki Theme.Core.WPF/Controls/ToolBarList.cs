using System.Windows;
using System.Windows.Controls;

namespace Yuki_Theme.Core.WPF.Controls
{
	public class ToolBarList : ListView
	{
		public ToolBarList ()
		{
			// DefaultStyleKeyProperty.OverrideMetadata (typeof (Manageable), new FrameworkPropertyMetadata (null));
		}
		
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ToolBarListItem();
		}
	}
}