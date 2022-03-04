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
			this.closeButton = new System.Windows.Forms.Button ();
			this.rename_btn = new System.Windows.Forms.Button ();
			this.regenerate = new System.Windows.Forms.Button ();
			this.panel1 = new System.Windows.Forms.Panel ();
			this.mask_1 = new System.Windows.Forms.Panel ();
			this.panel2 = new System.Windows.Forms.Panel ();
			this.mask_2 = new System.Windows.Forms.Panel ();
			this.panel2_2 = new System.Windows.Forms.Panel ();
			this.label1 = new System.Windows.Forms.Label ();
			this.label2 = new System.Windows.Forms.Label ();
			this.panel1.SuspendLayout ();
			this.panel2.SuspendLayout ();
			this.mask_2.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// scheme
			// 
			this.scheme.AutoArrange = false;
			this.scheme.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.scheme.Columns.AddRange (new System.Windows.Forms.ColumnHeader [] { this.columnHeader1 });
			this.scheme.Font = new System.Drawing.Font ("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
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
			this.remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.remove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.remove.FlatAppearance.BorderSize = 0;
			this.remove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.remove.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.remove.Location = new System.Drawing.Point (45, 396);
			this.remove.Name = "remove";
			this.remove.Size = new System.Drawing.Size (24, 24);
			this.remove.TabIndex = 11;
			this.remove.UseVisualStyleBackColor = true;
			this.remove.Click += new System.EventHandler (this.remove_Click);
			// 
			// add
			// 
			this.add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.add.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.add.FlatAppearance.BorderSize = 0;
			this.add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.add.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.add.Location = new System.Drawing.Point (12, 396);
			this.add.Name = "add";
			this.add.Size = new System.Drawing.Size (24, 24);
			this.add.TabIndex = 12;
			this.add.UseVisualStyleBackColor = true;
			this.add.Click += new System.EventHandler (this.add_Click);
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.AutoSize = true;
			this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.closeButton.Font = new System.Drawing.Font ("Calibri", 10F);
			this.closeButton.Location = new System.Drawing.Point (249, 391);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size (71, 29);
			this.closeButton.TabIndex = 13;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler (this.close_Click);
			// 
			// rename_btn
			// 
			this.rename_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.rename_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.rename_btn.FlatAppearance.BorderSize = 0;
			this.rename_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.rename_btn.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.rename_btn.Location = new System.Drawing.Point (78, 396);
			this.rename_btn.Name = "rename_btn";
			this.rename_btn.Size = new System.Drawing.Size (24, 24);
			this.rename_btn.TabIndex = 14;
			this.rename_btn.UseVisualStyleBackColor = false;
			this.rename_btn.Click += new System.EventHandler (this.rename_btn_Click);
			// 
			// regenerate
			// 
			this.regenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.regenerate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.regenerate.FlatAppearance.BorderSize = 0;
			this.regenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.regenerate.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.regenerate.Location = new System.Drawing.Point (111, 396);
			this.regenerate.Name = "regenerate";
			this.regenerate.Size = new System.Drawing.Size (24, 24);
			this.regenerate.TabIndex = 15;
			this.regenerate.UseVisualStyleBackColor = false;
			this.regenerate.Click += new System.EventHandler (this.regenerate_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Maroon;
			this.panel1.Controls.Add (this.mask_1);
			this.panel1.Location = new System.Drawing.Point (152, 382);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size (10, 20);
			this.panel1.TabIndex = 16;
			// 
			// mask_1
			// 
			this.mask_1.BackColor = System.Drawing.SystemColors.Control;
			this.mask_1.Location = new System.Drawing.Point (0, 2);
			this.mask_1.Name = "mask_1";
			this.mask_1.Size = new System.Drawing.Size (8, 16);
			this.mask_1.TabIndex = 20;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.Orchid;
			this.panel2.Controls.Add (this.mask_2);
			this.panel2.Location = new System.Drawing.Point (152, 407);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size (10, 20);
			this.panel2.TabIndex = 17;
			// 
			// mask_2
			// 
			this.mask_2.BackColor = System.Drawing.SystemColors.Control;
			this.mask_2.Controls.Add (this.panel2_2);
			this.mask_2.Location = new System.Drawing.Point (0, 2);
			this.mask_2.Name = "mask_2";
			this.mask_2.Size = new System.Drawing.Size (8, 16);
			this.mask_2.TabIndex = 21;
			// 
			// panel2_2
			// 
			this.panel2_2.BackColor = System.Drawing.Color.Orchid;
			this.panel2_2.Location = new System.Drawing.Point (0, 3);
			this.panel2_2.Name = "panel2_2";
			this.panel2_2.Size = new System.Drawing.Size (5, 10);
			this.panel2_2.TabIndex = 22;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point (167, 382);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (63, 20);
			this.label1.TabIndex = 18;
			this.label1.Text = "Old Theme";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point (168, 407);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size (75, 20);
			this.label2.TabIndex = 19;
			this.label2.Text = "New Theme";
			// 
			// ThemeManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (334, 431);
			this.Controls.Add (this.label2);
			this.Controls.Add (this.label1);
			this.Controls.Add (this.panel2);
			this.Controls.Add (this.panel1);
			this.Controls.Add (this.regenerate);
			this.Controls.Add (this.rename_btn);
			this.Controls.Add (this.closeButton);
			this.Controls.Add (this.add);
			this.Controls.Add (this.remove);
			this.Controls.Add (this.scheme);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ThemeManager";
			this.Text = "Scheme Manager";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler (this.ThemeManager_FormClosing);
			this.Load += new System.EventHandler (this.ThemeManager_Load);
			this.Shown += new System.EventHandler (this.ThemeManager_Shown);
			this.panel1.ResumeLayout (false);
			this.panel2.ResumeLayout (false);
			this.mask_2.ResumeLayout (false);
			this.ResumeLayout (false);
			this.PerformLayout ();
		}

		private System.Windows.Forms.Panel panel2_2;

		private System.Windows.Forms.Panel mask_2;

		private System.Windows.Forms.Panel mask_1;

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.Button regenerate;

		private System.Windows.Forms.Button rename_btn;

		private System.Windows.Forms.ColumnHeader columnHeader1;

		public System.Windows.Forms.ListView scheme;

		private System.Windows.Forms.Button closeButton;

		private System.Windows.Forms.Button remove;

		private System.Windows.Forms.Button add;

		#endregion
	}
}