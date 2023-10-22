using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Svg;
using YukiTheme.Engine;

namespace YukiTheme.Tools;

public struct SvgRenderInfo
{
	public SvgDocument Svg;
	public bool Custom;
	public Size CSize;
	public bool CustomColor;
	public Color Clr;

	public SvgRenderInfo(SvgDocument svg)
	{
		Svg = svg;
		Custom = false;
		CSize = Size.Empty;
		CustomColor = false;
		Clr = Color.White;
	}
	
	public SvgRenderInfo(SvgDocument svg, bool custom, Size cSize, bool customColor, Color clr)
	{
		Svg = svg;
		Custom = custom;
		CSize = cSize;
		CustomColor = customColor;
		Clr = clr;
	}
}

public static class SvgRenderer
{
	private static Size Standart32 = new Size(32, 32);

	public static SvgDocument LoadSvg(string name, string nameSpace = "Icons.svg.")
	{
		var doc = new XmlDocument();
		if (!name.EndsWith(".svg")) name += ".svg";
		doc.Load(ResourceHelper.LoadStream(name, nameSpace));
		SvgDocument svg = SvgDocument.Open(doc);
		return svg;
	}

	public static void RenderSvg(Control im, SvgRenderInfo context)
	{
		im.BackgroundImage?.Dispose();

		im.BackgroundImage = RenderSvg(im.Size, context);
	}

	public static void RenderSvg(ToolStripButton im, SvgRenderInfo context)
	{
		im.Image?.Dispose();

		im.Image = RenderSvg(im.Size, context);
	}

	public static void RenderSvg(Form im, SvgRenderInfo context)
	{
		// im.Icon?.Dispose ();
		IntPtr ptr = ((Bitmap)RenderSvg(Standart32, context)).GetHicon();

		im.Icon = Icon.FromHandle(ptr);
		// DestroyIcon (ptr);
	}

	public static void RenderSvg(ToolStripMenuItem im, SvgRenderInfo context)
	{
		im.Image?.Dispose();

		im.Image = RenderSvg(im.Size, context);
	}

	public static Image RenderSvg(Size im, SvgRenderInfo context)
	{
		if (context.CustomColor)
			context.Svg.Color = new SvgColourServer(context.Clr);
		else
			context.Svg.Color = new SvgColourServer(ColorReference.ForegroundColor);

		if (!context.Custom)
			return context.Svg.Draw(im.Width, im.Height);
		else
			return context.Svg.Draw(context.CSize.Width, context.CSize.Height);
	}

	public static Image RenderSvg(Size im, SvgDocument svg, Dictionary<string, Color> idColors, bool customColor = false, Color clr = default)
	{
		if (customColor)
			svg.Color = new SvgColourServer(clr);
		else
			svg.Color = new SvgColourServer(ColorReference.ForegroundColor);

		if (idColors != null)
		{
			foreach (KeyValuePair<string, Color> idColor in idColors)
			{
				SvgElement element = svg.GetElementById(idColor.Key);
				element.Fill = new SvgColourServer(idColor.Value);
			}
		}

		return svg.Draw(im.Width, im.Height);
	}

	public static Image RenderSvg(Size im, SvgRenderInfo context, Dictionary<string, Color> rewrite)
	{
		return RenderSvg(im, context.Svg, rewrite, context.CustomColor, context.Clr);
	}

	private static string GetResourcePath(string name, string nameSpace)
	{
		return $"YukiTheme.Resources.{nameSpace}{name}";
	}
}