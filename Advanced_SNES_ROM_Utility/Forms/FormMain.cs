﻿using System;
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
        List<ComboBoxExpandROMList> _listExpandROM = new List<ComboBoxExpandROMList>();
        List<ComboBoxSplitROMList> _listSplitROM = new List<ComboBoxSplitROMList>();

        public FormMain()
        {
            InitializeComponent();
            FillComboBoxCountryRegionList();
        }

        private void ButtonSelectROM_Click(object sender, EventArgs e)
        {
            // Select ROM file dialogue
            OpenFileDialog selectROMDialog = new OpenFileDialog();

            selectROMDialog.Filter = "SNES/SFC ROMs (*.smc;*.swc;*;*.sfc;*.fig)|*.smc;*.swc*;*.sfc;*.fig|" +
                                     "All Files (*.*)|*.*";

            // If successfully selected a ROM file...
            if (selectROMDialog.ShowDialog() == DialogResult.OK)
            {
                // Create new ROM
                _sourceROM = new SNESROM(@selectROMDialog.FileName);
                if (_sourceROM.SourceROM == null) { return; }

                // Store CRC32 for dirty tracking
                _savedFileHash = GetCRC32FromFile(@selectROMDialog.FileName);

                // Initialize combo box for country and region
                comboBoxCountryRegion.DataSource = _listCountryRegion;
                comboBoxCountryRegion.DisplayMember = "Name";
                comboBoxCountryRegion.ValueMember = "Id";

                // Enable / disable text, combo boxes and buttons
                if (!textBoxTitle.Enabled) { textBoxTitle.Enabled = true; }
                if (_sourceROM.IsBSROM) { comboBoxCountryRegion.Enabled = false; } else { comboBoxCountryRegion.Enabled = true; }
                textBoxTitle.MaxLength = _sourceROM.StringTitle.Length;
                if (!textBoxVersion.Enabled) { textBoxVersion.Enabled = true; }
                if (!checkBoxExpandMirroring.Enabled) { checkBoxExpandMirroring.Enabled = true; }
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
            if ((int)comboBoxExpandROM.SelectedValue < 1) { return; }

            int sizeExpandedROM = (int)comboBoxExpandROM.SelectedValue - ((int)comboBoxExpandROM.SelectedValue % 2);
            _sourceROM.Expand(sizeExpandedROM, checkBoxExpandMirroring.Checked);
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
                    _sourceROM.Initialize();
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
                _sourceROM = new SNESROM(@saveROMDialog.FileName);
                RefreshLabelsAndButtons();
            }
        }

        private void Save(string filepath, string message, bool saveWithHeader)
        {
            if (saveWithHeader)
            {
                // Merge header with ROM
                byte[] haderedROM = new byte[_sourceROM.SourceROMSMCHeader.Length + _sourceROM.SourceROM.Length];

                Buffer.BlockCopy(_sourceROM.SourceROMSMCHeader, 0, haderedROM, 0, _sourceROM.SourceROMSMCHeader.Length);
                Buffer.BlockCopy(_sourceROM.SourceROM, 0, haderedROM, _sourceROM.SourceROMSMCHeader.Length, _sourceROM.SourceROM.Length);

                // Write to file
                File.WriteAllBytes(filepath, haderedROM);
            }

            else
            {
                // Just write ROM to file
                File.WriteAllBytes(filepath, _sourceROM.SourceROM);
            }

            // Store CRC32 for dirty tracking
            _savedFileHash = GetCRC32FromFile(filepath);

            // Show message
            MessageBox.Show(message);
        }

        private void TextBoxGetTitle_TextChanged(object sender, EventArgs e)
        {
            if (!_sourceROM.StringTitle.Trim().Equals(textBoxTitle.Text.Trim()))
            {
                _sourceROM.SetTitle(textBoxTitle.Text, textBoxTitle.MaxLength);
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
            textBoxCode.Text = textBoxCode.Text.ToUpper();

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
            // Set combo boxes for expanding and splitting, if file size has changed
            if (labelGetFileSize.Text.Split(' ')[0] != _sourceROM.IntCalcFileSize.ToString())
            {
                comboBoxExpandROM.DataSource = null;
                comboBoxSplitROM.DataSource = null;
                comboBoxExpandROM.Items.Clear();
                comboBoxSplitROM.Items.Clear();
                comboBoxExpandROM.Enabled = false;
                comboBoxSplitROM.Enabled = false;

                if ((!_sourceROM.IsBSROM && _sourceROM.IntCalcFileSize < 64) || (_sourceROM.IsBSROM && _sourceROM.IntCalcFileSize < 32))
                {
                    RefreshComboBoxExpandROMList();
                }

                else
                {
                    buttonExpandROM.Enabled = false;
                    comboBoxExpandROM.Enabled = false;
                }

                if (_sourceROM.IntCalcFileSize > 1)
                {
                    RefreshComboBoxSplitROMList();
                }

                else
                {
                    buttonSplitROM.Enabled = false;
                    comboBoxSplitROM.Enabled = false;
                }
            }

            // Set text boxes
            textBoxROMName.Text = _sourceROM.ROMFullPath;
            textBoxTitle.Text = _sourceROM.StringTitle.Trim();
            textBoxVersion.Text = _sourceROM.StringVersion;
            textBoxCode.Text = _sourceROM.StringGameCode.Trim();
            if (_sourceROM.StringGameCode.Trim() != "N/A") { textBoxCode.Enabled = true; } else { textBoxCode.Enabled = false; }

            // Set labels
            labelGetMapMode.Text = _sourceROM.StringMapMode;
            labelGetROMType.Text = _sourceROM.StringROMType;
            labelGetROMSize.Text = _sourceROM.StringROMSize;
            labelGetSRAM.Text = _sourceROM.StringRAMSize;
            labelSRAM.Text = "(S)RAM"; if (_sourceROM.ByteSRAMSize > 0x00) { labelSRAM.Text = "SRAM"; } else if (_sourceROM.ByteExRAMSize > 0x00) { labelSRAM.Text = "RAM"; }
            labelGetFileSize.Text = _sourceROM.IntCalcFileSize.ToString() + " Mbit (" + ((float)_sourceROM.IntCalcFileSize / 8) + " MByte)";
            labelGetSMCHeader.Text = _sourceROM.StringSMCHeader;
            labelGetROMSpeed.Text = _sourceROM.StringROMSpeed;
            labelGetCompany.Text = _sourceROM.StringCompany;
            labelGetIntChksm.Text = BitConverter.ToString(_sourceROM.ByteArrayChecksum).Replace("-", "");
            labelGetIntInvChksm.Text = BitConverter.ToString(_sourceROM.ByteArrayInvChecksum).Replace("-", "");
            labelGetCalcChksm.Text = BitConverter.ToString(_sourceROM.ByteArrayCalcChecksum).Replace("-", "");
            labelGetCalcInvChksm.Text = BitConverter.ToString(_sourceROM.ByteArrayCalcInvChecksum).Replace("-", "");
            labelGetCRC32Chksm.Text = _sourceROM.CRC32Hash;

            // Set combo boxes
            if (_sourceROM.ByteCountry <= SNESROMList.CountryRegion.GetLength(0)) { comboBoxCountryRegion.SelectedIndex = _sourceROM.ByteCountry; } else { comboBoxCountryRegion.SelectedIndex = -1; }

            // Set check boxes
            checkBoxExpandMirroring.Checked = Properties.Settings.Default.MirrorROMSetting;
            checkBoxScan.Checked = Properties.Settings.Default.ScanROMSetting;

            // Set buttons
            if (!buttonSave.Enabled) { buttonSave.Enabled = true; }
            if (!buttonSaveAs.Enabled) { buttonSaveAs.Enabled = true; }
            if (!buttonPatch.Enabled) { buttonPatch.Enabled = true; }
            if (!buttonScan.Enabled) { buttonScan.Enabled = true; }
            if (_sourceROM.SourceROMSMCHeader == null) { buttonAddHeader.Enabled = true; buttonRemoveHeader.Enabled = false; _saveWithHeader = false; } else { buttonAddHeader.Enabled = false; buttonRemoveHeader.Enabled = true; _saveWithHeader = true; }
            if (_sourceROM.SourceROM.Length % 1048576 == 0) { buttonSwapBinROM.Enabled = true; } else { buttonSwapBinROM.Enabled = false; }
            if (!_sourceROM.IsInterleaved && _sourceROM.UIntROMHeaderOffset != (uint)HeaderOffset.exlorom && _sourceROM.UIntROMHeaderOffset != (uint)HeaderOffset.exhirom) { buttonConvertMapMode.Enabled = true; } else { buttonConvertMapMode.Enabled = false; }
            if (_sourceROM.IntROMSize < _sourceROM.IntCalcFileSize || _sourceROM.IntCalcFileSize <= (_sourceROM.IntROMSize / 2)) { buttonFixROMSize.Enabled = true; } else { buttonFixROMSize.Enabled = false; }
            if (!_sourceROM.IsInterleaved && (_sourceROM.UIntROMHeaderOffset == (uint)HeaderOffset.hirom || _sourceROM.UIntROMHeaderOffset == (uint)HeaderOffset.exhirom)) { buttonInterleave.Enabled = true; buttonDeinterleave.Enabled = false; } else { buttonInterleave.Enabled = false; }
            if (_sourceROM.IsInterleaved) { buttonDeinterleave.Enabled = true; buttonInterleave.Enabled = false; buttonFixChksm.Enabled = false; return; }
            if (!_sourceROM.ByteArrayChecksum.SequenceEqual(_sourceROM.ByteArrayCalcChecksum)) { buttonFixChksm.Enabled = true; } else { buttonFixChksm.Enabled = false; }
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
            if (checkBoxExpandMirroring.Enabled)
            {
                List<ComboBoxExpandROMList> list = new List<ComboBoxExpandROMList>();

                if (_sourceROM.IntCalcFileSize < 1) { list.Add(new ComboBoxExpandROMList { Id = 1, Name = "1 Mbit (128 kByte) | 27C1001" }); };
                if (_sourceROM.IntCalcFileSize < 2) { list.Add(new ComboBoxExpandROMList { Id = 2, Name = "2 Mbit (256 kByte) | 27C2001" }); };
                if (_sourceROM.IntCalcFileSize < 4) { list.Add(new ComboBoxExpandROMList { Id = 4, Name = "4 Mbit (512 kByte) | 274001" }); };
                if (_sourceROM.IntCalcFileSize < 8) { list.Add(new ComboBoxExpandROMList { Id = 8, Name = "8 Mbit (1 MByte) | 27C801" }); };
                if (_sourceROM.IntCalcFileSize < 12 && !checkBoxExpandMirroring.Checked) { list.Add(new ComboBoxExpandROMList { Id = 12, Name = "12 Mbit (1,5 MByte)" }); };
                if (_sourceROM.IntCalcFileSize < 16) { list.Add(new ComboBoxExpandROMList { Id = 16, Name = "16 Mbit (2 MByte) | 27C160" }); };
                if (_sourceROM.IntCalcFileSize < 20 && !checkBoxExpandMirroring.Checked) { list.Add(new ComboBoxExpandROMList { Id = 20, Name = "20 Mbit (2,5 MByte)" }); };
                if (_sourceROM.IntCalcFileSize < 24 && !checkBoxExpandMirroring.Checked) { list.Add(new ComboBoxExpandROMList { Id = 24, Name = "24 Mbit (3 MByte)" }); };
                if (_sourceROM.IntCalcFileSize < 28 && !checkBoxExpandMirroring.Checked) { list.Add(new ComboBoxExpandROMList { Id = 28, Name = "28 Mbit (3,5 MByte)" }); };
                if (_sourceROM.IntCalcFileSize < 32) { list.Add(new ComboBoxExpandROMList { Id = 32, Name = "32 Mbit (4 MByte) | 27C322" }); };
                if (_sourceROM.IntCalcFileSize < 48 && !_sourceROM.IsBSROM) { list.Add(new ComboBoxExpandROMList { Id = 48, Name = "48 Mbit (6 MByte)" }); };
                if (_sourceROM.IntCalcFileSize < 64 && !_sourceROM.IsBSROM) { list.Add(new ComboBoxExpandROMList { Id = 64, Name = "64 Mbit (8 MByte)" }); };

                comboBoxExpandROM.DataSource = list;
                comboBoxExpandROM.DisplayMember = "Name";
                comboBoxExpandROM.ValueMember = "Id";

                buttonExpandROM.Enabled = true;
                comboBoxExpandROM.Enabled = true;
            }
        }

        private void RefreshComboBoxSplitROMList()
        {
            List<ComboBoxSplitROMList> list = new List<ComboBoxSplitROMList>();

            if (_sourceROM.IntCalcFileSize % 64 == 0 && _sourceROM.IntCalcFileSize > 64) { list.Add(new ComboBoxSplitROMList { Id = 64, Name = "64 Mbit (8 MByte)" }); };
            if (_sourceROM.IntCalcFileSize % 32 == 0 && _sourceROM.IntCalcFileSize > 32) { list.Add(new ComboBoxSplitROMList { Id = 32, Name = "32 Mbit (4 MByte) | 27C322" }); };
            if (_sourceROM.IntCalcFileSize % 16 == 0 && _sourceROM.IntCalcFileSize > 16) { list.Add(new ComboBoxSplitROMList { Id = 16, Name = "16 Mbit (2 MByte) | 27C160" }); };
            if (_sourceROM.IntCalcFileSize % 8 == 0 && _sourceROM.IntCalcFileSize > 8) { list.Add(new ComboBoxSplitROMList { Id = 8, Name = "8 Mbit (1 MByte) | 27C801" }); };
            if (_sourceROM.IntCalcFileSize % 4 == 0 && _sourceROM.IntCalcFileSize > 4) { list.Add(new ComboBoxSplitROMList { Id = 4, Name = "4 Mbit (512 kByte) | 27C4001" }); };
            if (_sourceROM.IntCalcFileSize % 2 == 0 && _sourceROM.IntCalcFileSize > 2) { list.Add(new ComboBoxSplitROMList { Id = 2, Name = "2 Mbit (256 kByte) | 27C2001" }); };
            list.Add(new ComboBoxSplitROMList { Id = 1, Name = "1 Mbit (128 kByte) | 27C1001" });

            comboBoxSplitROM.DataSource = list;
            comboBoxSplitROM.DisplayMember = "Name";
            comboBoxSplitROM.ValueMember = "Id";

            buttonSplitROM.Enabled = true;
            comboBoxSplitROM.Enabled = true;
        }

        private void FillComboBoxCountryRegionList()
        {
            for (int l = 0; l < SNESROMList.CountryRegion.GetLength(0); l++)
            {
                _listCountryRegion.Add(new ComboBoxCountryRegionList { Id = l, Name = $"{SNESROMList.CountryRegion[l, 0]} | {SNESROMList.CountryRegion[l, 1]}" });
            }
        }

        private void CheckBoxExpandMirroring_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxExpandMirroring.Checked)
            {
                Properties.Settings.Default.MirrorROMSetting = true;
            }

            else if (!checkBoxExpandMirroring.Checked)
            {
                Properties.Settings.Default.MirrorROMSetting = false;
            }

            Properties.Settings.Default.Save();
            RefreshComboBoxExpandROMList();
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
            public int Id { get; set; }
            public string Name { get; set; }
        }

        class ComboBoxSplitROMList
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
            if (_savedFileHash != null && _sourceROM.SourceROM != null)
            {
                // Generate actual ROM
                byte[] trackingROM;

                if (_saveWithHeader)
                {
                    // Merge header with ROM
                    trackingROM = new byte[_sourceROM.SourceROMSMCHeader.Length + _sourceROM.SourceROM.Length];

                    Buffer.BlockCopy(_sourceROM.SourceROMSMCHeader, 0, trackingROM, 0, _sourceROM.SourceROMSMCHeader.Length);
                    Buffer.BlockCopy(_sourceROM.SourceROM, 0, trackingROM, _sourceROM.SourceROMSMCHeader.Length, _sourceROM.SourceROM.Length);
                }

                else
                {
                    trackingROM = new byte[_sourceROM.SourceROM.Length];
                    Buffer.BlockCopy(_sourceROM.SourceROM, 0, trackingROM, 0, _sourceROM.SourceROM.Length);
                }

                Crc32 trackingROMCRC32 = new Crc32();
                string trackingROMHash = null;

                foreach (byte singleByte in trackingROMCRC32.ComputeHash(trackingROM))
                {
                    trackingROMHash += singleByte.ToString("X2").ToUpper();
                }

                if (_savedFileHash != trackingROMHash)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to save your progress before closing?", "Attention!", MessageBoxButtons.YesNoCancel);

                    if (dialogResult == DialogResult.Yes)
                    {
                        // Generate ROM, write to file and store copy for dirty tracking
                        Save(_sourceROM.ROMFullPath, "ROM file has successfully been saved to: " + _sourceROM.ROMFullPath, _saveWithHeader);
                        e.Cancel = false;
                    }

                    else if (dialogResult == DialogResult.No)
                    {
                        e.Cancel = false;
                    }

                    else if (dialogResult == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void ResetOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("This will reset all your saved settings and\n" +
                                                        "restore all your suppressed dialog messages.\n\n" +
                                                        "Do you want to proceed?", "Attention!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                checkBoxExpandMirroring.Checked = false;
                checkBoxScan.Checked = false;
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