namespace Advanced_SNES_ROM_Utility
{
    partial class FormSlowROMFix
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSlowROMFix));
            this.labelSlowROMFixMessage = new System.Windows.Forms.Label();
            this.buttonYes = new System.Windows.Forms.Button();
            this.buttonNo = new System.Windows.Forms.Button();
            this.checkBoxDontShowAgain = new System.Windows.Forms.CheckBox();
            this.pictureBoxInfo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // labelSlowROMFixMessage
            // 
            this.labelSlowROMFixMessage.AutoSize = true;
            this.labelSlowROMFixMessage.Location = new System.Drawing.Point(69, 9);
            this.labelSlowROMFixMessage.Name = "labelSlowROMFixMessage";
            this.labelSlowROMFixMessage.Size = new System.Drawing.Size(344, 143);
            this.labelSlowROMFixMessage.TabIndex = 0;
            this.labelSlowROMFixMessage.Text = resources.GetString("labelSlowROMFixMessage.Text");
            // 
            // buttonYes
            // 
            this.buttonYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.buttonYes.Location = new System.Drawing.Point(258, 164);
            this.buttonYes.Name = "buttonYes";
            this.buttonYes.Size = new System.Drawing.Size(75, 23);
            this.buttonYes.TabIndex = 1;
            this.buttonYes.Text = "Yes";
            this.buttonYes.UseVisualStyleBackColor = true;
            // 
            // buttonNo
            // 
            this.buttonNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.buttonNo.Location = new System.Drawing.Point(339, 164);
            this.buttonNo.Name = "buttonNo";
            this.buttonNo.Size = new System.Drawing.Size(75, 23);
            this.buttonNo.TabIndex = 2;
            this.buttonNo.Text = "No";
            this.buttonNo.UseVisualStyleBackColor = true;
            // 
            // checkBoxDontShowAgain
            // 
            this.checkBoxDontShowAgain.AutoSize = true;
            this.checkBoxDontShowAgain.Location = new System.Drawing.Point(72, 168);
            this.checkBoxDontShowAgain.Name = "checkBoxDontShowAgain";
            this.checkBoxDontShowAgain.Size = new System.Drawing.Size(172, 17);
            this.checkBoxDontShowAgain.TabIndex = 3;
            this.checkBoxDontShowAgain.Text = "Don\'t show this message again";
            this.checkBoxDontShowAgain.UseVisualStyleBackColor = true;
            this.checkBoxDontShowAgain.CheckedChanged += new System.EventHandler(this.CheckBoxDontShowAgain_CheckedChanged);
            // 
            // pictureBoxInfo
            // 
            this.pictureBoxInfo.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxInfo.Name = "pictureBoxInfo";
            this.pictureBoxInfo.Size = new System.Drawing.Size(50, 50);
            this.pictureBoxInfo.TabIndex = 4;
            this.pictureBoxInfo.TabStop = false;
            // 
            // FormSlowROMFix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 199);
            this.Controls.Add(this.pictureBoxInfo);
            this.Controls.Add(this.checkBoxDontShowAgain);
            this.Controls.Add(this.buttonNo);
            this.Controls.Add(this.buttonYes);
            this.Controls.Add(this.labelSlowROMFixMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSlowROMFix";
            this.Text = "ATTENTION!!!";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSlowROMFixMessage;
        private System.Windows.Forms.Button buttonYes;
        private System.Windows.Forms.Button buttonNo;
        private System.Windows.Forms.CheckBox checkBoxDontShowAgain;
        private System.Windows.Forms.PictureBox pictureBoxInfo;
    }
}