﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;
using Yuki_Theme.Core.Database;
using Yuki_Theme.Core.Forms;

namespace Yuki_Theme.Core.Controls
{
	public class CustomPicture : PictureBox
	{
		public  Point           margin;
		public  PointF          relativePosition;
		private AnchorStyles    align;
		private RelativeUnit    unit     => Settings.unit;
		private DatabaseManager database => Settings.database;
		public  CustomPanel     pnl;
		public  int             width;
		public  int             width3;
		public  int             width32;
		public  int             height;
		public  int             height3;
		public  int             height32;
		private float           unitx;
		private float           unity;
		private Form            tmp_form;
		private Image           originalImage;
		private Image           resizedImage;

		public CustomPicture (Form fm) : base ()
		{
			margin = new Point (10, 20);
			relativePosition = unit == RelativeUnit.Pixel ? margin : new Point (1, 1);
			align = AnchorStyles.Bottom | AnchorStyles.Right;
			tmp_form = fm;
			// align = Content;
			ReadData ();
			MouseDown += On_MouseDown;
			MouseMove += On_MouseMove;
			MouseUp += On_MouseUp;
			typeof (PictureBox).InvokeMember ("DoubleBuffered", BindingFlags.SetProperty
			                                                  | BindingFlags.Instance | BindingFlags.NonPublic, null,
			                                  this, new object [] {true});
			ParentChanged += OnParentChanged;
			SizeMode = PictureBoxSizeMode.Zoom;

		}

		private void OnParentChanged (object sender, EventArgs e)
		{
			((Form) TopLevelControl).Resize += OnResize;
		}

		private void OnResize (object sender, EventArgs e)
		{
			width = Parent.ClientSize.Width / 2;
			height = Parent.ClientSize.Height / 2;
			UpdateLocation ();
		}

		public Image img
		{
			get { return Image; }
			set
			{
				if (originalImage != value)
				{
					ClearImages ();
				}
				originalImage = value;
				if (originalImage != null)
				{
					ReCalculatePositionNSize ();
				} else
				{
					Region = null;
					ClearImages ();
				}
			}
		}

		private void ClearImages ()
		{
			Image = null;
			if (originalImage != null)
				originalImage.Dispose ();
			if (resizedImage != null)
				resizedImage.Dispose ();
		}

		public void ReCalculatePositionNSize ()
		{
			if (originalImage != null)
			{
				Size size = Helper.CalculateDimension (originalImage.Size);
				if (Helper.IsDimensionAvailable ())
				{
					if (resizedImage != null)
						resizedImage.Dispose ();

					resizedImage = ResizeImage (originalImage, size.Width, size.Height);
					Image = resizedImage;
				} else
				{
					if (resizedImage != null)
						resizedImage.Dispose ();

					Image = originalImage;
				}

				Size = size;
				width = Parent.ClientSize.Width / 2;
				width3 = Parent.ClientSize.Width / 3;
				width32 = width3 * 2;
				height = Parent.ClientSize.Height / 2;
				height3 = Parent.ClientSize.Height / 3;
				height32 = height3 * 2;
				UpdateLocation ();
				CreateRegion ();
			} else
			{
				Region = null;
				ClearImages ();
			}
		}

		public void UpdateLocation ()
		{
			if (unit == RelativeUnit.Percent)
			{
				unitx = Parent.ClientSize.Width / 100f;
				unity = Parent.ClientSize.Height / 100f;
			}

			int x = CalculateX ();
			int y = CalculateY ();
			Location = new Point (x, y);
		}

		private int CalculateX ()
		{
			int x = 0;
			if (align.HasFlag (AnchorStyles.Left))
			{
				x = (int) (margin.X + relativePosition.X * (unit == RelativeUnit.Percent ? unitx : 1));
			} else if (align.HasFlag (AnchorStyles.Right))
			{
				x = (int) (Parent.ClientSize.Width - Size.Width - margin.X -
				           relativePosition.X * (unit == RelativeUnit.Percent ? unitx : 1));
			} else
			{
				x = (int) (width - (Size.Width / 2) - (margin.X / 2) - relativePosition.X * (unit == RelativeUnit.Percent ? unitx : 1));
			}

			return x;
		}

