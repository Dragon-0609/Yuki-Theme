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
		public const int BORDER_OUTLINE = 10;
			
		public float borderOutlineX = BORDER_OUTLINE;
		public float borderOutlineY = BORDER_OUTLINE;
		
		public Window target;
		public Form   targetForm;
		
		public AlignmentX AlignX = AlignmentX.Left;
		public AlignmentY AlignY = AlignmentY.Top;
		
		private FormWindowState _formWindowState = FormWindowState.Normal;

		private bool lockState = false;
		
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
			Loaded += SnapWindow_OnLoaded;
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
				res = left + borderOutlineX;
			}else
			{
				double width = GetWidth ();
				if (AlignX == AlignmentX.Center)
				{
					res = left + (width / 2) + (borderOutlineX == BORDER_OUTLINE ? 0 : borderOutlineX);
				} else
				{
					res = left + width - RenderSize.Width - borderOutlineX;
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
				res = top + borderOutlineY;
			}else
			{
				double height = GetHeight ();
				if (AlignY == AlignmentY.Center)
				{
					res = top + (height / 2) + (borderOutlineY == BORDER_OUTLINE ? 0 : borderOutlineY);
				} else
				{
					res = top + height - RenderSize.Height - borderOutlineY;
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
			if (!lockState)
			{
				if (target != null)
					OnWindowStateChanged (sender, e);
				else
					OnFormStateChanged (sender, e);
			}
		}

		private void OnWindowStateChanged (object sender, EventArgs e)
		{
			if (target.WindowState == WindowState.Minimized)
				WindowState = WindowState.Minimized;
			else
			{
				WindowState = WindowState.Normal;
				if (target.WindowState == WindowState.Maximized)
				{
					ShowWindowOverWindow ();
				}
			}
			
			PositionChanged (sender, e);
		}

		private void ShowWindowOverWindow ()
		{
			lockState = true;
			target.WindowState = WindowState.Minimized;
			WindowState = WindowState.Minimized;
			target.WindowState = WindowState.Maximized;
			WindowState = WindowState.Normal;
			lockState = false;
		}

		private void OnFormStateChanged (object sender, EventArgs e)
		{
			if (targetForm.WindowState == FormWindowState.Minimized)
				WindowState = WindowState.Minimized;
			else
			{
				WindowState = WindowState.Normal;
				if (targetForm.WindowState == FormWindowState.Maximized)
				{
					
				}
			}

			PositionChanged (sender, e);
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