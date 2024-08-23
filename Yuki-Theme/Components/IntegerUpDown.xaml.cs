using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Yuki_Theme.Core.WPF.Controls;

public partial class IntegerUpDown : UserControl
{
	public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
		"Text", typeof(string), typeof(IntegerUpDown),
		new PropertyMetadata("", (d, e) =>
		{
			if (d is IntegerUpDown integerUpDown &&
			    integerUpDown.box != null)
				integerUpDown.box.Text =
					integerUpDown.GetValue(e.Property) as
						string ?? string.Empty;
		}, null));

	public static readonly DependencyProperty MinValueProperty =
		DependencyProperty.Register("MinValue", typeof(int), typeof(IntegerUpDown),
			new PropertyMetadata(0));

	public static readonly DependencyProperty MaxValueProperty =
		DependencyProperty.Register("MaxValue", typeof(int), typeof(IntegerUpDown),
			new PropertyMetadata(100));

	public static readonly DependencyProperty DefaultValueProperty =
		DependencyProperty.Register("DefaultValue", typeof(int), typeof(IntegerUpDown),
			new PropertyMetadata(0));

	public static readonly DependencyProperty ValueChangeProperty =
		DependencyProperty.Register("ValueChange", typeof(int), typeof(IntegerUpDown),
			new PropertyMetadata(1));

	public IntegerUpDown()
	{
		InitializeComponent();
	}


	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public int MinValue
	{
		get => (int)GetValue(MinValueProperty);
		set => SetValue(MinValueProperty, value);
	}

	public int MaxValue
	{
		get => (int)GetValue(MaxValueProperty);
		set => SetValue(MaxValueProperty, value);
	}

	public int DefaultValue
	{
		get => (int)GetValue(DefaultValueProperty);
		set => SetValue(DefaultValueProperty, value);
	}

	public int ValueChange
	{
		get => (int)GetValue(ValueChangeProperty);
		set => SetValue(ValueChangeProperty, value);
	}

	private void TextInputChecker(object sender, TextCompositionEventArgs e)
	{
		var regex = new Regex("[^0-9]+");
		e.Handled = regex.IsMatch(e.Text);
	}

	private void Box_OnKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Back && box.Text.Length == 0) box.Text = "0";
	}

	private void UpClick(object sender, RoutedEventArgs e)
	{
		int res;
		if (int.TryParse(box.Text, out res))
		{
			if (res + ValueChange <= MaxValue)
			{
				res += ValueChange;
				box.Text = res.ToString();
			}
		}
		else
		{
			box.Text = DefaultValue.ToString();
		}
	}

	private void DownClick(object sender, RoutedEventArgs e)
	{
		int res;
		if (int.TryParse(box.Text, out res))
		{
			if (res - ValueChange >= MinValue)
			{
				res -= ValueChange;
				box.Text = res.ToString();
			}
		}
		else
		{
			box.Text = DefaultValue.ToString();
		}
	}

	private void ValidateValue(object sender, RoutedEventArgs e)
	{
		int res;
		if (!int.TryParse(box.Text, out res))
			box.Text = DefaultValue.ToString();
		else
			ValidateCurrentNumber(res);
	}

	private void ValidateCurrentNumber(int res)
	{
		if (res > MaxValue)
			box.Text = MaxValue.ToString();
		else if (res < MinValue)
			box.Text = MinValue.ToString();
	}

	public int GetNumber()
	{
		var res = DefaultValue;
		if (!int.TryParse(box.Text, out res))
			box.Text = DefaultValue.ToString();
		else
			ValidateCurrentNumber(res);

		return res;
	}
}