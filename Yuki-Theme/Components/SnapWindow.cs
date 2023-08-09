using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using YukiTheme.Tools;

namespace YukiTheme.Components
{
	public class SnapWindow : Window
	{
		public const int BORDER_OUTLINE_X = 10;
		public const int BORDER_OUTLINE_Y = 20;
		public const int PLUGIN_BORDER_OUTLINE_Y = 30;

		public float BorderOutlineX = BORDER_OUTLINE_X;
		public float BorderOutlineY = BORDER_OUTLINE_Y;

		public Window Target;
		public Form TargetForm;

		private float _unitx;
		private float _unity;

		public AlignmentX AlignX = AlignmentX.Left;
		public AlignmentY AlignY = AlignmentY.Top;

		private FormWindowState _formWindowState = FormWindowState.Normal;

		private bool _lockState = false;

		private Rect _currentRect;

		protected bool CanUsePercents;

		public bool AutoSize;

		public void ResetPosition()
		{
			if (Target != null || TargetForm != null)
			{
				_currentRect = GetOwnerRectangle();

				if (CanUsePercents) //&& Settings.unit == RelativeUnit.Percent
				{
					Rect rect = GetOwnerRectangle();
					_unitx = (float)(rect.Width / 100f);
					_unity = (float)(rect.Height / 100f);
				}

				Left = GetX();
				Top = GetY();
				if (AutoSize)
				{
					Width = _currentRect.Width;
					Height = _currentRect.Height;
				}

				// Console.WriteLine($"x: {Left}, y: {Top}, width: {Width}, height: {Height}");
			}
		}

		public SnapWindow()
		{
			Loaded += SnapWindow_OnLoaded;
			ResizeMode = ResizeMode.NoResize;
			WindowStyle = WindowStyle.None;
			ShowInTaskbar = false;
			BorderOutlineY = PLUGIN_BORDER_OUTLINE_Y;
		}


		#region Get

		private double GetX()
		{
			double res = 0;
			double left = _currentRect.X;
			double posX = BorderOutlineX; // CanUsePercents && Settings.unit == RelativeUnit.Percent ? unitx : * 1; 
			if (AlignX == AlignmentX.Left)
			{
				res = left + posX;
			}
			else
			{
				double width = _currentRect.Width;
				if (AlignX == AlignmentX.Center)
				{
					res = left + (width / 2) + (BorderOutlineX == BORDER_OUTLINE_X ? 0 : posX);
				}
				else
				{
					res = left + width - RenderSize.Width - posX;
				}
			}

			return res;
		}

		private double GetY()
		{
			double res = 0;
			double top = _currentRect.Y;
			double posY = BorderOutlineY; // * (CanUsePercents && Settings.unit == RelativeUnit.Percent ? unity : 1);
			if (AlignY == AlignmentY.Top)
			{
				res = top + posY;
			}
			else
			{
				double height = _currentRect.Height;
				if (AlignY == AlignmentY.Center)
				{
					res = top + (height / 2) + (BorderOutlineY == BORDER_OUTLINE_Y ? 0 : posY);
				}
				else
				{
					res = top + height - RenderSize.Height - posY;
				}
			}

			return res;
		}


		private double GetLeft()
		{
			if (Target != null)
				return Target.Left;
			else
				return TargetForm.Left;
		}

		private double GetTop()
		{
			if (Target != null)
				return Target.Top;
			else
				return TargetForm.Top;
		}


		private double GetWidth()
		{
			if (Target != null)
				return Target.RenderSize.Width;
			else
				return TargetForm.Width;
		}

		private double GetHeight()
		{
			if (Target != null)
				return Target.RenderSize.Height;
			else
				return TargetForm.Height;
		}

		#endregion


		private void BindPosition()
		{
			if (Target != null)
			{
				Target.LocationChanged += PositionChanged;
				Target.SizeChanged += PositionChanged;
				Target.StateChanged += OnStateChanged;
				Target.Closing += OnClosing;
			}
			else if (TargetForm != null)
			{
				TargetForm.LocationChanged += PositionChanged;
				TargetForm.SizeChanged += PositionOrStateChanged;
				TargetForm.Closing += OnClosing;
			}
		}

