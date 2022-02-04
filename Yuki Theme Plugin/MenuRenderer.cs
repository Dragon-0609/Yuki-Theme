using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme_Plugin
{
	public class MenuRenderer : ToolStripProfessionalRenderer
	{
		public MenuRenderer () : base (new MyColors ())
		{
		}

		protected override void OnRenderSeparator (ToolStripSeparatorRenderEventArgs e)
		{
			if (e.Item as ToolStripSeparator == null)
			{
				base.OnRenderSeparator (e);
				return;
			}

			var bounds = new Rectangle (Point.Empty, e.Item.Size);

			using (var brush = new SolidBrush (e.Item.BackColor))
			{
				e.Graphics.FillRectangle (brush, bounds);
			}

			bounds.Y = bounds.Height / 2;

			e.Graphics.DrawLine (YukiTheme_VisualPascalABCPlugin.clrPen, 0, bounds.Y, bounds.Width, bounds.Y);
		}

		protected override void OnRenderArrow (ToolStripArrowRenderEventArgs e)
		{
			var tsMenuItem = e.Item as ToolStripMenuItem;
			if (tsMenuItem != null)
				e.ArrowColor = YukiTheme_VisualPascalABCPlugin.clr;
			base.OnRenderArrow (e);
		}

		protected override void OnRenderItemText (ToolStripItemTextRenderEventArgs e)
		{
			e.TextColor = YukiTheme_VisualPascalABCPlugin.clr;
			base.OnRenderItemText (e);
		}
	}

	public class MyColors : ProfessionalColorTable
	{
		public override Color MenuStripGradientEnd => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color MenuItemSelected => YukiTheme_VisualPascalABCPlugin.bgClick;

		public override Color MenuItemSelectedGradientBegin => YukiTheme_VisualPascalABCPlugin.bgClick;

		public override Color MenuItemSelectedGradientEnd => YukiTheme_VisualPascalABCPlugin.bgClick;

		public override Color MenuItemBorder => YukiTheme_VisualPascalABCPlugin.bgBorder;

		public override Color MenuBorder => YukiTheme_VisualPascalABCPlugin.bgBorder;

		public override Color GripDark => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color GripLight => YukiTheme_VisualPascalABCPlugin.bgdef;

		public override Color ImageMarginGradientBegin => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ImageMarginGradientEnd => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ImageMarginGradientMiddle => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ImageMarginRevealedGradientBegin => YukiTheme_VisualPascalABCPlugin.bgdef;

		public override Color ImageMarginRevealedGradientEnd => YukiTheme_VisualPascalABCPlugin.bgdef;

		public override Color MenuStripGradientBegin => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ImageMarginRevealedGradientMiddle => YukiTheme_VisualPascalABCPlugin.bgdef;

		public override Color MenuItemPressedGradientBegin => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color MenuItemPressedGradientEnd => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color RaftingContainerGradientBegin => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color RaftingContainerGradientEnd => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color SeparatorDark => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color SeparatorLight => YukiTheme_VisualPascalABCPlugin.clr;

		public override Color StatusStripGradientBegin => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color StatusStripGradientEnd => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color MenuItemPressedGradientMiddle => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ButtonPressedHighlight => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ToolStripBorder => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ToolStripDropDownBackground => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ToolStripGradientBegin => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ToolStripGradientMiddle => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ToolStripGradientEnd => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ToolStripContentPanelGradientBegin => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ToolStripContentPanelGradientEnd => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ButtonSelectedHighlight => YukiTheme_VisualPascalABCPlugin.bgClick;

		public override Color ButtonPressedGradientBegin => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ButtonPressedGradientMiddle => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ButtonPressedGradientEnd => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color CheckBackground => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color CheckSelectedBackground => YukiTheme_VisualPascalABCPlugin.bgClick;

		public override Color CheckPressedBackground => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ButtonCheckedHighlight => YukiTheme_VisualPascalABCPlugin.bgBorder;

		public override Color ButtonCheckedHighlightBorder => YukiTheme_VisualPascalABCPlugin.bgBorder;

		public override Color ButtonCheckedGradientEnd => YukiTheme_VisualPascalABCPlugin.bgdef;

		public override Color ButtonSelectedGradientBegin => YukiTheme_VisualPascalABCPlugin.bgClick;

		public override Color ButtonSelectedGradientMiddle => YukiTheme_VisualPascalABCPlugin.bgClick;

		public override Color ButtonSelectedGradientEnd => YukiTheme_VisualPascalABCPlugin.bgClick;

		public override Color ButtonSelectedBorder => YukiTheme_VisualPascalABCPlugin.bgBorder;

		public override Color ButtonCheckedGradientBegin => YukiTheme_VisualPascalABCPlugin.bgdef;

		public override Color ButtonCheckedGradientMiddle => YukiTheme_VisualPascalABCPlugin.bgdef;


		public override Color ButtonPressedBorder => YukiTheme_VisualPascalABCPlugin.clrHover;

		public override Color ButtonPressedHighlightBorder => YukiTheme_VisualPascalABCPlugin.bgBorder;

		public override Color ButtonSelectedHighlightBorder => YukiTheme_VisualPascalABCPlugin.bgBorder;

		public override Color ToolStripPanelGradientBegin => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color ToolStripPanelGradientEnd => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color OverflowButtonGradientBegin => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color OverflowButtonGradientMiddle => YukiTheme_VisualPascalABCPlugin.bg;

		public override Color OverflowButtonGradientEnd => YukiTheme_VisualPascalABCPlugin.bg;
	}
}