using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Advanced_SNES_ROM_Utility.Converter;
using Advanced_SNES_ROM_Utility.Functions;
using Advanced_SNES_ROM_Utility.Helper;
using Advanced_SNES_ROM_Utility.Lists;
using Advanced_SNES_ROM_Utility.Patcher;

namespace Advanced_SNES_ROM_Utility
{
    public partial class FormMain : Form
    {
        // Create empty ROM
        SNESROM _sourceROM;

        Crc32 _savedFileCRC32 = new Crc32();
        string _savedFileHash;

        bool _saveWithHeader;

        // Create combo boxes for selecting some options
        List<ComboBoxCountryRegionList> _listCountryRegion = new List<ComboBoxCountryRegionList>();

        // Values the expand / split lists were built for (rebuild only if something changed)
        string _listKey;

        // Cached overdump detection (detection reads the whole ROM)
        string _overdumpKey;
        OverdumpInfo _overdumpInfo;

        // Suppresses change events while the form fills its controls
        bool _updatingControls;

        FormHeatmap _heatmapForm;

        public FormMain() : this(null)
        {
        }

        public FormMain(string startupFile)
        {
            InitializeComponent();
            FillComboBoxCountryRegionList();

            // Title length is limited in bytes (Shift-JIS), not in characters - checked on input
            textBoxTitle.MaxLength = 64;

            // Take over the old "Mirror ROM on expanding" option once
            if (!Properties.Settings.Default.ExpandFillMigrated)
            {
                if (Properties.Settings.Default.MirrorROMSetting) { Properties.Settings.Default.ExpandFill = "mirror"; }
                Properties.Settings.Default.ExpandFillMigrated = true;
                Properties.Settings.Default.Save();
            }

            _updatingControls = true;
            comboBoxExpandFill.SelectedIndex = (int)SNESROMConvert.ParseExpandFill(Properties.Settings.Default.ExpandFill);
            _updatingControls = false;

            // Round status light right of the "CHECKSUMS" caption
            int captionWidth = TextRenderer.MeasureText(groupBoxChecksums.Text, groupBoxChecksums.Font).Width;
            checkLightChecksum.Location = new System.Drawing.Point(8 + captionWidth, 2);

            if (!string.IsNullOrEmpty(startupFile))
            {
                Shown += (sender, e) => LoadROM(startupFile);
            }
        }

