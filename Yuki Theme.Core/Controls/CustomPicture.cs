using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Controls
{
	public class CustomPicture : PictureBox
	{
		public  Point        margin;
		
		public CustomPicture () : base ()
		{
			margin = new Point (10, 20);
			// MouseDown += On_MouseDown;
			// MouseMove += On_MouseMove;
			typeof (PictureBox).InvokeMember ("DoubleBuffered", BindingFlags.SetProperty
			                                                  | BindingFlags.Instance | BindingFlags.NonPublic, null,
			                                  this, new object [] {true});
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
		
		private Point MouseDownLocation;

		private Point prevMouseDownLocation;


		private void On_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				MouseDownLocation = e.Location;
				prevMouseDownLocation = e.Location;
			}
		}

		private void On_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (
					(Right <= Parent.ClientSize.Width - margin.X || prevMouseDownLocation.X - e.X >= 0) &&
					(Left >= margin.X || prevMouseDownLocation.X - e.X <= 0)
				)
				{
					Left = e.X + Left - MouseDownLocation.X;
					prevMouseDownLocation.X = e.Location.X;
				}

				if (
					(Bottom <= Parent.ClientSize.Height - margin.Y || prevMouseDownLocation.Y - e.Y >= 0) &&
					(Top >= margin.Y || prevMouseDownLocation.Y - e.Y <= 0)
					)
				{
					Top = e.Y + Top - MouseDownLocation.Y;
					prevMouseDownLocation.Y = e.Location.Y;
				}
			}
		}

	}
}