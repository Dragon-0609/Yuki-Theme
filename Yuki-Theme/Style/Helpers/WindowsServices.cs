using System;
using System.Runtime.InteropServices;

namespace YukiTheme.Style.Helpers;

public static class WindowsServices
{
	private const int WS_EX_TRANSPARENT = 0x00000020;
	private const int GWL_EXSTYLE = -20;

	[DllImport("user32.dll")]
	private static extern int GetWindowLong(IntPtr hwnd, int index);

	[DllImport("user32.dll")]
	private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

	public static void SetWindowExTransparent(IntPtr hwnd)
	{
		var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
		SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
	}
}