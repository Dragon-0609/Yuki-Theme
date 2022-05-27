using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Yuki_Theme.Core.WPF.Controls;
using Yuki_Theme.Core.WPF.Windows;
using SDColor = System.Drawing.Color;
using SWMColor = System.Windows.Media.Color;
using Drawing = System.Drawing;
using WBrush = System.Windows.Media.Brush;
using Window = System.Windows.Window;

namespace Yuki_Theme.Core.WPF
{
	public static class WPFHelper
	{
		internal static Window      windowForDialogs;
		internal static Func <bool> checkDialog;

		#region Colors and Brushes

		public static SWMColor bgColor;
		public static SWMColor bgdefColor;
		public static SWMColor bgClickColor;
		public static SWMColor fgColor;
		public static SWMColor borderColor;
		public static SWMColor selectionColor;
		public static SWMColor keywordColor;
		public static Brush    bgBrush;
		public static Brush    bgdefBrush;
		public static Brush    bgClickBrush;
		public static Brush    fgBrush;
		public static Brush    borderBrush;
		public static Brush    selectionBrush;
		public static Brush    keywordBrush;

		#endregion


		public static StyleConfig GenerateTag => new StyleConfig
		{
			BorderColor = borderColor,
			SelectionColor = selectionColor,
			KeywordColor = keywordColor,
			BorderBrush = borderBrush,
			SelectionBrush = selectionBrush,
			KeywordBrush = keywordBrush,
			BackgroundClickColor = bgClickColor,
			BackgroundClickBrush = bgClickBrush,
			BackgroundDefaultColor = bgdefColor,
			BackgroundDefaultBrush = bgdefBrush
		};

		public static ThemeAddition AddTheme (Window owner, string theme = "")
		{
			AddThemeWindow themeWindow = new AddThemeWindow
			{
				Background = WPFHelper.bgBrush,
				Foreground = WPFHelper.fgBrush,
				Tag = owner.Tag,
				Owner = owner
			};

			themeWindow.AddThemes (theme);

			bool? dialog = themeWindow.ShowDialog ();
			windowForDialogs = null;
			checkDialog = null;
			return new ThemeAddition (themeWindow.Themes.SelectedItem.ToString (), themeWindow.TName.Text, dialog, themeWindow.result);
		}

		#region SVG

		public static void SetSVGImage (Button btn, string source, bool customColor = false, SDColor color = default)
		{
			btn.Content = new Image ()
			{
				Source = (Helper.RenderSvg (new Drawing.Size (Convert.ToInt32 (btn.Width), Convert.ToInt32 (btn.Height)),
				                            Helper.LoadSvg (source, CLI.GetCore ()), false, Drawing.Size.Empty, customColor, color))
					.ToWPFImage ()
			};
		}

		public static Image GetSVGImage (string source, Size size, bool customColor = false, SDColor color = default)
		{
			return new Image ()
			{
				Source = (Helper.RenderSvg (new Drawing.Size (Convert.ToInt32 (size.Width), Convert.ToInt32 (size.Height)),
				                            Helper.LoadSvg (source, CLI.GetCore ()), false, Drawing.Size.Empty,
				                            customColor, color))
					.ToWPFImage ()
			};
		}

		public static BitmapImage GetSvg (string source, Dictionary <string, SDColor> idColor, Drawing.Size size,
		                                  string nameSpace = "Yuki_Theme.Core.WPF.Resources.SVG")
		{
			return (Helper.RenderSvg (size, Helper.LoadSvg (source, Assembly.GetExecutingAssembly (), nameSpace), idColor, true,
			                          Helper.bgBorder))
				.ToWPFImage ();
		}

		public static BitmapImage GetSvg (string source, Dictionary <string, SDColor> idColor, bool withCustomColor, Drawing.Size size,
		                                  string nameSpace = "Yuki_Theme.Core.WPF.Resources.SVG", Assembly assm = null)
		{
			return (Helper.RenderSvg (size, Helper.LoadSvg (source, assm, nameSpace), idColor, withCustomColor, Helper.bgBorder))
				.ToWPFImage ();
		}


		public static void SetResourceSvg (this ResourceDictionary      dictionary, string       name, string source,
		                                   Dictionary <string, SDColor> idColor,    Drawing.Size size)
		{
			dictionary [name] = GetSvg (source, idColor, size);
		}

		#endregion

		public static string PutLineBreaks (string st)
		{
			return st.Replace ("\n", "&amp;#10;");
		}

		public static Dictionary <string, Drawing.Color> GenerateDisabledBGColors ()
		{
			Dictionary <string, Drawing.Color> disabledIdColors = new Dictionary <string, Drawing.Color> ()
				{ { "bg", Helper.DarkerOrLighter (Helper.bgColor, 0.2f) } };
			return disabledIdColors;
		}

		public static Dictionary <string, Drawing.Color> GenerateBGColors ()
		{
			Dictionary <string, Drawing.Color> idColors = new Dictionary <string, Drawing.Color> () { { "bg", Helper.bgColor } };
			return idColors;
		}

		[DllImport ("gdi32.dll", EntryPoint = "DeleteObject")]
		[return: MarshalAs (UnmanagedType.Bool)]
		public static extern bool DeleteObject ([In] IntPtr hObject);
	}

	public struct ThemeAddition
	{
		public string from;
		public string to;
		public bool?  save;
		public int    result;

		public ThemeAddition (string fr, string t, bool? sv, int res)
		{
			from = fr;
			to = t;
			save = sv;
			result = res;
		}
	}

	public class MouseWheelBehavior
	{
		public static double GetValue (Slider slider)
		{
			return (double)slider.GetValue (ValueProperty);
		}

		public static void SetValue (Slider slider, double value)
		{
			slider.SetValue (ValueProperty, value);
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.RegisterAttached (
				"Value",
				typeof (double),
				typeof (MouseWheelBehavior),
				new UIPropertyMetadata (0.0, OnValueChanged));

		public static Slider GetSlider (UIElement parentElement)
		{
			return (Slider)parentElement.GetValue (SliderProperty);
		}

		public static void SetSlider (UIElement parentElement, Slider value)
		{
			parentElement.SetValue (SliderProperty, value);
		}

		public static readonly DependencyProperty SliderProperty =
			DependencyProperty.RegisterAttached (
				"Slider",
				typeof (Slider),
				typeof (MouseWheelBehavior));


		private static void OnValueChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Slider slider = d as Slider;
			slider.Loaded += (ss, ee) =>
			{
				Window window = Window.GetWindow (slider);
				if (window != null)
				{
					SetSlider (window, slider);
					window.PreviewMouseWheel += Window_PreviewMouseWheel;
				}
			};
			slider.Unloaded += (ss, ee) =>
			{
				Window window = Window.GetWindow (slider);
				if (window != null)
				{
					SetSlider (window, null);
					window.PreviewMouseWheel -= Window_PreviewMouseWheel;
				}
			};
		}

		private static void Window_PreviewMouseWheel (object sender, MouseWheelEventArgs e)
		{
			Window window = sender as Window;
			Slider slider = GetSlider (window);
			double value = GetValue (slider);
			if (slider != null && value != 0)
			{
				slider.Value += slider.SmallChange * e.Delta / value;
			}
		}
	}
}