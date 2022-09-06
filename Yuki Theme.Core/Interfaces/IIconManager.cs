using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Interfaces
{
	public interface IIconManager
	{
		Image RenderToolBarItemImage (ToolStripItem btn);
	}
}