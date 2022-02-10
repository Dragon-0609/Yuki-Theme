using System.ComponentModel;

namespace Yuki_Theme.Core.Forms
{
	partial class QuestionForm_2
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
			this.Message = new System.Windows.Forms.Label ();
			this.button1 = new System.Windows.Forms.Button ();
			this.button2 = new System.Windows.Forms.Button ();
			this.SuspendLayout ();
			// 
			// Message
			// 
			this.Message.Dock = System.Windows.Forms.DockStyle.Top;
			this.Message.Font = new System.Drawing.Font ("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.Message.Location = new System.Drawing.Point (0, 0);
			this.Message.Name = "Message";
			this.Message.Size = new System.Drawing.Size (404, 72);
			this.Message.TabIndex = 0;
			this.Message.Text = "I use Google Analytics to track installs. If you don\'t want to be tracked, I won\'" + "t track.";
			this.Message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new System.Drawing.Font ("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button1.Location = new System.Drawing.Point (318, 77);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size (74, 34);
			this.button1.TabIndex = 1;
			this.button1.Text = "Decline";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler (this.button1_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Font = new System.Drawing.Font ("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button2.Location = new System.Drawing.Point (238, 77);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size (74, 34);
			this.button2.TabIndex = 2;
			this.button2.Text = "Accept";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler (this.button2_Click);
			// 
			// QuestionForm_2
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size (404, 117);
			this.Controls.Add (this.button2);
			this.Controls.Add (this.button1);
			this.Controls.Add (this.Message);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "QuestionForm_2";
			this.Text = "Google Analytics";
			this.ResumeLayout (false);
		}

		private System.Windows.Forms.Button button2;

		private System.Windows.Forms.Button button1;

		private System.Windows.Forms.Label Message;

		#endregion
	}
}