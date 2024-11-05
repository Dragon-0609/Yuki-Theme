using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace YukiTheme.Tools;

public static class WindowExtension
{
	private static Int32Rect _getOsInteropRect(Window w)
	{
		var multimonSupported = OSInterop.GetSystemMetrics(OSInterop.SM_CMONITORS) != 0;
		if (!multimonSupported)
		{
			var rc = new OSInterop.RECT();
			OSInterop.SystemParametersInfo(48, 0, ref rc, 0);
			return new Int32Rect(rc.left, rc.top, rc.width, rc.height);
		}

		var helper = new WindowInteropHelper(w);
		var hmonitor = OSInterop.MonitorFromWindow(new HandleRef(null, helper.EnsureHandle()), 2);
		var info = new OSInterop.MONITORINFOEX();
		OSInterop.GetMonitorInfo(new HandleRef(null, hmonitor), info);
		return new Int32Rect(info.rcWork.left, info.rcWork.top, info.rcWork.width, info.rcWork.height);
	}

	public static Rect GetAbsoluteRect(this Window w)
	{
		if (w.WindowState != WindowState.Maximized)
			return new Rect(w.Left, w.Top, w.ActualWidth, w.ActualHeight);

		var r = _getOsInteropRect(w);
		return new Rect(r.X, r.Y, r.Width, r.Height);
	}

	public static Point GetAbsolutePosition(this Window w)
	{
		if (w.WindowState != WindowState.Maximized)
			return new Point(w.Left, w.Top);

		var r = _getOsInteropRect(w);
		return new Point(r.X, r.Y);
	}

	private static class OSInterop
	{
		public const int SM_CMONITORS = 80;

		[DllImport("user32.dll")]
		public static extern int GetSystemMetrics(int smIndex);

		[DllImport("user32.dll")]
		public static extern bool SystemParametersInfo(int nAction, int nParam, ref RECT rc, int nUpdate);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetMonitorInfo(HandleRef hmonitor, [In] [Out] MONITORINFOEX info);

		[DllImport("user32.dll")]
		public static extern IntPtr MonitorFromWindow(HandleRef handle, int flags);

		public struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;

			public int width => right - left;

			public int height => bottom - top;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
		public class MONITORINFOEX
		{
			public int cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));

			public int dwFlags;
			public RECT rcMonitor = new();
			public RECT rcWork;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public char[] szDevice = new char[32];
		}
	}
}