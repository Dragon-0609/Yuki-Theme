// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections;
using System.Drawing;
using System.Reflection;
using CodeCompletion;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.InsightWindow;
using VisualPascalABC;
using KeywordKind = PascalABCCompiler.Parsers.KeywordKind;

namespace YukiTheme.Style.CodeCompletion
{
	internal class CodeCompletionKeyHandler
	{
		private static readonly Hashtable Ht = new Hashtable();
		private YukiCodeCompletionWindow _codeCompletionWindow;
		private CodeCompletionProvider _completionDataProvider;
		private readonly TextEditorControl _editor;
		private PABCNETInsightWindow _insightWindow;

		public CodeCompletionKeyHandler(TextEditorControl editor)
		{
			this._editor = editor;
		}

		public static CodeCompletionKeyHandler Attach(TextEditorControl editor)
		{
			var h = new CodeCompletionKeyHandler(editor);
			Ht[editor] = h;
			editor.ActiveTextAreaControl.TextArea.KeyEventHandler += h.TextAreaKeyEventHandler;
			editor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += h.CaretPositionChangedEventHandler;
			//editor.ActiveTextAreaControl.TextArea.KeyDown += new System.Windows.Forms.KeyEventHandler(TextArea_KeyDown);
			//editor.ActiveTextAreaControl.KeyDown += h.TextControlEventHandler;
			// When the editor is disposed, close the code completion window
			editor.Disposed += h.CloseCodeCompletionWindow;
			return h;
		}

		public static void Detach(TextEditorControl editor)
		{
			if (Ht.ContainsKey(editor))
				Ht.Remove(editor);
		}

		public void CaretPositionChangedEventHandler(object sender, EventArgs e)
		{
			if (!VisualPABCSingleton.MainForm.UserOptions.HighlightOperatorBrackets || WorkbenchServiceFactory.DebuggerManager.IsRunning)
				return;
			VisualPascalABC.CodeCompletionHighlighter.Highlight(_editor.ActiveTextAreaControl.TextArea);
		}

