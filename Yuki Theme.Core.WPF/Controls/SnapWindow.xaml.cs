using System;
using System.Windows;
using System.Windows.Media;

namespace Yuki_Theme.Core.WPF.Controls
{
	public partial class SnapWindow : Window
	{
		private Window target;
		private Window targetForm;
		
		public SnapWindow ()
		{
			InitializeComponent ();
		}

		public AlignmentX AlignX = AlignmentX.Left;
		public AlignmentY AlignY = AlignmentY.Top;
		
		public void ResetPosition ()
		{
			Left = GetX ();
			Top = GetY ();
		}

		
		#region Get

		private double GetX ()
		{
			double res = 0;
			double left = GetLeft ();
			if (AlignX == AlignmentX.Left)
			{
				res = left;
			}else
			{
				double width = GetWidth ();
				if (AlignX == AlignmentX.Center)
				{
					res = left + (width / 2);
				} else
				{
					res = left + width - RenderSize.Width;
				}
			}

			return res;
		}

		private double GetY ()
		{
			double res = 0;
			double top = GetTop ();
			if (AlignY == AlignmentY.Top)
			{
				res = top;
			}else
			{
				double height = GetHeight ();
				if (AlignY == AlignmentY.Center)
				{
					res = top + (height / 2);
				} else
				{
					res = top + height - RenderSize.Height;
				}
			}

			return res;
		}
		

		private double GetLeft ()
		{
			if (target != null)
				return target.Left;
			else
				return targetForm.Left;
		}

		private double GetTop ()
		{
			if (target != null)
				return target.Top;
			else
				return targetForm.Top;
		}
		
		

		private double GetWidth ()
		{
			if (target != null)
				return target.RenderSize.Width;
			else
				return targetForm.Width;
		}

		private double GetHeight ()
		{
			if (target != null)
				return target.RenderSize.Height;
			else
				return targetForm.Height;
		}

		#endregion

		
		private void BindPosition ()
		{
			target.LocationChanged += PositionChanged;
			target.SizeChanged += PositionChanged;
			target.StateChanged += OnStateChanged;
		}

		private void OnStateChanged (object sender, EventArgs e)
		{
			if (target.WindowState == WindowState.Minimized)
				WindowState = WindowState.Minimized;
			else if (target.WindowState == WindowState.Maximized)
				ResetPosition ();
			else
				WindowState = WindowState.Normal;
		}

		private void PositionChanged (object sender, EventArgs e)
		{
			ResetPosition ();
		}

		private void UnbindPosition ()
		{
			target.LocationChanged -= PositionChanged;
			target.SizeChanged -= PositionChanged;
			target.StateChanged -= OnStateChanged;
		}

		private void SnapWindow_OnLoaded (object sender, RoutedEventArgs e)
		{
			BindPosition ();
		}

		private void SnapWindow_OnClosed (object sender, EventArgs e)
		{
			UnbindPosition ();
		}
	}
}