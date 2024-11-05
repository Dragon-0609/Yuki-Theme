using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using YukiTheme.Engine;
using YukiTheme.Style.Helpers;

namespace YukiTheme.Style;

internal class YukiDockPaneStrip : DockPaneStripBase
{
	private static Bitmap _imageButtonClose;

	private static Bitmap _imageButtonWindowList;

	private static Bitmap _imageButtonWindowListOverflow;
	private Font m_boldFont;

	private InertButton m_buttonClose;

	private InertButton m_buttonWindowList;

	private bool m_documentTabsOverflow;

	private Font m_font;

	private int m_startDisplayingTab;
	private readonly ToolTip m_toolTip;

	public YukiDockPaneStrip(DockPane pane) : base(pane)
	{
		SetStyle(ControlStyles.ResizeRedraw |
		         ControlStyles.UserPaint |
		         ControlStyles.AllPaintingInWmPaint |
		         ControlStyles.OptimizedDoubleBuffer, true);

		SuspendLayout();

		Components = new Container();
		m_toolTip = new ToolTip(Components);
		SelectMenu = new ContextMenuStrip(Components);

		ResumeLayout();
		PluginEvents.Instance.Reload += Invalidate;
	}

	private static Bitmap ImageButtonClose
	{
		get
		{
			if (_imageButtonClose == null)
			{
				var sc = ScreenScale.Calc();
				if (sc >= 1.99)
					_imageButtonClose = Resources.DockPane_Close32;
				else _imageButtonClose = Resources.DockPane_Close;
			}

			return _imageButtonClose;
		}
	}

	private InertButton ButtonClose
	{
		get
		{
			if (m_buttonClose == null)
			{
				m_buttonClose = new InertButton(ImageButtonClose, ImageButtonClose);
				m_toolTip.SetToolTip(m_buttonClose, ToolTipClose);
				m_buttonClose.Click += Close_Click;
				Controls.Add(m_buttonClose);
			}

			return m_buttonClose;
		}
	}

	private static Bitmap ImageButtonWindowList
	{
		get
		{
			if (_imageButtonWindowList == null)
			{
				var sc = ScreenScale.Calc();
				if (sc >= 1.99)
					_imageButtonWindowList = Resources.DockPane_Option32;
				else _imageButtonWindowList = Resources.DockPane_Option;
			}

			return _imageButtonWindowList;
		}
	}

	private static Bitmap ImageButtonWindowListOverflow
	{
		get
		{
			if (_imageButtonWindowListOverflow == null) _imageButtonWindowListOverflow = Resources.DockPane_OptionOverflow;

			return _imageButtonWindowListOverflow;
		}
	}

	private InertButton ButtonWindowList
	{
		get
		{
			if (m_buttonWindowList == null)
			{
				m_buttonWindowList = new InertButton(ImageButtonWindowList, ImageButtonWindowListOverflow);
				m_toolTip.SetToolTip(m_buttonWindowList, ToolTipSelect);
				m_buttonWindowList.Click += WindowList_Click;
				Controls.Add(m_buttonWindowList);
			}

			return m_buttonWindowList;
		}
	}

	private static GraphicsPath GraphicsPath => YukiAutoHideStrip.GraphicsPath;

	private IContainer Components { get; }

	private static Font TextFont => SystemInformation.MenuFont;

	private Font BoldFont
	{
		get
		{
			if (IsDisposed)
				return null;

			if (m_boldFont == null)
			{
				m_font = TextFont;
				m_boldFont = new Font(TextFont, FontStyle.Bold);
			}
			else if (m_font != TextFont)
			{
				m_boldFont.Dispose();
				m_font = TextFont;
				m_boldFont = new Font(TextFont, FontStyle.Bold);
			}

			return m_boldFont;
		}
	}

	private int StartDisplayingTab
	{
		get => m_startDisplayingTab;
		set
		{
			m_startDisplayingTab = value;
			Invalidate();
		}
	}

	private int EndDisplayingTab { get; set; }

	private bool DocumentTabsOverflow
	{
		set
		{
			if (m_documentTabsOverflow == value)
				return;

			m_documentTabsOverflow = value;
			if (value)
				ButtonWindowList.ImageCategory = 1;
			else
				ButtonWindowList.ImageCategory = 0;
		}
	}

	private Rectangle TabStripRectangle
	{
		get
		{
			if (Appearance == DockPane.AppearanceStyle.Document)
				return TabStripRectangle_Document;
			return TabStripRectangle_ToolWindow;
		}
	}

	private Rectangle TabStripRectangle_ToolWindow
	{
		get
		{
			var rect = ClientRectangle;
			return new Rectangle(rect.X, rect.Top + ToolWindowStripGapTop, rect.Width,
				rect.Height - ToolWindowStripGapTop - ToolWindowStripGapBottom);
		}
	}

	private Rectangle TabStripRectangle_Document
	{
		get
		{
			var rect = ClientRectangle;
			return new Rectangle(rect.X, rect.Top + DocumentStripGapTop, rect.Width,
				rect.Height - DocumentStripGapTop - ToolWindowStripGapBottom);
		}
	}

