// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="none" email=""/>
//     <version>$Revision: 2487 $</version>
// </file>

using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Gui.InsightWindow;
using YukiTheme.Tools;

namespace YukiTheme.Style.CodeCompletion;

internal static class TipPainter
{
	private const float HorizontalBorder = 2;
	private const float VerticalBorder = 1;
	private static RectangleF workingArea = RectangleF.Empty;

	//static StringFormat centerTipFormat = CreateTipStringFormat();

	public static Size GetTipSize(Control control, Graphics graphics, Font font, string description)
	{
		return GetTipSize(control, graphics, new TipText(graphics, font, description));
	}

	public static Size GetTipSize(Control control, Graphics graphics, TipSection tipData)
	{
		var tipSize = Size.Empty;
		var tipSizeF = SizeF.Empty;

		if (workingArea == RectangleF.Empty)
		{
			var ownerForm = control.FindForm();
			if (ownerForm.Owner != null) ownerForm = ownerForm.Owner;

			workingArea = Screen.GetWorkingArea(ownerForm);
		}

		PointF screenLocation = control.PointToScreen(Point.Empty);
		var maxLayoutSize = new SizeF(workingArea.Right /*- screenLocation.X*/ - HorizontalBorder * 2,
			workingArea.Bottom /*- screenLocation.Y*/ - VerticalBorder * 2);

		var global_max_x = workingArea.Right - HorizontalBorder * 2;

		if (maxLayoutSize.Width > 0 && maxLayoutSize.Height > 0)
		{
			/*graphics.TextRenderingHint =
			TextRenderingHint.AntiAliasGridFit;*/
			tipData.GlobalMaxX = global_max_x;
			tipData.SetMaximumSize(maxLayoutSize);
			//if (tipData.LeftOffset > 0) 
			//	control.Left = control.Left - tipData.LeftOffset;
			tipSizeF = tipData.GetRequiredSize();
			tipData.SetAllocatedSize(tipSizeF);

			tipSizeF += new SizeF(HorizontalBorder * 2,
				VerticalBorder * 2);
			tipSize = Size.Ceiling(tipSizeF);
		}

		if (control is PABCNETInsightWindow)
		{
			var rect = Rectangle.Ceiling(workingArea);
			var pt = (control as PABCNETInsightWindow).CallFunctionByReflection<Point>("GetCusorCoord");
			if (pt.X + tipSize.Width > rect.Width) control.Location = new Point(rect.Width - tipSize.Width, pt.Y);
		}
		else
		{
			var rect = Rectangle.Ceiling(workingArea);
			var pt = control.Location;
			if (pt.X + tipSize.Width > rect.Width) control.Location = new Point(rect.Width - tipSize.Width, pt.Y);
		}

		if (control.ClientSize != tipSize) control.ClientSize = tipSize;

		return tipSize;
	}

	public static Size DrawTip(Control control, Graphics graphics, Font font, string description)
	{
		return DrawTip(control, graphics, new TipText(graphics, font, description));
	}

	public static Size DrawTip(Control control, Graphics graphics, TipSection tipData)
	{
		var tipSize = Size.Empty;
		var tipSizeF = SizeF.Empty;

		PointF screenLocation = control.PointToScreen(Point.Empty);

		if (workingArea == RectangleF.Empty)
		{
			var ownerForm = control.FindForm();
			if (ownerForm.Owner != null) ownerForm = ownerForm.Owner;

			workingArea = Screen.GetWorkingArea(ownerForm);
		}

		var maxLayoutSize = new SizeF(workingArea.Right /*- screenLocation.X*/ - HorizontalBorder * 2,
			workingArea.Bottom /*- screenLocation.Y*/ - VerticalBorder * 2);
		if (maxLayoutSize.Width > 0 && maxLayoutSize.Height > 0)
		{
			/*graphics.TextRenderingHint =
			TextRenderingHint.AntiAliasGridFit;*/

			tipData.SetMaximumSize(maxLayoutSize);
			tipSizeF = tipData.GetRequiredSize();
			tipData.SetAllocatedSize(tipSizeF);

			tipSizeF += new SizeF(HorizontalBorder * 2,
				VerticalBorder * 2);
			tipSize = Size.Ceiling(tipSizeF);
		}

		if (control is PABCNETInsightWindow)
		{
			var rect = Rectangle.Ceiling(workingArea);
			var pt = (control as PABCNETInsightWindow).CallFunctionByReflection<Point>("GetCusorCoord");
			if (pt.X + tipSize.Width > rect.Width) control.Location = new Point(rect.Width - tipSize.Width, pt.Y);
		}
		else
		{
			var rect = Rectangle.Ceiling(workingArea);
			var pt = control.Location;
			if (pt.X + tipSize.Width > rect.Width) control.Location = rect.Width - tipSize.Width > 0 ? new Point(rect.Width - tipSize.Width, pt.Y) : new Point(10, pt.Y);
		}

		if (control.ClientSize != tipSize) control.ClientSize = tipSize;

		if (tipSize != Size.Empty)
		{
			var borderRectangle = new Rectangle
				(Point.Empty, tipSize - new Size(1, 1));

			var displayRectangle = new RectangleF
			(HorizontalBorder, VerticalBorder,
				tipSizeF.Width - HorizontalBorder * 2,
				tipSizeF.Height - VerticalBorder * 2);

			// DrawRectangle draws from Left to Left + Width. A bug? :-/
			graphics.DrawRectangle(SystemPens.WindowFrame,
				borderRectangle);
			tipData.Draw(new PointF(HorizontalBorder, VerticalBorder));
		}

		return tipSize;
	}
}