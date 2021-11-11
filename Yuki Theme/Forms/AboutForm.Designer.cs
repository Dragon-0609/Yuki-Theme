using System.ComponentModel;

namespace Yuki_Theme.Forms
{
	partial class AboutForm
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox ();
			this.linkLabel4 = new System.Windows.Forms.LinkLabel ();
			this.vers = new System.Windows.Forms.Label ();
			this.label3 = new System.Windows.Forms.Label ();
			this.button1 = new System.Windows.Forms.Button ();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel ();
			this.label2 = new System.Windows.Forms.Label ();
			this.label4 = new System.Windows.Forms.Label ();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel ();
			this.linkLabel3 = new System.Windows.Forms.LinkLabel ();
			((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit ();
			this.SuspendLayout ();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.pictureBox1.Image = global::Yuki_Theme.Properties.Resources.yuki;
			this.pictureBox1.Location = new System.Drawing.Point (0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size (287, 141);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// linkLabel4
			// 
			this.linkLabel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.linkLabel4.Font = new System.Drawing.Font ("Times New Roman", 24F);
			this.linkLabel4.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel4.LinkColor = System.Drawing.Color.Black;
			this.linkLabel4.Location = new System.Drawing.Point (0, 141);
			this.linkLabel4.Name = "linkLabel4";
			this.linkLabel4.Size = new System.Drawing.Size (287, 51);
			this.linkLabel4.TabIndex = 2;
			this.linkLabel4.TabStop = true;
			this.linkLabel4.Text = "Yuki Theme";
			this.linkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.linkLabel4.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel4_LinkClicked);
			// 
			// vers
			// 
			this.vers.Dock = System.Windows.Forms.DockStyle.Top;
			this.vers.Font = new System.Drawing.Font ("Times New Roman", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.vers.Location = new System.Drawing.Point (0, 192);
			this.vers.Name = "vers";
			this.vers.Size = new System.Drawing.Size (287, 23);
			this.vers.TabIndex = 3;
			this.vers.Text = "version: 1.0";
			this.vers.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.Font = new System.Drawing.Font ("Times New Roman", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label3.Location = new System.Drawing.Point (0, 398);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size (204, 45);
			this.label3.TabIndex = 4;
			this.label3.Text = "Made by: Dragon-LV";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point (210, 408);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size (65, 29);
			this.button1.TabIndex = 5;
			this.button1.Text = "Back";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler (this.button1_Click);
			// 
			// linkLabel1
			// 
			this.linkLabel1.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel1.LinkColor = System.Drawing.Color.Black;
			this.linkLabel1.Location = new System.Drawing.Point (129, 211);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size (102, 27);
			this.linkLabel1.TabIndex = 7;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Doki Theme";
			this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel1_LinkClicked);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point (7, 212);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size (116, 26);
			this.label2.TabIndex = 8;
			this.label2.Text = "Motivation:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point (7, 238);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size (87, 26);
			this.label4.TabIndex = 9;
			this.label4.Text = "Used:";
			// 
			// linkLabel2
			// 
			this.linkLabel2.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.linkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel2.LinkColor = System.Drawing.Color.Black;
			this.linkLabel2.Location = new System.Drawing.Point (100, 238);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size (175, 27);
			this.linkLabel2.TabIndex = 10;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "FastColoredTextBox";
			this.linkLabel2.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel2_LinkClicked);
			// 
			// linkLabel3
			// 
			this.linkLabel3.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.linkLabel3.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel3.LinkColor = System.Drawing.Color.Black;
			this.linkLabel3.Location = new System.Drawing.Point (100, 265);
			this.linkLabel3.Name = "linkLabel3";
			this.linkLabel3.Size = new System.Drawing.Size (175, 27);
			this.linkLabel3.TabIndex = 11;
			this.linkLabel3.TabStop = true;
			this.linkLabel3.Text = "Cyotek ColorPicker";
			this.linkLabel3.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel3_LinkClicked);
			// 
			// AboutForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size (287, 443);
			this.Controls.Add (this.linkLabel3);
			this.Controls.Add (this.linkLabel2);
			this.Controls.Add (this.label4);
			this.Controls.Add (this.label2);
			this.Controls.Add (this.linkLabel1);
			this.Controls.Add (this.button1);
			this.Controls.Add (this.label3);
			this.Controls.Add (this.vers);
			this.Controls.Add (this.linkLabel4);
			this.Controls.Add (this.pictureBox1);
			this.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "AboutForm";
			this.Text = "About";
			((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit ();
			this.ResumeLayout (false);
		}

		private System.Windows.Forms.LinkLabel linkLabel3;

		private System.Windows.Forms.LinkLabel linkLabel2;

		private System.Windows.Forms.Label label4;

		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.LinkLabel linkLabel1;

		private System.Windows.Forms.Button button1;

		private System.Windows.Forms.Label vers;
		private System.Windows.Forms.Label label3;

		private System.Windows.Forms.LinkLabel linkLabel4;

		private System.Windows.Forms.PictureBox pictureBox1;

		#endregion
	}
}