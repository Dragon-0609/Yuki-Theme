namespace Yuki_Theme.Core.Forms
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
			this.bottomPanel = new System.Windows.Forms.Panel ();
			this.select_btn = new System.Windows.Forms.Button ();
			this.close_btn = new System.Windows.Forms.Button ();
			this.sBox = new FastColoredTextBoxNS.FastColoredTextBox ();
			this.panel1 = new System.Windows.Forms.Panel ();
			this.import_directory = new System.Windows.Forms.Button ();
			this.schemes = new CustomControls.RJControls.RJComboBox ();
			this.imageEditor = new System.Windows.Forms.Panel ();
			this.numericUpDown1 = new Yuki_Theme.Core.Controls.FlatNumericUpDown ();
			this.label3 = new System.Windows.Forms.Label ();
			this.opacity_slider = new Yuki_Theme.Core.Controls.Slider ();
			this.button7 = new System.Windows.Forms.Button ();
			this.button11 = new System.Windows.Forms.Button ();
			this.button10 = new System.Windows.Forms.Button ();
			this.imagePath = new Yuki_Theme.Core.Controls.CustomText ();
			this.pright = new System.Windows.Forms.Button ();
			this.pcenter = new System.Windows.Forms.Button ();
			this.pleft = new System.Windows.Forms.Button ();
			this.colorEditor = new System.Windows.Forms.Panel ();
			this.check_italic = new System.Windows.Forms.CheckBox ();
			this.check_bold = new System.Windows.Forms.CheckBox ();
			this.import_button = new System.Windows.Forms.Button ();
			this.export_button = new System.Windows.Forms.Button ();
			this.manage_button = new System.Windows.Forms.Button ();
			this.settings_button = new System.Windows.Forms.Button ();
			this.add_button = new System.Windows.Forms.Button ();
			this.restore_button = new System.Windows.Forms.Button ();
			this.save_button = new System.Windows.Forms.Button ();
			this.tip = new System.Windows.Forms.ToolTip (this.components);
			this.bottomPanel.SuspendLayout ();
			((System.ComponentModel.ISupportInitialize) (this.sBox)).BeginInit ();
			this.panel1.SuspendLayout ();
			this.imageEditor.SuspendLayout ();
			((System.ComponentModel.ISupportInitialize) (this.numericUpDown1)).BeginInit ();
			this.colorEditor.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// list_1
			// 
			this.list_1.Dock = System.Windows.Forms.DockStyle.Left;
			this.list_1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.list_1.Font = new System.Drawing.Font ("Lucida Fax", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
			this.list_1.FormattingEnabled = true;
			this.list_1.ItemHeight = 16;
			this.list_1.Location = new System.Drawing.Point (0, 0);
			this.list_1.Name = "list_1";
			this.list_1.Size = new System.Drawing.Size (170, 461);
			this.list_1.TabIndex = 0;
			this.list_1.DrawItem += new System.Windows.Forms.DrawItemEventHandler (this.list_1_DrawItem);
			this.list_1.SelectedIndexChanged += new System.EventHandler (this.onSelectItem);
			// 
			// colorButton
			// 
			this.colorButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
			this.colorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.colorButton.Location = new System.Drawing.Point (294, 11);
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
			this.bgButton.Location = new System.Drawing.Point (171, 10);
			this.bgButton.Name = "bgButton";
			this.bgButton.Size = new System.Drawing.Size (35, 35);
			this.bgButton.TabIndex = 2;
			this.bgButton.UseVisualStyleBackColor = true;
			this.bgButton.Click += new System.EventHandler (this.bgButton_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label1.Location = new System.Drawing.Point (233, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (55, 35);
			this.label1.TabIndex = 3;
			this.label1.Text = "Color";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.label2.Location = new System.Drawing.Point (20, 10);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size (145, 35);
			this.label2.TabIndex = 4;
			this.label2.Text = "Background Color";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// bottomPanel
			// 
			this.bottomPanel.Controls.Add (this.select_btn);
			this.bottomPanel.Controls.Add (this.close_btn);
			this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomPanel.Location = new System.Drawing.Point (170, 423);
			this.bottomPanel.Name = "bottomPanel";
			this.bottomPanel.Size = new System.Drawing.Size (520, 38);
			this.bottomPanel.TabIndex = 17;
			// 
			// select_btn
			// 
			this.select_btn.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.select_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.select_btn.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F);
			this.select_btn.Location = new System.Drawing.Point (365, 6);
			this.select_btn.Name = "select_btn";
			this.select_btn.Size = new System.Drawing.Size (67, 23);
			this.select_btn.TabIndex = 10;
			this.select_btn.Text = "Select";
			this.select_btn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.select_btn.UseVisualStyleBackColor = true;
			this.select_btn.Click += new System.EventHandler (this.select_btn_Click);
			// 
			// close_btn
			// 
			this.close_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.close_btn.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F);
			this.close_btn.Location = new System.Drawing.Point (446, 6);
			this.close_btn.Name = "close_btn";
			this.close_btn.Size = new System.Drawing.Size (67, 23);
			this.close_btn.TabIndex = 9;
			this.close_btn.Text = "Close";
			this.close_btn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.close_btn.UseVisualStyleBackColor = true;
			this.close_btn.Click += new System.EventHandler (this.close_btn_Click);
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
			this.sBox.Location = new System.Drawing.Point (170, 145);
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
			this.sBox.Size = new System.Drawing.Size (520, 278);
			this.sBox.TabIndex = 5;
			this.sBox.Text = "begin\r\nWriteln(\'Hello World\');\r\nend.";
			this.sBox.Zoom = 100;
			// 
			// panel1
			// 
			this.panel1.Controls.Add (this.import_directory);
			this.panel1.Controls.Add (this.schemes);
			this.panel1.Controls.Add (this.imageEditor);
			this.panel1.Controls.Add (this.colorEditor);
			this.panel1.Controls.Add (this.import_button);
			this.panel1.Controls.Add (this.export_button);
			this.panel1.Controls.Add (this.manage_button);
			this.panel1.Controls.Add (this.settings_button);
			this.panel1.Controls.Add (this.add_button);
			this.panel1.Controls.Add (this.restore_button);
			this.panel1.Controls.Add (this.save_button);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point (170, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size (520, 142);
			this.panel1.TabIndex = 6;
			// 
			// import_directory
			// 
			this.import_directory.BackColor = System.Drawing.Color.Chocolate;
			this.import_directory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.import_directory.FlatAppearance.BorderSize = 0;
			this.import_directory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.import_directory.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.import_directory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.import_directory.Location = new System.Drawing.Point (453, 10);
			this.import_directory.Name = "import_directory";
			this.import_directory.Size = new System.Drawing.Size (28, 28);
			this.import_directory.TabIndex = 17;
			this.import_directory.UseVisualStyleBackColor = false;
			this.import_directory.Click += new System.EventHandler (this.import_directory_Click);
			// 
			// schemes
			// 
			this.schemes.BackColor = System.Drawing.Color.WhiteSmoke;
			this.schemes.BorderColor = System.Drawing.Color.Red;
			this.schemes.BorderSize = 1;
			this.schemes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.schemes.Font = new System.Drawing.Font ("Lucida Fax", 10F);
			this.schemes.ForeColor = System.Drawing.Color.DimGray;
			this.schemes.IconColor = System.Drawing.Color.Red;
			this.schemes.ListBackColor = System.Drawing.Color.FromArgb (((int) (((byte) (230)))), ((int) (((byte) (228)))), ((int) (((byte) (245)))));
			this.schemes.ListTextColor = System.Drawing.Color.DimGray;
			this.schemes.Location = new System.Drawing.Point (16, 12);
			this.schemes.MinimumSize = new System.Drawing.Size (200, 30);
			this.schemes.Name = "schemes";
			this.schemes.Padding = new System.Windows.Forms.Padding (1);
			this.schemes.Size = new System.Drawing.Size (214, 30);
			this.schemes.TabIndex = 1;
			this.schemes.Texts = "";
			this.schemes.OnSelectedIndexChanged += new System.EventHandler (this.schemes_SelectedIndexChanged);
			// 
			// imageEditor
			// 
			this.imageEditor.Controls.Add (this.numericUpDown1);
			this.imageEditor.Controls.Add (this.label3);
			this.imageEditor.Controls.Add (this.opacity_slider);
			this.imageEditor.Controls.Add (this.button7);
			this.imageEditor.Controls.Add (this.button11);
			this.imageEditor.Controls.Add (this.button10);
			this.imageEditor.Controls.Add (this.imagePath);
			this.imageEditor.Controls.Add (this.pright);
			this.imageEditor.Controls.Add (this.pcenter);
			this.imageEditor.Controls.Add (this.pleft);
			this.imageEditor.Location = new System.Drawing.Point (16, 66);
			this.imageEditor.Name = "imageEditor";
			this.imageEditor.Size = new System.Drawing.Size (497, 73);
			this.imageEditor.TabIndex = 16;
			this.imageEditor.Visible = false;
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.BackColor = System.Drawing.Color.Black;
			this.numericUpDown1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.numericUpDown1.ForeColor = System.Drawing.Color.Silver;
			this.numericUpDown1.Location = new System.Drawing.Point (225, 48);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size (43, 20);
			this.numericUpDown1.TabIndex = 9;
			this.numericUpDown1.ValueChanged += new System.EventHandler (this.numericUpDown1_ValueChanged);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font ("Microsoft Sans Serif", 10F);
			this.label3.Location = new System.Drawing.Point (159, 46);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size (68, 21);
			this.label3.TabIndex = 8;
			this.label3.Text = "Opacity:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// opacity_slider
			// 
			this.opacity_slider.BackColor = System.Drawing.Color.FromArgb (((int) (((byte) (70)))), ((int) (((byte) (77)))), ((int) (((byte) (95)))));
			this.opacity_slider.BarPenColorBottom = System.Drawing.Color.Black;
			this.opacity_slider.BarPenColorTop = System.Drawing.Color.FromArgb (((int) (((byte) (55)))), ((int) (((byte) (60)))), ((int) (((byte) (74)))));
			this.opacity_slider.BorderRoundRectSize = new System.Drawing.Size (18, 18);
			this.opacity_slider.ElapsedInnerColor = System.Drawing.Color.Gray;
			this.opacity_slider.ElapsedPenColorBottom = System.Drawing.Color.Silver;
			this.opacity_slider.ElapsedPenColorTop = System.Drawing.Color.Gray;
			this.opacity_slider.Font = new System.Drawing.Font ("Microsoft Sans Serif", 6F);
			this.opacity_slider.ForeColor = System.Drawing.Color.White;
			this.opacity_slider.LargeChange = new decimal (new int [] {5, 0, 0, 0});
			this.opacity_slider.Location = new System.Drawing.Point (275, 45);
			this.opacity_slider.Maximum = new decimal (new int [] {100, 0, 0, 0});
			this.opacity_slider.Minimum = new decimal (new int [] {0, 0, 0, 0});
			this.opacity_slider.MouseWheelBarPartitions = 20;
			this.opacity_slider.Name = "opacity_slider";
			this.opacity_slider.Padding = 10;
			this.opacity_slider.ScaleDivisions = new decimal (new int [] {20, 0, 0, 0});
			this.opacity_slider.ScaleSubDivisions = new decimal (new int [] {5, 0, 0, 0});
			this.opacity_slider.ShowDivisionsText = false;
			this.opacity_slider.ShowSmallScale = false;
			this.opacity_slider.Size = new System.Drawing.Size (217, 23);
			this.opacity_slider.SmallChange = new decimal (new int [] {1, 0, 0, 0});
			this.opacity_slider.TabIndex = 7;
			this.opacity_slider.Text = "slider1";
			this.opacity_slider.ThumbInnerColor = System.Drawing.Color.Red;
			this.opacity_slider.ThumbPenColor = System.Drawing.Color.Red;
			this.opacity_slider.ThumbRoundRectSize = new System.Drawing.Size (2, 10);
			this.opacity_slider.ThumbSize = new System.Drawing.Size (8, 16);
			this.opacity_slider.TickAdd = 0F;
			this.opacity_slider.TickColor = System.Drawing.Color.White;
			this.opacity_slider.TickDivide = 0F;
			this.opacity_slider.TickStyle = System.Windows.Forms.TickStyle.None;
			this.opacity_slider.Value = new decimal (new int [] {10, 0, 0, 0});
			this.opacity_slider.Scroll += new System.Windows.Forms.ScrollEventHandler (this.opacity_slider_Scroll);
			// 
			// button7
			// 
			this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button7.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F);
			this.button7.Location = new System.Drawing.Point (89, 45);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size (67, 23);
			this.button7.TabIndex = 6;
			this.button7.Text = "Clear";
			this.button7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += new System.EventHandler (this.button7_Click_1);
			// 
			// button11
			// 
			this.button11.BackColor = System.Drawing.Color.Chocolate;
			this.button11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.button11.FlatAppearance.BorderSize = 0;
			this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button11.Font = new System.Drawing.Font ("Book Antiqua", 12F);
			this.button11.Location = new System.Drawing.Point (237, 12);
			this.button11.Name = "button11";
			this.button11.Size = new System.Drawing.Size (23, 23);
			this.button11.TabIndex = 5;
			this.button11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.button11.UseVisualStyleBackColor = false;
			this.button11.Click += new System.EventHandler (this.button11_Click);
			// 
			// button10
			// 
			this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button10.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F);
			this.button10.Location = new System.Drawing.Point (16, 45);
			this.button10.Name = "button10";
			this.button10.Size = new System.Drawing.Size (67, 23);
			this.button10.TabIndex = 4;
			this.button10.Text = "Apply";
			this.button10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.button10.UseVisualStyleBackColor = true;
			this.button10.Click += new System.EventHandler (this.button10_Click);
			// 
			// imagePath
			// 
			this.imagePath.BorderColor = System.Drawing.Color.Blue;
			this.imagePath.Font = new System.Drawing.Font ("Book Antiqua", 10F);
			this.imagePath.Location = new System.Drawing.Point (16, 10);
			this.imagePath.Name = "imagePath";
			this.imagePath.Size = new System.Drawing.Size (215, 24);
			this.imagePath.TabIndex = 3;
			// 
			// pright
			// 
			this.pright.BackgroundImage = global::Yuki_Theme.Core.Properties.Resources.right;
			this.pright.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.pright.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.pright.Location = new System.Drawing.Point (438, 7);
			this.pright.Name = "pright";
			this.pright.Size = new System.Drawing.Size (54, 32);
			this.pright.TabIndex = 2;
			this.pright.UseVisualStyleBackColor = true;
			this.pright.Click += new System.EventHandler (this.pright_Click);
			// 
			// pcenter
			// 
			this.pcenter.BackgroundImage = global::Yuki_Theme.Core.Properties.Resources.center;
			this.pcenter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.pcenter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.pcenter.Location = new System.Drawing.Point (378, 7);
			this.pcenter.Name = "pcenter";
			this.pcenter.Size = new System.Drawing.Size (54, 32);
			this.pcenter.TabIndex = 1;
			this.pcenter.UseVisualStyleBackColor = true;
			this.pcenter.Click += new System.EventHandler (this.pcenter_Click);
			// 
			// pleft
			// 
			this.pleft.BackgroundImage = global::Yuki_Theme.Core.Properties.Resources.left;
			this.pleft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.pleft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.pleft.Location = new System.Drawing.Point (318, 7);
			this.pleft.Name = "pleft";
			this.pleft.Size = new System.Drawing.Size (54, 32);
			this.pleft.TabIndex = 0;
			this.pleft.UseVisualStyleBackColor = true;
			this.pleft.Click += new System.EventHandler (this.pleft_Click);
			// 
			// colorEditor
			// 
			this.colorEditor.Controls.Add (this.check_italic);
			this.colorEditor.Controls.Add (this.check_bold);
			this.colorEditor.Controls.Add (this.colorButton);
			this.colorEditor.Controls.Add (this.bgButton);
			this.colorEditor.Controls.Add (this.label1);
			this.colorEditor.Controls.Add (this.label2);
			this.colorEditor.Location = new System.Drawing.Point (16, 66);
			this.colorEditor.Name = "colorEditor";
			this.colorEditor.Size = new System.Drawing.Size (497, 57);
			this.colorEditor.TabIndex = 15;
			this.colorEditor.Visible = false;
			// 
			// check_italic
			// 
			this.check_italic.FlatAppearance.BorderColor = System.Drawing.Color.Black;
			this.check_italic.FlatAppearance.BorderSize = 2;
			this.check_italic.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.check_italic.Location = new System.Drawing.Point (422, 11);
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
			this.check_bold.Location = new System.Drawing.Point (346, 12);
			this.check_bold.Name = "check_bold";
			this.check_bold.Size = new System.Drawing.Size (70, 32);
			this.check_bold.TabIndex = 7;
			this.check_bold.Text = "Bold";
			this.check_bold.UseVisualStyleBackColor = true;
			this.check_bold.CheckedChanged += new System.EventHandler (this.check_bold_CheckedChanged);
			// 
			// import_button
			// 
			this.import_button.BackColor = System.Drawing.Color.Chocolate;
			this.import_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.import_button.FlatAppearance.BorderSize = 0;
			this.import_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.import_button.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.import_button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.import_button.Location = new System.Drawing.Point (419, 10);
			this.import_button.Name = "import_button";
			this.import_button.Size = new System.Drawing.Size (28, 28);
			this.import_button.TabIndex = 14;
			this.import_button.UseVisualStyleBackColor = false;
			this.import_button.Click += new System.EventHandler (this.import_Click);
			// 
			// export_button
			// 
			this.export_button.BackColor = System.Drawing.Color.Chocolate;
			this.export_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.export_button.FlatAppearance.BorderSize = 0;
			this.export_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.export_button.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.export_button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.export_button.Location = new System.Drawing.Point (385, 10);
			this.export_button.Name = "export_button";
			this.export_button.Size = new System.Drawing.Size (28, 28);
			this.export_button.TabIndex = 13;
			this.export_button.UseVisualStyleBackColor = false;
			this.export_button.Click += new System.EventHandler (this.export);
			// 
			// manage_button
			// 
			this.manage_button.BackColor = System.Drawing.Color.Chocolate;
			this.manage_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.manage_button.FlatAppearance.BorderSize = 0;
			this.manage_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.manage_button.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.manage_button.Location = new System.Drawing.Point (283, 10);
			this.manage_button.Name = "manage_button";
			this.manage_button.Size = new System.Drawing.Size (28, 28);
			this.manage_button.TabIndex = 12;
			this.manage_button.UseVisualStyleBackColor = false;
			this.manage_button.Click += new System.EventHandler (this.button5_Click);
			// 
			// settings_button
			// 
			this.settings_button.BackColor = System.Drawing.Color.Chocolate;
			this.settings_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.settings_button.FlatAppearance.BorderSize = 0;
			this.settings_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.settings_button.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.settings_button.Location = new System.Drawing.Point (485, 10);
			this.settings_button.Name = "settings_button";
			this.settings_button.Size = new System.Drawing.Size (28, 28);
			this.settings_button.TabIndex = 11;
			this.settings_button.UseVisualStyleBackColor = false;
			this.settings_button.Click += new System.EventHandler (this.button4_Click);
			// 
			// add_button
			// 
			this.add_button.BackColor = System.Drawing.Color.Chocolate;
			this.add_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.add_button.FlatAppearance.BorderSize = 0;
			this.add_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.add_button.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.add_button.Location = new System.Drawing.Point (249, 10);
			this.add_button.Name = "add_button";
			this.add_button.Size = new System.Drawing.Size (28, 28);
			this.add_button.TabIndex = 10;
			this.add_button.UseVisualStyleBackColor = false;
			this.add_button.Click += new System.EventHandler (this.button3_Click);
			// 
			// restore_button
			// 
			this.restore_button.BackColor = System.Drawing.Color.Chocolate;
			this.restore_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.restore_button.FlatAppearance.BorderSize = 0;
			this.restore_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.restore_button.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.restore_button.Location = new System.Drawing.Point (351, 10);
			this.restore_button.Name = "restore_button";
			this.restore_button.Size = new System.Drawing.Size (28, 28);
			this.restore_button.TabIndex = 6;
			this.restore_button.UseVisualStyleBackColor = false;
			this.restore_button.Click += new System.EventHandler (this.restore_Click);
			// 
			// save_button
			// 
			this.save_button.BackColor = System.Drawing.Color.Chocolate;
			this.save_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.save_button.FlatAppearance.BorderSize = 0;
			this.save_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.save_button.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
			this.save_button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.save_button.Location = new System.Drawing.Point (317, 10);
			this.save_button.Name = "save_button";
			this.save_button.Size = new System.Drawing.Size (28, 28);
			this.save_button.TabIndex = 5;
			this.save_button.UseVisualStyleBackColor = false;
			this.save_button.Click += new System.EventHandler (this.save_Click);
			// 
			// MForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size (690, 461);
			this.Controls.Add (this.panel1);
			this.Controls.Add (this.sBox);
			this.Controls.Add (this.bottomPanel);
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
			this.bottomPanel.ResumeLayout (false);
			((System.ComponentModel.ISupportInitialize) (this.sBox)).EndInit ();
			this.panel1.ResumeLayout (false);
			this.imageEditor.ResumeLayout (false);
			this.imageEditor.PerformLayout ();
			((System.ComponentModel.ISupportInitialize) (this.numericUpDown1)).EndInit ();
			this.colorEditor.ResumeLayout (false);
			this.ResumeLayout (false);
		}

		private Yuki_Theme.Core.Controls.FlatNumericUpDown numericUpDown1;

		private System.Windows.Forms.Button import_directory;

		private System.Windows.Forms.Button close_btn;
		private System.Windows.Forms.Button select_btn;

		private System.Windows.Forms.Panel bottomPanel;

		private System.Windows.Forms.Label label3;

		private Yuki_Theme.Core.Controls.Slider opacity_slider;

		private System.Windows.Forms.Button import_button;
		private System.Windows.Forms.Button export_button;

		private System.Windows.Forms.Button button7;

		private Yuki_Theme.Core.Controls.CustomText imagePath;

		private System.Windows.Forms.Button pright;
		private System.Windows.Forms.Button pcenter;
		private System.Windows.Forms.Button pleft;
		
		private System.Windows.Forms.Button button10;
		private System.Windows.Forms.Button button11;

		private System.Windows.Forms.Panel imageEditor;

		private System.Windows.Forms.Panel colorEditor;

		private System.Windows.Forms.ToolTip tip;

		private System.Windows.Forms.Button manage_button;

		private System.Windows.Forms.Button settings_button;

		private System.Windows.Forms.Button add_button;

		public CustomControls.RJControls.RJComboBox schemes;

		private System.Windows.Forms.CheckBox check_bold;
		private System.Windows.Forms.CheckBox check_italic;

		private System.Windows.Forms.Button save_button;
		private System.Windows.Forms.Button restore_button;

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