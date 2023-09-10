using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Drawing = System.Drawing; 

namespace Yuki_Theme.Core.WPF.Styles
{
	public partial class CheckboxStyles : ResourceDictionary
	{
		private Drawing.Size defaultSize = new Drawing.Size (20, 20);
		
		public CheckboxStyles ()
		{
			InitializeComponent ();
			LoadSVG ();
		}

		private void LoadSVG()
		{
			/*Dictionary <string, Drawing.Color> idColors = WPFHelper.GenerateBGColors ();
			var disabledIdColors = WPFHelper.GenerateDisabledBGColors ();

			this.SetResourceSvg ("checkBoxDefault", "checkBox", idColors, defaultSize);
			this.SetResourceSvg ("checkBoxDisabled", "checkBoxDisabled", disabledIdColors, defaultSize);
			this.SetResourceSvg ("checkBoxFocused", "checkBoxFocused", idColors, defaultSize);
			this.SetResourceSvg ("checkBoxSelected", "checkBoxSelected", idColors, defaultSize);
			this.SetResourceSvg ("checkBoxSelectedDisabled", "checkBoxSelectedDisabled", disabledIdColors, defaultSize);
			this.SetResourceSvg ("checkBoxSelectedFocused", "checkBoxSelectedFocused", idColors, defaultSize);*/
		}

		private void SaveButton_OnClick (object sender, RoutedEventArgs e)
		{
			/*bool can = true;
			if (WPFHelper.checkDialog != null)
				can = WPFHelper.checkDialog ();
			
			if (can)
				WPFHelper.windowForDialogs.DialogResult = true;*/
		}
	}
}