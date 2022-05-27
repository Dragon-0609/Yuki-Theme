using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Yuki_Theme.Core.WPF.Controls
{
	public partial class IntegerUpDown : UserControl
	{
		public IntegerUpDown ()
		{
			InitializeComponent ();
		}

		private void TextInputChecker (object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex ("[^0-9]+");
			e.Handled = regex.IsMatch (e.Text);
		}

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register (
			"Text", typeof (string), typeof (IntegerUpDown),
			new PropertyMetadata ("", new PropertyChangedCallback (
				                      (d, e) =>
				                      {
					                      if (d is IntegerUpDown integerUpDown &&
					                          integerUpDown.box != null)
					                      {
						                      integerUpDown.box.Text =
							                      integerUpDown.GetValue (e.Property) as
								                      string ?? string.Empty;
					                      }
				                      }), null));


		public string Text
		{
			get => (string)GetValue (TextProperty);
			set => SetValue (TextProperty, value);
		}

		public int MinValue
		{
			get => (int)GetValue (MinValueProperty);
			set => SetValue (MinValueProperty, value);
		}

		public static readonly DependencyProperty MinValueProperty =
			DependencyProperty.Register ("MinValue", typeof (int), typeof (IntegerUpDown),
			                             new PropertyMetadata (0));

		private void Box_OnKeyDown (object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Back && box.Text.Length == 0)
			{
				box.Text = "0";
			}
		}

		private void UpClick (object sender, RoutedEventArgs e)
		{
			int res;
			if (int.TryParse (box.Text, out res))
			{
				res++;
				box.Text = res.ToString ();
			} else
			{
				box.Text = "-1";
			}
		}

		private void DownClick (object sender, RoutedEventArgs e)
		{
			int res;
			if (int.TryParse (box.Text, out res))
			{
				if (res - 1 >= MinValue)
				{
					res--;
					box.Text = res.ToString ();
				}
			} else
			{
				box.Text = "-1";
			}
		}

		private void ValidateValue (object sender, RoutedEventArgs e)
		{
			int res;
			if (!int.TryParse (box.Text, out res))
			{
				box.Text = "-1";
			}
		}
	}
}