using System.ComponentModel;
using Yuki_Theme.Core.Controls;

namespace Yuki_Theme.Core.Forms
{
	partial class DownloadForm
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
			this.progressBar1 = new System.Windows.Forms.ProgressBar ();
			this.pr = new System.Windows.Forms.Label ();
			this.button1 = new System.Windows.Forms.Button ();
			this.pr_mb = new System.Windows.Forms.Label ();
			this.downl = new Yuki_Theme.Core.Controls.CustomButton ();
			this.SuspendLayout ();
			// 
			// progressBar1
			// 
			this.progressBar1.BackColor = System.Drawing.Color.Gray;
			this.progressBar1.Location = new System.Drawing.Point (8, 11);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size (263, 15);
			this.progressBar1.Step = 1;
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar1.TabIndex = 0;
			// 
			// pr
			// 
			this.pr.Location = new System.Drawing.Point (228, 30);
			this.pr.Name = "pr";
			this.pr.Size = new System.Drawing.Size (43, 20);
			this.pr.TabIndex = 1;
			this.pr.Text = "0%";
			this.pr.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point (196, 53);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size (75, 28);
			this.button1.TabIndex = 2;
			this.button1.Text = "Cancel";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler (this.button1_Click);
			// 
			// pr_mb
			// 
			this.pr_mb.Location = new System.Drawing.Point (13, 29);
			this.pr_mb.Name = "pr_mb";
			this.pr_mb.Size = new System.Drawing.Size (209, 20);
			this.pr_mb.TabIndex = 3;
			// 
			// downl
			// 
			this.downl.Location = new System.Drawing.Point (86, -6);
			this.downl.Name = "downl";
			this.downl.Size = new System.Drawing.Size (75, 24);
			this.downl.TabIndex = 4;
			this.downl.UseVisualStyleBackColor = true;
			this.downl.Visible = false;
			this.downl.Click += new System.EventHandler (this.downl_Click);
			// 
			// DownloadForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size (284, 93);
			this.Controls.Add (this.downl);
			this.Controls.Add (this.pr_mb);
			this.Controls.Add (this.button1);
			this.Controls.Add (this.pr);
			this.Controls.Add (this.progressBar1);
			this.Font = new System.Drawing.Font ("Calibri", 12F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "DownloadForm";
			this.Text = "DownloadForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler (this.DownloadForm_FormClosing);
			this.Shown += new System.EventHandler (this.DownloadForm_Shown);
			this.ResumeLayout (false);
		}

		public CustomButton downl;

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label  pr;
		private System.Windows.Forms.Label  pr_mb;

		private System.Windows.Forms.ProgressBar progressBar1;

		#endregion
	}
}