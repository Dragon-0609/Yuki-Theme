using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using YukiTheme.Engine;

namespace YukiTheme.Style.Helpers;

internal abstract class InertButtonBase : Control
{
	private bool m_isMouseOver;

	protected InertButtonBase()
	{
		SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		BackColor = Color.Transparent;
	}

	public abstract Bitmap Image { get; }

	protected bool IsMouseOver
	{
		get => m_isMouseOver;
		private set
		{
			if (m_isMouseOver == value)
				return;

			m_isMouseOver = value;
			Invalidate();
		}
	}

	protected override Size DefaultSize => Resources.DockPane_Close.Size;

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);
		var over = ClientRectangle.Contains(e.X, e.Y);
		if (IsMouseOver != over)
			IsMouseOver = over;
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		base.OnMouseEnter(e);
		if (!IsMouseOver)
			IsMouseOver = true;
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
		if (IsMouseOver)
			IsMouseOver = false;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (IsMouseOver && Enabled) e.Graphics.DrawRectangle(ColorChanger.Instance.GetPen(ColorChanger.FOREGROUND), Rectangle.Inflate(ClientRectangle, -1, -1));

		using (var imageAttributes = new ImageAttributes())
		{
			var colorMap = new ColorMap[2];
			colorMap[0] = new ColorMap();
			colorMap[0].OldColor = Color.FromArgb(0, 0, 0);
			colorMap[0].NewColor = ColorChanger.Instance.GetColor(ColorChanger.FOREGROUND);
			colorMap[1] = new ColorMap();
			colorMap[1].OldColor = Image.GetPixel(0, 0);
			colorMap[1].NewColor = Color.Transparent;

			imageAttributes.SetRemapTable(colorMap);

			e.Graphics.DrawImage(
				Image,
				new Rectangle(0, 0, Image.Width, Image.Height),
				0, 0,
				Image.Width,
				Image.Height,
				GraphicsUnit.Pixel,
				imageAttributes);
		}

		base.OnPaint(e);
	}

	public void RefreshChanges()
	{
		if (IsDisposed)
			return;

		var mouseOver = ClientRectangle.Contains(PointToClient(MousePosition));
		if (mouseOver != IsMouseOver)
			IsMouseOver = mouseOver;

		OnRefreshChanges();
	}

	protected virtual void OnRefreshChanges()
	{
	}
}