using System.ComponentModel;

namespace Yuki_Theme.Core.Forms
{
	partial class HelperForm
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
			this.panel1 = new System.Windows.Forms.Panel ();
			this.label2 = new System.Windows.Forms.Label ();
			this.panel1.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.AutoSize = true;
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point (199, 49);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size (70, 27);
			this.button1.TabIndex = 0;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler (this.button1_Click);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.AutoSize = true;
			this.panel1.Controls.Add (this.label2);
			this.panel1.Location = new System.Drawing.Point (12, 12);
			this.panel1.MaximumSize = new System.Drawing.Size (257, 0);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding (0, 5, 0, 5);
			this.panel1.Size = new System.Drawing.Size (257, 27);
			this.panel1.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font ("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point (2, 7);
			this.label2.MaximumSize = new System.Drawing.Size (250, 0);
			this.label2.Name = "label2";
			this.label2.Padding = new System.Windows.Forms.Padding (5, 0, 0, 0);
			this.label2.Size = new System.Drawing.Size (5, 15);
			this.label2.TabIndex = 0;
			// 
			// HelperForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size (281, 88);
			this.Controls.Add (this.panel1);
			this.Controls.Add (this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Location = new System.Drawing.Point (15, 15);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HelperForm";
			this.ShowIcon = false;
			this.panel1.ResumeLayout (false);
			this.panel1.PerformLayout ();
			this.ResumeLayout (false);
			this.PerformLayout ();
		}

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.Button button1;

		#endregion
	}
}