		private int CalculateY ()
		{
			int y = 0;
			if (align.HasFlag (AnchorStyles.Top))
			{
				y = (int) (margin.Y + relativePosition.Y * (unit == RelativeUnit.Percent ? unity : 1));
			} else if (align.HasFlag (AnchorStyles.Bottom))
			{
				y = (int) (Parent.ClientSize.Height - Size.Height - margin.Y -
				           relativePosition.Y * (unit == RelativeUnit.Percent ? unity : 1));
			} else
			{
				y = (int) (height - (Size.Height / 2) - (margin.Y / 2) - relativePosition.Y * (unit == RelativeUnit.Percent ? unity : 1));
			}

			return y;
		}

		private void CreateRegion ()
		{
			Bitmap maskImage = (Bitmap) Image;
			// We're using pixel 0,0 as the "transparent" color.
			Color mask = maskImage.GetPixel (0, 0);
			// This is mask for transparent color
			int TransparentMask = mask.A < 20 ? 20 : mask.A;
			GraphicsPath graphicsPath = new GraphicsPath ();
			for (int x = 0; x < maskImage.Width; x++)
			{
				for (int y = 0; y < maskImage.Height; y++)
				{
					if (maskImage.GetPixel (x, y).A > TransparentMask)
					{
						graphicsPath.AddRectangle (new Rectangle (x, y, 1, 1));
					}
				}
			}

			Region = new Region (graphicsPath);
		}

		private Point MouseDownLocation;

		private Point prevMouseDownLocation;


		private void On_MouseDown (object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				MouseDownLocation = e.Location;
				prevMouseDownLocation = e.Location;
				width = Parent.ClientSize.Width / 2;
				width3 = Parent.ClientSize.Width / 3;
				width32 = width3 * 2;
				height = Parent.ClientSize.Height / 2;
				height3 = Parent.ClientSize.Height / 3;
				height32 = height3 * 2;
				if (unit == RelativeUnit.Percent)
				{
					unitx = Parent.ClientSize.Width / 100f;
					unity = Parent.ClientSize.Height / 100f;
				}
				
				if(Settings.showGrids)
					pnl.Prepare ();
				// pnl.Visible = true;
			}
		}

