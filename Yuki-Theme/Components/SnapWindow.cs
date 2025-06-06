﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using YukiTheme.Tools;

namespace YukiTheme.Components;

public class SnapWindow : Window
{
	internal const int BORDER_OUTLINE_X = 10;
	internal const int BORDER_OUTLINE_Y = 20;
	private const int PLUGIN_BORDER_OUTLINE_Y = 30;

	private Rect _currentRect;

	private FormWindowState _formWindowState = FormWindowState.Normal;

	private bool _lockState;

	private float _unitx;
	private float _unity;

	internal AlignmentX AlignX = AlignmentX.Left;
	internal AlignmentY AlignY = AlignmentY.Top;

	internal bool AutoSize;

	internal float BorderOutlineX = BORDER_OUTLINE_X;
	internal float BorderOutlineY = BORDER_OUTLINE_Y;

	protected bool CanUsePercents;

	protected Window Target;
	protected Form TargetForm;

	public SnapWindow()
	{
		Loaded += SnapWindow_OnLoaded;
		ResizeMode = ResizeMode.NoResize;
		WindowStyle = WindowStyle.None;
		ShowInTaskbar = false;
		BorderOutlineY = PLUGIN_BORDER_OUTLINE_Y;
	}

	internal void ResetPosition()
	{
		if (Target != null || TargetForm != null)
		{
			_currentRect = GetOwnerRectangle();

			if (CanUsePercents) //&& Settings.unit == RelativeUnit.Percent
			{
				_unitx = (float)(_currentRect.Width / 100f);
				_unity = (float)(_currentRect.Height / 100f);
			}

			Left = GetX();
			Top = GetY();
			if (AutoSize)
			{
				Width = _currentRect.Width;
				Height = _currentRect.Height;
			}
		}
	}


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
		{
			WindowState = WindowState.Minimized;
		}
		else
		{
			WindowState = WindowState.Normal;
			if (Target.WindowState == WindowState.Maximized)
				// ShowWindowOverWindow ();
				ResetPosition();
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
		{
			WindowState = WindowState.Minimized;
		}
		else
		{
			WindowState = WindowState.Normal;
			if (TargetForm.WindowState == FormWindowState.Maximized)
			{
				// WindowState = WindowState.Maximized;
				TargetForm.Focus();
#if LOG
					Console.WriteLine("Maximized");
#endif
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

	internal void SetAlign(SnapWindowAlign align)
	{
		AlignX = align.AlignX;
		AlignY = align.AlignY;
	}

	internal void SetOwner(Window parent)
	{
		Target = parent;
		try
		{
			Owner = parent;
		}
		catch (InvalidOperationException e)
		{
#if LOG
				Console.WriteLine(e);
#endif
		}
	}

	internal void SetOwner(Form parent)
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
				var area = Screen.GetWorkingArea(TargetForm);
				return area.ToRect();
			}

			return new Rectangle(TargetForm.Location.X + 2, TargetForm.Location.Y, TargetForm.Size.Width, TargetForm.Size.Height).ToRect();
		}
		// return targetForm.ClientRectangle.ToRect(targetForm);

		throw new NullReferenceException("Owner wasn't set");
	}


	#region Get

	private double GetX()
	{
		double res = 0;
		var left = _currentRect.X;
		double posX = BorderOutlineX * (CanUsePercents ? _unitx : 1);
		if (AlignX == AlignmentX.Left)
		{
			res = left + posX;
		}
		else
		{
			var width = _currentRect.Width;
			if (AlignX == AlignmentX.Center)
				res = left + width / 2 + (BorderOutlineX == BORDER_OUTLINE_X ? 0 : posX);
			else
				res = left + width - RenderSize.Width - posX;
		}

		return res;
	}

	private double GetY()
	{
		double res = 0;
		var top = _currentRect.Y;
		double posY = BorderOutlineY * (CanUsePercents ? _unity : 1);
		if (AlignY == AlignmentY.Top)
		{
			res = top + posY;
		}
		else
		{
			var height = _currentRect.Height;
			if (AlignY == AlignmentY.Center)
				res = top + height / 2 + (BorderOutlineY == BORDER_OUTLINE_Y ? 0 : posY);
			else
				res = top + height - RenderSize.Height - posY;
		}

		return res;
	}


	private double GetLeft()
	{
		if (Target != null)
			return Target.Left;
		return TargetForm.Left;
	}

	private double GetTop()
	{
		if (Target != null)
			return Target.Top;
		return TargetForm.Top;
	}


	private double GetWidth()
	{
		if (Target != null)
			return Target.RenderSize.Width;
		return TargetForm.Width;
	}

	private double GetHeight()
	{
		if (Target != null)
			return Target.RenderSize.Height;
		return TargetForm.Height;
	}

	#endregion
}