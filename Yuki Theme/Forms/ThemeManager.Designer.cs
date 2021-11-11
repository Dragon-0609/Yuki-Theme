using System.ComponentModel;

namespace Yuki_Theme.Forms
{
	partial class ThemeManager
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
			this.scheme = new System.Windows.Forms.ListView ();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader ();
			this.remove = new System.Windows.Forms.Button ();
			this.button3 = new System.Windows.Forms.Button ();
			this.button2 = new System.Windows.Forms.Button ();
			this.SuspendLayout ();
			// 
			// scheme
			// 
			this.scheme.AllowColumnReorder = true;
			this.scheme.Columns.AddRange (new System.Windows.Forms.ColumnHeader [] {this.columnHeader1});
			this.scheme.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.scheme.FullRowSelect = true;
			this.scheme.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.scheme.HideSelection = false;
			this.scheme.Location = new System.Drawing.Point (12, 12);
			this.scheme.Margin = new System.Windows.Forms.Padding (0);
			this.scheme.MultiSelect = false;
			this.scheme.Name = "scheme";
			this.scheme.Size = new System.Drawing.Size (228, 302);
			this.scheme.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.scheme.TabIndex = 0;
			this.scheme.UseCompatibleStateImageBehavior = false;
			this.scheme.View = System.Windows.Forms.View.Details;
			this.scheme.SelectedIndexChanged += new System.EventHandler (this.scheme_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 195;
			// 
			// remove
			// 
			this.remove.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.remove.BackgroundImage = global::Yuki_Theme.Properties.Resources.dash_square;
			this.remove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.remove.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.remove.Location = new System.Drawing.Point (59, 320);
			this.remove.Name = "remove";
			this.remove.Size = new System.Drawing.Size (36, 36);
			this.remove.TabIndex = 11;
			this.remove.UseVisualStyleBackColor = true;
			this.remove.Click += new System.EventHandler (this.remove_Click);
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button3.BackgroundImage = global::Yuki_Theme.Properties.Resources.plus_square;
			this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.button3.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button3.Location = new System.Drawing.Point (17, 320);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size (36, 36);
			this.button3.TabIndex = 12;
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler (this.button3_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.AutoSize = true;
			this.button2.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button2.Location = new System.Drawing.Point (169, 326);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size (71, 30);
			this.button2.TabIndex = 13;
			this.button2.Text = "Close";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler (this.button2_Click);
			// 
			// ThemeManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (260, 368);
			this.Controls.Add (this.button2);
			this.Controls.Add (this.button3);
			this.Controls.Add (this.remove);
			this.Controls.Add (this.scheme);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ThemeManager";
			this.Text = "Scheme Manager";
			this.ResumeLayout (false);
			this.PerformLayout ();
		}

		public System.Windows.Forms.ListView scheme;

		private System.Windows.Forms.Button button2;

		private System.Windows.Forms.Button remove;

		private System.Windows.Forms.Button button3;

		private System.Windows.Forms.ColumnHeader columnHeader1;

		#endregion
	}
}