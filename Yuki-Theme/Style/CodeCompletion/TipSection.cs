// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="none" email=""/>
//     <version>$Revision: 915 $</version>
// </file>

using System;
using System.Drawing;

namespace YukiTheme.Style.CodeCompletion;

internal abstract class TipSection
{
	public float GlobalMaxX;
	public int LeftOffset;
	private SizeF tipMaxSize;
	private SizeF tipRequiredSize;

	protected TipSection(Graphics graphics)
	{
		Graphics = graphics;
	}

	public Graphics Graphics { get; }

	public SizeF AllocatedSize { get; private set; }

	public SizeF MaximumSize
	{
		get => tipMaxSize;
		set => tipMaxSize = value;
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

		AllocatedSize = allocatedSize;
		OnAllocatedSizeChanged();
	}

	public void SetMaximumSize(SizeF maximumSize)
	{
		tipMaxSize = maximumSize;
		OnMaximumSizeChanged();
	}

	protected virtual void OnAllocatedSizeChanged()
	{
	}

	protected virtual void OnMaximumSizeChanged()
	{
	}

	public void SetRequiredSize(SizeF requiredSize)
	{
		requiredSize.Width = Math.Max(0, requiredSize.Width);
		requiredSize.Height = Math.Max(0, requiredSize.Height);
		requiredSize.Width = Math.Min(tipMaxSize.Width, requiredSize.Width);
		requiredSize.Height = Math.Min(tipMaxSize.Height, requiredSize.Height);

		tipRequiredSize = requiredSize;
	}
}