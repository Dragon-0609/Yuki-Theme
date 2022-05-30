using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Drawing = System.Drawing; 

namespace Yuki_Theme.Core.WPF.Styles
{
	public partial class RadioButtonStyles : ResourceDictionary
	{
		private Drawing.Size defaultSize = new Drawing.Size (16, 16);
		
		public RadioButtonStyles ()
		{
			InitializeComponent ();
			LoadSVG ();
		}

		private void LoadSVG()
		{
			Dictionary <string, Drawing.Color> idColors = WPFHelper.GenerateRadioButtonColors ();

			this.SetResourceSvg ("RadioButtonDefault", "radioButton", null, defaultSize, Helper.fgColor);
			this.SetResourceSvg ("RadioButtonFocused", "radioButtonFocused", null, defaultSize);
			this.SetResourceSvg ("RadioButtonSelected", "radioButtonSelected", idColors, defaultSize, Helper.fgColor);
			this.SetResourceSvg ("RadioButtonSelectedFocused", "radioButtonSelectedFocused", idColors, defaultSize);
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