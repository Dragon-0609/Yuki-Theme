using System.ComponentModel;

namespace Yuki_Theme.Core.Forms
{
	partial class RenameForm
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
			this.label2 = new System.Windows.Forms.Label ();
			this.toTBox = new Yuki_Theme.Core.Controls.CustomText ();
			this.fromTBox = new Yuki_Theme.Core.Controls.CustomText ();
			this.SuspendLayout ();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.AutoSize = true;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button1.Location = new System.Drawing.Point (215, 91);
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
			this.button2.Location = new System.Drawing.Point (305, 91);
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
			this.label1.Size = new System.Drawing.Size (117, 29);
			this.label1.TabIndex = 2;
			this.label1.Text = "From:";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label2.Location = new System.Drawing.Point (11, 53);
			this.label2.Margin = new System.Windows.Forms.Padding (5, 0, 5, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size (117, 29);
			this.label2.TabIndex = 4;
			this.label2.Text = "To:";
			// 
			// toTBox
			// 
			this.toTBox.BorderColor = System.Drawing.Color.Blue;
			this.toTBox.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.toTBox.Location = new System.Drawing.Point (138, 53);
			this.toTBox.Margin = new System.Windows.Forms.Padding (5);
			this.toTBox.MaxLength = 35;
			this.toTBox.Name = "toTBox";
			this.toTBox.Size = new System.Drawing.Size (249, 24);
			this.toTBox.TabIndex = 5;
			// 
			// fromTBox
			// 
			this.fromTBox.BorderColor = System.Drawing.Color.Blue;
			this.fromTBox.Enabled = false;
			this.fromTBox.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.fromTBox.Location = new System.Drawing.Point (138, 13);
			this.fromTBox.Margin = new System.Windows.Forms.Padding (5);
			this.fromTBox.MaxLength = 35;
			this.fromTBox.Name = "fromTBox";
			this.fromTBox.Size = new System.Drawing.Size (249, 24);
			this.fromTBox.TabIndex = 6;
			// 
			// RenameForm
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleDimensions = new System.Drawing.SizeF (8F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size (400, 136);
			this.Controls.Add (this.fromTBox);
			this.Controls.Add (this.toTBox);
			this.Controls.Add (this.label2);
			this.Controls.Add (this.label1);
			this.Controls.Add (this.button2);
			this.Controls.Add (this.button1);
			this.Font = new System.Drawing.Font ("Book Antiqua", 10F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding (5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RenameForm";
			this.ShowInTaskbar = false;
			this.Text = "Rename";
			this.Shown += new System.EventHandler (this.RenameForm_Shown);
			this.ResumeLayout (false);
			this.PerformLayout ();
		}

		private System.Windows.Forms.Label          label2;
		public  Yuki_Theme.Core.Controls.CustomText toTBox;
		public  Yuki_Theme.Core.Controls.CustomText fromTBox;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.Button button2;

		private System.Windows.Forms.Button button1;

		#endregion
	}
}