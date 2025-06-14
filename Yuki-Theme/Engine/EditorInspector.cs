using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Actions;
using ICSharpCode.TextEditor.Document;
using VisualPascalABC;
using YukiTheme.Engine;
using YukiTheme.Style.CodeCompletion;

namespace Yuki_Theme_Plugin;

public class EditorInspector
{
	private readonly EditorComponents _editor;
	private Hashtable _ht;

	public EditorInspector(EditorComponents editor)
	{
		_editor = editor;
	}


	internal void SubscribeCompletion()
	{
		// Console.WriteLine ("Unsubscribed");
		var handler = new CodeCompletionKeyHandler(_editor.TextEditor);
		_ht[_editor.TextEditor] = handler;

		_editor.TextEditor.ActiveTextAreaControl.TextArea.KeyEventHandler += handler.TextAreaKeyEventHandler;
		// handler.AttachCaretPositionChanged();
		_editor.TextEditor.Disposed += handler.OnCodeCompletionWindowClosed;
		ChangeEditorShortcutForCompletion();

		EventAdd(typeof(Form1), "TextArea_KeyEventHandler", IDEAlterer.Instance.Form1, typeof(TextArea),
			"KeyEventHandler",
			_editor.TextEditor.ActiveTextAreaControl.TextArea);
	}

	internal void Unsubscribe()
	{
		if (Unsubscribe(_ht[_editor.TextEditor])) SubscribeCompletion();
	}

	internal bool Unsubscribe(object target)
	{
		// Console.WriteLine (target.GetType ().Name);
		var handler = target.GetType()
			.GetMethod("TextAreaKeyEventHandler", BindingFlags.Instance | BindingFlags.NonPublic);
		if (handler == null) return false;


		/*
		editor.ActiveTextAreaControl.TextArea.KeyEventHandler += h.TextAreaKeyEventHandler;
		editor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += h.CaretPositionChangedEventHandler;
		*/

		handler = null;

		EventRemove(target.GetType(), "TextAreaKeyEventHandler", target, typeof(TextArea), "KeyEventHandler",
			_editor.TextEditor.ActiveTextAreaControl.TextArea);

		EventRemove(target.GetType(), "CaretPositionChangedEventHandler", target, typeof(Caret), "PositionChanged",
			_editor.TextEditor.ActiveTextAreaControl.TextArea.Caret);

		EventRemove(typeof(Form1), "TextArea_KeyEventHandler", IDEAlterer.Instance.Form1, typeof(TextArea),
			"KeyEventHandler",
			_editor.TextEditor.ActiveTextAreaControl.TextArea);

		EventRemove(target.GetType(), "OnCodeCompletionWindowClosed", target, typeof(TextEditorControl), "Disposed",
			_editor.TextEditor.ActiveTextAreaControl.TextArea);

		return true;
	}

	internal void ChangeEditorShortcutForCompletion()
	{
		ChangeShortcut(Keys.Space | Keys.Control, new CodeCompletionAllNames());
	}

	internal void InspectBrackets()
	{
		_editor.TextArea.Paint += InspectBracketsPaint;
	}

	internal void StopInspectBrackets()
	{
		_editor.TextArea.Paint -= InspectBracketsPaint;
	}

	private void InspectBracketsPaint(object sender, PaintEventArgs e)
	{
		var g = e.Graphics;
		if (_editor.TextArea.TextView.Highlight != null && _editor.TextArea.TextView.Highlight.OpenBrace != null &&
		    _editor.TextArea.TextView.Highlight.CloseBrace != null)
		{
			// int lineNumber = _editor.TextArea.Caret.Line;
			DrawLine(_editor.TextArea.TextView.Highlight.OpenBrace.Y, g);
			DrawLine(_editor.TextArea.TextView.Highlight.CloseBrace.Y, g);
		}
	}

