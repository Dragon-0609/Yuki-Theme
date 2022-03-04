using System;
using System.Reflection;
using CodeCompletion;
using ICSharpCode.TextEditor;
using VisualPascalABC;
using VisualPascalABCPlugins;
using KeywordKind = PascalABCCompiler.Parsers.KeywordKind;

namespace Yuki_Theme_Plugin.Controls.CodeCompletion
{
	public class CodeCompletionAllNames : CodeCompletionAllNamesAction
	{
		private YukiCodeCompletionWindow codecomplet;

		public override void Execute (TextArea _textArea)
		{
			//try
			{
				textArea = _textArea;
				var off = textArea.Caret.Offset;
				var text = textArea.Document.TextContent.Substring (0, textArea.Caret.Offset);
				if (key == '\0')
					if (off > 2 && text [off - 1] == '/' && text [off - 2] == '/' && text [off - 3] == '/')
					{
						CodeCompletionActionsManager.GenerateCommentTemplate (textArea);
						return;
					} else
					{
						var mthd = typeof (CodeCompletionAllNamesAction).GetMethod (
							"get_prev_text", BindingFlags.Instance | BindingFlags.NonPublic);
						var prev = (string)mthd.Invoke (this, new object [] { text, off - 1 });

						if (!string.IsNullOrEmpty (prev))
						{
							CodeCompletionActionsManager.GenerateTemplate (prev, textArea);
							return;
						}
					}

				if (!WorkbenchServiceFactory.Workbench.UserOptions.CodeCompletionDot)
					return;
				if (CodeCompletionController.CurrentParser == null) return;
				var completionDataProvider = new CodeCompletionProvider ();

				var is_pattern = false;


				is_begin = true;

				completionDataProvider.preSelection =
					CodeCompletionController.CurrentParser.LanguageInformation.FindPattern (off, text, out is_pattern);

				if (!is_pattern && off > 0 && text [off - 1] == '.')
					key = '$';

				codecomplet =
					YukiCodeCompletionWindow.ShowCompletionWindow (
						VisualPABCSingleton.MainForm,              // The parent window for the completion window
						textArea.MotherTextEditorControl,          // The text editor to show the window for
						textArea.MotherTextEditorControl.FileName, // Filename - will be passed back to the provider
						completionDataProvider,                    // Provider to get the list of possible completions
						key,                                       // Key pressed - will be passed to the provider
						false,
						false,
						KeywordKind.None
					);

				key = '_';
				comp_windows [textArea] = codecomplet;

				if (codecomplet != null)
					// ShowCompletionWindow can return null when the provider returns an empty list
					codecomplet.Closed += CloseCodeCompletionWindow;
			}
			//catch (Exception e)
			{
			}
		}

		private new void CloseCodeCompletionWindow (object sender, EventArgs e)
		{
			base.CloseCodeCompletionWindow (sender, e);
			if (codecomplet != null)
			{
				codecomplet.Closed -= CloseCodeCompletionWindow;
				CodeCompletionProvider.disp.Reset ();
				AssemblyDocCache.Reset ();
				UnitDocCache.Reset ();
				codecomplet.Dispose ();
				codecomplet = null;
			}
		}
	}

	public class CodeCompletionNamesOnlyInModule : CodeCompletionAllNames
	{
		public override void Execute (TextArea _textArea)
		{
			key = '\0';
			base.Execute (_textArea);
		}
	}
}