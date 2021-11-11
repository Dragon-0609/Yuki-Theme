using System.ComponentModel;

namespace Yuki_Theme.Forms
{
	partial class ColorPicker
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
			this.colorEditor1 = new Cyotek.Windows.Forms.ColorEditor ();
			this.colorEditorManager1 = new Cyotek.Windows.Forms.ColorEditorManager ();
			this.colorWheel1 = new Cyotek.Windows.Forms.ColorWheel ();
			this.screenColorPicker1 = new Cyotek.Windows.Forms.ScreenColorPicker ();
			this.button1 = new System.Windows.Forms.Button ();
			this.button2 = new System.Windows.Forms.Button ();
			this.textBox1 = new System.Windows.Forms.TextBox ();
			this.SuspendLayout ();
			// 
			// colorEditor1
			// 
			this.colorEditor1.Location = new System.Drawing.Point (7, 201);
			this.colorEditor1.Name = "colorEditor1";
			this.colorEditor1.ShowAlphaChannel = false;
			this.colorEditor1.ShowColorSpaceLabels = false;
			this.colorEditor1.ShowHex = false;
			this.colorEditor1.Size = new System.Drawing.Size (236, 148);
			this.colorEditor1.TabIndex = 0;
			// 
			// colorEditorManager1
			// 
			this.colorEditorManager1.Color = System.Drawing.Color.Empty;
			this.colorEditorManager1.ColorEditor = this.colorEditor1;
			this.colorEditorManager1.ColorWheel = this.colorWheel1;
			this.colorEditorManager1.ScreenColorPicker = this.screenColorPicker1;
			// 
			// colorWheel1
			// 
			this.colorWheel1.Alpha = 1D;
			this.colorWheel1.Location = new System.Drawing.Point (7, 12);
			this.colorWheel1.Name = "colorWheel1";
			this.colorWheel1.Size = new System.Drawing.Size (185, 183);
			this.colorWheel1.TabIndex = 1;
			// 
			// screenColorPicker1
			// 
			this.screenColorPicker1.Color = System.Drawing.Color.Empty;
			this.screenColorPicker1.Location = new System.Drawing.Point (198, 12);
			this.screenColorPicker1.Name = "screenColorPicker1";
			this.screenColorPicker1.Size = new System.Drawing.Size (45, 183);
			this.screenColorPicker1.Text = "screenColorPicker1";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button1.Location = new System.Drawing.Point (101, 404);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size (68, 28);
			this.button1.TabIndex = 2;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler (this.button1_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button2.Location = new System.Drawing.Point (175, 404);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size (68, 28);
			this.button2.TabIndex = 3;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler (this.button2_Click);
			// 
			// textBox1
			// 
			this.textBox1.Font = new System.Drawing.Font ("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.textBox1.Location = new System.Drawing.Point (32, 355);
			this.textBox1.MaxLength = 12;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size (97, 26);
			this.textBox1.TabIndex = 4;
			this.textBox1.Text = "#212121";
			this.textBox1.TextChanged += new System.EventHandler (this.textBox1_TextChanged);
			// 
			// ColorPicker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (252, 438);
			this.Controls.Add (this.screenColorPicker1);
			this.Controls.Add (this.textBox1);
			this.Controls.Add (this.button2);
			this.Controls.Add (this.button1);
			this.Controls.Add (this.colorWheel1);
			this.Controls.Add (this.colorEditor1);
			this.Name = "ColorPicker";
			this.Text = "ColorPicker";
			this.ResumeLayout (false);
			this.PerformLayout ();
		}

		private Cyotek.Windows.Forms.ScreenColorPicker screenColorPicker1;

		private System.Windows.Forms.TextBox textBox1;

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;

		private Cyotek.Windows.Forms.ColorWheel colorWheel1;

		private Cyotek.Windows.Forms.ColorEditorManager colorEditorManager1;

		private Cyotek.Windows.Forms.ColorEditor colorEditor1;

		#endregion
	}
}