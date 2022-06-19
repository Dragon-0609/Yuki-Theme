using System;
using System.Windows;
using System.Windows.Forms.Integration;
using Cyotek.Windows.Forms;
using Drawing = System.Drawing;
namespace Yuki_Theme.Core.WPF.Controls
{
	public class ScreenColorPickerHost : WindowsFormsHost
	{
		public ScreenColorPicker  box = new ScreenColorPicker ();
		
		public event EventHandler ColorChanged;

		public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register (
			"SelectedColor", typeof (Drawing.Color), typeof (ScreenColorPickerHost), new PropertyMetadata (Drawing.Color.Empty,
				OnChanged));

		private static void OnChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScreenColorPickerHost pickerHost = (ScreenColorPickerHost)d;
			EventHandler handler = pickerHost.ColorChanged;
			if (handler != null)
			{
				handler(pickerHost, EventArgs.Empty);
			}
		}

		public ScreenColorPickerHost ()
		{
			Child = box;
			box.Text = "Pick a Color";
			box.ColorChanged += InnerColorChanged;
		}

		private void InnerColorChanged (object sender, EventArgs e)
		{
			SelectedColor = box.Color;
		}

		public Drawing.Color SelectedColor
		{
			get { return (Drawing.Color)GetValue (SelectedColorProperty); }
			set { SetValue (SelectedColorProperty, value); }
		}
	}
}