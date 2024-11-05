using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using YukiTheme.Engine;
using YukiTheme.Style.Helpers;

namespace YukiTheme.Style;

internal class YukiAutoHideStrip : AutoHideStripBase
{
	private const int _ImageHeight = 16;
	private const int _ImageWidth = 16;
	private const int _ImageGapTop = 3;
	private const int _ImageGapLeft = 5;
	private const int _ImageGapRight = 0;
	private const int _ImageGapBottom = 1;
	private const int _TextGapLeft = 0;
	private const int _TextGapRight = 0;
	private const int _TabGapTop = 0;
	private const int _TabGapLeft = 0;
	private const int _TabGapBetween = 10;

	private static DockState[] _dockStates;

	private static GraphicsPath _graphicsPath;

	private Tab current;
	private readonly List<Tab> oldTabs = new();

	public YukiAutoHideStrip(DockPanel panel) : base(panel)
	{
		SetStyle(ControlStyles.ResizeRedraw |
		         ControlStyles.UserPaint |
		         ControlStyles.AllPaintingInWmPaint |
		         ControlStyles.OptimizedDoubleBuffer, true);
		BackColor = ColorReference.BackgroundDefaultColor;
		PluginEvents.Instance.Reload += Invalidate;
	}

	private static Matrix MatrixIdentity { get; } = new();

	private static DockState[]
		DockStates
	{
		get
		{
			if (_dockStates == null)
			{
				_dockStates = new DockState[4];
				_dockStates[0] = DockState.DockLeftAutoHide;
				_dockStates[1] = DockState.DockRightAutoHide;
				_dockStates[2] = DockState.DockTopAutoHide;
				_dockStates[3] = DockState.DockBottomAutoHide;
			}

			return _dockStates;
		}
	}

	internal static GraphicsPath GraphicsPath
	{
		get
		{
			if (_graphicsPath == null)
				_graphicsPath = new GraphicsPath();

			return _graphicsPath;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (BackColor != ColorReference.BackgroundDefaultColor)
			BackColor = ColorReference.BackgroundDefaultColor;
		var g = e.Graphics;
		DrawTabStrip(g);
	}

	protected override void OnLayout(LayoutEventArgs levent)
	{
		CalculateTabs();
		base.OnLayout(levent);
	}

	private void DrawTabStrip(Graphics g)
	{
		DrawTabStrip(g, DockState.DockTopAutoHide);
		DrawTabStrip(g, DockState.DockBottomAutoHide);
		DrawTabStrip(g, DockState.DockLeftAutoHide);
		DrawTabStrip(g, DockState.DockRightAutoHide);
	}

	private void DrawTabStrip(Graphics g, DockState dockState)
	{
		var rectTabStrip = GetLogicalTabStripRectangle(dockState);

		if (rectTabStrip.IsEmpty)
			return;

		var matrixIdentity = g.Transform;
		if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide)
		{
			var matrixRotated = new Matrix();
			matrixRotated.RotateAt(90, new PointF(rectTabStrip.X + (float)rectTabStrip.Height / 2,
				rectTabStrip.Y + (float)rectTabStrip.Height / 2));
			g.Transform = matrixRotated;
		}

		foreach (var pane in GetPanes(dockState))
		foreach (var tab in pane.AutoHideTabs)
			DrawTab(g, tab);

		g.Transform = matrixIdentity;
	}

	private void CalculateTabs()
	{
		CalculateTabs(DockState.DockTopAutoHide);
		CalculateTabs(DockState.DockBottomAutoHide);
		CalculateTabs(DockState.DockLeftAutoHide);
		CalculateTabs(DockState.DockRightAutoHide);
	}

	private void CalculateTabs(DockState dockState)
	{
		var rectTabStrip = GetLogicalTabStripRectangle(dockState);

		var imageHeight = rectTabStrip.Height - ImageGapTop - ImageGapBottom;
		var imageWidth = ImageWidth;
		if (imageHeight > ImageHeight)
			imageWidth = ImageWidth * (imageHeight / ImageHeight);

		var x = TabGapLeft + rectTabStrip.X;
		foreach (var pane in GetPanes(dockState))
		{
			foreach (var tab in pane.AutoHideTabs)
			{
				var width = imageWidth + ImageGapLeft + ImageGapRight +
				            TextRenderer.MeasureText(tab.Content.DockHandler.TabText, TextFont).Width +
				            TextGapLeft + TextGapRight;

				var tbx = tab.GetType()
					.GetProperty("TabX", BindingFlags.Public | BindingFlags.Instance);
				tbx.SetValue(tab, x);

				var tbw = tab.GetType()
					.GetProperty("TabWidth", BindingFlags.Public | BindingFlags.Instance);
				tbw.SetValue(tab, width);
				x += width;
			}

			x += TabGapBetween;
		}
	}

