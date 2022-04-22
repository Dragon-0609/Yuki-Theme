using System;

namespace CLITools.Abstractions
{
	internal class Console2 : IConsole
	{
		public int CursorLeft
		{
			get { return Console.CursorLeft; }
		}

		public int CursorTop
		{
			get { return Console.CursorTop; }
		}

		public int BufferWidth
		{
			get { return Console.BufferWidth; }
		}

		public int BufferHeight
		{
			get { return Console.BufferHeight; }
		}

		public bool PasswordMode { get; set; }

		public void SetBufferSize (int width, int height)
		{
			Console.SetBufferSize (width, height);
		}

		public void SetCursorPosition (int left, int top)
		{
			if (!PasswordMode)
				Console.SetCursorPosition (left, top);
		}

		public void Write (string value)
		{
			if (PasswordMode)
				value = new String (default (char), value.Length);

			Console.Write (value);
		}

		public void WriteLine (string value)
		{
			Console.WriteLine (value);
		}
	}
}