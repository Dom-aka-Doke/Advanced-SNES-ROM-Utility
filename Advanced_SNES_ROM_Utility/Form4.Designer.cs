namespace Advanced_SNES_ROM_Utility
{
    partial class Form4
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form4));
            this.buttonAboutClose = new System.Windows.Forms.Button();
            this.labelAboutLicense = new System.Windows.Forms.Label();
            this.labelAboutLogo = new System.Windows.Forms.Label();
            this.linkLabelAboutLogo = new System.Windows.Forms.LinkLabel();
            this.labelAboutSpeicalThanks = new System.Windows.Forms.Label();
            this.linkLabelAboutCommunityLink = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // buttonAboutClose
            // 
            this.buttonAboutClose.Location = new System.Drawing.Point(12, 200);
            this.buttonAboutClose.Name = "buttonAboutClose";
            this.buttonAboutClose.Size = new System.Drawing.Size(411, 23);
            this.buttonAboutClose.TabIndex = 0;
            this.buttonAboutClose.Text = "Close";
            this.buttonAboutClose.UseVisualStyleBackColor = true;
            this.buttonAboutClose.Click += new System.EventHandler(this.buttonAboutClose_Click);
            // 
            // labelAboutLicense
            // 
            this.labelAboutLicense.AutoSize = true;
            this.labelAboutLicense.Location = new System.Drawing.Point(12, 9);
            this.labelAboutLicense.Name = "labelAboutLicense";
            this.labelAboutLicense.Size = new System.Drawing.Size(399, 52);
            this.labelAboutLicense.TabIndex = 1;
            this.labelAboutLicense.Text = resources.GetString("labelAboutLicense.Text");
            // 
            // labelAboutLogo
            // 
            this.labelAboutLogo.AutoSize = true;
            this.labelAboutLogo.Location = new System.Drawing.Point(12, 179);
            this.labelAboutLogo.Name = "labelAboutLogo";
            this.labelAboutLogo.Size = new System.Drawing.Size(142, 13);
            this.labelAboutLogo.TabIndex = 2;
            this.labelAboutLogo.Text = "Logo design by chrisbanks2:";
            // 
            // linkLabelAboutLogo
            // 
            this.linkLabelAboutLogo.AutoSize = true;
            this.linkLabelAboutLogo.Location = new System.Drawing.Point(152, 179);
            this.linkLabelAboutLogo.Name = "linkLabelAboutLogo";
            this.linkLabelAboutLogo.Size = new System.Drawing.Size(254, 13);
            this.linkLabelAboutLogo.TabIndex = 3;
            this.linkLabelAboutLogo.TabStop = true;
            this.linkLabelAboutLogo.Text = "http://www.iconarchive.com/artist/chrisbanks2.html";
            this.linkLabelAboutLogo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelAboutLogo_LinkClicked);
            // 
            // labelAboutSpeicalThanks
            // 
            this.labelAboutSpeicalThanks.AutoSize = true;
            this.labelAboutSpeicalThanks.Location = new System.Drawing.Point(12, 72);
            this.labelAboutSpeicalThanks.Name = "labelAboutSpeicalThanks";
            this.labelAboutSpeicalThanks.Size = new System.Drawing.Size(352, 91);
            this.labelAboutSpeicalThanks.TabIndex = 4;
            this.labelAboutSpeicalThanks.Text = resources.GetString("labelAboutSpeicalThanks.Text");
            // 
            // linkLabelAboutCommunityLink
            // 
            this.linkLabelAboutCommunityLink.AutoSize = true;
            this.linkLabelAboutCommunityLink.Location = new System.Drawing.Point(324, 110);
            this.linkLabelAboutCommunityLink.Name = "linkLabelAboutCommunityLink";
            this.linkLabelAboutCommunityLink.Size = new System.Drawing.Size(84, 13);
            this.linkLabelAboutCommunityLink.TabIndex = 5;
            this.linkLabelAboutCommunityLink.TabStop = true;
            this.linkLabelAboutCommunityLink.Text = "snes-projects.de";
            this.linkLabelAboutCommunityLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelAboutCommunityLink_LinkClicked);
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 232);
            this.Controls.Add(this.labelAboutLicense);
            this.Controls.Add(this.linkLabelAboutCommunityLink);
            this.Controls.Add(this.labelAboutSpeicalThanks);
            this.Controls.Add(this.linkLabelAboutLogo);
            this.Controls.Add(this.labelAboutLogo);
            this.Controls.Add(this.buttonAboutClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form4";
            this.Text = "About the Advanced SNES ROM Utility v0.8";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAboutClose;
        private System.Windows.Forms.Label labelAboutLicense;
        private System.Windows.Forms.Label labelAboutLogo;
        private System.Windows.Forms.LinkLabel linkLabelAboutLogo;
        private System.Windows.Forms.Label labelAboutSpeicalThanks;
        private System.Windows.Forms.LinkLabel linkLabelAboutCommunityLink;
    }
}