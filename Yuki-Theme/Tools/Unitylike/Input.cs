using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace YukiTheme.Tools;

// ReSharper disable InconsistentNaming
public enum KeyCode
{
	None = 0x00,

	Backspace = 0x08,
	Tab = 0x09,
	Return = 0x0D,
	Escape = 0x1B,
	Space = 0x20,

	PageUp = 0x21,
	PageDown = 0x22,
	End = 0x23,
	Home = 0x24,
	LeftArrow = 0x25,
	UpArrow = 0x26,
	RightArrow = 0x27,
	DownArrow = 0x28,
	Insert = 0x2D,
	Delete = 0x2E,

	Alpha0 = 0x30,
	Alpha1 = 0x31,
	Alpha2 = 0x32,
	Alpha3 = 0x33,
	Alpha4 = 0x34,
	Alpha5 = 0x35,
	Alpha6 = 0x36,
	Alpha7 = 0x37,
	Alpha8 = 0x38,
	Alpha9 = 0x39,

	A = 0x41,
	B = 0x42,
	C = 0x43,
	D = 0x44,
	E = 0x45,
	F = 0x46,
	G = 0x47,
	H = 0x48,
	I = 0x49,
	J = 0x4A,
	K = 0x4B,
	L = 0x4C,
	M = 0x4D,
	N = 0x4E,
	O = 0x4F,
	P = 0x50,
	Q = 0x51,
	R = 0x52,
	S = 0x53,
	T = 0x54,
	U = 0x55,
	V = 0x56,
	W = 0x57,
	X = 0x58,
	Y = 0x59,
	Z = 0x5A,

	LeftWindows = 0x5B,
	RightWindows = 0x5C,

	Keypad0 = 0x60,
	Keypad1 = 0x61,
	Keypad2 = 0x62,
	Keypad3 = 0x63,
	Keypad4 = 0x64,
	Keypad5 = 0x65,
	Keypad6 = 0x66,
	Keypad7 = 0x67,
	Keypad8 = 0x68,
	Keypad9 = 0x69,
	KeypadMultiply = 0x6A,
	KeypadPlus = 0x6B,
	KeypadMinus = 0x6D,
	KeypadPeriod = 0x6E,
	KeypadDivide = 0x6F,

	F1 = 0x70,
	F2 = 0x71,
	F3 = 0x72,
	F4 = 0x73,
	F5 = 0x74,
	F6 = 0x75,
	F7 = 0x76,
	F8 = 0x77,
	F9 = 0x78,
	F10 = 0x79,
	F11 = 0x7A,
	F12 = 0x7B,

	LeftShift = 0xA0,
	RightShift = 0xA1,
	LeftControl = 0xA2,
	RightControl = 0xA3,
	LeftAlt = 0xA4,
	RightAlt = 0xA5
}

public class Input
{
	private const int STATE_COUNT = 256;
	private const int KEY_DOWN_BIT = 0x8000;

	private static readonly int[] MouseButtonVK = { 0x01, 0x02, 0x04 };

	private static readonly int CurrentProcessId = Process.GetCurrentProcess().Id;

	private readonly bool[] _current = new bool[STATE_COUNT];
	private readonly bool[] _previous = new bool[STATE_COUNT];

	[DllImport("user32.dll")]
	private static extern short GetAsyncKeyState(int vKey);

	[DllImport("user32.dll")]
	private static extern IntPtr GetForegroundWindow();

	[DllImport("user32.dll")]
	private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);

	public Input()
	{
		Poll();
		Array.Copy(_current, _previous, STATE_COUNT);
	}

	public Point MousePosition => Cursor.Position;

	public bool GetKey(KeyCode key) => _current[(int)key];

	public bool GetKeyDown(KeyCode key) => _current[(int)key] && !_previous[(int)key];

	public bool GetKeyUp(KeyCode key) => !_current[(int)key] && _previous[(int)key];

	public bool GetMouseButton(int button) => IsDown(_current, MouseVK(button));

	public bool GetMouseButtonDown(int button) => IsDown(_current, MouseVK(button)) && !IsDown(_previous, MouseVK(button));

	public bool GetMouseButtonUp(int button) => !IsDown(_current, MouseVK(button)) && IsDown(_previous, MouseVK(button));

	internal void Poll()
	{
		Array.Copy(_current, _previous, STATE_COUNT);

		bool focused = IsApplicationFocused();
		for (int i = 0; i < STATE_COUNT; i++)
			_current[i] = focused && (GetAsyncKeyState(i) & KEY_DOWN_BIT) != 0;
	}

	private static bool IsApplicationFocused()
	{
		IntPtr foreground = GetForegroundWindow();
		if (foreground == IntPtr.Zero) return false;

		GetWindowThreadProcessId(foreground, out int processId);
		return processId == CurrentProcessId;
	}

	private static int MouseVK(int button) => button >= 0 && button < MouseButtonVK.Length ? MouseButtonVK[button] : 0;

	private static bool IsDown(bool[] state, int vKey) => vKey != 0 && state[vKey];
}