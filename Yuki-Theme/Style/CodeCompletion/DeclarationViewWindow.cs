// Decompiled with JetBrains decompiler
// Type: ICSharpCode.TextEditor.Gui.CompletionWindow.DeclarationViewWindow
// Assembly: ICSharpCode.TextEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=4d61825e8dd49f1a
// MVID: D88D579C-EB24-410A-98F6-1C04625F0E95
// Assembly location: C:\Data\Personal\Documents\C#\Yuki-Theme\Yuki-Theme\bin\Debug\ICSharpCode.TextEditor.dll

#nullable disable
using System;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using YukiTheme.Engine;

namespace YukiTheme.Style.CodeCompletion
{
	public class DeclarationViewWindow : Form, IDeclarationViewWindow
	{
		private string description = string.Empty;
		public bool HideOnClick;

		public string Description
		{
			get => description;
			set
			{
				description = value;
				if (value == null && Visible)
				{
					Visible = false;
				}
				else
				{
					if (value == null)
						return;
					if (!Visible)
						ShowDeclarationViewWindow();
					Refresh();
				}
			}
		}

		public DeclarationViewWindow(Form parent)
		{
			SetStyle(ControlStyles.Selectable, false);
			StartPosition = FormStartPosition.Manual;
			FormBorderStyle = FormBorderStyle.None;
			Owner = parent;
			ShowInTaskbar = false;
			Size = new Size(0, 0);
			CreateHandle();
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				AbstractCompletionWindow.AddShadowToWindow(createParams);
				return createParams;
			}
		}

		protected override bool ShowWithoutActivation => true;

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			if (!HideOnClick)
				return;
			Hide();
		}

		public void ShowDeclarationViewWindow() => Show();

		public void CloseDeclarationViewWindow()
		{
			Close();
			Dispose();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			if (description == null || description.Length <= 0)
				return;
			TipPainterTools.DrawHelpTipFromCombinedDescription(this, pe.Graphics, Font, null, description);
		}

		protected override void OnPaintBackground(PaintEventArgs pe)
		{
			pe.Graphics.FillRectangle(ColorReference.BackgroundBrush, pe.ClipRectangle);
		}
	}
}