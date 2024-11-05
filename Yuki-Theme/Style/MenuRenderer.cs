using System.Drawing;
using System.Windows.Forms;
using YukiTheme.Engine;

namespace YukiTheme.Style;

public class MenuRenderer : ToolStripProfessionalRenderer
{
	public MenuRenderer() : base(new MyColors())
	{
	}

	protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
	{
		if (e.Item as ToolStripSeparator == null)
		{
			base.OnRenderSeparator(e);
			return;
		}

		var bounds = new Rectangle(Point.Empty, e.Item.Size);

		e.Graphics.FillRectangle(ColorReference.BackgroundBrush, bounds);

		bounds.Y = bounds.Height / 2;

		e.Graphics.DrawLine(ColorReference.BackgroundClick3Pen, 0, bounds.Y, bounds.Width, bounds.Y);
	}

	protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
	{
		if (e.Item is ToolStripMenuItem tsMenuItem)
			e.ArrowColor = ColorReference.ForegroundColor;
		base.OnRenderArrow(e);
	}

	protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
	{
		e.TextColor = ColorReference.ForegroundColor;
		base.OnRenderItemText(e);
	}
}

public class MyColors : ProfessionalColorTable
{
	public override Color MenuStripGradientEnd => ColorReference.BackgroundColor;

	public override Color MenuItemSelected => ColorReference.BackgroundClickColor;

	public override Color MenuItemSelectedGradientBegin => ColorReference.BackgroundClickColor;

	public override Color MenuItemSelectedGradientEnd => ColorReference.BackgroundClickColor;

	public override Color MenuItemBorder => ColorReference.BorderColor;

	public override Color MenuBorder => ColorReference.BorderColor;

	public override Color GripDark => ColorReference.BackgroundColor;

	public override Color GripLight => ColorReference.BackgroundDefaultColor;

	public override Color ImageMarginGradientBegin => ColorReference.BackgroundColor;

	public override Color ImageMarginGradientEnd => ColorReference.BackgroundColor;

	public override Color ImageMarginGradientMiddle => ColorReference.BackgroundColor;

	public override Color ImageMarginRevealedGradientBegin => ColorReference.BackgroundDefaultColor;

	public override Color ImageMarginRevealedGradientEnd => ColorReference.BackgroundDefaultColor;

	public override Color MenuStripGradientBegin => ColorReference.BackgroundColor;

	public override Color ImageMarginRevealedGradientMiddle => ColorReference.BackgroundDefaultColor;

	public override Color MenuItemPressedGradientBegin => ColorReference.BackgroundColor;

	public override Color MenuItemPressedGradientEnd => ColorReference.BackgroundColor;

	public override Color RaftingContainerGradientBegin => ColorReference.BackgroundColor;

	public override Color RaftingContainerGradientEnd => ColorReference.BackgroundColor;

	public override Color SeparatorDark => ColorReference.BackgroundColor;

	public override Color SeparatorLight => ColorReference.ForegroundColor;

	public override Color StatusStripGradientBegin => ColorReference.BackgroundColor;

	public override Color StatusStripGradientEnd => ColorReference.BackgroundColor;

	public override Color MenuItemPressedGradientMiddle => ColorReference.BackgroundColor;

	public override Color ButtonPressedHighlight => ColorReference.BackgroundColor;

	public override Color ToolStripBorder => ColorReference.BackgroundColor;

	public override Color ToolStripDropDownBackground => ColorReference.BackgroundColor;

	public override Color ToolStripGradientBegin => ColorReference.BackgroundColor;

	public override Color ToolStripGradientMiddle => ColorReference.BackgroundColor;

	public override Color ToolStripGradientEnd => ColorReference.BackgroundColor;

	public override Color ToolStripContentPanelGradientBegin => ColorReference.BackgroundColor;

	public override Color ToolStripContentPanelGradientEnd => ColorReference.BackgroundColor;

	public override Color ButtonSelectedHighlight => ColorReference.BackgroundClickColor;

	public override Color ButtonPressedGradientBegin => ColorReference.BackgroundColor;

	public override Color ButtonPressedGradientMiddle => ColorReference.BackgroundColor;

	public override Color ButtonPressedGradientEnd => ColorReference.BackgroundColor;

	public override Color CheckBackground => ColorReference.BackgroundColor;

	public override Color CheckSelectedBackground => ColorReference.BackgroundClickColor;

	public override Color CheckPressedBackground => ColorReference.BackgroundColor;

	public override Color ButtonCheckedHighlight => ColorReference.BorderColor;

	public override Color ButtonCheckedHighlightBorder => ColorReference.BorderColor;

	public override Color ButtonCheckedGradientEnd => ColorReference.BackgroundDefaultColor;

	public override Color ButtonSelectedGradientBegin => ColorReference.BackgroundClickColor;

	public override Color ButtonSelectedGradientMiddle => ColorReference.BackgroundClickColor;

	public override Color ButtonSelectedGradientEnd => ColorReference.BackgroundClickColor;

	public override Color ButtonSelectedBorder => ColorReference.BorderColor;

	public override Color ButtonCheckedGradientBegin => ColorReference.BackgroundDefaultColor;

	public override Color ButtonCheckedGradientMiddle => ColorReference.BackgroundDefaultColor;


	public override Color ButtonPressedBorder => ColorReference.ForegroundHoverColor;

	public override Color ButtonPressedHighlightBorder => ColorReference.BorderColor;

	public override Color ButtonSelectedHighlightBorder => ColorReference.BorderColor;

	public override Color ToolStripPanelGradientBegin => ColorReference.BackgroundColor;

	public override Color ToolStripPanelGradientEnd => ColorReference.BackgroundColor;

	public override Color OverflowButtonGradientBegin => ColorReference.BackgroundColor;

	public override Color OverflowButtonGradientMiddle => ColorReference.BackgroundColor;

	public override Color OverflowButtonGradientEnd => ColorReference.BackgroundColor;
}