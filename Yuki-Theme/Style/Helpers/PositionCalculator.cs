using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using YukiTheme.Components;
using YukiTheme.Tools;
using Point = System.Windows.Point;

namespace YukiTheme.Style.Helpers
{
	public class PositionCalculator
	{
		private int width;
		private int width3;
		private int width32;
		private int height;
		private int height3;
		private int height32;
		private float unitx;
		private float unity;
		private Point _margin;

		private StickerWindow window;

		public PositionCalculator(StickerWindow window)
		{
			this.window = window;
			_margin = new Point(SnapWindow.BORDER_OUTLINE_X, SnapWindow.BORDER_OUTLINE_Y);
		}

		internal void PrepareData()
		{
			Rect owner = window.GetOwnerRectangle();
			width = (int)(owner.Width / 2);
			width3 = (int)(owner.Width / 3);
			width32 = width3 * 2;
			height = (int)(owner.Height / 2);
			height3 = (int)(owner.Height / 3);
			height32 = height3 * 2;
			unitx = (float)(owner.Width / 100);
			unity = (float)(owner.Height / 100);
		}

		public void KeepBounds(ref double x, ref double y)
		{
			Rect owner = window.GetOwnerRectangle();

			if (x < owner.Left + _margin.X)
				x = owner.Left + _margin.X;
			else
			{
				if (x + window.Width > owner.Left + owner.Width - _margin.X)
					x = owner.Left + owner.Width - window.Width - _margin.X;
			}

			if (y < owner.Top + _margin.Y)
				y = owner.Top + _margin.Y;
			else
			{
				if (y + window.Height > owner.Top + owner.Height - _margin.Y)
					y = owner.Top + owner.Height - window.Height - _margin.Y;
			}
		}


		public void SetAligns(Point position)
		{
			if (position.X < width3) // X - Left
			{
				window.AlignX = AlignmentX.Left;
			}
			else
			{
				// X > Left ->  Right || Center 
				window.AlignX = position.X > width32 ? AlignmentX.Right : AlignmentX.Center;
			}

			if (position.Y < height3) // Y - Top
			{
				window.AlignY = AlignmentY.Top;
			}
			else
			{
				// Y > Top -> Bottom || Center 
				window.AlignY = position.Y > height32 ? AlignmentY.Bottom : AlignmentY.Center;
			}
		}

		private float GetRelatedX(Rect owner, Point point)
		{
			float res = 0;
			AlignmentX style = window.AlignX;
			double left = Math.Abs(owner.Left - window.Left);
			if (style == AlignmentX.Left)
			{
				res = (float)left;
			}
			else if (style == AlignmentX.Center)
			{
				res = (float)((point.X - (point.X - left)) - width);
			}
			else if (style == AlignmentX.Right)
			{
				res = (float)(owner.Width - (left + window.Width));
			}

			res /= unitx;
			return res;
		}

		private float GetRelatedY(Rect owner, Point point)
		{
			float res = 0;
			AlignmentY style = window.AlignY;
			double top = Math.Abs(owner.Top - window.Top);
			if (style == AlignmentY.Top)
			{
				res = (float)top;
			}
			else if (style == AlignmentY.Center)
			{
				res = (float)((point.Y - (point.Y - top)) - height);
			}
			else if (style == AlignmentY.Bottom)
			{
				res = (float)(owner.Height - (top + window.Height));
			}

			res /= unity;
			return res;
		}

		public void SaveRelatedPosition(Point point)
		{
			Rect owner = window.GetOwnerRectangle();
			window.RelativePosition = new PointF(GetRelatedX(owner, point), GetRelatedY(owner, point));
			window.SetBorderOutline();
			
			window.ResetPosition();
		}
	}
}