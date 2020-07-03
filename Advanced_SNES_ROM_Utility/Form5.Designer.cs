namespace Advanced_SNES_ROM_Utility
{
    partial class Form5
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form5));
            this.labelAboutFunctions = new System.Windows.Forms.Label();
            this.buttonManualClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelAboutFunctions
            // 
            this.labelAboutFunctions.AutoSize = true;
            this.labelAboutFunctions.Location = new System.Drawing.Point(12, 9);
            this.labelAboutFunctions.Name = "labelAboutFunctions";
            this.labelAboutFunctions.Size = new System.Drawing.Size(385, 533);
            this.labelAboutFunctions.TabIndex = 7;
            this.labelAboutFunctions.Text = resources.GetString("labelAboutFunctions.Text");
            // 
            // buttonManualClose
            // 
            this.buttonManualClose.Location = new System.Drawing.Point(13, 551);
            this.buttonManualClose.Name = "buttonManualClose";
            this.buttonManualClose.Size = new System.Drawing.Size(384, 23);
            this.buttonManualClose.TabIndex = 8;
            this.buttonManualClose.Text = "Close";
            this.buttonManualClose.UseVisualStyleBackColor = true;
            this.buttonManualClose.Click += new System.EventHandler(this.buttonManualClose_Click);
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 583);
            this.Controls.Add(this.buttonManualClose);
            this.Controls.Add(this.labelAboutFunctions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form5";
            this.Text = "Manual of the Advanced SNES ROM Utility v0.7.4";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAboutFunctions;
        private System.Windows.Forms.Button buttonManualClose;
    }
}