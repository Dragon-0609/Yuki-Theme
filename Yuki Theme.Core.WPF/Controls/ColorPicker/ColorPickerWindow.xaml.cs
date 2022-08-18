using System.Windows;
using System.Windows.Media;

namespace Yuki_Theme.Core.WPF.Controls.ColorPicker
{
	public partial class ColorPickerWindow : Window
	{
		public bool allowSave = true;
		
		public ColorPickerWindow ()
		{
			InitializeComponent ();
			WPFHelper.windowForDialogs = this;
			WPFHelper.checkDialog = CanReturn;
		}

		private bool CanReturn ()
		{
			if (!allowSave)
			{
				MessageBox.Show (this, API_Base.Current.Translate ("colors.default.error.full"),
				                 API_Base.Current.Translate ("colors.default.error.short"), MessageBoxButton.OK, MessageBoxImage.Warning);
			}
			
			return allowSave;
		}

		public Color MainColor
		{
			get
			{
				if (Picker.SelectedColor != null) return (Color)Picker.SelectedColor;
				return Colors.Black;
			}
			set => Picker.SelectedColor = value;
		}

		private void ColorPickerWindow_OnLoaded (object sender, RoutedEventArgs e)
		{
			Background = WPFHelper.bgBrush;
			Foreground = WPFHelper.fgBrush;
		}
	}
}