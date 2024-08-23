using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using YukiTheme.Engine;
using YukiTheme.Style.Helpers;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace YukiTheme.Style;

public static class ScreenScale
{
	private static double scale = -1;

	public static double Calc()
	{
		if (scale > 0)
			return scale;
		var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
		var dpiX = (int)dpiXProperty.GetValue(null, null);
		scale = (double)dpiX / 96;
		return scale;
	}
}

internal class YukiDockPaneCaption : DockPaneCaptionBase
{
	private static Bitmap _imageButtonClose;

	private static Bitmap _imageButtonAutoHide;

	private static Bitmap _imageButtonDock;

	private static Bitmap _imageButtonOptions;

	private static string _toolTipClose;

	private static string _toolTipOptions;

	private static string _toolTipAutoHide;

	private static Blend _activeBackColorGradientBlend;

	private static readonly TextFormatFlags _textFormat =
		TextFormatFlags.SingleLine |
		TextFormatFlags.EndEllipsis |
		TextFormatFlags.VerticalCenter;

	private InertButton m_buttonAutoHide;

	private InertButton m_buttonClose;

	private InertButton m_buttonOptions;

	private readonly ToolTip m_toolTip;

	public YukiDockPaneCaption(DockPane pane) : base(pane)
	{
		SuspendLayout();

		Components = new Container();
		m_toolTip = new ToolTip(Components);

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
				m_buttonClose = new InertButton(this, ImageButtonClose, ImageButtonClose);
				m_toolTip.SetToolTip(m_buttonClose, ToolTipClose);
				m_buttonClose.Click += Close_Click;
				Controls.Add(m_buttonClose);
			}

