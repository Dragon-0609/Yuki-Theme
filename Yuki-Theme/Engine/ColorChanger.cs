using System;
using System.Collections.Generic;
using System.Drawing;
using ICSharpCode.TextEditor.Document;

// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace YukiTheme.Engine;

public class ColorChanger
{
	internal const string BG = "bg";
	internal const string BG_DEF = "bgdef";
	internal const string CLICK_BG = "click_bg";
	internal const string CLICK_BG2 = "click_bg2";
	internal const string CLICK_BG3 = "click_bg3";
	internal const string SELECTION = "selection";
	internal const string BG_INACTIVE = "bg_inactive";
	internal const string BORDER = "border";
	internal const string BORDER_THICK = "border_thick";
	internal const string TYPE = "type";
	internal const string FOREGROUND = "foreground";
	internal const string FG_HOVER = "fg_hover";
	internal const string KEYWORD = "keyword";
	internal static ColorChanger Instance;

	private Dictionary<string, Brush> _brushes = new();

	private Dictionary<string, Color> _colors = new()
	{
		{ BG, Color.Black },
		{ BG_DEF, Color.Black },
		{ CLICK_BG, Color.Black },
		{ CLICK_BG2, Color.Black },
		{ CLICK_BG3, Color.Black },
		{ SELECTION, Color.Black },
		{ BG_INACTIVE, Color.Black },
		{ BORDER, Color.Black },
		{ BORDER_THICK, Color.Black },
		{ TYPE, Color.Black },
		{ FOREGROUND, Color.Black },
		{ FG_HOVER, Color.Black },
		{ KEYWORD, Color.Black }
	};

	private string[] _keys =
	{
		BG, BG_DEF, CLICK_BG, CLICK_BG2, CLICK_BG3, SELECTION, BG_INACTIVE, BORDER, BORDER_THICK, TYPE, FOREGROUND,
		FG_HOVER, KEYWORD
	};

	private Dictionary<string, Pen> _pens = new();

	public event Action<string, Color> Update = (key, color) =>
	{
	};

	internal event Action UpdatedColors = () =>
	{
	};

	public ColorChanger()
	{
		Instance = this;
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
#if LOG
        Console.WriteLine(Resources.ColorChanger_GetT_There_s_no__0_, typeof(T).Name.ToLower());
#endif
		return default;
	}

	internal void GetColors()
	{
		var highlighting = HighlightingManager.Manager.FindHighlighterForFile("A.pas");
		var bgdef = highlighting.GetColorFor("Default").BackgroundColor;
		var bg = DarkerOrLighter(bgdef, 0.05f);

		var bgClick = DarkerOrLighter(bgdef, 0.25f);
		var bgClick2 = DarkerOrLighter(bgdef, 0.4f);
		var bgClick3 = DarkerOrLighter(bgdef, 0.1f);
		var bgSelection = highlighting.GetColorFor("Selection").BackgroundColor;

		var bgInactive = ChangeColorBrightness(bgdef, -0.3f);
		var bgBorder = highlighting.GetColorFor("CaretMarker").Color;
		var bgType = highlighting.GetColorFor("EOLMarkers").Color;

		var defaultForeground = highlighting.GetColorFor("Default").Color;
		var clr = DarkerOrLighter(defaultForeground, 0.2f);
		var clrKey = highlighting.GetColorFor("Keywords").Color;

		_colors[BG] = bg;
		_colors[BG_DEF] = bgdef;
		_colors[CLICK_BG] = bgClick;
		_colors[CLICK_BG2] = bgClick2;
		_colors[CLICK_BG3] = bgClick3;
		_colors[SELECTION] = bgSelection;
		_colors[BG_INACTIVE] = bgInactive;
		_colors[BORDER] = bgBorder;
		_colors[BORDER_THICK] = bgBorder;
		_colors[TYPE] = bgType;
		_colors[FOREGROUND] = clr;
		_colors[FG_HOVER] = DarkerOrLighter(defaultForeground, 0.4f);
		_colors[KEYWORD] = clrKey;

		ResetBrush(BG_DEF);
		ResetBrush(BG);
		ResetBrush(SELECTION);
		ResetBrush(CLICK_BG);
		ResetBrush(CLICK_BG3);
		ResetBrush(BG_INACTIVE);
		ResetBrush(FOREGROUND);
		ResetBrush(TYPE);
		ResetBrush(BORDER);

		ResetPen(CLICK_BG3, 1);
		ResetPen(BORDER, 1);
		ResetPen(BORDER_THICK, 8);
		ResetPen(FOREGROUND, 1);
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

	internal void UpdateColors()
	{
		foreach (var key in _keys) Update(key, _colors[key]);

		UpdatedColors();

		YukiTheme_VisualPascalABCPlugin.StopTimer();
	}

	#region Color Management

	private static Color ChangeColorBrightness(Color color, float correctionFactor)
	{
		float red = color.R;
		float green = color.G;
		float blue = color.B;

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

	private static bool IsDark(Color clr)
	{
		var dark = (clr.R + clr.G + clr.B) / 3 < 127;
		return dark;
	}

	private static Color DarkerOrLighter(Color clr, float percent = 0)
	{
		if (IsDark(clr))
			return ChangeColorBrightness(clr, percent);
		return ChangeColorBrightness(clr, -percent);
	}

	#endregion
}