	private Rectangle TabsRectangle
	{
		get
		{
			if (Appearance == DockPane.AppearanceStyle.ToolWindow)
				return TabStripRectangle;

			var rectWindow = TabStripRectangle;
			var x = rectWindow.X;
			var y = rectWindow.Y;
			var width = rectWindow.Width;
			var height = rectWindow.Height;

			x += DocumentTabGapLeft;
			width -= DocumentTabGapLeft +
			         DocumentTabGapRight +
			         DocumentButtonGapRight +
			         ButtonClose.Width +
			         ButtonWindowList.Width +
			         2 * DocumentButtonGapBetween;

			return new Rectangle(x, y, width, height);
		}
	}

	private ContextMenuStrip SelectMenu { get; }

	protected override Tab CreateTab(IDockContent content)
	{
		return new TabYuki(content);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			Components.Dispose();
			if (m_boldFont != null)
			{
				m_boldFont.Dispose();
				m_boldFont = null;
			}
		}

		base.Dispose(disposing);
	}

	protected override int MeasureHeight()
	{
		if (Appearance == DockPane.AppearanceStyle.ToolWindow)
			return MeasureHeight_ToolWindow();
		return MeasureHeight_Document();
	}

	private int MeasureHeight_ToolWindow()
	{
		if (DockPane.IsAutoHide || Tabs.Count <= 1)
			return 0;

		var height = Math.Max(TextFont.Height,
			             ToolWindowImageHeight + ToolWindowImageGapTop + ToolWindowImageGapBottom)
		             + ToolWindowStripGapTop + ToolWindowStripGapBottom;

		return height;
	}

	private int MeasureHeight_Document()
	{
		var height = Math.Max(TextFont.Height + DocumentTabGapTop,
			             ButtonClose.Height + DocumentButtonGapTop + DocumentButtonGapBottom)
		             + DocumentStripGapBottom + DocumentStripGapTop;

		return height;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (BackColor != ColorReference.BackgroundDefaultColor)
			BackColor = ColorReference.BackgroundDefaultColor;
		base.OnPaint(e);
		CalculateTabs();
		if (Appearance == DockPane.AppearanceStyle.Document && DockPane.ActiveContent != null)
			if (EnsureDocumentTabVisible(DockPane.ActiveContent, false))
				CalculateTabs();


		DrawTabStrip(e.Graphics);
	}

	protected override void OnRefreshChanges()
	{
		SetInertButtons();
		Invalidate();
	}

	protected override GraphicsPath GetOutline(int index)
	{
		if (Appearance == DockPane.AppearanceStyle.Document)
			return GetOutline_Document(index);
		return GetOutline_ToolWindow(index);
	}

	private GraphicsPath GetOutline_Document(int index)
	{
		var rectTab = GetTabRectangle(index);
		rectTab.X -= rectTab.Height / 2;
		rectTab.Intersect(TabsRectangle);
		rectTab = RectangleToScreen(DrawHelper.RtlTransform(this, rectTab));
		var y = rectTab.Top;
		var rectPaneClient = DockPane.RectangleToScreen(DockPane.ClientRectangle);

		var path = new GraphicsPath();
		var pathTab = GetTabOutline_Document(Tabs[index], true, true, true);
		path.AddPath(pathTab, true);
		path.AddLine(rectTab.Right, rectTab.Bottom, rectPaneClient.Right, rectTab.Bottom);
		path.AddLine(rectPaneClient.Right, rectTab.Bottom, rectPaneClient.Right, rectPaneClient.Bottom);
		path.AddLine(rectPaneClient.Right, rectPaneClient.Bottom, rectPaneClient.Left, rectPaneClient.Bottom);
		path.AddLine(rectPaneClient.Left, rectPaneClient.Bottom, rectPaneClient.Left, rectTab.Bottom);
		path.AddLine(rectPaneClient.Left, rectTab.Bottom, rectTab.Right, rectTab.Bottom);
		return path;
	}

	private GraphicsPath GetOutline_ToolWindow(int index)
	{
		var rectTab = GetTabRectangle(index);
		rectTab.Intersect(TabsRectangle);
		rectTab = RectangleToScreen(DrawHelper.RtlTransform(this, rectTab));
		var y = rectTab.Top;
		var rectPaneClient = DockPane.RectangleToScreen(DockPane.ClientRectangle);

		var path = new GraphicsPath();
		var pathTab = GetTabOutline(Tabs[index], true, true);
		path.AddPath(pathTab, true);
		path.AddLine(rectTab.Left, rectTab.Top, rectPaneClient.Left, rectTab.Top);
		path.AddLine(rectPaneClient.Left, rectTab.Top, rectPaneClient.Left, rectPaneClient.Top);
		path.AddLine(rectPaneClient.Left, rectPaneClient.Top, rectPaneClient.Right, rectPaneClient.Top);
		path.AddLine(rectPaneClient.Right, rectPaneClient.Top, rectPaneClient.Right, rectTab.Top);
		path.AddLine(rectPaneClient.Right, rectTab.Top, rectTab.Right, rectTab.Top);
		return path;
	}

	private void CalculateTabs()
	{
		if (Appearance == DockPane.AppearanceStyle.ToolWindow)
			CalculateTabs_ToolWindow();
		else
			CalculateTabs_Document();
	}

	private void CalculateTabs_ToolWindow()
	{
		if (Tabs.Count <= 1 || DockPane.IsAutoHide)
			return;

		var rectTabStrip = TabStripRectangle;

		// Calculate tab widths
		var countTabs = Tabs.Count;
		foreach (var tab in Tabs)
		{
			var tbw = tab.GetType()
				.GetProperty("MaxWidth", BindingFlags.Public | BindingFlags.Instance);
			tbw.SetValue(tab, GetMaxTabWidth(Tabs.IndexOf(tab)));

			var tbf = tab.GetType()
				.GetProperty("Flag", BindingFlags.NonPublic | BindingFlags.Instance);
			tbf.SetValue(tab, false);
		}

		// Set tab whose max width less than average width
		var anyWidthWithinAverage = true;
		var totalWidth = rectTabStrip.Width - ToolWindowStripGapLeft - ToolWindowStripGapRight;
		var totalAllocatedWidth = 0;
		var averageWidth = totalWidth / countTabs;
		var remainedTabs = countTabs;
		for (anyWidthWithinAverage = true; anyWidthWithinAverage && remainedTabs > 0;)
		{
			anyWidthWithinAverage = false;
			foreach (var tab in Tabs)
			{
				var tbf = tab.GetType()
					.GetProperty("Flag", BindingFlags.NonPublic | BindingFlags.Instance);

				if ((bool)tbf.GetValue(tab))
					continue;
				var tbm = tab.GetType()
					.GetProperty("MaxWidth", BindingFlags.Public | BindingFlags.Instance);

				if ((int)tbm.GetValue(tab) <= averageWidth)
				{
					tbf.SetValue(tab, true);
					var tbw = tab.GetType()
						.GetProperty("TabWidth", BindingFlags.Public | BindingFlags.Instance);
					tbw.SetValue(tab, (int)tbm.GetValue(tab));
					totalAllocatedWidth += (int)tbm.GetValue(tab);
					anyWidthWithinAverage = true;
					remainedTabs--;
				}
			}

			if (remainedTabs != 0)
				averageWidth = (totalWidth - totalAllocatedWidth) / remainedTabs;
		}

		// If any tab width not set yet, set it to the average width
		if (remainedTabs > 0)
		{
			var roundUpWidth = totalWidth - totalAllocatedWidth - averageWidth * remainedTabs;
			foreach (var tab in Tabs)
			{
				var tbf = tab.GetType()
					.GetProperty("Flag", BindingFlags.NonPublic | BindingFlags.Instance);

				if ((bool)tbf.GetValue(tab))
					continue;

				tbf.SetValue(tab, true);
				var tbw = tab.GetType()
					.GetProperty("TabWidth", BindingFlags.Public | BindingFlags.Instance);
				if (roundUpWidth > 0)
				{
					tbw.SetValue(tab, averageWidth + 1);
					roundUpWidth--;
				}
				else
				{
					tbw.SetValue(tab, averageWidth);
				}
			}
		}

		// Set the X position of the tabs
		var x = rectTabStrip.X + ToolWindowStripGapLeft;
		foreach (var tab in Tabs)
		{
			var tbx = tab.GetType()
				.GetProperty("TabX", BindingFlags.Public | BindingFlags.Instance);
			tbx.SetValue(tab, x);
			var tbw = tab.GetType()
				.GetProperty("TabWidth", BindingFlags.Public | BindingFlags.Instance);
			x += (int)tbw.GetValue(tab);
		}
	}

	private bool CalculateDocumentTab(Rectangle rectTabStrip, ref int x, int index)
	{
		var overflow = false;

		var tab = Tabs[index];
		var tbm = tab.GetType()
			.GetProperty("MaxWidth", BindingFlags.Public | BindingFlags.Instance);
		tbm.SetValue(tab, GetMaxTabWidth(index));
		var width = Math.Min((int)tbm.GetValue(tab), DocumentTabMaxWidth);

		var tbx = tab.GetType()
			.GetProperty("TabX", BindingFlags.Public | BindingFlags.Instance);

		var tbw = tab.GetType()
			.GetProperty("TabWidth", BindingFlags.Public | BindingFlags.Instance);
		if (x + width < rectTabStrip.Right || index == StartDisplayingTab)
		{
			tbx.SetValue(tab, x);
			tbw.SetValue(tab, width);
			EndDisplayingTab = index;
		}
		else
		{
			tbx.SetValue(tab, 0);
			tbw.SetValue(tab, 0);
			overflow = true;
		}

		x += width;

		return overflow;
	}

	private void CalculateTabs_Document()
	{
		if (m_startDisplayingTab >= Tabs.Count)
			m_startDisplayingTab = 0;

		var rectTabStrip = TabsRectangle;

		var x = rectTabStrip.X + rectTabStrip.Height / 2;

		var overflow = false;
		for (var i = StartDisplayingTab; i < Tabs.Count; i++)
			overflow = CalculateDocumentTab(rectTabStrip, ref x, i);

		for (var i = 0; i < StartDisplayingTab; i++)
			overflow = CalculateDocumentTab(rectTabStrip, ref x, i);

		if (!overflow)
		{
			m_startDisplayingTab = 0;
			x = rectTabStrip.X;
			foreach (var tab in Tabs)
			{
				var tbx = tab.GetType()
					.GetProperty("TabX", BindingFlags.Public | BindingFlags.Instance);
				tbx.SetValue(tab, x);

				var tbw = tab.GetType()
					.GetProperty("TabWidth", BindingFlags.Public | BindingFlags.Instance);

				x += (int)tbw.GetValue(tab);
			}
		}

		DocumentTabsOverflow = overflow;
	}

	protected override void EnsureTabVisible(IDockContent content)
	{
		if (Appearance != DockPane.AppearanceStyle.Document || !Tabs.Contains(content))
			return;

		CalculateTabs();
		EnsureDocumentTabVisible(content, true);
	}

	private bool EnsureDocumentTabVisible(IDockContent content, bool repaint)
	{
		var index = Tabs.IndexOf(content);
		var tab = Tabs[index];
		var tbw = tab.GetType()
			.GetProperty("TabWidth", BindingFlags.Public | BindingFlags.Instance);
		if ((int)tbw.GetValue(tab) != 0)
			return false;

		StartDisplayingTab = index;
		if (repaint)
			Invalidate();

		return true;
	}

	private int GetMaxTabWidth(int index)
	{
		if (Appearance == DockPane.AppearanceStyle.ToolWindow)
			return GetMaxTabWidth_ToolWindow(index);
		return GetMaxTabWidth_Document(index);
	}

	private int GetMaxTabWidth_ToolWindow(int index)
	{
		var content = Tabs[index].Content;
		var sizeString = TextRenderer.MeasureText(content.DockHandler.TabText, TextFont);
		return ToolWindowImageWidth + sizeString.Width + ToolWindowImageGapLeft
		       + ToolWindowImageGapRight + ToolWindowTextGapRight;
	}

	private int GetMaxTabWidth_Document(int index)
	{
		var content = Tabs[index].Content;

		var height = GetTabRectangle_Document(index).Height;

		var sizeText = TextRenderer.MeasureText(content.DockHandler.TabText, BoldFont,
			new Size(DocumentTabMaxWidth, height), DocumentTextFormat);

		if (DockPane.DockPanel.ShowDocumentIcon)
			return sizeText.Width + DocumentIconWidth + DocumentIconGapLeft + DocumentIconGapRight +
			       DocumentTextGapRight;
		return sizeText.Width + DocumentIconGapLeft + DocumentTextGapRight;
	}

	private void DrawTabStrip(Graphics g)
	{
		if (Appearance == DockPane.AppearanceStyle.Document)
			DrawTabStrip_Document(g);
		else
			DrawTabStrip_ToolWindow(g);
	}

	private void DrawTabStrip_Document(Graphics g)
	{
		var count = Tabs.Count;
		if (count == 0)
			return;

		var rectTabStrip = TabStripRectangle;

		// Draw the tabs
		var rectTabOnly = TabsRectangle;
		var rectTab = Rectangle.Empty;
		Tab tabActive = null;
		g.SetClip(DrawHelper.RtlTransform(this, rectTabOnly));
		for (var i = 0; i < count; i++)
		{
			rectTab = GetTabRectangle(i);
			if (Tabs[i].Content == DockPane.ActiveContent)
			{
				tabActive = Tabs[i];
				continue;
			}

			if (rectTab.IntersectsWith(rectTabOnly))
				DrawTab(g, Tabs[i], rectTab);
		}


		g.SetClip(rectTabStrip);
		g.DrawLine(PenDocumentTabActiveBorder, rectTabStrip.Left, rectTabStrip.Bottom - 1,
			rectTabStrip.Right, rectTabStrip.Bottom - 1);


		g.SetClip(DrawHelper.RtlTransform(this, rectTabOnly));
		if (tabActive != null)
		{
			rectTab = GetTabRectangle(Tabs.IndexOf(tabActive));
			if (rectTab.IntersectsWith(rectTabOnly))
				DrawTab(g, tabActive, rectTab);
			if (DockPane.IsActiveDocumentPane)
				g.DrawLine(ColorReference.BorderThickPen, rectTab.Left, rectTab.Bottom,
					rectTab.Right, rectTab.Bottom);
		}
	}

	private void DrawTabStrip_ToolWindow(Graphics g)
	{
		g.DrawRectangle(PenToolWindowTabBorder, TabStripRectangle);

		for (var i = 0; i < Tabs.Count; i++)
			DrawTab(g, Tabs[i], GetTabRectangle(i));
	}

	private Rectangle GetTabRectangle(int index)
	{
		if (Appearance == DockPane.AppearanceStyle.ToolWindow)
			return GetTabRectangle_ToolWindow(index);
		return GetTabRectangle_Document(index);
	}

	private Rectangle GetTabRectangle_ToolWindow(int index)
	{
		var rectTabStrip = TabStripRectangle;

		var tab = Tabs[index];
		var tbx = tab.GetType()
			.GetProperty("TabX", BindingFlags.Public | BindingFlags.Instance);

		var tbw = tab.GetType()
			.GetProperty("TabWidth", BindingFlags.Public | BindingFlags.Instance);
		return new Rectangle((int)tbx.GetValue(tab), rectTabStrip.Y, (int)tbw.GetValue(tab),
			rectTabStrip.Height);
	}

	private Rectangle GetTabRectangle_Document(int index)
	{
		var rectTabStrip = TabStripRectangle;
		var tab = Tabs[index];
		var tbx = tab.GetType()
			.GetProperty("TabX", BindingFlags.Public | BindingFlags.Instance);

		var tbw = tab.GetType()
			.GetProperty("TabWidth", BindingFlags.Public | BindingFlags.Instance);

		return new Rectangle((int)tbx.GetValue(tab), rectTabStrip.Y + DocumentTabGapTop,
			(int)tbw.GetValue(tab), rectTabStrip.Height - DocumentTabGapTop);
	}

	private void DrawTab(Graphics g, Tab tab, Rectangle rect)
	{
		if (Appearance == DockPane.AppearanceStyle.ToolWindow)
			DrawTab_ToolWindow(g, tab, rect);
		else
			DrawTab_Document(g, tab, rect);
	}

	private GraphicsPath GetTabOutline(Tab tab, bool rtlTransform, bool toScreen)
	{
		if (Appearance == DockPane.AppearanceStyle.ToolWindow)
			return GetTabOutline_ToolWindow(tab, rtlTransform, toScreen);
		return GetTabOutline_Document(tab, rtlTransform, toScreen, false);
	}

	private GraphicsPath GetTabOutline_ToolWindow(Tab tab, bool rtlTransform, bool toScreen)
	{
		var rect = GetTabRectangle(Tabs.IndexOf(tab));
		if (rtlTransform)
			rect = DrawHelper.RtlTransform(this, rect);
		if (toScreen)
			rect = RectangleToScreen(rect);

		DrawHelper.GetRoundedCornerTab(GraphicsPath, rect, false);
		return GraphicsPath;
	}

	private GraphicsPath GetTabOutline_Document(Tab tab, bool rtlTransform, bool toScreen, bool full)
	{
		GraphicsPath.Reset();
		var rect = GetTabRectangle(Tabs.IndexOf(tab));

		// Shorten TabOutline so it doesn't get overdrawn by icons next to it
		rect.Intersect(TabsRectangle);
		rect.Width--;

		if (rtlTransform)
			rect = DrawHelper.RtlTransform(this, rect);
		if (toScreen)
			rect = RectangleToScreen(rect);

		GraphicsPath.AddRectangle(rect);
		return GraphicsPath;
	}


	private void DrawTab_ToolWindow(Graphics g, Tab tab, Rectangle rect)
	{
		var rectIcon = new Rectangle(
			rect.X + ToolWindowImageGapLeft,
			rect.Y + rect.Height - 1 - ToolWindowImageGapBottom - ToolWindowImageHeight,
			ToolWindowImageWidth, ToolWindowImageHeight);
		var rectText = rectIcon;
		rectText.X += rectIcon.Width + ToolWindowImageGapRight;
		rectText.Width = rect.Width - rectIcon.Width - ToolWindowImageGapLeft -
		                 ToolWindowImageGapRight - ToolWindowTextGapRight;
		if (TextFont.Height > rectIcon.Height)
		{
			rectText.Y -= TextFont.Height - rectIcon.Height;
			rectText.Height = TextFont.Height;
		}
		//rectText.Y = rect.Y + Convert.ToInt32((rect.Height - TextFont.Height) / 2) - 1;
		//rectText.Height = TextFont.Height;

		var rectTab = DrawHelper.RtlTransform(this, rect);
		rectText = DrawHelper.RtlTransform(this, rectText);
		rectIcon = DrawHelper.RtlTransform(this, rectIcon);
		var path = GetTabOutline(tab, true, false);
		if (DockPane.ActiveContent == tab.Content)
		{
			g.FillPath(BrushToolWindowActiveBackground, path);
			TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText,
				ColorToolWindowActiveText, ToolWindowTextFormat);
		}
		else
		{
			if (oldTabs.Contains(tab)) g.FillPath(BrushToolWindowHoverBackground, path);

			TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText,
				ColorToolWindowInactiveText, ToolWindowTextFormat);
		}

		if (rectTab.Contains(rectIcon))
			g.DrawIcon(tab.Content.DockHandler.Icon, rectIcon);
	}

	private void DrawTab_Document(Graphics g, Tab tab, Rectangle rect)
	{
		var tbw = tab.GetType()
			.GetProperty("TabWidth", BindingFlags.Public | BindingFlags.Instance);
		if ((int)tbw.GetValue(tab) == 0)
			return;

		var rectIcon = new Rectangle(
			rect.X + DocumentIconGapLeft,
			rect.Y + rect.Height - 1 - DocumentIconGapBottom - DocumentIconHeight + 1,
			DocumentIconWidth, DocumentIconHeight);
		var rectText = rectIcon;
		if (DockPane.DockPanel.ShowDocumentIcon)
		{
			rectText.X += rectIcon.Width + DocumentIconGapRight;
			rectText.Y = rect.Y;
			rectText.Width = rect.Width - rectIcon.Width - DocumentIconGapLeft -
			                 DocumentIconGapRight - DocumentTextGapRight;
			rectText.Height = rect.Height;
		}
		else
		{
			rectText.Width = rect.Width - DocumentIconGapLeft - DocumentTextGapRight;
		}

		var fnt = TextFont;
		if (DockPane.IsActiveDocumentPane)
			fnt = BoldFont;
		rectText.Y = rect.Y + Convert.ToInt32((rect.Height - fnt.Height) / 2) - 1;
		rectText.Height = fnt.Height + 1;
		var rectTab = DrawHelper.RtlTransform(this, rect);
		rectText = DrawHelper.RtlTransform(this, rectText);
		rectIcon = DrawHelper.RtlTransform(this, rectIcon);
		var path = GetTabOutline(tab, true, false);

		var toDrawTabText = ColorDocumentActiveText;
		if (tab.Content.DockHandler.TabText.Contains("*")) toDrawTabText = ColorReference.ChangedTab;

		if (DockPane.ActiveContent == tab.Content)
		{
			g.FillPath(BrushDocumentActiveBackground, path);
			// g.DrawPath(PenDocumentTabActiveBorder, path);
			// Console.WriteLine($"Drawing: {tab.Content.DockHandler.TabText}");

			if (DockPane.IsActiveDocumentPane)
				TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, BoldFont, rectText,
					toDrawTabText, DocumentTextFormat);
			else
				TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText,
					toDrawTabText, DocumentTextFormat);
		}
		else
		{
			if (oldTabs.Contains(tab))
				g.FillPath(BrushToolWindowHoverBackground, path);
			else
				g.FillPath(BrushDocumentInactiveBackground, path);
			// g.DrawPath(PenDocumentTabInactiveBorder, path);
			TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText,
				toDrawTabText, DocumentTextFormat);
		}

		if (rectTab.Contains(rectIcon) && DockPane.DockPanel.ShowDocumentIcon)
			g.DrawIcon(tab.Content.DockHandler.Icon, rectIcon);
	}

	private void WindowList_Click(object sender, EventArgs e)
	{
		var x = 0;
		var y = ButtonWindowList.Location.Y + ButtonWindowList.Height;

		SelectMenu.Items.Clear();
		foreach (var tab in Tabs)
		{
			var content = tab.Content;
			var item =
				SelectMenu.Items.Add(content.DockHandler.TabText, content.DockHandler.Icon.ToBitmap());
			item.Tag = tab.Content;
			item.Click += ContextMenuItem_Click;
		}

		SelectMenu.Show(ButtonWindowList, x, y);
	}

	private void ContextMenuItem_Click(object sender, EventArgs e)
	{
		var item = sender as ToolStripMenuItem;
		if (item != null)
		{
			var content = (IDockContent)item.Tag;
			DockPane.ActiveContent = content;
		}
	}

	private void SetInertButtons()
	{
		if (Appearance == DockPane.AppearanceStyle.ToolWindow)
		{
			if (m_buttonClose != null)
				m_buttonClose.Left = -m_buttonClose.Width;

			if (m_buttonWindowList != null)
				m_buttonWindowList.Left = -m_buttonWindowList.Width;
		}
		else
		{
			var showCloseButton = DockPane.ActiveContent == null
				? true
				: DockPane.ActiveContent.DockHandler.CloseButton;
			ButtonClose.Enabled = showCloseButton;
			ButtonClose.RefreshChanges();
			ButtonWindowList.RefreshChanges();
		}
	}

	protected override void OnLayout(LayoutEventArgs levent)
	{
		if (Appearance != DockPane.AppearanceStyle.Document)
		{
			base.OnLayout(levent);
			return;
		}

		var rectTabStrip = TabStripRectangle;

		// Set position and size of the buttons
		var buttonWidth = ButtonClose.Image.Width;
		var buttonHeight = ButtonClose.Image.Height;
		var height = rectTabStrip.Height - DocumentButtonGapTop - DocumentButtonGapBottom;
		if (buttonHeight < height)
		{
			buttonWidth = buttonWidth * (height / buttonHeight);
			buttonHeight = height;
		}

		var buttonSize = new Size(buttonWidth, buttonHeight);

		var x = rectTabStrip.X + rectTabStrip.Width - DocumentTabGapLeft
		                                            - DocumentButtonGapRight - buttonWidth;
		var y = rectTabStrip.Y + DocumentButtonGapTop;
		var point = new Point(x, y);
		ButtonClose.Bounds = DrawHelper.RtlTransform(this, new Rectangle(point, buttonSize));
		point.Offset(-(DocumentButtonGapBetween + buttonWidth), 0);
		ButtonWindowList.Bounds = DrawHelper.RtlTransform(this, new Rectangle(point, buttonSize));

		OnRefreshChanges();

		base.OnLayout(levent);
	}

	private void Close_Click(object sender, EventArgs e)
	{
		DockPane.CloseActiveContent();
	}

	protected override int HitTest(Point ptMouse)
	{
		var rectTabStrip = TabsRectangle;
		if (!TabsRectangle.Contains(ptMouse))
			return -1;

		foreach (var tab in Tabs)
		{
			var path = GetTabOutline(tab, true, false);
			if (path.IsVisible(ptMouse))
				return Tabs.IndexOf(tab);
		}

		return -1;
	}

	protected override void OnMouseHover(EventArgs e)
	{
		var index = HitTest(PointToClient(MousePosition));
		var toolTip = string.Empty;

		base.OnMouseHover(e);

		if (index != -1)
		{
			var tab = Tabs[index];

			var tbm = tab.GetType()
				.GetProperty("MaxWidth", BindingFlags.Public | BindingFlags.Instance);

			var tbw = tab.GetType()
				.GetProperty("TabWidth", BindingFlags.Public | BindingFlags.Instance);

			if (!string.IsNullOrEmpty(tab.Content.DockHandler.ToolTipText))
				toolTip = tab.Content.DockHandler.ToolTipText;
			else if ((int)tbm.GetValue(tab) > (int)tbw.GetValue(tab))
				toolTip = tab.Content.DockHandler.TabText;
		}

		if (m_toolTip.GetToolTip(this) != toolTip)
		{
			m_toolTip.Active = false;
			m_toolTip.SetToolTip(this, toolTip);
			m_toolTip.Active = true;
		}

		// requires further tracking of mouse hover behavior,
		ResetMouseEventArgs();
	}

	protected override void OnRightToLeftChanged(EventArgs e)
	{
		base.OnRightToLeftChanged(e);
		PerformLayout();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		var index = HitTest(PointToClient(MousePosition));

		if (index != -1)
		{
			if (currentIndex != index && DockPane.ActiveContent != Tabs[index].Content)
			{
				CleanCurrentTab();
				currentIndex = index;
				oldTabs.Add(Tabs[index]);
				oldTabsIndex.Add(index);
				Invalidate(GetTabRectangle(index));
			}
		}
		else
		{
			CleanCurrentTab();
		}

		base.OnMouseMove(e);
	}


	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
		CleanCurrentTab();
	}

	private void CleanCurrentTab()
	{
		for (var index = 0; index < oldTabsIndex.Count; index++)
		{
			if (currentIndex == oldTabsIndex[index]) currentIndex = -1;

			oldTabs.Remove(Tabs[oldTabsIndex[index]]);
			Invalidate(GetTabRectangle(oldTabsIndex[index]));
			oldTabsIndex.Remove(oldTabsIndex[index]);
		}

		if (currentIndex != -1)
		{
			Invalidate(GetTabRectangle(currentIndex));
			currentIndex = -1;
		}
	}

	private class TabYuki : Tab
	{
		public TabYuki(IDockContent content)
			: base(content)
		{
		}

		public int TabX { get; set; }

		public int TabWidth { get; set; }

		public int MaxWidth { get; set; }

		protected internal bool Flag { get; set; }
	}

	private sealed class InertButton : InertButtonBase
	{
		private readonly Bitmap m_image0;
		private readonly Bitmap m_image1;

		private int m_imageCategory;

		public InertButton(Bitmap image0, Bitmap image1)
		{
			m_image0 = image0;
			m_image1 = image1;
		}

		public int ImageCategory
		{
			get => m_imageCategory;
			set
			{
				if (m_imageCategory == value)
					return;

				m_imageCategory = value;
				Invalidate();
			}
		}

		public override Bitmap Image => ImageCategory == 0 ? m_image0 : m_image1;

		protected override void OnRefreshChanges()
		{
			if (ColorDocumentActiveText != ForeColor)
			{
				ForeColor = ColorDocumentActiveText;
				Invalidate();
			}
		}
	}

	#region consts

	private const int _ToolWindowStripGapTop = 0;
	private const int _ToolWindowStripGapBottom = 1;
	private const int _ToolWindowStripGapLeft = 0;
	private const int _ToolWindowStripGapRight = 5;
	private const int _ToolWindowImageHeight = 16;
	private const int _ToolWindowImageWidth = 16;
	private const int _ToolWindowImageGapTop = 3;
	private const int _ToolWindowImageGapBottom = 1;
	private const int _ToolWindowImageGapLeft = 5;
	private const int _ToolWindowImageGapRight = 0;
	private const int _ToolWindowTextGapRight = 0;
	private const int _ToolWindowTabSeperatorGapTop = 3;
	private const int _ToolWindowTabSeperatorGapBottom = 3;

	private const int _DocumentStripGapTop = -1;
	private const int _DocumentStripGapBottom = 1;
	private const int _DocumentTabMaxWidth = 400;
	private const int _DocumentButtonGapTop = 4;
	private const int _DocumentButtonGapBottom = 8;
	private const int _DocumentButtonGapBetween = 0;
	private const int _DocumentButtonGapRight = 3;
	private const int _DocumentTabGapTop = 0;
	private const int _DocumentTabGapLeft = 0;
	private const int _DocumentTabGapRight = 0;
	private const int _DocumentIconGapBottom = 2;
	private const int _DocumentIconGapLeft = 8;
	private const int _DocumentIconGapRight = 0;
	private const int _DocumentIconHeight = 16;
	private const int _DocumentIconWidth = 16;
	private const int _DocumentTextGapRight = 6;

	#endregion

	#region Customizable Properties

	private static int ToolWindowStripGapTop => _ToolWindowStripGapTop;

	private static int ToolWindowStripGapBottom => _ToolWindowStripGapBottom;

	private static int ToolWindowStripGapLeft => _ToolWindowStripGapLeft;

	private static int ToolWindowStripGapRight => _ToolWindowStripGapRight;

	private static int ToolWindowImageHeight => Convert.ToInt32(_ToolWindowImageHeight * ScreenScale.Calc());

	private static int ToolWindowImageWidth => Convert.ToInt32(_ToolWindowImageWidth * ScreenScale.Calc());

	private static int ToolWindowImageGapTop => _ToolWindowImageGapTop;

	private static int ToolWindowImageGapBottom => _ToolWindowImageGapBottom;

	private static int ToolWindowImageGapLeft => _ToolWindowImageGapLeft;

	private static int ToolWindowImageGapRight => _ToolWindowImageGapRight;

	private static int ToolWindowTextGapRight => _ToolWindowTextGapRight;

	private static int ToolWindowTabSeperatorGapTop => _ToolWindowTabSeperatorGapTop;

	private static int ToolWindowTabSeperatorGapBottom => _ToolWindowTabSeperatorGapBottom;

	private static string _toolTipClose;

	private static string ToolTipClose
	{
		get
		{
			if (_toolTipClose == null)
				_toolTipClose = Strings.DockPaneStrip_ToolTipClose;
			return _toolTipClose;
		}
	}

	private static string _toolTipSelect;

	private static string ToolTipSelect
	{
		get
		{
			if (_toolTipSelect == null)
				_toolTipSelect = Strings.DockPaneStrip_ToolTipWindowList;
			return _toolTipSelect;
		}
	}

	private TextFormatFlags ToolWindowTextFormat
	{
		get
		{
			var textFormat = TextFormatFlags.EndEllipsis |
			                 TextFormatFlags.HorizontalCenter |
			                 TextFormatFlags.SingleLine |
			                 TextFormatFlags.VerticalCenter;
			if (RightToLeft == RightToLeft.Yes)
				return textFormat | TextFormatFlags.RightToLeft | TextFormatFlags.Right;
			return textFormat;
		}
	}

	private static int DocumentStripGapTop => _DocumentStripGapTop;

	private static int DocumentStripGapBottom => _DocumentStripGapBottom;

	private TextFormatFlags DocumentTextFormat
	{
		get
		{
			var textFormat = TextFormatFlags.PathEllipsis |
			                 TextFormatFlags.SingleLine |
			                 TextFormatFlags.VerticalCenter |
			                 TextFormatFlags.PreserveGraphicsClipping |
			                 TextFormatFlags.HorizontalCenter;
			if (RightToLeft == RightToLeft.Yes)
				return textFormat | TextFormatFlags.RightToLeft;
			return textFormat;
		}
	}

	private static int DocumentTabMaxWidth => _DocumentTabMaxWidth;

	private static int DocumentButtonGapTop => _DocumentButtonGapTop;

	private static int DocumentButtonGapBottom => _DocumentButtonGapBottom;

	private static int DocumentButtonGapBetween => _DocumentButtonGapBetween;

	private static int DocumentButtonGapRight => _DocumentButtonGapRight;

	private static int DocumentTabGapTop => _DocumentTabGapTop;

	private static int DocumentTabGapLeft => _DocumentTabGapLeft;

	private static int DocumentTabGapRight => _DocumentTabGapRight;

	private static int DocumentIconGapBottom => _DocumentIconGapBottom;

	private static int DocumentIconGapLeft => _DocumentIconGapLeft;

	private static int DocumentIconGapRight => _DocumentIconGapRight;

	private static int DocumentIconWidth => _DocumentIconWidth;

	private static int DocumentIconHeight => _DocumentIconHeight;

	private static int DocumentTextGapRight => _DocumentTextGapRight;

	private static Pen PenToolWindowTabBorder => SystemPens.GrayText;

	private static Pen PenDocumentTabActiveBorder => SystemPens.GrayText;

	private static Pen PenDocumentTabInactiveBorder => SystemPens.GrayText;


	private static Brush BrushToolWindowActiveBackground => ColorReference.BackgroundClick3Brush;

	private static Brush BrushToolWindowHoverBackground => ColorReference.BackgroundClickBrush;

	private static Brush BrushDocumentActiveBackground => ColorReference.BackgroundClick3Brush;

	private static Brush BrushDocumentInactiveBackground => ColorReference.BackgroundDefaultBrush;

	private static Color ColorToolWindowActiveText => ColorReference.ForegroundColor;

	private static Color ColorDocumentActiveText => ColorReference.ForegroundColor;

	private static Color ColorToolWindowInactiveText => ColorReference.ForegroundColor;

	private static Color ColorDocumentInactiveText => ColorReference.ForegroundColor;

	private int currentIndex = -1;
	private readonly List<Tab> oldTabs = new();
	private readonly List<int> oldTabsIndex = new();

	#endregion
}