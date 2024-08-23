// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
// This is a modified file from SharpDevelop project (Copyright (c) AlphaSierraPapa)

using System;
using System.Drawing;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using VisualPascalABC;

namespace YukiTheme.Style.Helpers;

public class ErrorLineBookmarkNew : SDMarkerBookmark
{
	private static ErrorLineBookmarkNew _instance;

	private static int _startLine;
	private static int _startColumn;
	private static int _endLine;
	private static int _endColumn;

	public ErrorLineBookmarkNew(string fileName, IDocument document, int startLine)
		: base(fileName, document, startLine)
	{
		IsSaved = false;
		IsVisibleInBookmarkPad = false;
	}

	public override bool CanToggle => false;

	//public static void SetPosition(string fileName, IDocument document, int makerStartLine, int makerEndLine)
	public static void SetPosition1(TextEditorControl ctrl, int makerStartLine)
	{
		try
		{
			Remove();
			var document = ctrl.Document;
			var fileName = ctrl.FileName;
			_startLine = makerStartLine;
			_endLine = makerStartLine;
			_startColumn = 1;
			var line = document.GetLineSegment(_startLine - 1);
			_endColumn = line.Length + 1;
			_instance = new ErrorLineBookmarkNew(fileName, document, _startLine - 1);
			document.BookmarkManager.AddMark(_instance);

			document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.LinesBetween, _startLine - 1, _startLine - 1));
			document.CommitUpdate();
		}
		catch
		{
			// ignored
		}
	}

	public static void Remove()
	{
		try
		{
			if (_instance != null)
			{
				_instance.Document.BookmarkManager.RemoveMark(_instance);
				_instance.RemoveMarker();
				_instance = null;
			}
		}
		catch
		{
			// ignored
		}
	}

	public override void Draw(IconBarMargin margin, Graphics g, Point p)
	{
		//margin.DrawArrow(g, p.Y);
	}

	protected override TextMarker CreateMarker()
	{
		var lineSeg = Document.GetLineSegment(_startLine - 1);
		//TextMarker marker = new TextMarker(lineSeg.Offset + startColumn - 1, Math.Max(endColumn - startColumn, 1), TextMarkerType.SolidBlock, Color.Yellow, Color.Blue);
		var marker = new TextMarker(lineSeg.Offset + _startColumn - 1, Math.Max(_endColumn - _startColumn, 1), TextMarkerType.SolidBlock, Color.Black, Color.White);
		Document.MarkerStrategy.InsertMarker(0, marker);
		return marker;
	}
}