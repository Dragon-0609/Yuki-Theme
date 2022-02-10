using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Forms
{
	public partial class TesterForm : Form
	{
		public TesterForm ()
		{
			InitializeComponent ();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (TesterForm));
			stickerPicture1.Image = ((System.Drawing.Image) (resources.GetObject ("panel1.BackgroundImage")));
			stickerPicture1.BringToFront ();
			/*SizeChanged += (sender, e) =>
			{
				stickerPicture1.Invalidate();
			};*/
		}
	}
}