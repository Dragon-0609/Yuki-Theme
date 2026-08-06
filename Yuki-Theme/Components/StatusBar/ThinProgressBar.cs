using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace YukiTheme.Components.StatusBar;

public class ThinProgressBar : Control
{
	private int _value;
	private int _maximum = 100;
	private float _outlineWidth;

	public ThinProgressBar()
	{
		SetStyle(ControlStyles.AllPaintingInWmPaint
		         | ControlStyles.OptimizedDoubleBuffer
		         | ControlStyles.UserPaint
		         | ControlStyles.ResizeRedraw
		         | ControlStyles.SupportsTransparentBackColor, true);
		BackColor = Color.Transparent;
		Height = 6; // thin
		TrackColor = Color.FromArgb(60, ForeColor);
		FillColor = Color.FromArgb(0xE0, 0x6C, 0x6C); // salmon/red like the screenshot
		OutlineColor = Color.Transparent;
		OutlineWidth = 0f;
	}

	public Color TrackColor { get; set; }
	public Color FillColor { get; set; }
	public Color OutlineColor { get; set; }


	public float OutlineWidth
	{
		get => _outlineWidth;
		set
		{
			_outlineWidth = Math.Max(0f, value);
			Invalidate();
		}
	}

	public int Maximum
	{
		get => _maximum;
		set
		{
			_maximum = Math.Max(1, value);
			Invalidate();
		}
	}

	public int Value
	{
		get => _value;
		set
		{
			_value = Math.Min(_maximum, Math.Max(0, value));
			Invalidate();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		var g = e.Graphics;
		g.SmoothingMode = SmoothingMode.AntiAlias;

		int h = Math.Min(Height, 6);

		// inset by half the outline so the stroke stays inside the control bounds
		float inset = _outlineWidth / 2f;
		var track = RectangleF.FromLTRB(inset, (Height - h) / 2f + inset,
			Width - 1 - inset, (Height - h) / 2f + h - 1 - inset);
		if (track.Width <= 0 || track.Height <= 0) return;

		float radius = track.Height / 2f;

		// track fill
		using (var b = new SolidBrush(TrackColor))
		using (var path = Rounded(track, radius))
			g.FillPath(b, path);

		// value fill
		float ratio = _value / (float)_maximum;
		float fillW = track.Width * ratio;
		if (fillW > 0)
		{
			var fill = new RectangleF(track.X, track.Y, fillW, track.Height);
			using (var b = new SolidBrush(FillColor))
			using (var path = Rounded(fill, radius))
				g.FillPath(b, path);
		}

		// outline around the whole capsule
		if (_outlineWidth > 0 && OutlineColor.A > 0)
		{
			using var pen = new Pen(OutlineColor, _outlineWidth) { Alignment = PenAlignment.Center };
			using var path = Rounded(track, radius);
			g.DrawPath(pen, path);
		}
	}

	private static GraphicsPath Rounded(RectangleF r, float radius)
	{
		float d = radius * 2;
		var p = new GraphicsPath();
		if (d <= 0 || r.Width < d)
		{
			p.AddRectangle(r);
			return p;
		}

		p.AddArc(r.X, r.Y, d, d, 90, 180);
		p.AddArc(r.Right - d, r.Y, d, d, 270, 180);
		p.CloseFigure();
		return p;
	}
}