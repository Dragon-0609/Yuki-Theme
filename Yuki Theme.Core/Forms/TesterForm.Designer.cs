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
			this.customTab1 = new Yuki_Theme.Core.Controls.CustomTab ();
			this.tabPage1 = new System.Windows.Forms.TabPage ();
			this.tabPage2 = new System.Windows.Forms.TabPage ();
			this.tabPage3 = new System.Windows.Forms.TabPage ();
			this.customPB1 = new Yuki_Theme.Core.Controls.CustomPB ();
			this.customTab1.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// customTab1
			// 
			this.customTab1.AllowDrop = true;
			this.customTab1.BackgroundColor = System.Drawing.Color.FromArgb (((int) (((byte) (45)))), ((int) (((byte) (45)))), ((int) (((byte) (48)))));
			this.customTab1.BorderTabLineColor = System.Drawing.Color.FromArgb (((int) (((byte) (0)))), ((int) (((byte) (122)))), ((int) (((byte) (204)))));
			this.customTab1.Controls.Add (this.tabPage1);
			this.customTab1.Controls.Add (this.tabPage2);
			this.customTab1.Controls.Add (this.tabPage3);
			this.customTab1.DisableClose = true;
			this.customTab1.DisableDragging = true;
			this.customTab1.Font = new System.Drawing.Font ("Segoe UI", 9F);
			this.customTab1.HoverTabButtonColor = System.Drawing.Color.FromArgb (((int) (((byte) (82)))), ((int) (((byte) (176)))), ((int) (((byte) (239)))));
			this.customTab1.HoverTabColor = System.Drawing.Color.FromArgb (((int) (((byte) (28)))), ((int) (((byte) (151)))), ((int) (((byte) (234)))));
			this.customTab1.HoverUnselectedTabButtonColor = System.Drawing.Color.FromArgb (((int) (((byte) (85)))), ((int) (((byte) (85)))), ((int) (((byte) (85)))));
			this.customTab1.Location = new System.Drawing.Point (39, 47);
			this.customTab1.Name = "customTab1";
			this.customTab1.Padding = new System.Drawing.Point (14, 4);
			this.customTab1.SelectedIndex = 0;
			this.customTab1.SelectedTabButtonColor = System.Drawing.Color.FromArgb (((int) (((byte) (28)))), ((int) (((byte) (151)))), ((int) (((byte) (234)))));
			this.customTab1.SelectedTabColor = System.Drawing.Color.FromArgb (((int) (((byte) (0)))), ((int) (((byte) (122)))), ((int) (((byte) (204)))));
			this.customTab1.Size = new System.Drawing.Size (409, 338);
			this.customTab1.TabIndex = 0;
			this.customTab1.TextColor = System.Drawing.Color.White;
			this.customTab1.UnderBorderTabLineColor = System.Drawing.Color.FromArgb (((int) (((byte) (67)))), ((int) (((byte) (67)))), ((int) (((byte) (70)))));
			this.customTab1.UnselectedBorderTabLineColor = System.Drawing.Color.FromArgb (((int) (((byte) (63)))), ((int) (((byte) (63)))), ((int) (((byte) (70)))));
			this.customTab1.UnselectedTabColor = System.Drawing.Color.FromArgb (((int) (((byte) (63)))), ((int) (((byte) (63)))), ((int) (((byte) (70)))));
			this.customTab1.UpDownBackColor = System.Drawing.Color.FromArgb (((int) (((byte) (63)))), ((int) (((byte) (63)))), ((int) (((byte) (70)))));
			this.customTab1.UpDownTextColor = System.Drawing.Color.FromArgb (((int) (((byte) (109)))), ((int) (((byte) (109)))), ((int) (((byte) (112)))));
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.Gray;
			this.tabPage1.Location = new System.Drawing.Point (4, 27);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding (3);
			this.tabPage1.Size = new System.Drawing.Size (401, 307);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point (4, 27);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding (3);
			this.tabPage2.Size = new System.Drawing.Size (401, 307);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tabPage3
			// 
			this.tabPage3.Location = new System.Drawing.Point (4, 27);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding (3);
			this.tabPage3.Size = new System.Drawing.Size (401, 307);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "tabPage3";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// customPB1
			// 
			this.customPB1.BarColor = System.Drawing.Color.Lime;
			this.customPB1.BorderThickness = 0;
			this.customPB1.DrawBorder = true;
			this.customPB1.Location = new System.Drawing.Point (479, 128);
			this.customPB1.Name = "customPB1";
			this.customPB1.Size = new System.Drawing.Size (162, 22);
			this.customPB1.TabIndex = 2;
			this.customPB1.Text = "customPB1";
			this.customPB1.Value = 10;
			// 
			// TesterForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (800, 450);
			this.Controls.Add (this.customPB1);
			this.Controls.Add (this.customTab1);
			this.Name = "TesterForm";
			this.Text = "TesterForm";
			this.customTab1.ResumeLayout (false);
			this.ResumeLayout (false);
		}

		private Yuki_Theme.Core.Controls.CustomPB customPB1;

		private System.Windows.Forms.TabPage tabPage3;

		private Yuki_Theme.Core.Controls.CustomTab customTab1;
		private System.Windows.Forms.TabPage       tabPage1;
		private System.Windows.Forms.TabPage       tabPage2;

		#endregion
	}
}