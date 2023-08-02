﻿namespace Advanced_SNES_ROM_Utility
{
    partial class FormConvertMapMode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConvertMapMode));
            this.labelConvertMapModeMessage = new System.Windows.Forms.Label();
            this.pictureBoxInfo = new System.Windows.Forms.PictureBox();
            this.buttonYes = new System.Windows.Forms.Button();
            this.buttonNo = new System.Windows.Forms.Button();
            this.checkBoxDontShowAgain = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // labelConvertMapModeMessage
            // 
            this.labelConvertMapModeMessage.AutoSize = true;
            this.labelConvertMapModeMessage.Location = new System.Drawing.Point(68, 9);
            this.labelConvertMapModeMessage.Name = "labelConvertMapModeMessage";
            this.labelConvertMapModeMessage.Size = new System.Drawing.Size(342, 182);
            this.labelConvertMapModeMessage.TabIndex = 0;
            this.labelConvertMapModeMessage.Text = resources.GetString("labelConvertMapModeMessage.Text");
            // 
            // pictureBoxInfo
            // 
            this.pictureBoxInfo.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxInfo.Name = "pictureBoxInfo";
            this.pictureBoxInfo.Size = new System.Drawing.Size(50, 50);
            this.pictureBoxInfo.TabIndex = 1;
            this.pictureBoxInfo.TabStop = false;
            // 
            // buttonYes
            // 
            this.buttonYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.buttonYes.Location = new System.Drawing.Point(258, 208);
            this.buttonYes.Name = "buttonYes";
            this.buttonYes.Size = new System.Drawing.Size(75, 23);
            this.buttonYes.TabIndex = 2;
            this.buttonYes.Text = "Yes";
            this.buttonYes.UseVisualStyleBackColor = true;
            // 
            // buttonNo
            // 
            this.buttonNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.buttonNo.Location = new System.Drawing.Point(339, 208);
            this.buttonNo.Name = "buttonNo";
            this.buttonNo.Size = new System.Drawing.Size(75, 23);
            this.buttonNo.TabIndex = 3;
            this.buttonNo.Text = "No";
            this.buttonNo.UseVisualStyleBackColor = true;
            // 
            // checkBoxDontShowAgain
            // 
            this.checkBoxDontShowAgain.AutoSize = true;
            this.checkBoxDontShowAgain.Location = new System.Drawing.Point(71, 212);
            this.checkBoxDontShowAgain.Name = "checkBoxDontShowAgain";
            this.checkBoxDontShowAgain.Size = new System.Drawing.Size(172, 17);
            this.checkBoxDontShowAgain.TabIndex = 4;
            this.checkBoxDontShowAgain.Text = "Don\'t show this message again";
            this.checkBoxDontShowAgain.UseVisualStyleBackColor = true;
            this.checkBoxDontShowAgain.CheckedChanged += new System.EventHandler(this.CheckBoxDontShowAgain_CheckedChanged);
            // 
            // FormConvertMapMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 240);
            this.Controls.Add(this.checkBoxDontShowAgain);
            this.Controls.Add(this.buttonNo);
            this.Controls.Add(this.buttonYes);
            this.Controls.Add(this.pictureBoxInfo);
            this.Controls.Add(this.labelConvertMapModeMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConvertMapMode";
            this.Text = "ATTENTION!!!";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelConvertMapModeMessage;
        private System.Windows.Forms.PictureBox pictureBoxInfo;
        private System.Windows.Forms.Button buttonYes;
        private System.Windows.Forms.Button buttonNo;
        private System.Windows.Forms.CheckBox checkBoxDontShowAgain;
    }
}