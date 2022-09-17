using System.Drawing;
using System.Windows;
using System.Windows.Media;
using Point = System.Windows.Point;
namespace Yuki_Theme.Core.WPF.Windows
{
	public class PositionCalculator
	{
		private int          width;
		private int          width3;
		private int          width32;
		private int          height;
		private int          height3;
		private int          height32;
		private float        unitx;
		private float        unity;
		private Point        _margin;
		private RelativeUnit unit => Settings.unit;

		private StickerWindow window;
		public PositionCalculator (StickerWindow window)
		{
			this.window = window;
			_margin = new Point (10, 20);
		}

		internal void PrepareData ()
		{
			Rect owner = window.Owner.GetAbsoluteRect ();
			width = (int)(owner.Width / 2);
			width3 = (int)(owner.Width / 3);
			width32 = width3 * 2;
			height = (int)(owner.Height / 2);
			height3 = (int)(owner.Height / 3);
			height32 = height3 * 2;
		}
		public void KeepBounds (ref double x, ref double y)
		{
			Rect owner = window.Owner.GetAbsoluteRect ();

			if (x < owner.Left)
				x = owner.Left;
			else
			{
				if (x + window.Width > owner.Left + owner.Width)
					x = owner.Left + owner.Width - window.Width;
			}

			if (y < owner.Top)
				y = owner.Top;
			else
			{

				if (y + window.Height > owner.Top + owner.Height)
					y = owner.Top + owner.Height - window.Height;
			}
		}


		public void SetAligns (Point position)
		{
			if (position.X < width3) // X - Left
			{
				window.AlignX = AlignmentX.Left;
			} else
			{
				// X > Left ->  Right || Center 
				window.AlignX = position.X > width32 ? AlignmentX.Right : AlignmentX.Center;
			}
			if (position.Y < height3) // Y - Top
			{
				window.AlignY = AlignmentY.Top;
			} else
			{
				// Y > Top -> Bottom || Center 
				window.AlignY = position.Y > height32 ? AlignmentY.Bottom : AlignmentY.Center;
			}


		}

		private float GetRelatedX (Rect owner)
		{
			float res = 0;
			AlignmentX style = window.AlignX;
			if (style == AlignmentX.Left)
			{
				res = (float)((window.Left - _margin.X) / (unit == RelativeUnit.Percent ? unitx : 1));
			} else if (style == AlignmentX.Center)
			{
				res = (float)((width - (window.Width / 2) - (_margin.X / 2) - window.Left) / (unit == RelativeUnit.Percent ? unitx : 1));
			} else if (style == AlignmentX.Right)
			{
				res = (float)((owner.Width - _margin.X - (window.Left + window.Width)) / (unit == RelativeUnit.Percent ? unitx : 1));
			}

			return res;
		}

		private float GetRelatedY (Rect owner)
		{
			float res = 0;
			AlignmentY style = window.AlignY;
			if (style == AlignmentY.Top)
			{
				res = (float)((window.Top - _margin.Y) / (unit == RelativeUnit.Percent ? unity : 1));
			} else if (style == AlignmentY.Center)
			{
				res = (float)((height - (window.Height / 2) - (_margin.Y / 2) - window.Top) / (unit == RelativeUnit.Percent ? unity : 1));
			} else if (style == AlignmentY.Bottom)
			{
				res = (float)((owner.Height - _margin.Y - window.Top + window.Height) / (unit == RelativeUnit.Percent ? unity : 1));
			}

			return res;
		}
		public void SaveRelatedPosition ()
		{
			Rect owner = window.Owner.GetAbsoluteRect ();
			window._relativePosition = new PointF (GetRelatedX (owner), GetRelatedY (owner));
			window.SetBorderOutline ();
		}
	}
}