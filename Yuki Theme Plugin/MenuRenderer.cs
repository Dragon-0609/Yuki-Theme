using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Yuki_Theme.Core;

namespace Yuki_Theme_Plugin
{
	public class MenuRenderer:ToolStripProfessionalRenderer
	{
		
		public MenuRenderer() : base(new MyColors()) {}

		protected override void OnRenderSeparator (ToolStripSeparatorRenderEventArgs e)
		{
			if((e.Item as ToolStripSeparator) == null)
			{
				base.OnRenderSeparator (e);
				return;
			}

			Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);

			using (SolidBrush brush = new SolidBrush (e.Item.BackColor))
				e.Graphics.FillRectangle (brush, bounds);

			bounds.Y = bounds.Height / 2;
			
			e.Graphics.DrawLine (YukiTheme_VisualPascalABCPlugin.clrPen, 0, bounds.Y, bounds.Width, bounds.Y);
		}
		
		protected override void OnRenderArrow (ToolStripArrowRenderEventArgs e) { 
			var tsMenuItem = e.Item as ToolStripMenuItem;
			if (tsMenuItem != null)
				e.ArrowColor = YukiTheme_VisualPascalABCPlugin.clr;
			base.OnRenderArrow(e);
		}

		protected override void OnRenderItemText (ToolStripItemTextRenderEventArgs e)
		{
			e.TextColor = YukiTheme_VisualPascalABCPlugin.clr;
			base.OnRenderItemText (e);
		}

	}

	public class MyColors : ProfessionalColorTable
	{
		public override Color MenuStripGradientEnd
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color MenuItemSelected
		{
			get => YukiTheme_VisualPascalABCPlugin.bgClick;
		}

		public override Color MenuItemSelectedGradientBegin
		{
			get => YukiTheme_VisualPascalABCPlugin.bgClick;
		}

		public override Color MenuItemSelectedGradientEnd
		{
			get => YukiTheme_VisualPascalABCPlugin.bgClick;
		}

		public override Color MenuItemBorder
		{
			get => YukiTheme_VisualPascalABCPlugin.bgBorder;
		}

		public override Color MenuBorder
		{
			get => YukiTheme_VisualPascalABCPlugin.bgBorder;
		}

		public override Color GripDark
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color GripLight
		{
			get => YukiTheme_VisualPascalABCPlugin.bgdef;
		}

		public override Color ImageMarginGradientBegin
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ImageMarginGradientEnd
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ImageMarginGradientMiddle
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ImageMarginRevealedGradientBegin
		{
			get => YukiTheme_VisualPascalABCPlugin.bgdef;
		}

		public override Color ImageMarginRevealedGradientEnd
		{
			get => YukiTheme_VisualPascalABCPlugin.bgdef;
		}

		public override Color MenuStripGradientBegin
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ImageMarginRevealedGradientMiddle
		{
			get => YukiTheme_VisualPascalABCPlugin.bgdef;
		}

		public override Color MenuItemPressedGradientBegin
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color MenuItemPressedGradientEnd
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color RaftingContainerGradientBegin
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color RaftingContainerGradientEnd
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color SeparatorDark
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color SeparatorLight
		{
			get => YukiTheme_VisualPascalABCPlugin.clr;
		}

		public override Color StatusStripGradientBegin
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color StatusStripGradientEnd
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color MenuItemPressedGradientMiddle
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ButtonPressedHighlight
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ToolStripBorder
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ToolStripDropDownBackground
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ToolStripGradientBegin
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ToolStripGradientMiddle
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ToolStripGradientEnd
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ToolStripContentPanelGradientBegin
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ToolStripContentPanelGradientEnd
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ButtonSelectedHighlight
		{
			get => YukiTheme_VisualPascalABCPlugin.bgClick;
		}

		public override Color ButtonPressedGradientBegin
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ButtonPressedGradientMiddle
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ButtonPressedGradientEnd
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color CheckBackground
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color CheckSelectedBackground
		{
			get => YukiTheme_VisualPascalABCPlugin.bgClick;
		}

		public override Color CheckPressedBackground
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ButtonCheckedHighlight
		{
			get => YukiTheme_VisualPascalABCPlugin.bgBorder;
		}

		public override Color ButtonCheckedHighlightBorder
		{
			get => YukiTheme_VisualPascalABCPlugin.bgBorder;
		}

		public override Color ButtonCheckedGradientEnd
		{
			get => YukiTheme_VisualPascalABCPlugin.bgdef;
		}

		public override Color ButtonSelectedGradientBegin
		{
			get => YukiTheme_VisualPascalABCPlugin.bgClick;
		}

		public override Color ButtonSelectedGradientMiddle
		{
			get => YukiTheme_VisualPascalABCPlugin.bgClick;
		}

		public override Color ButtonSelectedGradientEnd
		{
			get => YukiTheme_VisualPascalABCPlugin.bgClick;
		}

		public override Color ButtonSelectedBorder
		{
			get => YukiTheme_VisualPascalABCPlugin.bgBorder;
		}

		public override Color ButtonCheckedGradientBegin
		{
			get => YukiTheme_VisualPascalABCPlugin.bgdef;
		}

		public override Color ButtonCheckedGradientMiddle
		{
			get => YukiTheme_VisualPascalABCPlugin.bgdef;
		}


		public override Color ButtonPressedBorder
		{
			get => YukiTheme_VisualPascalABCPlugin.clrHover;
		}

		public override Color ButtonPressedHighlightBorder
		{
			get => YukiTheme_VisualPascalABCPlugin.bgBorder;
		}

		public override Color ButtonSelectedHighlightBorder
		{
			get => YukiTheme_VisualPascalABCPlugin.bgBorder;
		}

		public override Color ToolStripPanelGradientBegin
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color ToolStripPanelGradientEnd
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color OverflowButtonGradientBegin
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color OverflowButtonGradientMiddle
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

		public override Color OverflowButtonGradientEnd
		{
			get => YukiTheme_VisualPascalABCPlugin.bg;
		}

	}
}