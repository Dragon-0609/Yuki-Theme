using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Yuki_Theme.Core.Forms
{
	[ComVisible (true)]
	public partial class ChangelogForm : Form
	{
		private bool expanded = false;

		public ChangelogForm ()
		{
			InitializeComponent ();
			Text = label1.Text = API_Base.Current.Translate ("changelog.title");

			string md = Helper.ReadHTML ("CHANGELOG.md");
			string html = Helper.ReadHTML ("CHANGELOG.html");

			md = md.Split (new [] { "###" }, StringSplitOptions.None) [1]
			       .Split (new [] { "##" }, StringSplitOptions.None) [1];
			
			html = Helper.ReplaceHTMLColors (html);

			md = ReplaceCheckbox (md);
			string str = CommonMark.CommonMarkConverter.Convert (md);

			html = html.Replace ("Expand", API_Base.Current.Translate ("changelog.expand")).Replace ("__content__", str);
			webBrowser1.DocumentText = html;
			webBrowser1.ScrollBarsEnabled = true;
			webBrowser1.ObjectForScripting = this;
		}

		private static string ReplaceCheckbox (string md)
		{
			md = md.Replace ("- [x]", "<br><input disabled type='checkbox' checked='checked'>");
			md = md.Replace ("- [ ]", "<br><input disabled type='checkbox'>");
			return md;
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
			Close ();
		}

		private void webBrowser1_DocumentCompleted (object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			webBrowser1.Height = webBrowser1.Document.Body.ClientRectangle.Height + 35;
			ClientSize = new Size (ClientSize.Width, webBrowser1.Height + 29);
		}

		public void Expand ()
		{
			expanded = !expanded;
			string ntxt = "";
			if (expanded)
			{
				ntxt = API_Base.Current.Translate ("changelog.collapse");
			} else
			{
				ntxt = API_Base.Current.Translate ("changelog.expand");
			}

			Assembly a = Assembly.GetExecutingAssembly ();

			Stream stm = a.GetManifestResourceStream ($"Yuki_Theme.Core.Resources.CHANGELOG.md");
			string mdd = "";
			using (StreamReader reader = new StreamReader (stm))
			{
				mdd = reader.ReadToEnd ();
			}

			mdd = mdd.Split (new [] { "###" }, StringSplitOptions.None) [1];
			if (!expanded)
				mdd = mdd.Split (new [] { "##" }, StringSplitOptions.None) [1];
			mdd = ReplaceCheckbox (mdd);
			mdd = CommonMark.CommonMarkConverter.Convert (mdd);
			stm.Dispose ();
			// Load SVG
			webBrowser1.Document.GetElementById ("content").InnerHtml = mdd;
			webBrowser1.Document.GetElementById ("expander_button").InnerHtml = ntxt;
			if (!expanded)
			{
				webBrowser1.Height = webBrowser1.Document.Body.ClientRectangle.Height + 35;
				ClientSize = new Size (ClientSize.Width, webBrowser1.Height + 29);
			} else
			{
				webBrowser1.Height = 400;
				ClientSize = new Size (ClientSize.Width, 429);
			}
		}
	}
}