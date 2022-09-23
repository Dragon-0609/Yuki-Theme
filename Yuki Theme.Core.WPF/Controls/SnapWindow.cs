using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using MessageBox = System.Windows.MessageBox;

namespace Yuki_Theme.Core.WPF.Controls
{
	public class SnapWindow : Window
	{
		public const int BORDER_OUTLINE_X   = 10;
		public const int BORDER_OUTLINE_Y = 20;
		public const int PLUGIN_BORDER_OUTLINE_Y = 30;

		public float borderOutlineX = BORDER_OUTLINE_X;
		public float borderOutlineY = BORDER_OUTLINE_Y;

		public Window target;
		public Form   targetForm;
		
		private float           unitx;
		private float           unity;

		public AlignmentX AlignX = AlignmentX.Left;
		public AlignmentY AlignY = AlignmentY.Top;

		private FormWindowState _formWindowState = FormWindowState.Normal;

		private bool lockState = false;

		private Rect _currentRect;

		protected bool CanUsePercents;

		public void ResetPosition ()
		{
			if (target != null || targetForm != null)
			{
				_currentRect = GetOwnerRectangle();
				
				if (CanUsePercents && Settings.unit == RelativeUnit.Percent)
				{
					Rect rect = GetOwnerRectangle();
					unitx = (float) (rect.Width / 100f);
					unity = (float) (rect.Height / 100f);
				}

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
			if (Helper.mode == ProductMode.Plugin && borderOutlineY == BORDER_OUTLINE_Y)
				borderOutlineY = PLUGIN_BORDER_OUTLINE_Y;
		}


		#region Get

		private double GetX ()
		{
			double res = 0;
			double left = _currentRect.X;
			double posX = borderOutlineX * (CanUsePercents && Settings.unit == RelativeUnit.Percent ?  unitx : 1);
			if (AlignX == AlignmentX.Left)
			{
				res = left + posX;
			} else
			{
				double width = _currentRect.Width;
				if (AlignX == AlignmentX.Center)
				{
					res = left + (width / 2) + (borderOutlineX == BORDER_OUTLINE_X ? 0 : posX);
				} else
				{
					res = left + width - RenderSize.Width - posX;
				}
			}
			
			return res;
		}

		private double GetY ()
		{
			double res = 0;
			double top = _currentRect.Y;
			double posY = borderOutlineY * (CanUsePercents && Settings.unit == RelativeUnit.Percent ? unity : 1);
			if (AlignY == AlignmentY.Top)
			{
				res = top + posY;
			} else
			{
				double height = _currentRect.Height;
				if (AlignY == AlignmentY.Center)
				{
					res = top + (height / 2) + (borderOutlineY == BORDER_OUTLINE_Y ? 0 : posY);
				} else
				{
					res = top + height - RenderSize.Height - posY;
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
					// ShowWindowOverWindow ();
					ResetPosition ();
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

		public void SetOwner (Window parent)
		{
			target = parent;
			Owner = parent;
		}

		public void SetOwner (Form parent)
		{
			targetForm = parent;
			WindowInteropHelper helper = new WindowInteropHelper (this)
			{
				Owner = parent.Handle
			};
		}
		
		internal Rect GetOwnerRectangle ()
		{
			if (target != null)
				return Owner.GetAbsoluteRect ();
			if (targetForm != null)
				return targetForm.ClientRectangle.ToRect (targetForm);

			throw new NullReferenceException ("Owner wasn't set");
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