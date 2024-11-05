using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;

namespace YukiTheme.Style.Helpers;

[StructLayout(LayoutKind.Sequential)]
public struct POINT
{
	public int X;
	public int Y;

	public POINT(int x, int y)
	{
		X = x;
		Y = y;
	}

	public POINT(Point pt) : this(pt.X, pt.Y)
	{
	}

	public static implicit operator Point(POINT p)
	{
		return new Point(p.X, p.Y);
	}

	public static implicit operator POINT(Point p)
	{
		return new POINT(p.X, p.Y);
	}
}

internal static class NativeMethods
{
	public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool DragDetect(IntPtr hWnd, POINT pt);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr GetFocus();

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr SetFocus(IntPtr hWnd);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern uint SendMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern int ShowWindow(IntPtr hWnd, int cmdShow);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int X, int Y, int Width, int Height, FlagsSetWindowPos flags);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern int GetWindowLong(IntPtr hWnd, int Index);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern int SetWindowLong(IntPtr hWnd, int Index, int Value);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern int ShowScrollBar(IntPtr hWnd, int wBar, int bShow);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	[SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
	public static extern IntPtr WindowFromPoint(Point point);

	[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
	public static extern int GetCurrentThreadId();

	[DllImport("user32.dll")]
	public static extern IntPtr SetWindowsHookEx(HookType code, HookProc func, IntPtr hInstance, int threadID);

	[DllImport("user32.dll")]
	public static extern int UnhookWindowsHookEx(IntPtr hhook);

	[DllImport("user32.dll")]
	public static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wParam, IntPtr lParam);

	public static bool IsUnix()
	{
		return Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX;
	}
}