using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using MessageBox = System.Windows.MessageBox;

namespace Yuki_Theme.Core.WPF.Controls
{
	public class SnapWindow : Window
	{
		private const int    BORDER_OUTLINE = 10;
		public        Window target;
		public        Form   targetForm;
		
		public AlignmentX AlignX = AlignmentX.Left;
		public AlignmentY AlignY = AlignmentY.Top;
		
		private FormWindowState _formWindowState = FormWindowState.Normal;

		public void ResetPosition ()
		{
			if (target != null || targetForm != null)
			{
				Left = GetX ();
				Top = GetY ();
			}
		}

		public SnapWindow ()
		{
			this.Loaded += SnapWindow_OnLoaded;
			ResizeMode = ResizeMode.NoResize;
			WindowStyle = WindowStyle.None;
			ShowInTaskbar = false;
		}
		
		
		#region Get

		private double GetX ()
		{
			double res = 0;
			double left = GetLeft ();
			if (AlignX == AlignmentX.Left)
			{
				res = left + BORDER_OUTLINE;
			}else
			{
				double width = GetWidth ();
				if (AlignX == AlignmentX.Center)
				{
					res = left + (width / 2);
				} else
				{
					res = left + width - RenderSize.Width - BORDER_OUTLINE;
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
				res = top + BORDER_OUTLINE;
			}else
			{
				double height = GetHeight ();
				if (AlignY == AlignmentY.Center)
				{
					res = top + (height / 2);
				} else
				{
					res = top + height - RenderSize.Height - BORDER_OUTLINE;
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
			if (target != null)
			{
				target.LocationChanged += PositionChanged;
				target.SizeChanged += PositionChanged;
				target.StateChanged += OnStateChanged;
				target.Closing += OnClosing;
			} else if (targetForm != null)
			{
				targetForm.LocationChanged += PositionChanged;
				targetForm.SizeChanged += PositionOrStateChanged;
				targetForm.Closing += OnClosing;
			}
		}

		private void OnClosing (object sender, CancelEventArgs e)
		{
			UnbindPosition ();
		}

		private void OnStateChanged (object sender, EventArgs e)
		{
			if (target.WindowState == WindowState.Minimized)
				WindowState = WindowState.Minimized;
			else
			{
				WindowState = WindowState.Normal;
				if (target.WindowState == WindowState.Maximized)
				{
					
				}
			}
				

			Console.WriteLine ($"BEFORE: LEFT: {Left}, TOP: {Top}");
			PositionChanged (sender, e);
			Console.WriteLine ($"AFTER:   LEFT: {Left}, TOP: {Top}");
		}

		private void PositionChanged (object sender, EventArgs e)
		{
			ResetPosition ();
		}

		private void PositionOrStateChanged (object sender, EventArgs e)
		{
			if (_formWindowState != targetForm.WindowState)
			{
				_formWindowState = targetForm.WindowState;
				OnStateChanged (sender, e);
			} else
			{
				PositionChanged (sender, e);
			}
		}

		private void UnbindPosition ()
		{
			if (target != null)
			{
				target.LocationChanged -= PositionChanged;
				target.SizeChanged -= PositionChanged;
				target.StateChanged -= OnStateChanged;
			} else if (targetForm != null)
			{
				targetForm.LocationChanged -= PositionChanged;
				targetForm.SizeChanged -= PositionOrStateChanged;
			}
		}

		protected override void OnClosing (CancelEventArgs e)
		{
			UnbindPosition ();
			base.OnClosed (e);
		}
		
		private void SnapWindow_OnLoaded (object sender, RoutedEventArgs e)
		{
			BindPosition ();
		}

		public void SetAlign (SnapWindowAlign align)
		{
			AlignX = align.AlignX;
			AlignY = align.AlignY;
		}
	}

	public class SnapWindowAlign
	{
		public AlignmentX AlignX;
		public AlignmentY AlignY;

		public SnapWindowAlign (AlignmentX x, AlignmentY y)
		{
			AlignX = x;
			AlignY = y;
		}
	}
}