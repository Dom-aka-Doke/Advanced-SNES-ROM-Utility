namespace Advanced_SNES_ROM_Utility
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.buttonAboutClose = new System.Windows.Forms.Button();
            this.labelAboutLicense = new System.Windows.Forms.Label();
            this.labelAboutLogo = new System.Windows.Forms.Label();
            this.linkLabelAboutLogo = new System.Windows.Forms.LinkLabel();
            this.labelAboutSpeicalThanks = new System.Windows.Forms.Label();
            this.linkLabelAboutCommunityLink = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAboutClose
            // 
            this.buttonAboutClose.Location = new System.Drawing.Point(12, 315);
            this.buttonAboutClose.Name = "buttonAboutClose";
            this.buttonAboutClose.Size = new System.Drawing.Size(427, 23);
            this.buttonAboutClose.TabIndex = 0;
            this.buttonAboutClose.Text = "Close";
            this.buttonAboutClose.UseVisualStyleBackColor = true;
            this.buttonAboutClose.Click += new System.EventHandler(this.buttonAboutClose_Click);
            // 
            // labelAboutLicense
            // 
            this.labelAboutLicense.AutoSize = true;
            this.labelAboutLicense.Location = new System.Drawing.Point(13, 24);
            this.labelAboutLicense.Name = "labelAboutLicense";
            this.labelAboutLicense.Size = new System.Drawing.Size(276, 143);
            this.labelAboutLicense.TabIndex = 1;
            this.labelAboutLicense.Text = resources.GetString("labelAboutLicense.Text");
            this.labelAboutLicense.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAboutLogo
            // 
            this.labelAboutLogo.AutoSize = true;
            this.labelAboutLogo.Location = new System.Drawing.Point(15, 74);
            this.labelAboutLogo.Name = "labelAboutLogo";
            this.labelAboutLogo.Size = new System.Drawing.Size(142, 13);
            this.labelAboutLogo.TabIndex = 2;
            this.labelAboutLogo.Text = "Logo design by chrisbanks2:";
            // 
            // linkLabelAboutLogo
            // 
            this.linkLabelAboutLogo.AutoSize = true;
            this.linkLabelAboutLogo.Location = new System.Drawing.Point(155, 74);
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
            this.labelAboutSpeicalThanks.Location = new System.Drawing.Point(15, 26);
            this.labelAboutSpeicalThanks.Name = "labelAboutSpeicalThanks";
            this.labelAboutSpeicalThanks.Size = new System.Drawing.Size(352, 39);
            this.labelAboutSpeicalThanks.TabIndex = 4;
            this.labelAboutSpeicalThanks.Text = "- Ice Man for testing, reporting bugs and providing ideas\r\n- RedScorpion for runn" +
    "ing the most awesome community on earth\r\n- All the other people who helped impro" +
    "ving this software and like to use it";
            // 
            // linkLabelAboutCommunityLink
            // 
            this.linkLabelAboutCommunityLink.AutoSize = true;
            this.linkLabelAboutCommunityLink.Location = new System.Drawing.Point(327, 38);
            this.linkLabelAboutCommunityLink.Name = "linkLabelAboutCommunityLink";
            this.linkLabelAboutCommunityLink.Size = new System.Drawing.Size(84, 13);
            this.linkLabelAboutCommunityLink.TabIndex = 5;
            this.linkLabelAboutCommunityLink.TabStop = true;
            this.linkLabelAboutCommunityLink.Text = "snes-projects.de";
            this.linkLabelAboutCommunityLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelAboutCommunityLink_LinkClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelAboutLicense);
            this.groupBox1.Location = new System.Drawing.Point(74, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(305, 179);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Disclaimer";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.linkLabelAboutCommunityLink);
            this.groupBox2.Controls.Add(this.labelAboutSpeicalThanks);
            this.groupBox2.Controls.Add(this.linkLabelAboutLogo);
            this.groupBox2.Controls.Add(this.labelAboutLogo);
            this.groupBox2.Location = new System.Drawing.Point(12, 208);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(427, 101);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Credits / Special Thanks";
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 346);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonAboutClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form4";
            this.Text = "About the Advanced SNES ROM Utility v1.0";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAboutClose;
        private System.Windows.Forms.Label labelAboutLicense;
        private System.Windows.Forms.Label labelAboutLogo;
        private System.Windows.Forms.LinkLabel linkLabelAboutLogo;
        private System.Windows.Forms.Label labelAboutSpeicalThanks;
        private System.Windows.Forms.LinkLabel linkLabelAboutCommunityLink;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}