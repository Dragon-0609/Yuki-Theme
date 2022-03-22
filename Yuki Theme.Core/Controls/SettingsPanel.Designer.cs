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
			this.tbpanel = new System.Windows.Forms.Panel ();
			this.panel1 = new System.Windows.Forms.Panel ();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel ();
			this.backImage = new System.Windows.Forms.CheckBox ();
			this.editor = new System.Windows.Forms.CheckBox ();
			this.fitWidth = new System.Windows.Forms.CheckBox ();
			this.askSave = new System.Windows.Forms.CheckBox ();
			this.label5 = new System.Windows.Forms.Label ();
			this.groupBox1 = new System.Windows.Forms.GroupBox ();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel ();
			this.swsticker = new System.Windows.Forms.CheckBox ();
			this.cstm_sticker = new System.Windows.Forms.Button ();
			this.use_cstm_sticker = new System.Windows.Forms.CheckBox ();
			this.checkBox3 = new System.Windows.Forms.CheckBox ();
			this.label2 = new System.Windows.Forms.Label ();
			this.unit = new CustomControls.RJControls.RJComboBox ();
			this.checkBox4 = new System.Windows.Forms.CheckBox ();
			this.reset_margin = new System.Windows.Forms.Button ();
			this.saveOld = new System.Windows.Forms.CheckBox ();
			this.roundLabel1 = new Yuki_Theme.Core.Controls.RoundLabel ();
			this.showHelp = new System.Windows.Forms.Button ();
			this.label4 = new System.Windows.Forms.Label ();
			this.mode = new CustomControls.RJControls.RJComboBox ();
			this.checkBox2 = new System.Windows.Forms.CheckBox ();
			this.button5 = new System.Windows.Forms.Button ();
			this.button6 = new System.Windows.Forms.Button ();
			this.restartUpdate = new System.Windows.Forms.Button ();
			this.checkBox1 = new System.Windows.Forms.CheckBox ();
			this.button4 = new System.Windows.Forms.Button ();
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
			this.add_toolbar = new System.Windows.Forms.TabPage ();
			this.toolBarPosition = new System.Windows.Forms.CheckBox ();
			this.toolBarPositionLabel = new System.Windows.Forms.Label ();
			this.button2 = new System.Windows.Forms.Button ();
			this.toolBarVisible = new System.Windows.Forms.CheckBox ();
			this.toolBarVisibleLabel = new System.Windows.Forms.Label ();
			this.toolBarIconLabel = new System.Windows.Forms.Label ();
			this.toolBarImage = new System.Windows.Forms.PictureBox ();
			this.toolBarList = new System.Windows.Forms.ListBox ();
			this.tabs.SuspendLayout ();
			this.tabPage1.SuspendLayout ();
			this.tbpanel.SuspendLayout ();
			this.panel1.SuspendLayout ();
			this.flowLayoutPanel1.SuspendLayout ();
			this.groupBox1.SuspendLayout ();
			this.flowLayoutPanel2.SuspendLayout ();
			this.add_program.SuspendLayout ();
			this.add_plugin.SuspendLayout ();
			this.add_toolbar.SuspendLayout ();
			((System.ComponentModel.ISupportInitialize)(this.toolBarImage)).BeginInit ();
			this.SuspendLayout ();
			// 
			// tabs
			// 
			this.tabs.Controls.Add (this.tabPage1);
			this.tabs.Controls.Add (this.add_program);
			this.tabs.Controls.Add (this.add_plugin);
			this.tabs.Controls.Add (this.add_toolbar);
			this.tabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabs.Font = new System.Drawing.Font ("Calibri", 9F);
			this.tabs.Location = new System.Drawing.Point (0, 0);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size (405, 253);
			this.tabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabs.TabIndex = 32;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add (this.tbpanel);
			this.tabPage1.Location = new System.Drawing.Point (4, 25);
			this.tabPage1.Margin = new System.Windows.Forms.Padding (12, 6, 12, 6);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size (397, 224);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "General";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tbpanel
			// 
			this.tbpanel.Controls.Add (this.panel1);
			this.tbpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbpanel.Location = new System.Drawing.Point (0, 0);
			this.tbpanel.Name = "tbpanel";
			this.tbpanel.Size = new System.Drawing.Size (397, 224);
			this.tbpanel.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.AutoScrollMargin = new System.Drawing.Size (0, 10);
			this.panel1.AutoScrollMinSize = new System.Drawing.Size (100, 0);
			this.panel1.Controls.Add (this.flowLayoutPanel1);
			this.panel1.Font = new System.Drawing.Font ("Calibri", 9F);
			this.panel1.Location = new System.Drawing.Point (0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size (397, 224);
			this.panel1.TabIndex = 36;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add (this.backImage);
			this.flowLayoutPanel1.Controls.Add (this.editor);
			this.flowLayoutPanel1.Controls.Add (this.fitWidth);
			this.flowLayoutPanel1.Controls.Add (this.askSave);
			this.flowLayoutPanel1.Controls.Add (this.label5);
			this.flowLayoutPanel1.Controls.Add (this.groupBox1);
			this.flowLayoutPanel1.Controls.Add (this.saveOld);
			this.flowLayoutPanel1.Controls.Add (this.roundLabel1);
			this.flowLayoutPanel1.Controls.Add (this.showHelp);
			this.flowLayoutPanel1.Controls.Add (this.label4);
			this.flowLayoutPanel1.Controls.Add (this.mode);
			this.flowLayoutPanel1.Controls.Add (this.checkBox2);
			this.flowLayoutPanel1.Controls.Add (this.button5);
			this.flowLayoutPanel1.Controls.Add (this.button6);
			this.flowLayoutPanel1.Controls.Add (this.restartUpdate);
			this.flowLayoutPanel1.Controls.Add (this.checkBox1);
			this.flowLayoutPanel1.Controls.Add (this.button4);
			this.flowLayoutPanel1.Location = new System.Drawing.Point (0, 0);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding (3, 3, 3, 0);
			this.flowLayoutPanel1.MaximumSize = new System.Drawing.Size (379, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding (3);
			this.flowLayoutPanel1.Size = new System.Drawing.Size (379, 473);
			this.flowLayoutPanel1.TabIndex = 43;
			// 
			// backImage
			// 
			this.backImage.AutoSize = true;
			this.backImage.Location = new System.Drawing.Point (6, 6);
			this.backImage.Margin = new System.Windows.Forms.Padding (3, 3, 3, 6);
			this.backImage.Name = "backImage";
			this.backImage.Size = new System.Drawing.Size (158, 18);
			this.backImage.TabIndex = 1;
			this.backImage.Text = "Show Background Image";
			this.backImage.UseVisualStyleBackColor = true;
			this.backImage.CheckedChanged += new System.EventHandler (this.backImage_CheckedChanged);
			// 
			// editor
			// 
			this.editor.AutoSize = true;
			this.editor.Location = new System.Drawing.Point (173, 6);
			this.editor.Margin = new System.Windows.Forms.Padding (6, 3, 3, 6);
			this.editor.Name = "editor";
			this.editor.Size = new System.Drawing.Size (92, 18);
			this.editor.TabIndex = 2;
			this.editor.Text = "Editor Mode";
			this.editor.UseVisualStyleBackColor = true;
			// 
			// fitWidth
			// 
			this.fitWidth.AutoSize = true;
			this.fitWidth.Location = new System.Drawing.Point (6, 33);
			this.fitWidth.Margin = new System.Windows.Forms.Padding (3, 3, 3, 6);
			this.fitWidth.Name = "fitWidth";
			this.fitWidth.Size = new System.Drawing.Size (104, 18);
			this.fitWidth.TabIndex = 3;
			this.fitWidth.Text = "Auto Fit Width";
			this.fitWidth.UseVisualStyleBackColor = true;
			// 
			// askSave
			// 
			this.askSave.AutoSize = true;
			this.askSave.Location = new System.Drawing.Point (128, 36);
			this.askSave.Margin = new System.Windows.Forms.Padding (15, 6, 3, 3);
			this.askSave.Name = "askSave";
			this.askSave.Size = new System.Drawing.Size (128, 18);
			this.askSave.TabIndex = 4;
			this.askSave.Text = "Always ask to save";
			this.askSave.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point (6, 63);
			this.label5.Margin = new System.Windows.Forms.Padding (3, 6, 3, 9);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size (314, 14);
			this.label5.TabIndex = 5;
			this.label5.Text = "By this, wallpaper will be fitted by width if it\'s necessary";
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.Controls.Add (this.flowLayoutPanel2);
			this.groupBox1.Font = new System.Drawing.Font ("Calibri", 10F);
			this.groupBox1.Location = new System.Drawing.Point (3, 89);
			this.groupBox1.Margin = new System.Windows.Forms.Padding (0, 3, 0, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding (3, 3, 3, 0);
			this.groupBox1.Size = new System.Drawing.Size (373, 190);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Sticker";
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.AutoSize = true;
			this.flowLayoutPanel2.Controls.Add (this.swsticker);
			this.flowLayoutPanel2.Controls.Add (this.cstm_sticker);
			this.flowLayoutPanel2.Controls.Add (this.use_cstm_sticker);
			this.flowLayoutPanel2.Controls.Add (this.checkBox3);
			this.flowLayoutPanel2.Controls.Add (this.label2);
			this.flowLayoutPanel2.Controls.Add (this.unit);
			this.flowLayoutPanel2.Controls.Add (this.checkBox4);
			this.flowLayoutPanel2.Controls.Add (this.reset_margin);
			this.flowLayoutPanel2.Location = new System.Drawing.Point (3, 20);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding (3, 3, 3, 0);
			this.flowLayoutPanel2.MaximumSize = new System.Drawing.Size (364, 0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Padding = new System.Windows.Forms.Padding (3, 3, 3, 6);
			this.flowLayoutPanel2.Size = new System.Drawing.Size (364, 153);
			this.flowLayoutPanel2.TabIndex = 0;
			// 
			// swsticker
			// 
			this.swsticker.AutoSize = true;
			this.flowLayoutPanel2.SetFlowBreak (this.swsticker, true);
			this.swsticker.Location = new System.Drawing.Point (6, 11);
			this.swsticker.Margin = new System.Windows.Forms.Padding (3, 8, 3, 3);
			this.swsticker.Name = "swsticker";
			this.swsticker.Size = new System.Drawing.Size (98, 21);
			this.swsticker.TabIndex = 7;
			this.swsticker.Text = "Show Sticker";
			this.swsticker.UseVisualStyleBackColor = true;
			this.swsticker.CheckedChanged += new System.EventHandler (this.swsticker_CheckedChanged);
			// 
			// cstm_sticker
			// 
			this.cstm_sticker.AutoSize = true;
			this.cstm_sticker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cstm_sticker.Location = new System.Drawing.Point (6, 42);
			this.cstm_sticker.Name = "cstm_sticker";
			this.cstm_sticker.Size = new System.Drawing.Size (108, 30);
			this.cstm_sticker.TabIndex = 9;
			this.cstm_sticker.Text = "Choose Image";
			this.cstm_sticker.UseVisualStyleBackColor = true;
			this.cstm_sticker.Click += new System.EventHandler (this.cstm_sticker_Click);
			// 
			// use_cstm_sticker
			// 
			this.use_cstm_sticker.AutoSize = true;
			this.flowLayoutPanel2.SetFlowBreak (this.use_cstm_sticker, true);
			this.use_cstm_sticker.Location = new System.Drawing.Point (123, 47);
			this.use_cstm_sticker.Margin = new System.Windows.Forms.Padding (6, 8, 3, 3);
			this.use_cstm_sticker.Name = "use_cstm_sticker";
			this.use_cstm_sticker.Size = new System.Drawing.Size (134, 21);
			this.use_cstm_sticker.TabIndex = 8;
			this.use_cstm_sticker.Text = "Use Custom Sticker";
			this.use_cstm_sticker.UseVisualStyleBackColor = true;
			// 
			// checkBox3
			// 
			this.checkBox3.AutoSize = true;
			this.checkBox3.Location = new System.Drawing.Point (6, 81);
			this.checkBox3.Margin = new System.Windows.Forms.Padding (3, 6, 5, 3);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size (129, 21);
			this.checkBox3.TabIndex = 10;
			this.checkBox3.Text = "Enable Positioning";
			this.checkBox3.UseVisualStyleBackColor = false;
			this.checkBox3.CheckedChanged += new System.EventHandler (this.checkBox3_CheckedChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font ("Book Antiqua", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point (144, 81);
			this.label2.Margin = new System.Windows.Forms.Padding (4, 6, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size (83, 16);
			this.label2.TabIndex = 11;
			this.label2.Text = "Position Unit:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// unit
			// 
			this.unit.BackColor = System.Drawing.Color.WhiteSmoke;
			this.unit.BorderColor = System.Drawing.Color.MediumSlateBlue;
			this.unit.BorderSize = 1;
			this.unit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.flowLayoutPanel2.SetFlowBreak (this.unit, true);
			this.unit.Font = new System.Drawing.Font ("Calibri", 9F);
			this.unit.ForeColor = System.Drawing.Color.DimGray;
			this.unit.IconColor = System.Drawing.Color.MediumSlateBlue;
			this.unit.ListBackColor = System.Drawing.Color.FromArgb (((int)(((byte)(230)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
			this.unit.ListTextColor = System.Drawing.Color.DimGray;
			this.unit.Location = new System.Drawing.Point (234, 78);
			this.unit.MinimumSize = new System.Drawing.Size (120, 30);
			this.unit.Name = "unit";
			this.unit.Padding = new System.Windows.Forms.Padding (1);
			this.unit.Size = new System.Drawing.Size (120, 30);
			this.unit.TabIndex = 12;
			this.unit.Texts = "";
			// 
			// checkBox4
			// 
			this.checkBox4.AutoSize = true;
			this.checkBox4.Location = new System.Drawing.Point (6, 119);
			this.checkBox4.Margin = new System.Windows.Forms.Padding (3, 8, 6, 3);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size (89, 21);
			this.checkBox4.TabIndex = 13;
			this.checkBox4.Text = "Show Grids";
			this.checkBox4.UseVisualStyleBackColor = false;
			// 
			// reset_margin
			// 
			this.reset_margin.AutoSize = true;
			this.reset_margin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.reset_margin.Location = new System.Drawing.Point (104, 114);
			this.reset_margin.Name = "reset_margin";
			this.reset_margin.Size = new System.Drawing.Size (162, 30);
			this.reset_margin.TabIndex = 14;
			this.reset_margin.Text = "Reset Sticker Margins";
			this.reset_margin.UseVisualStyleBackColor = true;
			this.reset_margin.Click += new System.EventHandler (this.reset_margin_Click);
			// 
			// saveOld
			// 
			this.saveOld.AutoSize = true;
			this.saveOld.Location = new System.Drawing.Point (6, 291);
			this.saveOld.Margin = new System.Windows.Forms.Padding (3, 6, 0, 3);
			this.saveOld.Name = "saveOld";
			this.saveOld.Size = new System.Drawing.Size (125, 18);
			this.saveOld.TabIndex = 15;
			this.saveOld.Text = "Save in old format";
			this.saveOld.UseVisualStyleBackColor = true;
			// 
			// roundLabel1
			// 
			this.roundLabel1._BackColor = System.Drawing.Color.Black;
			this.roundLabel1.AutoSize = true;
			this.roundLabel1.Font = new System.Drawing.Font ("Calibri", 8F);
			this.roundLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.roundLabel1.Location = new System.Drawing.Point (131, 285);
			this.roundLabel1.Margin = new System.Windows.Forms.Padding (0, 0, 3, 0);
			this.roundLabel1.Name = "roundLabel1";
			this.roundLabel1.Radius = 10;
			this.roundLabel1.Size = new System.Drawing.Size (24, 13);
			this.roundLabel1.TabIndex = 16;
			this.roundLabel1.Text = "dev";
			this.roundLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// showHelp
			// 
			this.showHelp.AutoSize = true;
			this.showHelp.FlatAppearance.BorderSize = 0;
			this.showHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.flowLayoutPanel1.SetFlowBreak (this.showHelp, true);
			this.showHelp.Location = new System.Drawing.Point (161, 288);
			this.showHelp.Name = "showHelp";
			this.showHelp.Size = new System.Drawing.Size (26, 26);
			this.showHelp.TabIndex = 41;
			this.showHelp.UseVisualStyleBackColor = true;
			this.showHelp.Click += new System.EventHandler (this.showHelp_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font ("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label4.Location = new System.Drawing.Point (7, 323);
			this.label4.Margin = new System.Windows.Forms.Padding (4, 6, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size (87, 17);
			this.label4.TabIndex = 17;
			this.label4.Text = "Setting Mode:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mode
			// 
			this.mode.BackColor = System.Drawing.Color.WhiteSmoke;
			this.mode.BorderColor = System.Drawing.Color.MediumSlateBlue;
			this.mode.BorderSize = 1;
			this.mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.mode.Font = new System.Drawing.Font ("Calibri", 9F);
			this.mode.ForeColor = System.Drawing.Color.DimGray;
			this.mode.IconColor = System.Drawing.Color.MediumSlateBlue;
			this.mode.ListBackColor = System.Drawing.Color.FromArgb (((int)(((byte)(230)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
			this.mode.ListTextColor = System.Drawing.Color.DimGray;
			this.mode.Location = new System.Drawing.Point (101, 320);
			this.mode.MinimumSize = new System.Drawing.Size (100, 30);
			this.mode.Name = "mode";
			this.mode.Padding = new System.Windows.Forms.Padding (1);
			this.mode.Size = new System.Drawing.Size (105, 30);
			this.mode.TabIndex = 18;
			this.mode.Texts = "";
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new System.Drawing.Point (6, 362);
			this.checkBox2.Margin = new System.Windows.Forms.Padding (3, 9, 3, 3);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size (182, 18);
			this.checkBox2.TabIndex = 19;
			this.checkBox2.Text = "Automatically Check updates";
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// button5
			// 
			this.button5.AutoSize = true;
			this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button5.Location = new System.Drawing.Point (194, 356);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size (98, 32);
			this.button5.TabIndex = 20;
			this.button5.Text = "Check Update";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler (this.button5_Click);
			// 
			// button6
			// 
			this.button6.AutoSize = true;
			this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button6.Location = new System.Drawing.Point (6, 397);
			this.button6.Margin = new System.Windows.Forms.Padding (3, 6, 6, 3);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size (130, 32);
			this.button6.TabIndex = 21;
			this.button6.Text = "Install from Disk";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler (this.button6_Click);
			// 
			// restartUpdate
			// 
			this.restartUpdate.AutoSize = true;
			this.restartUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.restartUpdate.Location = new System.Drawing.Point (150, 397);
			this.restartUpdate.Margin = new System.Windows.Forms.Padding (8, 6, 3, 3);
			this.restartUpdate.Name = "restartUpdate";
			this.restartUpdate.Size = new System.Drawing.Size (130, 32);
			this.restartUpdate.TabIndex = 22;
			this.restartUpdate.Text = "Restart for update";
			this.restartUpdate.UseVisualStyleBackColor = true;
			this.restartUpdate.Click += new System.EventHandler (this.restartUpdate_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point (6, 438);
			this.checkBox1.Margin = new System.Windows.Forms.Padding (3, 6, 9, 3);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size (161, 18);
			this.checkBox1.TabIndex = 23;
			this.checkBox1.Text = "Updates for Beta version";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// button4
			// 
			this.button4.AutoSize = true;
			this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button4.Location = new System.Drawing.Point (184, 435);
			this.button4.Margin = new System.Windows.Forms.Padding (8, 3, 3, 3);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size (71, 32);
			this.button4.TabIndex = 24;
			this.button4.Text = "About";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler (this.button4_Click);
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
			this.askC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.askC.Location = new System.Drawing.Point (15, 76);
			this.askC.Name = "askC";
			this.askC.Size = new System.Drawing.Size (367, 42);
			this.askC.TabIndex = 12;
			this.askC.Text = "Ask if there are other themes in PascalABC directory";
			this.askC.UseVisualStyleBackColor = true;
			this.askC.CheckedChanged += new System.EventHandler (this.checkBox2_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font ("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point (15, 6);
			this.label1.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (199, 26);
			this.label1.TabIndex = 0;
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBox1
			// 
			this.textBox1.BorderColor = System.Drawing.Color.Blue;
			this.textBox1.Font = new System.Drawing.Font ("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.textBox1.Location = new System.Drawing.Point (15, 36);
			this.textBox1.Margin = new System.Windows.Forms.Padding (4);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size (306, 22);
			this.textBox1.TabIndex = 1;
			// 
			// ActionBox
			// 
			this.ActionBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ActionBox.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ActionBox.BorderColor = System.Drawing.Color.MediumSlateBlue;
			this.ActionBox.BorderSize = 1;
			this.ActionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ActionBox.Font = new System.Drawing.Font ("Calibri", 9F);
			this.ActionBox.ForeColor = System.Drawing.Color.DimGray;
			this.ActionBox.IconColor = System.Drawing.Color.MediumSlateBlue;
			this.ActionBox.ListBackColor = System.Drawing.Color.FromArgb (((int)(((byte)(230)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
			this.ActionBox.ListTextColor = System.Drawing.Color.DimGray;
			this.ActionBox.Location = new System.Drawing.Point (182, 124);
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
			this.button1.Font = new System.Drawing.Font ("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.button1.Location = new System.Drawing.Point (325, 34);
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
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.Font = new System.Drawing.Font ("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.Location = new System.Drawing.Point (15, 121);
			this.label3.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size (160, 33);
			this.label3.TabIndex = 14;
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
			// add_toolbar
			// 
			this.add_toolbar.Controls.Add (this.toolBarPosition);
			this.add_toolbar.Controls.Add (this.toolBarPositionLabel);
			this.add_toolbar.Controls.Add (this.button2);
			this.add_toolbar.Controls.Add (this.toolBarVisible);
			this.add_toolbar.Controls.Add (this.toolBarVisibleLabel);
			this.add_toolbar.Controls.Add (this.toolBarIconLabel);
			this.add_toolbar.Controls.Add (this.toolBarImage);
			this.add_toolbar.Controls.Add (this.toolBarList);
			this.add_toolbar.Location = new System.Drawing.Point (4, 25);
			this.add_toolbar.Name = "add_toolbar";
			this.add_toolbar.Size = new System.Drawing.Size (397, 224);
			this.add_toolbar.TabIndex = 3;
			this.add_toolbar.Text = "ToolBar";
			this.add_toolbar.UseVisualStyleBackColor = true;
			// 
			// toolBarPosition
			// 
			this.toolBarPosition.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolBarPosition.Location = new System.Drawing.Point (253, 114);
			this.toolBarPosition.Name = "toolBarPosition";
			this.toolBarPosition.Size = new System.Drawing.Size (32, 28);
			this.toolBarPosition.TabIndex = 7;
			this.toolBarPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolBarPosition.UseVisualStyleBackColor = true;
			this.toolBarPosition.CheckedChanged += new System.EventHandler (this.toolBarPosition_CheckedChanged);
			// 
			// toolBarPositionLabel
			// 
			this.toolBarPositionLabel.AutoSize = true;
			this.toolBarPositionLabel.Location = new System.Drawing.Point (157, 113);
			this.toolBarPositionLabel.Name = "toolBarPositionLabel";
			this.toolBarPositionLabel.Size = new System.Drawing.Size (0, 14);
			this.toolBarPositionLabel.TabIndex = 6;
			// 
			// button2
			// 
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Location = new System.Drawing.Point (157, 182);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size (72, 32);
			this.button2.TabIndex = 5;
			this.button2.Text = "Reset";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler (this.button2_Click);
			// 
			// toolBarVisible
			// 
			this.toolBarVisible.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolBarVisible.Location = new System.Drawing.Point (253, 68);
			this.toolBarVisible.Name = "toolBarVisible";
			this.toolBarVisible.Size = new System.Drawing.Size (32, 28);
			this.toolBarVisible.TabIndex = 4;
			this.toolBarVisible.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolBarVisible.UseVisualStyleBackColor = true;
			this.toolBarVisible.CheckedChanged += new System.EventHandler (this.toolBarVisible_CheckedChanged);
			// 
			// toolBarVisibleLabel
			// 
			this.toolBarVisibleLabel.AutoSize = true;
			this.toolBarVisibleLabel.Location = new System.Drawing.Point (157, 67);
			this.toolBarVisibleLabel.Name = "toolBarVisibleLabel";
			this.toolBarVisibleLabel.Size = new System.Drawing.Size (0, 14);
			this.toolBarVisibleLabel.TabIndex = 3;
			// 
			// toolBarIconLabel
			// 
			this.toolBarIconLabel.AutoSize = true;
			this.toolBarIconLabel.Location = new System.Drawing.Point (157, 10);
			this.toolBarIconLabel.Name = "toolBarIconLabel";
			this.toolBarIconLabel.Size = new System.Drawing.Size (0, 14);
			this.toolBarIconLabel.TabIndex = 2;
			// 
			// toolBarImage
			// 
			this.toolBarImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.toolBarImage.Location = new System.Drawing.Point (253, 10);
			this.toolBarImage.Name = "toolBarImage";
			this.toolBarImage.Size = new System.Drawing.Size (32, 32);
			this.toolBarImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.toolBarImage.TabIndex = 1;
			this.toolBarImage.TabStop = false;
			// 
			// toolBarList
			// 
			this.toolBarList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.toolBarList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.toolBarList.FormattingEnabled = true;
			this.toolBarList.ItemHeight = 17;
			this.toolBarList.Location = new System.Drawing.Point (5, 10);
			this.toolBarList.Name = "toolBarList";
			this.toolBarList.Size = new System.Drawing.Size (142, 204);
			this.toolBarList.TabIndex = 0;
			this.toolBarList.DrawItem += new System.Windows.Forms.DrawItemEventHandler (this.list_1_DrawItem);
			this.toolBarList.SelectedIndexChanged += new System.EventHandler (this.toolBarList_SelectedIndexChanged);
			this.toolBarList.MouseMove += new System.Windows.Forms.MouseEventHandler (this.Lst_MouseHover);
			// 
			// SettingsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.Controls.Add (this.tabs);
			this.Font = new System.Drawing.Font ("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Location = new System.Drawing.Point (15, 15);
			this.Name = "SettingsPanel";
			this.Size = new System.Drawing.Size (405, 253);
			this.tabs.ResumeLayout (false);
			this.tabPage1.ResumeLayout (false);
			this.tbpanel.ResumeLayout (false);
			this.panel1.ResumeLayout (false);
			this.panel1.PerformLayout ();
			this.flowLayoutPanel1.ResumeLayout (false);
			this.flowLayoutPanel1.PerformLayout ();
			this.groupBox1.ResumeLayout (false);
			this.groupBox1.PerformLayout ();
			this.flowLayoutPanel2.ResumeLayout (false);
			this.flowLayoutPanel2.PerformLayout ();
			this.add_program.ResumeLayout (false);
			this.add_program.PerformLayout ();
			this.add_plugin.ResumeLayout (false);
			this.add_toolbar.ResumeLayout (false);
			this.add_toolbar.PerformLayout ();
			((System.ComponentModel.ISupportInitialize)(this.toolBarImage)).EndInit ();
			this.ResumeLayout (false);
		}

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;

		public System.Windows.Forms.Button restartUpdate;

		public System.Windows.Forms.Button showHelp;

		public System.Windows.Forms.CheckBox askSave;

		public System.Windows.Forms.CheckBox fitWidth;
		
		private System.Windows.Forms.Label label5;

		public System.Windows.Forms.CheckBox use_cstm_sticker;
		public System.Windows.Forms.Button   cstm_sticker;

		public System.Windows.Forms.CheckBox saveOld;

		public System.Windows.Forms.CheckBox checkBox4;

		public System.Windows.Forms.GroupBox groupBox1;

		private System.Windows.Forms.Panel panel1;

		public System.Windows.Forms.Button reset_margin;

		public Yuki_Theme.Core.Controls.RoundLabel roundLabel1;

		public System.Windows.Forms.CheckBox        checkBox3;
		public System.Windows.Forms.Label           label2;
		public CustomControls.RJControls.RJComboBox unit;

		private System.Windows.Forms.Panel tbpanel;

		public System.Windows.Forms.CheckBox toolBarPosition;
		public System.Windows.Forms.Label    toolBarPositionLabel;

		public System.Windows.Forms.Button button2;

		public System.Windows.Forms.CheckBox toolBarVisible;

		public System.Windows.Forms.Label toolBarVisibleLabel;

		public System.Windows.Forms.Label toolBarIconLabel;

		public System.Windows.Forms.PictureBox toolBarImage;

		public System.Windows.Forms.ListBox toolBarList;

		public System.Windows.Forms.TabPage add_toolbar;

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