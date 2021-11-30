using System;
using System.Drawing;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Forms
{
	public partial class ColorPicker : Form
	{
		private MForm form;
		public ColorPicker (MForm fm)
		{
			InitializeComponent ();
			form = fm;
			this.StartPosition = FormStartPosition.CenterParent;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (MForm));
			Icon = ((Icon) (resources.GetObject ("$this.Icon")));
			colorEditorManager1.ColorChanged += (sender, args) => { textBox1.Text = ColorTranslator.ToHtml (colorEditorManager1.Color); };
		}

		public Color MainColor
		{
			get => colorEditorManager1.Color;
			set => colorEditorManager1.Color = value;
		}

		private void button1_Click (object sender, EventArgs e)
		{
			if (form.isDefault ())
			{
				MessageBox.Show ((IWin32Window) this, "This theme is default theme, so it's readonly! It can't be changed. You can just copy this and after that you can change it.", "This theme is readonly", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}else
				DialogResult = DialogResult.OK;
		}

		private void button2_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void textBox1_TextChanged (object sender, EventArgs e)
		{
			Color clrs = TranslateColor (textBox1.Text);
			if (colorEditorManager1.Color != clrs)
				colorEditorManager1.Color = clrs;
		}

		private Color TranslateColor (string str)
		{
			if (textBox1.Text.StartsWith ("#") && textBox1.Text.Length == 7)
			{
				try
				{
					 return ColorTranslator.FromHtml (str);
				} catch (FormatException)
				{
					
				}
				
			} else
			{
				return Color.FromName (textBox1.Text);
			}

			return Color.White;
		}
	}
}