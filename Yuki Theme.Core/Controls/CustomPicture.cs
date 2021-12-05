using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Controls
{
	public class CustomPicture : PictureBox
	{
		public  Point        margin;
		
		public CustomPicture () : base ()
		{
			margin = new Point (10, 20);
		}
		
		public Image img
		{
			get { return Image; }
			set
			{
				Image = value;
				if(Image != null)
				{
					Size = value.Size;
					Location = new Point (
						Parent.ClientSize.Width - Size.Width - margin.X,
						Parent.ClientSize.Height - Size.Height - margin.Y);

					Region = CreateRegion ((Bitmap) Image);
				}
				else
				{
					Region = null;
				}
			}
		}
		
		private Region CreateRegion (Bitmap maskImage)
		{
			// We're using pixel 0,0 as the "transparent" color.
			Color mask = maskImage.GetPixel(0, 0);
			GraphicsPath graphicsPath = new GraphicsPath();
			for (int x = 0; x < maskImage.Width; x++)
			{
				for (int y = 0; y < maskImage.Height; y++)
				{
					if (!maskImage.GetPixel(x, y).Equals(mask))
					{
						graphicsPath.AddRectangle(new Rectangle(x, y, 1, 1));
					}
				}
			}

			return new Region(graphicsPath);
		}
	}
}