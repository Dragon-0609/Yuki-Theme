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
			Dictionary <string, Drawing.Color> idColors = new Dictionary <string, Drawing.Color> () { {"bg", Helper.bgColor} };
			Dictionary <string, Drawing.Color> disabledIdColors = new Dictionary <string, Drawing.Color> ()
				{ { "bg", Helper.DarkerOrLighter (Helper.bgColor, 0.2f) } };

			SetResourceSvg ("checkBoxDefault", "checkBox", idColors);
			SetResourceSvg ("checkBoxDisabled", "checkBoxDisabled", disabledIdColors);
			SetResourceSvg ("checkBoxFocused", "checkBoxFocused", idColors);
			SetResourceSvg ("checkBoxSelected", "checkBoxSelected", idColors);
			SetResourceSvg ("checkBoxSelectedDisabled", "checkBoxSelectedDisabled", disabledIdColors);
			SetResourceSvg ("checkBoxSelectedFocused", "checkBoxSelectedFocused", idColors);
		}

		private void SetResourceSvg (string name, string source, Dictionary <string, Drawing.Color> idColor)
		{
			this [name] = WPFHelper.GetSvg (source, idColor, defaultSize);
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