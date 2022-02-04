using System.ComponentModel;

namespace Yuki_Theme.Core.Forms
{
	partial class PathForm
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
			this.button2 = new System.Windows.Forms.Button ();
			this.label1 = new System.Windows.Forms.Label ();
			this.path = new Yuki_Theme.Core.Controls.CustomText ();
			this.other = new System.Windows.Forms.Button ();
			this.SuspendLayout ();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.AutoSize = true;
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button1.Location = new System.Drawing.Point (215, 85);
			this.button1.Margin = new System.Windows.Forms.Padding (5);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size (80, 30);
			this.button1.TabIndex = 0;
			this.button1.Text = "Save";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler (this.button1_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.AutoSize = true;
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button2.Location = new System.Drawing.Point (305, 85);
			this.button2.Margin = new System.Windows.Forms.Padding (5);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size (79, 30);
			this.button2.TabIndex = 1;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler (this.button2_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label1.Location = new System.Drawing.Point (11, 13);
			this.label1.Margin = new System.Windows.Forms.Padding (5, 0, 5, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (115, 29);
			this.label1.TabIndex = 2;
			this.label1.Text = "Custom Sticker:";
			// 
			// path
			// 
			this.path.BorderColor = System.Drawing.Color.Blue;
			this.path.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.path.Location = new System.Drawing.Point (14, 47);
			this.path.Margin = new System.Windows.Forms.Padding (5);
			this.path.MaxLength = 1000;
			this.path.Name = "path";
			this.path.Size = new System.Drawing.Size (347, 24);
			this.path.TabIndex = 5;
			// 
			// other
			// 
			this.other.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.other.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.other.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.other.Location = new System.Drawing.Point (360, 47);
			this.other.Margin = new System.Windows.Forms.Padding (5);
			this.other.Name = "other";
			this.other.Size = new System.Drawing.Size (24, 24);
			this.other.TabIndex = 6;
			this.other.UseVisualStyleBackColor = true;
			this.other.Click += new System.EventHandler (this.other_Click);
			// 
			// PathForm
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleDimensions = new System.Drawing.SizeF (8F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size (400, 130);
			this.Controls.Add (this.other);
			this.Controls.Add (this.path);
			this.Controls.Add (this.label1);
			this.Controls.Add (this.button2);
			this.Controls.Add (this.button1);
			this.Font = new System.Drawing.Font ("Book Antiqua", 10F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding (5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PathForm";
			this.ShowInTaskbar = false;
			this.Text = "Image Path";
			this.Shown += new System.EventHandler (this.PathForm_Shown);
			this.ResumeLayout (false);
			this.PerformLayout ();
		}

		private System.Windows.Forms.Button other;

		public  Yuki_Theme.Core.Controls.CustomText path;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.Button button2;

		private System.Windows.Forms.Button button1;

		#endregion
	}
}