	private Rectangle RtlTransform(Rectangle rect, DockState dockState)
	{
		Rectangle rectTransformed;
		if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide)
			rectTransformed = rect;
		else
			rectTransformed = DrawHelper.RtlTransform(this, rect);

		return rectTransformed;
	}

	private GraphicsPath GetTabOutline(Tab tab, bool transformed, bool rtlTransform)
	{
		var dockState = tab.Content.DockHandler.DockState;
		var rectTab = GetTabRectangle(tab, transformed);
		if (rtlTransform)
			rectTab = RtlTransform(rectTab, dockState);
		var upTab = dockState == DockState.DockLeftAutoHide || dockState == DockState.DockBottomAutoHide;
		DrawHelper.GetRoundedCornerTab(GraphicsPath, rectTab, upTab);

		return GraphicsPath;
	}

	private void DrawTab(Graphics g, Tab tab)
	{
		var rectTabOrigin = GetTabRectangle(tab);
		if (rectTabOrigin.IsEmpty)
			return;

		var dockState = tab.Content.DockHandler.DockState;
		var content = tab.Content;

		var path = GetTabOutline(tab, false, true);
		if (current != null && current == tab)
			g.FillPath(BrushTabPressBackground, path);
		else
			g.FillPath(BrushTabBackground, path);
		g.DrawPath(PenTabBorder, path);

		// Set no rotate for drawing icon and text
		var matrixRotate = g.Transform;
		g.Transform = MatrixIdentity;

		// Draw the icon
		var rectImage = rectTabOrigin;
		rectImage.X += ImageGapLeft;
		rectImage.Y += ImageGapTop;
		var imageHeight = rectTabOrigin.Height - ImageGapTop - ImageGapBottom;
		var imageWidth = ImageWidth;
		if (imageHeight > ImageHeight)
			imageWidth = ImageWidth * (imageHeight / ImageHeight);
		rectImage.Height = imageHeight;
		rectImage.Width = imageWidth;
		rectImage = GetTransformedRectangle(dockState, rectImage);
		g.DrawIcon(((Form)content).Icon, RtlTransform(rectImage, dockState));

		// Draw the text
		var rectText = rectTabOrigin;
		rectText.X += ImageGapLeft + imageWidth + ImageGapRight + TextGapLeft;
		rectText.Width -= ImageGapLeft + imageWidth + ImageGapRight + TextGapLeft;
		rectText = RtlTransform(GetTransformedRectangle(dockState, rectText), dockState);
		if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide)
			g.DrawString(content.DockHandler.TabText, TextFont, BrushTabText, rectText, StringFormatTabVertical);
		else
			g.DrawString(content.DockHandler.TabText, TextFont, BrushTabText, rectText, StringFormatTabHorizontal);

		// Set rotate back
		g.Transform = matrixRotate;
	}

	private Rectangle GetLogicalTabStripRectangle(DockState dockState)
	{
		return GetLogicalTabStripRectangle(dockState, false);
	}

	private Rectangle GetLogicalTabStripRectangle(DockState dockState, bool transformed)
	{
		if (!DockHelper.IsDockStateAutoHide(dockState))
			return Rectangle.Empty;

		var leftPanes = GetPanes(DockState.DockLeftAutoHide).Count;
		var rightPanes = GetPanes(DockState.DockRightAutoHide).Count;
		var topPanes = GetPanes(DockState.DockTopAutoHide).Count;
		var bottomPanes = GetPanes(DockState.DockBottomAutoHide).Count;

		int x, y, width, height;

		height = MeasureHeight();
		if (dockState == DockState.DockLeftAutoHide && leftPanes > 0)
		{
			x = 0;
			y = topPanes == 0 ? 0 : height;
			width = Height - (topPanes == 0 ? 0 : height) - (bottomPanes == 0 ? 0 : height);
		}
		else if (dockState == DockState.DockRightAutoHide && rightPanes > 0)
		{
			x = Width - height;
			if (leftPanes != 0 && x < height)
				x = height;
			y = topPanes == 0 ? 0 : height;
			width = Height - (topPanes == 0 ? 0 : height) - (bottomPanes == 0 ? 0 : height);
		}
		else if (dockState == DockState.DockTopAutoHide && topPanes > 0)
		{
			x = leftPanes == 0 ? 0 : height;
			y = 0;
			width = Width - (leftPanes == 0 ? 0 : height) - (rightPanes == 0 ? 0 : height);
		}
		else if (dockState == DockState.DockBottomAutoHide && bottomPanes > 0)
		{
			x = leftPanes == 0 ? 0 : height;
			y = Height - height;
			if (topPanes != 0 && y < height)
				y = height;
			width = Width - (leftPanes == 0 ? 0 : height) - (rightPanes == 0 ? 0 : height);
		}
		else
		{
			return Rectangle.Empty;
		}

		if (!transformed)
			return new Rectangle(x, y, width, height);
		return GetTransformedRectangle(dockState, new Rectangle(x, y, width, height));
	}

	private Rectangle GetTabRectangle(Tab tab)
	{
		return GetTabRectangle(tab, false);
	}

	private Rectangle GetTabRectangle(Tab tab, bool transformed)
	{
		var dockState = tab.Content.DockHandler.DockState;
		var rectTabStrip = GetLogicalTabStripRectangle(dockState);

		if (rectTabStrip.IsEmpty)
			return Rectangle.Empty;

		var tbx = tab.GetType().GetProperty("TabX", BindingFlags.Public | BindingFlags.Instance);
		var x = (int)tbx.GetValue(tab);
		var y = rectTabStrip.Y +
		        (dockState == DockState.DockTopAutoHide || dockState == DockState.DockRightAutoHide ? 0 : TabGapTop);
		var tbw = tab.GetType().GetProperty("TabWidth", BindingFlags.Public | BindingFlags.Instance);
		var width = (int)tbw.GetValue(tab);

		var height = rectTabStrip.Height - TabGapTop;

		if (!transformed)
			return new Rectangle(x, y, width, height);
		return GetTransformedRectangle(dockState, new Rectangle(x, y, width, height));
	}

	private Rectangle GetTransformedRectangle(DockState dockState, Rectangle rect)
	{
		if (dockState != DockState.DockLeftAutoHide && dockState != DockState.DockRightAutoHide)
			return rect;

		var pts = new PointF[1];
		// the center of the rectangle
		pts[0].X = rect.X + (float)rect.Width / 2;
		pts[0].Y = rect.Y + (float)rect.Height / 2;
		var rectTabStrip = GetLogicalTabStripRectangle(dockState);
		var matrix = new Matrix();
		matrix.RotateAt(90, new PointF(rectTabStrip.X + (float)rectTabStrip.Height / 2,
			rectTabStrip.Y + (float)rectTabStrip.Height / 2));
		matrix.TransformPoints(pts);

		return new Rectangle((int)(pts[0].X - (float)rect.Height / 2 + .5F),
			(int)(pts[0].Y - (float)rect.Width / 2 + .5F),
			rect.Height, rect.Width);
	}

	protected override IDockContent HitTest(Point ptMouse)
	{
		foreach (var state in DockStates)
		{
			var rectTabStrip = GetLogicalTabStripRectangle(state, true);
			if (!rectTabStrip.Contains(ptMouse))
				continue;

			foreach (var pane in GetPanes(state))
			{
				var dockState = pane.DockPane.DockState;
				foreach (var tab in pane.AutoHideTabs)
				{
					var path = GetTabOutline(tab, true, true);
					if (path.IsVisible(ptMouse))
						return tab.Content;
				}
			}
		}

		return null;
	}

	protected override int MeasureHeight()
	{
		return Math.Max(ImageGapBottom +
		                ImageGapTop + ImageHeight,
			TextFont.Height) + TabGapTop;
	}

	protected override void OnRefreshChanges()
	{
		CalculateTabs();
		Invalidate();
	}

	protected override Tab CreateTab(IDockContent content)
	{
		return new TabYuki(content);
	}


	protected override void OnMouseHover(EventArgs e)
	{
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		var ptMouse = PointToClient(MousePosition);
		var content = HitTest(ptMouse);
		if (content == null)
		{
			CleanCurrentTab();
			return;
		}

		if (DockHelper.IsDockStateAutoHide(content.DockHandler.DockState))
			foreach (var pane in GetPanes(content.DockHandler.DockState))
			foreach (var tab in pane.AutoHideTabs)
				if (content.DockHandler.TabText == tab.Content.DockHandler.TabText && current != tab)
				{
					CleanCurrentTab();
					current = tab;
					oldTabs.Add(tab);
					Invalidate(GetTabRectangle(current));
					return;
				}

		current = null;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (e.Button != MouseButtons.Left)
			return;

		var ptMouse = PointToClient(MousePosition);
		var content = HitTest(ptMouse);
		if (content == null)
			return;
		/*foreach (Pane pane in GetPanes(DockState.DockBottomAutoHide))
		{
			pane.DockPane.Hide ();
		}*/

		/*
		foreach (DockPane pane in content.DockHandler.Pane.DockWindow.NestedPanes)
		{
			if(DockHelper.IsDockWindowState (pane.DockState))
			{
				pane.DockState = DockHelper.ToggleAutoHideState (pane.DockState);
			}
		}

		content.DockHandler.Show (content.DockHandler.DockPanel,
		                          DockHelper.ToggleAutoHideState (content.DockHandler.Pane.DockState));*/
		content.DockHandler.Pane.DockState = DockHelper.ToggleAutoHideState(content.DockHandler.Pane.DockState);
		content.DockHandler.Activate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
		CleanCurrentTab();
	}

	private void CleanCurrentTab()
	{
		for (var index = 0; index < oldTabs.Count; index++)
		{
			if (current == oldTabs[index])
				current = null;
			Invalidate(GetTabRectangle(oldTabs[index]));
			oldTabs.Remove(oldTabs[index]);
		}

		if (current != null)
		{
			var tb = current;
			current = null;
			Invalidate(GetTabRectangle(tb));
		}
	}

	private class TabYuki : Tab
	{
		internal TabYuki(IDockContent content)
			: base(content)
		{
		}

		public int TabX { get; set; } = 0;

		public int TabWidth { get; set; } = 0;
	}

	#region Customizable Properties

	private static Font TextFont => SystemInformation.MenuFont;

	private static StringFormat _stringFormatTabHorizontal;

	private StringFormat StringFormatTabHorizontal
	{
		get
		{
			if (_stringFormatTabHorizontal == null)
			{
				_stringFormatTabHorizontal = new StringFormat();
				_stringFormatTabHorizontal.Alignment = StringAlignment.Near;
				_stringFormatTabHorizontal.LineAlignment = StringAlignment.Center;
				_stringFormatTabHorizontal.FormatFlags = StringFormatFlags.NoWrap;
			}

			if (RightToLeft == RightToLeft.Yes)
				_stringFormatTabHorizontal.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
			else
				_stringFormatTabHorizontal.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;

			return _stringFormatTabHorizontal;
		}
	}

	private static StringFormat _stringFormatTabVertical;

	private StringFormat StringFormatTabVertical
	{
		get
		{
			if (_stringFormatTabVertical == null)
			{
				_stringFormatTabVertical = new StringFormat();
				_stringFormatTabVertical.Alignment = StringAlignment.Near;
				_stringFormatTabVertical.LineAlignment = StringAlignment.Center;
				_stringFormatTabVertical.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.DirectionVertical;
			}

			if (RightToLeft == RightToLeft.Yes)
				_stringFormatTabVertical.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
			else
				_stringFormatTabVertical.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;

			return _stringFormatTabVertical;
		}
	}

	private static int ImageHeight => _ImageHeight;

	private static int ImageWidth => _ImageWidth;

	private static int ImageGapTop => _ImageGapTop;

	private static int ImageGapLeft => _ImageGapLeft;

	private static int ImageGapRight => _ImageGapRight;

	private static int ImageGapBottom => _ImageGapBottom;

	private static int TextGapLeft => _TextGapLeft;

	private static int TextGapRight => _TextGapRight;

	private static int TabGapTop => _TabGapTop;

	private static int TabGapLeft => _TabGapLeft;

	private static int TabGapBetween => _TabGapBetween;

	private static Brush BrushTabBackground => ColorReference.BackgroundBrush;


	private static Brush BrushTabPressBackground => ColorReference.BackgroundClickBrush;

	private static Pen PenTabBorder => ColorReference.BackgroundClick3Pen;

	private static Brush BrushTabText => ColorReference.ForegroundBrush;

	#endregion
}