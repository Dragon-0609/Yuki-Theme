using System;
using System.Windows;
using System.Windows.Media;
using YukiTheme.Engine;

namespace YukiTheme.Tools;

public static class WpfConverter
{
	public static void InitAppForWinforms()
	{
		if (null == Application.Current)
		{
			Console.WriteLine("Application Inited");

			new Application()
			{
				ShutdownMode = ShutdownMode.OnExplicitShutdown
			};
			ResourceDictionary rd = new ResourceDictionary();

			FontFamily saoFont = new FontFamily(
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