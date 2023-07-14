namespace Advanced_SNES_ROM_Utility
{
    partial class FormHelp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHelp));
            this.buttonManualClose = new System.Windows.Forms.Button();
            this.webBrowserHelp = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // buttonManualClose
            // 
            this.buttonManualClose.Location = new System.Drawing.Point(0, 563);
            this.buttonManualClose.Name = "buttonManualClose";
            this.buttonManualClose.Size = new System.Drawing.Size(583, 23);
            this.buttonManualClose.TabIndex = 8;
            this.buttonManualClose.Text = "Close";
            this.buttonManualClose.UseVisualStyleBackColor = true;
            this.buttonManualClose.Click += new System.EventHandler(this.buttonManualClose_Click);
            // 
            // webBrowserHelp
            // 
            this.webBrowserHelp.Location = new System.Drawing.Point(0, 0);
            this.webBrowserHelp.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserHelp.Name = "webBrowserHelp";
            this.webBrowserHelp.Size = new System.Drawing.Size(583, 557);
            this.webBrowserHelp.TabIndex = 8;
            // 
            // FormHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 587);
            this.Controls.Add(this.webBrowserHelp);
            this.Controls.Add(this.buttonManualClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormHelp";
            this.Text = "Manual of the Advanced SNES ROM Utility v1.1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonManualClose;
        private System.Windows.Forms.WebBrowser webBrowserHelp;
    }
}