        /// <summary>Folder of the last opened ROM; the documents folder if it does not exist anymore</summary>
        private string OpenDialogFolder()
        {
            string lastFolder = Properties.Settings.Default.LastROMFolder;

            try
            {
                if (!string.IsNullOrEmpty(lastFolder) && Directory.Exists(lastFolder)) { return lastFolder; }
            }

            catch
            {
                // drive not available
            }

            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private void ButtonSelectROM_Click(object sender, EventArgs e)
        {
            // Select ROM file dialogue
            OpenFileDialog selectROMDialog = new OpenFileDialog();

            selectROMDialog.Filter = "SNES/SFC ROMs (*.smc;*.swc;*;*.sfc;*.fig)|*.smc;*.swc*;*.sfc;*.fig|" +
                                     "All Files (*.*)|*.*";
            selectROMDialog.InitialDirectory = OpenDialogFolder();

            // If successfully selected a ROM file...
            if (selectROMDialog.ShowDialog() == DialogResult.OK)
            {
                LoadROM(selectROMDialog.FileName);
            }
        }

        private void LoadROM(string romPath)
        {
            {
                // Create new ROM
                SNESROM newROM = new SNESROM(@romPath);
                if (newROM.SourceROM == null) { return; }
                _sourceROM = newROM;
                _listKey = null;

                // Remember folder for the open dialog
                Properties.Settings.Default.LastROMFolder = Path.GetDirectoryName(Path.GetFullPath(romPath));
                Properties.Settings.Default.Save();

                // Store CRC32 for dirty tracking
                _savedFileHash = GetCRC32FromFile(@romPath);

                // Initialize combo box for country and region
                comboBoxCountryRegion.DataSource = _listCountryRegion;
                comboBoxCountryRegion.DisplayMember = "Name";
                comboBoxCountryRegion.ValueMember = "Id";

                // Enable / disable text, combo boxes and buttons
                if (!textBoxTitle.Enabled) { textBoxTitle.Enabled = true; }
                if (_sourceROM.IsBSROM) { comboBoxCountryRegion.Enabled = false; } else { comboBoxCountryRegion.Enabled = true; }
                if (!textBoxVersion.Enabled) { textBoxVersion.Enabled = true; }
                if (!comboBoxExpandFill.Enabled) { comboBoxExpandFill.Enabled = true; }
                if (!comboBoxCompany.Enabled) { comboBoxCompany.Enabled = true; }
                if (!checkBoxScan.Enabled) { checkBoxScan.Enabled = true; }
                if (buttonFixRegion.Enabled) { buttonFixRegion.Enabled = false; }
                if (buttonFixSlowROMChecks.Enabled) { buttonFixSlowROMChecks.Enabled = false; }
                if (buttonFixSRAMChecks.Enabled) { buttonFixSRAMChecks.Enabled = false; }
                if (buttonConvertMapMode.Enabled) { buttonConvertMapMode.Enabled = false; }

                // Load values into labels and enable / disable buttons
                RefreshLabelsAndButtons();

                // Scan for copy protections
                if (checkBoxScan.Checked)
                {
                    ScanCopyProtections();
                }
            }
        }

        private void ScanCopyProtections()
        {
            Cursor = Cursors.WaitCursor;

            // Check if ROM contains region locks
            if (_sourceROM.RemoveRegionChecks(false))
            {
                buttonFixRegion.Enabled = true;
            }

            else
            {
                buttonFixRegion.Enabled = false;
            }

            // Check if FastROM contains SlowROM checks
            if (_sourceROM.ByteROMSpeed == (byte)Speed.fast)
            {
                if (_sourceROM.RemoveSlowROMChecks(false))
                {
                    buttonFixSlowROMChecks.Enabled = true;
                }
            }

            else
            {
                buttonFixSlowROMChecks.Enabled = false;
            }

            // Check if ROM contains SRAM checks
            if (_sourceROM.RemoveSRAMChecks(false))
            {
                buttonFixSRAMChecks.Enabled = true;
            }

            else
            {
                buttonFixSRAMChecks.Enabled = false;
            }

            Cursor = Cursors.Arrow;
        }

        private void ButtonAddHeader_Click(object sender, EventArgs e)
        {
            _sourceROM.AddHeader();
            RefreshLabelsAndButtons();
        }

        private void ButtonRemoveHeader_Click(object sender, EventArgs e)
        {
            _sourceROM.RemoveHeader();
            RefreshLabelsAndButtons();
        }

        private void ButtonSwapBinROM_Click(object sender, EventArgs e)
        {
            _sourceROM.SwapBin();
            MessageBox.Show("ROM successfully swapped!\n\nFile(s) saved to: '" + _sourceROM.ROMFolder + "\n\nIn case there was a header, it has been removed!");
        }

        private void ButtonExpandROM_Click(object sender, EventArgs e)
        {
            if (comboBoxExpandROM.SelectedValue == null) { return; }

            // Only sizes from the list (the old rounding to even sizes would turn 1 Mbit into 0)
            double sizeExpandedROM = (double)comboBoxExpandROM.SelectedValue;
            if (sizeExpandedROM <= 0) { return; }

            _sourceROM.Expand(sizeExpandedROM, (ExpandFill)comboBoxExpandFill.SelectedIndex);
            RefreshLabelsAndButtons();
        }

        private void ButtonSplitROM_Click(object sender, EventArgs e)
        {
            int splitROMSize = (int)comboBoxSplitROM.SelectedValue;
            _sourceROM.Split(splitROMSize);
        }

        private void ButtonDeinterleave_Click(object sender, EventArgs e)
        {
            _sourceROM.Deinterleave();
            RefreshLabelsAndButtons();
        }

        private void ButtonInterleave_Click(object sender, EventArgs e)
        {
            _sourceROM.Interleave();
            RefreshLabelsAndButtons();
        }

        private void ButtonFixChksm_Click(object sender, EventArgs e)
        {
            _sourceROM.FixChecksum();
            RefreshLabelsAndButtons();
        }

        private void ButtonFixRegion_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _sourceROM.RemoveRegionChecks();
            RefreshLabelsAndButtons();
            // Set button manually, because RefreshLabelsAndButtons doesn't do that for performace reasons
            buttonFixRegion.Enabled = false;
            Cursor = Cursors.Arrow;
        }

        private void ButtonSlowROMFix_Click(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.SlowROMFixMessage)
            {
                DialogResult dialogResult = new DialogResult();
                FormSlowROMFix slowromfixForm = new FormSlowROMFix();
                dialogResult = slowromfixForm.ShowDialog();

                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            Cursor = Cursors.WaitCursor;
            _sourceROM.RemoveSlowROMChecks();
            RefreshLabelsAndButtons();
            // Set button manually, because RefreshLabelsAndButtons doesn't do that for performace reasons
            buttonFixSlowROMChecks.Enabled = false;
            Cursor = Cursors.Arrow;
        }

        private void ButtonFixSRAMChecks_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _sourceROM.RemoveSRAMChecks();
            RefreshLabelsAndButtons();
            // Set button manually, because RefreshLabelsAndButtons doesn't do that for performace reasons
            buttonFixSRAMChecks.Enabled = false;
            Cursor = Cursors.Arrow;
        }

        private void ButtonFixROMSize_Click(object sender, EventArgs e)
        {
            _sourceROM.FixInternalROMSize();
            RefreshLabelsAndButtons();
        }

