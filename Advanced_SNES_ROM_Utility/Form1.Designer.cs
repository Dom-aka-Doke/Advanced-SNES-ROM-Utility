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
            this.labelSMCHeader = new System.Windows.Forms.Label();
            this.labelCompany = new System.Windows.Forms.Label();
            this.labelCountry = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelGetCompany = new System.Windows.Forms.Label();
            this.labelGetSMCHeader = new System.Windows.Forms.Label();
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
            this.buttonSaveAs = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.textBoxVersion = new System.Windows.Forms.TextBox();
            this.comboBoxCountryRegion = new System.Windows.Forms.ComboBox();
            this.buttonFixROMSize = new System.Windows.Forms.Button();
            this.buttonAbout = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
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
            this.buttonSelectROM.Location = new System.Drawing.Point(13, 11);
            this.buttonSelectROM.Name = "buttonSelectROM";
            this.buttonSelectROM.Size = new System.Drawing.Size(430, 23);
            this.buttonSelectROM.TabIndex = 0;
            this.buttonSelectROM.Text = "Select ROM File";
            this.buttonSelectROM.UseVisualStyleBackColor = true;
            this.buttonSelectROM.Click += new System.EventHandler(this.buttonSelectROM_Click);
            // 
            // textBoxROMName
            // 
            this.textBoxROMName.Enabled = false;
            this.textBoxROMName.Location = new System.Drawing.Point(13, 41);
            this.textBoxROMName.Name = "textBoxROMName";
            this.textBoxROMName.Size = new System.Drawing.Size(431, 20);
            this.textBoxROMName.TabIndex = 1;
            this.textBoxROMName.Text = "Select a ROM File!";
            this.textBoxROMName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonSwapBinROM
            // 
            this.buttonSwapBinROM.Enabled = false;
            this.buttonSwapBinROM.Location = new System.Drawing.Point(13, 496);
            this.buttonSwapBinROM.Name = "buttonSwapBinROM";
            this.buttonSwapBinROM.Size = new System.Drawing.Size(255, 23);
            this.buttonSwapBinROM.TabIndex = 2;
            this.buttonSwapBinROM.Text = "SwapBin ROM (27C801)";
            this.buttonSwapBinROM.UseVisualStyleBackColor = true;
            this.buttonSwapBinROM.Click += new System.EventHandler(this.ButtonSwapBinROM_Click);
            // 
            // labelCalcChksm
            // 
            this.labelCalcChksm.AutoSize = true;
            this.labelCalcChksm.Location = new System.Drawing.Point(12, 304);
            this.labelCalcChksm.Name = "labelCalcChksm";
            this.labelCalcChksm.Size = new System.Drawing.Size(57, 13);
            this.labelCalcChksm.TabIndex = 3;
            this.labelCalcChksm.Text = "Calculated";
            // 
            // labelGetCalcChksm
            // 
            this.labelGetCalcChksm.AutoSize = true;
            this.labelGetCalcChksm.Location = new System.Drawing.Point(75, 304);
            this.labelGetCalcChksm.Name = "labelGetCalcChksm";
            this.labelGetCalcChksm.Size = new System.Drawing.Size(77, 13);
            this.labelGetCalcChksm.TabIndex = 4;
            this.labelGetCalcChksm.Text = "- Select ROM -";
            // 
            // labelIntChksm
            // 
            this.labelIntChksm.AutoSize = true;
            this.labelIntChksm.Location = new System.Drawing.Point(12, 282);
            this.labelIntChksm.Name = "labelIntChksm";
            this.labelIntChksm.Size = new System.Drawing.Size(42, 13);
            this.labelIntChksm.TabIndex = 5;
            this.labelIntChksm.Text = "Internal";
            // 
            // labelGetIntChksm
            // 
            this.labelGetIntChksm.AutoSize = true;
            this.labelGetIntChksm.Location = new System.Drawing.Point(75, 282);
            this.labelGetIntChksm.Name = "labelGetIntChksm";
            this.labelGetIntChksm.Size = new System.Drawing.Size(77, 13);
            this.labelGetIntChksm.TabIndex = 6;
            this.labelGetIntChksm.Text = "- Select ROM -";
            // 
            // labelCalcInvChksm
            // 
            this.labelCalcInvChksm.AutoSize = true;
            this.labelCalcInvChksm.Location = new System.Drawing.Point(251, 304);
            this.labelCalcInvChksm.Name = "labelCalcInvChksm";
            this.labelCalcInvChksm.Size = new System.Drawing.Size(94, 13);
            this.labelCalcInvChksm.TabIndex = 7;
            this.labelCalcInvChksm.Text = "Calculated inverse";
            // 
            // labelGetCalcInvChksm
            // 
            this.labelGetCalcInvChksm.AutoSize = true;
            this.labelGetCalcInvChksm.Location = new System.Drawing.Point(351, 304);
            this.labelGetCalcInvChksm.Name = "labelGetCalcInvChksm";
            this.labelGetCalcInvChksm.Size = new System.Drawing.Size(77, 13);
            this.labelGetCalcInvChksm.TabIndex = 8;
            this.labelGetCalcInvChksm.Text = "- Select ROM -";
            // 
            // labelIntInvChksm
            // 
            this.labelIntInvChksm.AutoSize = true;
            this.labelIntInvChksm.Location = new System.Drawing.Point(251, 282);
            this.labelIntInvChksm.Name = "labelIntInvChksm";
            this.labelIntInvChksm.Size = new System.Drawing.Size(79, 13);
            this.labelIntInvChksm.TabIndex = 9;
            this.labelIntInvChksm.Text = "Internal inverse";
            // 
            // labelGetIntInvChksm
            // 
            this.labelGetIntInvChksm.AutoSize = true;
            this.labelGetIntInvChksm.Location = new System.Drawing.Point(351, 282);
            this.labelGetIntInvChksm.Name = "labelGetIntInvChksm";
            this.labelGetIntInvChksm.Size = new System.Drawing.Size(77, 13);
            this.labelGetIntInvChksm.TabIndex = 10;
            this.labelGetIntInvChksm.Text = "- Select ROM -";
            // 
            // labelCRC32Chksm
            // 
            this.labelCRC32Chksm.AutoSize = true;
            this.labelCRC32Chksm.Location = new System.Drawing.Point(12, 326);
            this.labelCRC32Chksm.Name = "labelCRC32Chksm";
            this.labelCRC32Chksm.Size = new System.Drawing.Size(41, 13);
            this.labelCRC32Chksm.TabIndex = 11;
            this.labelCRC32Chksm.Text = "CRC32";
            // 
            // labelGetCRC32Chksm
            // 
            this.labelGetCRC32Chksm.AutoSize = true;
            this.labelGetCRC32Chksm.Location = new System.Drawing.Point(75, 326);
            this.labelGetCRC32Chksm.Name = "labelGetCRC32Chksm";
            this.labelGetCRC32Chksm.Size = new System.Drawing.Size(77, 13);
            this.labelGetCRC32Chksm.TabIndex = 12;
            this.labelGetCRC32Chksm.Text = "- Select ROM -";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(10, 108);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(27, 13);
            this.labelTitle.TabIndex = 13;
            this.labelTitle.Text = "Title";
            // 
            // labelSMCHeader
            // 
            this.labelSMCHeader.AutoSize = true;
            this.labelSMCHeader.Location = new System.Drawing.Point(249, 108);
            this.labelSMCHeader.Name = "labelSMCHeader";
            this.labelSMCHeader.Size = new System.Drawing.Size(68, 13);
            this.labelSMCHeader.TabIndex = 15;
            this.labelSMCHeader.Text = "SMC-Header";
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Location = new System.Drawing.Point(10, 130);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(51, 13);
            this.labelCompany.TabIndex = 16;
            this.labelCompany.Text = "Company";
            // 
            // labelCountry
            // 
            this.labelCountry.AutoSize = true;
            this.labelCountry.Location = new System.Drawing.Point(10, 153);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(43, 13);
            this.labelCountry.TabIndex = 17;
            this.labelCountry.Text = "Country";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(10, 180);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(42, 13);
            this.labelVersion.TabIndex = 18;
            this.labelVersion.Text = "Version";
            // 
            // labelGetCompany
            // 
            this.labelGetCompany.AutoSize = true;
            this.labelGetCompany.Location = new System.Drawing.Point(73, 130);
            this.labelGetCompany.Name = "labelGetCompany";
            this.labelGetCompany.Size = new System.Drawing.Size(77, 13);
            this.labelGetCompany.TabIndex = 20;
            this.labelGetCompany.Text = "- Select ROM -";
            // 
            // labelGetSMCHeader
            // 
            this.labelGetSMCHeader.AutoSize = true;
            this.labelGetSMCHeader.Location = new System.Drawing.Point(349, 108);
            this.labelGetSMCHeader.Name = "labelGetSMCHeader";
            this.labelGetSMCHeader.Size = new System.Drawing.Size(77, 13);
            this.labelGetSMCHeader.TabIndex = 23;
            this.labelGetSMCHeader.Text = "- Select ROM -";
            // 
            // labelMapMode
            // 
            this.labelMapMode.AutoSize = true;
            this.labelMapMode.Location = new System.Drawing.Point(249, 131);
            this.labelMapMode.Name = "labelMapMode";
            this.labelMapMode.Size = new System.Drawing.Size(58, 13);
            this.labelMapMode.TabIndex = 25;
            this.labelMapMode.Text = "Map Mode";
            // 
            // labelGetMapMode
            // 
            this.labelGetMapMode.AutoSize = true;
            this.labelGetMapMode.Location = new System.Drawing.Point(349, 131);
            this.labelGetMapMode.Name = "labelGetMapMode";
            this.labelGetMapMode.Size = new System.Drawing.Size(77, 13);
            this.labelGetMapMode.TabIndex = 26;
            this.labelGetMapMode.Text = "- Select ROM -";
            // 
            // labelChksmInf
            // 
            this.labelChksmInf.AutoSize = true;
            this.labelChksmInf.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChksmInf.Location = new System.Drawing.Point(12, 253);
            this.labelChksmInf.Name = "labelChksmInf";
            this.labelChksmInf.Size = new System.Drawing.Size(74, 13);
            this.labelChksmInf.TabIndex = 27;
            this.labelChksmInf.Text = "CHECKSUMS";
            // 
            // labelGeneralInfo
            // 
            this.labelGeneralInfo.AutoSize = true;
            this.labelGeneralInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGeneralInfo.Location = new System.Drawing.Point(10, 79);
            this.labelGeneralInfo.Name = "labelGeneralInfo";
            this.labelGeneralInfo.Size = new System.Drawing.Size(136, 13);
            this.labelGeneralInfo.TabIndex = 28;
            this.labelGeneralInfo.Text = "GENERAL INFORMATION";
            // 
            // labelFunctions
            // 
            this.labelFunctions.AutoSize = true;
            this.labelFunctions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFunctions.Location = new System.Drawing.Point(10, 354);
            this.labelFunctions.Name = "labelFunctions";
            this.labelFunctions.Size = new System.Drawing.Size(69, 13);
            this.labelFunctions.TabIndex = 29;
            this.labelFunctions.Text = "FUNCTIONS";
            // 
            // buttonRemoveHeader
            // 
            this.buttonRemoveHeader.Enabled = false;
            this.buttonRemoveHeader.Location = new System.Drawing.Point(278, 377);
            this.buttonRemoveHeader.Name = "buttonRemoveHeader";
            this.buttonRemoveHeader.Size = new System.Drawing.Size(255, 23);
            this.buttonRemoveHeader.TabIndex = 30;
            this.buttonRemoveHeader.Text = "Remove Header";
            this.buttonRemoveHeader.UseVisualStyleBackColor = true;
            this.buttonRemoveHeader.Click += new System.EventHandler(this.ButtonRemoveHeader_Click);
            // 
            // buttonAddHeader
            // 
            this.buttonAddHeader.Enabled = false;
            this.buttonAddHeader.Location = new System.Drawing.Point(13, 377);
            this.buttonAddHeader.Name = "buttonAddHeader";
            this.buttonAddHeader.Size = new System.Drawing.Size(255, 23);
            this.buttonAddHeader.TabIndex = 31;
            this.buttonAddHeader.Text = "Add Header";
            this.buttonAddHeader.UseVisualStyleBackColor = true;
            this.buttonAddHeader.Click += new System.EventHandler(this.ButtonAddHeader_Click);
            // 
            // labelROMSize
            // 
            this.labelROMSize.AutoSize = true;
            this.labelROMSize.Location = new System.Drawing.Point(249, 175);
            this.labelROMSize.Name = "labelROMSize";
            this.labelROMSize.Size = new System.Drawing.Size(55, 13);
            this.labelROMSize.TabIndex = 32;
            this.labelROMSize.Text = "ROM Size";
            // 
            // labelGetROMSize
            // 
            this.labelGetROMSize.AutoSize = true;
            this.labelGetROMSize.Location = new System.Drawing.Point(349, 175);
            this.labelGetROMSize.Name = "labelGetROMSize";
            this.labelGetROMSize.Size = new System.Drawing.Size(77, 13);
            this.labelGetROMSize.TabIndex = 33;
            this.labelGetROMSize.Text = "- Select ROM -";
            // 
            // labelROMType
            // 
            this.labelROMType.AutoSize = true;
            this.labelROMType.Location = new System.Drawing.Point(249, 153);
            this.labelROMType.Name = "labelROMType";
            this.labelROMType.Size = new System.Drawing.Size(59, 13);
            this.labelROMType.TabIndex = 34;
            this.labelROMType.Text = "ROM Type";
            // 
            // labelGetROMType
            // 
            this.labelGetROMType.AutoSize = true;
            this.labelGetROMType.Location = new System.Drawing.Point(349, 153);
            this.labelGetROMType.Name = "labelGetROMType";
            this.labelGetROMType.Size = new System.Drawing.Size(77, 13);
            this.labelGetROMType.TabIndex = 35;
            this.labelGetROMType.Text = "- Select ROM -";
            // 
            // labelFileSize
            // 
            this.labelFileSize.AutoSize = true;
            this.labelFileSize.Location = new System.Drawing.Point(249, 197);
            this.labelFileSize.Name = "labelFileSize";
            this.labelFileSize.Size = new System.Drawing.Size(46, 13);
            this.labelFileSize.TabIndex = 36;
            this.labelFileSize.Text = "File Size";
            // 
            // labelGetFileSize
            // 
            this.labelGetFileSize.AutoSize = true;
            this.labelGetFileSize.Location = new System.Drawing.Point(349, 197);
            this.labelGetFileSize.Name = "labelGetFileSize";
            this.labelGetFileSize.Size = new System.Drawing.Size(77, 13);
            this.labelGetFileSize.TabIndex = 37;
            this.labelGetFileSize.Text = "- Select ROM -";
            // 
            // labelROMInfo
            // 
            this.labelROMInfo.AutoSize = true;
            this.labelROMInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelROMInfo.Location = new System.Drawing.Point(249, 79);
            this.labelROMInfo.Name = "labelROMInfo";
            this.labelROMInfo.Size = new System.Drawing.Size(110, 13);
            this.labelROMInfo.TabIndex = 38;
            this.labelROMInfo.Text = "ROM INFORMATION";
            // 
            // labelROMSpeed
            // 
            this.labelROMSpeed.AutoSize = true;
            this.labelROMSpeed.Location = new System.Drawing.Point(249, 219);
            this.labelROMSpeed.Name = "labelROMSpeed";
            this.labelROMSpeed.Size = new System.Drawing.Size(66, 13);
            this.labelROMSpeed.TabIndex = 39;
            this.labelROMSpeed.Text = "ROM Speed";
            // 
            // labelGetROMSpeed
            // 
            this.labelGetROMSpeed.AutoSize = true;
            this.labelGetROMSpeed.Location = new System.Drawing.Point(349, 219);
            this.labelGetROMSpeed.Name = "labelGetROMSpeed";
            this.labelGetROMSpeed.Size = new System.Drawing.Size(77, 13);
            this.labelGetROMSpeed.TabIndex = 40;
            this.labelGetROMSpeed.Text = "- Select ROM -";
            // 
            // buttonSplitROM
            // 
            this.buttonSplitROM.Enabled = false;
            this.buttonSplitROM.Location = new System.Drawing.Point(13, 467);
            this.buttonSplitROM.Name = "buttonSplitROM";
            this.buttonSplitROM.Size = new System.Drawing.Size(348, 23);
            this.buttonSplitROM.TabIndex = 41;
            this.buttonSplitROM.Text = "Split ROM";
            this.buttonSplitROM.UseVisualStyleBackColor = true;
            this.buttonSplitROM.Click += new System.EventHandler(this.ButtonSplitROM_Click);
            // 
            // comboBoxSplitROM
            // 
            this.comboBoxSplitROM.Enabled = false;
            this.comboBoxSplitROM.FormattingEnabled = true;
            this.comboBoxSplitROM.Location = new System.Drawing.Point(371, 467);
            this.comboBoxSplitROM.Name = "comboBoxSplitROM";
            this.comboBoxSplitROM.Size = new System.Drawing.Size(162, 21);
            this.comboBoxSplitROM.TabIndex = 42;
            // 
            // buttonExpandROM
            // 
            this.buttonExpandROM.Enabled = false;
            this.buttonExpandROM.Location = new System.Drawing.Point(13, 436);
            this.buttonExpandROM.Name = "buttonExpandROM";
            this.buttonExpandROM.Size = new System.Drawing.Size(348, 23);
            this.buttonExpandROM.TabIndex = 43;
            this.buttonExpandROM.Text = "Expand ROM";
            this.buttonExpandROM.UseVisualStyleBackColor = true;
            this.buttonExpandROM.Click += new System.EventHandler(this.ButtonExpandROM_Click);
            // 
            // comboBoxExpandROM
            // 
            this.comboBoxExpandROM.Enabled = false;
            this.comboBoxExpandROM.FormattingEnabled = true;
            this.comboBoxExpandROM.Location = new System.Drawing.Point(371, 436);
            this.comboBoxExpandROM.Name = "comboBoxExpandROM";
            this.comboBoxExpandROM.Size = new System.Drawing.Size(162, 21);
            this.comboBoxExpandROM.TabIndex = 44;
            // 
            // buttonFixChksm
            // 
            this.buttonFixChksm.Enabled = false;
            this.buttonFixChksm.Location = new System.Drawing.Point(13, 406);
            this.buttonFixChksm.Name = "buttonFixChksm";
            this.buttonFixChksm.Size = new System.Drawing.Size(255, 23);
            this.buttonFixChksm.TabIndex = 45;
            this.buttonFixChksm.Text = "Fix Checksum";
            this.buttonFixChksm.UseVisualStyleBackColor = true;
            this.buttonFixChksm.Click += new System.EventHandler(this.ButtonFixChksm_Click);
            // 
            // buttonFixRegion
            // 
            this.buttonFixRegion.Enabled = false;
            this.buttonFixRegion.Location = new System.Drawing.Point(13, 525);
            this.buttonFixRegion.Name = "buttonFixRegion";
            this.buttonFixRegion.Size = new System.Drawing.Size(520, 23);
            this.buttonFixRegion.TabIndex = 46;
            this.buttonFixRegion.Text = "Region Unlock";
            this.buttonFixRegion.UseVisualStyleBackColor = true;
            this.buttonFixRegion.Click += new System.EventHandler(this.buttonFixRegion_Click);
            // 
            // labelSRAM
            // 
            this.labelSRAM.AutoSize = true;
            this.labelSRAM.Location = new System.Drawing.Point(249, 242);
            this.labelSRAM.Name = "labelSRAM";
            this.labelSRAM.Size = new System.Drawing.Size(44, 13);
            this.labelSRAM.TabIndex = 47;
            this.labelSRAM.Text = "(S)RAM";
            // 
            // labelGetSRAM
            // 
            this.labelGetSRAM.AutoSize = true;
            this.labelGetSRAM.Location = new System.Drawing.Point(349, 242);
            this.labelGetSRAM.Name = "labelGetSRAM";
            this.labelGetSRAM.Size = new System.Drawing.Size(77, 13);
            this.labelGetSRAM.TabIndex = 48;
            this.labelGetSRAM.Text = "- Select ROM -";
            // 
            // buttonDeinterleave
            // 
            this.buttonDeinterleave.Enabled = false;
            this.buttonDeinterleave.Location = new System.Drawing.Point(278, 496);
            this.buttonDeinterleave.Name = "buttonDeinterleave";
            this.buttonDeinterleave.Size = new System.Drawing.Size(255, 23);
            this.buttonDeinterleave.TabIndex = 49;
            this.buttonDeinterleave.Text = "Deinterleave";
            this.buttonDeinterleave.UseVisualStyleBackColor = true;
            this.buttonDeinterleave.Click += new System.EventHandler(this.buttonDeinterleave_Click);
            // 
            // buttonSaveAs
            // 
            this.buttonSaveAs.Enabled = false;
            this.buttonSaveAs.Image = global::Advanced_SNES_ROM_Utility.Properties.Resources.save_as_icon;
            this.buttonSaveAs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSaveAs.Location = new System.Drawing.Point(448, 39);
            this.buttonSaveAs.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSaveAs.Name = "buttonSaveAs";
            this.buttonSaveAs.Size = new System.Drawing.Size(80, 23);
            this.buttonSaveAs.TabIndex = 54;
            this.buttonSaveAs.Text = "Save As";
            this.buttonSaveAs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSaveAs.UseVisualStyleBackColor = true;
            this.buttonSaveAs.Click += new System.EventHandler(this.buttonSaveAs_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Enabled = false;
            this.buttonSave.Image = global::Advanced_SNES_ROM_Utility.Properties.Resources.save_icon;
            this.buttonSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSave.Location = new System.Drawing.Point(448, 11);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(80, 23);
            this.buttonSave.TabIndex = 53;
            this.buttonSave.Text = "Save";
            this.buttonSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Enabled = false;
            this.textBoxTitle.Location = new System.Drawing.Point(76, 105);
            this.textBoxTitle.MaxLength = 21;
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(167, 20);
            this.textBoxTitle.TabIndex = 55;
            this.textBoxTitle.TextChanged += new System.EventHandler(this.textBoxGetTitle_TextChanged);
            // 
            // textBoxVersion
            // 
            this.textBoxVersion.Enabled = false;
            this.textBoxVersion.Location = new System.Drawing.Point(76, 177);
            this.textBoxVersion.MaxLength = 5;
            this.textBoxVersion.Name = "textBoxVersion";
            this.textBoxVersion.Size = new System.Drawing.Size(45, 20);
            this.textBoxVersion.TabIndex = 56;
            this.textBoxVersion.Leave += new System.EventHandler(this.textBoxGetVersion_Leave);
            // 
            // comboBoxCountryRegion
            // 
            this.comboBoxCountryRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCountryRegion.Enabled = false;
            this.comboBoxCountryRegion.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBoxCountryRegion.FormattingEnabled = true;
            this.comboBoxCountryRegion.Location = new System.Drawing.Point(76, 150);
            this.comboBoxCountryRegion.Name = "comboBoxCountryRegion";
            this.comboBoxCountryRegion.Size = new System.Drawing.Size(167, 21);
            this.comboBoxCountryRegion.TabIndex = 57;
            this.comboBoxCountryRegion.SelectedIndexChanged += new System.EventHandler(this.comboBoxCountryRegion_SelectedIndexChanged);
            // 
            // buttonFixROMSize
            // 
            this.buttonFixROMSize.Enabled = false;
            this.buttonFixROMSize.Location = new System.Drawing.Point(278, 406);
            this.buttonFixROMSize.Name = "buttonFixROMSize";
            this.buttonFixROMSize.Size = new System.Drawing.Size(255, 23);
            this.buttonFixROMSize.TabIndex = 58;
            this.buttonFixROMSize.Text = "Fix Internal ROM Size";
            this.buttonFixROMSize.UseVisualStyleBackColor = true;
            this.buttonFixROMSize.Click += new System.EventHandler(this.buttonFixROMSize_Click);
            // 
            // buttonAbout
            // 
            this.buttonAbout.Location = new System.Drawing.Point(408, 348);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(125, 23);
            this.buttonAbout.TabIndex = 59;
            this.buttonAbout.Text = "About";
            this.buttonAbout.UseVisualStyleBackColor = true;
            this.buttonAbout.Click += new System.EventHandler(this.buttonAbout_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Location = new System.Drawing.Point(278, 348);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(125, 23);
            this.buttonHelp.TabIndex = 60;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 558);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonAbout);
            this.Controls.Add(this.buttonFixROMSize);
            this.Controls.Add(this.comboBoxCountryRegion);
            this.Controls.Add(this.textBoxVersion);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.buttonSaveAs);
            this.Controls.Add(this.buttonSave);
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
            this.Controls.Add(this.labelGetSMCHeader);
            this.Controls.Add(this.labelGetCompany);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelCountry);
            this.Controls.Add(this.labelCompany);
            this.Controls.Add(this.labelSMCHeader);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Advanced SNES ROM Utility v0.8";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
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
        private System.Windows.Forms.Label labelSMCHeader;
        private System.Windows.Forms.Label labelCompany;
        private System.Windows.Forms.Label labelCountry;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelGetCompany;
        private System.Windows.Forms.Label labelGetSMCHeader;
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
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonSaveAs;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.TextBox textBoxVersion;
        private System.Windows.Forms.ComboBox comboBoxCountryRegion;
        private System.Windows.Forms.Button buttonFixROMSize;
        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.Button buttonHelp;
    }
}

