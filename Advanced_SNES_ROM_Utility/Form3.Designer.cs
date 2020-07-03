namespace Advanced_SNES_ROM_Utility
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.labelChangeTitle = new System.Windows.Forms.Label();
            this.labelChangeCompany = new System.Windows.Forms.Label();
            this.labelChangeCountryRegion = new System.Windows.Forms.Label();
            this.labelChangeVersion = new System.Windows.Forms.Label();
            this.textBoxChangeTitle = new System.Windows.Forms.TextBox();
            this.comboBoxChangeCompany = new System.Windows.Forms.ComboBox();
            this.comboBoxChangeCountryRegion = new System.Windows.Forms.ComboBox();
            this.buttonSaveChanges = new System.Windows.Forms.Button();
            this.buttonCancelChages = new System.Windows.Forms.Button();
            this.textBoxChangeVersion = new System.Windows.Forms.TextBox();
            this.checkBoxFixSize = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelChangeTitle
            // 
            this.labelChangeTitle.AutoSize = true;
            this.labelChangeTitle.Location = new System.Drawing.Point(12, 21);
            this.labelChangeTitle.Name = "labelChangeTitle";
            this.labelChangeTitle.Size = new System.Drawing.Size(27, 13);
            this.labelChangeTitle.TabIndex = 0;
            this.labelChangeTitle.Text = "Title";
            // 
            // labelChangeCompany
            // 
            this.labelChangeCompany.AutoSize = true;
            this.labelChangeCompany.Location = new System.Drawing.Point(12, 48);
            this.labelChangeCompany.Name = "labelChangeCompany";
            this.labelChangeCompany.Size = new System.Drawing.Size(51, 13);
            this.labelChangeCompany.TabIndex = 1;
            this.labelChangeCompany.Text = "Company";
            // 
            // labelChangeCountryRegion
            // 
            this.labelChangeCountryRegion.AutoSize = true;
            this.labelChangeCountryRegion.Location = new System.Drawing.Point(12, 76);
            this.labelChangeCountryRegion.Name = "labelChangeCountryRegion";
            this.labelChangeCountryRegion.Size = new System.Drawing.Size(88, 13);
            this.labelChangeCountryRegion.TabIndex = 2;
            this.labelChangeCountryRegion.Text = "Country / Region";
            // 
            // labelChangeVersion
            // 
            this.labelChangeVersion.AutoSize = true;
            this.labelChangeVersion.Location = new System.Drawing.Point(12, 104);
            this.labelChangeVersion.Name = "labelChangeVersion";
            this.labelChangeVersion.Size = new System.Drawing.Size(42, 13);
            this.labelChangeVersion.TabIndex = 3;
            this.labelChangeVersion.Text = "Version";
            // 
            // textBoxChangeTitle
            // 
            this.textBoxChangeTitle.Location = new System.Drawing.Point(111, 18);
            this.textBoxChangeTitle.MaxLength = 21;
            this.textBoxChangeTitle.Name = "textBoxChangeTitle";
            this.textBoxChangeTitle.Size = new System.Drawing.Size(231, 20);
            this.textBoxChangeTitle.TabIndex = 4;
            // 
            // comboBoxChangeCompany
            // 
            this.comboBoxChangeCompany.FormattingEnabled = true;
            this.comboBoxChangeCompany.Location = new System.Drawing.Point(111, 45);
            this.comboBoxChangeCompany.Name = "comboBoxChangeCompany";
            this.comboBoxChangeCompany.Size = new System.Drawing.Size(231, 21);
            this.comboBoxChangeCompany.TabIndex = 5;
            // 
            // comboBoxChangeCountryRegion
            // 
            this.comboBoxChangeCountryRegion.FormattingEnabled = true;
            this.comboBoxChangeCountryRegion.Location = new System.Drawing.Point(111, 73);
            this.comboBoxChangeCountryRegion.Name = "comboBoxChangeCountryRegion";
            this.comboBoxChangeCountryRegion.Size = new System.Drawing.Size(231, 21);
            this.comboBoxChangeCountryRegion.TabIndex = 6;
            // 
            // buttonSaveChanges
            // 
            this.buttonSaveChanges.Location = new System.Drawing.Point(15, 137);
            this.buttonSaveChanges.Name = "buttonSaveChanges";
            this.buttonSaveChanges.Size = new System.Drawing.Size(327, 23);
            this.buttonSaveChanges.TabIndex = 8;
            this.buttonSaveChanges.Text = "Save Changes";
            this.buttonSaveChanges.UseVisualStyleBackColor = true;
            this.buttonSaveChanges.Click += new System.EventHandler(this.buttonSaveChanges_Click);
            // 
            // buttonCancelChages
            // 
            this.buttonCancelChages.Location = new System.Drawing.Point(15, 166);
            this.buttonCancelChages.Name = "buttonCancelChages";
            this.buttonCancelChages.Size = new System.Drawing.Size(327, 23);
            this.buttonCancelChages.TabIndex = 9;
            this.buttonCancelChages.Text = "Cancel";
            this.buttonCancelChages.UseVisualStyleBackColor = true;
            this.buttonCancelChages.Click += new System.EventHandler(this.buttonCancelChages_Click);
            // 
            // textBoxChangeVersion
            // 
            this.textBoxChangeVersion.Location = new System.Drawing.Point(111, 100);
            this.textBoxChangeVersion.MaxLength = 5;
            this.textBoxChangeVersion.Name = "textBoxChangeVersion";
            this.textBoxChangeVersion.Size = new System.Drawing.Size(47, 20);
            this.textBoxChangeVersion.TabIndex = 10;
            // 
            // checkBoxFixSize
            // 
            this.checkBoxFixSize.AutoSize = true;
            this.checkBoxFixSize.Location = new System.Drawing.Point(200, 102);
            this.checkBoxFixSize.Name = "checkBoxFixSize";
            this.checkBoxFixSize.Size = new System.Drawing.Size(142, 17);
            this.checkBoxFixSize.TabIndex = 11;
            this.checkBoxFixSize.Text = "Fix ROM size information";
            this.checkBoxFixSize.UseVisualStyleBackColor = true;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 199);
            this.Controls.Add(this.checkBoxFixSize);
            this.Controls.Add(this.textBoxChangeVersion);
            this.Controls.Add(this.buttonCancelChages);
            this.Controls.Add(this.buttonSaveChanges);
            this.Controls.Add(this.comboBoxChangeCountryRegion);
            this.Controls.Add(this.comboBoxChangeCompany);
            this.Controls.Add(this.textBoxChangeTitle);
            this.Controls.Add(this.labelChangeVersion);
            this.Controls.Add(this.labelChangeCountryRegion);
            this.Controls.Add(this.labelChangeCompany);
            this.Controls.Add(this.labelChangeTitle);
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

        private System.Windows.Forms.Label labelChangeTitle;
        private System.Windows.Forms.Label labelChangeCompany;
        private System.Windows.Forms.Label labelChangeCountryRegion;
        private System.Windows.Forms.Label labelChangeVersion;
        private System.Windows.Forms.TextBox textBoxChangeTitle;
        private System.Windows.Forms.ComboBox comboBoxChangeCompany;
        private System.Windows.Forms.ComboBox comboBoxChangeCountryRegion;
        private System.Windows.Forms.Button buttonSaveChanges;
        private System.Windows.Forms.Button buttonCancelChages;
        private System.Windows.Forms.TextBox textBoxChangeVersion;
        private System.Windows.Forms.CheckBox checkBoxFixSize;
    }
}