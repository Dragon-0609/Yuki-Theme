using System.ComponentModel;

namespace Yuki_Theme.Core.Forms
{
	partial class ThemeDownloaderForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose ();
			}

			base.Dispose (disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			this.browser = new System.Windows.Forms.WebBrowser ();
			this.SuspendLayout ();
			// 
			// browser
			// 
			this.browser.AllowNavigation = false;
			this.browser.AllowWebBrowserDrop = false;
			this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.browser.Location = new System.Drawing.Point (0, 0);
			this.browser.MinimumSize = new System.Drawing.Size (20, 20);
			this.browser.Name = "browser";
			this.browser.Size = new System.Drawing.Size (418, 331);
			this.browser.TabIndex = 0;
			this.browser.WebBrowserShortcutsEnabled = false;
			// 
			// ThemeDownloaderForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size (418, 331);
			this.Controls.Add (this.browser);
			this.MinimumSize = new System.Drawing.Size (300, 300);
			this.Name = "ThemeDownloaderForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ThemeDownloaderForm";
			this.ResumeLayout (false);
		}

		private System.Windows.Forms.WebBrowser browser;

		#endregion
	}

}