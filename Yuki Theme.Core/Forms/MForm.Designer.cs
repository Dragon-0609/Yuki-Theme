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
			this.colorLabel = new System.Windows.Forms.Label ();
			this.backgroundColorLabel = new System.Windows.Forms.Label ();
			this.bottomPanel = new System.Windows.Forms.Panel ();
			this.select_btn = new System.Windows.Forms.Button ();
			this.close_btn = new System.Windows.Forms.Button ();
			this.sBox = new FastColoredTextBoxNS.FastColoredTextBox ();
			this.panel1 = new System.Windows.Forms.Panel ();
			this.downloader = new System.Windows.Forms.Button ();
			this.editorp2 = new System.Windows.Forms.Panel ();
			this.imageEditor = new System.Windows.Forms.Panel ();
			this.alignpanel = new System.Windows.Forms.Panel ();
			this.pleft = new System.Windows.Forms.Button ();
			this.pcenter = new System.Windows.Forms.Button ();
			this.pright = new System.Windows.Forms.Button ();
			this.opacitySlider = new Yuki_Theme.Core.Controls.FlatNumericUpDown ();
			this.opacityLabel = new System.Windows.Forms.Label ();
			this.opacity_slider = new Yuki_Theme.Core.Controls.Slider ();
			this.clearButton = new System.Windows.Forms.Button ();
			this.selectImageButton = new System.Windows.Forms.Button ();
			this.applyButton = new System.Windows.Forms.Button ();
			this.imagePath = new Yuki_Theme.Core.Controls.CustomText ();
			this.colorEditor = new System.Windows.Forms.Panel ();
			this.check_italic = new System.Windows.Forms.CheckBox ();
			this.check_bold = new System.Windows.Forms.CheckBox ();
			this.editorpanel = new System.Windows.Forms.Panel ();
			this.add_button = new System.Windows.Forms.Button ();
			this.manage_button = new System.Windows.Forms.Button ();
			this.save_button = new System.Windows.Forms.Button ();
			this.restore_button = new System.Windows.Forms.Button ();
			this.import_directory = new System.Windows.Forms.Button ();
			this.schemes = new CustomControls.RJControls.RJComboBox ();
			this.import_button = new System.Windows.Forms.Button ();
			this.export_button = new System.Windows.Forms.Button ();
			this.settings_button = new System.Windows.Forms.Button ();
			this.tip = new System.Windows.Forms.ToolTip (this.components);
			this.bottomPanel.SuspendLayout ();
			((System.ComponentModel.ISupportInitialize)(this.sBox)).BeginInit ();
			this.panel1.SuspendLayout ();
			this.editorp2.SuspendLayout ();
			this.imageEditor.SuspendLayout ();
			this.alignpanel.SuspendLayout ();
			((System.ComponentModel.ISupportInitialize)(this.opacitySlider)).BeginInit ();
			this.colorEditor.SuspendLayout ();
			this.editorpanel.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// list_1
			// 
			this.list_1.Dock = System.Windows.Forms.DockStyle.Left;
			this.list_1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.list_1.Font = new System.Drawing.Font ("Lucida Fax", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
			// colorLabel
			// 
			this.colorLabel.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.colorLabel.Location = new System.Drawing.Point (233, 10);
			this.colorLabel.Name = "colorLabel";
			this.colorLabel.Size = new System.Drawing.Size (55, 35);
			this.colorLabel.TabIndex = 3;
			this.colorLabel.Text = "Color";
			this.colorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// backgroundColorLabel
			// 
			this.backgroundColorLabel.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.backgroundColorLabel.Location = new System.Drawing.Point (20, 10);
			this.backgroundColorLabel.Name = "backgroundColorLabel";
			this.backgroundColorLabel.Size = new System.Drawing.Size (145, 35);
			this.backgroundColorLabel.TabIndex = 4;
			this.backgroundColorLabel.Text = "Background Color";
			this.backgroundColorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// bottomPanel
			// 
			this.bottomPanel.Controls.Add (this.select_btn);
			this.bottomPanel.Controls.Add (this.close_btn);
			this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomPanel.Location = new System.Drawing.Point (170, 423);
			this.bottomPanel.Name = "bottomPanel";
			this.bottomPanel.Size = new System.Drawing.Size (547, 38);
			this.bottomPanel.TabIndex = 17;
			// 
			// select_btn
			// 
			this.select_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.select_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.select_btn.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F);
			this.select_btn.Location = new System.Drawing.Point (392, 6);
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
			this.close_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.close_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.close_btn.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F);
			this.close_btn.Location = new System.Drawing.Point (473, 6);
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
			this.sBox.AutoCompleteBracketsList = new char [] { '(', ')', '{', '}', '[', ']', '\"', '\"', '\'', '\'' };
			this.sBox.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:]*" + "(?<range>:)\\s*(?<range>[^;]+);";
			this.sBox.AutoScrollMinSize = new System.Drawing.Size (255, 66);
			this.sBox.BackBrush = null;
			this.sBox.BackColor = System.Drawing.Color.Black;
			this.sBox.CharHeight = 22;
			this.sBox.CharWidth = 9;
			this.sBox.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.sBox.DefaultMarkerSize = 8;
			this.sBox.DisabledColor = System.Drawing.Color.FromArgb (((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
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
			this.sBox.SelectionColor = System.Drawing.Color.FromArgb (((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.sBox.SelectionHighlightingForLineBreaksEnabled = false;
			this.sBox.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject ("sBox.ServiceColors")));
			this.sBox.ServiceLinesColor = System.Drawing.Color.Red;
			this.sBox.ShowFoldingLines = true;
			this.sBox.Size = new System.Drawing.Size (547, 278);
			this.sBox.TabIndex = 5;
			this.sBox.Text = "begin\r\nWriteln(\'Hello World\');\r\nend.";
			this.sBox.Zoom = 100;
			// 
			// panel1
			// 
			this.panel1.Controls.Add (this.downloader);
			this.panel1.Controls.Add (this.editorp2);
			this.panel1.Controls.Add (this.editorpanel);
			this.panel1.Controls.Add (this.import_directory);
			this.panel1.Controls.Add (this.schemes);
			this.panel1.Controls.Add (this.import_button);
			this.panel1.Controls.Add (this.export_button);
			this.panel1.Controls.Add (this.settings_button);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point (170, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size (547, 142);
			this.panel1.TabIndex = 6;
			// 
			// downloader
			// 
			this.downloader.BackColor = System.Drawing.Color.Chocolate;
			this.downloader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.downloader.FlatAppearance.BorderSize = 0;
			this.downloader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.downloader.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.downloader.Location = new System.Drawing.Point (515, 10);
			this.downloader.Name = "downloader";
			this.downloader.Size = new System.Drawing.Size (20, 20);
			this.downloader.TabIndex = 20;
			this.downloader.UseVisualStyleBackColor = false;
			this.downloader.Click += new System.EventHandler (this.downloader_Click);
			// 
			// editorp2
			// 
			this.editorp2.Controls.Add (this.imageEditor);
			this.editorp2.Controls.Add (this.colorEditor);
			this.editorp2.Location = new System.Drawing.Point (13, 63);
			this.editorp2.Name = "editorp2";
			this.editorp2.Size = new System.Drawing.Size (501, 76);
			this.editorp2.TabIndex = 19;
			// 
			// imageEditor
			// 
			this.imageEditor.Controls.Add (this.alignpanel);
			this.imageEditor.Controls.Add (this.opacitySlider);
			this.imageEditor.Controls.Add (this.opacityLabel);
			this.imageEditor.Controls.Add (this.opacity_slider);
			this.imageEditor.Controls.Add (this.clearButton);
			this.imageEditor.Controls.Add (this.selectImageButton);
			this.imageEditor.Controls.Add (this.applyButton);
			this.imageEditor.Controls.Add (this.imagePath);
			this.imageEditor.Location = new System.Drawing.Point (3, 3);
			this.imageEditor.Name = "imageEditor";
			this.imageEditor.Size = new System.Drawing.Size (497, 73);
			this.imageEditor.TabIndex = 16;
			this.imageEditor.Visible = false;
			// 
			// alignpanel
			// 
			this.alignpanel.Controls.Add (this.pleft);
			this.alignpanel.Controls.Add (this.pcenter);
			this.alignpanel.Controls.Add (this.pright);
			this.alignpanel.Location = new System.Drawing.Point (294, 3);
			this.alignpanel.Name = "alignpanel";
			this.alignpanel.Size = new System.Drawing.Size (198, 36);
			this.alignpanel.TabIndex = 10;
			// 
			// pleft
			// 
			this.pleft.BackgroundImage = global::Yuki_Theme.Core.Properties.Resources.left;
			this.pleft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.pleft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.pleft.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.pleft.Location = new System.Drawing.Point (11, 1);
			this.pleft.Name = "pleft";
			this.pleft.Size = new System.Drawing.Size (54, 32);
			this.pleft.TabIndex = 0;
			this.pleft.UseVisualStyleBackColor = true;
			this.pleft.Click += new System.EventHandler (this.pleft_Click);
			// 
			// pcenter
			// 
			this.pcenter.BackgroundImage = global::Yuki_Theme.Core.Properties.Resources.center;
			this.pcenter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.pcenter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.pcenter.Location = new System.Drawing.Point (74, 0);
			this.pcenter.Name = "pcenter";
			this.pcenter.Size = new System.Drawing.Size (54, 32);
			this.pcenter.TabIndex = 1;
			this.pcenter.UseVisualStyleBackColor = true;
			this.pcenter.Click += new System.EventHandler (this.pcenter_Click);
			// 
			// pright
			// 
			this.pright.BackgroundImage = global::Yuki_Theme.Core.Properties.Resources.right;
			this.pright.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.pright.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.pright.Location = new System.Drawing.Point (136, 1);
			this.pright.Name = "pright";
			this.pright.Size = new System.Drawing.Size (54, 32);
			this.pright.TabIndex = 2;
			this.pright.UseVisualStyleBackColor = true;
			this.pright.Click += new System.EventHandler (this.pright_Click);
			// 
			// opacitySlider
			// 
			this.opacitySlider.BackColor = System.Drawing.Color.Black;
			this.opacitySlider.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.opacitySlider.ForeColor = System.Drawing.Color.Silver;
			this.opacitySlider.Location = new System.Drawing.Point (225, 48);
			this.opacitySlider.Name = "opacitySlider";
			this.opacitySlider.Size = new System.Drawing.Size (43, 20);
			this.opacitySlider.TabIndex = 9;
			this.opacitySlider.ValueChanged += new System.EventHandler (this.opacitySlider_ValueChanged);
			// 
			// opacityLabel
			// 
			this.opacityLabel.Font = new System.Drawing.Font ("Microsoft Sans Serif", 10F);
			this.opacityLabel.Location = new System.Drawing.Point (159, 46);
			this.opacityLabel.Name = "opacityLabel";
			this.opacityLabel.Size = new System.Drawing.Size (68, 21);
			this.opacityLabel.TabIndex = 8;
			this.opacityLabel.Text = "Opacity:";
			this.opacityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// opacity_slider
			// 
			this.opacity_slider.BackColor = System.Drawing.Color.FromArgb (((int)(((byte)(70)))), ((int)(((byte)(77)))), ((int)(((byte)(95)))));
			this.opacity_slider.BarPenColorBottom = System.Drawing.Color.Black;
			this.opacity_slider.BarPenColorTop = System.Drawing.Color.FromArgb (((int)(((byte)(55)))), ((int)(((byte)(60)))), ((int)(((byte)(74)))));
			this.opacity_slider.BorderRoundRectSize = new System.Drawing.Size (18, 18);
			this.opacity_slider.ElapsedInnerColor = System.Drawing.Color.Gray;
			this.opacity_slider.ElapsedPenColorBottom = System.Drawing.Color.Silver;
			this.opacity_slider.ElapsedPenColorTop = System.Drawing.Color.Gray;
			this.opacity_slider.Font = new System.Drawing.Font ("Microsoft Sans Serif", 6F);
			this.opacity_slider.ForeColor = System.Drawing.Color.White;
			this.opacity_slider.LargeChange = new decimal (new int [] { 5, 0, 0, 0 });
			this.opacity_slider.Location = new System.Drawing.Point (275, 45);
			this.opacity_slider.Maximum = new decimal (new int [] { 100, 0, 0, 0 });
			this.opacity_slider.Minimum = new decimal (new int [] { 0, 0, 0, 0 });
			this.opacity_slider.MouseWheelBarPartitions = 20;
			this.opacity_slider.Name = "opacity_slider";
			this.opacity_slider.Padding = 10;
			this.opacity_slider.ScaleDivisions = new decimal (new int [] { 20, 0, 0, 0 });
			this.opacity_slider.ScaleSubDivisions = new decimal (new int [] { 5, 0, 0, 0 });
			this.opacity_slider.ShowDivisionsText = false;
			this.opacity_slider.ShowSmallScale = false;
			this.opacity_slider.Size = new System.Drawing.Size (217, 23);
			this.opacity_slider.SmallChange = new decimal (new int [] { 1, 0, 0, 0 });
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
			this.opacity_slider.Value = new decimal (new int [] { 10, 0, 0, 0 });
			this.opacity_slider.Scroll += new System.Windows.Forms.ScrollEventHandler (this.opacity_slider_Scroll);
			// 
			// clearButton
			// 
			this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.clearButton.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F);
			this.clearButton.Location = new System.Drawing.Point (89, 45);
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size (67, 23);
			this.clearButton.TabIndex = 6;
			this.clearButton.Text = "Clear";
			this.clearButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.clearButton.UseVisualStyleBackColor = true;
			this.clearButton.Click += new System.EventHandler (this.Clear_Click);
			// 
			// selectImageButton
			// 
			this.selectImageButton.BackColor = System.Drawing.Color.Chocolate;
			this.selectImageButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.selectImageButton.FlatAppearance.BorderSize = 0;
			this.selectImageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.selectImageButton.Font = new System.Drawing.Font ("Book Antiqua", 12F);
			this.selectImageButton.Location = new System.Drawing.Point (237, 12);
			this.selectImageButton.Name = "selectImageButton";
			this.selectImageButton.Size = new System.Drawing.Size (23, 23);
			this.selectImageButton.TabIndex = 5;
			this.selectImageButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.selectImageButton.UseVisualStyleBackColor = false;
			this.selectImageButton.Click += new System.EventHandler (this.selectImage_Click);
			// 
			// applyButton
			// 
			this.applyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.applyButton.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F);
			this.applyButton.Location = new System.Drawing.Point (16, 45);
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size (67, 23);
			this.applyButton.TabIndex = 4;
			this.applyButton.Text = "Apply";
			this.applyButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.applyButton.UseVisualStyleBackColor = true;
			this.applyButton.Click += new System.EventHandler (this.Apply_Click);
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
			// colorEditor
			// 
			this.colorEditor.Controls.Add (this.check_italic);
			this.colorEditor.Controls.Add (this.check_bold);
			this.colorEditor.Controls.Add (this.colorButton);
			this.colorEditor.Controls.Add (this.bgButton);
			this.colorEditor.Controls.Add (this.colorLabel);
			this.colorEditor.Controls.Add (this.backgroundColorLabel);
			this.colorEditor.Location = new System.Drawing.Point (3, 3);
			this.colorEditor.Name = "colorEditor";
			this.colorEditor.Size = new System.Drawing.Size (497, 57);
			this.colorEditor.TabIndex = 15;
			this.colorEditor.Visible = false;
			// 
			// check_italic
			// 
			this.check_italic.FlatAppearance.BorderColor = System.Drawing.Color.Black;
			this.check_italic.FlatAppearance.BorderSize = 2;
			this.check_italic.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
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
			this.check_bold.Font = new System.Drawing.Font ("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.check_bold.Location = new System.Drawing.Point (346, 12);
			this.check_bold.Name = "check_bold";
			this.check_bold.Size = new System.Drawing.Size (70, 32);
			this.check_bold.TabIndex = 7;
			this.check_bold.Text = "Bold";
			this.check_bold.UseVisualStyleBackColor = true;
			this.check_bold.CheckedChanged += new System.EventHandler (this.check_bold_CheckedChanged);
			// 
			// editorpanel
			// 
			this.editorpanel.Controls.Add (this.add_button);
			this.editorpanel.Controls.Add (this.manage_button);
			this.editorpanel.Controls.Add (this.save_button);
			this.editorpanel.Controls.Add (this.restore_button);
			this.editorpanel.Location = new System.Drawing.Point (241, 10);
			this.editorpanel.Name = "editorpanel";
			this.editorpanel.Size = new System.Drawing.Size (138, 32);
			this.editorpanel.TabIndex = 18;
			// 
			// add_button
			// 
			this.add_button.BackColor = System.Drawing.Color.Chocolate;
			this.add_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.add_button.FlatAppearance.BorderSize = 0;
			this.add_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.add_button.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.add_button.Location = new System.Drawing.Point (3, 0);
			this.add_button.Name = "add_button";
			this.add_button.Size = new System.Drawing.Size (20, 20);
			this.add_button.TabIndex = 10;
			this.add_button.UseVisualStyleBackColor = false;
			this.add_button.Click += new System.EventHandler (this.add_Click);
			// 
			// manage_button
			// 
			this.manage_button.BackColor = System.Drawing.Color.Chocolate;
			this.manage_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.manage_button.FlatAppearance.BorderSize = 0;
			this.manage_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.manage_button.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.manage_button.Location = new System.Drawing.Point (42, 0);
			this.manage_button.Name = "manage_button";
			this.manage_button.Size = new System.Drawing.Size (20, 20);
			this.manage_button.TabIndex = 12;
			this.manage_button.UseVisualStyleBackColor = false;
			this.manage_button.Click += new System.EventHandler (this.manage_Click);
			// 
			// save_button
			// 
			this.save_button.BackColor = System.Drawing.Color.Chocolate;
			this.save_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.save_button.FlatAppearance.BorderSize = 0;
			this.save_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.save_button.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.save_button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.save_button.Location = new System.Drawing.Point (76, 0);
			this.save_button.Name = "save_button";
			this.save_button.Size = new System.Drawing.Size (20, 20);
			this.save_button.TabIndex = 5;
			this.save_button.UseVisualStyleBackColor = false;
			this.save_button.Click += new System.EventHandler (this.save_Click);
			// 
			// restore_button
			// 
			this.restore_button.BackColor = System.Drawing.Color.Chocolate;
			this.restore_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.restore_button.FlatAppearance.BorderSize = 0;
			this.restore_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.restore_button.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.restore_button.Location = new System.Drawing.Point (110, 0);
			this.restore_button.Name = "restore_button";
			this.restore_button.Size = new System.Drawing.Size (20, 20);
			this.restore_button.TabIndex = 6;
			this.restore_button.UseVisualStyleBackColor = false;
			this.restore_button.Click += new System.EventHandler (this.restore_Click);
			// 
			// import_directory
			// 
			this.import_directory.BackColor = System.Drawing.Color.Chocolate;
			this.import_directory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.import_directory.FlatAppearance.BorderSize = 0;
			this.import_directory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.import_directory.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.import_directory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.import_directory.Location = new System.Drawing.Point (453, 10);
			this.import_directory.Name = "import_directory";
			this.import_directory.Size = new System.Drawing.Size (20, 20);
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
			this.schemes.ListBackColor = System.Drawing.Color.FromArgb (((int)(((byte)(230)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
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
			// import_button
			// 
			this.import_button.BackColor = System.Drawing.Color.Chocolate;
			this.import_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.import_button.FlatAppearance.BorderSize = 0;
			this.import_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.import_button.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.import_button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.import_button.Location = new System.Drawing.Point (419, 10);
			this.import_button.Name = "import_button";
			this.import_button.Size = new System.Drawing.Size (20, 20);
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
			this.export_button.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.export_button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.export_button.Location = new System.Drawing.Point (385, 10);
			this.export_button.Name = "export_button";
			this.export_button.Size = new System.Drawing.Size (20, 20);
			this.export_button.TabIndex = 13;
			this.export_button.UseVisualStyleBackColor = false;
			this.export_button.Click += new System.EventHandler (this.export);
			// 
			// settings_button
			// 
			this.settings_button.BackColor = System.Drawing.Color.Chocolate;
			this.settings_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.settings_button.FlatAppearance.BorderSize = 0;
			this.settings_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.settings_button.Font = new System.Drawing.Font ("Book Antiqua", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.settings_button.Location = new System.Drawing.Point (485, 10);
			this.settings_button.Name = "settings_button";
			this.settings_button.Size = new System.Drawing.Size (20, 20);
			this.settings_button.TabIndex = 11;
			this.settings_button.UseVisualStyleBackColor = false;
			this.settings_button.Click += new System.EventHandler (this.settings_Click);
			// 
			// MForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size (717, 461);
			this.Controls.Add (this.panel1);
			this.Controls.Add (this.sBox);
			this.Controls.Add (this.bottomPanel);
			this.Controls.Add (this.list_1);
			this.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F);
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
			((System.ComponentModel.ISupportInitialize)(this.sBox)).EndInit ();
			this.panel1.ResumeLayout (false);
			this.editorp2.ResumeLayout (false);
			this.imageEditor.ResumeLayout (false);
			this.imageEditor.PerformLayout ();
			this.alignpanel.ResumeLayout (false);
			((System.ComponentModel.ISupportInitialize)(this.opacitySlider)).EndInit ();
			this.colorEditor.ResumeLayout (false);
			this.editorpanel.ResumeLayout (false);
			this.ResumeLayout (false);
		}

		private System.Windows.Forms.Button downloader;

		private System.Windows.Forms.Panel editorp2;

		private System.Windows.Forms.Panel editorpanel;

		private System.Windows.Forms.Panel alignpanel;

		private Yuki_Theme.Core.Controls.FlatNumericUpDown opacitySlider;

		private System.Windows.Forms.Button import_directory;

		private System.Windows.Forms.Button close_btn;
		private System.Windows.Forms.Button select_btn;

		private System.Windows.Forms.Panel bottomPanel;

		private System.Windows.Forms.Label opacityLabel;

		private Yuki_Theme.Core.Controls.Slider opacity_slider;

		private System.Windows.Forms.Button import_button;
		private System.Windows.Forms.Button export_button;

		private System.Windows.Forms.Button clearButton;

		private Yuki_Theme.Core.Controls.CustomText imagePath;

		private System.Windows.Forms.Button pright;
		private System.Windows.Forms.Button pcenter;
		private System.Windows.Forms.Button pleft;
		
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.Button selectImageButton;

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

		private System.Windows.Forms.Label backgroundColorLabel;

		private System.Windows.Forms.Label colorLabel;

		private System.Windows.Forms.ListBox list_1;

		#endregion
	}
}