using System.Drawing;
using System.Windows.Forms;
using YukiTheme.Tools;

namespace YukiTheme.Components.StatusBar;

public abstract class StatusBarItem
{
	protected readonly ToolStripItem Item;

	protected StatusBarItem()
	{
		// ReSharper disable once VirtualMemberCallInConstructor
		Item = CreateItem();
		Item.Alignment = ToolStripItemAlignment.Right;
		Item.Padding = new Padding(2, 0, 2, 0);
		Item.Margin = Padding.Empty;
	}

	protected virtual ToolStripItem CreateItem() => new ToolStripButton();

	protected string Text
	{
		get => Item.Text;
		set => Item.Text = value;
	}

	protected void SetIcon(string svg, Color color)
	{
		Item.Image?.Dispose();
		SvgRenderInfo context = new SvgRenderInfo(SvgRenderer.LoadSvg(svg, "Icons."))
		{
			Clr = color,
			CustomColor = true
		};
		Item.Image = SvgRenderer.RenderSvg(Item.Size, context);
	}

	internal ToolStripItem GetControl() => Item;
}