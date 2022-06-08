namespace Advanced_SNES_ROM_Utility
{
    partial class FormCompanySelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCompanySelect));
            this.labelChangeCompany = new System.Windows.Forms.Label();
            this.comboBoxChangeCompany = new System.Windows.Forms.ComboBox();
            this.buttonCancelChages = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelChangeCompany
            // 
            this.labelChangeCompany.AutoSize = true;
            this.labelChangeCompany.Location = new System.Drawing.Point(12, 12);
            this.labelChangeCompany.Name = "labelChangeCompany";
            this.labelChangeCompany.Size = new System.Drawing.Size(51, 13);
            this.labelChangeCompany.TabIndex = 1;
            this.labelChangeCompany.Text = "Company";
            // 
            // comboBoxChangeCompany
            // 
            this.comboBoxChangeCompany.FormattingEnabled = true;
            this.comboBoxChangeCompany.Location = new System.Drawing.Point(111, 9);
            this.comboBoxChangeCompany.Name = "comboBoxChangeCompany";
            this.comboBoxChangeCompany.Size = new System.Drawing.Size(231, 21);
            this.comboBoxChangeCompany.TabIndex = 5;
            // 
            // buttonCancelChages
            // 
            this.buttonCancelChages.Location = new System.Drawing.Point(15, 36);
            this.buttonCancelChages.Name = "buttonCancelChages";
            this.buttonCancelChages.Size = new System.Drawing.Size(327, 23);
            this.buttonCancelChages.TabIndex = 9;
            this.buttonCancelChages.Text = "Cancel";
            this.buttonCancelChages.UseVisualStyleBackColor = true;
            this.buttonCancelChages.Click += new System.EventHandler(this.buttonCancelChages_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 68);
            this.Controls.Add(this.buttonCancelChages);
            this.Controls.Add(this.comboBoxChangeCompany);
            this.Controls.Add(this.labelChangeCompany);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form3";
            this.Text = "Edit ROM Information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelChangeCompany;
        private System.Windows.Forms.ComboBox comboBoxChangeCompany;
        private System.Windows.Forms.Button buttonCancelChages;
    }
}