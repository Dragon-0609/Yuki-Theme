using System.ComponentModel;

namespace Yuki_Theme.Core.Forms
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
			this.button1 = new System.Windows.Forms.Button ();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel ();
			this.label2 = new System.Windows.Forms.Label ();
			this.label4 = new System.Windows.Forms.Label ();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel ();
			this.linkLabel3 = new System.Windows.Forms.LinkLabel ();
			this.linkLabel5 = new System.Windows.Forms.LinkLabel ();
			this.linkLabel6 = new System.Windows.Forms.LinkLabel ();
			this.linkLabel7 = new System.Windows.Forms.LinkLabel ();
			this.linkLabel8 = new System.Windows.Forms.LinkLabel ();
			this.linkLabel10 = new System.Windows.Forms.LinkLabel ();
			this.linkLabel11 = new System.Windows.Forms.LinkLabel ();
			this.panel1 = new System.Windows.Forms.FlowLayoutPanel ();
			this.linkLabel12 = new System.Windows.Forms.LinkLabel ();
			this.linkLabel13 = new System.Windows.Forms.LinkLabel ();
			this.linkLabel14 = new System.Windows.Forms.LinkLabel ();
			this.linkLabel15 = new System.Windows.Forms.LinkLabel ();
			this.changelog_link = new System.Windows.Forms.LinkLabel ();
			((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit ();
			this.panel1.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.pictureBox1.Image = global::Yuki_Theme.Core.Properties.Resources.yuki;
			this.pictureBox1.Location = new System.Drawing.Point (0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size (312, 141);
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
			this.linkLabel4.Size = new System.Drawing.Size (312, 51);
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
			this.vers.Size = new System.Drawing.Size (312, 23);
			this.vers.TabIndex = 3;
			this.vers.Text = "version: 1.0";
			this.vers.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point (242, 469);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size (58, 29);
			this.button1.TabIndex = 5;
			this.button1.Text = "Back";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler (this.button1_Click);
			// 
			// linkLabel1
			// 
			this.linkLabel1.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.linkLabel1.Font = new System.Drawing.Font ("Book Antiqua", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel1.LinkColor = System.Drawing.Color.Black;
			this.linkLabel1.Location = new System.Drawing.Point (128, 215);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size (152, 27);
			this.linkLabel1.TabIndex = 7;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Doki Theme";
			this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel1_LinkClicked);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font ("Book Antiqua", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label2.Location = new System.Drawing.Point (6, 216);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size (116, 26);
			this.label2.TabIndex = 8;
			this.label2.Text = "Inspiration:";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font ("Book Antiqua", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label4.Location = new System.Drawing.Point (6, 242);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size (75, 26);
			this.label4.TabIndex = 9;
			this.label4.Text = "Used:";
			// 
			// linkLabel2
			// 
			this.linkLabel2.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.linkLabel2.AutoSize = true;
			this.linkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel2.LinkColor = System.Drawing.Color.Black;
			this.linkLabel2.Location = new System.Drawing.Point (3, 3);
			this.linkLabel2.Margin = new System.Windows.Forms.Padding (3);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size (138, 18);
			this.linkLabel2.TabIndex = 10;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "FastColoredTextBox";
			this.linkLabel2.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel2_LinkClicked);
			// 
			// linkLabel3
			// 
			this.linkLabel3.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.linkLabel3.AutoSize = true;
			this.linkLabel3.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel3.LinkColor = System.Drawing.Color.Black;
			this.linkLabel3.Location = new System.Drawing.Point (3, 27);
			this.linkLabel3.Margin = new System.Windows.Forms.Padding (3);
			this.linkLabel3.Name = "linkLabel3";
			this.linkLabel3.Size = new System.Drawing.Size (133, 18);
			this.linkLabel3.TabIndex = 11;
			this.linkLabel3.TabStop = true;
			this.linkLabel3.Text = "Cyotek ColorPicker";
			this.linkLabel3.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel3_LinkClicked);
			// 
			// linkLabel5
			// 
			this.linkLabel5.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.linkLabel5.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.linkLabel5.Font = new System.Drawing.Font ("Times New Roman", 16F);
			this.linkLabel5.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel5.LinkColor = System.Drawing.Color.Black;
			this.linkLabel5.Location = new System.Drawing.Point (7, 468);
			this.linkLabel5.Name = "linkLabel5";
			this.linkLabel5.Size = new System.Drawing.Size (204, 27);
			this.linkLabel5.TabIndex = 12;
			this.linkLabel5.TabStop = true;
			this.linkLabel5.Text = "Developer: Dragon-LV";
			this.linkLabel5.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel5.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel5_LinkClicked);
			// 
			// linkLabel6
			// 
			this.linkLabel6.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.linkLabel6.AutoSize = true;
			this.linkLabel6.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel6.LinkColor = System.Drawing.Color.Black;
			this.linkLabel6.Location = new System.Drawing.Point (3, 51);
			this.linkLabel6.Margin = new System.Windows.Forms.Padding (3);
			this.linkLabel6.Name = "linkLabel6";
			this.linkLabel6.Size = new System.Drawing.Size (115, 18);
			this.linkLabel6.TabIndex = 13;
			this.linkLabel6.TabStop = true;
			this.linkLabel6.Text = "Newtonsoft.Json";
			this.linkLabel6.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel6.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel6_LinkClicked);
			// 
			// linkLabel7
			// 
			this.linkLabel7.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.linkLabel7.AutoSize = true;
			this.linkLabel7.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel7.LinkColor = System.Drawing.Color.Black;
			this.linkLabel7.Location = new System.Drawing.Point (3, 75);
			this.linkLabel7.Margin = new System.Windows.Forms.Padding (3);
			this.linkLabel7.Name = "linkLabel7";
			this.linkLabel7.Size = new System.Drawing.Size (96, 18);
			this.linkLabel7.TabIndex = 14;
			this.linkLabel7.TabStop = true;
			this.linkLabel7.Text = "RJ ComboBox";
			this.linkLabel7.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel7.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel7_LinkClicked);
			// 
			// linkLabel8
			// 
			this.linkLabel8.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.linkLabel8.AutoSize = true;
			this.linkLabel8.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel8.LinkColor = System.Drawing.Color.Black;
			this.linkLabel8.Location = new System.Drawing.Point (3, 99);
			this.linkLabel8.Margin = new System.Windows.Forms.Padding (3);
			this.linkLabel8.Name = "linkLabel8";
			this.linkLabel8.Size = new System.Drawing.Size (70, 18);
			this.linkLabel8.TabIndex = 15;
			this.linkLabel8.TabStop = true;
			this.linkLabel8.Text = "SVG.NET";
			this.linkLabel8.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel8.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel8_LinkClicked);
			// 
			// linkLabel10
			// 
			this.linkLabel10.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.linkLabel10.AutoSize = true;
			this.linkLabel10.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel10.LinkColor = System.Drawing.Color.Black;
			this.linkLabel10.Location = new System.Drawing.Point (3, 123);
			this.linkLabel10.Margin = new System.Windows.Forms.Padding (3);
			this.linkLabel10.Name = "linkLabel10";
			this.linkLabel10.Size = new System.Drawing.Size (85, 18);
			this.linkLabel10.TabIndex = 17;
			this.linkLabel10.TabStop = true;
			this.linkLabel10.Text = "Color Slider";
			this.linkLabel10.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel10.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel10_LinkClicked);
			// 
			// linkLabel11
			// 
			this.linkLabel11.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.linkLabel11.AutoSize = true;
			this.linkLabel11.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel11.LinkColor = System.Drawing.Color.Black;
			this.linkLabel11.Location = new System.Drawing.Point (3, 147);
			this.linkLabel11.Margin = new System.Windows.Forms.Padding (3);
			this.linkLabel11.Name = "linkLabel11";
			this.linkLabel11.Size = new System.Drawing.Size (156, 18);
			this.linkLabel11.TabIndex = 18;
			this.linkLabel11.TabStop = true;
			this.linkLabel11.Text = "WindowsAPICodePack";
			this.linkLabel11.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel11.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel11_LinkClicked);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add (this.linkLabel2);
			this.panel1.Controls.Add (this.linkLabel3);
			this.panel1.Controls.Add (this.linkLabel6);
			this.panel1.Controls.Add (this.linkLabel7);
			this.panel1.Controls.Add (this.linkLabel8);
			this.panel1.Controls.Add (this.linkLabel10);
			this.panel1.Controls.Add (this.linkLabel11);
			this.panel1.Controls.Add (this.linkLabel12);
			this.panel1.Controls.Add (this.linkLabel13);
			this.panel1.Controls.Add (this.linkLabel14);
			this.panel1.Controls.Add (this.linkLabel15);
			this.panel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.panel1.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.panel1.Location = new System.Drawing.Point (87, 242);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size (215, 210);
			this.panel1.TabIndex = 19;
			this.panel1.WrapContents = false;
			// 
			// linkLabel12
			// 
			this.linkLabel12.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.linkLabel12.AutoSize = true;
			this.linkLabel12.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel12.LinkColor = System.Drawing.Color.Black;
			this.linkLabel12.Location = new System.Drawing.Point (3, 171);
			this.linkLabel12.Margin = new System.Windows.Forms.Padding (3);
			this.linkLabel12.Name = "linkLabel12";
			this.linkLabel12.Size = new System.Drawing.Size (145, 18);
			this.linkLabel12.TabIndex = 19;
			this.linkLabel12.TabStop = true;
			this.linkLabel12.Text = "FlatNumericUpDown";
			this.linkLabel12.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel12.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel12_LinkClicked);
			// 
			// linkLabel13
			// 
			this.linkLabel13.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.linkLabel13.AutoSize = true;
			this.linkLabel13.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel13.LinkColor = System.Drawing.Color.Black;
			this.linkLabel13.Location = new System.Drawing.Point (3, 195);
			this.linkLabel13.Margin = new System.Windows.Forms.Padding (3);
			this.linkLabel13.MaximumSize = new System.Drawing.Size (190, 0);
			this.linkLabel13.Name = "linkLabel13";
			this.linkLabel13.Size = new System.Drawing.Size (190, 36);
			this.linkLabel13.TabIndex = 20;
			this.linkLabel13.TabStop = true;
			this.linkLabel13.Text = "MechanikaDesign.WinForms.UI.ColorPicker";
			this.linkLabel13.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel13.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel13_LinkClicked);
			// 
			// linkLabel14
			// 
			this.linkLabel14.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.linkLabel14.AutoSize = true;
			this.linkLabel14.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel14.LinkColor = System.Drawing.Color.Black;
			this.linkLabel14.Location = new System.Drawing.Point (3, 237);
			this.linkLabel14.Margin = new System.Windows.Forms.Padding (3);
			this.linkLabel14.Name = "linkLabel14";
			this.linkLabel14.Size = new System.Drawing.Size (134, 18);
			this.linkLabel14.TabIndex = 21;
			this.linkLabel14.TabStop = true;
			this.linkLabel14.Text = "CommonMark.NET";
			this.linkLabel14.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel14.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel14_LinkClicked);
			// 
			// linkLabel15
			// 
			this.linkLabel15.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.linkLabel15.AutoSize = true;
			this.linkLabel15.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel15.LinkColor = System.Drawing.Color.Black;
			this.linkLabel15.Location = new System.Drawing.Point (3, 261);
			this.linkLabel15.Margin = new System.Windows.Forms.Padding (3);
			this.linkLabel15.Name = "linkLabel15";
			this.linkLabel15.Size = new System.Drawing.Size (102, 18);
			this.linkLabel15.TabIndex = 22;
			this.linkLabel15.TabStop = true;
			this.linkLabel15.Text = "JetBrains Icons";
			this.linkLabel15.VisitedLinkColor = System.Drawing.Color.Black;
			this.linkLabel15.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.linkLabel15_LinkClicked);
			// 
			// changelog_link
			// 
			this.changelog_link.ActiveLinkColor = System.Drawing.Color.FromArgb (((int) (((byte) (125)))), ((int) (((byte) (125)))), ((int) (((byte) (125)))));
			this.changelog_link.Font = new System.Drawing.Font ("Book Antiqua", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.changelog_link.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.changelog_link.LinkColor = System.Drawing.Color.Black;
			this.changelog_link.Location = new System.Drawing.Point (0, 290);
			this.changelog_link.Margin = new System.Windows.Forms.Padding (3);
			this.changelog_link.Name = "changelog_link";
			this.changelog_link.Size = new System.Drawing.Size (81, 31);
			this.changelog_link.TabIndex = 20;
			this.changelog_link.TabStop = true;
			this.changelog_link.Text = "Changelog";
			this.changelog_link.VisitedLinkColor = System.Drawing.Color.Black;
			this.changelog_link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.changelog_link_LinkClicked);
			// 
			// AboutForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size (312, 504);
			this.ControlBox = false;
			this.Controls.Add (this.changelog_link);
			this.Controls.Add (this.panel1);
			this.Controls.Add (this.linkLabel5);
			this.Controls.Add (this.label4);
			this.Controls.Add (this.label2);
			this.Controls.Add (this.linkLabel1);
			this.Controls.Add (this.button1);
			this.Controls.Add (this.vers);
			this.Controls.Add (this.linkLabel4);
			this.Controls.Add (this.pictureBox1);
			this.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.Text = "About";
			this.Shown += new System.EventHandler (this.AboutForm_Shown);
			((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit ();
			this.panel1.ResumeLayout (false);
			this.panel1.PerformLayout ();
			this.ResumeLayout (false);
		}

		private System.Windows.Forms.LinkLabel changelog_link;

		private System.Windows.Forms.LinkLabel linkLabel15;

		private System.Windows.Forms.LinkLabel linkLabel14;

		private System.Windows.Forms.LinkLabel linkLabel13;

		private System.Windows.Forms.FlowLayoutPanel panel1;
		private System.Windows.Forms.LinkLabel       linkLabel12;

		private System.Windows.Forms.LinkLabel linkLabel11;

		private System.Windows.Forms.LinkLabel linkLabel10;

		private System.Windows.Forms.LinkLabel linkLabel8;

		private System.Windows.Forms.LinkLabel linkLabel7;

		private System.Windows.Forms.LinkLabel linkLabel6;

		private System.Windows.Forms.LinkLabel linkLabel5;

		private System.Windows.Forms.LinkLabel linkLabel3;

		private System.Windows.Forms.LinkLabel linkLabel2;

		private System.Windows.Forms.Label label4;

		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.LinkLabel linkLabel1;

		private System.Windows.Forms.Button button1;

		private System.Windows.Forms.Label vers;

		private System.Windows.Forms.LinkLabel linkLabel4;

		private System.Windows.Forms.PictureBox pictureBox1;

		#endregion
	}
}