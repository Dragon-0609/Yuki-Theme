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
using VisualPascalABCPlugins;
using KeywordKind = PascalABCCompiler.Parsers.KeywordKind;

namespace Yuki_Theme_Plugin.Controls.CodeCompletion
{
	internal class CodeCompletionKeyHandler
	{
		private static readonly Hashtable                   ht = new Hashtable ();
		public                  YukiCodeCompletionWindow codeCompletionWindow;
		public                  CodeCompletionProvider      completionDataProvider;
		public                  TextEditorControl           editor;
		public                  PABCNETInsightWindow        insightWindow;

		public CodeCompletionKeyHandler (TextEditorControl editor)
		{
			this.editor = editor;
		}

		public static CodeCompletionKeyHandler Attach (TextEditorControl editor)
		{
			var h = new CodeCompletionKeyHandler (editor);
			ht [editor] = h;
			editor.ActiveTextAreaControl.TextArea.KeyEventHandler += h.TextAreaKeyEventHandler;
			editor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += h.CaretPositionChangedEventHandler;
			//editor.ActiveTextAreaControl.TextArea.KeyDown += new System.Windows.Forms.KeyEventHandler(TextArea_KeyDown);
			//editor.ActiveTextAreaControl.KeyDown += h.TextControlEventHandler;
			// When the editor is disposed, close the code completion window
			editor.Disposed += h.CloseCodeCompletionWindow;
			return h;
		}

		public static void Detach (TextEditorControl editor)
		{
			if (ht.ContainsKey (editor))
				ht.Remove (editor);
		}

		public void CaretPositionChangedEventHandler (object sender, EventArgs e)
		{
			if (!VisualPABCSingleton.MainForm.UserOptions.HighlightOperatorBrackets || WorkbenchServiceFactory.DebuggerManager.IsRunning)
				return;
			CodeCompletionHighlighter.Highlight (editor.ActiveTextAreaControl.TextArea);
		}

