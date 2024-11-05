using System;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using YukiTheme.Engine;

namespace YukiTheme.Style.CodeCompletion;

public class DeclarationPopup : DeclarationWindow
{
	public DeclarationPopup(Form parent) : base(parent)
	{
	}

	private bool has_tags()
	{
		return Description.Contains("<returns>") || Description.Contains("<params>");
	}

	protected override void OnPaint(PaintEventArgs pe)
	{
		// base.OnPaint(pe);

		//pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
		if (Description != null && Description.Length > 0)
		{
			if (!in_completion_list)
			{
				TipPainterTools.DrawHelpTipFromCombinedDescription(this, pe.Graphics, Font, null, Description, -1, false);
				// pe.Graphics.FillRectangle(Brushes.Green, pe.ClipRectangle);
			}
			else
			{
				if (has_tags())
				{
					var tmp = Description;
					var return_ind = tmp.IndexOf("<returns>");
					var params_ind = tmp.IndexOf("<params>");
					if (return_ind != -1 && params_ind != -1)
						tmp = tmp.Substring(0, Math.Min(return_ind, params_ind)).Trim(' ', '\n', '\t', '\r');
					else if (return_ind == -1)
						tmp = tmp.Substring(0, params_ind).Trim(' ', '\n', '\t', '\r');
					else
						tmp = tmp.Substring(0, return_ind).Trim(' ', '\n', '\t', '\r');
					Description = tmp;
					return;
				}

				// pe.Graphics.FillRectangle(Brushes.Red, pe.ClipRectangle);
				// base.OnPaint(pe);
				BasePaint(pe);
			}
		}
		else
			// base.OnPaint(pe);
		{
			BasePaint(pe);
		}
	}

	protected override void OnPaintBackground(PaintEventArgs pe)
	{
		pe.Graphics.FillRectangle(ColorReference.BackgroundDefaultBrush, pe.ClipRectangle);
	}

	private void BasePaint(PaintEventArgs pe)
	{
		if (Description == null || Description.Length <= 0)
			return;
		DrawHelpTipFromCombinedDescription(this, pe.Graphics, Font, null, Description);
	}

	public static Size DrawHelpTipFromCombinedDescription(
		Control control,
		Graphics graphics,
		Font font,
		string countMessage,
		string description)
	{
		string basicDescription = null;
		string documentation = null;
		if (TipPainterTools.IsVisibleText(description))
		{
			var strArray = description.Split(new char[1]
			{
				'\n'
			}, 2);
			if (strArray.Length != 0)
			{
				basicDescription = strArray[0];
				if (strArray.Length > 1)
					documentation = strArray[1].Trim();
			}
		}

		return DrawHelpTip(control, graphics, font, countMessage, basicDescription, documentation);
	}

	public static Size DrawHelpTip(
		Control control,
		Graphics graphics,
		Font font,
		string countMessage,
		string basicDescription,
		string documentation)
	{
		if (!TipPainterTools.IsVisibleText(countMessage) && !TipPainterTools.IsVisibleText(basicDescription) && !TipPainterTools.IsVisibleText(documentation))
			return Size.Empty;
		var countTipText = new CountTipText(graphics, font, countMessage);
		var tipSpacer1 = new TipSpacer(graphics, new SizeF(TipPainterTools.IsVisibleText(countMessage) ? 4f : 0.0f, 0.0f));
		var tipText1 = new TipText(graphics, font, basicDescription);
		var tipSpacer2 = new TipSpacer(graphics, new SizeF(0.0f, TipPainterTools.IsVisibleText(documentation) ? 4f : 0.0f));
		var tipText2 = new TipText(graphics, font, documentation);
		var tipSplitter1 = new TipSplitter(graphics, false, tipText1, tipSpacer2);
		var tipSplitter2 = new TipSplitter(graphics, true, countTipText, tipSpacer1, tipSplitter1);
		var tipData = new TipSplitter(graphics, false, tipSplitter2, tipText2);
		var size = TipPainter.DrawTip(control, graphics, tipData);
		TipPainterTools.DrawingRectangle1 = countTipText.DrawingRectangle1;
		TipPainterTools.DrawingRectangle2 = countTipText.DrawingRectangle2;
		return size;
	}
}