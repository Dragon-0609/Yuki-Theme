using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
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
		internal static Window  windowForDialogs;
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
			BackgroundClickBrush = bgClickBrush
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
			themeWindow.TName.Background = WPFHelper.bgdefBrush;
			themeWindow.TName.Foreground = WPFHelper.fgBrush;
			
			themeWindow.AddThemes (theme);

			bool? dialog = themeWindow.ShowDialog ();
			windowForDialogs = null;
			checkDialog = null;
			return new ThemeAddition (themeWindow.Themes.SelectedItem.ToString (), themeWindow.TName.Text, dialog, themeWindow.result);
		}

		public static SWMColor ToWPFColor (this      SDColor  color) => SWMColor.FromArgb (color.A, color.R, color.G, color.B);
		public static SDColor  ToWinformsColor (this SWMColor color) => SDColor.FromArgb (color.A, color.R, color.G, color.B);

		public static WBrush ToBrush (this SWMColor color) => new SolidColorBrush (color);

		public static void SetSVGImage (Button btn, string source, bool customColor = false, Drawing.Color color = default)
		{
			btn.Content = new Image ()
			{
				Source = (Helper.RenderSvg (new Drawing.Size (System.Convert.ToInt32 (btn.Width), System.Convert.ToInt32 (btn.Height)),
				                            Helper.LoadSvg (source, CLI.GetCore ()), false, Drawing.Size.Empty, customColor, color))
					.ToWPFImage ()
			};
		}

		public static Image GetSVGImage (string source, Size size, bool customColor = false, Drawing.Color color = default)
		{
			return new Image ()
			{
				Source = (Helper.RenderSvg (new Drawing.Size (System.Convert.ToInt32 (size.Width), System.Convert.ToInt32 (size.Height)),
				                            Helper.LoadSvg (source, CLI.GetCore ()), false, Drawing.Size.Empty,
				                            customColor, color))
					.ToWPFImage ()
			};
			
		}
		
		public static BitmapImage GetSvg (string source, Dictionary <string, Drawing.Color> idColor, Drawing.Size size, string nameSpace = "Yuki_Theme.Core.WPF.Resources.SVG")
		{
			return (Helper.RenderSvg ( size, Helper.LoadSvg (source, Assembly.GetExecutingAssembly (), nameSpace), idColor, true, Helper.bgBorder))
				.ToWPFImage ();
		}

		public static BitmapImage ToWPFImage (this Drawing.Image img)
		{
			BitmapImage bi = new BitmapImage ();
			using (var ms = new MemoryStream ())
			{
				img.Save (ms, System.Drawing.Imaging.ImageFormat.Png);
				// img.Dispose ();
				ms.Position = 0;

				bi.BeginInit ();
				bi.CacheOption = BitmapCacheOption.OnLoad;
				bi.StreamSource = ms;
				bi.EndInit ();
			}

			return bi;
		}

		public static int ToInt (this double d)
		{
			return Convert.ToInt32 (d);
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
}