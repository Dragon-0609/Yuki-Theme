using CLITools.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLITools
{
	internal class KeyHandler
	{
		private int                         _cursorPos;
		private int                         _cursorLimit;
		private StringBuilder               _text;
		private List <string>               _history;
		private int                         _historyIndex;
		private ConsoleKeyInfo              _keyInfo;
		private Dictionary <string, Action> _keyActions;
		private string []                   _completions;
		private int                         _completionStart;
		private int                         _completionsIndex;
		private IConsole                    Console2;

		private bool IsStartOfLine ()
		{
			return _cursorPos == 0;
		}

		private bool IsEndOfLine ()
		{
			return _cursorPos == _cursorLimit;
		}

		private bool IsStartOfBuffer ()
		{
			return Console2.CursorLeft == 0;
		}

		private bool IsEndOfBuffer ()
		{
			return Console2.CursorLeft == Console2.BufferWidth - 1;
		}

		private bool IsInAutoCompleteMode ()
		{
			return _completions != null;
		}

		private void MoveCursorLeft ()
		{
			MoveCursorLeft (1);
		}

		private void MoveCursorLeft (int count)
		{
			if (count > _cursorPos)
				count = _cursorPos;

			if (count > Console2.CursorLeft)
				Console2.SetCursorPosition (Console2.BufferWidth - 1, Console2.CursorTop - 1);
			else
				Console2.SetCursorPosition (Console2.CursorLeft - count, Console2.CursorTop);

			_cursorPos -= count;
		}

		private void MoveCursorHome ()
		{
			while (!IsStartOfLine ())
				MoveCursorLeft ();
		}

		private string BuildKeyInput ()
		{
			return (_keyInfo.Modifiers != ConsoleModifiers.Control && _keyInfo.Modifiers != ConsoleModifiers.Shift)
				? _keyInfo.Key.ToString ()
				: _keyInfo.Modifiers.ToString () + _keyInfo.Key.ToString ();
		}

		private void MoveCursorRight ()
		{
			if (IsEndOfLine ())
				return;

			if (IsEndOfBuffer ())
				Console2.SetCursorPosition (0, Console2.CursorTop + 1);
			else
				Console2.SetCursorPosition (Console2.CursorLeft + 1, Console2.CursorTop);

			_cursorPos++;
		}

		private void MoveCursorEnd ()
		{
			while (!IsEndOfLine ())
				MoveCursorRight ();
		}

		private void ClearLine ()
		{
			MoveCursorEnd ();
			Backspace (_cursorPos);
		}

		private void WriteNewString (string str)
		{
			ClearLine ();
			foreach (char character in str)
				WriteChar (character);
		}

		private void WriteString (string str)
		{
			foreach (char character in str)
				WriteChar (character);
		}

		private void WriteChar ()
		{
			WriteChar (_keyInfo.KeyChar);
		}

		private void WriteChar (char c)
		{
			if (IsEndOfLine ())
			{
				_text.Append (c);
				Console2.Write (c.ToString ());
				_cursorPos++;
			} else
			{
				int left = Console2.CursorLeft;
				int top = Console2.CursorTop;
				string str = _text.ToString ().Substring (_cursorPos);
				_text.Insert (_cursorPos, c);
				Console2.Write (c.ToString () + str);
				Console2.SetCursorPosition (left, top);
				MoveCursorRight ();
			}

			_cursorLimit++;
		}

		private void Backspace ()
		{
			Backspace (1);
		}

		private void Backspace (int count)
		{
			if (count > _cursorPos)
				count = _cursorPos;

			MoveCursorLeft (count);
			int index = _cursorPos;
			_text.Remove (index, count);
			string replacement = _text.ToString ().Substring (index);
			int left = Console2.CursorLeft;
			int top = Console2.CursorTop;
			string spaces = new string (' ', count);
			Console2.Write (string.Format ("{0}{1}", replacement, spaces));
			Console2.SetCursorPosition (left, top);
			_cursorLimit -= count;
		}

		private void Delete ()
		{
			if (IsEndOfLine ())
				return;

			int index = _cursorPos;
			_text.Remove (index, 1);
			string replacement = _text.ToString ().Substring (index);
			int left = Console2.CursorLeft;
			int top = Console2.CursorTop;
			Console2.Write (string.Format ("{0} ", replacement));
			Console2.SetCursorPosition (left, top);
			_cursorLimit--;
		}

		private void TransposeChars ()
		{
			// local helper functions

			if (IsStartOfLine ()) { return; }

			var firstIdx = decrementIf (IsEndOfLine, _cursorPos - 1);
			var secondIdx = decrementIf (IsEndOfLine, _cursorPos);

			var secondChar = _text [secondIdx];
			_text [secondIdx] = _text [firstIdx];
			_text [firstIdx] = secondChar;

			var left = incrementIf (almostEndOfLine, Console2.CursorLeft);
			var cursorPosition = incrementIf (almostEndOfLine, _cursorPos);

			WriteNewString (_text.ToString ());

			Console2.SetCursorPosition (left, Console2.CursorTop);
			_cursorPos = cursorPosition;

			MoveCursorRight ();
		}

		private int decrementIf (Func <bool> expression, int index)
		{
			return expression () ? index - 1 : index;
		}

		private int incrementIf (Func <bool> expression, int index)
		{
			return expression () ? index + 1 : index;
		}

		private bool almostEndOfLine ()
		{
			return (_cursorLimit - _cursorPos) == 1;
		}

		private void StartAutoComplete ()
		{
			Backspace (_cursorPos - _completionStart);

			_completionsIndex = 0;

			WriteString (_completions [_completionsIndex]);
		}

		private void NextAutoComplete ()
		{
			Backspace (_cursorPos - _completionStart);

			_completionsIndex++;

			if (_completionsIndex == _completions.Length)
				_completionsIndex = 0;

			WriteString (_completions [_completionsIndex]);
		}

		private void PreviousAutoComplete ()
		{
			Backspace (_cursorPos - _completionStart);

			_completionsIndex--;

			if (_completionsIndex == -1)
				_completionsIndex = _completions.Length - 1;

			WriteString (_completions [_completionsIndex]);
		}

		private void PrevHistory ()
		{
			if (_historyIndex > 0)
			{
				_historyIndex--;
				WriteNewString (_history [_historyIndex]);
			}
		}

		private void NextHistory ()
		{
			if (_historyIndex < _history.Count)
			{
				_historyIndex++;
				if (_historyIndex == _history.Count)
					ClearLine ();
				else
					WriteNewString (_history [_historyIndex]);
			}
		}

		private void ResetAutoComplete ()
		{
			_completions = null;
			_completionsIndex = 0;
		}

		public string Text
		{
			get { return _text.ToString (); }
		}

		public KeyHandler (IConsole console, List <string> history, IAutoCompleteHandler autoCompleteHandler)
		{
			Console2 = console;

			_history = history ?? new List <string> ();
			_historyIndex = _history.Count;
			_text = new StringBuilder ();
			_keyActions = new Dictionary <string, Action> ();

			_keyActions ["LeftArrow"] = MoveCursorLeft;
			_keyActions ["Home"] = MoveCursorHome;
			_keyActions ["End"] = MoveCursorEnd;
			_keyActions ["ControlA"] = MoveCursorHome;
			_keyActions ["ControlB"] = MoveCursorLeft;
			_keyActions ["RightArrow"] = MoveCursorRight;
			_keyActions ["ControlF"] = MoveCursorRight;
			_keyActions ["ControlE"] = MoveCursorEnd;
			_keyActions ["Backspace"] = Backspace;
			_keyActions ["Delete"] = Delete;
			_keyActions ["ControlD"] = Delete;
			_keyActions ["ControlH"] = Backspace;
			_keyActions ["ControlL"] = ClearLine;
			_keyActions ["Escape"] = ClearLine;
			_keyActions ["UpArrow"] = PrevHistory;
			_keyActions ["ControlP"] = PrevHistory;
			_keyActions ["DownArrow"] = NextHistory;
			_keyActions ["ControlN"] = NextHistory;
			_keyActions ["ControlU"] = () =>
			{
				Backspace (_cursorPos);
			};
			_keyActions ["ControlK"] = () =>
			{
				int pos = _cursorPos;
				MoveCursorEnd ();
				Backspace (_cursorPos - pos);
			};
			_keyActions ["ControlW"] = () =>
			{
				while (!IsStartOfLine () && _text [_cursorPos - 1] != ' ')
					Backspace ();
			};
			_keyActions ["ControlT"] = TransposeChars;

			_keyActions ["Tab"] = () =>
			{
				if (IsInAutoCompleteMode ())
				{
					NextAutoComplete ();
				} else
				{
					if (autoCompleteHandler == null || !IsEndOfLine ())
						return;

					string text = _text.ToString ();

					_completionStart = text.LastIndexOfAny (autoCompleteHandler.Separators);
					_completionStart = _completionStart == -1 ? 0 : _completionStart + 1;

					_completions = autoCompleteHandler.GetSuggestions (text, _completionStart);
					_completions = _completions != null && _completions.Length == 0 ? null : _completions;

					if (_completions == null)
						return;

					StartAutoComplete ();
				}
			};

			_keyActions ["ShiftTab"] = () =>
			{
				if (IsInAutoCompleteMode ())
				{
					PreviousAutoComplete ();
				}
			};
		}

		public void Handle (ConsoleKeyInfo keyInfo)
		{
			_keyInfo = keyInfo;

			// If in auto complete mode and Tab wasn't pressed
			if (IsInAutoCompleteMode () && _keyInfo.Key != ConsoleKey.Tab)
				ResetAutoComplete ();
			
			Action action;
			_keyActions.TryGetValue (BuildKeyInput (), out action);
			action = action ?? WriteChar;
			action.Invoke ();
		}
	}
}