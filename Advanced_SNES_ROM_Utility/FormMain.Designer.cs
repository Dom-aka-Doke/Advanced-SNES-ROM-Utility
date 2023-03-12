namespace Advanced_SNES_ROM_Utility
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
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
            this.buttonRemoveHeader = new System.Windows.Forms.Button();
            this.buttonAddHeader = new System.Windows.Forms.Button();
            this.labelROMSize = new System.Windows.Forms.Label();
            this.labelGetROMSize = new System.Windows.Forms.Label();
            this.labelROMType = new System.Windows.Forms.Label();
            this.labelGetROMType = new System.Windows.Forms.Label();
            this.labelFileSize = new System.Windows.Forms.Label();
            this.labelGetFileSize = new System.Windows.Forms.Label();
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
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.textBoxVersion = new System.Windows.Forms.TextBox();
            this.comboBoxCountryRegion = new System.Windows.Forms.ComboBox();
            this.buttonFixROMSize = new System.Windows.Forms.Button();
            this.buttonPatch = new System.Windows.Forms.Button();
            this.buttonSaveAs = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.labelCode = new System.Windows.Forms.Label();
            this.groupBoxChecksums = new System.Windows.Forms.GroupBox();
            this.groupBoxROMInfo = new System.Windows.Forms.GroupBox();
            this.buttonFixSlowROMChecks = new System.Windows.Forms.Button();
            this.buttonFixSRAMChecks = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBoxExpandMirroring = new System.Windows.Forms.CheckBox();
            this.groupBoxOptions = new System.Windows.Forms.GroupBox();
            this.checkBoxScan = new System.Windows.Forms.CheckBox();
            this.buttonExit = new System.Windows.Forms.Button();
            this.groupBoxInfo.SuspendLayout();
            this.groupBoxChecksums.SuspendLayout();
            this.groupBoxROMInfo.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBoxOptions.SuspendLayout();
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
            this.buttonSelectROM.Location = new System.Drawing.Point(12, 27);
            this.buttonSelectROM.Name = "buttonSelectROM";
            this.buttonSelectROM.Size = new System.Drawing.Size(430, 23);
            this.buttonSelectROM.TabIndex = 0;
            this.buttonSelectROM.Text = "Select ROM File";
            this.buttonSelectROM.UseVisualStyleBackColor = true;
            this.buttonSelectROM.Click += new System.EventHandler(this.ButtonSelectROM_Click);
            // 
            // textBoxROMName
            // 
            this.textBoxROMName.Enabled = false;
            this.textBoxROMName.Location = new System.Drawing.Point(12, 57);
            this.textBoxROMName.Name = "textBoxROMName";
            this.textBoxROMName.Size = new System.Drawing.Size(431, 20);
            this.textBoxROMName.TabIndex = 1;
            this.textBoxROMName.Text = "Select a ROM File!";
            this.textBoxROMName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonSwapBinROM
            // 
            this.buttonSwapBinROM.Enabled = false;
            this.buttonSwapBinROM.Location = new System.Drawing.Point(363, 293);
            this.buttonSwapBinROM.Name = "buttonSwapBinROM";
            this.buttonSwapBinROM.Size = new System.Drawing.Size(169, 23);
            this.buttonSwapBinROM.TabIndex = 2;
            this.buttonSwapBinROM.Text = "SwapBin ROM (27C801)";
            this.buttonSwapBinROM.UseVisualStyleBackColor = true;
            this.buttonSwapBinROM.Click += new System.EventHandler(this.ButtonSwapBinROM_Click);
            // 
            // labelCalcChksm
            // 
            this.labelCalcChksm.AutoSize = true;
            this.labelCalcChksm.Location = new System.Drawing.Point(6, 47);
            this.labelCalcChksm.Name = "labelCalcChksm";
            this.labelCalcChksm.Size = new System.Drawing.Size(57, 13);
            this.labelCalcChksm.TabIndex = 3;
            this.labelCalcChksm.Text = "Calculated";
            // 
            // labelGetCalcChksm
            // 
            this.labelGetCalcChksm.AutoSize = true;
            this.labelGetCalcChksm.Location = new System.Drawing.Point(69, 47);
            this.labelGetCalcChksm.Name = "labelGetCalcChksm";
            this.labelGetCalcChksm.Size = new System.Drawing.Size(77, 13);
            this.labelGetCalcChksm.TabIndex = 4;
            this.labelGetCalcChksm.Text = "- Select ROM -";
            this.labelGetCalcChksm.Click += new System.EventHandler(this.CopyToClipboard_Click);
            // 
            // labelIntChksm
            // 
            this.labelIntChksm.AutoSize = true;
            this.labelIntChksm.Location = new System.Drawing.Point(6, 25);
            this.labelIntChksm.Name = "labelIntChksm";
            this.labelIntChksm.Size = new System.Drawing.Size(42, 13);
            this.labelIntChksm.TabIndex = 5;
            this.labelIntChksm.Text = "Internal";
            // 
            // labelGetIntChksm
            // 
            this.labelGetIntChksm.AutoSize = true;
            this.labelGetIntChksm.Location = new System.Drawing.Point(69, 25);
            this.labelGetIntChksm.Name = "labelGetIntChksm";
            this.labelGetIntChksm.Size = new System.Drawing.Size(77, 13);
            this.labelGetIntChksm.TabIndex = 6;
            this.labelGetIntChksm.Text = "- Select ROM -";
            this.labelGetIntChksm.Click += new System.EventHandler(this.CopyToClipboard_Click);
            // 
            // labelCalcInvChksm
            // 
            this.labelCalcInvChksm.AutoSize = true;
            this.labelCalcInvChksm.Location = new System.Drawing.Point(160, 47);
            this.labelCalcInvChksm.Name = "labelCalcInvChksm";
            this.labelCalcInvChksm.Size = new System.Drawing.Size(94, 13);
            this.labelCalcInvChksm.TabIndex = 7;
            this.labelCalcInvChksm.Text = "Calculated inverse";
            // 
            // labelGetCalcInvChksm
            // 
            this.labelGetCalcInvChksm.AutoSize = true;
            this.labelGetCalcInvChksm.Location = new System.Drawing.Point(260, 47);
            this.labelGetCalcInvChksm.Name = "labelGetCalcInvChksm";
            this.labelGetCalcInvChksm.Size = new System.Drawing.Size(77, 13);
            this.labelGetCalcInvChksm.TabIndex = 8;
            this.labelGetCalcInvChksm.Text = "- Select ROM -";
            this.labelGetCalcInvChksm.Click += new System.EventHandler(this.CopyToClipboard_Click);
            // 
            // labelIntInvChksm
            // 
            this.labelIntInvChksm.AutoSize = true;
            this.labelIntInvChksm.Location = new System.Drawing.Point(160, 25);
            this.labelIntInvChksm.Name = "labelIntInvChksm";
            this.labelIntInvChksm.Size = new System.Drawing.Size(79, 13);
            this.labelIntInvChksm.TabIndex = 9;
            this.labelIntInvChksm.Text = "Internal inverse";
            // 
            // labelGetIntInvChksm
            // 
            this.labelGetIntInvChksm.AutoSize = true;
            this.labelGetIntInvChksm.Location = new System.Drawing.Point(260, 25);
            this.labelGetIntInvChksm.Name = "labelGetIntInvChksm";
            this.labelGetIntInvChksm.Size = new System.Drawing.Size(77, 13);
            this.labelGetIntInvChksm.TabIndex = 10;
            this.labelGetIntInvChksm.Text = "- Select ROM -";
            this.labelGetIntInvChksm.Click += new System.EventHandler(this.CopyToClipboard_Click);
            // 
            // labelCRC32Chksm
            // 
            this.labelCRC32Chksm.AutoSize = true;
            this.labelCRC32Chksm.Location = new System.Drawing.Point(6, 69);
            this.labelCRC32Chksm.Name = "labelCRC32Chksm";
            this.labelCRC32Chksm.Size = new System.Drawing.Size(41, 13);
            this.labelCRC32Chksm.TabIndex = 11;
            this.labelCRC32Chksm.Text = "CRC32";
            // 
            // labelGetCRC32Chksm
            // 
            this.labelGetCRC32Chksm.AutoSize = true;
            this.labelGetCRC32Chksm.Location = new System.Drawing.Point(69, 69);
            this.labelGetCRC32Chksm.Name = "labelGetCRC32Chksm";
            this.labelGetCRC32Chksm.Size = new System.Drawing.Size(77, 13);
            this.labelGetCRC32Chksm.TabIndex = 12;
            this.labelGetCRC32Chksm.Text = "- Select ROM -";
            this.labelGetCRC32Chksm.Click += new System.EventHandler(this.CopyToClipboard_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(6, 27);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(27, 13);
            this.labelTitle.TabIndex = 13;
            this.labelTitle.Text = "Title";
            // 
            // labelSMCHeader
            // 
            this.labelSMCHeader.AutoSize = true;
            this.labelSMCHeader.Location = new System.Drawing.Point(6, 25);
            this.labelSMCHeader.Name = "labelSMCHeader";
            this.labelSMCHeader.Size = new System.Drawing.Size(68, 13);
            this.labelSMCHeader.TabIndex = 15;
            this.labelSMCHeader.Text = "SMC-Header";
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Location = new System.Drawing.Point(6, 49);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(51, 13);
            this.labelCompany.TabIndex = 16;
            this.labelCompany.Text = "Company";
            // 
            // labelCountry
            // 
            this.labelCountry.AutoSize = true;
            this.labelCountry.Location = new System.Drawing.Point(6, 72);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(43, 13);
            this.labelCountry.TabIndex = 17;
            this.labelCountry.Text = "Country";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(6, 99);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(42, 13);
            this.labelVersion.TabIndex = 18;
            this.labelVersion.Text = "Version";
            // 
            // labelGetCompany
            // 
            this.labelGetCompany.AutoSize = true;
            this.labelGetCompany.Location = new System.Drawing.Point(69, 49);
            this.labelGetCompany.Name = "labelGetCompany";
            this.labelGetCompany.Size = new System.Drawing.Size(77, 13);
            this.labelGetCompany.TabIndex = 20;
            this.labelGetCompany.Text = "- Select ROM -";
            // 
            // labelGetSMCHeader
            // 
            this.labelGetSMCHeader.AutoSize = true;
            this.labelGetSMCHeader.Location = new System.Drawing.Point(106, 25);
            this.labelGetSMCHeader.Name = "labelGetSMCHeader";
            this.labelGetSMCHeader.Size = new System.Drawing.Size(77, 13);
            this.labelGetSMCHeader.TabIndex = 23;
            this.labelGetSMCHeader.Text = "- Select ROM -";
            // 
            // labelMapMode
            // 
            this.labelMapMode.AutoSize = true;
            this.labelMapMode.Location = new System.Drawing.Point(6, 48);
            this.labelMapMode.Name = "labelMapMode";
            this.labelMapMode.Size = new System.Drawing.Size(58, 13);
            this.labelMapMode.TabIndex = 25;
            this.labelMapMode.Text = "Map Mode";
            // 
            // labelGetMapMode
            // 
            this.labelGetMapMode.AutoSize = true;
            this.labelGetMapMode.Location = new System.Drawing.Point(106, 48);
            this.labelGetMapMode.Name = "labelGetMapMode";
            this.labelGetMapMode.Size = new System.Drawing.Size(77, 13);
            this.labelGetMapMode.TabIndex = 26;
            this.labelGetMapMode.Text = "- Select ROM -";
            // 
            // buttonRemoveHeader
            // 
            this.buttonRemoveHeader.Enabled = false;
            this.buttonRemoveHeader.Location = new System.Drawing.Point(405, 88);
            this.buttonRemoveHeader.Name = "buttonRemoveHeader";
            this.buttonRemoveHeader.Size = new System.Drawing.Size(127, 23);
            this.buttonRemoveHeader.TabIndex = 30;
            this.buttonRemoveHeader.Text = "Remove Header";
            this.buttonRemoveHeader.UseVisualStyleBackColor = true;
            this.buttonRemoveHeader.Click += new System.EventHandler(this.ButtonRemoveHeader_Click);
            // 
            // buttonAddHeader
            // 
            this.buttonAddHeader.Enabled = false;
            this.buttonAddHeader.Location = new System.Drawing.Point(268, 88);
            this.buttonAddHeader.Name = "buttonAddHeader";
            this.buttonAddHeader.Size = new System.Drawing.Size(127, 23);
            this.buttonAddHeader.TabIndex = 31;
            this.buttonAddHeader.Text = "Add Header";
            this.buttonAddHeader.UseVisualStyleBackColor = true;
            this.buttonAddHeader.Click += new System.EventHandler(this.ButtonAddHeader_Click);
            // 
            // labelROMSize
            // 
            this.labelROMSize.AutoSize = true;
            this.labelROMSize.Location = new System.Drawing.Point(6, 92);
            this.labelROMSize.Name = "labelROMSize";
            this.labelROMSize.Size = new System.Drawing.Size(55, 13);
            this.labelROMSize.TabIndex = 32;
            this.labelROMSize.Text = "ROM Size";
            // 
            // labelGetROMSize
            // 
            this.labelGetROMSize.AutoSize = true;
            this.labelGetROMSize.Location = new System.Drawing.Point(106, 92);
            this.labelGetROMSize.Name = "labelGetROMSize";
            this.labelGetROMSize.Size = new System.Drawing.Size(77, 13);
            this.labelGetROMSize.TabIndex = 33;
            this.labelGetROMSize.Text = "- Select ROM -";
            // 
            // labelROMType
            // 
            this.labelROMType.AutoSize = true;
            this.labelROMType.Location = new System.Drawing.Point(6, 70);
            this.labelROMType.Name = "labelROMType";
            this.labelROMType.Size = new System.Drawing.Size(59, 13);
            this.labelROMType.TabIndex = 34;
            this.labelROMType.Text = "ROM Type";
            // 
            // labelGetROMType
            // 
            this.labelGetROMType.AutoSize = true;
            this.labelGetROMType.Location = new System.Drawing.Point(106, 70);
            this.labelGetROMType.Name = "labelGetROMType";
            this.labelGetROMType.Size = new System.Drawing.Size(77, 13);
            this.labelGetROMType.TabIndex = 35;
            this.labelGetROMType.Text = "- Select ROM -";
            // 
            // labelFileSize
            // 
            this.labelFileSize.AutoSize = true;
            this.labelFileSize.Location = new System.Drawing.Point(6, 114);
            this.labelFileSize.Name = "labelFileSize";
            this.labelFileSize.Size = new System.Drawing.Size(46, 13);
            this.labelFileSize.TabIndex = 36;
            this.labelFileSize.Text = "File Size";
            // 
            // labelGetFileSize
            // 
            this.labelGetFileSize.AutoSize = true;
            this.labelGetFileSize.Location = new System.Drawing.Point(106, 114);
            this.labelGetFileSize.Name = "labelGetFileSize";
            this.labelGetFileSize.Size = new System.Drawing.Size(77, 13);
            this.labelGetFileSize.TabIndex = 37;
            this.labelGetFileSize.Text = "- Select ROM -";
            // 
            // labelROMSpeed
            // 
            this.labelROMSpeed.AutoSize = true;
            this.labelROMSpeed.Location = new System.Drawing.Point(6, 136);
            this.labelROMSpeed.Name = "labelROMSpeed";
            this.labelROMSpeed.Size = new System.Drawing.Size(66, 13);
            this.labelROMSpeed.TabIndex = 39;
            this.labelROMSpeed.Text = "ROM Speed";
            // 
            // labelGetROMSpeed
            // 
            this.labelGetROMSpeed.AutoSize = true;
            this.labelGetROMSpeed.Location = new System.Drawing.Point(106, 136);
            this.labelGetROMSpeed.Name = "labelGetROMSpeed";
            this.labelGetROMSpeed.Size = new System.Drawing.Size(77, 13);
            this.labelGetROMSpeed.TabIndex = 40;
            this.labelGetROMSpeed.Text = "- Select ROM -";
            // 
            // buttonSplitROM
            // 
            this.buttonSplitROM.Enabled = false;
            this.buttonSplitROM.Location = new System.Drawing.Point(268, 187);
            this.buttonSplitROM.Name = "buttonSplitROM";
            this.buttonSplitROM.Size = new System.Drawing.Size(92, 23);
            this.buttonSplitROM.TabIndex = 41;
            this.buttonSplitROM.Text = "Split ROM";
            this.buttonSplitROM.UseVisualStyleBackColor = true;
            this.buttonSplitROM.Click += new System.EventHandler(this.ButtonSplitROM_Click);
            // 
            // comboBoxSplitROM
            // 
            this.comboBoxSplitROM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSplitROM.Enabled = false;
            this.comboBoxSplitROM.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBoxSplitROM.FormattingEnabled = true;
            this.comboBoxSplitROM.Location = new System.Drawing.Point(366, 187);
            this.comboBoxSplitROM.Name = "comboBoxSplitROM";
            this.comboBoxSplitROM.Size = new System.Drawing.Size(168, 21);
            this.comboBoxSplitROM.TabIndex = 42;
            // 
            // buttonExpandROM
            // 
            this.buttonExpandROM.Enabled = false;
            this.buttonExpandROM.Location = new System.Drawing.Point(268, 158);
            this.buttonExpandROM.Name = "buttonExpandROM";
            this.buttonExpandROM.Size = new System.Drawing.Size(92, 23);
            this.buttonExpandROM.TabIndex = 43;
            this.buttonExpandROM.Text = "Expand ROM";
            this.buttonExpandROM.UseVisualStyleBackColor = true;
            this.buttonExpandROM.Click += new System.EventHandler(this.ButtonExpandROM_Click);
            // 
            // comboBoxExpandROM
            // 
            this.comboBoxExpandROM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxExpandROM.Enabled = false;
            this.comboBoxExpandROM.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBoxExpandROM.FormattingEnabled = true;
            this.comboBoxExpandROM.Location = new System.Drawing.Point(366, 158);
            this.comboBoxExpandROM.Name = "comboBoxExpandROM";
            this.comboBoxExpandROM.Size = new System.Drawing.Size(168, 21);
            this.comboBoxExpandROM.TabIndex = 44;
            // 
            // buttonFixChksm
            // 
            this.buttonFixChksm.Enabled = false;
            this.buttonFixChksm.Location = new System.Drawing.Point(268, 117);
            this.buttonFixChksm.Name = "buttonFixChksm";
            this.buttonFixChksm.Size = new System.Drawing.Size(127, 23);
            this.buttonFixChksm.TabIndex = 45;
            this.buttonFixChksm.Text = "Fix Checksum";
            this.buttonFixChksm.UseVisualStyleBackColor = true;
            this.buttonFixChksm.Click += new System.EventHandler(this.ButtonFixChksm_Click);
            // 
            // buttonFixRegion
            // 
            this.buttonFixRegion.Enabled = false;
            this.buttonFixRegion.Location = new System.Drawing.Point(363, 351);
            this.buttonFixRegion.Name = "buttonFixRegion";
            this.buttonFixRegion.Size = new System.Drawing.Size(169, 23);
            this.buttonFixRegion.TabIndex = 46;
            this.buttonFixRegion.Text = "Remove Region Locks";
            this.buttonFixRegion.UseVisualStyleBackColor = true;
            this.buttonFixRegion.Click += new System.EventHandler(this.ButtonFixRegion_Click);
            // 
            // labelSRAM
            // 
            this.labelSRAM.AutoSize = true;
            this.labelSRAM.Location = new System.Drawing.Point(6, 159);
            this.labelSRAM.Name = "labelSRAM";
            this.labelSRAM.Size = new System.Drawing.Size(44, 13);
            this.labelSRAM.TabIndex = 47;
            this.labelSRAM.Text = "(S)RAM";
            // 
            // labelGetSRAM
            // 
            this.labelGetSRAM.AutoSize = true;
            this.labelGetSRAM.Location = new System.Drawing.Point(106, 159);
            this.labelGetSRAM.Name = "labelGetSRAM";
            this.labelGetSRAM.Size = new System.Drawing.Size(77, 13);
            this.labelGetSRAM.TabIndex = 48;
            this.labelGetSRAM.Text = "- Select ROM -";
            // 
            // buttonDeinterleave
            // 
            this.buttonDeinterleave.Enabled = false;
            this.buttonDeinterleave.Location = new System.Drawing.Point(363, 322);
            this.buttonDeinterleave.Name = "buttonDeinterleave";
            this.buttonDeinterleave.Size = new System.Drawing.Size(169, 23);
            this.buttonDeinterleave.TabIndex = 49;
            this.buttonDeinterleave.Text = "Deinterleave";
            this.buttonDeinterleave.UseVisualStyleBackColor = true;
            this.buttonDeinterleave.Click += new System.EventHandler(this.ButtonDeinterleave_Click);
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Enabled = false;
            this.textBoxTitle.Location = new System.Drawing.Point(72, 24);
            this.textBoxTitle.MaxLength = 21;
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(167, 20);
            this.textBoxTitle.TabIndex = 55;
            this.textBoxTitle.TextChanged += new System.EventHandler(this.TextBoxGetTitle_TextChanged);
            // 
            // textBoxVersion
            // 
            this.textBoxVersion.Enabled = false;
            this.textBoxVersion.Location = new System.Drawing.Point(72, 96);
            this.textBoxVersion.MaxLength = 5;
            this.textBoxVersion.Name = "textBoxVersion";
            this.textBoxVersion.Size = new System.Drawing.Size(45, 20);
            this.textBoxVersion.TabIndex = 56;
            this.textBoxVersion.Leave += new System.EventHandler(this.TextBoxGetVersion_Leave);
            // 
            // comboBoxCountryRegion
            // 
            this.comboBoxCountryRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCountryRegion.Enabled = false;
            this.comboBoxCountryRegion.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBoxCountryRegion.FormattingEnabled = true;
            this.comboBoxCountryRegion.Location = new System.Drawing.Point(72, 69);
            this.comboBoxCountryRegion.Name = "comboBoxCountryRegion";
            this.comboBoxCountryRegion.Size = new System.Drawing.Size(167, 21);
            this.comboBoxCountryRegion.TabIndex = 57;
            this.comboBoxCountryRegion.SelectedIndexChanged += new System.EventHandler(this.ComboBoxCountryRegion_SelectedIndexChanged);
            // 
            // buttonFixROMSize
            // 
            this.buttonFixROMSize.Enabled = false;
            this.buttonFixROMSize.Location = new System.Drawing.Point(405, 117);
            this.buttonFixROMSize.Name = "buttonFixROMSize";
            this.buttonFixROMSize.Size = new System.Drawing.Size(127, 23);
            this.buttonFixROMSize.TabIndex = 58;
            this.buttonFixROMSize.Text = "Fix Internal ROM Size";
            this.buttonFixROMSize.UseVisualStyleBackColor = true;
            this.buttonFixROMSize.Click += new System.EventHandler(this.ButtonFixROMSize_Click);
            // 
            // buttonPatch
            // 
            this.buttonPatch.Enabled = false;
            this.buttonPatch.Image = global::Advanced_SNES_ROM_Utility.Properties.Resources.ips_patch_icon;
            this.buttonPatch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonPatch.Location = new System.Drawing.Point(363, 436);
            this.buttonPatch.Name = "buttonPatch";
            this.buttonPatch.Size = new System.Drawing.Size(168, 32);
            this.buttonPatch.TabIndex = 61;
            this.buttonPatch.Text = "Apply Patch";
            this.buttonPatch.UseVisualStyleBackColor = true;
            this.buttonPatch.Click += new System.EventHandler(this.ButtonPatch_Click);
            // 
            // buttonSaveAs
            // 
            this.buttonSaveAs.Enabled = false;
            this.buttonSaveAs.Image = global::Advanced_SNES_ROM_Utility.Properties.Resources.save_as_icon;
            this.buttonSaveAs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSaveAs.Location = new System.Drawing.Point(452, 54);
            this.buttonSaveAs.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSaveAs.Name = "buttonSaveAs";
            this.buttonSaveAs.Size = new System.Drawing.Size(80, 23);
            this.buttonSaveAs.TabIndex = 54;
            this.buttonSaveAs.Text = "Save As";
            this.buttonSaveAs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSaveAs.UseVisualStyleBackColor = true;
            this.buttonSaveAs.Click += new System.EventHandler(this.ButtonSaveAs_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Enabled = false;
            this.buttonSave.Image = global::Advanced_SNES_ROM_Utility.Properties.Resources.save_icon;
            this.buttonSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSave.Location = new System.Drawing.Point(452, 27);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(80, 23);
            this.buttonSave.TabIndex = 53;
            this.buttonSave.Text = "Save";
            this.buttonSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Controls.Add(this.textBoxCode);
            this.groupBoxInfo.Controls.Add(this.labelCode);
            this.groupBoxInfo.Controls.Add(this.labelTitle);
            this.groupBoxInfo.Controls.Add(this.labelCompany);
            this.groupBoxInfo.Controls.Add(this.labelCountry);
            this.groupBoxInfo.Controls.Add(this.labelVersion);
            this.groupBoxInfo.Controls.Add(this.comboBoxCountryRegion);
            this.groupBoxInfo.Controls.Add(this.labelGetCompany);
            this.groupBoxInfo.Controls.Add(this.textBoxVersion);
            this.groupBoxInfo.Controls.Add(this.textBoxTitle);
            this.groupBoxInfo.Location = new System.Drawing.Point(12, 83);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Size = new System.Drawing.Size(250, 127);
            this.groupBoxInfo.TabIndex = 62;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "GENERAL INFORMATION";
            // 
            // textBoxCode
            // 
            this.textBoxCode.Enabled = false;
            this.textBoxCode.Location = new System.Drawing.Point(194, 96);
            this.textBoxCode.MaxLength = 4;
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(45, 20);
            this.textBoxCode.TabIndex = 59;
            this.textBoxCode.TextChanged += new System.EventHandler(this.TextBoxGetCode_TextChanged);
            // 
            // labelCode
            // 
            this.labelCode.AutoSize = true;
            this.labelCode.Location = new System.Drawing.Point(125, 99);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(63, 13);
            this.labelCode.TabIndex = 58;
            this.labelCode.Text = "Game Code";
            // 
            // groupBoxChecksums
            // 
            this.groupBoxChecksums.Controls.Add(this.labelIntChksm);
            this.groupBoxChecksums.Controls.Add(this.labelCalcChksm);
            this.groupBoxChecksums.Controls.Add(this.labelGetCalcChksm);
            this.groupBoxChecksums.Controls.Add(this.labelGetIntChksm);
            this.groupBoxChecksums.Controls.Add(this.labelCRC32Chksm);
            this.groupBoxChecksums.Controls.Add(this.labelGetCRC32Chksm);
            this.groupBoxChecksums.Controls.Add(this.labelIntInvChksm);
            this.groupBoxChecksums.Controls.Add(this.labelCalcInvChksm);
            this.groupBoxChecksums.Controls.Add(this.labelGetCalcInvChksm);
            this.groupBoxChecksums.Controls.Add(this.labelGetIntInvChksm);
            this.groupBoxChecksums.Location = new System.Drawing.Point(12, 404);
            this.groupBoxChecksums.Name = "groupBoxChecksums";
            this.groupBoxChecksums.Size = new System.Drawing.Size(346, 93);
            this.groupBoxChecksums.TabIndex = 63;
            this.groupBoxChecksums.TabStop = false;
            this.groupBoxChecksums.Text = "CHECKSUMS";
            // 
            // groupBoxROMInfo
            // 
            this.groupBoxROMInfo.Controls.Add(this.labelSMCHeader);
            this.groupBoxROMInfo.Controls.Add(this.labelGetSMCHeader);
            this.groupBoxROMInfo.Controls.Add(this.labelMapMode);
            this.groupBoxROMInfo.Controls.Add(this.labelGetMapMode);
            this.groupBoxROMInfo.Controls.Add(this.labelROMSize);
            this.groupBoxROMInfo.Controls.Add(this.labelGetROMSize);
            this.groupBoxROMInfo.Controls.Add(this.labelROMType);
            this.groupBoxROMInfo.Controls.Add(this.labelGetROMType);
            this.groupBoxROMInfo.Controls.Add(this.labelFileSize);
            this.groupBoxROMInfo.Controls.Add(this.labelGetSRAM);
            this.groupBoxROMInfo.Controls.Add(this.labelGetFileSize);
            this.groupBoxROMInfo.Controls.Add(this.labelSRAM);
            this.groupBoxROMInfo.Controls.Add(this.labelROMSpeed);
            this.groupBoxROMInfo.Controls.Add(this.labelGetROMSpeed);
            this.groupBoxROMInfo.Location = new System.Drawing.Point(12, 216);
            this.groupBoxROMInfo.Name = "groupBoxROMInfo";
            this.groupBoxROMInfo.Size = new System.Drawing.Size(346, 182);
            this.groupBoxROMInfo.TabIndex = 64;
            this.groupBoxROMInfo.TabStop = false;
            this.groupBoxROMInfo.Text = "ROM INFORMATION";
            // 
            // buttonFixSlowROMChecks
            // 
            this.buttonFixSlowROMChecks.Enabled = false;
            this.buttonFixSlowROMChecks.Location = new System.Drawing.Point(363, 409);
            this.buttonFixSlowROMChecks.Name = "buttonFixSlowROMChecks";
            this.buttonFixSlowROMChecks.Size = new System.Drawing.Size(169, 23);
            this.buttonFixSlowROMChecks.TabIndex = 66;
            this.buttonFixSlowROMChecks.Text = "Remove SlowROM Checks";
            this.buttonFixSlowROMChecks.UseVisualStyleBackColor = true;
            this.buttonFixSlowROMChecks.Click += new System.EventHandler(this.ButtonSlowROMFix_Click);
            // 
            // buttonFixSRAMChecks
            // 
            this.buttonFixSRAMChecks.Enabled = false;
            this.buttonFixSRAMChecks.Location = new System.Drawing.Point(363, 380);
            this.buttonFixSRAMChecks.Name = "buttonFixSRAMChecks";
            this.buttonFixSRAMChecks.Size = new System.Drawing.Size(169, 23);
            this.buttonFixSRAMChecks.TabIndex = 67;
            this.buttonFixSRAMChecks.Text = "Remove SRAM Checks";
            this.buttonFixSRAMChecks.UseVisualStyleBackColor = true;
            this.buttonFixSRAMChecks.Click += new System.EventHandler(this.ButtonFixSRAMChecks_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(545, 24);
            this.menuStrip1.TabIndex = 68;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
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
            // checkBoxExpandMirroring
            // 
            this.checkBoxExpandMirroring.AutoSize = true;
            this.checkBoxExpandMirroring.Enabled = false;
            this.checkBoxExpandMirroring.Location = new System.Drawing.Point(6, 21);
            this.checkBoxExpandMirroring.Name = "checkBoxExpandMirroring";
            this.checkBoxExpandMirroring.Size = new System.Drawing.Size(147, 17);
            this.checkBoxExpandMirroring.TabIndex = 69;
            this.checkBoxExpandMirroring.Text = "Mirror ROM on expanding";
            this.checkBoxExpandMirroring.UseVisualStyleBackColor = true;
            this.checkBoxExpandMirroring.CheckedChanged += new System.EventHandler(this.checkBoxExpandMirroring_CheckedChanged);
            // 
            // groupBoxOptions
            // 
            this.groupBoxOptions.Controls.Add(this.checkBoxScan);
            this.groupBoxOptions.Controls.Add(this.checkBoxExpandMirroring);
            this.groupBoxOptions.Location = new System.Drawing.Point(367, 216);
            this.groupBoxOptions.Name = "groupBoxOptions";
            this.groupBoxOptions.Size = new System.Drawing.Size(168, 71);
            this.groupBoxOptions.TabIndex = 70;
            this.groupBoxOptions.TabStop = false;
            this.groupBoxOptions.Text = "Options";
            // 
            // checkBoxScan
            // 
            this.checkBoxScan.AutoSize = true;
            this.checkBoxScan.Enabled = false;
            this.checkBoxScan.Location = new System.Drawing.Point(6, 44);
            this.checkBoxScan.Name = "checkBoxScan";
            this.checkBoxScan.Size = new System.Drawing.Size(158, 17);
            this.checkBoxScan.TabIndex = 70;
            this.checkBoxScan.Text = "Scan protections on loading";
            this.checkBoxScan.UseVisualStyleBackColor = true;
            this.checkBoxScan.CheckedChanged += new System.EventHandler(this.checkBoxScan_CheckedChanged);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(363, 473);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(168, 23);
            this.buttonExit.TabIndex = 71;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 508);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.groupBoxOptions);
            this.Controls.Add(this.buttonFixSRAMChecks);
            this.Controls.Add(this.groupBoxROMInfo);
            this.Controls.Add(this.buttonFixSlowROMChecks);
            this.Controls.Add(this.groupBoxChecksums);
            this.Controls.Add(this.buttonPatch);
            this.Controls.Add(this.buttonFixROMSize);
            this.Controls.Add(this.buttonSaveAs);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonDeinterleave);
            this.Controls.Add(this.buttonFixRegion);
            this.Controls.Add(this.buttonFixChksm);
            this.Controls.Add(this.comboBoxExpandROM);
            this.Controls.Add(this.buttonExpandROM);
            this.Controls.Add(this.comboBoxSplitROM);
            this.Controls.Add(this.buttonSplitROM);
            this.Controls.Add(this.buttonAddHeader);
            this.Controls.Add(this.buttonRemoveHeader);
            this.Controls.Add(this.buttonSwapBinROM);
            this.Controls.Add(this.textBoxROMName);
            this.Controls.Add(this.buttonSelectROM);
            this.Controls.Add(this.groupBoxInfo);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "Advanced SNES ROM Utility v1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.groupBoxInfo.ResumeLayout(false);
            this.groupBoxInfo.PerformLayout();
            this.groupBoxChecksums.ResumeLayout(false);
            this.groupBoxChecksums.PerformLayout();
            this.groupBoxROMInfo.ResumeLayout(false);
            this.groupBoxROMInfo.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBoxOptions.ResumeLayout(false);
            this.groupBoxOptions.PerformLayout();
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
        private System.Windows.Forms.Button buttonRemoveHeader;
        private System.Windows.Forms.Button buttonAddHeader;
        private System.Windows.Forms.Label labelROMSize;
        private System.Windows.Forms.Label labelGetROMSize;
        private System.Windows.Forms.Label labelROMType;
        private System.Windows.Forms.Label labelGetROMType;
        private System.Windows.Forms.Label labelFileSize;
        private System.Windows.Forms.Label labelGetFileSize;
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
        private System.Windows.Forms.Button buttonPatch;
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.GroupBox groupBoxChecksums;
        private System.Windows.Forms.GroupBox groupBoxROMInfo;
        private System.Windows.Forms.Button buttonFixSlowROMChecks;
        private System.Windows.Forms.Button buttonFixSRAMChecks;
        private System.Windows.Forms.TextBox textBoxCode;
        private System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxExpandMirroring;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxOptions;
        private System.Windows.Forms.CheckBox checkBoxScan;
        private System.Windows.Forms.Button buttonExit;
    }
}

