using System;
using System.Collections;
using CodeCompletion;
using ICSharpCode.TextEditor;
using VisualPascalABC;
using KeywordKind = PascalABCCompiler.Parsers.KeywordKind;

namespace YukiTheme.Style.CodeCompletion
{
	public class CodeCompletionAllNames : ICSharpCode.TextEditor.Actions.AbstractEditAction
	{
		YukiCodeCompletionWindow codeCompletionWindow;
		public static TextArea textArea;
		public static Hashtable comp_windows = new Hashtable();

		public override void Execute(TextArea _textArea)
		{
			//try
			{
				textArea = _textArea;
				int off = textArea.Caret.Offset;
				string text = textArea.Document.TextContent.Substring(0, textArea.Caret.Offset);

				if (!WorkbenchServiceFactory.Workbench.UserOptions.CodeCompletionDot)
					return;

				if (CodeCompletionController.CurrentParser == null) return;
				CodeCompletionProvider completionDataProvider = new CodeCompletionProvider();

				completionDataProvider.preSelection =
					CodeCompletionController.CurrentParser.LanguageInformation.FindPattern(off, text,
						out var is_pattern);

				codeCompletionWindow = YukiCodeCompletionWindow.ShowCompletionWindow(
					VisualPABCSingleton.MainForm, // The parent window for the completion window
					textArea.MotherTextEditorControl, // The text editor to show the window for
					textArea.MotherTextEditorControl.FileName, // Filename - will be passed back to the provider
					completionDataProvider, // Provider to get the list of possible completions
					'_', // Key pressed - will be passed to the provider
					false,
					false,
					KeywordKind.None
				);
				CodeCompletionShiftSpaceActions.comp_windows[textArea] = codeCompletionWindow;

				if (codeCompletionWindow != null)
				{
					// ShowCompletionWindow can return null when the provider returns an empty list
					codeCompletionWindow.Closed += new EventHandler(OnCodeCompletionWindowClosed);
				}
			}
			//catch (Exception e)
			{
			}
		}

		// такой же метод есть в CodeCompletionKeyHandler   EVA
		public void OnCodeCompletionWindowClosed(object sender, EventArgs e)
		{
			if (codeCompletionWindow != null)
			{
				codeCompletionWindow.Closed -= new(OnCodeCompletionWindowClosed);
				CodeCompletionProvider.disp.Reset();
				AssemblyDocCache.Reset();
				UnitDocCache.Reset();
				codeCompletionWindow.Dispose();
				codeCompletionWindow = null;
			}

			comp_windows[textArea] = null;
		}
	}
}