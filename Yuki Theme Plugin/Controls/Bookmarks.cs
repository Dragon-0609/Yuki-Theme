// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
// This is a modified file from SharpDevelop project (Copyright (c) AlphaSierraPapa)
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using VisualPascalABC;

namespace Yuki_Theme_Plugin
{
    public class ErrorLineBookmarkNew : SDMarkerBookmark
    {
    	static ErrorLineBookmarkNew instance;

        static int startLine;
        static int startColumn;
        static int endLine;
        static int endColumn;
        
        //public static void SetPosition(string fileName, IDocument document, int makerStartLine, int makerEndLine)
        public static void SetPosition1(TextEditorControl ctrl, int makerStartLine)
        {
            try
            {
        	    Remove();
			    IDocument document = ctrl.Document;
			    string fileName = ctrl.FileName;
                startLine = makerStartLine;
                endLine = makerStartLine;
			    startColumn=1;
                LineSegment line = document.GetLineSegment(startLine - 1);
                endColumn = line.Length+1;
                instance = new ErrorLineBookmarkNew(fileName, document, startLine - 1);
                document.BookmarkManager.AddMark(instance);
                document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.LinesBetween, startLine - 1, startLine - 1));
                document.CommitUpdate();
            }
            catch
            {
            	
            }
        }

        public static void Remove()
        {
            try
            {
        		if (instance != null)
            	{
                	instance.Document.BookmarkManager.RemoveMark(instance);
                	instance.RemoveMarker();
                	instance = null;
            	}
            }
            catch
            {
            	
            }
        }

        public override bool CanToggle
        {
            get
            {
                return false;
            }
        }

        public ErrorLineBookmarkNew(string fileName, IDocument document, int startLine)
            : base(fileName, document, startLine)
        {
            this.IsSaved = false;
            this.IsVisibleInBookmarkPad = false;
        }

        public override void Draw(IconBarMargin margin, Graphics g, Point p)
        {
            //margin.DrawArrow(g, p.Y);
        }

        protected override TextMarker CreateMarker()
        {
            LineSegment lineSeg = Document.GetLineSegment(startLine - 1);
            //TextMarker marker = new TextMarker(lineSeg.Offset + startColumn - 1, Math.Max(endColumn - startColumn, 1), TextMarkerType.SolidBlock, Color.Yellow, Color.Blue);
            TextMarker marker = new TextMarker(lineSeg.Offset+startColumn - 1, Math.Max(endColumn-startColumn, 1), TextMarkerType.SolidBlock, Color.Black, Color.White);
            Document.MarkerStrategy.InsertMarker(0, marker);
            return marker;
        }
    }

}