		/// <summary>
		///     Return true to handle the keypress, return false to let the text area handle the keypress
		/// </summary>
		public bool TextAreaKeyEventHandler(char key)
		{
			if (!WorkbenchServiceFactory.Workbench.UserOptions.AllowCodeCompletion ||
			    !VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.compilerLoaded)
				return false;
			if (CodeCompletionController.CurrentParser == null) return false;
			if (_codeCompletionWindow != null)
			{
				// If completion window is open and wants to handle the key, don't let the text area
				// handle it
				if (_codeCompletionWindow.ProcessKeyEvent(key))
					return true;
			}
			else
			{
				var ccw =
					CodeCompletionAllNamesAction.comp_windows[_editor.ActiveTextAreaControl.TextArea] as
						YukiCodeCompletionWindow;

				if (ccw != null && CodeCompletionAllNamesAction.is_begin)
				{
					CodeCompletionAllNamesAction.is_begin = false;
					if (key != ' ')
					{
						ccw.ProcessKeyEvent(key);
					}
					else
					{
						ccw.ProcessKeyEvent('_');
						ccw.Close();
					}
				}
				else if (ccw != null && ccw.ProcessKeyEvent(key))
				{
					return true;
				}
			}

			if (key == '.')
			{
				if (CodeCompletionController.CurrentParser == null)
					return false;
				if (!string.IsNullOrEmpty(WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl
					    .SelectionManager.SelectedText))
				{
					WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.Caret.Position =
						WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.SelectionManager
							.SelectionCollection[0].StartPosition;
					WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.SelectionManager
						.RemoveSelectedText();
				}

				if (WorkbenchServiceFactory.Workbench.UserOptions.CodeCompletionDot)
				{
					_completionDataProvider = new CodeCompletionProvider();

					_codeCompletionWindow = YukiCodeCompletionWindow.ShowCompletionWindow(
						VisualPABCSingleton.MainForm, // The parent window for the completion window
						_editor, // The text editor to show the window for
						_editor.FileName, // Filename - will be passed back to the provider
						_completionDataProvider, // Provider to get the list of possible completions
						key, // Key pressed - will be passed to the provider
						true,
						true,
						KeywordKind.None
					);
					CodeCompletionAllNamesAction.is_begin = true;
					CodeCompletionAllNamesAction.comp_windows[_editor.ActiveTextAreaControl.TextArea] = _codeCompletionWindow;
					if (_codeCompletionWindow != null)
						_codeCompletionWindow.Closed += CloseCodeCompletionWindow;
				}
			}
			else if (key == '(' || key == '[' || key == ',')
			{
				if (CodeCompletionController.CurrentParser == null) return false;
				if (VisualPABCSingleton.MainForm.UserOptions.CodeCompletionParams)
				{
					var constructor = typeof(Form1).Assembly.GetType("VisualPascalABC.DefaultInsightDataProvider")
						.GetConstructors(BindingFlags.Public | BindingFlags.Instance)[0];

					var idp =
						(IInsightDataProvider)constructor.Invoke(new object[] { -1, false, key });
					if (_insightWindow == null || _insightWindow.InsightProviderStackLength == 0)
					{
						_insightWindow = new InsightPopup(VisualPABCSingleton.MainForm, _editor);
						_insightWindow.Font = new Font(Constants.CompletionInsightWindowFontName, _insightWindow.Font.Size);
						_insightWindow.Closed += CloseInsightWindow;
					}
					else
					{
						var field = idp.GetType().GetField("defaultIndex", BindingFlags.Public | BindingFlags.Instance);
						var fieldCurParamNum =
							idp.GetType().GetField("cur_param_num", BindingFlags.Public | BindingFlags.Instance);
						var fieldParamCount = idp.GetType().GetField("param_count", BindingFlags.Public | BindingFlags.Instance);
						field.SetValue(idp, _insightWindow.GetCurrentData());
						fieldCurParamNum.SetValue(idp, fieldParamCount.GetValue(_insightWindow.GetInsightProvider()));
					}

					_insightWindow.AddInsightDataProvider(idp, _editor.FileName);
					_insightWindow.ShowInsightWindow();
				}
			}
			else if (key == ' ')
			{
				if (CodeCompletionController.CurrentParser == null) return false;
				if (VisualPABCSingleton.MainForm.UserOptions.CodeCompletionDot)
				{
					var keyw =
						KeywordChecker.TestForKeyword(_editor.Document.TextContent, _editor.ActiveTextAreaControl.TextArea.Caret.Offset - 1);
					if (keyw == KeywordKind.New || keyw == KeywordKind.Uses)
					{
						_completionDataProvider = new CodeCompletionProvider();
						_codeCompletionWindow = YukiCodeCompletionWindow.ShowCompletionWindow(
							VisualPABCSingleton.MainForm, // The parent window for the completion window
							_editor, // The text editor to show the window for
							_editor.FileName, // Filename - will be passed back to the provider
							_completionDataProvider, // Provider to get the list of possible completions
							' ', // Key pressed - will be passed to the provider
							true,
							false,
							keyw
						);
						CodeCompletionAllNamesAction.comp_windows[_editor.ActiveTextAreaControl.TextArea] = _codeCompletionWindow;
						if (_codeCompletionWindow != null)
							_codeCompletionWindow.Closed += CloseCodeCompletionWindow;
						//return true;
					}
				}
			}
			else if (key == '\n')
			{
				if (VisualPABCSingleton.MainForm.UserOptions.AllowCodeCompletion &&
				    CodeCompletionController.CurrentParser != null)
					try
					{
						var dconv =
							(DomConverter)CodeCompletionController.comp_modules[_editor.FileName];
						if (dconv != null)
						{
							var ss = dconv.FindScopeByLocation(_editor.ActiveTextAreaControl.TextArea.Caret.Line + 1,
								_editor.ActiveTextAreaControl.TextArea.Caret.Column + 1);
							ss.IncreaseEndLine();
						}
					}
					catch
					{
					}
			}
			else if (_codeCompletionWindow == null && VisualPABCSingleton.MainForm.UserOptions.EnableSmartIntellisense &&
			         (char.IsLetter(key) || key == '_'))
			{
				if (VisualPABCSingleton.MainForm.UserOptions.CodeCompletionDot)
				{
					if (CodeCompletionController.CurrentParser == null) return false;
					var keyw =
						KeywordChecker.TestForKeyword(_editor.Document.TextContent, _editor.ActiveTextAreaControl.TextArea.Caret.Offset - 1);
					if (CodeCompletionController.CurrentParser.LanguageInformation.IsDefinitionIdentifierAfterKeyword(keyw))
						return false;

					if (_editor.ActiveTextAreaControl.TextArea.Caret.Offset > 0 &&
					    (char.IsLetterOrDigit(_editor.Document.TextContent[_editor.ActiveTextAreaControl.TextArea.Caret.Offset - 1]) ||
					     _editor.Document.TextContent[_editor.ActiveTextAreaControl.TextArea.Caret.Offset - 1] == '_'))
						return false;
					_completionDataProvider = new CodeCompletionProvider();
					_codeCompletionWindow = YukiCodeCompletionWindow.ShowCompletionWindowWithFirstChar(
						VisualPABCSingleton.MainForm, // The parent window for the completion window
						_editor, // The text editor to show the window for
						_editor.FileName, // Filename - will be passed back to the provider
						_completionDataProvider, // Provider to get the list of possible completions
						key, // Key pressed - will be passed to the provider
						keyw
					);
					CodeCompletionAllNamesAction.comp_windows[_editor.ActiveTextAreaControl.TextArea] = _codeCompletionWindow;
					if (_codeCompletionWindow != null)
						_codeCompletionWindow.Closed += CloseCodeCompletionWindow;
				}
			}
			/*else if (codeCompletionWindow == null && key == '\n')
			{
				if (mainForm.UserOptions.AllowCodeFormatting)
			    {
					if (CodeCompletion.CodeCompletionController.currentParser == null) return false;
					string bracket = CodeCompletion.CodeCompletionController.currentParser.LanguageInformation.GetBodyStartBracket();
					string end_bracket = CodeCompletion.CodeCompletionController.currentParser.LanguageInformation.GetBodyEndBracket();
					if (bracket != null)
					{
						int i = editor.ActiveTextAreaControl.TextArea.Caret.Offset-1;
						int j = bracket.Length-1;
						bool eq=true;
						while (i >= 0 && j >= 0)
						{
							if (editor.Document.TextContent[i] != bracket[j])
							{
								eq = false;
								break;
							}
							i--; j--;
						}
						if (eq && j<0)
						{
							TextArea textArea = editor.ActiveTextAreaControl.TextArea;
							int col = textArea.Caret.Column-bracket.Length;
							textArea.InsertString("\n\n"+end_bracket);
							textArea.Caret.Column = 0;
							textArea.Caret.Line = textArea.Caret.Line-1;
							textArea.Caret.Column = VisualPABCSingleton.MainForm.UserOptions.CursorTabCount+col;
							return true;
						}
					}
				}
			}*/

			return false;
		}

		private void CloseInsightWindow(object sender, EventArgs e)
		{
			if (_insightWindow != null)
			{
				_insightWindow.Closed -= CloseInsightWindow;
				_insightWindow.Dispose();
				_insightWindow = null;
			}
		}

		public void CloseCodeCompletionWindow(object sender, EventArgs e)
		{
			if (_codeCompletionWindow != null)
			{
				_codeCompletionWindow.Closed -= CloseCodeCompletionWindow;
				CodeCompletionProvider.disp.Reset();
				AssemblyDocCache.Reset();
				UnitDocCache.Reset();
				_codeCompletionWindow.Dispose();
				CodeCompletionAllNamesAction.comp_windows[_editor.ActiveTextAreaControl.TextArea] = null;
				_codeCompletionWindow = null;
			}
		}
	}
}