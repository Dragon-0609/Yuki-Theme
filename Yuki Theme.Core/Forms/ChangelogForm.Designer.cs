using System.ComponentModel;

namespace Yuki_Theme.Core.Forms
{
	partial class ChangelogForm
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
			this.panel1 = new System.Windows.Forms.Panel ();
			this.button1 = new System.Windows.Forms.Button ();
			this.label1 = new System.Windows.Forms.Label ();
			this.webBrowser1 = new System.Windows.Forms.WebBrowser ();
			this.panel1.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// panel1
			// 
			this.panel1.Controls.Add (this.button1);
			this.panel1.Controls.Add (this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point (0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size (284, 29);
			this.panel1.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.AutoSize = true;
			this.button1.FlatAppearance.BorderSize = 0;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point (256, 2);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size (25, 27);
			this.button1.TabIndex = 5;
			this.button1.Text = "X";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler (this.button1_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font ("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label1.Location = new System.Drawing.Point (44, 2);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (193, 27);
			this.label1.TabIndex = 1;
			this.label1.Text = "Changelog";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// webBrowser1
			// 
			this.webBrowser1.AllowNavigation = false;
			this.webBrowser1.AllowWebBrowserDrop = false;
			this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Top;
			this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
			this.webBrowser1.Location = new System.Drawing.Point (0, 29);
			this.webBrowser1.MinimumSize = new System.Drawing.Size (20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.ScrollBarsEnabled = false;
			this.webBrowser1.Size = new System.Drawing.Size (284, 36);
			this.webBrowser1.TabIndex = 1;
			this.webBrowser1.WebBrowserShortcutsEnabled = false;
			this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler (this.webBrowser1_DocumentCompleted);
			// 
			// ChangelogForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (284, 64);
			this.Controls.Add (this.webBrowser1);
			this.Controls.Add (this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "ChangelogForm";
			this.Text = "ChangelogForm";
			this.Shown += new System.EventHandler (this.ChangelogForm_Shown);
			this.panel1.ResumeLayout (false);
			this.panel1.PerformLayout ();
			this.ResumeLayout (false);
		}

		public  System.Windows.Forms.Button     button1;
		private System.Windows.Forms.Label      label1;
		private System.Windows.Forms.WebBrowser webBrowser1;

		private System.Windows.Forms.Panel panel1;

		#endregion
	}
}