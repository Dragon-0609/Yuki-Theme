using System;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.InsightWindow;
using ICSharpCode.TextEditor.Util;
using YukiTheme.Engine;
using YukiTheme.Tools;

namespace YukiTheme.Style.CodeCompletion;

public class InsightPopup : PABCNETInsightWindow
{

	public InsightPopup(Form parentForm, TextEditorControl control) : base(parentForm, control)
	{
	}

	private IInsightDataProvider DataProvider()
	{
		return ((PABCNETInsightWindow)this).GetPropertyByReflection<IInsightDataProvider>("DataProvider");
	}

	private int CurrentData() => ((PABCNETInsightWindow)this).GetPropertyByReflection<int>("CurrentData");


	protected override void OnPaint(PaintEventArgs pe)
	{
		string methodCountMessage = null, description;

		// pe.Graphics.FillRectangle(ColorReference.BackgroundDefaultBrush, pe.ClipRectangle);

		// pe.Graphics.FillRectangle(Brushes.Yellow, pe.ClipRectangle);
		IInsightDataProvider dataProvider = DataProvider();
		if (dataProvider == null || dataProvider.InsightDataCount < 1)
		{
			description = "Unknown Method";
		}
		else
		{
			if (dataProvider.InsightDataCount > 1)
			{
				methodCountMessage = VisualPascalABC.Tools.GetRangeDescription(CurrentData() + 1, dataProvider.InsightDataCount);
			}
			description = dataProvider.GetInsightData(CurrentData());
		}
		//pe.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
		int num_param = dataProvider.GetByReflection<int>("num_param", true);
		// int num_param = (DataProvider as VisualPascalABC.DefaultInsightDataProvider).num_param;
		drawingSize = TipPainterTools.GetDrawingSizeHelpTipFromCombinedDescription(this,
			pe.Graphics,
			Font,
			methodCountMessage,
			description, num_param, true);
		if (drawingSize != Size)
		{
			SetLocation();
		}
		else
		{
			TipPainterTools.DrawHelpTipFromCombinedDescription(this, pe.Graphics, Font, methodCountMessage, description, num_param, true);
		}
		this.SetByReflection("lastCursorScreenPosition", control.ActiveTextAreaControl.TextArea.Caret.ScreenPosition);
	}

	protected override void OnPaintBackground(PaintEventArgs pe)
	{
		pe.Graphics.FillRectangle(ColorReference.BackgroundDefaultBrush, pe.ClipRectangle);
	}

}