		private void OnClosing(object sender, CancelEventArgs e)
		{
			UnbindPosition();
		}

		private void OnStateChanged(object sender, EventArgs e)
		{
			if (!_lockState)
			{
				if (Target != null)
					OnWindowStateChanged(sender, e);
				else
					OnFormStateChanged(sender, e);
			}
		}

		private void OnWindowStateChanged(object sender, EventArgs e)
		{
			if (Target.WindowState == WindowState.Minimized)
				WindowState = WindowState.Minimized;
			else
			{
				WindowState = WindowState.Normal;
				if (Target.WindowState == WindowState.Maximized)
				{
					// ShowWindowOverWindow ();
					ResetPosition();
				}
			}

			PositionChanged(sender, e);
		}

		private void ShowWindowOverWindow()
		{
			_lockState = true;
			Target.WindowState = WindowState.Minimized;
			WindowState = WindowState.Minimized;
			Target.WindowState = WindowState.Maximized;
			WindowState = WindowState.Normal;
			_lockState = false;
		}

		private void OnFormStateChanged(object sender, EventArgs e)
		{
			if (TargetForm.WindowState == FormWindowState.Minimized)
				WindowState = WindowState.Minimized;
			else
			{
				WindowState = WindowState.Normal;
				if (TargetForm.WindowState == FormWindowState.Maximized)
				{
					// WindowState = WindowState.Maximized;
					TargetForm.Focus();
					Console.WriteLine("Maximized");
				}
			}

			PositionChanged(sender, e);
		}

		private void PositionChanged(object sender, EventArgs e)
		{
			ResetPosition();
		}

		private void PositionOrStateChanged(object sender, EventArgs e)
		{
			if (_formWindowState != TargetForm.WindowState)
			{
				_formWindowState = TargetForm.WindowState;
				OnStateChanged(sender, e);
			}
			else
			{
				PositionChanged(sender, e);
			}
		}

		private void UnbindPosition()
		{
			if (Target != null)
			{
				Target.LocationChanged -= PositionChanged;
				Target.SizeChanged -= PositionChanged;
				Target.StateChanged -= OnStateChanged;
			}
			else if (TargetForm != null)
			{
				TargetForm.LocationChanged -= PositionChanged;
				TargetForm.SizeChanged -= PositionOrStateChanged;
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			UnbindPosition();
			base.OnClosed(e);
		}

		private void SnapWindow_OnLoaded(object sender, RoutedEventArgs e)
		{
			BindPosition();
		}

		public void SetAlign(SnapWindowAlign align)
		{
			AlignX = align.AlignX;
			AlignY = align.AlignY;
		}

		public void SetOwner(Window parent)
		{
			Target = parent;
			try
			{
				Owner = parent;
			}
			catch (InvalidOperationException e)
			{
				Console.WriteLine(e);
			}
		}

		public void SetOwner(Form parent)
		{
			TargetForm = parent;
			TargetForm.LostFocus += (sender, args) => { Topmost = false; };
			TargetForm.GotFocus += (sender, args) => { Topmost = true; };
			new WindowInteropHelper(this)
			{
				Owner = parent.Handle
			};
		}

		internal Rect GetOwnerRectangle()
		{
			if (Target != null)
				return Owner.GetAbsoluteRect();
			if (TargetForm != null)
			{
				if (TargetForm.WindowState == FormWindowState.Maximized)
				{
					// var size = targetForm.ClientSize;
					// return new Rect(0, 0, size.Width, size.Height);
					Rectangle area = Screen.GetWorkingArea(TargetForm);
					return area.ToRect();
				}

				return new Rectangle(TargetForm.Location.X + 2, TargetForm.Location.Y, TargetForm.Size.Width, TargetForm.Size.Height).ToRect();
			}
			// return targetForm.ClientRectangle.ToRect(targetForm);

			throw new NullReferenceException("Owner wasn't set");
		}
		

		internal void FocusBack(object sender, MouseButtonEventArgs e)
		{
			if (Target != null)
				Target.Focus();
			else if (TargetForm != null)
				TargetForm.Focus();
		}
	}
}