using System.ComponentModel;

namespace Yuki_Theme.Core.Forms
{
	partial class TesterForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (TesterForm));
			this.textBox1 = new System.Windows.Forms.TextBox ();
			this.panel1 = new System.Windows.Forms.Panel ();
			this.button1 = new System.Windows.Forms.Button ();
			this.stickerPicture1 = new System.Windows.Forms.PictureBox ();
			this.panel1.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.Color.DimGray;
			this.textBox1.ForeColor = System.Drawing.Color.White;
			this.textBox1.Location = new System.Drawing.Point (0, 0);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox1.Size = new System.Drawing.Size (752, 305);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = resources.GetString ("textBox1.Text");
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb (((int) (((byte) (64)))), ((int) (((byte) (64)))), ((int) (((byte) (64)))));
			this.panel1.BackgroundImage = ((System.Drawing.Image) (resources.GetObject ("panel1.BackgroundImage")));
			this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.panel1.Controls.Add (this.button1);
			this.panel1.Location = new System.Drawing.Point (0, 302);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size (800, 148);
			this.panel1.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point (776, 3);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size (21, 22);
			this.button1.TabIndex = 0;
			this.button1.Text = "x";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// stickerPicture1
			// 
			this.stickerPicture1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.stickerPicture1.BackColor = System.Drawing.Color.Transparent;
			this.stickerPicture1.Location = new System.Drawing.Point (710, 370);
			this.stickerPicture1.Name = "stickerPicture1";
			this.stickerPicture1.Size = new System.Drawing.Size (83, 69);
			this.stickerPicture1.TabIndex = 2;
			// 
			// TesterForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (800, 450);
			this.Controls.Add (this.stickerPicture1);
			this.Controls.Add (this.panel1);
			this.Controls.Add (this.textBox1);
			this.Name = "TesterForm";
			this.Text = "TesterForm";
			this.panel1.ResumeLayout (false);
			this.ResumeLayout (false);
			this.PerformLayout ();
		}

		private System.Windows.Forms.PictureBox stickerPicture1;

		private System.Windows.Forms.Button         button1;

		private System.Windows.Forms.Panel panel1;

		private System.Windows.Forms.TextBox textBox1;

		#endregion
	}
}