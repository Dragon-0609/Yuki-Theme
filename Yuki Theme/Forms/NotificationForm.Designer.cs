using System.ComponentModel;

namespace Yuki_Theme.Forms
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
			this.label1 = new System.Windows.Forms.Label ();
			this.tcontent = new System.Windows.Forms.Label ();
			this.ttitle = new System.Windows.Forms.Label ();
			this.button1 = new System.Windows.Forms.Button ();
			this.SuspendLayout ();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point (255, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (17, 18);
			this.label1.TabIndex = 0;
			this.label1.Text = "X";
			this.label1.Click += new System.EventHandler (this.label1_Click);
			// 
			// tcontent
			// 
			this.tcontent.BackColor = System.Drawing.SystemColors.Control;
			this.tcontent.Location = new System.Drawing.Point (7, 38);
			this.tcontent.Name = "tcontent";
			this.tcontent.Size = new System.Drawing.Size (262, 73);
			this.tcontent.TabIndex = 1;
			// 
			// ttitle
			// 
			this.ttitle.Font = new System.Drawing.Font ("Book Antiqua", 12F);
			this.ttitle.Location = new System.Drawing.Point (7, 5);
			this.ttitle.Name = "ttitle";
			this.ttitle.Size = new System.Drawing.Size (239, 33);
			this.ttitle.TabIndex = 2;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point (193, 84);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size (76, 27);
			this.button1.TabIndex = 3;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler (this.button1_Click);
			// 
			// NotificationForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size (284, 116);
			this.Controls.Add (this.button1);
			this.Controls.Add (this.ttitle);
			this.Controls.Add (this.tcontent);
			this.Controls.Add (this.label1);
			this.Font = new System.Drawing.Font ("Book Antiqua", 10F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "NotificationForm";
			this.ShowInTaskbar = false;
			this.Text = "NotificationForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler (this.NotificationForm_FormClosing);
			this.ResumeLayout (false);
			this.PerformLayout ();
		}

		public System.Windows.Forms.Button button1;

		private System.Windows.Forms.Label tcontent;
		private System.Windows.Forms.Label ttitle;

		private System.Windows.Forms.Label label1;

		#endregion
	}
}