using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Yuki_Theme.Core.WPF.Controls;
using Yuki_Theme.Core.WPF.Windows;
using Image = System.Windows.Controls.Image;
using SDColor = System.Drawing.Color;
using Size = System.Windows.Size;
using SWMColor = System.Windows.Media.Color;
using WBrush = System.Windows.Media.Brush;

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
		public static WBrush    bgBrush;
		public static WBrush    bgdefBrush;
		public static WBrush    bgClickBrush;
		public static WBrush    fgBrush;
		public static WBrush    borderBrush;
		public static WBrush    selectionBrush;
		public static WBrush    keywordBrush;

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
				Background = bgBrush,
				Foreground = fgBrush,
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
			btn.Content = new Image
			{
				Source = (Helper.RenderSvg (new System.Drawing.Size (Convert.ToInt32 (btn.Width), Convert.ToInt32 (btn.Height)),
				                            Helper.LoadSvg (source, API.CentralAPI.Current.GetCore ()), false, System.Drawing.Size.Empty, customColor, color))
					.ToWPFImage ()
			};
		}

		public static Image GetSVGImage (string source, Size size, bool customColor = false, SDColor color = default)
		{
			return new Image
			{
				Source = (Helper.RenderSvg (new System.Drawing.Size (Convert.ToInt32 (size.Width), Convert.ToInt32 (size.Height)),
				                            Helper.LoadSvg (source, API.CentralAPI.Current.GetCore ()), false, System.Drawing.Size.Empty,
				                            customColor, color))
					.ToWPFImage ()
			};
		}

		public static BitmapImage GetSvg (string source, Dictionary <string, SDColor> idColor, System.Drawing.Size size, System.Drawing.Color customColor = default,
		                                  string nameSpace = "Yuki_Theme.Core.WPF.Resources.SVG")
		{
			
			return (Helper.RenderSvg (size, Helper.LoadSvg (source, Assembly.GetExecutingAssembly (), nameSpace), idColor, true,
			                          customColor))
				.ToWPFImage ();
		}

		public static BitmapImage GetSvg (string source,                                          Dictionary <string, SDColor> idColor, bool withCustomColor, System.Drawing.Size size,
		                                  string nameSpace = "Yuki_Theme.Core.WPF.Resources.SVG", Assembly                   assm = null)
		{
			return (Helper.RenderSvg (size, Helper.LoadSvg (source, assm, nameSpace), idColor, withCustomColor, Helper.bgBorder))
				.ToWPFImage ();
		}


		public static void SetResourceSvg (this ResourceDictionary      dictionary, string name, string source,
		                                   Dictionary <string, SDColor> idColor,    System.Drawing.Size size, System.Drawing.Color customColor = default)
		{
			if (customColor == default)
			{
				customColor = Helper.bgBorder;
			}
			dictionary [name] = GetSvg (source, idColor, size, customColor);
		}

		#endregion

		public static string PutLineBreaks (string st)
		{
			return st.Replace ("\n", "&amp;#10;");
		}

		public static Dictionary <string, SDColor> GenerateDisabledBGColors ()
		{
			Dictionary <string, SDColor> disabledIdColors = new Dictionary <string, SDColor> { { "bg", Helper.DarkerOrLighter (Helper.bgColor, 0.2f) } };
			return disabledIdColors;
		}

		public static Dictionary <string, SDColor> GenerateBGColors ()
		{
			Dictionary <string, SDColor> idColors = new Dictionary <string, SDColor> { { "bg", Helper.bgColor } };
			return idColors;
		}
		
		public static Dictionary <string, SDColor> GenerateRadioButtonColors ()
		{
			Dictionary <string, SDColor> idColors = new Dictionary <string, SDColor> { { "center", Helper.bgBorder } };
			return idColors;
		}

		
		
		[DllImport ("gdi32.dll", EntryPoint = "DeleteObject")]
		[return: MarshalAs (UnmanagedType.Bool)]
		public static extern bool DeleteObject ([In] IntPtr hObject);

		public static void ConvertGUIColorsNBrushes ()
		{
			bgColor = Helper.bgColor.ToWPFColor ();
			bgdefColor = Helper.bgdefColor.ToWPFColor ();
			bgClickColor = Helper.bgClick.ToWPFColor ();
			fgColor = Helper.fgColor.ToWPFColor ();
			borderColor = Helper.bgBorder.ToWPFColor ();
			selectionColor = Helper.selectionColor.ToWPFColor ();
			keywordColor = Helper.fgKeyword.ToWPFColor ();
			bgBrush = bgColor.ToBrush ();
			bgdefBrush = bgdefColor.ToBrush ();
			bgClickBrush = bgClickColor.ToBrush ();
			fgBrush = fgColor.ToBrush ();
			borderBrush = borderColor.ToBrush ();
			selectionBrush = selectionColor.ToBrush ();
			keywordBrush = keywordColor.ToBrush ();
		}

		public static void InitAppForWinforms ()
		{
			if (null == Application.Current)
			{
				Console.WriteLine ("Application Inited");

				new Application ()
				{
					ShutdownMode = ShutdownMode.OnExplicitShutdown
				};
				;
				ResourceDictionary rd = new ResourceDictionary ();

				System.Windows.Media.FontFamily saoFont = new System.Windows.Media.FontFamily (
					new Uri ("pack://application:,,,/Fonts/"), "#SAO UI TT Regular"
				);
				Console.WriteLine ("LINE: " + saoFont.LineSpacing);
				rd.Add ("SAOFont", saoFont);
				if (System.Windows.Application.Current != null) System.Windows.Application.Current.Resources = rd;
				Console.WriteLine ("Font Added");
			}
		}
		
		internal static void TranslateControls (Control that, params string[] startWith)
		{
			List <string> keys = new List <string> ();
			foreach (DictionaryEntry resource in that.Resources)
			{
				foreach (string starting in startWith)
				{
					if (resource.Key.ToString ().StartsWith (starting))
					{
						keys.Add (resource.Key.ToString ());
					}	
				}
			}

			foreach (string key in keys)
			{
				string translation = API.CentralAPI.Current.Translate (key);
				if (translation.Contains ("\n"))
					translation = translation.Replace ("\n", Environment.NewLine);
				that.Resources [key] = translation;
			}

		}
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
				slider.PreviewMouseWheel += Window_PreviewMouseWheel;
				/*Window window = Window.GetWindow (slider);
				if (window != null)
				{
					
					// SetSlider (window, slider);
					// window.PreviewMouseWheel += Window_PreviewMouseWheel;
				}*/
			};
			slider.Unloaded += (ss, ee) =>
			{
				slider.PreviewMouseWheel -= Window_PreviewMouseWheel;
				/*Window window = Window.GetWindow (slider);
				if (window != null)
				{
					SetSlider (window, null);
					window.PreviewMouseWheel -= Window_PreviewMouseWheel;
				}*/
			};
		}

		private static void Window_PreviewMouseWheel (object sender, MouseWheelEventArgs e)
		{
			// Window window = sender as Window;
			Slider slider = sender as Slider; /*GetSlider (window);*/
			double value = GetValue (slider);
			if (slider != null && value != 0)
			{
				slider.Value += slider.SmallChange * e.Delta / value;
			}
		}
	}
	
	
	public delegate void SetTheme ();
}