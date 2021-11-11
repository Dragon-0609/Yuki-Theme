namespace Yuki_Theme.Forms
{
	partial class MForm
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
			this.components = new System.ComponentModel.Container ();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (MForm));
			this.list_1 = new System.Windows.Forms.ListBox ();
			this.colorButton = new System.Windows.Forms.Button ();
			this.bgButton = new System.Windows.Forms.Button ();
			this.label1 = new System.Windows.Forms.Label ();
			this.label2 = new System.Windows.Forms.Label ();
			this.sBox = new FastColoredTextBoxNS.FastColoredTextBox ();
			this.panel1 = new System.Windows.Forms.Panel ();
			this.import = new System.Windows.Forms.Button ();
			this.button6 = new System.Windows.Forms.Button ();
			this.button5 = new System.Windows.Forms.Button ();
			this.button4 = new System.Windows.Forms.Button ();
			this.button3 = new System.Windows.Forms.Button ();
			this.schemes = new System.Windows.Forms.ComboBox ();
			this.check_italic = new System.Windows.Forms.CheckBox ();
			this.check_bold = new System.Windows.Forms.CheckBox ();
			this.button2 = new System.Windows.Forms.Button ();
			this.button1 = new System.Windows.Forms.Button ();
			this.tip = new System.Windows.Forms.ToolTip (this.components);
			((System.ComponentModel.ISupportInitialize) (this.sBox)).BeginInit ();
			this.panel1.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// list_1
			// 
			this.list_1.Dock = System.Windows.Forms.DockStyle.Left;
			this.list_1.Font = new System.Drawing.Font ("Lucida Fax", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.list_1.FormattingEnabled = true;
			this.list_1.ItemHeight = 16;
			this.list_1.Location = new System.Drawing.Point (0, 0);
			this.list_1.Name = "list_1";
			this.list_1.Size = new System.Drawing.Size (170, 461);
			this.list_1.TabIndex = 0;
			this.list_1.Click += new System.EventHandler (this.onSelectItem);
			// 
			// colorButton
			// 
			this.colorButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
			this.colorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.colorButton.Location = new System.Drawing.Point (562, 11);
			this.colorButton.Name = "colorButton";
			this.colorButton.Size = new System.Drawing.Size (35, 35);
			this.colorButton.TabIndex = 1;
			this.colorButton.UseVisualStyleBackColor = true;
			this.colorButton.Click += new System.EventHandler (this.colorButton_Click);
			// 
			// bgButton
			// 
			this.bgButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
			this.bgButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.bgButton.Location = new System.Drawing.Point (562, 56);
			this.bgButton.Name = "bgButton";
			this.bgButton.Size = new System.Drawing.Size (35, 35);
			this.bgButton.TabIndex = 2;
			this.bgButton.UseVisualStyleBackColor = true;
			this.bgButton.Click += new System.EventHandler (this.bgButton_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label1.Location = new System.Drawing.Point (477, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (55, 35);
			this.label1.TabIndex = 3;
			this.label1.Text = "Color";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label2.Location = new System.Drawing.Point (387, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size (145, 35);
			this.label2.TabIndex = 4;
			this.label2.Text = "Background Color";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// sBox
			// 
			this.sBox.AllowDrop = false;
			this.sBox.AllowMacroRecording = false;
			this.sBox.AutoCompleteBracketsList = new char [] {'(', ')', '{', '}', '[', ']', '\"', '\"', '\'', '\''};
			this.sBox.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:]*" + "(?<range>:)\\s*(?<range>[^;]+);";
			this.sBox.AutoScrollMinSize = new System.Drawing.Size (255, 66);
			this.sBox.BackBrush = null;
			this.sBox.BackColor = System.Drawing.Color.Black;
			this.sBox.CharHeight = 22;
			this.sBox.CharWidth = 9;
			this.sBox.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.sBox.DefaultMarkerSize = 8;
			this.sBox.DisabledColor = System.Drawing.Color.FromArgb (((int) (((byte) (100)))), ((int) (((byte) (180)))), ((int) (((byte) (180)))), ((int) (((byte) (180)))));
			this.sBox.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.sBox.FoldingIndicatorColor = System.Drawing.Color.Transparent;
			this.sBox.Font = new System.Drawing.Font ("Consolas", 12F);
			this.sBox.ForeColor = System.Drawing.Color.White;
			this.sBox.HighlightFoldingIndicator = false;
			this.sBox.IndentBackColor = System.Drawing.Color.Transparent;
			this.sBox.IsReplaceMode = false;
			this.sBox.LineInterval = 4;
			this.sBox.Location = new System.Drawing.Point (170, 127);
			this.sBox.Name = "sBox";
			this.sBox.PaddingBackColor = System.Drawing.Color.Maroon;
			this.sBox.Paddings = new System.Windows.Forms.Padding (10, 0, 0, 0);
			this.sBox.PreferredLineWidth = 70;
			this.sBox.ReadOnly = true;
			this.sBox.ReservedCountOfLineNumberChars = 2;
			this.sBox.SelectionColor = System.Drawing.Color.FromArgb (((int) (((byte) (60)))), ((int) (((byte) (0)))), ((int) (((byte) (0)))), ((int) (((byte) (255)))));
			this.sBox.ServiceColors = ((FastColoredTextBoxNS.ServiceColors) (resources.GetObject ("sBox.ServiceColors")));
			this.sBox.ServiceLinesColor = System.Drawing.Color.Red;
			this.sBox.ShowFoldingLines = true;
			this.sBox.Size = new System.Drawing.Size (633, 334);
			this.sBox.TabIndex = 5;
			this.sBox.Text = "begin\r\nWriteln(\'Hello World\');\r\nend.";
			this.sBox.Zoom = 100;
			// 
			// panel1
			// 
			this.panel1.Controls.Add (this.import);
			this.panel1.Controls.Add (this.button6);
			this.panel1.Controls.Add (this.button5);
			this.panel1.Controls.Add (this.button4);
			this.panel1.Controls.Add (this.button3);
			this.panel1.Controls.Add (this.schemes);
			this.panel1.Controls.Add (this.check_italic);
			this.panel1.Controls.Add (this.check_bold);
			this.panel1.Controls.Add (this.button2);
			this.panel1.Controls.Add (this.button1);
			this.panel1.Controls.Add (this.colorButton);
			this.panel1.Controls.Add (this.bgButton);
			this.panel1.Controls.Add (this.label1);
			this.panel1.Controls.Add (this.label2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point (170, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size (633, 124);
			this.panel1.TabIndex = 6;
			// 
			// import
			// 
			this.import.BackgroundImage = global::Yuki_Theme.Properties.Resources.download;
			this.import.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.import.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.import.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.import.Location = new System.Drawing.Point (196, 86);
			this.import.Name = "import";
			this.import.Size = new System.Drawing.Size (36, 36);
			this.import.TabIndex = 14;
			this.import.UseVisualStyleBackColor = true;
			this.import.Click += new System.EventHandler (this.import_Click);
			// 
			// button6
			// 
			this.button6.BackgroundImage = global::Yuki_Theme.Properties.Resources.upload;
			this.button6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.button6.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button6.Location = new System.Drawing.Point (154, 86);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size (36, 36);
			this.button6.TabIndex = 13;
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler (this.button6_Click);
			// 
			// button5
			// 
			this.button5.BackgroundImage = global::Yuki_Theme.Properties.Resources.list_task;
			this.button5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.button5.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button5.Location = new System.Drawing.Point (291, 10);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size (36, 36);
			this.button5.TabIndex = 12;
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler (this.button5_Click);
			// 
			// button4
			// 
			this.button4.BackgroundImage = ((System.Drawing.Image) (resources.GetObject ("button4.BackgroundImage")));
			this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.button4.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button4.Location = new System.Drawing.Point (249, 85);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size (36, 36);
			this.button4.TabIndex = 11;
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler (this.button4_Click);
			// 
			// button3
			// 
			this.button3.BackgroundImage = global::Yuki_Theme.Properties.Resources.plus_square;
			this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.button3.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button3.Location = new System.Drawing.Point (249, 10);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size (36, 36);
			this.button3.TabIndex = 10;
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler (this.button3_Click);
			// 
			// schemes
			// 
			this.schemes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.schemes.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.schemes.FormattingEnabled = true;
			this.schemes.Location = new System.Drawing.Point (15, 10);
			this.schemes.Name = "schemes";
			this.schemes.Size = new System.Drawing.Size (217, 28);
			this.schemes.TabIndex = 9;
			this.schemes.SelectedIndexChanged += new System.EventHandler (this.schemes_SelectedIndexChanged);
			// 
			// check_italic
			// 
			this.check_italic.FlatAppearance.BorderColor = System.Drawing.Color.Black;
			this.check_italic.FlatAppearance.BorderSize = 2;
			this.check_italic.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.check_italic.Location = new System.Drawing.Point (434, 87);
			this.check_italic.Name = "check_italic";
			this.check_italic.Size = new System.Drawing.Size (70, 32);
			this.check_italic.TabIndex = 8;
			this.check_italic.Text = "Italic";
			this.check_italic.UseVisualStyleBackColor = true;
			this.check_italic.CheckedChanged += new System.EventHandler (this.check_italic_CheckedChanged);
			// 
			// check_bold
			// 
			this.check_bold.FlatAppearance.BorderColor = System.Drawing.Color.Black;
			this.check_bold.FlatAppearance.BorderSize = 2;
			this.check_bold.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.check_bold.Location = new System.Drawing.Point (340, 88);
			this.check_bold.Name = "check_bold";
			this.check_bold.Size = new System.Drawing.Size (70, 32);
			this.check_bold.TabIndex = 7;
			this.check_bold.Text = "Bold";
			this.check_bold.UseVisualStyleBackColor = true;
			this.check_bold.CheckedChanged += new System.EventHandler (this.check_bold_CheckedChanged);
			// 
			// button2
			// 
			this.button2.BackgroundImage = global::Yuki_Theme.Properties.Resources.arrow_clockwise;
			this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.button2.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button2.Location = new System.Drawing.Point (68, 85);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size (36, 36);
			this.button2.TabIndex = 6;
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler (this.button2_Click);
			// 
			// button1
			// 
			this.button1.BackgroundImage = global::Yuki_Theme.Properties.Resources.save;
			this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.button1.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button1.Location = new System.Drawing.Point (15, 85);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size (36, 36);
			this.button1.TabIndex = 5;
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler (this.button1_Click);
			// 
			// MForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size (803, 461);
			this.Controls.Add (this.panel1);
			this.Controls.Add (this.sBox);
			this.Controls.Add (this.list_1);
			this.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F);
			this.Icon = ((System.Drawing.Icon) (resources.GetObject ("$this.Icon")));
			this.Location = new System.Drawing.Point (15, 15);
			this.MaximumSize = new System.Drawing.Size (1920, 1080);
			this.MinimumSize = new System.Drawing.Size (600, 400);
			this.Name = "MForm";
			this.Text = "Yuki Theme";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler (this.MForm_FormClosing);
			this.SizeChanged += new System.EventHandler (this.MForm_SizeChanged);
			this.Move += new System.EventHandler (this.MForm_Move);
			this.Resize += new System.EventHandler (this.panel1_Resize);
			((System.ComponentModel.ISupportInitialize) (this.sBox)).EndInit ();
			this.panel1.ResumeLayout (false);
			this.ResumeLayout (false);
		}

		private System.Windows.Forms.Button import;

		private System.Windows.Forms.ToolTip tip;

		private System.Windows.Forms.Button button6;

		private System.Windows.Forms.Button button5;

		private System.Windows.Forms.Button button4;

		private System.Windows.Forms.Button button3;

		public System.Windows.Forms.ComboBox schemes;

		private System.Windows.Forms.CheckBox check_bold;
		private System.Windows.Forms.CheckBox check_italic;

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;

		private System.Windows.Forms.Panel panel1;

		private FastColoredTextBoxNS.FastColoredTextBox sBox;

		private System.Windows.Forms.Button colorButton;

		private System.Windows.Forms.Button bgButton;

		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.ListBox list_1;

		#endregion
	}
}