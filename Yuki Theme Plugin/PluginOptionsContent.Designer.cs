namespace Yuki_Theme_Plugin
{
    partial class PluginOptionsContent
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.settingsPanel = new Yuki_Theme.Core.Controls.SettingsPanel ();
            this.SuspendLayout ();
            // 
            // settingsPanel
            // 
            this.settingsPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.settingsPanel.Location = new System.Drawing.Point (0, 0);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size (405, 253);
            this.settingsPanel.TabIndex = 6;
            // 
            // PluginOptionsContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add (this.settingsPanel);
            this.Name = "PluginOptionsContent";
            this.Size = new System.Drawing.Size (405, 253);
            this.ResumeLayout (false);
        }

        public Yuki_Theme.Core.Controls.SettingsPanel settingsPanel;

        #endregion
    }
}