			return m_buttonClose;
		}
	}

	private static Bitmap ImageButtonAutoHide
	{
		get
		{
			if (_imageButtonAutoHide == null)
			{
				var sc = ScreenScale.Calc();
				if (sc >= 1.99)
					_imageButtonAutoHide = Resources.DockPane_AutoHide32;
				else _imageButtonAutoHide = Resources.DockPane_AutoHide;
			}

			return _imageButtonAutoHide;
		}
	}

	private static Bitmap ImageButtonDock
	{
		get
		{
			if (_imageButtonDock == null)
			{
				var sc = ScreenScale.Calc();
				if (sc >= 1.99)
					_imageButtonDock = Resources.DockPane_Dock32;
				else _imageButtonDock = Resources.DockPane_Dock;
			}


			return _imageButtonDock;
		}
	}

	private InertButton ButtonAutoHide
	{
		get
		{
			if (m_buttonAutoHide == null)
			{
				m_buttonAutoHide = new InertButton(this, ImageButtonDock, ImageButtonAutoHide);
				m_toolTip.SetToolTip(m_buttonAutoHide, ToolTipAutoHide);
				m_buttonAutoHide.Click += AutoHide_Click;
				Controls.Add(m_buttonAutoHide);
			}

			return m_buttonAutoHide;
		}
	}

	private static Bitmap ImageButtonOptions
	{
		get
		{
			if (_imageButtonOptions == null)
			{
				var sc = ScreenScale.Calc();
				if (sc >= 1.99)
					_imageButtonOptions = Resources.DockPane_Option32;
				else _imageButtonOptions = Resources.DockPane_Option;
			}

			return _imageButtonOptions;
		}
	}

	private InertButton ButtonOptions
	{
		get
		{
			if (m_buttonOptions == null)
			{
				m_buttonOptions = new InertButton(this, ImageButtonOptions, ImageButtonOptions);
				m_toolTip.SetToolTip(m_buttonOptions, ToolTipOptions);
				m_buttonOptions.Click += Options_Click;
				Controls.Add(m_buttonOptions);
			}

			return m_buttonOptions;
		}
	}

	private IContainer Components { get; }

	private static int TextGapTop => _TextGapTop;

	private static Font TextFont => SystemInformation.MenuFont;

	private static int TextGapBottom => _TextGapBottom;

	private static int TextGapLeft => _TextGapLeft;

	private static int TextGapRight => _TextGapRight;

	private static int ButtonGapTop => _ButtonGapTop;

	private static int ButtonGapBottom => _ButtonGapBottom;

	private static int ButtonGapLeft => _ButtonGapLeft;

	private static int ButtonGapRight => _ButtonGapRight;

	private static int ButtonGapBetween => _ButtonGapBetween;

	private static string ToolTipClose
	{
		get
		{
			if (_toolTipClose == null)
				_toolTipClose = Strings.DockPaneCaption_ToolTipClose;
			return _toolTipClose;
		}
	}

	private static string ToolTipOptions
	{
		get
		{
			if (_toolTipOptions == null)
				_toolTipOptions = Strings.DockPaneCaption_ToolTipOptions;

			return _toolTipOptions;
		}
	}

	private static string ToolTipAutoHide
	{
		get
		{
			if (_toolTipAutoHide == null)
				_toolTipAutoHide = Strings.DockPaneCaption_ToolTipAutoHide;
			return _toolTipAutoHide;
		}
	}

	private static Blend ActiveBackColorGradientBlend
	{
		get
		{
			if (_activeBackColorGradientBlend == null)
			{
				var blend = new Blend(2);

				blend.Factors = new[] { 0F, 0.8F };
				blend.Positions = new[] { 0.0F, 1.0F };
				_activeBackColorGradientBlend = blend;
			}

			return _activeBackColorGradientBlend;
		}
	}

	private static Color ActiveBackColorGradientBegin => ColorReference.BackgroundInactiveColor;

	private static Color ActiveBackColorGradientEnd => ColorReference.BackgroundInactiveColor;

	private static Color InactiveBackColor => ColorReference.BackgroundColor;

	private static Color ActiveTextColor => ColorReference.ForegroundColor;

	private static Color InactiveTextColor => ColorReference.ForegroundHoverColor;

	private Color TextColor => DockPane.IsActivated ? ActiveTextColor : InactiveTextColor;

	private TextFormatFlags TextFormat
	{
		get
		{
			if (RightToLeft == RightToLeft.No)
				return _textFormat;
			return _textFormat | TextFormatFlags.RightToLeft | TextFormatFlags.Right;
		}
	}

	private bool CloseButtonEnabled => DockPane.ActiveContent != null ? DockPane.ActiveContent.DockHandler.CloseButton : false;

	private bool ShouldShowAutoHideButton => !DockPane.IsFloat;

	protected override void Dispose(bool disposing)
	{
		if (disposing)
			Components.Dispose();
		base.Dispose(disposing);
	}

	protected override int MeasureHeight()
	{
		var height = TextFont.Height + TextGapTop + TextGapBottom;

		if (height < ButtonClose.Image.Height + ButtonGapTop + ButtonGapBottom)
			height = ButtonClose.Image.Height + ButtonGapTop + ButtonGapBottom;

		return height;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		DrawCaption(e.Graphics);
	}

	private void DrawCaption(Graphics g)
	{
		if (ClientRectangle.Width == 0 || ClientRectangle.Height == 0)
			return;

		if (DockPane.IsActivated)
			using (var brush = new LinearGradientBrush(ClientRectangle, ActiveBackColorGradientEnd, ActiveBackColorGradientBegin, LinearGradientMode.Horizontal))
			{
				brush.Blend = ActiveBackColorGradientBlend;
				g.FillRectangle(brush, ClientRectangle);
			}
		else
			using (var brush = new SolidBrush(InactiveBackColor))
			{
				g.FillRectangle(brush, ClientRectangle);
			}

		var rectCaption = ClientRectangle;

		var rectCaptionText = rectCaption;
		rectCaptionText.X += TextGapLeft;
		rectCaptionText.Width -= TextGapLeft + TextGapRight;
		rectCaptionText.Width -= ButtonGapLeft + ButtonClose.Width + ButtonGapRight;
		if (ShouldShowAutoHideButton)
			rectCaptionText.Width -= ButtonAutoHide.Width + ButtonGapBetween;
		if (HasTabPageContextMenu)
			rectCaptionText.Width -= ButtonOptions.Width + ButtonGapBetween;
		rectCaptionText.Y += TextGapTop;
		rectCaptionText.Height -= TextGapTop + TextGapBottom - 1;
		TextRenderer.DrawText(g, DockPane.CaptionText, TextFont, DrawHelper.RtlTransform(this, rectCaptionText), TextColor, TextFormat);
	}

	protected override void OnLayout(LayoutEventArgs levent)
	{
		SetButtonsPosition();
		base.OnLayout(levent);
	}

	protected override void OnRefreshChanges()
	{
		SetButtons();
		Invalidate();
	}

	private void SetButtons()
	{
		ButtonClose.Enabled = CloseButtonEnabled;
		ButtonAutoHide.Visible = ShouldShowAutoHideButton;
		ButtonOptions.Visible = HasTabPageContextMenu;
		ButtonClose.RefreshChanges();
		ButtonAutoHide.RefreshChanges();
		ButtonOptions.RefreshChanges();

		SetButtonsPosition();
	}

	private void SetButtonsPosition()
	{
		// set the size and location for close and auto-hide buttons
		var rectCaption = ClientRectangle;
		var buttonWidth = ButtonClose.Image.Width;
		var buttonHeight = ButtonClose.Image.Height;
		var height = rectCaption.Height - ButtonGapTop - ButtonGapBottom;
		if (buttonHeight < height)
		{
			buttonWidth = buttonWidth * (height / buttonHeight);
			buttonHeight = height;
		}

		var buttonSize = new Size(buttonWidth, buttonHeight);
		var x = rectCaption.X + rectCaption.Width - 1 - ButtonGapRight - m_buttonClose.Width;
		var y = rectCaption.Y + ButtonGapTop;
		var point = new Point(x, y);
		ButtonClose.Bounds = DrawHelper.RtlTransform(this, new Rectangle(point, buttonSize));
		point.Offset(-(buttonWidth + ButtonGapBetween), 0);
		ButtonAutoHide.Bounds = DrawHelper.RtlTransform(this, new Rectangle(point, buttonSize));
		if (ShouldShowAutoHideButton)
			point.Offset(-(buttonWidth + ButtonGapBetween), 0);
		ButtonOptions.Bounds = DrawHelper.RtlTransform(this, new Rectangle(point, buttonSize));
	}

	private void Close_Click(object sender, EventArgs e)
	{
		DockPane.CloseActiveContent();
	}

	private void AutoHide_Click(object sender, EventArgs e)
	{
		DockPane.DockState = DockHelper.ToggleAutoHideState(DockPane.DockState);
		if (DockHelper.IsDockStateAutoHide(DockPane.DockState))
		{
			var prop3 = typeof(DockPanel).GetField("m_autoHideWindow",
				BindingFlags.NonPublic | BindingFlags.Instance);
			var m_autohide = prop3.GetValue(DockPane.DockPanel);
			var prop4 = m_autohide.GetType().GetField("m_flagAnimate", BindingFlags.NonPublic | BindingFlags.Instance);
			prop4.SetValue(m_autohide, false);

			DockPane.DockPanel.ActiveAutoHideContent = null;
		}
	}

	private void Options_Click(object sender, EventArgs e)
	{
		ShowTabPageContextMenu(PointToClient(MousePosition));
	}

	protected override void OnRightToLeftChanged(EventArgs e)
	{
		base.OnRightToLeftChanged(e);
		PerformLayout();
	}

	private sealed class InertButton : InertButtonBase
	{
		private readonly Bitmap m_image;
		private readonly Bitmap m_imageAutoHide;

		public InertButton(YukiDockPaneCaption dockPaneCaption, Bitmap image, Bitmap imageAutoHide)
		{
			DockPaneCaption = dockPaneCaption;
			m_image = image;
			m_imageAutoHide = imageAutoHide;
			RefreshChanges();
		}

		private YukiDockPaneCaption DockPaneCaption { get; }

		public bool IsAutoHide => DockPaneCaption.DockPane.IsAutoHide;

		public override Bitmap Image => IsAutoHide ? m_imageAutoHide : m_image;

		protected override void OnRefreshChanges()
		{
			if (DockPaneCaption.TextColor != ForeColor)
			{
				ForeColor = DockPaneCaption.TextColor;
				Invalidate();
			}
		}
	}

	#region consts

	private const int _TextGapTop = 2;
	private const int _TextGapBottom = 4;
	private const int _TextGapLeft = 3;
	private const int _TextGapRight = 3;
	private const int _ButtonGapTop = 2;
	private const int _ButtonGapBottom = 1;
	private const int _ButtonGapBetween = 1;
	private const int _ButtonGapLeft = 1;
	private const int _ButtonGapRight = 2;

	#endregion
}