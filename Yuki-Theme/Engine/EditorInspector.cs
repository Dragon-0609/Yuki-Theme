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

namespace Yuki_Theme_Plugin
{
	public class EditorInspector
	{
		private Hashtable _ht;
		private EditorComponents _editor;

		public EditorInspector(EditorComponents editor)
		{
			_editor = editor;
		}

		internal void SubscribeCompletion()
		{
			// Console.WriteLine ("Unsubscribed");
			CodeCompletionKeyHandler handler = new CodeCompletionKeyHandler(_editor.TextEditor);
			_ht[_editor.TextEditor] = handler;

			_editor.TextEditor.ActiveTextAreaControl.TextArea.KeyEventHandler += handler.TextAreaKeyEventHandler;
			_editor.TextEditor.Disposed += handler.CloseCodeCompletionWindow;
			ChangeEditorShortcutForCompletition();

			EventAdd(typeof(Form1), "TextArea_KeyEventHandler", IDEAlterer.Instance.Form1, typeof(TextArea), "KeyEventHandler",
				_editor.TextEditor.ActiveTextAreaControl.TextArea);
		}

		internal void Unsubscribe()
		{
			if (Unsubscribe(_ht[_editor.TextEditor]))
			{
				SubscribeCompletion();
			}
			else
			{
				// Console.WriteLine ("Couldn't Unsubscribe");
			}
		}

		internal bool Unsubscribe(object target)
		{
			// Console.WriteLine (target.GetType ().Name);
			MethodInfo handler = target.GetType().GetMethod("TextAreaKeyEventHandler", BindingFlags.Instance | BindingFlags.NonPublic);
			if (handler == null)
			{
				return false;
			}

			handler = null;

			EventRemove(target.GetType(), "TextAreaKeyEventHandler", target, typeof(TextArea), "KeyEventHandler",
				_editor.TextEditor.ActiveTextAreaControl.TextArea);

			EventRemove(typeof(Form1), "TextArea_KeyEventHandler", IDEAlterer.Instance.Form1, typeof(TextArea), "KeyEventHandler",
				_editor.TextEditor.ActiveTextAreaControl.TextArea);

			EventRemove(target.GetType(), "CloseCodeCompletionWindow", target, typeof(TextEditorControl), "Disposed",
				_editor.TextEditor.ActiveTextAreaControl.TextArea);

			return true;
		}

		internal void ChangeEditorShortcutForCompletition()
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

		internal void InspectBracketsPaint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			if (_editor.TextArea.TextView.Highlight != null && _editor.TextArea.TextView.Highlight.OpenBrace != null && _editor.TextArea.TextView.Highlight.CloseBrace != null)
			{
				int lineNumber = _editor.TextArea.Caret.Line;
				LineSegment currentLine = _editor.TextArea.Document.GetLineSegment(lineNumber);
				int currentWordOffset = 0;
				int startColumn = 0;

				if (_editor.TextEditor.TextEditorProperties.EnableFolding)
				{
					List<FoldMarker> starts = _editor.TextArea.Document.FoldingManager.GetFoldedFoldingsWithStartAfterColumn(lineNumber, startColumn - 1);
					if (starts != null && starts.Count > 0)
					{
						FoldMarker firstFolding = (FoldMarker)starts[0];
						foreach (FoldMarker fm in starts)
						{
							if (fm.StartColumn < firstFolding.StartColumn)
							{
								firstFolding = fm;
							}
						}

						startColumn = firstFolding.EndColumn;
					}
				}

				MethodInfo draw = typeof(TextView).GetMethod("DrawDocumentWord", BindingFlags.NonPublic | BindingFlags.Instance);

				TextWord currentWord;
				int drawingX = _editor.TextArea.TextView.DrawingPosition.X;

				for (int wordIdx = 0; wordIdx < currentLine.Words.Count; wordIdx++)
				{
					currentWord = currentLine.Words[wordIdx];
					int realPosX = _editor.TextArea.TextView.GetDrawingXPos(lineNumber, currentWordOffset) + drawingX;
					if (currentWordOffset < startColumn || realPosX < drawingX)
					{
						currentWordOffset += currentWord.Length;
						continue;
					}

					if (_editor.TextArea.TextView.Highlight.OpenBrace.Y == lineNumber && _editor.TextArea.TextView.Highlight.OpenBrace.X == currentWordOffset ||
					    _editor.TextArea.TextView.Highlight.CloseBrace.Y == lineNumber && _editor.TextArea.TextView.Highlight.CloseBrace.X == currentWordOffset)
					{
						int xpos = realPosX;
						int liny = _editor.TextArea.TextView.DrawingPosition.Top + (lineNumber - _editor.TextArea.TextView.FirstVisibleLine) * _editor.TextArea.TextView.FontHeight -
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
		}

		internal void InjectCodeCompletion()
		{
			var assembly = typeof(Form1).Assembly;
			Type type = assembly.GetType("VisualPascalABC.CodeCompletionKeyHandler");
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
			Dictionary<Keys, IEditAction> actions = (Dictionary<Keys, IEditAction>)fdactions.GetValue(_editor.TextEditor);
			actions[key] = val;
		}

		internal static void EventAdd(Type typeForMethod, string nameOfMethod, object objectOfMethod, Type typeForEvent, string nameOfEvent, object objectOfEvent)
		{
			MethodInfo keyEventHandler = typeForMethod.GetMethod(nameOfMethod, BindingFlags.Instance | BindingFlags.NonPublic);
			EventInfo evt = typeForEvent.GetEvent(nameOfEvent, BindingFlags.Instance | BindingFlags.Public);
			MethodInfo addMethod = evt.GetAddMethod(false);

			addMethod.Invoke(objectOfEvent, new object[]
			{
				Delegate.CreateDelegate(evt.EventHandlerType, objectOfMethod, keyEventHandler)
			});
		}

		internal static void EventRemove(Type typeForMethod, string nameOfMethod, object objectOfMethod, Type typeForEvent, string nameOfEvent, object objectOfEvent)
		{
			MethodInfo keyEventHandler = typeForMethod.GetMethod(nameOfMethod, BindingFlags.Instance | BindingFlags.NonPublic);
			EventInfo evt = typeForEvent.GetEvent(nameOfEvent, BindingFlags.Instance | BindingFlags.Public);
			MethodInfo addMethod = evt.GetRemoveMethod(false);

			addMethod.Invoke(objectOfEvent, new object[]
			{
				Delegate.CreateDelegate(evt.EventHandlerType, objectOfMethod, keyEventHandler)
			});
		}
	}
}