		private void On_MouseMove (object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				int left = Left;
				int top = Top;
				if (
					(Right <= Parent.ClientSize.Width - margin.X || prevMouseDownLocation.X - e.X >= 0) &&
					(Left >= margin.X || prevMouseDownLocation.X - e.X <= 0)
				)
				{
					left = e.X + Left - MouseDownLocation.X;

					if (left + Width > Parent.ClientSize.Width - margin.X)
					{
						left = Parent.ClientSize.Width - margin.X - this.Width;
					} else if (left < margin.X)
					{
						left = margin.X;
					} // Center

					prevMouseDownLocation.X = e.Location.X;
				}

				if (
					(Bottom <= Parent.ClientSize.Height - margin.Y || prevMouseDownLocation.Y - e.Y >= 0) &&
					(Top >= margin.Y || prevMouseDownLocation.Y - e.Y <= 0)
				)
				{
					top = e.Y + Top - MouseDownLocation.Y;

					if (top + Height > Parent.ClientSize.Height - margin.Y) // Bottom
					{
						top = Parent.ClientSize.Height - margin.Y - Height;
					} else if (top < margin.Y) // Top
					{
						top = margin.Y;
					} // Center

					prevMouseDownLocation.Y = e.Location.Y;
				}

				Location = new Point (left, top);
			}
		}

		private void On_MouseUp (object sender, MouseEventArgs e)
		{
			if(Settings.showGrids)
				pnl.Visible = false;
			Point nl = Parent.PointToClient (Cursor.Position);
			AnchorStyles styles = AnchorStyles.None;
			// Console.WriteLine(nl.ToString ());
			if (nl.X < width3) // X - Left
			{
				styles = AnchorStyles.Left;
				// Console.WriteLine("Left");
			} else
			{
				// X > Left -> Center || Right
				if (nl.X > width32) // Right
				{
					styles = AnchorStyles.Right;
					// Console.WriteLine("Right");
				} else // Center
				{
					styles = AnchorStyles.None;
					// Console.WriteLine("Center");
				}
			}
			if (nl.Y < height3) // Y - Top
			{
				styles |= AnchorStyles.Top;
				// Console.WriteLine ($"T: {styles}");
				// Console.WriteLine("Top");
			} else
			{
				// Y > Top -> Center || Bottom
				if (nl.Y > height32) // Bottom
				{
					styles |= AnchorStyles.Bottom;
					// Console.WriteLine ($"B: {styles}");
					// Console.WriteLine("Bottom");
				} else // Center
				{
					styles |= AnchorStyles.None;
					// Console.WriteLine ($"C: {styles}");
					// Console.WriteLine("Center");
					
				}
			}

			relativePosition.X = GetRelatedX (styles);
			relativePosition.Y = GetRelatedY (styles);
			SetAlign (styles);
			SaveData ();
		}

		private void SetAlign (AnchorStyles style)
		{
			// Console.WriteLine (align.ToString ());
			if (align != style)
			{
				align = style;
				Anchor = style;
			}
		}

		private float GetRelatedX (AnchorStyles style)
		{
			float res = 0;
			if (style.HasFlag (AnchorStyles.Left))
			{
				res = (Left - margin.X) / (unit == RelativeUnit.Percent ? unitx : 1);
			} else if (style.HasFlag (AnchorStyles.Right))
			{
				res = (Parent.ClientSize.Width - margin.X - Right) / (unit == RelativeUnit.Percent ? unitx : 1);
			} else
			{
				res = (width - (Size.Width / 2) - (margin.X / 2) - Left) / (unit == RelativeUnit.Percent ? unitx : 1);
			}

			return res;
		}

		private float GetRelatedY (AnchorStyles style)
		{
			float res = 0;
			if (style.HasFlag (AnchorStyles.Top))
			{
				res = (Top - margin.Y) / (unit == RelativeUnit.Percent ? unity : 1);
			} else if (style.HasFlag (AnchorStyles.Bottom))
			{
				res = (Parent.ClientSize.Height - margin.Y - Bottom) / (unit == RelativeUnit.Percent ? unity : 1);
			} else
			{
				res = (height - (Size.Height / 2) - (margin.Y / 2) - Top) / (unit == RelativeUnit.Percent ? unity : 1);
			}

			return res;
		}

		private string ConvertLocationToSave ()
		{
			string s =
				$"{relativePosition.X}|{relativePosition.Y}|{(int) align}|{(int) unit}";
			return s;
		}

		public void ReadData ()
		{
			string data = database.ReadData (Settings.STICKERPOSITION, "");
			if (data != "")
			{
				try
				{
					string [] cc = data.Split ('|');
					float x = float.Parse (cc [0]);
					float y = float.Parse (cc [1]);
					relativePosition = new PointF (x, y);
					align = (AnchorStyles) (int.Parse (cc [2]));
					RelativeUnit un = (RelativeUnit) (int.Parse (cc [3]));
					if (unit != un)
					{
						float tunitx = tmp_form.ClientSize.Width / 100f;
						float tunity = tmp_form.ClientSize.Height / 100f;
						if (un == RelativeUnit.Percent)
						{
							relativePosition = new PointF (relativePosition.X * tunitx, relativePosition.Y * tunity);
						} else
						{
							relativePosition = new PointF (relativePosition.X / tunitx, relativePosition.Y / tunity);
						}
					}
				} catch (Exception)
				{
					relativePosition = unit == RelativeUnit.Pixel ? margin : new Point (1, 1);
					align = AnchorStyles.Bottom | AnchorStyles.Right;
				}
			} else
			{
				relativePosition = unit == RelativeUnit.Pixel ? margin : new Point (1, 1);
				align = AnchorStyles.Bottom | AnchorStyles.Right;
			}
		}

		private void SaveData ()
		{
			database.UpdateData (Settings.STICKERPOSITION, ConvertLocationToSave ());
		}
		
		/// <summary>
		/// Resize the image to the specified width and height.
		/// </summary>
		/// <param name="image">The image to resize.</param>
		/// <param name="width">The width to resize to.</param>
		/// <param name="height">The height to resize to.</param>
		/// <returns>The resized image.</returns>
		public static Bitmap ResizeImage(Image image, int width, int height)
		{
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width,image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}
	}
}