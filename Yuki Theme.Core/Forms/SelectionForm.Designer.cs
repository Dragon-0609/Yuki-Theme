using System.ComponentModel;

namespace Yuki_Theme.Core.Forms
{
	partial class SelectionForm
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
			this.textBox1 = new Yuki_Theme.Core.Controls.CustomText ();
			this.comboBox1 = new CustomControls.RJControls.RJComboBox ();
			this.SuspendLayout ();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.AutoSize = true;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new System.Drawing.Font ("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
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
			this.button2.Font = new System.Drawing.Font ("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
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
			this.label1.Font = new System.Drawing.Font ("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label1.Location = new System.Drawing.Point (11, 13);
			this.label1.Margin = new System.Windows.Forms.Padding (5, 0, 5, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (117, 29);
			this.label1.TabIndex = 2;
			this.label1.Text = "Copy from:";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font ("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label2.Location = new System.Drawing.Point (11, 53);
			this.label2.Margin = new System.Windows.Forms.Padding (5, 0, 5, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size (117, 29);
			this.label2.TabIndex = 4;
			this.label2.Text = "Name:";
			// 
			// textBox1
			// 
			this.textBox1.BorderColor = System.Drawing.Color.Blue;
			this.textBox1.Font = new System.Drawing.Font ("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.textBox1.Location = new System.Drawing.Point (138, 53);
			this.textBox1.Margin = new System.Windows.Forms.Padding (5);
			this.textBox1.MaxLength = 35;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size (249, 24);
			this.textBox1.TabIndex = 5;
			// 
			// comboBox1
			// 
			this.comboBox1.BackColor = System.Drawing.Color.WhiteSmoke;
			this.comboBox1.BorderColor = System.Drawing.Color.MediumSlateBlue;
			this.comboBox1.BorderSize = 1;
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.Font = new System.Drawing.Font ("Microsoft Sans Serif", 10F);
			this.comboBox1.ForeColor = System.Drawing.Color.DimGray;
			this.comboBox1.IconColor = System.Drawing.Color.MediumSlateBlue;
			this.comboBox1.ListBackColor = System.Drawing.Color.FromArgb (((int) (((byte) (230)))), ((int) (((byte) (228)))), ((int) (((byte) (245)))));
			this.comboBox1.ListTextColor = System.Drawing.Color.DimGray;
			this.comboBox1.Location = new System.Drawing.Point (139, 12);
			this.comboBox1.MinimumSize = new System.Drawing.Size (200, 30);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Padding = new System.Windows.Forms.Padding (1);
			this.comboBox1.Size = new System.Drawing.Size (249, 30);
			this.comboBox1.TabIndex = 6;
			this.comboBox1.Texts = "";
			// 
			// SelectionForm
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleDimensions = new System.Drawing.SizeF (96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size (400, 136);
			this.Controls.Add (this.comboBox1);
			this.Controls.Add (this.textBox1);
			this.Controls.Add (this.label2);
			this.Controls.Add (this.label1);
			this.Controls.Add (this.button2);
			this.Controls.Add (this.button1);
			this.Font = new System.Drawing.Font ("Calibri", 10F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding (5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectionForm";
			this.ShowInTaskbar = false;
			this.Text = "Selection";
			this.Shown += new System.EventHandler (this.SelectionForm_Shown);
			this.ResumeLayout (false);
			this.PerformLayout ();
		}

		private System.Windows.Forms.Label          label2;
		public  Yuki_Theme.Core.Controls.CustomText textBox1;

		public CustomControls.RJControls.RJComboBox comboBox1;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.Button button2;

		private System.Windows.Forms.Button button1;

		#endregion
	}
}