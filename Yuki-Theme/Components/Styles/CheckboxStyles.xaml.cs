using System.Windows;
using YukiTheme.Tools;

namespace YukiTheme.Components.Styles;

public partial class CheckboxStyles : ResourceDictionary
{
	public CheckboxStyles()
	{
		InitializeComponent();
		LoadSVG();
	}

	private void LoadSVG()
	{
		this.SetResourceSvg("checkBoxDefault", "checkBox", false);
		this.SetResourceSvg("checkBoxDisabled", "checkBoxDisabled", true);
		this.SetResourceSvg("checkBoxFocused", "checkBoxFocused", false);
		this.SetResourceSvg("checkBoxSelected", "checkBoxSelected", false);
		this.SetResourceSvg("checkBoxSelectedDisabled", "checkBoxSelectedDisabled", true);
		this.SetResourceSvg("checkBoxSelectedFocused", "checkBoxSelectedFocused", false);
	}

	private void SaveButton_OnClick(object sender, RoutedEventArgs e)
	{
		/*bool can = true;
		if (WPFHelper.checkDialog != null)
			can = WPFHelper.checkDialog ();

		if (can)
			WPFHelper.windowForDialogs.DialogResult = true;*/
	}
}