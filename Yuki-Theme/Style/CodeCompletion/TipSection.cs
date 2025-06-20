// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="none" email=""/>
//     <version>$Revision: 915 $</version>
// </file>

using System;
using System.Drawing;

namespace YukiTheme.Style.CodeCompletion
{
	abstract class TipSection
	{
		SizeF    tipAllocatedSize;
		Graphics tipGraphics;
		SizeF    tipMaxSize;
		SizeF    tipRequiredSize;
		
		public float GlobalMaxX;
		public int LeftOffset;
		
		protected TipSection(Graphics graphics)
		{
			tipGraphics = graphics;
		}
		
		public abstract void Draw(PointF location);
		
		public SizeF GetRequiredSize()
		{
			return tipRequiredSize;
		}
		
		public void SetAllocatedSize(SizeF allocatedSize)
		{
			//Debug.Assert(allocatedSize.Width >= tipRequiredSize.Width &&
			//             allocatedSize.Height >= tipRequiredSize.Height);
			
			tipAllocatedSize = allocatedSize; OnAllocatedSizeChanged();
		}
		
		public void SetMaximumSize(SizeF maximumSize)
		{
			tipMaxSize = maximumSize; OnMaximumSizeChanged();
		}
		
		protected virtual void OnAllocatedSizeChanged()
		{
			
		}
		
		protected virtual void OnMaximumSizeChanged()
		{
			
		}
		
		public void SetRequiredSize(SizeF requiredSize)
		{
			requiredSize.Width  = Math.Max(0, requiredSize.Width);
			requiredSize.Height = Math.Max(0, requiredSize.Height);
			requiredSize.Width  = Math.Min(tipMaxSize.Width, requiredSize.Width);
			requiredSize.Height = Math.Min(tipMaxSize.Height, requiredSize.Height);
			
			tipRequiredSize = requiredSize;
		}
		
		public Graphics Graphics	{
			get {
				return tipGraphics;
			}
		}
		
		public SizeF AllocatedSize {
			get {
				return tipAllocatedSize;
			}
		}
		
		public SizeF MaximumSize {
			get {
				return tipMaxSize;
			}
			set
			{
				tipMaxSize = value;
			}
		}
	}
}
