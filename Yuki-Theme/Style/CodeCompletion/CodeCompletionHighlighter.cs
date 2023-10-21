using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using YukiTheme.Engine;

namespace YukiTheme.Style.CodeCompletion
{
	public class CodeCompletionHighlighter
	{
		public static void UpdateMarkers(TextArea textArea)
		{
			var ffd = typeof(VisualPascalABC.CodeCompletionHighlighter).GetField(
				"markers", BindingFlags.Static | BindingFlags.NonPublic);

			Hashtable maks = (Hashtable)ffd.GetValue(null);

			List<TextMarker> marks = maks[textArea] as List<TextMarker>;
			var field = typeof(TextMarker).GetField("color", BindingFlags.NonPublic | BindingFlags.Instance);
			foreach (TextMarker textMarker in marks)
			{
				field.SetValue(textMarker, ColorReference.TypeColor);
			}

			textArea.Document.CommitUpdate();
		}
	}
}