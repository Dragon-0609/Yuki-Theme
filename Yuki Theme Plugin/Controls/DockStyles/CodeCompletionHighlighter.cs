using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace Yuki_Theme_Plugin.Controls.DockStyles
{
    public class CodeCompletionHighlighter
    {
        
        public static void UpdateMarkers(TextArea textArea)
        {
            var ffd = typeof (VisualPascalABC.CodeCompletionHighlighter).GetField (
                "markers", BindingFlags.Static | BindingFlags.NonPublic);

            Hashtable maks = (Hashtable) ffd.GetValue (null);
            
            List<TextMarker> marks = maks[textArea] as List<TextMarker>;
            var field = typeof (TextMarker).GetField ("color", BindingFlags.NonPublic | BindingFlags.Instance);
            var field2 = typeof (TextMarker).GetField ("foreColor", BindingFlags.NonPublic | BindingFlags.Instance);
            var field3 = typeof (TextMarker).GetField ("overrideForeColor", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (TextMarker textMarker in marks)
            {
                field.SetValue (textMarker, YukiTheme_VisualPascalABCPlugin.bgType);
                field2.SetValue (textMarker, YukiTheme_VisualPascalABCPlugin.clr);
                field3.SetValue (textMarker, true);
            }
            
            textArea.Document.CommitUpdate();
        }

    }
}

