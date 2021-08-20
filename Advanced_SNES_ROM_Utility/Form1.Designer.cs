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
            this.buttonSaveAs = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
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
            this.buttonSelectROM.Location = new System.Drawing.Point(17, 36);
            this.buttonSelectROM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSelectROM.Name = "buttonSelectROM";
            this.buttonSelectROM.Size = new System.Drawing.Size(574, 28);
            this.buttonSelectROM.TabIndex = 0;
            this.buttonSelectROM.Text = "Select ROM File";
            this.buttonSelectROM.UseVisualStyleBackColor = true;
            this.buttonSelectROM.Click += new System.EventHandler(this.buttonSelectROM_Click);
            // 
            // textBoxROMName
            // 
            this.textBoxROMName.Enabled = false;
            this.textBoxROMName.Location = new System.Drawing.Point(17, 73);
            this.textBoxROMName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxROMName.Name = "textBoxROMName";
            this.textBoxROMName.Size = new System.Drawing.Size(573, 22);
            this.textBoxROMName.TabIndex = 1;
            this.textBoxROMName.Text = "Select a ROM File!";
            this.textBoxROMName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonSwapBinROM
            // 
            this.buttonSwapBinROM.Location = new System.Drawing.Point(17, 523);
            this.buttonSwapBinROM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSwapBinROM.Name = "buttonSwapBinROM";
            this.buttonSwapBinROM.Size = new System.Drawing.Size(688, 28);
            this.buttonSwapBinROM.TabIndex = 2;
            this.buttonSwapBinROM.Text = "SwapBin ROM (27C801)";
            this.buttonSwapBinROM.UseVisualStyleBackColor = true;
            this.buttonSwapBinROM.Click += new System.EventHandler(this.ButtonSwapBinROM_Click);
            // 
            // labelCalcChksm
            // 
            this.labelCalcChksm.AutoSize = true;
            this.labelCalcChksm.Location = new System.Drawing.Point(16, 396);
            this.labelCalcChksm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCalcChksm.Name = "labelCalcChksm";
            this.labelCalcChksm.Size = new System.Drawing.Size(74, 17);
            this.labelCalcChksm.TabIndex = 3;
            this.labelCalcChksm.Text = "Calculated";
            // 
            // labelGetCalcChksm
            // 
            this.labelGetCalcChksm.AutoSize = true;
            this.labelGetCalcChksm.Location = new System.Drawing.Point(100, 396);
            this.labelGetCalcChksm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetCalcChksm.Name = "labelGetCalcChksm";
            this.labelGetCalcChksm.Size = new System.Drawing.Size(101, 17);
            this.labelGetCalcChksm.TabIndex = 4;
            this.labelGetCalcChksm.Text = "- Select ROM -";
            // 
            // labelIntChksm
            // 
            this.labelIntChksm.AutoSize = true;
            this.labelIntChksm.Location = new System.Drawing.Point(16, 369);
            this.labelIntChksm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelIntChksm.Name = "labelIntChksm";
            this.labelIntChksm.Size = new System.Drawing.Size(55, 17);
            this.labelIntChksm.TabIndex = 5;
            this.labelIntChksm.Text = "Internal";
            // 
            // labelGetIntChksm
            // 
            this.labelGetIntChksm.AutoSize = true;
            this.labelGetIntChksm.Location = new System.Drawing.Point(100, 369);
            this.labelGetIntChksm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetIntChksm.Name = "labelGetIntChksm";
            this.labelGetIntChksm.Size = new System.Drawing.Size(101, 17);
            this.labelGetIntChksm.TabIndex = 6;
            this.labelGetIntChksm.Text = "- Select ROM -";
            // 
            // labelCalcInvChksm
            // 
            this.labelCalcInvChksm.AutoSize = true;
            this.labelCalcInvChksm.Location = new System.Drawing.Point(335, 396);
            this.labelCalcInvChksm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCalcInvChksm.Name = "labelCalcInvChksm";
            this.labelCalcInvChksm.Size = new System.Drawing.Size(124, 17);
            this.labelCalcInvChksm.TabIndex = 7;
            this.labelCalcInvChksm.Text = "Calculated inverse";
            // 
            // labelGetCalcInvChksm
            // 
            this.labelGetCalcInvChksm.AutoSize = true;
            this.labelGetCalcInvChksm.Location = new System.Drawing.Point(468, 396);
            this.labelGetCalcInvChksm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetCalcInvChksm.Name = "labelGetCalcInvChksm";
            this.labelGetCalcInvChksm.Size = new System.Drawing.Size(101, 17);
            this.labelGetCalcInvChksm.TabIndex = 8;
            this.labelGetCalcInvChksm.Text = "- Select ROM -";
            // 
            // labelIntInvChksm
            // 
            this.labelIntInvChksm.AutoSize = true;
            this.labelIntInvChksm.Location = new System.Drawing.Point(335, 369);
            this.labelIntInvChksm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelIntInvChksm.Name = "labelIntInvChksm";
            this.labelIntInvChksm.Size = new System.Drawing.Size(105, 17);
            this.labelIntInvChksm.TabIndex = 9;
            this.labelIntInvChksm.Text = "Internal inverse";
            // 
            // labelGetIntInvChksm
            // 
            this.labelGetIntInvChksm.AutoSize = true;
            this.labelGetIntInvChksm.Location = new System.Drawing.Point(468, 369);
            this.labelGetIntInvChksm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetIntInvChksm.Name = "labelGetIntInvChksm";
            this.labelGetIntInvChksm.Size = new System.Drawing.Size(101, 17);
            this.labelGetIntInvChksm.TabIndex = 10;
            this.labelGetIntInvChksm.Text = "- Select ROM -";
            // 
            // labelCRC32Chksm
            // 
            this.labelCRC32Chksm.AutoSize = true;
            this.labelCRC32Chksm.Location = new System.Drawing.Point(16, 423);
            this.labelCRC32Chksm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCRC32Chksm.Name = "labelCRC32Chksm";
            this.labelCRC32Chksm.Size = new System.Drawing.Size(52, 17);
            this.labelCRC32Chksm.TabIndex = 11;
            this.labelCRC32Chksm.Text = "CRC32";
            // 
            // labelGetCRC32Chksm
            // 
            this.labelGetCRC32Chksm.AutoSize = true;
            this.labelGetCRC32Chksm.Location = new System.Drawing.Point(100, 423);
            this.labelGetCRC32Chksm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetCRC32Chksm.Name = "labelGetCRC32Chksm";
            this.labelGetCRC32Chksm.Size = new System.Drawing.Size(101, 17);
            this.labelGetCRC32Chksm.TabIndex = 12;
            this.labelGetCRC32Chksm.Text = "- Select ROM -";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(13, 155);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(35, 17);
            this.labelTitle.TabIndex = 13;
            this.labelTitle.Text = "Title";
            // 
            // labelGetTitle
            // 
            this.labelGetTitle.AutoSize = true;
            this.labelGetTitle.Location = new System.Drawing.Point(97, 155);
            this.labelGetTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetTitle.Name = "labelGetTitle";
            this.labelGetTitle.Size = new System.Drawing.Size(101, 17);
            this.labelGetTitle.TabIndex = 14;
            this.labelGetTitle.Text = "- Select ROM -";
            // 
            // labelSMCHeader
            // 
            this.labelSMCHeader.AutoSize = true;
            this.labelSMCHeader.Location = new System.Drawing.Point(332, 155);
            this.labelSMCHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSMCHeader.Name = "labelSMCHeader";
            this.labelSMCHeader.Size = new System.Drawing.Size(89, 17);
            this.labelSMCHeader.TabIndex = 15;
            this.labelSMCHeader.Text = "SMC-Header";
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Location = new System.Drawing.Point(13, 182);
            this.labelCompany.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(67, 17);
            this.labelCompany.TabIndex = 16;
            this.labelCompany.Text = "Company";
            // 
            // labelCountry
            // 
            this.labelCountry.AutoSize = true;
            this.labelCountry.Location = new System.Drawing.Point(13, 210);
            this.labelCountry.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(57, 17);
            this.labelCountry.TabIndex = 17;
            this.labelCountry.Text = "Country";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(13, 265);
            this.labelVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(56, 17);
            this.labelVersion.TabIndex = 18;
            this.labelVersion.Text = "Version";
            // 
            // labelRegion
            // 
            this.labelRegion.AutoSize = true;
            this.labelRegion.Location = new System.Drawing.Point(13, 238);
            this.labelRegion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRegion.Name = "labelRegion";
            this.labelRegion.Size = new System.Drawing.Size(53, 17);
            this.labelRegion.TabIndex = 19;
            this.labelRegion.Text = "Region";
            // 
            // labelGetCompany
            // 
            this.labelGetCompany.AutoSize = true;
            this.labelGetCompany.Location = new System.Drawing.Point(97, 182);
            this.labelGetCompany.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetCompany.Name = "labelGetCompany";
            this.labelGetCompany.Size = new System.Drawing.Size(101, 17);
            this.labelGetCompany.TabIndex = 20;
            this.labelGetCompany.Text = "- Select ROM -";
            // 
            // labelGetCountry
            // 
            this.labelGetCountry.AutoSize = true;
            this.labelGetCountry.Location = new System.Drawing.Point(97, 210);
            this.labelGetCountry.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetCountry.Name = "labelGetCountry";
            this.labelGetCountry.Size = new System.Drawing.Size(101, 17);
            this.labelGetCountry.TabIndex = 21;
            this.labelGetCountry.Text = "- Select ROM -";
            // 
            // labelGetRegion
            // 
            this.labelGetRegion.AutoSize = true;
            this.labelGetRegion.Location = new System.Drawing.Point(97, 238);
            this.labelGetRegion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetRegion.Name = "labelGetRegion";
            this.labelGetRegion.Size = new System.Drawing.Size(101, 17);
            this.labelGetRegion.TabIndex = 22;
            this.labelGetRegion.Text = "- Select ROM -";
            // 
            // labelGetSMCHeader
            // 
            this.labelGetSMCHeader.AutoSize = true;
            this.labelGetSMCHeader.Location = new System.Drawing.Point(465, 155);
            this.labelGetSMCHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetSMCHeader.Name = "labelGetSMCHeader";
            this.labelGetSMCHeader.Size = new System.Drawing.Size(101, 17);
            this.labelGetSMCHeader.TabIndex = 23;
            this.labelGetSMCHeader.Text = "- Select ROM -";
            // 
            // labelGetVersion
            // 
            this.labelGetVersion.AutoSize = true;
            this.labelGetVersion.Location = new System.Drawing.Point(97, 265);
            this.labelGetVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetVersion.Name = "labelGetVersion";
            this.labelGetVersion.Size = new System.Drawing.Size(101, 17);
            this.labelGetVersion.TabIndex = 24;
            this.labelGetVersion.Text = "- Select ROM -";
            // 
            // labelMapMode
            // 
            this.labelMapMode.AutoSize = true;
            this.labelMapMode.Location = new System.Drawing.Point(332, 183);
            this.labelMapMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMapMode.Name = "labelMapMode";
            this.labelMapMode.Size = new System.Drawing.Size(74, 17);
            this.labelMapMode.TabIndex = 25;
            this.labelMapMode.Text = "Map Mode";
            // 
            // labelGetMapMode
            // 
            this.labelGetMapMode.AutoSize = true;
            this.labelGetMapMode.Location = new System.Drawing.Point(465, 183);
            this.labelGetMapMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetMapMode.Name = "labelGetMapMode";
            this.labelGetMapMode.Size = new System.Drawing.Size(101, 17);
            this.labelGetMapMode.TabIndex = 26;
            this.labelGetMapMode.Text = "- Select ROM -";
            // 
            // labelChksmInf
            // 
            this.labelChksmInf.AutoSize = true;
            this.labelChksmInf.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChksmInf.Location = new System.Drawing.Point(16, 334);
            this.labelChksmInf.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelChksmInf.Name = "labelChksmInf";
            this.labelChksmInf.Size = new System.Drawing.Size(93, 17);
            this.labelChksmInf.TabIndex = 27;
            this.labelChksmInf.Text = "CHECKSUMS";
            // 
            // labelGeneralInfo
            // 
            this.labelGeneralInfo.AutoSize = true;
            this.labelGeneralInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGeneralInfo.Location = new System.Drawing.Point(13, 119);
            this.labelGeneralInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGeneralInfo.Name = "labelGeneralInfo";
            this.labelGeneralInfo.Size = new System.Drawing.Size(173, 17);
            this.labelGeneralInfo.TabIndex = 28;
            this.labelGeneralInfo.Text = "GENERAL INFORMATION";
            // 
            // labelFunctions
            // 
            this.labelFunctions.AutoSize = true;
            this.labelFunctions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFunctions.Location = new System.Drawing.Point(13, 458);
            this.labelFunctions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFunctions.Name = "labelFunctions";
            this.labelFunctions.Size = new System.Drawing.Size(87, 17);
            this.labelFunctions.TabIndex = 29;
            this.labelFunctions.Text = "FUNCTIONS";
            // 
            // buttonRemoveHeader
            // 
            this.buttonRemoveHeader.Location = new System.Drawing.Point(365, 487);
            this.buttonRemoveHeader.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonRemoveHeader.Name = "buttonRemoveHeader";
            this.buttonRemoveHeader.Size = new System.Drawing.Size(340, 28);
            this.buttonRemoveHeader.TabIndex = 30;
            this.buttonRemoveHeader.Text = "Remove Header";
            this.buttonRemoveHeader.UseVisualStyleBackColor = true;
            this.buttonRemoveHeader.Click += new System.EventHandler(this.ButtonRemoveHeader_Click);
            // 
            // buttonAddHeader
            // 
            this.buttonAddHeader.Location = new System.Drawing.Point(17, 487);
            this.buttonAddHeader.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonAddHeader.Name = "buttonAddHeader";
            this.buttonAddHeader.Size = new System.Drawing.Size(340, 28);
            this.buttonAddHeader.TabIndex = 31;
            this.buttonAddHeader.Text = "Add Header";
            this.buttonAddHeader.UseVisualStyleBackColor = true;
            this.buttonAddHeader.Click += new System.EventHandler(this.ButtonAddHeader_Click);
            // 
            // labelROMSize
            // 
            this.labelROMSize.AutoSize = true;
            this.labelROMSize.Location = new System.Drawing.Point(332, 238);
            this.labelROMSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelROMSize.Name = "labelROMSize";
            this.labelROMSize.Size = new System.Drawing.Size(71, 17);
            this.labelROMSize.TabIndex = 32;
            this.labelROMSize.Text = "ROM Size";
            // 
            // labelGetROMSize
            // 
            this.labelGetROMSize.AutoSize = true;
            this.labelGetROMSize.Location = new System.Drawing.Point(465, 238);
            this.labelGetROMSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetROMSize.Name = "labelGetROMSize";
            this.labelGetROMSize.Size = new System.Drawing.Size(101, 17);
            this.labelGetROMSize.TabIndex = 33;
            this.labelGetROMSize.Text = "- Select ROM -";
            // 
            // labelROMType
            // 
            this.labelROMType.AutoSize = true;
            this.labelROMType.Location = new System.Drawing.Point(332, 210);
            this.labelROMType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelROMType.Name = "labelROMType";
            this.labelROMType.Size = new System.Drawing.Size(76, 17);
            this.labelROMType.TabIndex = 34;
            this.labelROMType.Text = "ROM Type";
            // 
            // labelGetROMType
            // 
            this.labelGetROMType.AutoSize = true;
            this.labelGetROMType.Location = new System.Drawing.Point(465, 210);
            this.labelGetROMType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetROMType.Name = "labelGetROMType";
            this.labelGetROMType.Size = new System.Drawing.Size(101, 17);
            this.labelGetROMType.TabIndex = 35;
            this.labelGetROMType.Text = "- Select ROM -";
            // 
            // labelFileSize
            // 
            this.labelFileSize.AutoSize = true;
            this.labelFileSize.Location = new System.Drawing.Point(332, 265);
            this.labelFileSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFileSize.Name = "labelFileSize";
            this.labelFileSize.Size = new System.Drawing.Size(61, 17);
            this.labelFileSize.TabIndex = 36;
            this.labelFileSize.Text = "File Size";
            // 
            // labelGetFileSize
            // 
            this.labelGetFileSize.AutoSize = true;
            this.labelGetFileSize.Location = new System.Drawing.Point(465, 265);
            this.labelGetFileSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetFileSize.Name = "labelGetFileSize";
            this.labelGetFileSize.Size = new System.Drawing.Size(101, 17);
            this.labelGetFileSize.TabIndex = 37;
            this.labelGetFileSize.Text = "- Select ROM -";
            // 
            // labelROMInfo
            // 
            this.labelROMInfo.AutoSize = true;
            this.labelROMInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelROMInfo.Location = new System.Drawing.Point(332, 119);
            this.labelROMInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelROMInfo.Name = "labelROMInfo";
            this.labelROMInfo.Size = new System.Drawing.Size(139, 17);
            this.labelROMInfo.TabIndex = 38;
            this.labelROMInfo.Text = "ROM INFORMATION";
            // 
            // labelROMSpeed
            // 
            this.labelROMSpeed.AutoSize = true;
            this.labelROMSpeed.Location = new System.Drawing.Point(332, 292);
            this.labelROMSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelROMSpeed.Name = "labelROMSpeed";
            this.labelROMSpeed.Size = new System.Drawing.Size(85, 17);
            this.labelROMSpeed.TabIndex = 39;
            this.labelROMSpeed.Text = "ROM Speed";
            // 
            // labelGetROMSpeed
            // 
            this.labelGetROMSpeed.AutoSize = true;
            this.labelGetROMSpeed.Location = new System.Drawing.Point(465, 292);
            this.labelGetROMSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetROMSpeed.Name = "labelGetROMSpeed";
            this.labelGetROMSpeed.Size = new System.Drawing.Size(101, 17);
            this.labelGetROMSpeed.TabIndex = 40;
            this.labelGetROMSpeed.Text = "- Select ROM -";
            // 
            // buttonSplitROM
            // 
            this.buttonSplitROM.Location = new System.Drawing.Point(17, 597);
            this.buttonSplitROM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSplitROM.Name = "buttonSplitROM";
            this.buttonSplitROM.Size = new System.Drawing.Size(464, 28);
            this.buttonSplitROM.TabIndex = 41;
            this.buttonSplitROM.Text = "Split ROM";
            this.buttonSplitROM.UseVisualStyleBackColor = true;
            this.buttonSplitROM.Click += new System.EventHandler(this.ButtonSplitROM_Click);
            // 
            // comboBoxSplitROM
            // 
            this.comboBoxSplitROM.FormattingEnabled = true;
            this.comboBoxSplitROM.Location = new System.Drawing.Point(489, 597);
            this.comboBoxSplitROM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxSplitROM.Name = "comboBoxSplitROM";
            this.comboBoxSplitROM.Size = new System.Drawing.Size(215, 24);
            this.comboBoxSplitROM.TabIndex = 42;
            // 
            // buttonExpandROM
            // 
            this.buttonExpandROM.Location = new System.Drawing.Point(17, 559);
            this.buttonExpandROM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonExpandROM.Name = "buttonExpandROM";
            this.buttonExpandROM.Size = new System.Drawing.Size(464, 28);
            this.buttonExpandROM.TabIndex = 43;
            this.buttonExpandROM.Text = "Expand ROM";
            this.buttonExpandROM.UseVisualStyleBackColor = true;
            this.buttonExpandROM.Click += new System.EventHandler(this.ButtonExpandROM_Click);
            // 
            // comboBoxExpandROM
            // 
            this.comboBoxExpandROM.FormattingEnabled = true;
            this.comboBoxExpandROM.Location = new System.Drawing.Point(489, 559);
            this.comboBoxExpandROM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxExpandROM.Name = "comboBoxExpandROM";
            this.comboBoxExpandROM.Size = new System.Drawing.Size(215, 24);
            this.comboBoxExpandROM.TabIndex = 44;
            // 
            // buttonFixChksm
            // 
            this.buttonFixChksm.Location = new System.Drawing.Point(17, 668);
            this.buttonFixChksm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonFixChksm.Name = "buttonFixChksm";
            this.buttonFixChksm.Size = new System.Drawing.Size(688, 28);
            this.buttonFixChksm.TabIndex = 45;
            this.buttonFixChksm.Text = "Fix Checksum";
            this.buttonFixChksm.UseVisualStyleBackColor = true;
            this.buttonFixChksm.Click += new System.EventHandler(this.ButtonFixChksm_Click);
            // 
            // buttonFixRegion
            // 
            this.buttonFixRegion.Location = new System.Drawing.Point(17, 705);
            this.buttonFixRegion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonFixRegion.Name = "buttonFixRegion";
            this.buttonFixRegion.Size = new System.Drawing.Size(688, 28);
            this.buttonFixRegion.TabIndex = 46;
            this.buttonFixRegion.Text = "Region Unlock";
            this.buttonFixRegion.UseVisualStyleBackColor = true;
            this.buttonFixRegion.Click += new System.EventHandler(this.buttonFixRegion_Click);
            // 
            // labelSRAM
            // 
            this.labelSRAM.AutoSize = true;
            this.labelSRAM.Location = new System.Drawing.Point(332, 320);
            this.labelSRAM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSRAM.Name = "labelSRAM";
            this.labelSRAM.Size = new System.Drawing.Size(57, 17);
            this.labelSRAM.TabIndex = 47;
            this.labelSRAM.Text = "(S)RAM";
            // 
            // labelGetSRAM
            // 
            this.labelGetSRAM.AutoSize = true;
            this.labelGetSRAM.Location = new System.Drawing.Point(465, 320);
            this.labelGetSRAM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGetSRAM.Name = "labelGetSRAM";
            this.labelGetSRAM.Size = new System.Drawing.Size(101, 17);
            this.labelGetSRAM.TabIndex = 48;
            this.labelGetSRAM.Text = "- Select ROM -";
            // 
            // buttonDeinterleave
            // 
            this.buttonDeinterleave.Location = new System.Drawing.Point(17, 633);
            this.buttonDeinterleave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonDeinterleave.Name = "buttonDeinterleave";
            this.buttonDeinterleave.Size = new System.Drawing.Size(688, 28);
            this.buttonDeinterleave.TabIndex = 49;
            this.buttonDeinterleave.Text = "Deinterleave";
            this.buttonDeinterleave.UseVisualStyleBackColor = true;
            this.buttonDeinterleave.Click += new System.EventHandler(this.buttonDeinterleave_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(17, 292);
            this.buttonEdit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(185, 28);
            this.buttonEdit.TabIndex = 50;
            this.buttonEdit.Text = "Edit/Fix ROM Information";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(721, 28);
            this.menuStrip1.TabIndex = 52;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAutoFixROM});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(75, 24);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // toolStripMenuItemAutoFixROM
            // 
            this.toolStripMenuItemAutoFixROM.CheckOnClick = true;
            this.toolStripMenuItemAutoFixROM.Name = "toolStripMenuItemAutoFixROM";
            this.toolStripMenuItemAutoFixROM.Size = new System.Drawing.Size(208, 26);
            this.toolStripMenuItemAutoFixROM.Text = "Auto fix Rom size";
            this.toolStripMenuItemAutoFixROM.Click += new System.EventHandler(this.toolStripMenuItemAutoFixROM_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manualToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // manualToolStripMenuItem
            // 
            this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            this.manualToolStripMenuItem.Size = new System.Drawing.Size(141, 26);
            this.manualToolStripMenuItem.Text = "Manual";
            this.manualToolStripMenuItem.Click += new System.EventHandler(this.manualToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(141, 26);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // buttonSaveAs
            // 
            this.buttonSaveAs.Image = global::Advanced_SNES_ROM_Utility.Properties.Resources.save_as_icon;
            this.buttonSaveAs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSaveAs.Location = new System.Drawing.Point(598, 70);
            this.buttonSaveAs.Name = "buttonSaveAs";
            this.buttonSaveAs.Size = new System.Drawing.Size(106, 28);
            this.buttonSaveAs.TabIndex = 54;
            this.buttonSaveAs.Text = "Save As";
            this.buttonSaveAs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSaveAs.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Image = global::Advanced_SNES_ROM_Utility.Properties.Resources.save_icon;
            this.buttonSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSave.Location = new System.Drawing.Point(598, 36);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(106, 28);
            this.buttonSave.TabIndex = 53;
            this.buttonSave.Text = "Save";
            this.buttonSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 744);
            this.Controls.Add(this.buttonSaveAs);
            this.Controls.Add(this.buttonSave);
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
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonSaveAs;
    }
}

