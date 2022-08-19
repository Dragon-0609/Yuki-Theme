using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Yuki_Theme.Core.Controls;

namespace Yuki_Theme.Core.Forms
{
	public partial class PathForm : Form
	{
		public  bool          isFromPascal = false;
		private SettingsPanel sp;
		public PathForm (SettingsPanel s)
		{
			InitializeComponent ();
			StartPosition = FormStartPosition.CenterParent;
			loadSVG ();
			button1.Text = API.API.Current.Translate ("messages.theme.save.short");
			button2.Text = API.API.Current.Translate ("download.cancel");
			label1.Text = API.API.Current.Translate ("custom_sticker.title");
			Text = API.API.Current.Translate ("custom_sticker.form.title");
			sp = s;
		}

		private void button1_Click (object sender, EventArgs e)
		{
			// DialogResult = DialogResult.OK;
		}

		private void button2_Click (object sender, EventArgs e)
		{
			// DialogResult = DialogResult.Cancel;
		}

		public void loadSVG ()
		{
			var a = Assembly.GetExecutingAssembly ();

			string add = Helper.IsDark (Helper.bgColor) ? "" : "_dark";
			Helper.RenderSvg (other, Helper.LoadSvg ("moreHorizontal" + add, a));
		}
		
		private void PathForm_Shown (object sender, EventArgs e)
		{
			Color bg = Color.Empty;
			Color fg = Color.Empty;
			Color border = Color.Empty;
			Color click = Color.Empty;

			if (!isFromPascal)
			{
				bg = Helper.bgColor;
				fg = Helper.fgColor;
				border = Helper.fgKeyword;
				click = Helper.bgClick;
			} else
			{
				bg = sp.bg;
				fg = sp.fg;
				border = sp.key;
				click = sp.click;
			}
			
			BackColor = button1.BackColor = button2.BackColor =
				path.BackColor = other.BackColor = bg;
				
			ForeColor = button1.FlatAppearance.BorderColor = button2.FlatAppearance.BorderColor = other.FlatAppearance.BorderColor =
				path.ForeColor = fg;
						
			button1.FlatAppearance.MouseOverBackColor = button2.FlatAppearance.MouseOverBackColor = 
				other.FlatAppearance.MouseOverBackColor = click;

			path.BorderColor = border;
		}

		private void other_Click (object sender, EventArgs e)
		{
			var op = new OpenFileDialog ();
			op.DefaultExt = "png";
			// op.Filter = "Transparent Image(*.png,*.gif)|*.png;*.gif|PNG (*.png)|*.png|GIF (*.gif)|*.gif";
			op.Filter = "PNG (*.png)|*.png";
			op.Multiselect = false;
			if (op.ShowDialog () == DialogResult.OK)
			{
				path.Text = op.FileName;
			}
		}
	}
}