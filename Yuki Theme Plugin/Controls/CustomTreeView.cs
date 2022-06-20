using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme_Plugin.Controls
{
	public class CustomTreeView
	{
		internal void AddCustomTreeViewEvents (TreeView treeView)
		{
			treeView.DrawMode = TreeViewDrawMode.OwnerDrawText;
			treeView.HideSelection= false;
			treeView.DrawNode += treeView_DrawNode;
		}
		
		private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			if (e.Node == null) return;

			TreeNodeStates state = e.State;
			Font font = e.Node.NodeFont ?? e.Node.TreeView.Font;
			Color fore = e.Node.ForeColor;
			if (fore == Color.Empty) 
				fore = e.Node.TreeView.ForeColor;
			if (e.Node == e.Node.TreeView.SelectedNode)
			{
				bool unfocused = !e.Node.TreeView.Focused;
				Color bg = e.Node.TreeView.BackColor;
				Color bgBorder = e.Node.TreeView.BackColor;

				if (unfocused)
				{
					bgBorder = bg = YukiTheme_VisualPascalABCPlugin.bgInactive;
				}
				else
				{
					bg = YukiTheme_VisualPascalABCPlugin.bgSelection;
					bgBorder = YukiTheme_VisualPascalABCPlugin.bgBorder;
				}
				
				e.Graphics.FillRectangle(new SolidBrush(bgBorder), e.Bounds);
				ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds, fore, bg);
				TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, fore, bg, TextFormatFlags.GlyphOverhangPadding);
			}
			else
			{
				e.DrawDefault = true;
			}
		}
	}
}