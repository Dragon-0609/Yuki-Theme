namespace Yuki_Theme_Plugin
{
	partial class Form3
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip ();
			this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
			this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
			this.testToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem ();
			this.test2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
			this.test3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
			((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit ();
			this.menuStrip1.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Yuki_Theme_Plugin.Properties.Resources.YukiTheme;
			this.pictureBox1.Location = new System.Drawing.Point (194, 134);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size (113, 108);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange (new System.Windows.Forms.ToolStripItem [] {this.menuToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point (0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size (800, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// menuToolStripMenuItem
			// 
			this.menuToolStripMenuItem.DropDownItems.AddRange (new System.Windows.Forms.ToolStripItem [] {this.testToolStripMenuItem});
			this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
			this.menuToolStripMenuItem.Size = new System.Drawing.Size (50, 20);
			this.menuToolStripMenuItem.Text = "Menu";
			// 
			// testToolStripMenuItem
			// 
			this.testToolStripMenuItem.DropDownItems.AddRange (new System.Windows.Forms.ToolStripItem [] {this.testToolStripMenuItem1, this.test2ToolStripMenuItem, this.test3ToolStripMenuItem});
			this.testToolStripMenuItem.Name = "testToolStripMenuItem";
			this.testToolStripMenuItem.Size = new System.Drawing.Size (152, 22);
			this.testToolStripMenuItem.Text = "Test";
			// 
			// testToolStripMenuItem1
			// 
			this.testToolStripMenuItem1.Name = "testToolStripMenuItem1";
			this.testToolStripMenuItem1.Size = new System.Drawing.Size (152, 22);
			this.testToolStripMenuItem1.Text = "Test1";
			// 
			// test2ToolStripMenuItem
			// 
			this.test2ToolStripMenuItem.Name = "test2ToolStripMenuItem";
			this.test2ToolStripMenuItem.Size = new System.Drawing.Size (152, 22);
			this.test2ToolStripMenuItem.Text = "Test2";
			// 
			// test3ToolStripMenuItem
			// 
			this.test3ToolStripMenuItem.Name = "test3ToolStripMenuItem";
			this.test3ToolStripMenuItem.Size = new System.Drawing.Size (152, 22);
			this.test3ToolStripMenuItem.Text = "Test3";
			// 
			// Form3
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size (800, 450);
			this.Controls.Add (this.pictureBox1);
			this.Controls.Add (this.menuStrip1);
			this.DoubleBuffered = true;
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form3";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit ();
			this.menuStrip1.ResumeLayout (false);
			this.menuStrip1.PerformLayout ();
			this.ResumeLayout (false);
			this.PerformLayout ();
		}

		private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem test2ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem test3ToolStripMenuItem;

		private System.Windows.Forms.MenuStrip menuStrip1;

		private System.Windows.Forms.PictureBox pictureBox1;

		#endregion
	}
}