		/// <summary>
		///     Return true to handle the keypress, return false to let the text area handle the keypress
		/// </summary>
		public bool TextAreaKeyEventHandler (char key)
		{
			if (!WorkbenchServiceFactory.Workbench.UserOptions.AllowCodeCompletion ||
			    !VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.compilerLoaded)
				return false;
			if (CodeCompletionController.CurrentParser == null) return false;
			if (codeCompletionWindow != null)
			{
				// If completion window is open and wants to handle the key, don't let the text area
				// handle it
				if (codeCompletionWindow.ProcessKeyEvent (key))
					return true;
			} else
			{
				var ccw =
					CodeCompletionAllNamesAction.comp_windows [editor.ActiveTextAreaControl.TextArea] as
						YukiCodeCompletionWindow;

				if (ccw != null && CodeCompletionAllNamesAction.is_begin)
				{
					CodeCompletionAllNamesAction.is_begin = false;
					if (key != ' ')
					{
						ccw.ProcessKeyEvent (key);
					} else
					{
						ccw.ProcessKeyEvent ('_');
						ccw.Close ();
					}
				} else if (ccw != null && ccw.ProcessKeyEvent (key))
				{
					return true;
				}
			}

			if (key == '.')
			{
				if (CodeCompletionController.CurrentParser == null)
					return false;
				if (!string.IsNullOrEmpty (WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl
				                                                  .SelectionManager.SelectedText))
				{
					WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.Caret.Position =
						WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.SelectionManager
						                       .SelectionCollection [0].StartPosition;
					WorkbenchServiceFactory.DocumentService.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.SelectionManager
					                       .RemoveSelectedText ();
				}

				if (WorkbenchServiceFactory.Workbench.UserOptions.CodeCompletionDot)
				{
					completionDataProvider = new CodeCompletionProvider ();

					codeCompletionWindow = YukiCodeCompletionWindow.ShowCompletionWindow (
						VisualPABCSingleton.MainForm, // The parent window for the completion window
						editor,                       // The text editor to show the window for
						editor.FileName,              // Filename - will be passed back to the provider
						completionDataProvider,       // Provider to get the list of possible completions
						key,                          // Key pressed - will be passed to the provider
						true,
						true,
						KeywordKind.None
					);
					CodeCompletionAllNamesAction.is_begin = true;
					CodeCompletionAllNamesAction.comp_windows [editor.ActiveTextAreaControl.TextArea] = codeCompletionWindow;
					if (codeCompletionWindow != null)
						codeCompletionWindow.Closed += CloseCodeCompletionWindow;
				}
			} else if (key == '(' || key == '[' || key == ',')
			{
				if (CodeCompletionController.CurrentParser == null) return false;
				if (VisualPABCSingleton.MainForm.UserOptions.CodeCompletionParams)
				{
					var constructor = typeof (Form1).Assembly.GetType ("VisualPascalABC.DefaultInsightDataProvider")
					                                .GetConstructors (BindingFlags.Public | BindingFlags.Instance) [0];

					var idp =
						(IInsightDataProvider)constructor.Invoke (new object [] { -1, false, key });
					if (insightWindow == null || insightWindow.InsightProviderStackLength == 0)
					{
						insightWindow = new PABCNETInsightWindow (VisualPABCSingleton.MainForm, editor);
						insightWindow.Font = new Font (Constants.CompletionInsightWindowFontName, insightWindow.Font.Size);
						insightWindow.Closed += CloseInsightWindow;
					} else
					{
						var field = idp.GetType ().GetField ("defaultIndex", BindingFlags.Public | BindingFlags.Instance);
						var field_cur_param_num =
							idp.GetType ().GetField ("cur_param_num", BindingFlags.Public | BindingFlags.Instance);
						var field_param_count = idp.GetType ().GetField ("param_count", BindingFlags.Public | BindingFlags.Instance);
						field.SetValue (idp, insightWindow.GetCurrentData ());
						field_cur_param_num.SetValue (idp, field_param_count.GetValue (insightWindow.GetInsightProvider ()));
					}

					insightWindow.AddInsightDataProvider (idp, editor.FileName);
					insightWindow.ShowInsightWindow ();
				}
			} else if (key == ' ')
			{
				if (CodeCompletionController.CurrentParser == null) return false;
				if (VisualPABCSingleton.MainForm.UserOptions.CodeCompletionDot)
				{
					var keyw =
						KeywordChecker.TestForKeyword (editor.Document.TextContent, editor.ActiveTextAreaControl.TextArea.Caret.Offset - 1);
					if (keyw == KeywordKind.New || keyw == KeywordKind.Uses)
					{
						completionDataProvider = new CodeCompletionProvider ();
						codeCompletionWindow = YukiCodeCompletionWindow.ShowCompletionWindow (
							VisualPABCSingleton.MainForm, // The parent window for the completion window
							editor,                       // The text editor to show the window for
							editor.FileName,              // Filename - will be passed back to the provider
							completionDataProvider,       // Provider to get the list of possible completions
							' ',                          // Key pressed - will be passed to the provider
							true,
							false,
							keyw
						);
						CodeCompletionAllNamesAction.comp_windows [editor.ActiveTextAreaControl.TextArea] = codeCompletionWindow;
						if (codeCompletionWindow != null)
							codeCompletionWindow.Closed += CloseCodeCompletionWindow;
						//return true;
					}
				}
			} else if (key == '\n')
			{
				if (VisualPABCSingleton.MainForm.UserOptions.AllowCodeCompletion &&
				    CodeCompletionController.CurrentParser != null)
					try
					{
						var dconv =
							(DomConverter)CodeCompletionController.comp_modules [editor.FileName];
						if (dconv != null)
						{
							var ss = dconv.FindScopeByLocation (editor.ActiveTextAreaControl.TextArea.Caret.Line + 1,
							                                    editor.ActiveTextAreaControl.TextArea.Caret.Column + 1);
							ss.IncreaseEndLine ();
						}
					} catch
					{
					}
			} else if (codeCompletionWindow == null && VisualPABCSingleton.MainForm.UserOptions.EnableSmartIntellisense &&
			           (char.IsLetter (key) || key == '_'))
			{
				if (VisualPABCSingleton.MainForm.UserOptions.CodeCompletionDot)
				{
					if (CodeCompletionController.CurrentParser == null) return false;
					var keyw =
						KeywordChecker.TestForKeyword (editor.Document.TextContent, editor.ActiveTextAreaControl.TextArea.Caret.Offset - 1);
					if (CodeCompletionController.CurrentParser.LanguageInformation.IsDefinitionIdentifierAfterKeyword (keyw))
						return false;

					if (editor.ActiveTextAreaControl.TextArea.Caret.Offset > 0 &&
					    (char.IsLetterOrDigit (editor.Document.TextContent [editor.ActiveTextAreaControl.TextArea.Caret.Offset - 1]) ||
					     editor.Document.TextContent [editor.ActiveTextAreaControl.TextArea.Caret.Offset - 1] == '_'))
						return false;
					completionDataProvider = new CodeCompletionProvider ();
					codeCompletionWindow = YukiCodeCompletionWindow.ShowCompletionWindowWithFirstChar (
						VisualPABCSingleton.MainForm, // The parent window for the completion window
						editor,                       // The text editor to show the window for
						editor.FileName,              // Filename - will be passed back to the provider
						completionDataProvider,       // Provider to get the list of possible completions
						key,                          // Key pressed - will be passed to the provider
						keyw
					);
					CodeCompletionAllNamesAction.comp_windows [editor.ActiveTextAreaControl.TextArea] = codeCompletionWindow;
					if (codeCompletionWindow != null)
						codeCompletionWindow.Closed += CloseCodeCompletionWindow;
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

		private void CloseInsightWindow (object sender, EventArgs e)
		{
			if (insightWindow != null)
			{
				insightWindow.Closed -= CloseInsightWindow;
				insightWindow.Dispose ();
				insightWindow = null;
			}
		}

		public void CloseCodeCompletionWindow (object sender, EventArgs e)
		{
			if (codeCompletionWindow != null)
			{
				codeCompletionWindow.Closed -= CloseCodeCompletionWindow;
				CodeCompletionProvider.disp.Reset ();
				AssemblyDocCache.Reset ();
				UnitDocCache.Reset ();
				codeCompletionWindow.Dispose ();
				CodeCompletionAllNamesAction.comp_windows [editor.ActiveTextAreaControl.TextArea] = null;
				codeCompletionWindow = null;
			}
		}
	}
}