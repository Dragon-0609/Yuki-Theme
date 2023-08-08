using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using YukiTheme.Tools;

namespace YukiTheme.Engine;

public class TextEditorChanger
{
	private IconBarMargin _margin;
	private FoldMargin _foldmargin;
	private Timer _documentUpdator;
	private Rectangle _oldSizeOfTextEditor = Rectangle.Empty;
	private EditorAlterer _editor;

	public void Init(EditorAlterer editor)
	{
		_editor = editor;
		SetMargin();
		editor.TextArea.Paint += PaintBg;

		editor.TextEditor.Controls[1].Paint += CtrlOnPaint;
		editor.TextEditor.Controls[1].Invalidate();


		_documentUpdator = new Timer() { Interval = 2 };
		_documentUpdator.Tick += (sender, r) =>
		{
			_documentUpdator.Stop();
			ReSetTextEditor();
		};

		editor.Fm.MainDockPanel.ActiveContentChanged += (sender, e) => { _documentUpdator.Start(); };
	}

	private void PaintBg(object sender, PaintEventArgs e)
	{
		if (_margin != null)
		{
			e.Graphics.FillRectangle(ColorReference.BackgroundDefaultBrush(), _margin.DrawingPosition.X,
				_margin.DrawingPosition.Y,
				_margin.DrawingPosition.Width, _margin.DrawingPosition.Height);
			var inside =
				typeof(IconBarMargin).GetMethod("IsLineInsideRegion",
					BindingFlags.Static | BindingFlags.NonPublic);
			// paint icons
			foreach (Bookmark mark in _editor.TextArea.Document.BookmarkManager.Marks)
			{
				int lineNumber = _editor.TextArea.Document.GetVisibleLine(mark.LineNumber);
				int lineHeight = _editor.TextArea.TextView.FontHeight;
				int yPos = (int)(lineNumber * lineHeight) - _editor.TextArea.VirtualTop.Y;
				if ((bool)inside.Invoke(
					    null, new object[] { yPos, yPos + lineHeight, _margin.DrawingPosition.Y, _margin.DrawingPosition.Height }))
				{
					if (lineNumber == _editor.TextArea.Document.GetVisibleLine(mark.LineNumber - 1))
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
			e.Graphics.DrawLine(BrushRegistry.GetDotPen(ColorReference.BackgroundDefaultColor(), ColorReference.BorderColor()),
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
		Size vm = _editor.TextEditor.ClientSize;
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
		e.Graphics.FillRectangle(ColorReference.BackgroundDefaultBrush(), e.ClipRectangle);
	}


	private void ReSetTextEditor()
	{
		if (_editor.TextEditor != _editor.Fm.CurrentCodeFileDocument.TextEditor)
		{
			_editor.TextArea.Paint -= PaintBg;

			try
			{
				_editor.TextEditor.Controls[1].Paint -= CtrlOnPaint;
			}
			catch (ArgumentOutOfRangeException)
			{
			}

			// textArea = textEditor.ActiveTextAreaControl.TextArea;
			_editor.TextEditor = _editor.Fm.CurrentCodeFileDocument.TextEditor;
			_editor.TextArea = _editor.TextEditor.ActiveTextAreaControl.TextArea;
			SetMargin();
			_editor.TextEditor.Parent.BackColor = ColorReference.BackgroundColor();
			try
			{
				_editor.TextEditor.Controls[1].Paint += CtrlOnPaint;
				_editor.TextEditor.Controls[1].Invalidate();
			}
			catch (ArgumentOutOfRangeException)
			{
			}

			_editor.Fm.CurrentCodeFileDocument.BackColor = ColorReference.BackgroundColor();

			_editor.TextArea.Paint += PaintBg;
			_editor.TextArea.Refresh();
			_editor.UpdateBottomTextPanel();
		}
	}


	#region Margins

	internal void SetMargin()
	{
		foreach (AbstractMargin margins in _editor.TextArea.LeftMargins)
		{
			if (margins is IconBarMargin)
			{
				_margin = (IconBarMargin)margins;
			}
			else if (margins is FoldMargin)
			{
				_foldmargin = (FoldMargin)margins;
			}
		}
	}

	internal void SetMarginPosition()
	{
		int currentXPos = 0;
		foreach (AbstractMargin margins in _editor.TextArea.LeftMargins)
		{
			Rectangle marginRectangle = new Rectangle(currentXPos, 0, margins.Size.Width, _editor.TextArea.Height);
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