using System.Windows.Controls;

namespace YukiTheme.Components;

public class SilentTextBox : TextBox
{
	// if true, than the TextChanged event should not be thrown
	private bool Silent { get; set; }

	public string SilentText
	{
		set
		{
			try
			{
				Silent = true;
				Text = value;
			}
			finally
			{
				Silent = false;
			}
		}
	}

	protected override void OnTextChanged(TextChangedEventArgs e)
	{
		if (!Silent) base.OnTextChanged(e);
	}
}