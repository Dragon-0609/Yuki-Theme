using System.ComponentModel;

namespace Yuki_Theme.Core.Forms
{
	internal partial class QuestionForm
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
			this.button3 = new System.Windows.Forms.Button ();
			this.SuspendLayout ();
			// 
			// Message
			// 
			this.Message.Dock = System.Windows.Forms.DockStyle.Top;
			this.Message.Font = new System.Drawing.Font ("Calibri", 11F);
			this.Message.Location = new System.Drawing.Point (0, 0);
			this.Message.Name = "Message";
			this.Message.Size = new System.Drawing.Size (404, 105);
			this.Message.TabIndex = 0;
			this.Message.Text = "Other themes found. Do you want to delete them?\r\nNOTE: PascalABC.NET may not use " + "this scheme, so you need to delete others.";
			this.Message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new System.Drawing.Font ("Calibri", 9F);
			this.button1.Location = new System.Drawing.Point (297, 105);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size (95, 34);
			this.button1.TabIndex = 1;
			this.button1.Text = "No";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler (this.button1_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Font = new System.Drawing.Font ("Calibri", 9F);
			this.button2.Location = new System.Drawing.Point (12, 105);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size (95, 34);
			this.button2.TabIndex = 2;
			this.button2.Text = "Yes";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler (this.button2_Click);
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button3.Font = new System.Drawing.Font ("Calibri", 9F);
			this.button3.Location = new System.Drawing.Point (122, 105);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size (158, 34);
			this.button3.TabIndex = 3;
			this.button3.Text = "Import and Delete";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler (this.button3_Click);
			// 
			// QuestionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size (404, 145);
			this.Controls.Add (this.button3);
			this.Controls.Add (this.button2);
			this.Controls.Add (this.button1);
			this.Controls.Add (this.Message);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Location = new System.Drawing.Point (15, 15);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "QuestionForm";
			this.Shown += new System.EventHandler (this.QuestionForm_Shown);
			this.ResumeLayout (false);
		}

		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;

		private System.Windows.Forms.Button button1;

		private System.Windows.Forms.Label Message;

		#endregion
	}
}