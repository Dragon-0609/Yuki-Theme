using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Controls
{
	public class StickerPicture : Control
	{
		public StickerPicture ()
		{
			SetStyle (ControlStyles.SupportsTransparentBackColor | ControlStyles.Opaque, true);
			BackColor = Color.Transparent;
		} //TransparentControl


		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
				return cp;
			}
		}

		protected override void OnPaint (PaintEventArgs e)
		{
			if (image != null)
				e.Graphics.DrawImage (image, 0, 0);
		} //OnPaint

		Image image;

		internal Image Image
		{
			get { return image; }
			set
			{
				image = value;
				if (image == null) return;
				Width = image.Width;
				Height = image.Height;
				if (Parent != null)
				{
					Location = new Point (Parent.ClientSize.Width - Width - 10, Parent.ClientSize.Height - Height - 10);
				}
				Invalidate ();
			} //set Image
		}     //Image
	}
}