        private void ButtonPatch_Click(object sender, EventArgs e)
        {
            // Select patch file dialogue
            OpenFileDialog selectPatchDialog = new OpenFileDialog();

            selectPatchDialog.Filter = "Patch File (*.ips;*.ups;*.bps;*.bdf;*.xdelta)|*.ips;*.ups;*.bps;*.bdf;*.xdelta";

            // If successfully selected a patch file...
            if (selectPatchDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] patchedSourceROM = null;
                byte[] mergedSourceROM = new byte[_sourceROM.SourceROM.Length + _sourceROM.UIntSMCHeader];

                if (_sourceROM.SourceROMSMCHeader != null && _sourceROM.UIntSMCHeader > 0)
                {
                    // Merge header with ROM if header exists
                    Buffer.BlockCopy(_sourceROM.SourceROMSMCHeader, 0, mergedSourceROM, 0, _sourceROM.SourceROMSMCHeader.Length);
                    Buffer.BlockCopy(_sourceROM.SourceROM, 0, mergedSourceROM, _sourceROM.SourceROMSMCHeader.Length, _sourceROM.SourceROM.Length);
                }

                else
                {
                    // Just copy source ROM if no header exists
                    Buffer.BlockCopy(_sourceROM.SourceROM, 0, mergedSourceROM, 0, _sourceROM.SourceROM.Length);
                }

                switch (Path.GetExtension(selectPatchDialog.FileName))
                {
                    case ".ips": patchedSourceROM = IPSPatch.Apply(mergedSourceROM, selectPatchDialog.FileName); break;
                    case ".ups": patchedSourceROM = UPSPatch.Apply(mergedSourceROM, _sourceROM.CRC32Hash, selectPatchDialog.FileName); break;
                    case ".bps": patchedSourceROM = BPSPatch.Apply(mergedSourceROM, _sourceROM.CRC32Hash, selectPatchDialog.FileName); break;
                    case ".bdf": patchedSourceROM = BDFPatch.Apply(mergedSourceROM, selectPatchDialog.FileName); break;
                    case ".xdelta": patchedSourceROM = XDELTAPatch.Apply(mergedSourceROM, selectPatchDialog.FileName); break;
                }
                
                if (patchedSourceROM != null)
                {
                    _sourceROM.SourceROM = patchedSourceROM;
                    _sourceROM.UIntSMCHeader = 0;
                    _sourceROM.SourceROMSMCHeader = null;
                    _sourceROM.Initialize(new string[] { "all" });
                    RefreshLabelsAndButtons();
                    MessageBox.Show("ROM has successfully been patched!");
                }

                else
                {
                    MessageBox.Show("Could not apply patch! Please check if your patch is valid for your ROM.");
                }
            }
        }

        private void ButtonConvertMapMode_Click(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.ConvertMapModeMessage)
            {
                DialogResult dialogResult = new DialogResult();
                FormConvertMapMode convertmapmodeForm = new FormConvertMapMode();
                dialogResult = convertmapmodeForm.ShowDialog();

                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            _sourceROM.ConvertMapMode();
            RefreshLabelsAndButtons();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            Save(_sourceROM.ROMFullPath, "ROM file has successfully been saved!", _saveWithHeader);
        }

        private void ButtonSaveAs_Click(object sender, EventArgs e)
        {
            // Save ROM file dialogue
            SaveFileDialog saveROMDialog = new SaveFileDialog();
            saveROMDialog.InitialDirectory = Path.GetFullPath(_sourceROM.ROMFolder);
            saveROMDialog.FileName = Path.GetFileName(_sourceROM.ROMFullPath);
            saveROMDialog.DefaultExt = Path.GetExtension(_sourceROM.ROMFullPath);

            saveROMDialog.Filter = "SNES/SFC ROMs (*.smc;*.swc;*;*.sfc;*.fig)|*.smc;*.swc*;*.sfc;*.fig|" +
                                   "All Files (*.*)|*.*";

            if (saveROMDialog.ShowDialog() == DialogResult.OK)
            {
                // Use Save function for writing file
                Save(@saveROMDialog.FileName, "ROM file has successfully been saved to: " + @saveROMDialog.FileName, _saveWithHeader);

                // Reload ROM
                LoadROM(@saveROMDialog.FileName);
            }
        }

        private void Save(string filepath, string message, bool saveWithHeader)
        {
            // Write ROM (incl. header, if one exists) to file
            File.WriteAllBytes(filepath, _sourceROM.GetSaveData());

            // Store CRC32 for dirty tracking
            _savedFileHash = GetCRC32FromFile(filepath);

            // Show message
            MessageBox.Show(message);
        }

        private void TextBoxGetTitle_TextChanged(object sender, EventArgs e)
        {
            int maxBytes = _sourceROM.ByteArrayTitle.Length;

            // Title is limited in bytes: Japanese (Shift-JIS) characters need two bytes
            if (SNESROM.TitleByteLength(textBoxTitle.Text.Trim()) > maxBytes)
            {
                int cursor = Math.Max(0, textBoxTitle.SelectionStart - 1);
                textBoxTitle.TextChanged -= TextBoxGetTitle_TextChanged;
                textBoxTitle.Text = _sourceROM.StringTitle.Trim();
                textBoxTitle.SelectionStart = Math.Min(cursor, textBoxTitle.Text.Length);
                textBoxTitle.TextChanged += TextBoxGetTitle_TextChanged;
                System.Media.SystemSounds.Beep.Play();
                toolTipMain.Show("The title can have " + maxBytes + " bytes at most (Japanese characters need 2 bytes).", textBoxTitle, 0, textBoxTitle.Height, 2500);
                return;
            }

            if (!_sourceROM.StringTitle.Trim().Equals(textBoxTitle.Text.Trim()))
            {
                _sourceROM.SetTitle(textBoxTitle.Text, maxBytes);
                RefreshLabelsAndButtons();
            }
        }

        private void TextBoxGetVersion_Leave(object sender, EventArgs e)
        {
            if (_sourceROM.StringVersion != "" && _sourceROM.StringVersion.Trim() != textBoxVersion.Text.Trim())
            {
                string versionPattern = @"^([1]\.)([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|2[5][0-5])$";

                if (Regex.IsMatch(textBoxVersion.Text.Trim(), versionPattern))
                {
                    byte[] byteArrayVersion = new byte[1];
                    string[] splitVersion = textBoxVersion.Text.Split('.');
                    int intVersion = Int16.Parse(splitVersion[1]);
                    byteArrayVersion = BitConverter.GetBytes(intVersion);

                    _sourceROM.SetVersion(byteArrayVersion[0]);
                    RefreshLabelsAndButtons();
                }

                else
                {
                    textBoxVersion.Text = _sourceROM.StringVersion;
                    MessageBox.Show("Enter version number between 1.0 and 1.255");
                }
            }
        }

        private void TextBoxGetCode_TextChanged(object sender, EventArgs e)
        {
            // Game codes consist of A-Z and 0-9 only
            string clean = SNESROM.SanitizeGameCode(textBoxCode.Text);

            if (textBoxCode.Text != clean)
            {
                int cursor = Math.Max(0, textBoxCode.SelectionStart - (textBoxCode.Text.Length - clean.Length));
                textBoxCode.TextChanged -= TextBoxGetCode_TextChanged;
                textBoxCode.Text = clean;
                textBoxCode.SelectionStart = Math.Min(cursor, clean.Length);
                textBoxCode.TextChanged += TextBoxGetCode_TextChanged;
            }

            if (!_sourceROM.StringGameCode.Trim().Equals(textBoxCode.Text.Trim()))
            {
                _sourceROM.SetGameCode(textBoxCode.Text);
                RefreshLabelsAndButtons();
            }
        }

        private void ComboBoxCountryRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!comboBoxCountryRegion.Enabled || comboBoxCountryRegion.SelectedIndex < 0) { return; }

            int selectedCountryRegion = (int)comboBoxCountryRegion.SelectedValue;
            byte byteCountryRegion = Convert.ToByte(selectedCountryRegion);
            byte[] byteArrayCountryRegion = new byte[1];
            byteArrayCountryRegion[0] = byteCountryRegion;

            if (_sourceROM.ByteCountry != byteArrayCountryRegion[0])
            {
                _sourceROM.SetCountryRegion(byteArrayCountryRegion[0]);
                RefreshLabelsAndButtons();
            }
        }

        private void RefreshLabelsAndButtons()
        {
            _updatingControls = true;

            // Set combo boxes for expanding and splitting, if file size or fill option have changed
            string listKey = _sourceROM.IntCalcFileSize + "|" + _sourceROM.IsBSROM + "|" + comboBoxExpandFill.SelectedIndex;

            if (listKey != _listKey)
            {
                _listKey = listKey;
                RefreshComboBoxExpandROMList();
                RefreshComboBoxSplitROMList();
            }

            // Set text boxes
            textBoxROMName.Text = _sourceROM.ROMFullPath;
            textBoxTitle.TextChanged -= TextBoxGetTitle_TextChanged;
            textBoxTitle.Text = _sourceROM.StringTitle.Trim();
            textBoxTitle.TextChanged += TextBoxGetTitle_TextChanged;
            textBoxVersion.Text = _sourceROM.StringVersion;
            textBoxCode.TextChanged -= TextBoxGetCode_TextChanged;
            textBoxCode.Text = _sourceROM.StringGameCode.Trim();
            textBoxCode.TextChanged += TextBoxGetCode_TextChanged;
            if (_sourceROM.StringGameCode.Trim() != "N/A") { textBoxCode.Enabled = true; } else { textBoxCode.Enabled = false; }

            // Set labels
            labelGetMapMode.Text = _sourceROM.StringMapMode;
            labelGetROMType.Text = _sourceROM.StringROMType;
            labelGetROMSize.Text = _sourceROM.StringROMSize;
            labelSRAM.Text = "(S)RAM"; if (_sourceROM.ByteSRAMSize > 0x00) { labelSRAM.Text = "SRAM"; } else if (_sourceROM.ByteExRAMSize > 0x00) { labelSRAM.Text = "RAM"; }
            labelGetFileSize.Text = SNESROMFunction.FormatMbit(_sourceROM.IntCalcFileSize) + " (" + SNESROMFunction.FormatMByte(_sourceROM.IntCalcFileSize) + ")";
            labelGetSMCHeader.Text = _sourceROM.StringSMCHeader;
            labelGetROMSpeed.Text = _sourceROM.StringROMSpeed;
            labelGetIntChksm.Text = BitConverter.ToString(_sourceROM.ByteArrayChecksum).Replace("-", "");
            labelGetIntInvChksm.Text = BitConverter.ToString(_sourceROM.ByteArrayInvChecksum).Replace("-", "");
            labelGetCalcChksm.Text = BitConverter.ToString(_sourceROM.ByteArrayCalcChecksum).Replace("-", "");
            labelGetCalcInvChksm.Text = BitConverter.ToString(_sourceROM.ByteArrayCalcInvChecksum).Replace("-", "");
            labelGetCRC32Chksm.Text = _sourceROM.CRC32Hash;
            labelGetMD5Chksm.Text = _sourceROM.MD5Hash;
            labelGetSHA1Chksm.Text = _sourceROM.SHA1Hash;
            labelGetSHA256Chksm.Text = _sourceROM.SHA256Hash;

            // Checksum light: green if the internal checksum and its inverse are correct
            checkLightChecksum.OK = _sourceROM.ByteArrayChecksum.SequenceEqual(_sourceROM.ByteArrayCalcChecksum) &&
                                    _sourceROM.ByteArrayInvChecksum.SequenceEqual(_sourceROM.ByteArrayCalcInvChecksum);
            checkLightChecksum.Visible = true;
            toolTipMain.SetToolTip(checkLightChecksum, checkLightChecksum.OK ? "Internal checksum is correct" : "Internal checksum does not match");

            // Set combo boxes
            if (_sourceROM.ByteCountry < SNESROMList.CountryRegion.GetLength(0)) { comboBoxCountryRegion.SelectedIndex = _sourceROM.ByteCountry; } else { comboBoxCountryRegion.SelectedIndex = -1; }
            RefreshComboBoxCompany();
            RefreshComboBoxSRAM();

            // Set check boxes
            checkBoxScan.Checked = Properties.Settings.Default.ScanROMSetting;

            // Set buttons
            if (!buttonSave.Enabled) { buttonSave.Enabled = true; }
            if (!buttonSaveAs.Enabled) { buttonSaveAs.Enabled = true; }
            if (!buttonPatch.Enabled) { buttonPatch.Enabled = true; }
            if (!buttonScan.Enabled) { buttonScan.Enabled = true; }
            if (!buttonHeatmap.Enabled) { buttonHeatmap.Enabled = true; }
            buttonRemoveOverdump.Enabled = GetOverdumpInfo().Overdump;
            if (_sourceROM.SourceROMSMCHeader == null) { buttonAddHeader.Enabled = true; buttonRemoveHeader.Enabled = false; _saveWithHeader = false; } else { buttonAddHeader.Enabled = false; buttonRemoveHeader.Enabled = true; _saveWithHeader = true; }
            if (_sourceROM.SourceROM.Length % 1048576 == 0) { buttonSwapBinROM.Enabled = true; } else { buttonSwapBinROM.Enabled = false; }
            if (!_sourceROM.IsInterleaved && _sourceROM.UIntROMHeaderOffset != (uint)HeaderOffset.exlorom && _sourceROM.UIntROMHeaderOffset != (uint)HeaderOffset.exhirom) { buttonConvertMapMode.Enabled = true; } else { buttonConvertMapMode.Enabled = false; }
            buttonFixROMSize.Enabled = _sourceROM.NeedsInternalROMSizeFix();

            _updatingControls = false;

            // Keep an open heatmap up to date
            if (_heatmapForm != null && !_heatmapForm.IsDisposed) { _heatmapForm.UpdateROM(_sourceROM); }

            if (!_sourceROM.IsInterleaved && (_sourceROM.UIntROMHeaderOffset == (uint)HeaderOffset.hirom || _sourceROM.UIntROMHeaderOffset == (uint)HeaderOffset.exhirom)) { buttonInterleave.Enabled = true; buttonDeinterleave.Enabled = false; } else { buttonInterleave.Enabled = false; }
            if (_sourceROM.IsInterleaved) { buttonDeinterleave.Enabled = true; buttonInterleave.Enabled = false; buttonFixChksm.Enabled = false; return; }
            if (!_sourceROM.ByteArrayChecksum.SequenceEqual(_sourceROM.ByteArrayCalcChecksum)) { buttonFixChksm.Enabled = true; } else { buttonFixChksm.Enabled = false; }
        }

        private OverdumpInfo GetOverdumpInfo()
        {
            string key = _sourceROM.CRC32Hash + ":" + _sourceROM.SourceROM.Length + ":" + _sourceROM.UIntROMHeaderOffset + ":" + _sourceROM.IntROMSize;

            if (key != _overdumpKey)
            {
                _overdumpKey = key;
                _overdumpInfo = _sourceROM.DetectOverdump();
            }

            return _overdumpInfo;
        }

        private void RefreshComboBoxCompany()
        {
            List<CompanyOption> options = _sourceROM.GetCompanyOptions();

            comboBoxCompany.DataSource = null;
            comboBoxCompany.DisplayMember = "DisplayName";
            comboBoxCompany.ValueMember = "Code";
            comboBoxCompany.DataSource = options;
            comboBoxCompany.SelectedIndex = options.FindIndex(o => o.Code == _sourceROM.IntCompany);

            toolTipMain.SetToolTip(comboBoxCompany, _sourceROM.HasMakerCode
                ? "Extended header: maker code at $FFB0 (2 characters)"
                : "Old header: licensee byte at $FFDA (only companies with a 1 byte code available)");
        }

        private static readonly string[] SRAMSizeNames = { "None", "2 kByte (16 Kbit)", "4 kByte (32 Kbit)", "8 kByte (64 Kbit)", "16 kByte (128 Kbit)",
                                                           "32 kByte (256 Kbit)", "64 kByte (512 Kbit)", "128 kByte (1 Mbit)" };

        private void RefreshComboBoxSRAM()
        {
            SRAMSizeInfo info = _sourceROM.GetSRAMSizeInfo();
            List<ComboBoxSRAMList> list = new List<ComboBoxSRAMList>();

            for (int i = 0; i < SRAMSizeNames.Length; i++) { list.Add(new ComboBoxSRAMList { Id = i, Name = SRAMSizeNames[i] }); }
            if (info.Value >= SRAMSizeNames.Length) { list.Add(new ComboBoxSRAMList { Id = info.Value, Name = Math.Pow(2, info.Value) + " kByte" }); }

            comboBoxSRAM.DataSource = null;
            comboBoxSRAM.DisplayMember = "Name";
            comboBoxSRAM.ValueMember = "Id";
            comboBoxSRAM.DataSource = list;
            comboBoxSRAM.SelectedIndex = list.FindIndex(l => l.Id == info.Value);
            comboBoxSRAM.Enabled = info.Editable;
            toolTipMain.SetToolTip(comboBoxSRAM, info.Reason);
        }

        private void ComboBoxCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updatingControls || _sourceROM == null || comboBoxCompany.SelectedIndex < 0) { return; }

            int code = ((CompanyOption)comboBoxCompany.SelectedItem).Code;
            if (code == _sourceROM.IntCompany) { return; }

            if (!_sourceROM.SetCompany(code))
            {
                MessageBox.Show("This company code cannot be stored in the header of this ROM.", "Attention!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            RefreshLabelsAndButtons();
        }

        private void ComboBoxSRAM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updatingControls || _sourceROM == null || comboBoxSRAM.SelectedIndex < 0) { return; }

            int size = ((ComboBoxSRAMList)comboBoxSRAM.SelectedItem).Id;
            SRAMSizeInfo info = _sourceROM.GetSRAMSizeInfo();
            if (!info.Editable || info.Value == size) { return; }

            if (!_sourceROM.SetSRAMSize(size))
            {
                MessageBox.Show("The SRAM size of this ROM cannot be changed.", "Attention!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            RefreshLabelsAndButtons();

            // SRAM checks depend on the SRAM size
            if (checkBoxScan.Checked) { ScanCopyProtections(); }
        }

        private void ComboBoxExpandFill_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updatingControls) { return; }

            Properties.Settings.Default.ExpandFill = SNESROMConvert.ExpandFillToSetting((ExpandFill)comboBoxExpandFill.SelectedIndex);
            Properties.Settings.Default.Save();

            if (_sourceROM != null && _sourceROM.SourceROM != null)
            {
                _listKey = null;
                RefreshLabelsAndButtons();
            }
        }

        private void ButtonRemoveOverdump_Click(object sender, EventArgs e)
        {
            OverdumpInfo info = GetOverdumpInfo();

            if (!info.Overdump)
            {
                MessageBox.Show(info.Reason ?? "No overdump found.");
                return;
            }

            string how = info.Type == "mirror"
                ? "The last " + SNESROMFunction.FormatBytes(info.FileSize - info.RealSize) + " of the ROM are a mirror of the data before."
                : "The ROM is larger than its internal size and the last " + SNESROMFunction.FormatBytes(info.FileSize - info.RealSize) + " only contain 0x" + info.Fill.ToString("X2") + ".";

            DialogResult dialogResult = MessageBox.Show(how + "\n\nFile size: " + SNESROMFunction.FormatBytes(info.FileSize) + "\nReal ROM size: " + SNESROMFunction.FormatBytes(info.RealSize) +
                                                        "\n\nDo you want to remove the overdump?", "Overdump detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult != DialogResult.Yes) { return; }

            _sourceROM.RemoveOverdump();
            RefreshLabelsAndButtons();

            MessageBox.Show("Overdump has been removed. The ROM now has " + SNESROMFunction.FormatBytes(_sourceROM.SourceROM.Length) + "." +
                            (_sourceROM.NeedsInternalROMSizeFix() ? "\n\nThe internal ROM size does not match anymore - use \"Fix Internal ROM Size\"." : string.Empty));
        }

        private void ButtonHeatmap_Click(object sender, EventArgs e)
        {
            if (_heatmapForm == null || _heatmapForm.IsDisposed)
            {
                _heatmapForm = new FormHeatmap(_sourceROM);
                _heatmapForm.Show(this);
            }

            else
            {
                _heatmapForm.UpdateROM(_sourceROM);
                _heatmapForm.Activate();
            }
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];

            if (files != null && files.Length > 0 && File.Exists(files[0]))
            {
                // Same check as when closing: ask before replacing a changed ROM
                if (!ConfirmDiscardChanges("Do you want to save your progress before opening another ROM?")) { return; }
                LoadROM(files[0]);
            }
        }

        private string GetCRC32FromFile(string filepath)
        {
            string tempFileHash = null;

            using (FileStream fs = File.Open(filepath, FileMode.Open))
            {
                foreach (byte b in _savedFileCRC32.ComputeHash(fs))
                {
                    tempFileHash += b.ToString("x2").ToUpper();
                }
            }

            return tempFileHash;
        }

        private void ButtonScan_Click(object sender, EventArgs e)
        {
            ScanCopyProtections();
        }

        private void RefreshComboBoxExpandROMList()
        {
            double size = _sourceROM.IntCalcFileSize;
            bool mirror = comboBoxExpandFill.SelectedIndex == (int)ExpandFill.Mirror;
            List<ComboBoxExpandROMList> list = new List<ComboBoxExpandROMList>();

            if ((!_sourceROM.IsBSROM && size < 64) || (_sourceROM.IsBSROM && size < 32))
            {
                if (size < 0.5) { list.Add(new ComboBoxExpandROMList { Id = 0.5, Name = "512 Kbit (64 kByte) | 27C512" }); }
                if (size < 1) { list.Add(new ComboBoxExpandROMList { Id = 1, Name = "1 Mbit (128 kByte) | 27C1001" }); }
                if (size < 2) { list.Add(new ComboBoxExpandROMList { Id = 2, Name = "2 Mbit (256 kByte) | 27C2001" }); }
                if (size < 4) { list.Add(new ComboBoxExpandROMList { Id = 4, Name = "4 Mbit (512 kByte) | 274001" }); }
                if (size < 8) { list.Add(new ComboBoxExpandROMList { Id = 8, Name = "8 Mbit (1 MByte) | 27C801" }); }
                if (size < 12 && !mirror) { list.Add(new ComboBoxExpandROMList { Id = 12, Name = "12 Mbit (1,5 MByte)" }); }
                if (size < 16) { list.Add(new ComboBoxExpandROMList { Id = 16, Name = "16 Mbit (2 MByte) | 27C160" }); }
                if (size < 20 && !mirror) { list.Add(new ComboBoxExpandROMList { Id = 20, Name = "20 Mbit (2,5 MByte)" }); }
                if (size < 24 && !mirror) { list.Add(new ComboBoxExpandROMList { Id = 24, Name = "24 Mbit (3 MByte)" }); }
                if (size < 28 && !mirror) { list.Add(new ComboBoxExpandROMList { Id = 28, Name = "28 Mbit (3,5 MByte)" }); }
                if (size < 32) { list.Add(new ComboBoxExpandROMList { Id = 32, Name = "32 Mbit (4 MByte) | 27C322" }); }
                // BS-X ROMs (Satellaview memory packs) never exceed 32 Mbit
                if (size < 48 && !_sourceROM.IsBSROM) { list.Add(new ComboBoxExpandROMList { Id = 48, Name = "48 Mbit (6 MByte)" }); }
                if (size < 64 && !_sourceROM.IsBSROM) { list.Add(new ComboBoxExpandROMList { Id = 64, Name = "64 Mbit (8 MByte)" }); }
            }

            comboBoxExpandROM.DataSource = null;
            comboBoxExpandROM.Items.Clear();

            if (list.Count > 0)
            {
                comboBoxExpandROM.DataSource = list;
                comboBoxExpandROM.DisplayMember = "Name";
                comboBoxExpandROM.ValueMember = "Id";
            }

            buttonExpandROM.Enabled = list.Count > 0;
            comboBoxExpandROM.Enabled = list.Count > 0;
        }

        private void RefreshComboBoxSplitROMList()
        {
            double size = _sourceROM.IntCalcFileSize;
            List<ComboBoxSplitROMList> list = new List<ComboBoxSplitROMList>();

            if (size > 1)
            {
                if (size % 64 == 0 && size > 64) { list.Add(new ComboBoxSplitROMList { Id = 64, Name = "64 Mbit (8 MByte)" }); };
                if (size % 32 == 0 && size > 32) { list.Add(new ComboBoxSplitROMList { Id = 32, Name = "32 Mbit (4 MByte) | 27C322" }); };
                if (size % 16 == 0 && size > 16) { list.Add(new ComboBoxSplitROMList { Id = 16, Name = "16 Mbit (2 MByte) | 27C160" }); };
                if (size % 8 == 0 && size > 8) { list.Add(new ComboBoxSplitROMList { Id = 8, Name = "8 Mbit (1 MByte) | 27C801" }); };
                if (size % 4 == 0 && size > 4) { list.Add(new ComboBoxSplitROMList { Id = 4, Name = "4 Mbit (512 kByte) | 27C4001" }); };
                if (size % 2 == 0 && size > 2) { list.Add(new ComboBoxSplitROMList { Id = 2, Name = "2 Mbit (256 kByte) | 27C2001" }); };
                list.Add(new ComboBoxSplitROMList { Id = 1, Name = "1 Mbit (128 kByte) | 27C1001" });
            }

            comboBoxSplitROM.DataSource = null;
            comboBoxSplitROM.Items.Clear();

            if (list.Count > 0)
            {
                comboBoxSplitROM.DataSource = list;
                comboBoxSplitROM.DisplayMember = "Name";
                comboBoxSplitROM.ValueMember = "Id";
            }

            buttonSplitROM.Enabled = list.Count > 0;
            comboBoxSplitROM.Enabled = list.Count > 0;
        }

        private void FillComboBoxCountryRegionList()
        {
            for (int l = 0; l < SNESROMList.CountryRegion.GetLength(0); l++)
            {
                _listCountryRegion.Add(new ComboBoxCountryRegionList { Id = l, Name = $"{SNESROMList.CountryRegion[l, 0]} | {SNESROMList.CountryRegion[l, 1]}" });
            }
        }

        private void CheckBoxScan_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxScan.Checked)
            {
                Properties.Settings.Default.ScanROMSetting = true;
            }

            else if (!checkBoxScan.Checked)
            {
                Properties.Settings.Default.ScanROMSetting = false;
            }

            Properties.Settings.Default.Save();
        }

        private void CopyToClipboard_Click(object sender, EventArgs e)
        {
            Label clipboardLabel = (Label)sender;
            ToolTip clipboardToolTipp = new ToolTip();

            Clipboard.SetText(clipboardLabel.Text);

            clipboardToolTipp.Show("Checksum has been copied to clipboard!", this, PointToClient(Cursor.Position), 1500);
        }

        class ComboBoxExpandROMList
        {
            public double Id { get; set; }
            public string Name { get; set; }
        }

        class ComboBoxSplitROMList
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        class ComboBoxSRAMList
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        class ComboBoxCountryRegionList
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !ConfirmDiscardChanges("Do you want to save your progress before closing?");
        }

        /// <summary>Asks to save unsaved changes. Returns false if the user cancels.</summary>
        private bool ConfirmDiscardChanges(string question)
        {
            if (_savedFileHash == null || _sourceROM == null || _sourceROM.SourceROM == null) { return true; }

            Crc32 trackingROMCRC32 = new Crc32();
            string trackingROMHash = null;

            foreach (byte singleByte in trackingROMCRC32.ComputeHash(_sourceROM.GetSaveData()))
            {
                trackingROMHash += singleByte.ToString("X2").ToUpper();
            }

            if (_savedFileHash == trackingROMHash) { return true; }

            DialogResult dialogResult = MessageBox.Show(question, "Attention!", MessageBoxButtons.YesNoCancel);

            if (dialogResult == DialogResult.Yes)
            {
                // Generate ROM, write to file and store copy for dirty tracking
                Save(_sourceROM.ROMFullPath, "ROM file has successfully been saved to: " + _sourceROM.ROMFullPath, _saveWithHeader);
                return true;
            }

            return dialogResult == DialogResult.No;
        }

        private void ResetOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("This will reset all your saved settings and\n" +
                                                        "restore all your suppressed dialog messages.\n\n" +
                                                        "Do you want to proceed?", "Attention!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                comboBoxExpandFill.SelectedIndex = (int)ExpandFill.Empty00;
                checkBoxScan.Checked = false;
                Properties.Settings.Default.ExpandFill = "00";
                Properties.Settings.Default.SlowROMFixMessage = false;
                Properties.Settings.Default.ConvertMapModeMessage = false;
                Properties.Settings.Default.Save();

                MessageBox.Show("All settings have been reset successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                return;
            }
        }

        private void ManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHelp helpForm = new FormHelp();
            helpForm.Show();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout aboutForm = new FormAbout();
            aboutForm.Show();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonSelectROM.PerformClick();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}