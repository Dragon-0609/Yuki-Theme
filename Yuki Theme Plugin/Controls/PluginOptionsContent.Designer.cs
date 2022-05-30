namespace Yuki_Theme_Plugin.Controls
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
            this.PanelHost = new System.Windows.Forms.Integration.ElementHost ();
            this.SuspendLayout ();
            // 
            // PanelHost
            // 
            this.PanelHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelHost.Location = new System.Drawing.Point (0, 0);
            this.PanelHost.Name = "PanelHost";
            this.PanelHost.TabIndex = 0;
            // 
            // PluginOptionsContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Name = "PluginOptionsContent";
            this.Size = new System.Drawing.Size (405, 253);
            this.ResumeLayout (false);
        }

        private System.Windows.Forms.Integration.ElementHost PanelHost;

        #endregion
    }
}
