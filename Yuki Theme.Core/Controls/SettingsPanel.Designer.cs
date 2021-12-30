using System.ComponentModel;

namespace Yuki_Theme.Core.Controls
{
	partial class SettingsPanel
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

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			this.tabs = new Yuki_Theme.Core.Controls.CustomTab ();
			this.tabPage1 = new System.Windows.Forms.TabPage ();
			this.backImage = new System.Windows.Forms.CheckBox ();
			this.checkBox1 = new System.Windows.Forms.CheckBox ();
			this.swsticker = new System.Windows.Forms.CheckBox ();
			this.editor = new System.Windows.Forms.CheckBox ();
			this.button6 = new System.Windows.Forms.Button ();
			this.button5 = new System.Windows.Forms.Button ();
			this.mode = new CustomControls.RJControls.RJComboBox ();
			this.label4 = new System.Windows.Forms.Label ();
			this.button4 = new System.Windows.Forms.Button ();
			this.checkBox2 = new System.Windows.Forms.CheckBox ();
			this.add_program = new System.Windows.Forms.TabPage ();
			this.askC = new System.Windows.Forms.CheckBox ();
			this.label1 = new System.Windows.Forms.Label ();
			this.textBox1 = new Yuki_Theme.Core.Controls.CustomText ();
			this.ActionBox = new CustomControls.RJControls.RJComboBox ();
			this.button1 = new System.Windows.Forms.Button ();
			this.label3 = new System.Windows.Forms.Label ();
			this.add_plugin = new System.Windows.Forms.TabPage ();
			this.logo = new System.Windows.Forms.CheckBox ();
			this.swStatusbar = new System.Windows.Forms.CheckBox ();
			this.tabs.SuspendLayout ();
			this.tabPage1.SuspendLayout ();
			this.add_program.SuspendLayout ();
			this.add_plugin.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// tabs
			// 
			this.tabs.Controls.Add (this.tabPage1);
			this.tabs.Controls.Add (this.add_program);
			this.tabs.Controls.Add (this.add_plugin);
			this.tabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabs.Font = new System.Drawing.Font ("Book Antiqua", 10F);
			this.tabs.Location = new System.Drawing.Point (0, 0);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size (405, 253);
			this.tabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabs.TabIndex = 32;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add (this.backImage);
			this.tabPage1.Controls.Add (this.checkBox1);
			this.tabPage1.Controls.Add (this.swsticker);
			this.tabPage1.Controls.Add (this.editor);
			this.tabPage1.Controls.Add (this.button6);
			this.tabPage1.Controls.Add (this.button5);
			this.tabPage1.Controls.Add (this.mode);
			this.tabPage1.Controls.Add (this.label4);
			this.tabPage1.Controls.Add (this.button4);
			this.tabPage1.Controls.Add (this.checkBox2);
			this.tabPage1.Location = new System.Drawing.Point (4, 25);
			this.tabPage1.Margin = new System.Windows.Forms.Padding (12, 6, 12, 6);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding (12, 6, 12, 6);
			this.tabPage1.Size = new System.Drawing.Size (397, 224);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "General";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// backImage
			// 
			this.backImage.Location = new System.Drawing.Point (15, 9);
			this.backImage.Name = "backImage";
			this.backImage.Size = new System.Drawing.Size (211, 29);
			this.backImage.TabIndex = 25;
			this.backImage.Text = "Show Background Image";
			this.backImage.UseVisualStyleBackColor = true;
			// 
			// checkBox1
			// 
			this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBox1.Location = new System.Drawing.Point (15, 166);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size (127, 29);
			this.checkBox1.TabIndex = 30;
			this.checkBox1.Text = "Beta version";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// swsticker
			// 
			this.swsticker.Location = new System.Drawing.Point (15, 43);
			this.swsticker.Name = "swsticker";
			this.swsticker.Size = new System.Drawing.Size (167, 29);
			this.swsticker.TabIndex = 26;
			this.swsticker.Text = "Show Sticker";
			this.swsticker.UseVisualStyleBackColor = true;
			// 
			// editor
			// 
			this.editor.Location = new System.Drawing.Point (232, 9);
			this.editor.Name = "editor";
			this.editor.Size = new System.Drawing.Size (150, 29);
			this.editor.TabIndex = 29;
			this.editor.Text = "Editor Mode";
			this.editor.UseVisualStyleBackColor = true;
			// 
			// button6
			// 
			this.button6.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button6.AutoSize = true;
			this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button6.Location = new System.Drawing.Point (175, 129);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size (130, 30);
			this.button6.TabIndex = 21;
			this.button6.Text = "Update Manually";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler (this.button6_Click);
			// 
			// button5
			// 
			this.button5.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button5.AutoSize = true;
			this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button5.Location = new System.Drawing.Point (311, 129);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size (71, 30);
			this.button5.TabIndex = 19;
			this.button5.Text = "Check";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler (this.button5_Click);
			// 
			// mode
			// 
			this.mode.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mode.BackColor = System.Drawing.Color.WhiteSmoke;
			this.mode.BorderColor = System.Drawing.Color.MediumSlateBlue;
			this.mode.BorderSize = 1;
			this.mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.mode.Font = new System.Drawing.Font ("Microsoft Sans Serif", 10F);
			this.mode.ForeColor = System.Drawing.Color.DimGray;
			this.mode.IconColor = System.Drawing.Color.MediumSlateBlue;
			this.mode.ListBackColor = System.Drawing.Color.FromArgb (((int) (((byte) (230)))), ((int) (((byte) (228)))), ((int) (((byte) (245)))));
			this.mode.ListTextColor = System.Drawing.Color.DimGray;
			this.mode.Location = new System.Drawing.Point (182, 80);
			this.mode.MinimumSize = new System.Drawing.Size (200, 30);
			this.mode.Name = "mode";
			this.mode.Padding = new System.Windows.Forms.Padding (1);
			this.mode.Size = new System.Drawing.Size (200, 30);
			this.mode.TabIndex = 24;
			this.mode.Texts = "";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label4.Location = new System.Drawing.Point (15, 77);
			this.label4.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size (160, 31);
			this.label4.TabIndex = 15;
			this.label4.Text = "Setting Mode";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// button4
			// 
			this.button4.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button4.AutoSize = true;
			this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button4.Location = new System.Drawing.Point (311, 175);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size (71, 30);
			this.button4.TabIndex = 17;
			this.button4.Text = "About";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler (this.button4_Click);
			// 
			// checkBox2
			// 
			this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBox2.Location = new System.Drawing.Point (15, 131);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size (127, 29);
			this.checkBox2.TabIndex = 18;
			this.checkBox2.Text = "Check updates";
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// add_program
			// 
			this.add_program.Controls.Add (this.askC);
			this.add_program.Controls.Add (this.label1);
			this.add_program.Controls.Add (this.textBox1);
			this.add_program.Controls.Add (this.ActionBox);
			this.add_program.Controls.Add (this.button1);
			this.add_program.Controls.Add (this.label3);
			this.add_program.Location = new System.Drawing.Point (4, 25);
			this.add_program.Margin = new System.Windows.Forms.Padding (12, 6, 12, 6);
			this.add_program.Name = "add_program";
			this.add_program.Padding = new System.Windows.Forms.Padding (12, 6, 12, 6);
			this.add_program.Size = new System.Drawing.Size (397, 224);
			this.add_program.TabIndex = 1;
			this.add_program.Text = "Additional";
			this.add_program.UseVisualStyleBackColor = true;
			// 
			// askC
			// 
			this.askC.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.askC.Location = new System.Drawing.Point (15, 86);
			this.askC.Name = "askC";
			this.askC.Size = new System.Drawing.Size (367, 42);
			this.askC.TabIndex = 12;
			this.askC.Text = "Ask if there are other themes in PascalABC directory";
			this.askC.UseVisualStyleBackColor = true;
			this.askC.CheckedChanged += new System.EventHandler (this.checkBox2_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label1.Location = new System.Drawing.Point (15, 6);
			this.label1.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (199, 26);
			this.label1.TabIndex = 0;
			this.label1.Text = "Path to PascalABC.NET:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBox1
			// 
			this.textBox1.BorderColor = System.Drawing.Color.Blue;
			this.textBox1.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.textBox1.Location = new System.Drawing.Point (16, 36);
			this.textBox1.Margin = new System.Windows.Forms.Padding (4);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size (306, 24);
			this.textBox1.TabIndex = 1;
			// 
			// ActionBox
			// 
			this.ActionBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ActionBox.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ActionBox.BorderColor = System.Drawing.Color.MediumSlateBlue;
			this.ActionBox.BorderSize = 1;
			this.ActionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ActionBox.Font = new System.Drawing.Font ("Lucida Fax", 10F);
			this.ActionBox.ForeColor = System.Drawing.Color.DimGray;
			this.ActionBox.IconColor = System.Drawing.Color.MediumSlateBlue;
			this.ActionBox.ListBackColor = System.Drawing.Color.FromArgb (((int) (((byte) (230)))), ((int) (((byte) (228)))), ((int) (((byte) (245)))));
			this.ActionBox.ListTextColor = System.Drawing.Color.DimGray;
			this.ActionBox.Location = new System.Drawing.Point (182, 142);
			this.ActionBox.MinimumSize = new System.Drawing.Size (200, 30);
			this.ActionBox.Name = "ActionBox";
			this.ActionBox.Padding = new System.Windows.Forms.Padding (1);
			this.ActionBox.Size = new System.Drawing.Size (200, 30);
			this.ActionBox.TabIndex = 23;
			this.ActionBox.Texts = "";
			// 
			// button1
			// 
			this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button1.Location = new System.Drawing.Point (326, 36);
			this.button1.Margin = new System.Windows.Forms.Padding (0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size (25, 25);
			this.button1.TabIndex = 2;
			this.button1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler (this.button1_Click);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label3.Location = new System.Drawing.Point (15, 131);
			this.label3.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size (160, 56);
			this.label3.TabIndex = 14;
			this.label3.Text = "Do action, if there are other themes";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// add_plugin
			// 
			this.add_plugin.BackColor = System.Drawing.Color.Transparent;
			this.add_plugin.Controls.Add (this.logo);
			this.add_plugin.Controls.Add (this.swStatusbar);
			this.add_plugin.Location = new System.Drawing.Point (4, 25);
			this.add_plugin.Name = "add_plugin";
			this.add_plugin.Size = new System.Drawing.Size (397, 224);
			this.add_plugin.TabIndex = 2;
			this.add_plugin.Text = "Additional";
			// 
			// logo
			// 
			this.logo.Location = new System.Drawing.Point (12, 15);
			this.logo.Name = "logo";
			this.logo.Size = new System.Drawing.Size (211, 29);
			this.logo.TabIndex = 28;
			this.logo.Text = "Logo on Start";
			this.logo.UseVisualStyleBackColor = true;
			// 
			// swStatusbar
			// 
			this.swStatusbar.Location = new System.Drawing.Point (12, 50);
			this.swStatusbar.Name = "swStatusbar";
			this.swStatusbar.Size = new System.Drawing.Size (211, 29);
			this.swStatusbar.TabIndex = 27;
			this.swStatusbar.Text = "Name in StatusBar";
			this.swStatusbar.UseVisualStyleBackColor = true;
			// 
			// SettingsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.Controls.Add (this.tabs);
			this.Name = "SettingsPanel";
			this.Size = new System.Drawing.Size (405, 253);
			this.tabs.ResumeLayout (false);
			this.tabPage1.ResumeLayout (false);
			this.tabPage1.PerformLayout ();
			this.add_program.ResumeLayout (false);
			this.add_program.PerformLayout ();
			this.add_plugin.ResumeLayout (false);
			this.ResumeLayout (false);
		}

		public Yuki_Theme.Core.Controls.CustomTab   tabs;
		public System.Windows.Forms.TabPage         tabPage1;
		public System.Windows.Forms.CheckBox        backImage;
		public System.Windows.Forms.CheckBox        checkBox1;
		public System.Windows.Forms.CheckBox        swsticker;
		public System.Windows.Forms.CheckBox        editor;
		public System.Windows.Forms.Button          button6;
		public System.Windows.Forms.Button          button5;
		public CustomControls.RJControls.RJComboBox mode;
		public System.Windows.Forms.Label           label4;
		public System.Windows.Forms.Button          button4;
		public System.Windows.Forms.CheckBox        checkBox2;
		public System.Windows.Forms.TabPage         add_program;
		public System.Windows.Forms.CheckBox        askC;
		public System.Windows.Forms.Label           label1;
		public Yuki_Theme.Core.Controls.CustomText  textBox1;
		public CustomControls.RJControls.RJComboBox ActionBox;
		public System.Windows.Forms.Button          button1;
		public System.Windows.Forms.Label           label3;
		public System.Windows.Forms.TabPage         add_plugin;
		public System.Windows.Forms.CheckBox        logo;
		public System.Windows.Forms.CheckBox        swStatusbar;

		#endregion
	}
}