using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Yuki_Theme_Plugin.Controls.DockStyles
{
	internal class YukiDockPaneStrip : DockPaneStripBase
	{
		private class TabYuki : Tab
		{
			public TabYuki (IDockContent content)
				: base (content)
			{
			}

			private int m_tabX;

			public int TabX
			{
				get { return m_tabX; }
				set { m_tabX = value; }
			}


			private int m_tabWidth;

			public int TabWidth
			{
				get { return m_tabWidth; }
				set { m_tabWidth = value; }
			}

			private int m_maxWidth;

			public int MaxWidth
			{
				get { return m_maxWidth; }
				set { m_maxWidth = value; }
			}

			private bool m_flag;

			protected internal bool Flag
			{
				get { return m_flag; }
				set { m_flag = value; }
			}
		}

		protected override DockPaneStripBase.Tab CreateTab (IDockContent content)
		{
			return new TabYuki (content);
		}

		private sealed class InertButton : InertButtonBase
		{
			private Bitmap m_image0, m_image1;

			public InertButton (Bitmap image0, Bitmap image1)
				: base ()
			{
				m_image0 = image0;
				m_image1 = image1;
			}

			private int m_imageCategory = 0;

			public int ImageCategory
			{
				get { return m_imageCategory; }
				set
				{
					if (m_imageCategory == value)
						return;

					m_imageCategory = value;
					Invalidate ();
				}
			}

			public override Bitmap Image
			{
				get { return ImageCategory == 0 ? m_image0 : m_image1; }
			}

			protected override void OnRefreshChanges ()
			{
				if (YukiDockPaneStrip.ColorDocumentActiveText != ForeColor)
				{
					ForeColor = YukiDockPaneStrip.ColorDocumentActiveText;
					Invalidate ();
				}
			}
		}

		#region consts

		private const int _ToolWindowStripGapTop           = 0;
		private const int _ToolWindowStripGapBottom        = 1;
		private const int _ToolWindowStripGapLeft          = 0;
		private const int _ToolWindowStripGapRight         = 5;
		private const int _ToolWindowImageHeight           = 16;
		private const int _ToolWindowImageWidth            = 16;
		private const int _ToolWindowImageGapTop           = 3;
		private const int _ToolWindowImageGapBottom        = 1;
		private const int _ToolWindowImageGapLeft          = 5;
		private const int _ToolWindowImageGapRight         = 0;
		private const int _ToolWindowTextGapRight          = 0;
		private const int _ToolWindowTabSeperatorGapTop    = 3;
		private const int _ToolWindowTabSeperatorGapBottom = 3;

		private const int _DocumentStripGapTop      = -1;
		private const int _DocumentStripGapBottom   = 1;
		private const int _DocumentTabMaxWidth      = 400;
		private const int _DocumentButtonGapTop     = 4;
		private const int _DocumentButtonGapBottom  = 8;
		private const int _DocumentButtonGapBetween = 0;
		private const int _DocumentButtonGapRight   = 3;
		private const int _DocumentTabGapTop        = 0;
		private const int _DocumentTabGapLeft       = 0;
		private const int _DocumentTabGapRight      = 0;
		private const int _DocumentIconGapBottom    = 2;
		private const int _DocumentIconGapLeft      = 8;
		private const int _DocumentIconGapRight     = 0;
		private const int _DocumentIconHeight       = 16;
		private const int _DocumentIconWidth        = 16;
		private const int _DocumentTextGapRight     = 6;

		#endregion

		private static Bitmap _imageButtonClose;

		private static Bitmap ImageButtonClose
		{
			get
			{
				if (_imageButtonClose == null)
				{
					var sc = ScreenScale.Calc ();
					if (sc >= 1.99)
						_imageButtonClose = Resources.DockPane_Close32;
					else _imageButtonClose = Resources.DockPane_Close;
				}

				return _imageButtonClose;
			}
		}

		private InertButton m_buttonClose;

		private InertButton ButtonClose
		{
			get
			{
				if (m_buttonClose == null)
				{
					m_buttonClose = new InertButton (ImageButtonClose, ImageButtonClose);
					m_toolTip.SetToolTip (m_buttonClose, ToolTipClose);
					m_buttonClose.Click += new EventHandler (Close_Click);
					Controls.Add (m_buttonClose);
				}

				return m_buttonClose;
			}
		}

		private static Bitmap _imageButtonWindowList;

		private static Bitmap ImageButtonWindowList
		{
			get
			{
				if (_imageButtonWindowList == null)
				{
					var sc = ScreenScale.Calc ();
					if (sc >= 1.99)
						_imageButtonWindowList = Resources.DockPane_Option32;
					else _imageButtonWindowList = Resources.DockPane_Option;
				}

				return _imageButtonWindowList;
			}
		}

		private static Bitmap _imageButtonWindowListOverflow;

		private static Bitmap ImageButtonWindowListOverflow
		{
			get
			{
				if (_imageButtonWindowListOverflow == null)
				{
					_imageButtonWindowListOverflow = Resources.DockPane_OptionOverflow;
				}

				return _imageButtonWindowListOverflow;
			}
		}

		private InertButton m_buttonWindowList;

		private InertButton ButtonWindowList
		{
			get
			{
				if (m_buttonWindowList == null)
				{
					m_buttonWindowList = new InertButton (ImageButtonWindowList, ImageButtonWindowListOverflow);
					m_toolTip.SetToolTip (m_buttonWindowList, ToolTipSelect);
					m_buttonWindowList.Click += new EventHandler (WindowList_Click);
					Controls.Add (m_buttonWindowList);
				}

				return m_buttonWindowList;
			}
		}

		private static GraphicsPath GraphicsPath
		{
			get { return YukiAutoHideStrip.GraphicsPath; }
		}

		private IContainer m_components;
		private ToolTip    m_toolTip;

		private IContainer Components
		{
			get { return m_components; }
		}

		#region Customizable Properties

		private static int ToolWindowStripGapTop
		{
			get { return _ToolWindowStripGapTop; }
		}

		private static int ToolWindowStripGapBottom
		{
			get { return _ToolWindowStripGapBottom; }
		}

		private static int ToolWindowStripGapLeft
		{
			get { return _ToolWindowStripGapLeft; }
		}

		private static int ToolWindowStripGapRight
		{
			get { return _ToolWindowStripGapRight; }
		}

		private static int ToolWindowImageHeight
		{
			get { return Convert.ToInt32 (_ToolWindowImageHeight * ScreenScale.Calc ()); }
		}

		private static int ToolWindowImageWidth
		{
			get { return Convert.ToInt32 (_ToolWindowImageWidth * ScreenScale.Calc ()); }
		}

		private static int ToolWindowImageGapTop
		{
			get { return _ToolWindowImageGapTop; }
		}

		private static int ToolWindowImageGapBottom
		{
			get { return _ToolWindowImageGapBottom; }
		}

		private static int ToolWindowImageGapLeft
		{
			get { return _ToolWindowImageGapLeft; }
		}

		private static int ToolWindowImageGapRight
		{
			get { return _ToolWindowImageGapRight; }
		}

		private static int ToolWindowTextGapRight
		{
			get { return _ToolWindowTextGapRight; }
		}

		private static int ToolWindowTabSeperatorGapTop
		{
			get { return _ToolWindowTabSeperatorGapTop; }
		}

		private static int ToolWindowTabSeperatorGapBottom
		{
			get { return _ToolWindowTabSeperatorGapBottom; }
		}

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
				TextFormatFlags textFormat = TextFormatFlags.EndEllipsis |
				                             TextFormatFlags.HorizontalCenter |
				                             TextFormatFlags.SingleLine |
				                             TextFormatFlags.VerticalCenter;
				if (RightToLeft == RightToLeft.Yes)
					return textFormat | TextFormatFlags.RightToLeft | TextFormatFlags.Right;
				else
					return textFormat;
			}
		}

		private static int DocumentStripGapTop
		{
			get { return _DocumentStripGapTop; }
		}

		private static int DocumentStripGapBottom
		{
			get { return _DocumentStripGapBottom; }
		}

		private TextFormatFlags DocumentTextFormat
		{
			get
			{
				TextFormatFlags textFormat = TextFormatFlags.PathEllipsis |
				                             TextFormatFlags.SingleLine |
				                             TextFormatFlags.VerticalCenter |
				                             TextFormatFlags.PreserveGraphicsClipping |
				                             TextFormatFlags.HorizontalCenter;
				if (RightToLeft == RightToLeft.Yes)
					return textFormat | TextFormatFlags.RightToLeft;
				else
					return textFormat;
			}
		}

		private static int DocumentTabMaxWidth
		{
			get { return _DocumentTabMaxWidth; }
		}

		private static int DocumentButtonGapTop
		{
			get { return _DocumentButtonGapTop; }
		}

		private static int DocumentButtonGapBottom
		{
			get { return _DocumentButtonGapBottom; }
		}

		private static int DocumentButtonGapBetween
		{
			get { return _DocumentButtonGapBetween; }
		}

		private static int DocumentButtonGapRight
		{
			get { return _DocumentButtonGapRight; }
		}

		private static int DocumentTabGapTop
		{
			get { return _DocumentTabGapTop; }
		}

		private static int DocumentTabGapLeft
		{
			get { return _DocumentTabGapLeft; }
		}

		private static int DocumentTabGapRight
		{
			get { return _DocumentTabGapRight; }
		}

		private static int DocumentIconGapBottom
		{
			get { return _DocumentIconGapBottom; }
		}

		private static int DocumentIconGapLeft
		{
			get { return _DocumentIconGapLeft; }
		}

		private static int DocumentIconGapRight
		{
			get { return _DocumentIconGapRight; }
		}

		private static int DocumentIconWidth
		{
			get { return _DocumentIconWidth; }
		}

		private static int DocumentIconHeight
		{
			get { return _DocumentIconHeight; }
		}

		private static int DocumentTextGapRight
		{
			get { return _DocumentTextGapRight; }
		}

		private static Pen PenToolWindowTabBorder
		{
			get { return SystemPens.GrayText; }
		}

		private static Pen PenDocumentTabActiveBorder
		{
			get { return SystemPens.GrayText; }
		}

		private static Pen PenDocumentTabInactiveBorder
		{
			get { return SystemPens.GrayText; }
		}


		private static Brush BrushToolWindowActiveBackground
		{
			get { return YukiTheme_VisualPascalABCPlugin.bgClick3Brush; }
		}

		private static Brush BrushToolWindowHoverBackground
		{
			get { return YukiTheme_VisualPascalABCPlugin.bgClickBrush; }
		}

		private static Brush BrushDocumentActiveBackground
		{
			get { return YukiTheme_VisualPascalABCPlugin.bgClick3Brush; }
		}

		private static Brush BrushDocumentInactiveBackground
		{
			get { return YukiTheme_VisualPascalABCPlugin.bgdefBrush; }
		}

		private static Color ColorToolWindowActiveText
		{
			get { return YukiTheme_VisualPascalABCPlugin.clr; }
		}

		private static Color ColorDocumentActiveText
		{
			get { return YukiTheme_VisualPascalABCPlugin.clr; }
		}

		private static Color ColorToolWindowInactiveText
		{
			get { return YukiTheme_VisualPascalABCPlugin.clr; }
		}

		private static Color ColorDocumentInactiveText
		{
			get { return YukiTheme_VisualPascalABCPlugin.clr; }
		}

		private int        currentIndex = -1;
		private List <Tab> oldTabs      = new List <Tab> ();
		private List <int> oldTabsIndex = new List <int> ();

		#endregion

		public YukiDockPaneStrip (DockPane pane) : base (pane)
		{
			SetStyle (ControlStyles.ResizeRedraw |
			          ControlStyles.UserPaint |
			          ControlStyles.AllPaintingInWmPaint |
			          ControlStyles.OptimizedDoubleBuffer, true);

			SuspendLayout ();

			m_components = new Container ();
			m_toolTip = new ToolTip (Components);
			m_selectMenu = new ContextMenuStrip (Components);

			ResumeLayout ();
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing)
			{
				Components.Dispose ();
				if (m_boldFont != null)
				{
					m_boldFont.Dispose ();
					m_boldFont = null;
				}
			}

			base.Dispose (disposing);
		}

		private static Font TextFont
		{
			get { return SystemInformation.MenuFont; }
		}

		private Font m_font;
		private Font m_boldFont;

		private Font BoldFont
		{
			get
			{
				if (IsDisposed)
					return null;

				if (m_boldFont == null)
				{
					m_font = TextFont;
					m_boldFont = new Font (TextFont, FontStyle.Bold);
				} else if (m_font != TextFont)
				{
					m_boldFont.Dispose ();
					m_font = TextFont;
					m_boldFont = new Font (TextFont, FontStyle.Bold);
				}

				return m_boldFont;
			}
		}

		private int m_startDisplayingTab = 0;

		private int StartDisplayingTab
		{
			get { return m_startDisplayingTab; }
			set
			{
				m_startDisplayingTab = value;
				Invalidate ();
			}
		}

		private int m_endDisplayingTab = 0;

		private int EndDisplayingTab
		{
			get { return m_endDisplayingTab; }
			set { m_endDisplayingTab = value; }
		}

		private bool m_documentTabsOverflow = false;

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

		protected override int MeasureHeight ()
		{
			if (Appearance == DockPane.AppearanceStyle.ToolWindow)
				return MeasureHeight_ToolWindow ();
			else
				return MeasureHeight_Document ();
		}

		private int MeasureHeight_ToolWindow ()
		{
			if (DockPane.IsAutoHide || Tabs.Count <= 1)
				return 0;

			int height = Math.Max (TextFont.Height,
			                       ToolWindowImageHeight + ToolWindowImageGapTop + ToolWindowImageGapBottom)
			           + ToolWindowStripGapTop + ToolWindowStripGapBottom;

			return height;
		}

		private int MeasureHeight_Document ()
		{
			int height = Math.Max (TextFont.Height + DocumentTabGapTop,
			                       ButtonClose.Height + DocumentButtonGapTop + DocumentButtonGapBottom)
			           + DocumentStripGapBottom + DocumentStripGapTop;

			return height;
		}

		protected override void OnPaint (PaintEventArgs e)
		{
			if (BackColor != YukiTheme_VisualPascalABCPlugin.bgdef)
				BackColor = YukiTheme_VisualPascalABCPlugin.bgdef;
			base.OnPaint (e);
			CalculateTabs ();
			if (Appearance == DockPane.AppearanceStyle.Document && DockPane.ActiveContent != null)
			{
				if (EnsureDocumentTabVisible (DockPane.ActiveContent, false))
					CalculateTabs ();
			}
			
			
			DrawTabStrip (e.Graphics);
		}

		protected override void OnRefreshChanges ()
		{
			SetInertButtons ();
			Invalidate ();
		}

		protected override GraphicsPath GetOutline (int index)
		{
			if (Appearance == DockPane.AppearanceStyle.Document)
				return GetOutline_Document (index);
			else
				return GetOutline_ToolWindow (index);
		}

		private GraphicsPath GetOutline_Document (int index)
		{
			Rectangle rectTab = GetTabRectangle (index);
			rectTab.X -= rectTab.Height / 2;
			rectTab.Intersect (TabsRectangle);
			rectTab = RectangleToScreen (DrawHelper.RtlTransform (this, rectTab));
			int y = rectTab.Top;
			Rectangle rectPaneClient = DockPane.RectangleToScreen (DockPane.ClientRectangle);

			GraphicsPath path = new GraphicsPath ();
			GraphicsPath pathTab = GetTabOutline_Document (Tabs [index], true, true, true);
			path.AddPath (pathTab, true);
			path.AddLine (rectTab.Right, rectTab.Bottom, rectPaneClient.Right, rectTab.Bottom);
			path.AddLine (rectPaneClient.Right, rectTab.Bottom, rectPaneClient.Right, rectPaneClient.Bottom);
			path.AddLine (rectPaneClient.Right, rectPaneClient.Bottom, rectPaneClient.Left, rectPaneClient.Bottom);
			path.AddLine (rectPaneClient.Left, rectPaneClient.Bottom, rectPaneClient.Left, rectTab.Bottom);
			path.AddLine (rectPaneClient.Left, rectTab.Bottom, rectTab.Right, rectTab.Bottom);
			return path;
		}

		private GraphicsPath GetOutline_ToolWindow (int index)
		{
			Rectangle rectTab = GetTabRectangle (index);
			rectTab.Intersect (TabsRectangle);
			rectTab = RectangleToScreen (DrawHelper.RtlTransform (this, rectTab));
			int y = rectTab.Top;
			Rectangle rectPaneClient = DockPane.RectangleToScreen (DockPane.ClientRectangle);

			GraphicsPath path = new GraphicsPath ();
			GraphicsPath pathTab = GetTabOutline (Tabs [index], true, true);
			path.AddPath (pathTab, true);
			path.AddLine (rectTab.Left, rectTab.Top, rectPaneClient.Left, rectTab.Top);
			path.AddLine (rectPaneClient.Left, rectTab.Top, rectPaneClient.Left, rectPaneClient.Top);
			path.AddLine (rectPaneClient.Left, rectPaneClient.Top, rectPaneClient.Right, rectPaneClient.Top);
			path.AddLine (rectPaneClient.Right, rectPaneClient.Top, rectPaneClient.Right, rectTab.Top);
			path.AddLine (rectPaneClient.Right, rectTab.Top, rectTab.Right, rectTab.Top);
			return path;
		}

		private void CalculateTabs ()
		{
			if (Appearance == DockPane.AppearanceStyle.ToolWindow)
				CalculateTabs_ToolWindow ();
			else
				CalculateTabs_Document ();
		}

		private void CalculateTabs_ToolWindow ()
		{
			if (Tabs.Count <= 1 || DockPane.IsAutoHide)
				return;

			Rectangle rectTabStrip = TabStripRectangle;

			// Calculate tab widths
			int countTabs = Tabs.Count;
			foreach (Tab tab in Tabs)
			{
				PropertyInfo tbw = tab.GetType ()
				                      .GetProperty ("MaxWidth", BindingFlags.Public | BindingFlags.Instance);
				tbw.SetValue (tab, GetMaxTabWidth (Tabs.IndexOf (tab)));

				PropertyInfo tbf = tab.GetType ()
				                      .GetProperty ("Flag", BindingFlags.NonPublic | BindingFlags.Instance);
				tbf.SetValue (tab, false);
			}

			// Set tab whose max width less than average width
			bool anyWidthWithinAverage = true;
			int totalWidth = rectTabStrip.Width - ToolWindowStripGapLeft - ToolWindowStripGapRight;
			int totalAllocatedWidth = 0;
			int averageWidth = totalWidth / countTabs;
			int remainedTabs = countTabs;
			for (anyWidthWithinAverage = true; anyWidthWithinAverage && remainedTabs > 0;)
			{
				anyWidthWithinAverage = false;
				foreach (Tab tab in Tabs)
				{
					PropertyInfo tbf = tab.GetType ()
					                      .GetProperty ("Flag", BindingFlags.NonPublic | BindingFlags.Instance);

					if ((bool) tbf.GetValue (tab))
						continue;
					PropertyInfo tbm = tab.GetType ()
					                      .GetProperty ("MaxWidth", BindingFlags.Public | BindingFlags.Instance);

					if ((int) tbm.GetValue (tab) <= averageWidth)
					{
						tbf.SetValue (tab, true);
						PropertyInfo tbw = tab.GetType ()
						                      .GetProperty ("TabWidth", BindingFlags.Public | BindingFlags.Instance);
						tbw.SetValue (tab, (int) tbm.GetValue (tab));
						totalAllocatedWidth += (int) tbm.GetValue (tab);
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
				int roundUpWidth = (totalWidth - totalAllocatedWidth) - (averageWidth * remainedTabs);
				foreach (Tab tab in Tabs)
				{
					PropertyInfo tbf = tab.GetType ()
					                      .GetProperty ("Flag", BindingFlags.NonPublic | BindingFlags.Instance);

					if ((bool) tbf.GetValue (tab))
						continue;

					tbf.SetValue (tab, true);
					PropertyInfo tbw = tab.GetType ()
					                      .GetProperty ("TabWidth", BindingFlags.Public | BindingFlags.Instance);
					if (roundUpWidth > 0)
					{
						tbw.SetValue (tab, averageWidth + 1);
						roundUpWidth--;
					} else
						tbw.SetValue (tab, averageWidth);
				}
			}

			// Set the X position of the tabs
			int x = rectTabStrip.X + ToolWindowStripGapLeft;
			foreach (Tab tab in Tabs)
			{
				PropertyInfo tbx = tab.GetType ()
				                      .GetProperty ("TabX", BindingFlags.Public | BindingFlags.Instance);
				tbx.SetValue (tab, x);
				PropertyInfo tbw = tab.GetType ()
				                      .GetProperty ("TabWidth", BindingFlags.Public | BindingFlags.Instance);
				x += (int) tbw.GetValue (tab);
			}
		}

		private bool CalculateDocumentTab (Rectangle rectTabStrip, ref int x, int index)
		{
			bool overflow = false;

			Tab tab = Tabs [index];
			PropertyInfo tbm = tab.GetType ()
			                      .GetProperty ("MaxWidth", BindingFlags.Public | BindingFlags.Instance);
			tbm.SetValue (tab, GetMaxTabWidth (index));
			int width = Math.Min ((int) tbm.GetValue (tab), DocumentTabMaxWidth);

			PropertyInfo tbx = tab.GetType ()
			                      .GetProperty ("TabX", BindingFlags.Public | BindingFlags.Instance);

			PropertyInfo tbw = tab.GetType ()
			                      .GetProperty ("TabWidth", BindingFlags.Public | BindingFlags.Instance);
			if (x + width < rectTabStrip.Right || index == StartDisplayingTab)
			{
				tbx.SetValue (tab, x);
				tbw.SetValue (tab, width);
				EndDisplayingTab = index;
			} else
			{
				tbx.SetValue (tab, 0);
				tbw.SetValue (tab, 0);
				overflow = true;
			}

			x += width;

			return overflow;
		}

		private void CalculateTabs_Document ()
		{
			if (m_startDisplayingTab >= Tabs.Count)
				m_startDisplayingTab = 0;

			Rectangle rectTabStrip = TabsRectangle;

			int x = rectTabStrip.X + rectTabStrip.Height / 2;

			bool overflow = false;
			for (int i = StartDisplayingTab; i < Tabs.Count; i++)
				overflow = CalculateDocumentTab (rectTabStrip, ref x, i);

			for (int i = 0; i < StartDisplayingTab; i++)
				overflow = CalculateDocumentTab (rectTabStrip, ref x, i);

			if (!overflow)
			{
				m_startDisplayingTab = 0;
				x = rectTabStrip.X;
				foreach (Tab tab in Tabs)
				{
					PropertyInfo tbx = tab.GetType ()
					                      .GetProperty ("TabX", BindingFlags.Public | BindingFlags.Instance);
					tbx.SetValue (tab, x);

					PropertyInfo tbw = tab.GetType ()
					                      .GetProperty ("TabWidth", BindingFlags.Public | BindingFlags.Instance);

					x += (int) tbw.GetValue (tab);
				}
			}

			DocumentTabsOverflow = overflow;
		}

		protected override void EnsureTabVisible (IDockContent content)
		{
			if (Appearance != DockPane.AppearanceStyle.Document || !Tabs.Contains (content))
				return;

			CalculateTabs ();
			EnsureDocumentTabVisible (content, true);
		}

		private bool EnsureDocumentTabVisible (IDockContent content, bool repaint)
		{
			int index = Tabs.IndexOf (content);
			Tab tab = Tabs [index];
			PropertyInfo tbw = tab.GetType ()
			                      .GetProperty ("TabWidth", BindingFlags.Public | BindingFlags.Instance);
			if ((int) tbw.GetValue (tab) != 0)
				return false;

			StartDisplayingTab = index;
			if (repaint)
				Invalidate ();

			return true;
		}

		private int GetMaxTabWidth (int index)
		{
			if (Appearance == DockPane.AppearanceStyle.ToolWindow)
				return GetMaxTabWidth_ToolWindow (index);
			else
				return GetMaxTabWidth_Document (index);
		}

		private int GetMaxTabWidth_ToolWindow (int index)
		{
			IDockContent content = Tabs [index].Content;
			Size sizeString = TextRenderer.MeasureText (content.DockHandler.TabText, TextFont);
			return ToolWindowImageWidth + sizeString.Width + ToolWindowImageGapLeft
			     + ToolWindowImageGapRight + ToolWindowTextGapRight;
		}

		private int GetMaxTabWidth_Document (int index)
		{
			IDockContent content = Tabs [index].Content;

			int height = GetTabRectangle_Document (index).Height;

			Size sizeText = TextRenderer.MeasureText (content.DockHandler.TabText, BoldFont,
			                                          new Size (DocumentTabMaxWidth, height), DocumentTextFormat);

			if (DockPane.DockPanel.ShowDocumentIcon)
				return sizeText.Width + DocumentIconWidth + DocumentIconGapLeft + DocumentIconGapRight +
				       DocumentTextGapRight;
			else
				return sizeText.Width + DocumentIconGapLeft + DocumentTextGapRight;
		}

		private void DrawTabStrip (Graphics g)
		{
			if (Appearance == DockPane.AppearanceStyle.Document)
				DrawTabStrip_Document (g);
			else
				DrawTabStrip_ToolWindow (g);
		}

		private void DrawTabStrip_Document (Graphics g)
		{
			int count = Tabs.Count;
			if (count == 0)
				return;

			Rectangle rectTabStrip = TabStripRectangle;

			// Draw the tabs
			Rectangle rectTabOnly = TabsRectangle;
			Rectangle rectTab = Rectangle.Empty;
			Tab tabActive = null;
			g.SetClip (DrawHelper.RtlTransform (this, rectTabOnly));
			for (int i = 0; i < count; i++)
			{
				rectTab = GetTabRectangle (i);
				if (Tabs [i].Content == DockPane.ActiveContent)
				{
					tabActive = Tabs [i];
					continue;
				}

				if (rectTab.IntersectsWith (rectTabOnly))
					DrawTab (g, Tabs [i], rectTab);
			}


			g.SetClip (rectTabStrip);
			g.DrawLine (PenDocumentTabActiveBorder, rectTabStrip.Left, rectTabStrip.Bottom - 1,
			            rectTabStrip.Right, rectTabStrip.Bottom - 1);


			g.SetClip (DrawHelper.RtlTransform (this, rectTabOnly));
			if (tabActive != null)
			{
				rectTab = GetTabRectangle (Tabs.IndexOf (tabActive));
				if (rectTab.IntersectsWith (rectTabOnly))
					DrawTab (g, tabActive, rectTab);
				if (DockPane.IsActiveDocumentPane)
					g.DrawLine (YukiTheme_VisualPascalABCPlugin.bgBorderPen, rectTab.Left, rectTab.Bottom,
					            rectTab.Right, rectTab.Bottom);
			}
		}

		private void DrawTabStrip_ToolWindow (Graphics g)
		{
			g.DrawRectangle (PenToolWindowTabBorder, TabStripRectangle);

			for (int i = 0; i < Tabs.Count; i++)
				DrawTab (g, Tabs [i], GetTabRectangle (i));
		}

		private Rectangle GetTabRectangle (int index)
		{
			if (Appearance == DockPane.AppearanceStyle.ToolWindow)
				return GetTabRectangle_ToolWindow (index);
			else
				return GetTabRectangle_Document (index);
		}

		private Rectangle GetTabRectangle_ToolWindow (int index)
		{
			Rectangle rectTabStrip = TabStripRectangle;

			Tab tab = Tabs [index];
			PropertyInfo tbx = tab.GetType ()
			                      .GetProperty ("TabX", BindingFlags.Public | BindingFlags.Instance);

			PropertyInfo tbw = tab.GetType ()
			                      .GetProperty ("TabWidth", BindingFlags.Public | BindingFlags.Instance);
			return new Rectangle ((int) tbx.GetValue (tab), rectTabStrip.Y, (int) tbw.GetValue (tab),
			                      rectTabStrip.Height);
		}

		private Rectangle GetTabRectangle_Document (int index)
		{
			Rectangle rectTabStrip = TabStripRectangle;
			Tab tab = Tabs [index];
			PropertyInfo tbx = tab.GetType ()
			                      .GetProperty ("TabX", BindingFlags.Public | BindingFlags.Instance);

			PropertyInfo tbw = tab.GetType ()
			                      .GetProperty ("TabWidth", BindingFlags.Public | BindingFlags.Instance);

			return new Rectangle ((int) tbx.GetValue (tab), rectTabStrip.Y + DocumentTabGapTop,
			                      (int) tbw.GetValue (tab), rectTabStrip.Height - DocumentTabGapTop);
		}

		private void DrawTab (Graphics g, Tab tab, Rectangle rect)
		{
			if (Appearance == DockPane.AppearanceStyle.ToolWindow)
				DrawTab_ToolWindow (g, tab, rect);
			else
				DrawTab_Document (g, tab, rect);
		}

		private GraphicsPath GetTabOutline (Tab tab, bool rtlTransform, bool toScreen)
		{
			if (Appearance == DockPane.AppearanceStyle.ToolWindow)
				return GetTabOutline_ToolWindow (tab, rtlTransform, toScreen);
			else
				return GetTabOutline_Document (tab, rtlTransform, toScreen, false);
		}

		private GraphicsPath GetTabOutline_ToolWindow (Tab tab, bool rtlTransform, bool toScreen)
		{
			Rectangle rect = GetTabRectangle (Tabs.IndexOf (tab));
			if (rtlTransform)
				rect = DrawHelper.RtlTransform (this, rect);
			if (toScreen)
				rect = RectangleToScreen (rect);

			DrawHelper.GetRoundedCornerTab (GraphicsPath, rect, false);
			return GraphicsPath;
		}

		private GraphicsPath GetTabOutline_Document (Tab tab, bool rtlTransform, bool toScreen, bool full)
		{
			GraphicsPath.Reset ();
			Rectangle rect = GetTabRectangle (Tabs.IndexOf (tab));

			// Shorten TabOutline so it doesn't get overdrawn by icons next to it
			rect.Intersect (TabsRectangle);
			rect.Width--;

			if (rtlTransform)
				rect = DrawHelper.RtlTransform (this, rect);
			if (toScreen)
				rect = RectangleToScreen (rect);

			GraphicsPath.AddRectangle (rect);
			return GraphicsPath;
		}


		private void DrawTab_ToolWindow (Graphics g, Tab tab, Rectangle rect)
		{
			Rectangle rectIcon = new Rectangle (
				rect.X + ToolWindowImageGapLeft,
				rect.Y + rect.Height - 1 - ToolWindowImageGapBottom - ToolWindowImageHeight,
				ToolWindowImageWidth, ToolWindowImageHeight);
			Rectangle rectText = rectIcon;
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

			Rectangle rectTab = DrawHelper.RtlTransform (this, rect);
			rectText = DrawHelper.RtlTransform (this, rectText);
			rectIcon = DrawHelper.RtlTransform (this, rectIcon);
			GraphicsPath path = GetTabOutline (tab, true, false);
			if (DockPane.ActiveContent == tab.Content)
			{
				g.FillPath (BrushToolWindowActiveBackground, path);
				TextRenderer.DrawText (g, tab.Content.DockHandler.TabText, TextFont, rectText,
				                       ColorToolWindowActiveText, ToolWindowTextFormat);
			} else
			{
				if (oldTabs.Contains (tab))
				{
					g.FillPath (BrushToolWindowHoverBackground, path);
				}

				TextRenderer.DrawText (g, tab.Content.DockHandler.TabText, TextFont, rectText,
				                       ColorToolWindowInactiveText, ToolWindowTextFormat);
			}

			if (rectTab.Contains (rectIcon))
				g.DrawIcon (tab.Content.DockHandler.Icon, rectIcon);
		}

		private void DrawTab_Document (Graphics g, Tab tab, Rectangle rect)
		{
			PropertyInfo tbw = tab.GetType ()
			                      .GetProperty ("TabWidth", BindingFlags.Public | BindingFlags.Instance);
			if ((int) tbw.GetValue (tab) == 0)
				return;

			Rectangle rectIcon = new Rectangle (
				rect.X + DocumentIconGapLeft,
				rect.Y + rect.Height - 1 - DocumentIconGapBottom - DocumentIconHeight + 1,
				DocumentIconWidth, DocumentIconHeight);
			Rectangle rectText = rectIcon;
			if (DockPane.DockPanel.ShowDocumentIcon)
			{
				rectText.X += rectIcon.Width + DocumentIconGapRight;
				rectText.Y = rect.Y;
				rectText.Width = rect.Width - rectIcon.Width - DocumentIconGapLeft -
				                 DocumentIconGapRight - DocumentTextGapRight;
				rectText.Height = rect.Height;
			} else
			{
				rectText.Width = rect.Width - DocumentIconGapLeft - DocumentTextGapRight;
			}

			Font fnt = TextFont;
			if (DockPane.IsActiveDocumentPane)
				fnt = BoldFont;
			rectText.Y = rect.Y + Convert.ToInt32 ((rect.Height - fnt.Height) / 2) - 1;
			rectText.Height = fnt.Height + 1;
			Rectangle rectTab = DrawHelper.RtlTransform (this, rect);
			rectText = DrawHelper.RtlTransform (this, rectText);
			rectIcon = DrawHelper.RtlTransform (this, rectIcon);
			GraphicsPath path = GetTabOutline (tab, true, false);
			if (DockPane.ActiveContent == tab.Content)
			{
				g.FillPath (BrushDocumentActiveBackground, path);
				// g.DrawPath(PenDocumentTabActiveBorder, path);
				if (DockPane.IsActiveDocumentPane)
					TextRenderer.DrawText (g, tab.Content.DockHandler.TabText, BoldFont, rectText,
					                       ColorDocumentActiveText, DocumentTextFormat);
				else
					TextRenderer.DrawText (g, tab.Content.DockHandler.TabText, TextFont, rectText,
					                       ColorDocumentActiveText, DocumentTextFormat);
			} else
			{
				if (oldTabs.Contains (tab))
					g.FillPath (BrushToolWindowHoverBackground, path);
				else
					g.FillPath (BrushDocumentInactiveBackground, path);
				// g.DrawPath(PenDocumentTabInactiveBorder, path);
				TextRenderer.DrawText (g, tab.Content.DockHandler.TabText, TextFont, rectText,
				                       ColorDocumentInactiveText, DocumentTextFormat);
			}

			if (rectTab.Contains (rectIcon) && DockPane.DockPanel.ShowDocumentIcon)
				g.DrawIcon (tab.Content.DockHandler.Icon, rectIcon);
		}

		private Rectangle TabStripRectangle
		{
			get
			{
				if (Appearance == DockPane.AppearanceStyle.Document)
					return TabStripRectangle_Document;
				else
					return TabStripRectangle_ToolWindow;
			}
		}

		private Rectangle TabStripRectangle_ToolWindow
		{
			get
			{
				Rectangle rect = ClientRectangle;
				return new Rectangle (rect.X, rect.Top + ToolWindowStripGapTop, rect.Width,
				                      rect.Height - ToolWindowStripGapTop - ToolWindowStripGapBottom);
			}
		}

		private Rectangle TabStripRectangle_Document
		{
			get
			{
				Rectangle rect = ClientRectangle;
				return new Rectangle (rect.X, rect.Top + DocumentStripGapTop, rect.Width,
				                      rect.Height - DocumentStripGapTop - ToolWindowStripGapBottom);
			}
		}

		private Rectangle TabsRectangle
		{
			get
			{
				if (Appearance == DockPane.AppearanceStyle.ToolWindow)
					return TabStripRectangle;

				Rectangle rectWindow = TabStripRectangle;
				int x = rectWindow.X;
				int y = rectWindow.Y;
				int width = rectWindow.Width;
				int height = rectWindow.Height;

				x += DocumentTabGapLeft;
				width -= DocumentTabGapLeft +
				         DocumentTabGapRight +
				         DocumentButtonGapRight +
				         ButtonClose.Width +
				         ButtonWindowList.Width +
				         2 * DocumentButtonGapBetween;

				return new Rectangle (x, y, width, height);
			}
		}

		private ContextMenuStrip m_selectMenu;

		private ContextMenuStrip SelectMenu
		{
			get { return m_selectMenu; }
		}

		private void WindowList_Click (object sender, EventArgs e)
		{
			int x = 0;
			int y = ButtonWindowList.Location.Y + ButtonWindowList.Height;

			SelectMenu.Items.Clear ();
			foreach (Tab tab in Tabs)
			{
				IDockContent content = tab.Content;
				ToolStripItem item =
					SelectMenu.Items.Add (content.DockHandler.TabText, content.DockHandler.Icon.ToBitmap ());
				item.Tag = tab.Content;
				item.Click += new EventHandler (ContextMenuItem_Click);
			}

			SelectMenu.Show (ButtonWindowList, x, y);
		}

		private void ContextMenuItem_Click (object sender, EventArgs e)
		{
			ToolStripMenuItem item = sender as ToolStripMenuItem;
			if (item != null)
			{
				IDockContent content = (IDockContent) item.Tag;
				DockPane.ActiveContent = content;
			}
		}

		private void SetInertButtons ()
		{
			if (Appearance == DockPane.AppearanceStyle.ToolWindow)
			{
				if (m_buttonClose != null)
					m_buttonClose.Left = -m_buttonClose.Width;

				if (m_buttonWindowList != null)
					m_buttonWindowList.Left = -m_buttonWindowList.Width;
			} else
			{
				bool showCloseButton = DockPane.ActiveContent == null
					? true
					: DockPane.ActiveContent.DockHandler.CloseButton;
				ButtonClose.Enabled = showCloseButton;
				ButtonClose.RefreshChanges ();
				ButtonWindowList.RefreshChanges ();
			}
		}

		protected override void OnLayout (LayoutEventArgs levent)
		{
			if (Appearance != DockPane.AppearanceStyle.Document)
			{
				base.OnLayout (levent);
				return;
			}

			Rectangle rectTabStrip = TabStripRectangle;

			// Set position and size of the buttons
			int buttonWidth = ButtonClose.Image.Width;
			int buttonHeight = ButtonClose.Image.Height;
			int height = rectTabStrip.Height - DocumentButtonGapTop - DocumentButtonGapBottom;
			if (buttonHeight < height)
			{
				buttonWidth = buttonWidth * (height / buttonHeight);
				buttonHeight = height;
			}

			Size buttonSize = new Size (buttonWidth, buttonHeight);

			int x = rectTabStrip.X + rectTabStrip.Width - DocumentTabGapLeft
			                                            - DocumentButtonGapRight - buttonWidth;
			int y = rectTabStrip.Y + DocumentButtonGapTop;
			Point point = new Point (x, y);
			ButtonClose.Bounds = DrawHelper.RtlTransform (this, new Rectangle (point, buttonSize));
			point.Offset (-(DocumentButtonGapBetween + buttonWidth), 0);
			ButtonWindowList.Bounds = DrawHelper.RtlTransform (this, new Rectangle (point, buttonSize));

			OnRefreshChanges ();

			base.OnLayout (levent);
		}

		private void Close_Click (object sender, EventArgs e)
		{
			DockPane.CloseActiveContent ();
		}

		protected override int HitTest (Point ptMouse)
		{
			Rectangle rectTabStrip = TabsRectangle;
			if (!TabsRectangle.Contains (ptMouse))
				return -1;

			foreach (Tab tab in Tabs)
			{
				GraphicsPath path = GetTabOutline (tab, true, false);
				if (path.IsVisible (ptMouse))
					return Tabs.IndexOf (tab);
			}

			return -1;
		}

		protected override void OnMouseHover (EventArgs e)
		{
			int index = HitTest (PointToClient (Control.MousePosition));
			string toolTip = string.Empty;

			base.OnMouseHover (e);

			if (index != -1)
			{
				Tab tab = Tabs [index];

				PropertyInfo tbm = tab.GetType ()
				                      .GetProperty ("MaxWidth", BindingFlags.Public | BindingFlags.Instance);

				PropertyInfo tbw = tab.GetType ()
				                      .GetProperty ("TabWidth", BindingFlags.Public | BindingFlags.Instance);

				if (!String.IsNullOrEmpty (tab.Content.DockHandler.ToolTipText))
					toolTip = tab.Content.DockHandler.ToolTipText;
				else if ((int) tbm.GetValue (tab) > (int) tbw.GetValue (tab))
					toolTip = tab.Content.DockHandler.TabText;
			}

			if (m_toolTip.GetToolTip (this) != toolTip)
			{
				m_toolTip.Active = false;
				m_toolTip.SetToolTip (this, toolTip);
				m_toolTip.Active = true;
			}

			// requires further tracking of mouse hover behavior,
			ResetMouseEventArgs ();
		}

		protected override void OnRightToLeftChanged (EventArgs e)
		{
			base.OnRightToLeftChanged (e);
			PerformLayout ();
		}

		protected override void OnMouseMove (MouseEventArgs e)
		{
			int index = HitTest (PointToClient (Control.MousePosition));

			if (index != -1)
			{
				if (currentIndex != index && DockPane.ActiveContent != Tabs [index].Content)
				{
					CleanCurrentTab ();
					currentIndex = index;
					oldTabs.Add (Tabs [index]);
					oldTabsIndex.Add (index);
					Invalidate (GetTabRectangle (index));
				}
			} else
			{
				CleanCurrentTab ();
			}

			base.OnMouseMove (e);
		}


		protected override void OnMouseLeave (EventArgs e)
		{
			base.OnMouseLeave (e);
			CleanCurrentTab ();
		}

		private void CleanCurrentTab ()
		{
			for (var index = 0; index < oldTabsIndex.Count; index++)
			{
				if (currentIndex == oldTabsIndex [index])
				{
					currentIndex = -1;
				}

				oldTabs.Remove (Tabs [oldTabsIndex [index]]);
				Invalidate (GetTabRectangle (oldTabsIndex [index]));
				oldTabsIndex.Remove (oldTabsIndex [index]);
			}

			if (currentIndex != -1)
			{
				Invalidate (GetTabRectangle (currentIndex));
				currentIndex = -1;
			}
		}
	}
}