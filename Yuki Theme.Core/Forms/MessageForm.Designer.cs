using System.ComponentModel;

namespace Yuki_Theme.Core.Forms
{
	partial class MessageForm
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
			this.button1 = new System.Windows.Forms.Button ();
			this.label1 = new System.Windows.Forms.Label ();
			this.panel1 = new System.Windows.Forms.Panel ();
			this.label2 = new System.Windows.Forms.Label ();
			this.panel1.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// button1
			// 
			this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.button1.AutoSize = true;
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point (207, 368);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size (70, 27);
			this.button1.TabIndex = 0;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = false;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font ("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label1.Location = new System.Drawing.Point (6, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (466, 34);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add (this.label2);
			this.panel1.Location = new System.Drawing.Point (5, 37);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size (475, 325);
			this.panel1.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font ("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label2.Location = new System.Drawing.Point (2, 2);
			this.label2.MaximumSize = new System.Drawing.Size (455, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size (41, 15);
			this.label2.TabIndex = 0;
			this.label2.Text = "label2";
			// 
			// MessageForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size (484, 401);
			this.Controls.Add (this.panel1);
			this.Controls.Add (this.label1);
			this.Controls.Add (this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MessageForm";
			this.ShowIcon = false;
			this.Text = "MessageForm_Designer";
			this.panel1.ResumeLayout (false);
			this.panel1.PerformLayout ();
			this.ResumeLayout (false);
			this.PerformLayout ();
		}

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.Button button1;

		#endregion
	}
}