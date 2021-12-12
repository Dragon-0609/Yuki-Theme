using System.ComponentModel;

namespace Yuki_Theme.Core.Forms
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
			this.add = new System.Windows.Forms.Button ();
			this.button2 = new System.Windows.Forms.Button ();
			this.rename_btn = new System.Windows.Forms.Button ();
			this.SuspendLayout ();
			// 
			// scheme
			// 
			this.scheme.AutoArrange = false;
			this.scheme.Columns.AddRange (new System.Windows.Forms.ColumnHeader [] {this.columnHeader1});
			this.scheme.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.scheme.FullRowSelect = true;
			this.scheme.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.scheme.HideSelection = false;
			this.scheme.Location = new System.Drawing.Point (12, 12);
			this.scheme.Margin = new System.Windows.Forms.Padding (0);
			this.scheme.MultiSelect = false;
			this.scheme.Name = "scheme";
			this.scheme.OwnerDraw = true;
			this.scheme.ShowGroups = false;
			this.scheme.Size = new System.Drawing.Size (308, 367);
			this.scheme.TabIndex = 0;
			this.scheme.TileSize = new System.Drawing.Size (228, 302);
			this.scheme.UseCompatibleStateImageBehavior = false;
			this.scheme.View = System.Windows.Forms.View.Details;
			this.scheme.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler (this.scheme_DrawColumnHeader);
			this.scheme.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler (this.scheme_DrawItem);
			this.scheme.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler (this.scheme_ItemSelectionChanged);
			this.scheme.SelectedIndexChanged += new System.EventHandler (this.scheme_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Width = 270;
			// 
			// remove
			// 
			this.remove.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.remove.BackgroundImage = global::Yuki_Theme.Core.Properties.Resources.dash_square;
			this.remove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.remove.Enabled = false;
			this.remove.FlatAppearance.BorderSize = 0;
			this.remove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.remove.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.remove.Location = new System.Drawing.Point (45, 396);
			this.remove.Name = "remove";
			this.remove.Size = new System.Drawing.Size (24, 24);
			this.remove.TabIndex = 11;
			this.remove.UseVisualStyleBackColor = true;
			this.remove.Click += new System.EventHandler (this.remove_Click);
			// 
			// add
			// 
			this.add.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.add.BackgroundImage = global::Yuki_Theme.Core.Properties.Resources.plus_square;
			this.add.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.add.FlatAppearance.BorderSize = 0;
			this.add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.add.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.add.Location = new System.Drawing.Point (12, 396);
			this.add.Name = "add";
			this.add.Size = new System.Drawing.Size (24, 24);
			this.add.TabIndex = 12;
			this.add.UseVisualStyleBackColor = true;
			this.add.Click += new System.EventHandler (this.button3_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.AutoSize = true;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Font = new System.Drawing.Font ("Lucida Fax", 10F);
			this.button2.Location = new System.Drawing.Point (249, 392);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size (71, 28);
			this.button2.TabIndex = 13;
			this.button2.Text = "Close";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler (this.button2_Click);
			// 
			// rename_btn
			// 
			this.rename_btn.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.rename_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.rename_btn.Enabled = false;
			this.rename_btn.FlatAppearance.BorderSize = 0;
			this.rename_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.rename_btn.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.rename_btn.Location = new System.Drawing.Point (78, 396);
			this.rename_btn.Name = "rename_btn";
			this.rename_btn.Size = new System.Drawing.Size (24, 24);
			this.rename_btn.TabIndex = 14;
			this.rename_btn.UseVisualStyleBackColor = false;
			this.rename_btn.Click += new System.EventHandler (this.rename_btn_Click);
			// 
			// ThemeManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (334, 431);
			this.Controls.Add (this.rename_btn);
			this.Controls.Add (this.button2);
			this.Controls.Add (this.add);
			this.Controls.Add (this.remove);
			this.Controls.Add (this.scheme);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ThemeManager";
			this.Text = "Scheme Manager";
			this.Load += new System.EventHandler (this.ThemeManager_Load);
			this.Shown += new System.EventHandler (this.ThemeManager_Shown);
			this.ResumeLayout (false);
			this.PerformLayout ();
		}

		private System.Windows.Forms.Button rename_btn;

		private System.Windows.Forms.Button button1;

		private System.Windows.Forms.ColumnHeader columnHeader1;

		public System.Windows.Forms.ListView scheme;

		private System.Windows.Forms.Button button2;

		private System.Windows.Forms.Button remove;

		private System.Windows.Forms.Button add;

		#endregion
	}
}