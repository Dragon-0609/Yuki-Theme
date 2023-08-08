using System;
using System.Collections.Generic;
using System.Drawing;
using ICSharpCode.TextEditor.Document;

namespace YukiTheme.Engine;

public class ColorChanger
{
	public static ColorChanger Changer;
	public const string Bg = "bg";
	public const string BgDef = "bgdef";
	public const string ClickBg = "click_bg";
	public const string ClickBg2 = "click_bg2";
	public const string ClickBg3 = "click_bg3";
	public const string Selection = "selection";
	public const string BgInactive = "bg_inactive";
	public const string Border = "border";
	public const string BorderThick = "border_thick";
	public const string Type = "type";
	public const string Foreground = "foreground";
	public const string FgHover = "fg_hover";
	public const string Keyword = "keyword";

	private Dictionary<string, Color> _colors = new()
	{
		{ Bg, Color.Black },
		{ BgDef, Color.Black },
		{ ClickBg, Color.Black },
		{ ClickBg2, Color.Black },
		{ ClickBg3, Color.Black },
		{ Selection, Color.Black },
		{ BgInactive, Color.Black },
		{ Border, Color.Black },
		{ BorderThick, Color.Black },
		{ Type, Color.Black },
		{ Foreground, Color.Black },
		{ FgHover, Color.Black },
		{ Keyword, Color.Black },
	};

	private Dictionary<string, Brush> _brushes = new()
	{
	};

	private Dictionary<string, Pen> _pens = new()
	{
	};

	private string[] keys = new[]
	{
		Bg, BgDef, ClickBg, ClickBg2, ClickBg3, Selection, BgInactive, Border, BorderThick, Type, Foreground, FgHover, Keyword
	};

	public Action<string, Color> Update = (key, color) => { };
	public Action UpdatedColors = () => { };

	public ColorChanger()
	{
		Changer = this;
	}

	public Color GetColor(string key)
	{
		return GetT(key, _colors);
	}

	public Brush GetBrush(string key)
	{
		return GetT(key, _brushes);
	}

	public Pen GetPen(string key)
	{
		return GetT(key, _pens);
	}

	private T GetT<T>(string key, Dictionary<string, T> dictionary)
	{
		if (dictionary.TryGetValue(key, out var res)) return res;
		Console.WriteLine($"There's no {typeof(T).Name.ToLower()}");
		return default;
	}

	public void GetColors()
	{
		IHighlightingStrategy highlighting = HighlightingManager.Manager.FindHighlighterForFile("A.pas");
		Color bgdef = highlighting.GetColorFor("Default").BackgroundColor;
		Color bg = DarkerOrLighter(bgdef, 0.05f);

		Color bgClick = DarkerOrLighter(bgdef, 0.25f);
		Color bgClick2 = DarkerOrLighter(bgdef, 0.4f);
		Color bgClick3 = DarkerOrLighter(bgdef, 0.1f);
		Color bgSelection = highlighting.GetColorFor("Selection").BackgroundColor;

		Color bgInactive = ChangeColorBrightness(bgdef, -0.3f);
		Color bgBorder = highlighting.GetColorFor("CaretMarker").Color;
		Color bgType = highlighting.GetColorFor("EOLMarkers").Color;

		Color defaultForeground = highlighting.GetColorFor("Default").Color;
		Color clr = DarkerOrLighter(defaultForeground, 0.2f);
		Color clrKey = highlighting.GetColorFor("Keywords").Color;

		_colors[Bg] = bg;
		_colors[BgDef] = bgdef;
		_colors[ClickBg] = bgClick;
		_colors[ClickBg2] = bgClick2;
		_colors[ClickBg3] = bgClick3;
		_colors[Selection] = bgSelection;
		_colors[BgInactive] = bgInactive;
		_colors[Border] = bgBorder;
		_colors[BorderThick] = bgBorder;
		_colors[Type] = bgType;
		_colors[Foreground] = clr;
		_colors[FgHover] = DarkerOrLighter(defaultForeground, 0.4f);
		_colors[Keyword] = clrKey;

		ResetBrush(BgDef);
		ResetBrush(Bg);
		ResetBrush(Selection);
		ResetBrush(ClickBg);
		ResetBrush(ClickBg3);
		ResetBrush(BgInactive);
		ResetBrush(Foreground);
		ResetBrush(Type);

		ResetPen(ClickBg3, 1);
		ResetPen(Border, 1);
		ResetPen(BorderThick, 8);
		ResetPen(Foreground, 1);
	}

	private void ResetBrush(string key)
	{
		if (_brushes.TryGetValue(key, out var brush))
		{
			if (brush != null) brush.Dispose();
			_brushes.Remove(key);
		}

		_brushes.Add(key, new SolidBrush(_colors[key]));
	}

	private void ResetPen(string key, int thickness)
	{
		if (_pens.TryGetValue(key, out var pen))
		{
			if (pen != null) pen.Dispose();
			_pens.Remove(key);
		}

		_pens.Add(key, new Pen(_colors[key], thickness));
	}

	public void UpdateColors()
	{
		foreach (string key in keys)
		{
			Update(key, _colors[key]);
		}

		UpdatedColors();

		YukiTheme_VisualPascalABCPlugin.StopTimer();
	}

	#region Color Management

	public static Color ChangeColorBrightness(Color color, float correctionFactor)
	{
		float red = (float)color.R;
		float green = (float)color.G;
		float blue = (float)color.B;

		if (correctionFactor < 0)
		{
			correctionFactor = 1 + correctionFactor;
			red *= correctionFactor;
			green *= correctionFactor;
			blue *= correctionFactor;
		}
		else
		{
			red = (255 - red) * correctionFactor + red;
			green = (255 - green) * correctionFactor + green;
			blue = (255 - blue) * correctionFactor + blue;
		}

		return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
	}

	public static bool IsDark(Color clr)
	{
		bool dark = ((clr.R + clr.G + clr.B) / 3 < 127);
		return dark;
	}

	public static Color DarkerOrLighter(Color clr, float percent = 0)
	{
		if (IsDark(clr))
			return ChangeColorBrightness(clr, percent);
		else
			return ChangeColorBrightness(clr, -percent);
	}

	#endregion
}