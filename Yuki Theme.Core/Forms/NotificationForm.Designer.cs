using System.ComponentModel;

namespace Yuki_Theme.Core.Forms
{
	partial class NotificationForm
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
			this.tcontent = new System.Windows.Forms.Label ();
			this.ttitle = new System.Windows.Forms.Label ();
			this.button2 = new System.Windows.Forms.Button ();
			this.button1 = new System.Windows.Forms.LinkLabel ();
			this.button3 = new System.Windows.Forms.LinkLabel ();
			this.SuspendLayout ();
			// 
			// tcontent
			// 
			this.tcontent.BackColor = System.Drawing.SystemColors.Control;
			this.tcontent.Location = new System.Drawing.Point (12, 30);
			this.tcontent.Name = "tcontent";
			this.tcontent.Size = new System.Drawing.Size (291, 17);
			this.tcontent.TabIndex = 1;
			// 
			// ttitle
			// 
			this.ttitle.Font = new System.Drawing.Font ("Calibri", 10F, System.Drawing.FontStyle.Bold);
			this.ttitle.Location = new System.Drawing.Point (12, 5);
			this.ttitle.Name = "ttitle";
			this.ttitle.Size = new System.Drawing.Size (268, 25);
			this.ttitle.TabIndex = 2;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.AutoSize = true;
			this.button2.FlatAppearance.BorderSize = 0;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Location = new System.Drawing.Point (283, 5);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size (25, 27);
			this.button2.TabIndex = 4;
			this.button2.Text = "X";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler (this.label1_Click);
			this.button2.MouseEnter += new System.EventHandler (this.button2_MouseEnter);
			this.button2.MouseLeave += new System.EventHandler (this.button2_MouseLeave);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.button1.LinkColor = System.Drawing.Color.Black;
			this.button1.Location = new System.Drawing.Point (12, 52);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size (93, 17);
			this.button1.TabIndex = 5;
			this.button1.TabStop = true;
			this.button1.Text = "linkLabel1";
			this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.button1.VisitedLinkColor = System.Drawing.Color.Black;
			this.button1.Click += new System.EventHandler (this.button1_Click);
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button3.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.button3.LinkColor = System.Drawing.Color.Black;
			this.button3.Location = new System.Drawing.Point (111, 52);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size (93, 17);
			this.button3.TabIndex = 6;
			this.button3.TabStop = true;
			this.button3.Text = "linkLabel1";
			this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.button3.VisitedLinkColor = System.Drawing.Color.Black;
			this.button3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.button3_LinkClicked);
			// 
			// NotificationForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size (315, 78);
			this.Controls.Add (this.button3);
			this.Controls.Add (this.button1);
			this.Controls.Add (this.button2);
			this.Controls.Add (this.ttitle);
			this.Controls.Add (this.tcontent);
			this.Font = new System.Drawing.Font ("Calibri", 10F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "NotificationForm";
			this.ShowInTaskbar = false;
			this.Text = "NotificationForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler (this.NotificationForm_FormClosing);
			this.Shown += new System.EventHandler (this.NotificationForm_Shown);
			this.ResumeLayout (false);
			this.PerformLayout ();
		}

		public System.Windows.Forms.LinkLabel button3;

		public System.Windows.Forms.LinkLabel linkLabel1;

		public System.Windows.Forms.Button button2;

		public System.Windows.Forms.LinkLabel button1;

		private System.Windows.Forms.Label tcontent;
		private System.Windows.Forms.Label ttitle;

		#endregion
	}
}