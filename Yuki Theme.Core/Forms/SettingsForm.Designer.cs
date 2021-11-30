using System.ComponentModel;

namespace Yuki_Theme.Core.Forms
{
	partial class SettingsForm
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
			this.textBox1 = new Yuki_Theme.Core.Controls.CustomText ();
			this.button1 = new System.Windows.Forms.Button ();
			this.checkBox1 = new System.Windows.Forms.CheckBox ();
			this.button2 = new System.Windows.Forms.Button ();
			this.button3 = new System.Windows.Forms.Button ();
			this.label2 = new System.Windows.Forms.Label ();
			this.askC = new System.Windows.Forms.CheckBox ();
			this.label3 = new System.Windows.Forms.Label ();
			this.label4 = new System.Windows.Forms.Label ();
			this.button4 = new System.Windows.Forms.Button ();
			this.checkBox2 = new System.Windows.Forms.CheckBox ();
			this.button5 = new System.Windows.Forms.Button ();
			this.label5 = new System.Windows.Forms.Label ();
			this.button6 = new System.Windows.Forms.Button ();
			this.schemes = new CustomControls.RJControls.RJComboBox ();
			this.ActionBox = new CustomControls.RJControls.RJComboBox ();
			this.mode = new CustomControls.RJControls.RJComboBox ();
			this.SuspendLayout ();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label1.Location = new System.Drawing.Point (16, 12);
			this.label1.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (199, 26);
			this.label1.TabIndex = 0;
			this.label1.Text = "Path to PascalABC.NET:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBox1
			// 
			this.textBox1.BorderColor = System.Drawing.Color.Blue;
			this.textBox1.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.textBox1.Location = new System.Drawing.Point (262, 14);
			this.textBox1.Margin = new System.Windows.Forms.Padding (4);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size (196, 24);
			this.textBox1.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button1.Location = new System.Drawing.Point (462, 14);
			this.button1.Margin = new System.Windows.Forms.Padding (0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size (25, 25);
			this.button1.TabIndex = 2;
			this.button1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler (this.button1_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point (12, 93);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size (210, 29);
			this.checkBox1.TabIndex = 3;
			this.checkBox1.Text = "Remember active scheme";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.AutoSize = true;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Location = new System.Drawing.Point (361, 301);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size (71, 30);
			this.button2.TabIndex = 4;
			this.button2.Text = "Save";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler (this.button2_Click);
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button3.AutoSize = true;
			this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button3.Location = new System.Drawing.Point (447, 301);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size (71, 30);
			this.button3.TabIndex = 5;
			this.button3.Text = "Cancel";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler (this.button3_Click);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label2.Location = new System.Drawing.Point (12, 59);
			this.label2.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size (203, 31);
			this.label2.TabIndex = 11;
			this.label2.Text = "Active Scheme";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// askC
			// 
			this.askC.Location = new System.Drawing.Point (12, 128);
			this.askC.Name = "askC";
			this.askC.Size = new System.Drawing.Size (395, 29);
			this.askC.TabIndex = 12;
			this.askC.Text = "Ask if there are other themes in PascalABC directory";
			this.askC.UseVisualStyleBackColor = true;
			this.askC.CheckedChanged += new System.EventHandler (this.checkBox2_CheckedChanged);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label3.Location = new System.Drawing.Point (12, 163);
			this.label3.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size (268, 31);
			this.label3.TabIndex = 14;
			this.label3.Text = "Do action, if there are other themes";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label4.Location = new System.Drawing.Point (12, 205);
			this.label4.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size (203, 31);
			this.label4.TabIndex = 15;
			this.label4.Text = "Setting Mode";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// button4
			// 
			this.button4.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button4.AutoSize = true;
			this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button4.Location = new System.Drawing.Point (16, 301);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size (71, 30);
			this.button4.TabIndex = 17;
			this.button4.Text = "About";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler (this.button4_Click);
			// 
			// checkBox2
			// 
			this.checkBox2.Location = new System.Drawing.Point (12, 257);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size (127, 29);
			this.checkBox2.TabIndex = 18;
			this.checkBox2.Text = "Check updates";
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// button5
			// 
			this.button5.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button5.AutoSize = true;
			this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button5.Location = new System.Drawing.Point (447, 256);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size (71, 30);
			this.button5.TabIndex = 19;
			this.button5.Text = "Check";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler (this.button5_Click);
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label5.Location = new System.Drawing.Point (146, 258);
			this.label5.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size (160, 31);
			this.label5.TabIndex = 20;
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// button6
			// 
			this.button6.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button6.AutoSize = true;
			this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button6.Location = new System.Drawing.Point (311, 256);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size (130, 30);
			this.button6.TabIndex = 21;
			this.button6.Text = "Update Manually";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler (this.button6_Click);
			// 
			// schemes
			// 
			this.schemes.BackColor = System.Drawing.Color.WhiteSmoke;
			this.schemes.BorderColor = System.Drawing.Color.MediumSlateBlue;
			this.schemes.BorderSize = 1;
			this.schemes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.schemes.Font = new System.Drawing.Font ("Lucida Fax", 10F);
			this.schemes.ForeColor = System.Drawing.Color.DimGray;
			this.schemes.IconColor = System.Drawing.Color.MediumSlateBlue;
			this.schemes.ListBackColor = System.Drawing.Color.FromArgb (((int) (((byte) (230)))), ((int) (((byte) (228)))), ((int) (((byte) (245)))));
			this.schemes.ListTextColor = System.Drawing.Color.DimGray;
			this.schemes.Location = new System.Drawing.Point (262, 60);
			this.schemes.MinimumSize = new System.Drawing.Size (200, 30);
			this.schemes.Name = "schemes";
			this.schemes.Padding = new System.Windows.Forms.Padding (1);
			this.schemes.Size = new System.Drawing.Size (256, 30);
			this.schemes.TabIndex = 22;
			this.schemes.Texts = "";
			// 
			// ActionBox
			// 
			this.ActionBox.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ActionBox.BorderColor = System.Drawing.Color.MediumSlateBlue;
			this.ActionBox.BorderSize = 1;
			this.ActionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ActionBox.Font = new System.Drawing.Font ("Lucida Fax", 10F);
			this.ActionBox.ForeColor = System.Drawing.Color.DimGray;
			this.ActionBox.IconColor = System.Drawing.Color.MediumSlateBlue;
			this.ActionBox.ListBackColor = System.Drawing.Color.FromArgb (((int) (((byte) (230)))), ((int) (((byte) (228)))), ((int) (((byte) (245)))));
			this.ActionBox.ListTextColor = System.Drawing.Color.DimGray;
			this.ActionBox.Location = new System.Drawing.Point (320, 163);
			this.ActionBox.MinimumSize = new System.Drawing.Size (200, 30);
			this.ActionBox.Name = "ActionBox";
			this.ActionBox.Padding = new System.Windows.Forms.Padding (1);
			this.ActionBox.Size = new System.Drawing.Size (200, 30);
			this.ActionBox.TabIndex = 23;
			this.ActionBox.Texts = "";
			// 
			// mode
			// 
			this.mode.BackColor = System.Drawing.Color.WhiteSmoke;
			this.mode.BorderColor = System.Drawing.Color.MediumSlateBlue;
			this.mode.BorderSize = 1;
			this.mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.mode.Font = new System.Drawing.Font ("Microsoft Sans Serif", 10F);
			this.mode.ForeColor = System.Drawing.Color.DimGray;
			this.mode.IconColor = System.Drawing.Color.MediumSlateBlue;
			this.mode.ListBackColor = System.Drawing.Color.FromArgb (((int) (((byte) (230)))), ((int) (((byte) (228)))), ((int) (((byte) (245)))));
			this.mode.ListTextColor = System.Drawing.Color.DimGray;
			this.mode.Location = new System.Drawing.Point (321, 206);
			this.mode.MinimumSize = new System.Drawing.Size (200, 30);
			this.mode.Name = "mode";
			this.mode.Padding = new System.Windows.Forms.Padding (1);
			this.mode.Size = new System.Drawing.Size (200, 30);
			this.mode.TabIndex = 24;
			this.mode.Texts = "";
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (8F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (533, 343);
			this.Controls.Add (this.mode);
			this.Controls.Add (this.ActionBox);
			this.Controls.Add (this.schemes);
			this.Controls.Add (this.button6);
			this.Controls.Add (this.label5);
			this.Controls.Add (this.button5);
			this.Controls.Add (this.checkBox2);
			this.Controls.Add (this.button4);
			this.Controls.Add (this.label4);
			this.Controls.Add (this.label3);
			this.Controls.Add (this.askC);
			this.Controls.Add (this.label2);
			this.Controls.Add (this.button3);
			this.Controls.Add (this.button2);
			this.Controls.Add (this.checkBox1);
			this.Controls.Add (this.button1);
			this.Controls.Add (this.textBox1);
			this.Controls.Add (this.label1);
			this.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding (4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SettingsForm";
			this.Text = "Settings";
			this.Shown += new System.EventHandler (this.SettingsForm_Shown);
			this.ResumeLayout (false);
			this.PerformLayout ();
		}

		public CustomControls.RJControls.RJComboBox ActionBox;

		private System.Windows.Forms.Button button6;

		public  System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.Button   button5;
		private System.Windows.Forms.Label    label5;

		private System.Windows.Forms.Button button4;

		public CustomControls.RJControls.RJComboBox mode;

		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;

		public System.Windows.Forms.CheckBox askC;

		public System.Windows.Forms.ComboBox comboBox1;

		private System.Windows.Forms.Label label2;

		public CustomControls.RJControls.RJComboBox schemes;

		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;

		private System.Windows.Forms.CheckBox checkBox1;

		private Yuki_Theme.Core.Controls.CustomText textBox1;
		private System.Windows.Forms.Button         button1;

		private System.Windows.Forms.Label label1;

		#endregion
	}
}