using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using VisualPascalABC;
using Yuki_Theme_Plugin;
using YukiTheme.Style.Helpers;
using YukiTheme.Tools;
using CodeCompletionHighlighter = YukiTheme.Style.CodeCompletion.CodeCompletionHighlighter;

namespace YukiTheme.Engine;

public class TextEditorChanger
{
	private IconBarMargin _margin;
	private FoldMargin _foldmargin;
	private Timer _documentUpdator;
	private Rectangle _oldSizeOfTextEditor = Rectangle.Empty;
	private EditorComponents _editorComponents;
	private EditorInspector _editorInspector;
	private Form1 Fm => IDEAlterer.Instance.Form1;

	internal void Init(EditorComponents editor)
	{
		_editorComponents = editor;
		SetMargin();
		_editorComponents.TextArea.Paint += PaintBg;

		_editorComponents.TextEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += UpdateMarkerBGOnCaretPositionChanged;
		_editorComponents.TextEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += RemoveErrorMarksOnCaretPositionChanged;

		_editorComponents.TextEditor.Controls[1].Paint += CtrlOnPaint;
		_editorComponents.TextEditor.Controls[1].Invalidate();

		_editorInspector = new EditorInspector(_editorComponents);

		_documentUpdator = new Timer() { Interval = 2 };
		_documentUpdator.Tick += (sender, r) =>
		{
			_documentUpdator.Stop();
			ReSetTextEditor();
		};

		Fm.MainDockPanel.ActiveContentChanged += (sender, e) => { _documentUpdator.Start(); };
	}

	private void PaintBg(object sender, PaintEventArgs e)
	{
		if (_margin != null)
		{
			e.Graphics.FillRectangle(ColorReference.BackgroundDefaultBrush, _margin.DrawingPosition.X,
				_margin.DrawingPosition.Y,
				_margin.DrawingPosition.Width, _margin.DrawingPosition.Height);
			var inside =
				typeof(IconBarMargin).GetMethod("IsLineInsideRegion",
					BindingFlags.Static | BindingFlags.NonPublic);
			// paint icons
			foreach (Bookmark mark in _editorComponents.TextArea.Document.BookmarkManager.Marks)
			{
				int lineNumber = _editorComponents.TextArea.Document.GetVisibleLine(mark.LineNumber);
				int lineHeight = _editorComponents.TextArea.TextView.FontHeight;
				int yPos = (int)(lineNumber * lineHeight) - _editorComponents.TextArea.VirtualTop.Y;
				if ((bool)inside.Invoke(
					    null, new object[] { yPos, yPos + lineHeight, _margin.DrawingPosition.Y, _margin.DrawingPosition.Height }))
				{
					if (lineNumber == _editorComponents.TextArea.Document.GetVisibleLine(mark.LineNumber - 1))
					{
						// marker is inside folded region, do not draw it
						continue;
					}

					mark.Draw(_margin, e.Graphics, new Point(0, yPos));
				}
			}
		}

		if (_foldmargin != null)
		{
			SetMarginPosition();
			e.Graphics.DrawLine(BrushRegistry.GetDotPen(ColorReference.BackgroundDefaultColor, ColorReference.BorderColor),
				_foldmargin.DrawingPosition.X,
				_foldmargin.DrawingPosition.Y,
				_foldmargin.DrawingPosition.X,
				_foldmargin.DrawingPosition.Height);
		}

		if (false) //IDEAlterer.HasWallpaper)
		{
			DrawWallpaperInEditor(e);
		}
	}

	private void DrawWallpaperInEditor(PaintEventArgs e)
	{
		Size vm = _editorComponents.TextEditor.ClientSize;
		var img = IDEAlterer.GetWallpaper;

		if (_oldSizeOfTextEditor.Width != vm.Width ||
		    _oldSizeOfTextEditor.Height != vm.Height)
		{
			_oldSizeOfTextEditor = MiscHelper.GetSizes(img.Size, vm.Width, vm.Height, Alignment.Center);
		}

		e.Graphics.DrawImage(img, _oldSizeOfTextEditor);
	}

