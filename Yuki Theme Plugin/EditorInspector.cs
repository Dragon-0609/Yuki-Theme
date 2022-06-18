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
using Yuki_Theme_Plugin.Controls.CodeCompletion;

namespace Yuki_Theme_Plugin
{
	public class EditorInspector
	{
		private YukiTheme_VisualPascalABCPlugin plugin;

		private Hashtable ht;
		
		public EditorInspector (YukiTheme_VisualPascalABCPlugin yukiPlugin)
		{
			plugin = yukiPlugin;
		}
		
		internal void SubscribeCompletion ()
		{
			// Console.WriteLine ("Unsubscribed");
			CodeCompletionKeyHandler handler = new CodeCompletionKeyHandler (plugin.ideComponents.textEditor);
			ht [plugin.ideComponents.textEditor] = handler;

			plugin.ideComponents.textEditor.ActiveTextAreaControl.TextArea.KeyEventHandler += handler.TextAreaKeyEventHandler;
			plugin.ideComponents.textEditor.Disposed += handler.CloseCodeCompletionWindow;
			ChangeEditorShortcutForCompletition ();

			EventAdd (typeof (Form1), "TextArea_KeyEventHandler", plugin.ideComponents.fm, typeof (TextArea), "KeyEventHandler",
			          plugin.ideComponents.textEditor.ActiveTextAreaControl.TextArea);
		}

		internal void Unsubscribe ()
		{
			if (Unsubscribe (ht [plugin.ideComponents.textEditor]))
			{
				SubscribeCompletion ();
			} else
			{
				// Console.WriteLine ("Couldn't Unsubscribe");
			}
		}
		
		internal bool Unsubscribe (object target)
		{
			// Console.WriteLine (target.GetType ().Name);
			MethodInfo handler = target.GetType ().GetMethod ("TextAreaKeyEventHandler", BindingFlags.Instance | BindingFlags.NonPublic);
			if (handler == null)
			{
				return false;
			}

			handler = null;
			
			EventRemove (target.GetType (), "TextAreaKeyEventHandler", target, typeof (TextArea), "KeyEventHandler",
			             plugin.ideComponents.textEditor.ActiveTextAreaControl.TextArea);
			
			EventRemove (typeof (Form1), "TextArea_KeyEventHandler", plugin.ideComponents.fm, typeof (TextArea), "KeyEventHandler",
			             plugin.ideComponents.textEditor.ActiveTextAreaControl.TextArea);

			EventRemove (target.GetType (), "CloseCodeCompletionWindow", target, typeof (TextEditorControl), "Disposed",
			             plugin.ideComponents.textEditor.ActiveTextAreaControl.TextArea);
			
			return true;
		}
		
		internal void ChangeEditorShortcutForCompletition ()
		{
			ChangeShortcut (Keys.Space | Keys.Control, new CodeCompletionAllNames ());
		}

		internal void InspectBrackets ()
		{
			plugin.ideComponents.textArea.Paint += InspectBracketsPaint;
		}

		internal void StopInspectBrackets ()
		{
			plugin.ideComponents.textArea.Paint -= InspectBracketsPaint;
		}

