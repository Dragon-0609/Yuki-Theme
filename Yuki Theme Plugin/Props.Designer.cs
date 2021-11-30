using System.ComponentModel;

namespace Yuki_Theme_Plugin
{
	partial class Props
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
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid ();
			this.listBox1 = new System.Windows.Forms.ListBox ();
			this.SuspendLayout ();
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
			this.propertyGrid1.Location = new System.Drawing.Point (196, 0);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size (315, 385);
			this.propertyGrid1.TabIndex = 0;
			this.propertyGrid1.SelectedObjectsChanged += new System.EventHandler (this.propertyGrid1_SelectedObjectsChanged);
			// 
			// listBox1
			// 
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point (0, 0);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size (196, 385);
			this.listBox1.TabIndex = 1;
			this.listBox1.SelectedIndexChanged += new System.EventHandler (this.listBox1_SelectedIndexChanged);
			// 
			// Props
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (511, 385);
			this.Controls.Add (this.listBox1);
			this.Controls.Add (this.propertyGrid1);
			this.Name = "Props";
			this.Text = "Props";
			this.ResumeLayout (false);
		}

		private System.Windows.Forms.ListBox listBox1;

		public System.Windows.Forms.PropertyGrid propertyGrid1;

		#endregion
	}
}