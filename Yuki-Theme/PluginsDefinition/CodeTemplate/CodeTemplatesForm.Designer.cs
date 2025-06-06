namespace CodeTemplatesPlugin
{
    partial class CodeTemplatesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBox1.FullRowSelect = true;
            this.listBox1.HideSelection = false;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2);
            this.listBox1.MultiSelect = false;
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(232, 292);
            this.listBox1.TabIndex = 0;
            this.listBox1.TileSize = new System.Drawing.Size(380, 14);
            this.listBox1.UseCompatibleStateImageBehavior = false;
            this.listBox1.View = System.Windows.Forms.View.Tile;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.SizeChanged += new System.EventHandler(this.listBox1_SizeChanged);
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick_1);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label1.Location = new System.Drawing.Point(0, 294);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.MaximumSize = new System.Drawing.Size(0, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 45);
            this.label1.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 292);
            this.splitter1.Margin = new System.Windows.Forms.Padding(2);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(232, 2);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // CodeTemplatesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(232, 339);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.label1);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
            this.ForeColor = System.Drawing.Color.Coral;
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "CodeTemplatesForm";
            this.TabText = "Образцы кода";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CodeTemplatesForm_FormClosing);
            this.ResumeLayout(false);
        }

        #endregion

        internal System.Windows.Forms.ListView listBox1;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Splitter splitter1;
    }
}