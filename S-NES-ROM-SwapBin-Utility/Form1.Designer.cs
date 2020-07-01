namespace Advanced_SNES_ROM_Utility
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.openROMFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonSelectROM = new System.Windows.Forms.Button();
            this.textBoxROMName = new System.Windows.Forms.TextBox();
            this.buttonSwapBinROM = new System.Windows.Forms.Button();
            this.labelCalcChksm = new System.Windows.Forms.Label();
            this.labelGetCalcChksm = new System.Windows.Forms.Label();
            this.labelIntChksm = new System.Windows.Forms.Label();
            this.labelGetIntChksm = new System.Windows.Forms.Label();
            this.labelCalcInvChksm = new System.Windows.Forms.Label();
            this.labelGetCalcInvChksm = new System.Windows.Forms.Label();
            this.labelIntInvChksm = new System.Windows.Forms.Label();
            this.labelGetIntInvChksm = new System.Windows.Forms.Label();
            this.labelCRC32Chksm = new System.Windows.Forms.Label();
            this.labelGetCRC32Chksm = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelGetTitle = new System.Windows.Forms.Label();
            this.labelSMCHeader = new System.Windows.Forms.Label();
            this.labelCompany = new System.Windows.Forms.Label();
            this.labelCountry = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelRegion = new System.Windows.Forms.Label();
            this.labelGetCompany = new System.Windows.Forms.Label();
            this.labelGetCountry = new System.Windows.Forms.Label();
            this.labelGetRegion = new System.Windows.Forms.Label();
            this.labelGetSMCHeader = new System.Windows.Forms.Label();
            this.labelGetVersion = new System.Windows.Forms.Label();
            this.labelMapMode = new System.Windows.Forms.Label();
            this.labelGetMapMode = new System.Windows.Forms.Label();
            this.labelChksmInf = new System.Windows.Forms.Label();
            this.labelGeneralInfo = new System.Windows.Forms.Label();
            this.labelFunctions = new System.Windows.Forms.Label();
            this.buttonRemoveHeader = new System.Windows.Forms.Button();
            this.buttonAddHeader = new System.Windows.Forms.Button();
            this.labelROMSize = new System.Windows.Forms.Label();
            this.labelGetROMSize = new System.Windows.Forms.Label();
            this.labelROMType = new System.Windows.Forms.Label();
            this.labelGetROMType = new System.Windows.Forms.Label();
            this.labelFileSize = new System.Windows.Forms.Label();
            this.labelGetFileSize = new System.Windows.Forms.Label();
            this.labelROMInfo = new System.Windows.Forms.Label();
            this.labelROMSpeed = new System.Windows.Forms.Label();
            this.labelGetROMSpeed = new System.Windows.Forms.Label();
            this.buttonSplitROM = new System.Windows.Forms.Button();
            this.comboBoxSplitROM = new System.Windows.Forms.ComboBox();
            this.buttonExpandROM = new System.Windows.Forms.Button();
            this.comboBoxExpandROM = new System.Windows.Forms.ComboBox();
            this.buttonFixChksm = new System.Windows.Forms.Button();
            this.buttonFixRegion = new System.Windows.Forms.Button();
            this.labelSRAM = new System.Windows.Forms.Label();
            this.labelGetSRAM = new System.Windows.Forms.Label();
            this.buttonDeinterleave = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAutoFixROM = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openROMFileDialog
            // 
            this.openROMFileDialog.FileName = "openROMFileDialog";
            this.openROMFileDialog.InitialDirectory = "C:\\";
            this.openROMFileDialog.RestoreDirectory = true;
            this.openROMFileDialog.Title = "Select a ROM file";
            // 
            // buttonSelectROM
            // 
            this.buttonSelectROM.Location = new System.Drawing.Point(13, 29);
            this.buttonSelectROM.Name = "buttonSelectROM";
            this.buttonSelectROM.Size = new System.Drawing.Size(516, 23);
            this.buttonSelectROM.TabIndex = 0;
            this.buttonSelectROM.Text = "Select ROM File";
            this.buttonSelectROM.UseVisualStyleBackColor = true;
            this.buttonSelectROM.Click += new System.EventHandler(this.buttonSelectROM_Click);
            // 
            // textBoxROMName
            // 
            this.textBoxROMName.Enabled = false;
            this.textBoxROMName.Location = new System.Drawing.Point(13, 59);
            this.textBoxROMName.Name = "textBoxROMName";
            this.textBoxROMName.Size = new System.Drawing.Size(516, 20);
            this.textBoxROMName.TabIndex = 1;
            this.textBoxROMName.Text = "Select a ROM File!";
            this.textBoxROMName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonSwapBinROM
            // 
            this.buttonSwapBinROM.Location = new System.Drawing.Point(13, 455);
            this.buttonSwapBinROM.Name = "buttonSwapBinROM";
            this.buttonSwapBinROM.Size = new System.Drawing.Size(516, 23);
            this.buttonSwapBinROM.TabIndex = 2;
            this.buttonSwapBinROM.Text = "SwapBin ROM (27C801)";
            this.buttonSwapBinROM.UseVisualStyleBackColor = true;
            this.buttonSwapBinROM.Click += new System.EventHandler(this.ButtonSwapBinROM_Click);
            // 
            // labelCalcChksm
            // 
            this.labelCalcChksm.AutoSize = true;
            this.labelCalcChksm.Location = new System.Drawing.Point(12, 322);
            this.labelCalcChksm.Name = "labelCalcChksm";
            this.labelCalcChksm.Size = new System.Drawing.Size(57, 13);
            this.labelCalcChksm.TabIndex = 3;
            this.labelCalcChksm.Text = "Calculated";
            // 
            // labelGetCalcChksm
            // 
            this.labelGetCalcChksm.AutoSize = true;
            this.labelGetCalcChksm.Location = new System.Drawing.Point(75, 322);
            this.labelGetCalcChksm.Name = "labelGetCalcChksm";
            this.labelGetCalcChksm.Size = new System.Drawing.Size(77, 13);
            this.labelGetCalcChksm.TabIndex = 4;
            this.labelGetCalcChksm.Text = "- Select ROM -";
            // 
            // labelIntChksm
            // 
            this.labelIntChksm.AutoSize = true;
            this.labelIntChksm.Location = new System.Drawing.Point(12, 300);
            this.labelIntChksm.Name = "labelIntChksm";
            this.labelIntChksm.Size = new System.Drawing.Size(42, 13);
            this.labelIntChksm.TabIndex = 5;
            this.labelIntChksm.Text = "Internal";
            // 
            // labelGetIntChksm
            // 
            this.labelGetIntChksm.AutoSize = true;
            this.labelGetIntChksm.Location = new System.Drawing.Point(75, 300);
            this.labelGetIntChksm.Name = "labelGetIntChksm";
            this.labelGetIntChksm.Size = new System.Drawing.Size(77, 13);
            this.labelGetIntChksm.TabIndex = 6;
            this.labelGetIntChksm.Text = "- Select ROM -";
            // 
            // labelCalcInvChksm
            // 
            this.labelCalcInvChksm.AutoSize = true;
            this.labelCalcInvChksm.Location = new System.Drawing.Point(251, 322);
            this.labelCalcInvChksm.Name = "labelCalcInvChksm";
            this.labelCalcInvChksm.Size = new System.Drawing.Size(94, 13);
            this.labelCalcInvChksm.TabIndex = 7;
            this.labelCalcInvChksm.Text = "Calculated inverse";
            // 
            // labelGetCalcInvChksm
            // 
            this.labelGetCalcInvChksm.AutoSize = true;
            this.labelGetCalcInvChksm.Location = new System.Drawing.Point(351, 322);
            this.labelGetCalcInvChksm.Name = "labelGetCalcInvChksm";
            this.labelGetCalcInvChksm.Size = new System.Drawing.Size(77, 13);
            this.labelGetCalcInvChksm.TabIndex = 8;
            this.labelGetCalcInvChksm.Text = "- Select ROM -";
            // 
            // labelIntInvChksm
            // 
            this.labelIntInvChksm.AutoSize = true;
            this.labelIntInvChksm.Location = new System.Drawing.Point(251, 300);
            this.labelIntInvChksm.Name = "labelIntInvChksm";
            this.labelIntInvChksm.Size = new System.Drawing.Size(79, 13);
            this.labelIntInvChksm.TabIndex = 9;
            this.labelIntInvChksm.Text = "Internal inverse";
            // 
            // labelGetIntInvChksm
            // 
            this.labelGetIntInvChksm.AutoSize = true;
            this.labelGetIntInvChksm.Location = new System.Drawing.Point(351, 300);
            this.labelGetIntInvChksm.Name = "labelGetIntInvChksm";
            this.labelGetIntInvChksm.Size = new System.Drawing.Size(77, 13);
            this.labelGetIntInvChksm.TabIndex = 10;
            this.labelGetIntInvChksm.Text = "- Select ROM -";
            // 
            // labelCRC32Chksm
            // 
            this.labelCRC32Chksm.AutoSize = true;
            this.labelCRC32Chksm.Location = new System.Drawing.Point(12, 344);
            this.labelCRC32Chksm.Name = "labelCRC32Chksm";
            this.labelCRC32Chksm.Size = new System.Drawing.Size(41, 13);
            this.labelCRC32Chksm.TabIndex = 11;
            this.labelCRC32Chksm.Text = "CRC32";
            // 
            // labelGetCRC32Chksm
            // 
            this.labelGetCRC32Chksm.AutoSize = true;
            this.labelGetCRC32Chksm.Location = new System.Drawing.Point(75, 344);
            this.labelGetCRC32Chksm.Name = "labelGetCRC32Chksm";
            this.labelGetCRC32Chksm.Size = new System.Drawing.Size(77, 13);
            this.labelGetCRC32Chksm.TabIndex = 12;
            this.labelGetCRC32Chksm.Text = "- Select ROM -";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(10, 126);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(27, 13);
            this.labelTitle.TabIndex = 13;
            this.labelTitle.Text = "Title";
            // 
            // labelGetTitle
            // 
            this.labelGetTitle.AutoSize = true;
            this.labelGetTitle.Location = new System.Drawing.Point(73, 126);
            this.labelGetTitle.Name = "labelGetTitle";
            this.labelGetTitle.Size = new System.Drawing.Size(77, 13);
            this.labelGetTitle.TabIndex = 14;
            this.labelGetTitle.Text = "- Select ROM -";
            // 
            // labelSMCHeader
            // 
            this.labelSMCHeader.AutoSize = true;
            this.labelSMCHeader.Location = new System.Drawing.Point(249, 126);
            this.labelSMCHeader.Name = "labelSMCHeader";
            this.labelSMCHeader.Size = new System.Drawing.Size(68, 13);
            this.labelSMCHeader.TabIndex = 15;
            this.labelSMCHeader.Text = "SMC-Header";
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Location = new System.Drawing.Point(10, 148);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(51, 13);
            this.labelCompany.TabIndex = 16;
            this.labelCompany.Text = "Company";
            // 
            // labelCountry
            // 
            this.labelCountry.AutoSize = true;
            this.labelCountry.Location = new System.Drawing.Point(10, 171);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(43, 13);
            this.labelCountry.TabIndex = 17;
            this.labelCountry.Text = "Country";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(10, 215);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(42, 13);
            this.labelVersion.TabIndex = 18;
            this.labelVersion.Text = "Version";
            // 
            // labelRegion
            // 
            this.labelRegion.AutoSize = true;
            this.labelRegion.Location = new System.Drawing.Point(10, 193);
            this.labelRegion.Name = "labelRegion";
            this.labelRegion.Size = new System.Drawing.Size(41, 13);
            this.labelRegion.TabIndex = 19;
            this.labelRegion.Text = "Region";
            // 
            // labelGetCompany
            // 
            this.labelGetCompany.AutoSize = true;
            this.labelGetCompany.Location = new System.Drawing.Point(73, 148);
            this.labelGetCompany.Name = "labelGetCompany";
            this.labelGetCompany.Size = new System.Drawing.Size(77, 13);
            this.labelGetCompany.TabIndex = 20;
            this.labelGetCompany.Text = "- Select ROM -";
            // 
            // labelGetCountry
            // 
            this.labelGetCountry.AutoSize = true;
            this.labelGetCountry.Location = new System.Drawing.Point(73, 171);
            this.labelGetCountry.Name = "labelGetCountry";
            this.labelGetCountry.Size = new System.Drawing.Size(77, 13);
            this.labelGetCountry.TabIndex = 21;
            this.labelGetCountry.Text = "- Select ROM -";
            // 
            // labelGetRegion
            // 
            this.labelGetRegion.AutoSize = true;
            this.labelGetRegion.Location = new System.Drawing.Point(73, 193);
            this.labelGetRegion.Name = "labelGetRegion";
            this.labelGetRegion.Size = new System.Drawing.Size(77, 13);
            this.labelGetRegion.TabIndex = 22;
            this.labelGetRegion.Text = "- Select ROM -";
            // 
            // labelGetSMCHeader
            // 
            this.labelGetSMCHeader.AutoSize = true;
            this.labelGetSMCHeader.Location = new System.Drawing.Point(349, 126);
            this.labelGetSMCHeader.Name = "labelGetSMCHeader";
            this.labelGetSMCHeader.Size = new System.Drawing.Size(77, 13);
            this.labelGetSMCHeader.TabIndex = 23;
            this.labelGetSMCHeader.Text = "- Select ROM -";
            // 
            // labelGetVersion
            // 
            this.labelGetVersion.AutoSize = true;
            this.labelGetVersion.Location = new System.Drawing.Point(73, 215);
            this.labelGetVersion.Name = "labelGetVersion";
            this.labelGetVersion.Size = new System.Drawing.Size(77, 13);
            this.labelGetVersion.TabIndex = 24;
            this.labelGetVersion.Text = "- Select ROM -";
            // 
            // labelMapMode
            // 
            this.labelMapMode.AutoSize = true;
            this.labelMapMode.Location = new System.Drawing.Point(249, 149);
            this.labelMapMode.Name = "labelMapMode";
            this.labelMapMode.Size = new System.Drawing.Size(58, 13);
            this.labelMapMode.TabIndex = 25;
            this.labelMapMode.Text = "Map Mode";
            // 
            // labelGetMapMode
            // 
            this.labelGetMapMode.AutoSize = true;
            this.labelGetMapMode.Location = new System.Drawing.Point(349, 149);
            this.labelGetMapMode.Name = "labelGetMapMode";
            this.labelGetMapMode.Size = new System.Drawing.Size(77, 13);
            this.labelGetMapMode.TabIndex = 26;
            this.labelGetMapMode.Text = "- Select ROM -";
            // 
            // labelChksmInf
            // 
            this.labelChksmInf.AutoSize = true;
            this.labelChksmInf.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChksmInf.Location = new System.Drawing.Point(12, 271);
            this.labelChksmInf.Name = "labelChksmInf";
            this.labelChksmInf.Size = new System.Drawing.Size(74, 13);
            this.labelChksmInf.TabIndex = 27;
            this.labelChksmInf.Text = "CHECKSUMS";
            // 
            // labelGeneralInfo
            // 
            this.labelGeneralInfo.AutoSize = true;
            this.labelGeneralInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGeneralInfo.Location = new System.Drawing.Point(10, 97);
            this.labelGeneralInfo.Name = "labelGeneralInfo";
            this.labelGeneralInfo.Size = new System.Drawing.Size(136, 13);
            this.labelGeneralInfo.TabIndex = 28;
            this.labelGeneralInfo.Text = "GENERAL INFORMATION";
            // 
            // labelFunctions
            // 
            this.labelFunctions.AutoSize = true;
            this.labelFunctions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFunctions.Location = new System.Drawing.Point(10, 372);
            this.labelFunctions.Name = "labelFunctions";
            this.labelFunctions.Size = new System.Drawing.Size(69, 13);
            this.labelFunctions.TabIndex = 29;
            this.labelFunctions.Text = "FUNCTIONS";
            // 
            // buttonRemoveHeader
            // 
            this.buttonRemoveHeader.Location = new System.Drawing.Point(13, 426);
            this.buttonRemoveHeader.Name = "buttonRemoveHeader";
            this.buttonRemoveHeader.Size = new System.Drawing.Size(516, 23);
            this.buttonRemoveHeader.TabIndex = 30;
            this.buttonRemoveHeader.Text = "Remove Header";
            this.buttonRemoveHeader.UseVisualStyleBackColor = true;
            this.buttonRemoveHeader.Click += new System.EventHandler(this.ButtonRemoveHeader_Click);
            // 
            // buttonAddHeader
            // 
            this.buttonAddHeader.Location = new System.Drawing.Point(13, 397);
            this.buttonAddHeader.Name = "buttonAddHeader";
            this.buttonAddHeader.Size = new System.Drawing.Size(516, 23);
            this.buttonAddHeader.TabIndex = 31;
            this.buttonAddHeader.Text = "Add Header";
            this.buttonAddHeader.UseVisualStyleBackColor = true;
            this.buttonAddHeader.Click += new System.EventHandler(this.ButtonAddHeader_Click);
            // 
            // labelROMSize
            // 
            this.labelROMSize.AutoSize = true;
            this.labelROMSize.Location = new System.Drawing.Point(249, 193);
            this.labelROMSize.Name = "labelROMSize";
            this.labelROMSize.Size = new System.Drawing.Size(55, 13);
            this.labelROMSize.TabIndex = 32;
            this.labelROMSize.Text = "ROM Size";
            // 
            // labelGetROMSize
            // 
            this.labelGetROMSize.AutoSize = true;
            this.labelGetROMSize.Location = new System.Drawing.Point(349, 193);
            this.labelGetROMSize.Name = "labelGetROMSize";
            this.labelGetROMSize.Size = new System.Drawing.Size(77, 13);
            this.labelGetROMSize.TabIndex = 33;
            this.labelGetROMSize.Text = "- Select ROM -";
            // 
            // labelROMType
            // 
            this.labelROMType.AutoSize = true;
            this.labelROMType.Location = new System.Drawing.Point(249, 171);
            this.labelROMType.Name = "labelROMType";
            this.labelROMType.Size = new System.Drawing.Size(59, 13);
            this.labelROMType.TabIndex = 34;
            this.labelROMType.Text = "ROM Type";
            // 
            // labelGetROMType
            // 
            this.labelGetROMType.AutoSize = true;
            this.labelGetROMType.Location = new System.Drawing.Point(349, 171);
            this.labelGetROMType.Name = "labelGetROMType";
            this.labelGetROMType.Size = new System.Drawing.Size(77, 13);
            this.labelGetROMType.TabIndex = 35;
            this.labelGetROMType.Text = "- Select ROM -";
            // 
            // labelFileSize
            // 
            this.labelFileSize.AutoSize = true;
            this.labelFileSize.Location = new System.Drawing.Point(249, 215);
            this.labelFileSize.Name = "labelFileSize";
            this.labelFileSize.Size = new System.Drawing.Size(46, 13);
            this.labelFileSize.TabIndex = 36;
            this.labelFileSize.Text = "File Size";
            // 
            // labelGetFileSize
            // 
            this.labelGetFileSize.AutoSize = true;
            this.labelGetFileSize.Location = new System.Drawing.Point(349, 215);
            this.labelGetFileSize.Name = "labelGetFileSize";
            this.labelGetFileSize.Size = new System.Drawing.Size(77, 13);
            this.labelGetFileSize.TabIndex = 37;
            this.labelGetFileSize.Text = "- Select ROM -";
            // 
            // labelROMInfo
            // 
            this.labelROMInfo.AutoSize = true;
            this.labelROMInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelROMInfo.Location = new System.Drawing.Point(249, 97);
            this.labelROMInfo.Name = "labelROMInfo";
            this.labelROMInfo.Size = new System.Drawing.Size(110, 13);
            this.labelROMInfo.TabIndex = 38;
            this.labelROMInfo.Text = "ROM INFORMATION";
            // 
            // labelROMSpeed
            // 
            this.labelROMSpeed.AutoSize = true;
            this.labelROMSpeed.Location = new System.Drawing.Point(249, 237);
            this.labelROMSpeed.Name = "labelROMSpeed";
            this.labelROMSpeed.Size = new System.Drawing.Size(66, 13);
            this.labelROMSpeed.TabIndex = 39;
            this.labelROMSpeed.Text = "ROM Speed";
            // 
            // labelGetROMSpeed
            // 
            this.labelGetROMSpeed.AutoSize = true;
            this.labelGetROMSpeed.Location = new System.Drawing.Point(349, 237);
            this.labelGetROMSpeed.Name = "labelGetROMSpeed";
            this.labelGetROMSpeed.Size = new System.Drawing.Size(77, 13);
            this.labelGetROMSpeed.TabIndex = 40;
            this.labelGetROMSpeed.Text = "- Select ROM -";
            // 
            // buttonSplitROM
            // 
            this.buttonSplitROM.Location = new System.Drawing.Point(13, 515);
            this.buttonSplitROM.Name = "buttonSplitROM";
            this.buttonSplitROM.Size = new System.Drawing.Size(348, 23);
            this.buttonSplitROM.TabIndex = 41;
            this.buttonSplitROM.Text = "Split ROM";
            this.buttonSplitROM.UseVisualStyleBackColor = true;
            this.buttonSplitROM.Click += new System.EventHandler(this.ButtonSplitROM_Click);
            // 
            // comboBoxSplitROM
            // 
            this.comboBoxSplitROM.FormattingEnabled = true;
            this.comboBoxSplitROM.Location = new System.Drawing.Point(367, 515);
            this.comboBoxSplitROM.Name = "comboBoxSplitROM";
            this.comboBoxSplitROM.Size = new System.Drawing.Size(162, 21);
            this.comboBoxSplitROM.TabIndex = 42;
            // 
            // buttonExpandROM
            // 
            this.buttonExpandROM.Location = new System.Drawing.Point(13, 484);
            this.buttonExpandROM.Name = "buttonExpandROM";
            this.buttonExpandROM.Size = new System.Drawing.Size(348, 23);
            this.buttonExpandROM.TabIndex = 43;
            this.buttonExpandROM.Text = "Expand ROM";
            this.buttonExpandROM.UseVisualStyleBackColor = true;
            this.buttonExpandROM.Click += new System.EventHandler(this.ButtonExpandROM_Click);
            // 
            // comboBoxExpandROM
            // 
            this.comboBoxExpandROM.FormattingEnabled = true;
            this.comboBoxExpandROM.Location = new System.Drawing.Point(367, 484);
            this.comboBoxExpandROM.Name = "comboBoxExpandROM";
            this.comboBoxExpandROM.Size = new System.Drawing.Size(162, 21);
            this.comboBoxExpandROM.TabIndex = 44;
            // 
            // buttonFixChksm
            // 
            this.buttonFixChksm.Location = new System.Drawing.Point(13, 573);
            this.buttonFixChksm.Name = "buttonFixChksm";
            this.buttonFixChksm.Size = new System.Drawing.Size(516, 23);
            this.buttonFixChksm.TabIndex = 45;
            this.buttonFixChksm.Text = "Fix Checksum";
            this.buttonFixChksm.UseVisualStyleBackColor = true;
            this.buttonFixChksm.Click += new System.EventHandler(this.ButtonFixChksm_Click);
            // 
            // buttonFixRegion
            // 
            this.buttonFixRegion.Location = new System.Drawing.Point(13, 603);
            this.buttonFixRegion.Name = "buttonFixRegion";
            this.buttonFixRegion.Size = new System.Drawing.Size(516, 23);
            this.buttonFixRegion.TabIndex = 46;
            this.buttonFixRegion.Text = "Region Unlock";
            this.buttonFixRegion.UseVisualStyleBackColor = true;
            this.buttonFixRegion.Click += new System.EventHandler(this.buttonFixRegion_Click);
            // 
            // labelSRAM
            // 
            this.labelSRAM.AutoSize = true;
            this.labelSRAM.Location = new System.Drawing.Point(249, 260);
            this.labelSRAM.Name = "labelSRAM";
            this.labelSRAM.Size = new System.Drawing.Size(44, 13);
            this.labelSRAM.TabIndex = 47;
            this.labelSRAM.Text = "(S)RAM";
            // 
            // labelGetSRAM
            // 
            this.labelGetSRAM.AutoSize = true;
            this.labelGetSRAM.Location = new System.Drawing.Point(349, 260);
            this.labelGetSRAM.Name = "labelGetSRAM";
            this.labelGetSRAM.Size = new System.Drawing.Size(77, 13);
            this.labelGetSRAM.TabIndex = 48;
            this.labelGetSRAM.Text = "- Select ROM -";
            // 
            // buttonDeinterleave
            // 
            this.buttonDeinterleave.Location = new System.Drawing.Point(13, 544);
            this.buttonDeinterleave.Name = "buttonDeinterleave";
            this.buttonDeinterleave.Size = new System.Drawing.Size(516, 23);
            this.buttonDeinterleave.TabIndex = 49;
            this.buttonDeinterleave.Text = "Deinterleave";
            this.buttonDeinterleave.UseVisualStyleBackColor = true;
            this.buttonDeinterleave.Click += new System.EventHandler(this.buttonDeinterleave_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(13, 237);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(139, 23);
            this.buttonEdit.TabIndex = 50;
            this.buttonEdit.Text = "Edit/Fix ROM Information";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(541, 24);
            this.menuStrip1.TabIndex = 52;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAutoFixROM});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // toolStripMenuItemAutoFixROM
            // 
            this.toolStripMenuItemAutoFixROM.CheckOnClick = true;
            this.toolStripMenuItemAutoFixROM.Name = "toolStripMenuItemAutoFixROM";
            this.toolStripMenuItemAutoFixROM.Size = new System.Drawing.Size(166, 22);
            this.toolStripMenuItemAutoFixROM.Text = "Auto fix Rom size";
            this.toolStripMenuItemAutoFixROM.Click += new System.EventHandler(this.toolStripMenuItemAutoFixROM_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manualToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // manualToolStripMenuItem
            // 
            this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            this.manualToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.manualToolStripMenuItem.Text = "Manual";
            this.manualToolStripMenuItem.Click += new System.EventHandler(this.manualToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 633);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonDeinterleave);
            this.Controls.Add(this.labelGetSRAM);
            this.Controls.Add(this.labelSRAM);
            this.Controls.Add(this.buttonFixRegion);
            this.Controls.Add(this.buttonFixChksm);
            this.Controls.Add(this.comboBoxExpandROM);
            this.Controls.Add(this.buttonExpandROM);
            this.Controls.Add(this.comboBoxSplitROM);
            this.Controls.Add(this.buttonSplitROM);
            this.Controls.Add(this.labelGetROMSpeed);
            this.Controls.Add(this.labelROMSpeed);
            this.Controls.Add(this.labelROMInfo);
            this.Controls.Add(this.labelGetFileSize);
            this.Controls.Add(this.labelFileSize);
            this.Controls.Add(this.labelGetROMType);
            this.Controls.Add(this.labelROMType);
            this.Controls.Add(this.labelGetROMSize);
            this.Controls.Add(this.labelROMSize);
            this.Controls.Add(this.buttonAddHeader);
            this.Controls.Add(this.buttonRemoveHeader);
            this.Controls.Add(this.labelFunctions);
            this.Controls.Add(this.labelGeneralInfo);
            this.Controls.Add(this.labelChksmInf);
            this.Controls.Add(this.labelGetMapMode);
            this.Controls.Add(this.labelMapMode);
            this.Controls.Add(this.labelGetVersion);
            this.Controls.Add(this.labelGetSMCHeader);
            this.Controls.Add(this.labelGetRegion);
            this.Controls.Add(this.labelGetCountry);
            this.Controls.Add(this.labelGetCompany);
            this.Controls.Add(this.labelRegion);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelCountry);
            this.Controls.Add(this.labelCompany);
            this.Controls.Add(this.labelSMCHeader);
            this.Controls.Add(this.labelGetTitle);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.labelGetCRC32Chksm);
            this.Controls.Add(this.labelCRC32Chksm);
            this.Controls.Add(this.labelGetIntInvChksm);
            this.Controls.Add(this.labelIntInvChksm);
            this.Controls.Add(this.labelGetCalcInvChksm);
            this.Controls.Add(this.labelCalcInvChksm);
            this.Controls.Add(this.labelGetIntChksm);
            this.Controls.Add(this.labelIntChksm);
            this.Controls.Add(this.labelGetCalcChksm);
            this.Controls.Add(this.labelCalcChksm);
            this.Controls.Add(this.buttonSwapBinROM);
            this.Controls.Add(this.textBoxROMName);
            this.Controls.Add(this.buttonSelectROM);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Advanced SNES ROM Utility v0.7.4";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openROMFileDialog;
        private System.Windows.Forms.Button buttonSelectROM;
        private System.Windows.Forms.TextBox textBoxROMName;
        private System.Windows.Forms.Button buttonSwapBinROM;
        private System.Windows.Forms.Label labelCalcChksm;
        private System.Windows.Forms.Label labelGetCalcChksm;
        private System.Windows.Forms.Label labelIntChksm;
        private System.Windows.Forms.Label labelGetIntChksm;
        private System.Windows.Forms.Label labelCalcInvChksm;
        private System.Windows.Forms.Label labelGetCalcInvChksm;
        private System.Windows.Forms.Label labelIntInvChksm;
        private System.Windows.Forms.Label labelGetIntInvChksm;
        private System.Windows.Forms.Label labelCRC32Chksm;
        private System.Windows.Forms.Label labelGetCRC32Chksm;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelGetTitle;
        private System.Windows.Forms.Label labelSMCHeader;
        private System.Windows.Forms.Label labelCompany;
        private System.Windows.Forms.Label labelCountry;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelRegion;
        private System.Windows.Forms.Label labelGetCompany;
        private System.Windows.Forms.Label labelGetCountry;
        private System.Windows.Forms.Label labelGetRegion;
        private System.Windows.Forms.Label labelGetSMCHeader;
        private System.Windows.Forms.Label labelGetVersion;
        private System.Windows.Forms.Label labelMapMode;
        private System.Windows.Forms.Label labelGetMapMode;
        private System.Windows.Forms.Label labelChksmInf;
        private System.Windows.Forms.Label labelGeneralInfo;
        private System.Windows.Forms.Label labelFunctions;
        private System.Windows.Forms.Button buttonRemoveHeader;
        private System.Windows.Forms.Button buttonAddHeader;
        private System.Windows.Forms.Label labelROMSize;
        private System.Windows.Forms.Label labelGetROMSize;
        private System.Windows.Forms.Label labelROMType;
        private System.Windows.Forms.Label labelGetROMType;
        private System.Windows.Forms.Label labelFileSize;
        private System.Windows.Forms.Label labelGetFileSize;
        private System.Windows.Forms.Label labelROMInfo;
        private System.Windows.Forms.Label labelROMSpeed;
        private System.Windows.Forms.Label labelGetROMSpeed;
        private System.Windows.Forms.Button buttonSplitROM;
        private System.Windows.Forms.ComboBox comboBoxSplitROM;
        private System.Windows.Forms.Button buttonExpandROM;
        private System.Windows.Forms.ComboBox comboBoxExpandROM;
        private System.Windows.Forms.Button buttonFixChksm;
        private System.Windows.Forms.Button buttonFixRegion;
        private System.Windows.Forms.Label labelSRAM;
        private System.Windows.Forms.Label labelGetSRAM;
        private System.Windows.Forms.Button buttonDeinterleave;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAutoFixROM;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

