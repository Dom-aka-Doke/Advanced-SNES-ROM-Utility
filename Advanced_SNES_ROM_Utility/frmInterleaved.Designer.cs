namespace Advanced_SNES_ROM_Utility
{
    partial class frmInterleaved
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInterleaved));
            this.buttonUFO = new System.Windows.Forms.Button();
            this.buttonStandard = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonGD = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonUFO
            // 
            this.buttonUFO.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.buttonUFO.Location = new System.Drawing.Point(12, 174);
            this.buttonUFO.Name = "buttonUFO";
            this.buttonUFO.Size = new System.Drawing.Size(429, 23);
            this.buttonUFO.TabIndex = 1;
            this.buttonUFO.Text = "Super UFO";
            this.buttonUFO.UseVisualStyleBackColor = true;
            // 
            // buttonStandard
            // 
            this.buttonStandard.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.buttonStandard.Location = new System.Drawing.Point(12, 203);
            this.buttonStandard.Name = "buttonStandard";
            this.buttonStandard.Size = new System.Drawing.Size(429, 23);
            this.buttonStandard.TabIndex = 2;
            this.buttonStandard.Text = "Standard Deinterleaving";
            this.buttonStandard.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(430, 117);
            this.label1.TabIndex = 3;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // buttonGD
            // 
            this.buttonGD.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonGD.Location = new System.Drawing.Point(12, 145);
            this.buttonGD.Name = "buttonGD";
            this.buttonGD.Size = new System.Drawing.Size(429, 23);
            this.buttonGD.TabIndex = 4;
            this.buttonGD.Text = "Game Doctor SF";
            this.buttonGD.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(12, 233);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(429, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 266);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonGD);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonStandard);
            this.Controls.Add(this.buttonUFO);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.Text = "ATTENTION!!!";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonUFO;
        private System.Windows.Forms.Button buttonStandard;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonGD;
        private System.Windows.Forms.Button buttonCancel;
    }
}