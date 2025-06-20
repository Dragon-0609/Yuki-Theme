// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="none" email=""/>
//     <version>$Revision: 2487 $</version>
// </file>

using System.Drawing;
using System.Windows.Forms;

namespace YukiTheme.Style.CodeCompletion
{
	static class TipPainter
	{
		const float HorizontalBorder = 2;
		const float VerticalBorder   = 1;
		static RectangleF workingArea = RectangleF.Empty;
		
		//static StringFormat centerTipFormat = CreateTipStringFormat();
		
		public static Size GetTipSize(Control control, Graphics graphics, Font font, string description)
		{
			return GetTipSize(control, graphics, new TipText (graphics, font, description));
		}
		
		public static Size GetTipSize(Control control, Graphics graphics, TipSection tipData)
		{
			Size tipSize = Size.Empty;
			SizeF tipSizeF = SizeF.Empty;
			
			if (workingArea == RectangleF.Empty) {
				Form ownerForm = control.FindForm();
				if (ownerForm.Owner != null) {
					ownerForm = ownerForm.Owner;
				}
				
				workingArea = Screen.GetWorkingArea(ownerForm);
			}
			
			PointF screenLocation = control.PointToScreen(Point.Empty);
			SizeF maxLayoutSize = new SizeF(workingArea.Right /*- screenLocation.X*/ - HorizontalBorder * 2,
			                                workingArea.Bottom /*- screenLocation.Y*/ - VerticalBorder * 2);
			
			float global_max_x = workingArea.Right - HorizontalBorder * 2;
			
			if (maxLayoutSize.Width > 0 && maxLayoutSize.Height > 0) {
				/*graphics.TextRenderingHint =
				TextRenderingHint.AntiAliasGridFit;*/
				tipData.GlobalMaxX = global_max_x;
				tipData.SetMaximumSize(maxLayoutSize);
				//if (tipData.LeftOffset > 0) 
				//	control.Left = control.Left - tipData.LeftOffset;
				tipSizeF = tipData.GetRequiredSize();
				tipData.SetAllocatedSize(tipSizeF);
				
				tipSizeF += new SizeF(HorizontalBorder * 2,
				                      VerticalBorder   * 2);
				tipSize = Size.Ceiling(tipSizeF);
			}
			if (control is YukiInsightWindow)
			{
				Rectangle rect = Rectangle.Ceiling(workingArea);
				Point pt = (control as YukiInsightWindow).GetCusorCoord();
				if (pt.X + tipSize.Width > rect.Width)
				{
					control.Location = new Point(rect.Width-tipSize.Width,pt.Y);
				}
			}
			else
			{
				Rectangle rect = Rectangle.Ceiling(workingArea);
				Point pt = control.Location;
				if (pt.X + tipSize.Width > rect.Width)
				{
					control.Location = new Point(rect.Width-tipSize.Width,pt.Y);
				}
			}
			if (control.ClientSize != tipSize) {
				control.ClientSize = tipSize;
			}
			
			return tipSize;
		}
		
		public static Size DrawTip(Control control, Graphics graphics, Font font, string description)
		{
			return DrawTip(control, graphics, new TipText (graphics, font, description));
		}
		
		public static Size DrawTip(Control control, Graphics graphics, TipSection tipData)
		{
			Size tipSize = Size.Empty;
			SizeF tipSizeF = SizeF.Empty;
			
			PointF screenLocation = control.PointToScreen(Point.Empty);
			
			if (workingArea == RectangleF.Empty) {
				Form ownerForm = control.FindForm();
				if (ownerForm.Owner != null) {
					ownerForm = ownerForm.Owner;
				}
				
				workingArea = Screen.GetWorkingArea(ownerForm);
			}
				
			SizeF maxLayoutSize = new SizeF(workingArea.Right /*- screenLocation.X*/ - HorizontalBorder * 2,
			                                workingArea.Bottom /*- screenLocation.Y*/ - VerticalBorder * 2);
			if (maxLayoutSize.Width > 0 && maxLayoutSize.Height > 0) {
				/*graphics.TextRenderingHint =
				TextRenderingHint.AntiAliasGridFit;*/
				
				tipData.SetMaximumSize(maxLayoutSize);
				tipSizeF = tipData.GetRequiredSize();
				tipData.SetAllocatedSize(tipSizeF);
				
				tipSizeF += new SizeF(HorizontalBorder * 2,
				                      VerticalBorder   * 2);
				tipSize = Size.Ceiling(tipSizeF);
			}
			
			if (control is YukiInsightWindow)
			{
				Rectangle rect = Rectangle.Ceiling(workingArea);
				Point pt = (control as YukiInsightWindow).GetCusorCoord();
				if (pt.X + tipSize.Width > rect.Width)
				{
					control.Location = new Point(rect.Width-tipSize.Width,pt.Y);
				}
			}
			else
			{
				Rectangle rect = Rectangle.Ceiling(workingArea);
				Point pt = control.Location;
				if (pt.X + tipSize.Width > rect.Width)
				{
					if (rect.Width-tipSize.Width > 0)
					control.Location = new Point(rect.Width-tipSize.Width,pt.Y);
					else
					control.Location = new Point(10,pt.Y);
				}
			}
			
			if (control.ClientSize != tipSize) {
				control.ClientSize = tipSize;
			}
			
			if (tipSize != Size.Empty) {
				Rectangle borderRectangle = new Rectangle
				(Point.Empty, tipSize - new Size(1, 1));
				
				RectangleF displayRectangle = new RectangleF
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
}
