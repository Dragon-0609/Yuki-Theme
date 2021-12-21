using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Forms
{
	public partial class ChangelogForm : Form
	{
		public ChangelogForm ()
		{
			InitializeComponent ();

			Assembly a = Assembly.GetExecutingAssembly ();

			Stream stm = a.GetManifestResourceStream ($"Yuki_Theme.Core.Resources.CHANGELOG.md");
			string md = "";
			using (StreamReader reader = new StreamReader (stm))
			{
				md = reader.ReadToEnd ();
			}

			md = md.Split (new [] {"###"}, StringSplitOptions.None) [1]
			       .Split (new [] {"##"}, StringSplitOptions.None) [1];

			stm = a.GetManifestResourceStream ($"Yuki_Theme.Core.Resources.CHANGELOG.html");
			string html = "";
			using (StreamReader reader = new StreamReader (stm))
			{
				html = reader.ReadToEnd ();
			}

			html = html.Replace ("__bg__", ColorTranslator.ToHtml (Helper.bgColor));
			html = html.Replace ("__clr__", ColorTranslator.ToHtml (Helper.fgColor));
			string str = CommonMark.CommonMarkConverter.Convert (md);

			html = html.Replace ("__content__", str);
			webBrowser1.DocumentText = html;
		}

		private void ChangelogForm_Shown (object sender, EventArgs e)
		{
			StartPosition = FormStartPosition.Manual;
			Location = new Point (Owner.Location.X, Owner.Location.Y);
			button1.BackColor = panel1.BackColor = label1.BackColor = BackColor = Helper.bgColor;
			button1.ForeColor = panel1.ForeColor = label1.ForeColor = ForeColor = Helper.fgColor;
			label1.Focus ();
		}

		private void button1_Click (object sender, EventArgs e)
		{
			this.Close ();
		}

		private void webBrowser1_DocumentCompleted (object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			webBrowser1.Height = webBrowser1.Document.Body.ClientRectangle.Height;
			ClientSize = new Size (ClientSize.Width, webBrowser1.Height + 29);
		}
	}
}