		internal void InspectBracketsPaint (object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			if (plugin.ideComponents.textArea.TextView.Highlight != null && plugin.ideComponents.textArea.TextView.Highlight.OpenBrace != null &&
			    plugin.ideComponents.textArea.TextView.Highlight.CloseBrace != null)
			{
				int lineNumber = plugin.ideComponents.textArea.Caret.Line;
				LineSegment currentLine    = plugin.ideComponents.textArea.Document.GetLineSegment(lineNumber);
				int currentWordOffset = 0;
				int startColumn = 0;

				if (plugin.ideComponents.textEditor.TextEditorProperties.EnableFolding)
				{
					List<FoldMarker> starts = plugin.ideComponents.textArea.Document.FoldingManager.GetFoldedFoldingsWithStartAfterColumn(lineNumber, startColumn - 1);
					if (starts != null && starts.Count > 0) {
						FoldMarker firstFolding = (FoldMarker)starts[0];
						foreach (FoldMarker fm in starts) {
							if (fm.StartColumn < firstFolding.StartColumn) {
								firstFolding = fm;
							}
						}
						startColumn     = firstFolding.EndColumn;
					}
				}

				MethodInfo draw = typeof (TextView).GetMethod ("DrawDocumentWord", BindingFlags.NonPublic | BindingFlags.Instance);
				
				TextWord currentWord;
				int drawingX = plugin.ideComponents.textArea.TextView.DrawingPosition.X - plugin.ideComponents.textArea.VirtualTop.X;
				for (int wordIdx = 0; wordIdx < currentLine.Words.Count; wordIdx++)
				{
					currentWord = currentLine.Words[wordIdx];
					
					if (currentWordOffset < startColumn) {
						currentWordOffset += currentWord.Length;
						continue;
					}
					if (plugin.ideComponents.textArea.TextView.Highlight.OpenBrace.Y == lineNumber && plugin.ideComponents.textArea.TextView.Highlight.OpenBrace.X == currentWordOffset ||
					    plugin.ideComponents.textArea.TextView.Highlight.CloseBrace.Y == lineNumber && plugin.ideComponents.textArea.TextView.Highlight.CloseBrace.X == currentWordOffset)
					{
						int xpos = plugin.ideComponents.textArea.TextView.GetDrawingXPos (lineNumber, currentWordOffset) + drawingX;
						int liny = plugin.ideComponents.textArea.TextView.DrawingPosition.Top + (lineNumber - plugin.ideComponents.textArea.TextView.FirstVisibleLine) * plugin.ideComponents.textArea.TextView.FontHeight - plugin.ideComponents.textArea.TextView.VisibleLineDrawingRemainder;
						draw.Invoke (plugin.ideComponents.textArea.TextView, new object []
						{
							g,
							currentWord.Word,
							new Point (xpos, liny),
							currentWord.GetFont (plugin.ideComponents.textArea.TextView.TextEditorProperties.FontContainer),
							currentWord.Color,
							YukiTheme_VisualPascalABCPlugin.typeBrush
						});
					}
					currentWordOffset += currentWord.Length;
				}

			}
		}
		
		internal void InjectCodeCompletion ()
		{
			var assembly = typeof (Form1).Assembly;
			Type type = assembly.GetType ("VisualPascalABC.CodeCompletionKeyHandler");
			ht = (Hashtable)type.GetField ("ht",
			                               BindingFlags.NonPublic | BindingFlags.Static).GetValue (null);
			// Console.WriteLine ("Unsubscribing: ");
			Unsubscribe (ht [plugin.ideComponents.textEditor]);
			SubscribeCompletion ();
		}
		
		
		private void ChangeShortcut (Keys key, IEditAction val)
		{
			var fdactions = plugin.ideComponents.textEditor.GetType ()
			                          .GetField ("editactions", BindingFlags.NonPublic | BindingFlags.Instance);
			Dictionary <Keys, IEditAction> actions = (Dictionary <Keys, IEditAction>)fdactions.GetValue (plugin.ideComponents.textEditor);
			actions [key] = val;
		}
		
		private void EventAdd (Type typeForMethod, string nameOfMethod, object objectOfMethod, Type typeForEvent, string nameOfEvent, object objectOfEvent)
		{
			MethodInfo keyEventHandler = typeForMethod.GetMethod (nameOfMethod, BindingFlags.Instance | BindingFlags.NonPublic);
			EventInfo evt = typeForEvent.GetEvent (nameOfEvent, BindingFlags.Instance | BindingFlags.Public);
			MethodInfo addMethod = evt.GetAddMethod (false);

			addMethod.Invoke (objectOfEvent, new object []
			{
				Delegate.CreateDelegate (evt.EventHandlerType, objectOfMethod, keyEventHandler)
			});
		}

		private void EventRemove (Type typeForMethod, string nameOfMethod, object objectOfMethod, Type typeForEvent, string nameOfEvent, object objectOfEvent)
		{
			MethodInfo keyEventHandler = typeForMethod.GetMethod (nameOfMethod, BindingFlags.Instance | BindingFlags.NonPublic);
			EventInfo evt = typeForEvent.GetEvent (nameOfEvent, BindingFlags.Instance | BindingFlags.Public);
			MethodInfo addMethod = evt.GetRemoveMethod (false);

			addMethod.Invoke (objectOfEvent, new object []
			{
				Delegate.CreateDelegate (evt.EventHandlerType, objectOfMethod, keyEventHandler)
			});
		}

	}
}