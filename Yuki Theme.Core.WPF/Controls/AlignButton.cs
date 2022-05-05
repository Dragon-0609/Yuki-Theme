using System.Windows;
using System.Windows.Controls;

namespace Yuki_Theme.Core.WPF.Controls
{
	public class AlignButton : Button
	{
		public static readonly DependencyProperty IsSelectedProperty;
		
		static AlignButton ()
		{
			PropertyMetadata isSelectedMetadata = new PropertyMetadata (false);
			IsSelectedProperty = DependencyProperty.Register ("IsSelected", typeof (bool), typeof (AlignButton), isSelectedMetadata);
		}

		public bool IsSelected
		{
			get { return (bool)GetValue (IsSelectedProperty); }
			set { SetValue (IsSelectedProperty, value); }
		}
	}
}