using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using YukiTheme.Engine;
using Size = System.Drawing.Size;

namespace YukiTheme.Tools;

public static class ResourceDictionaryHelper
{
	public static void SetResourceSvg(this ResourceDictionary dictionary, string name, string source, bool disabled)
	{
		var size = new Size(20, 20);
		var ids = new Dictionary<string, Color>();
		if (!disabled)
			ids.Add("bg", ColorReference.BackgroundColor);
		else
			ids.Add("bg", ColorReference.BackgroundInactiveColor);

		dictionary[name] = GetSvg(source, ids, size, ColorReference.BorderColor);
	}

	public static void SetResourceSvg()
	{
	}

	private static BitmapImage GetSvg(string source, Dictionary<string, Color> idColor, Size size, Color color)
	{
		return SvgRenderer.RenderSvg(size, new SvgRenderInfo(SvgRenderer.LoadSvg(source, "Icons."), false, Size.Empty, true, color), idColor).ToWPFImage();
	}
}