	private void CtrlOnPaint(object sender, PaintEventArgs e)
	{
		// e.Graphics.FillRectangle(ColorReference.BackgroundDefaultBrush, e.ClipRectangle);
	}


	private void ReSetTextEditor()
	{
		if (_editorComponents.TextEditor != Fm.CurrentCodeFileDocument.TextEditor)
		{
			_editorComponents.TextArea.Paint -= PaintBg;
			_editorInspector.StopInspectBrackets();

			_editorComponents.TextEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged -= UpdateMarkerBGOnCaretPositionChanged;
			_editorComponents.TextEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged -= RemoveErrorMarksOnCaretPositionChanged;

			try
			{
				_editorComponents.TextEditor.Controls[1].Paint -= CtrlOnPaint;
			}
			catch (ArgumentOutOfRangeException)
			{
			}

			// textArea = textEditor.ActiveTextAreaControl.TextArea;
			_editorComponents.TextEditor = Fm.CurrentCodeFileDocument.TextEditor;
			_editorComponents.TextArea = _editorComponents.TextEditor.ActiveTextAreaControl.TextArea;
			SetMargin();
			_editorComponents.TextEditor.Parent.BackColor = ColorReference.BackgroundColor;
			try
			{
				_editorComponents.TextEditor.Controls[1].Paint += CtrlOnPaint;
				_editorComponents.TextEditor.Controls[1].Invalidate();
			}
			catch (ArgumentOutOfRangeException)
			{
			}

			Fm.CurrentCodeFileDocument.BackColor = ColorReference.BackgroundColor;

			_editorComponents.TextArea.Paint += PaintBg;
			_editorComponents.TextEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += UpdateMarkerBGOnCaretPositionChanged;
			_editorComponents.TextEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += RemoveErrorMarksOnCaretPositionChanged;

			_editorInspector.InspectBrackets();

			_editorComponents.TextArea.Refresh();
			IDEAlterer.Instance.RequestBottomBarUpdate();

			_editorInspector.Unsubscribe();
		}
	}

	internal void InjectCompletion()
	{
		_editorInspector.InjectCodeCompletion();
	}

	internal void StartInspectingBrackets()
	{
		_editorInspector.InspectBrackets();
	}

	private void UpdateMarkerBGOnCaretPositionChanged(object sender, EventArgs e)
	{
		if (!VisualPABCSingleton.MainForm.UserOptions.HighlightOperatorBrackets ||
		    WorkbenchServiceFactory.DebuggerManager.IsRunning)
			return;
		CodeCompletionHighlighter.UpdateMarkers(_editorComponents.TextArea);
	}

	private void RemoveErrorMarksOnCaretPositionChanged(object sender, EventArgs e)
	{
		ErrorLineBookmarkNew.Remove();
	}


	#region Margins

	internal void SetMargin()
	{
		foreach (AbstractMargin margins in _editorComponents.TextArea.LeftMargins)
		{
			if (margins is IconBarMargin margin)
			{
				_margin = margin;
			}
			else if (margins is FoldMargin foldMargin)
			{
				_foldmargin = foldMargin;
			}
		}
	}

	internal void SetMarginPosition()
	{
		int currentXPos = 0;
		foreach (AbstractMargin margins in _editorComponents.TextArea.LeftMargins)
		{
			Rectangle marginRectangle = new Rectangle(currentXPos, 0, margins.Size.Width, _editorComponents.TextArea.Height);
			if (margins.IsVisible || margins is FoldMargin)
			{
				currentXPos += margins.DrawingPosition.Width;
			}

			if (margins is FoldMargin)
			{
				if (marginRectangle != _margin.DrawingPosition)
				{
					// Be sure that the line has valid rectangle
					_foldmargin.DrawingPosition = marginRectangle;
				}

				break;
			}
		}
	}

	#endregion
}