	private void DrawLine(int lineNumber, Graphics g)
	{
		var currentLine = _editor.TextArea.Document.GetLineSegment(lineNumber);
		var currentWordOffset = 0;
		var startColumn = 0;

		if (_editor.TextEditor.TextEditorProperties.EnableFolding)
		{
			var starts =
				_editor.TextArea.Document.FoldingManager.GetFoldedFoldingsWithStartAfterColumn(lineNumber,
					startColumn - 1);
			if (starts != null && starts.Count > 0)
			{
				var firstFolding = starts[0];
				foreach (var fm in starts)
					if (fm.StartColumn < firstFolding.StartColumn)
						firstFolding = fm;

				startColumn = firstFolding.EndColumn;
			}
		}

		var draw = typeof(TextView).GetMethod("DrawDocumentWord", BindingFlags.NonPublic | BindingFlags.Instance);

		TextWord currentWord;
		var drawingX = _editor.TextArea.TextView.DrawingPosition.X;

		for (var wordIdx = 0; wordIdx < currentLine.Words.Count; wordIdx++)
		{
			currentWord = currentLine.Words[wordIdx];
			var realPosX = _editor.TextArea.TextView.GetDrawingXPos(lineNumber, currentWordOffset) + drawingX;
			if (currentWordOffset < startColumn || realPosX < drawingX)
			{
				currentWordOffset += currentWord.Length;
				continue;
			}

			if ((_editor.TextArea.TextView.Highlight.OpenBrace.Y == lineNumber &&
			     _editor.TextArea.TextView.Highlight.OpenBrace.X == currentWordOffset) ||
			    (_editor.TextArea.TextView.Highlight.CloseBrace.Y == lineNumber &&
			     _editor.TextArea.TextView.Highlight.CloseBrace.X == currentWordOffset))
			{
				var xpos = realPosX;
				var liny = _editor.TextArea.TextView.DrawingPosition.Top +
				           (lineNumber - _editor.TextArea.TextView.FirstVisibleLine) *
				           _editor.TextArea.TextView.FontHeight -
				           _editor.TextArea.TextView.VisibleLineDrawingRemainder;

				draw.Invoke(_editor.TextArea.TextView, new object[]
				{
					g,
					currentWord.Word,
					new Point(xpos, liny),
					currentWord.GetFont(_editor.TextArea.TextView.TextEditorProperties.FontContainer),
					currentWord.Color,
					ColorReference.TypeBrush
				});
			}

			currentWordOffset += currentWord.Length;
		}
	}

	internal void InjectCodeCompletion()
	{
		var assembly = typeof(Form1).Assembly;
		var type = assembly.GetType("VisualPascalABC.CodeCompletionKeyHandler");
		_ht = (Hashtable)type.GetField("ht",
			BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
		// Console.WriteLine ("Unsubscribing: ");
		Unsubscribe(_ht[_editor.TextEditor]);
		SubscribeCompletion();
	}


	private void ChangeShortcut(Keys key, IEditAction val)
	{
		var fdactions = _editor.TextEditor.GetType()
			.GetField("editactions", BindingFlags.NonPublic | BindingFlags.Instance);
		var actions = (Dictionary<Keys, IEditAction>)fdactions.GetValue(_editor.TextEditor);
		actions[key] = val;
	}

	internal static void EventAdd(Type typeForMethod, string nameOfMethod, object objectOfMethod, Type typeForEvent,
		string nameOfEvent, object objectOfEvent)
	{
		var keyEventHandler = typeForMethod.GetMethod(nameOfMethod, BindingFlags.Instance | BindingFlags.NonPublic);
		var evt = typeForEvent.GetEvent(nameOfEvent, BindingFlags.Instance | BindingFlags.Public);
		var addMethod = evt.GetAddMethod(false);

		addMethod.Invoke(objectOfEvent, new object[]
		{
			Delegate.CreateDelegate(evt.EventHandlerType, objectOfMethod, keyEventHandler)
		});
	}

	internal static void EventRemove(Type typeForMethod, string nameOfMethod, object objectOfMethod, Type typeForEvent,
		string nameOfEvent, object objectOfEvent)
	{
		var keyEventHandler = typeForMethod.GetMethod(nameOfMethod, BindingFlags.Instance | BindingFlags.NonPublic);
		var evt = typeForEvent.GetEvent(nameOfEvent, BindingFlags.Instance | BindingFlags.Public);
		var addMethod = evt.GetRemoveMethod(false);

		addMethod.Invoke(objectOfEvent, new object[]
		{
			Delegate.CreateDelegate(evt.EventHandlerType, objectOfMethod, keyEventHandler)
		});
	}
}