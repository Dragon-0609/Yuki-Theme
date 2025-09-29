using System;
using System.Runtime.InteropServices;

namespace YukiTheme.Tools
{
	// ReSharper disable once InconsistentNaming
	public static class DPICalculator
	{

		[DllImport("user32.dll")]
		static extern IntPtr GetDC(IntPtr hWnd);

		[DllImport("gdi32.dll")]
		static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

		[DllImport("user32.dll")]
		static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

		const int LOGPIXELSX = 88;

		public static float GetScalingFactor()
		{
			IntPtr hdc = GetDC(IntPtr.Zero);
			int dpi = GetDeviceCaps(hdc, LOGPIXELSX);
			ReleaseDC(IntPtr.Zero, hdc);

			return dpi / 96f; // 1.0 = 100%, 1.25 = 125%, etc.
		}
	}
}