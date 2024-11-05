using System;
using System.Windows;
using System.Windows.Media;

namespace YukiTheme.Tools;

public static class WpfConverter
{
	public static void InitAppForWinforms()
	{
		if (null == Application.Current)
		{
			Console.WriteLine("Application Inited");

			new Application
			{
				ShutdownMode = ShutdownMode.OnExplicitShutdown
			};
			var rd = new ResourceDictionary();

			var saoFont = new FontFamily(
				new Uri("pack://application:,,,/Fonts/"), "#SAO UI TT Regular"
			);
			rd.Add("SAOFont", saoFont);
			if (Application.Current != null) Application.Current.Resources = rd;
		}
	}

	public static void ConvertGuiColorsNBrushes()
	{
		WpfColorContainer.Instance.GetColorsAndBrushes();
	}
}