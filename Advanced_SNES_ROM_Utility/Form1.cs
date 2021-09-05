using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advanced_SNES_ROM_Utility
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Prepare some global variables
        bool saveWithHeader;

        Crc32 savedFileCRC32 = new Crc32();
        string savedFileHash;

        // Create combo box for selecting country and region
        List<comboBoxCountryRegionList> listCountryRegion = new List<comboBoxCountryRegionList>();

        // Create empty ROM
        SNESROM sourceROM;

        private void buttonSelectROM_Click(object sender, EventArgs e)
        {
            // Select ROM file dialogue
            OpenFileDialog selectROMDialog = new OpenFileDialog();

            selectROMDialog.Filter = "SNES/SFC ROMs (*.smc;*.swc;*;*.sfc;*.fig)|*.smc;*.swc*;*.sfc;*.fig|" +
                                     "All Files (*.*)|*.*";

            // If successfully selected a ROM file...
            if (selectROMDialog.ShowDialog() == DialogResult.OK)
            {
                // Clear content of combo boxes and disable
                comboBoxExpandROM.DataSource = null;
                comboBoxSplitROM.DataSource = null;
                comboBoxExpandROM.Items.Clear();
                comboBoxSplitROM.Items.Clear();
                comboBoxExpandROM.Enabled = false;
                comboBoxSplitROM.Enabled = false;

                // Create new ROM
                sourceROM = new SNESROM(@selectROMDialog.FileName);

                // Store CRC32 for dirty tracking
                savedFileHash = GetCRC32FromFile(@selectROMDialog.FileName);

                // Initialize combo box for country and region
                listCountryRegion.Add(new comboBoxCountryRegionList { Id = 0, Name = "Japan | NTSC" });
                listCountryRegion.Add(new comboBoxCountryRegionList { Id = 1, Name = "USA | NTSC" });
                listCountryRegion.Add(new comboBoxCountryRegionList { Id = 2, Name = "Europe/Oceania/Asia | PAL" });
                listCountryRegion.Add(new comboBoxCountryRegionList { Id = 3, Name = "Sweden/Scandinavia | PAL" });
                listCountryRegion.Add(new comboBoxCountryRegionList { Id = 4, Name = "Finland | PAL" });
                listCountryRegion.Add(new comboBoxCountryRegionList { Id = 5, Name = "Denmark | PAL" });
                listCountryRegion.Add(new comboBoxCountryRegionList { Id = 6, Name = "France | SECAM (PAL-like, 50 Hz)" });
                listCountryRegion.Add(new comboBoxCountryRegionList { Id = 7, Name = "Netherlands | PAL" });
                listCountryRegion.Add(new comboBoxCountryRegionList { Id = 8, Name = "Spain | PAL" });
                listCountryRegion.Add(new comboBoxCountryRegionList { Id = 9, Name = "Germany/Austria/Switzerland | PAL" });
                listCountryRegion.Add(new comboBoxCountryRegionList { Id = 10, Name = "China/Hong Kong | PAL" });
                listCountryRegion.Add(new comboBoxCountryRegionList { Id = 11, Name = "Indonesia | PAL" });
                listCountryRegion.Add(new comboBoxCountryRegionList { Id = 12, Name = "South Korea | NTSC" });
                listCountryRegion.Add(new comboBoxCountryRegionList { Id = 14, Name = "Canada | NTSC" });
                listCountryRegion.Add(new comboBoxCountryRegionList { Id = 15, Name = "Brazil | PAL-M (NTSC-like, 60 Hz)" });
                listCountryRegion.Add(new comboBoxCountryRegionList { Id = 16, Name = "Australia | PAL" });

                comboBoxCountryRegion.DataSource = listCountryRegion;
                comboBoxCountryRegion.DisplayMember = "Name";
                comboBoxCountryRegion.ValueMember = "Id";

                int selectedCompanyRegion = sourceROM.ByteCountry;
                if (selectedCompanyRegion > 12) { selectedCompanyRegion--; }

                comboBoxCountryRegion.SelectedIndex = selectedCompanyRegion;

                // Check if ROM can be expanded
                if (sourceROM.IntCalcFileSize < 32)
                {
                    List<comboBoxExpandROMList> list = new List<comboBoxExpandROMList>();

                    if (sourceROM.IntCalcFileSize < 1) { list.Add(new comboBoxExpandROMList { Id = 1, Name = "1 Mbit (128 kByte) | 27C1001" }); };
                    if (sourceROM.IntCalcFileSize < 2) { list.Add(new comboBoxExpandROMList { Id = 2, Name = "2 Mbit (256 kByte) | 27C2001" }); };
                    if (sourceROM.IntCalcFileSize < 4) { list.Add(new comboBoxExpandROMList { Id = 4, Name = "4 Mbit (512 kByte) | 274001" }); };
                    if (sourceROM.IntCalcFileSize < 8) { list.Add(new comboBoxExpandROMList { Id = 8, Name = "8 Mbit (1 MByte) | 27C801" }); };
                    //if (sourceROM.CalcFileSize < 12) { list.Add(new comboBoxExpandROMList { Id = 12, Name = "12 Mbit (1,5 MByte)" }); };
                    if (sourceROM.IntCalcFileSize < 16) { list.Add(new comboBoxExpandROMList { Id = 16, Name = "16 Mbit (2 MByte) | 27C160" }); };
                    //if (sourceROM.CalcFileSize < 20) { list.Add(new comboBoxExpandROMList { Id = 20, Name = "20 Mbit (2,5 MByte)" }); };
                    //if (sourceROM.CalcFileSize < 24) { list.Add(new comboBoxExpandROMList { Id = 24, Name = "24 Mbit (3 MByte)" }); };
                    //if (sourceROM.CalcFileSize < 28) { list.Add(new comboBoxExpandROMList { Id = 28, Name = "28 Mbit (3,5 MByte)" }); };
                    list.Add(new comboBoxExpandROMList { Id = 32, Name = "32 Mbit (4 MByte) | 27C322" });

                    comboBoxExpandROM.DataSource = list;
                    comboBoxExpandROM.DisplayMember = "Name";
                    comboBoxExpandROM.ValueMember = "Id";

                    buttonExpandROM.Enabled = true;
                    comboBoxExpandROM.Enabled = true;
                }

                else
                {
                    buttonExpandROM.Enabled = false;
                    comboBoxExpandROM.Enabled = false;
                }

                // Check if ROM can be splittet
                if (sourceROM.IntCalcFileSize > 1)
                {
                    List<comboBoxSplitROMList> list = new List<comboBoxSplitROMList>();

                    if (sourceROM.IntCalcFileSize % 32 == 0 && sourceROM.IntCalcFileSize > 32) { list.Add(new comboBoxSplitROMList { Id = 32, Name = "32 Mbit (4 MByte) | 27C322" }); };
                    if (sourceROM.IntCalcFileSize % 16 == 0 && sourceROM.IntCalcFileSize > 16) { list.Add(new comboBoxSplitROMList { Id = 16, Name = "16 Mbit (2 MByte) | 27C160" }); };
                    if (sourceROM.IntCalcFileSize % 8 == 0 && sourceROM.IntCalcFileSize > 8) { list.Add(new comboBoxSplitROMList { Id = 8, Name = "8 Mbit (1 MByte) | 27C801" }); };
                    if (sourceROM.IntCalcFileSize % 4 == 0 && sourceROM.IntCalcFileSize > 4) { list.Add(new comboBoxSplitROMList { Id = 4, Name = "4 Mbit (512 kByte) | 27C4001" }); };
                    if (sourceROM.IntCalcFileSize % 2 == 0 && sourceROM.IntCalcFileSize > 2) { list.Add(new comboBoxSplitROMList { Id = 2, Name = "2 Mbit (256 kByte) | 27C2001" }); };
                    list.Add(new comboBoxSplitROMList { Id = 1, Name = "1 Mbit (128 kByte) | 27C1001" });

                    comboBoxSplitROM.DataSource = list;
                    comboBoxSplitROM.DisplayMember = "Name";
                    comboBoxSplitROM.ValueMember = "Id";

                    buttonSplitROM.Enabled = true;
                    comboBoxSplitROM.Enabled = true;
                }

                else
                {
                    buttonSplitROM.Enabled = false;
                    comboBoxSplitROM.Enabled = false;
                }

                // Check if ROM contains region locks
                if (sourceROM.UnlockRegion(false)) 
                {
                    buttonFixRegion.Enabled = true;
                }
                
                else
                {
                    buttonFixRegion.Enabled = false;
                }

                // Enable / disable text and combo boxes
                if (!textBoxTitle.Enabled) { textBoxTitle.Enabled = true; }
                if (sourceROM.IsBSROM) { comboBoxCountryRegion.Enabled = false; } else { comboBoxCountryRegion.Enabled = true; }
                textBoxTitle.MaxLength = sourceROM.StringTitle.Length;
                if (!textBoxVersion.Enabled) { textBoxVersion.Enabled = true; }

                // Load values into labels and enable / disable buttons
                RefreshLabelsAndButtons();
            }
        }

        private void ButtonAddHeader_Click(object sender, EventArgs e)
        {
            sourceROM.AddHeader();
            RefreshLabelsAndButtons();
        }

        private void ButtonRemoveHeader_Click(object sender, EventArgs e)
        {
            sourceROM.RemoveHeader();
            RefreshLabelsAndButtons();
        }

        private void ButtonSwapBinROM_Click(object sender, EventArgs e)
        {
            sourceROM.SwapBin();
            MessageBox.Show("ROM successfully swapped!\n\nFile(s) saved to: '" + sourceROM.ROMFolder + "\n\nIn case there was a header, it has been removed!");
        }

        private void ButtonExpandROM_Click(object sender, EventArgs e)
        {
            // Create new ROM for expanding
            int sizeExpandedROM = (int)comboBoxExpandROM.SelectedValue;
            byte[] expandedROM = new byte[sizeExpandedROM * 131072];

            foreach (byte singleByte in expandedROM)
            {
                expandedROM[singleByte] = 0x00;
            }

            Buffer.BlockCopy(sourceROM.SourceROM, 0, expandedROM, 0, sourceROM.SourceROM.Length);

            // Save file without header
            File.WriteAllBytes(@sourceROM.ROMFolder + @"\" + sourceROM.ROMName + "_[expanded]" + ".bin", expandedROM);
            MessageBox.Show("ROM successfully expanded!\n\nFile saved to: '" + @sourceROM.ROMFolder + @"\" + sourceROM.ROMName + "_[expanded]" + ".bin'\n\nIn case there was a header, it has been removed!");
        }

        private void ButtonSplitROM_Click(object sender, EventArgs e)
        {
            int splitROMSize = (int)comboBoxSplitROM.SelectedValue;
            int romChunks = sourceROM.SourceROM.Length / (splitROMSize * 131072);

            for (int index = 0; index < romChunks; index++)
            {
                string romChunkName = sourceROM.ROMName + "_[" + index + "]";
                byte[] splitROM = new byte[splitROMSize * 131072];

                Buffer.BlockCopy(sourceROM.SourceROM, index * (splitROMSize * 131072), splitROM, 0, splitROMSize * 131072);

                // Save file split
                File.WriteAllBytes(@sourceROM.ROMFolder + @"\" + romChunkName + "_[split]" + ".bin", splitROM);
                MessageBox.Show("ROM successfully splittet!\n\nFile saved to: '" + @sourceROM.ROMFolder + @"\" + romChunkName + "_[split]" + ".bin'\n\nIn case there was a header, it has been removed!");
            }
        }

        private void buttonDeinterleave_Click(object sender, EventArgs e)
        {
            sourceROM.Deinterlave();
            RefreshLabelsAndButtons();
        }

        private void ButtonFixChksm_Click(object sender, EventArgs e)
        {
            sourceROM.FixChecksum();
            RefreshLabelsAndButtons();
        }

        private void buttonFixRegion_Click(object sender, EventArgs e)
        {
            sourceROM.UnlockRegion(true);
            RefreshLabelsAndButtons();
            // Set button manually, because RefreshLabelsAndButtons doesn't do that for performace reasons
            buttonFixRegion.Enabled = false;
        }

        private void buttonFixROMSize_Click(object sender, EventArgs e)
        {
            sourceROM.FixInternalROMSize();
            RefreshLabelsAndButtons();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Save(sourceROM.ROMFullPath, "ROM file has successfully been saved!", saveWithHeader);
        }

        private void buttonSaveAs_Click(object sender, EventArgs e)
        {
            // Save ROM file dialogue
            SaveFileDialog saveROMDialog = new SaveFileDialog();
            saveROMDialog.InitialDirectory = Path.GetFullPath(sourceROM.ROMFolder);
            saveROMDialog.FileName = Path.GetFileName(sourceROM.ROMFullPath);
            saveROMDialog.DefaultExt = Path.GetExtension(sourceROM.ROMFullPath);

            saveROMDialog.Filter = "SNES/SFC ROMs (*.smc;*.swc;*;*.sfc;*.fig)|*.smc;*.swc*;*.sfc;*.fig|" +
                                   "All Files (*.*)|*.*";

            if (saveROMDialog.ShowDialog() == DialogResult.OK)
            {
                // Use Save function for writing file
                Save(@saveROMDialog.FileName, "ROM file has successfully been saved to: " + @saveROMDialog.FileName, saveWithHeader);

                // Reload ROM
                sourceROM = new SNESROM(@saveROMDialog.FileName);
                RefreshLabelsAndButtons();
            }
        }

        private void Save(string filepath, string message, bool saveWithHeader)
        {
            if (saveWithHeader)
            {
                // Merge header with ROM
                byte[] haderedROM = new byte[sourceROM.SourceROMSMCHeader.Length + sourceROM.SourceROM.Length];

                Buffer.BlockCopy(sourceROM.SourceROMSMCHeader, 0, haderedROM, 0, sourceROM.SourceROMSMCHeader.Length);
                Buffer.BlockCopy(sourceROM.SourceROM, 0, haderedROM, sourceROM.SourceROMSMCHeader.Length, sourceROM.SourceROM.Length);

                // Write to file
                File.WriteAllBytes(filepath, haderedROM);
            }

            else
            {
                // Just write ROM to file
                File.WriteAllBytes(filepath, sourceROM.SourceROM);
            }

            // Store CRC32 for dirty tracking
            savedFileHash = GetCRC32FromFile(filepath);

            // Show message
            MessageBox.Show(message);
        }

        private void textBoxGetTitle_TextChanged(object sender, EventArgs e)
        {
            if (!sourceROM.StringTitle.Trim().Equals(textBoxTitle.Text.Trim()))
            {
                sourceROM.SetTitle(textBoxTitle.Text, textBoxTitle.MaxLength);
                RefreshLabelsAndButtons();
            }
        }

        private void textBoxGetVersion_Leave(object sender, EventArgs e)
        {
            if (sourceROM.StringVersion != "" && sourceROM.StringVersion.Trim() != textBoxVersion.Text.Trim())
            {
                string versionPattern = @"^([1]\.)([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|2[5][0-5])$";

                if (Regex.IsMatch(textBoxVersion.Text.Trim(), versionPattern))
                {
                    byte[] byteArrayVersion = new byte[1];
                    string[] splitVersion = textBoxVersion.Text.Split('.');
                    int intVersion = Int16.Parse(splitVersion[1]);
                    byteArrayVersion = BitConverter.GetBytes(intVersion);

                    sourceROM.SetVersion(byteArrayVersion[0]);
                    RefreshLabelsAndButtons();
                }

                else
                {
                    textBoxVersion.Text = sourceROM.StringVersion;
                    MessageBox.Show("Enter version number between 1.0 and 1.255");
                }
            }
        }

        private void comboBoxCountryRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!comboBoxCountryRegion.Enabled || comboBoxCountryRegion.SelectedIndex < 0) { return; }

            int selectedCountryRegion = (int)comboBoxCountryRegion.SelectedValue;
            byte byteCountryRegion = Convert.ToByte(selectedCountryRegion);
            byte[] byteArrayCountryRegion = new byte[1];
            byteArrayCountryRegion[0] = byteCountryRegion;

            if (sourceROM.ByteCountry != byteArrayCountryRegion[0])
            {
                sourceROM.SetCountryRegion(byteArrayCountryRegion[0]);
                RefreshLabelsAndButtons();
            }
        }

        private void RefreshLabelsAndButtons()
        {
            // Set text boxes
            textBoxROMName.Text = sourceROM.ROMFullPath;
            textBoxTitle.Text = sourceROM.StringTitle.Trim();
            textBoxVersion.Text = sourceROM.StringVersion;

            // Set labels
            labelGetMapMode.Text = sourceROM.StringMapMode;
            labelGetROMType.Text = sourceROM.StringROMType;
            labelGetROMSize.Text = sourceROM.StringROMSize;
            labelGetSRAM.Text = sourceROM.StringRAMSize;
            labelSRAM.Text = "(S)RAM"; if (sourceROM.ByteSRAMSize > 0x00) { labelSRAM.Text = "SRAM"; } else if (sourceROM.ByteExRAMSize > 0x00) { labelSRAM.Text = "RAM"; }
            labelGetFileSize.Text = sourceROM.IntCalcFileSize.ToString() + " Mbit (" + ((float)sourceROM.IntCalcFileSize / 8) + " MByte)";
            labelGetSMCHeader.Text = sourceROM.StringSMCHeader;
            labelGetROMSpeed.Text = sourceROM.StringROMSpeed;
            labelGetCompany.Text = sourceROM.StringCompany;
            labelGetIntChksm.Text = BitConverter.ToString(sourceROM.ByteArrayChecksum).Replace("-", "");
            labelGetIntInvChksm.Text = BitConverter.ToString(sourceROM.ByteArrayInvChecksum).Replace("-", "");
            labelGetCalcChksm.Text = BitConverter.ToString(sourceROM.ByteArrayCalcChecksum).Replace("-", "");
            labelGetCalcInvChksm.Text = BitConverter.ToString(sourceROM.ByteArrayCalcInvChecksum).Replace("-", "");
            labelGetCRC32Chksm.Text = sourceROM.CRC32Hash;

            // Set buttons
            if (!buttonSave.Enabled) { buttonSave.Enabled = true; }
            if (!buttonSaveAs.Enabled) { buttonSaveAs.Enabled = true; }
            if (!buttonIPS.Enabled) { buttonIPS.Enabled = true; }
            if (sourceROM.SourceROMSMCHeader == null) { buttonAddHeader.Enabled = true; buttonRemoveHeader.Enabled = false; saveWithHeader = false; } else { buttonAddHeader.Enabled = false; buttonRemoveHeader.Enabled = true; saveWithHeader = true; }
            if (sourceROM.SourceROM.Length % 1048576 == 0) { buttonSwapBinROM.Enabled = true; } else { buttonSwapBinROM.Enabled = false; }
            if ((sourceROM.IntROMSize < sourceROM.IntCalcFileSize) && !sourceROM.IsBSROM) { buttonFixROMSize.Enabled = true; } else { buttonFixROMSize.Enabled = false; }
            if (sourceROM.IsInterleaved) { buttonDeinterleave.Enabled = true; buttonFixChksm.Enabled = false; return; } else { buttonDeinterleave.Enabled = false; }
            if (!sourceROM.ByteArrayChecksum.SequenceEqual(sourceROM.ByteArrayCalcChecksum)) { buttonFixChksm.Enabled = true; } else { buttonFixChksm.Enabled = false; }
        }

        private string GetCRC32FromFile(string filepath)
        {
            string tempFileHash = null;

            using (FileStream fs = File.Open(filepath, FileMode.Open))
            {
                foreach (byte b in savedFileCRC32.ComputeHash(fs))
                {
                    tempFileHash += b.ToString("x2").ToUpper();
                }
            }

            return tempFileHash;
        }

        private void buttonIPS_Click(object sender, EventArgs e)
        {
            // Select IPS file dialogue
            OpenFileDialog selectIPSDialog = new OpenFileDialog();

            selectIPSDialog.Filter = "IPS Patch File (*.ips)|*.ips|" +
                                     "All Files (*.*)|*.*";

            // If successfully selected an IPS file...
            if (selectIPSDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] ipsPatchedSourceROM = sourceROM.ApplyIPSPatch(@selectIPSDialog.FileName);

                if (ipsPatchedSourceROM != null)
                {
                    sourceROM.SourceROM = ipsPatchedSourceROM;
                    sourceROM.UIntSMCHeader = 0;
                    sourceROM.SourceROMSMCHeader = null;
                    sourceROM.Initialize();
                    RefreshLabelsAndButtons();
                    MessageBox.Show("ROM has successfully been patched!");
                }

                else
                {
                    MessageBox.Show("Could not apply IPS patch! Please check if your patch is valid.");
                }
            }
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            Form5 helpForm = new Form5();
            helpForm.Show();
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            Form4 aboutForm = new Form4();
            aboutForm.Show();
        }

        class comboBoxExpandROMList
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        class comboBoxSplitROMList
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        class comboBoxCountryRegionList
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (savedFileHash != null && sourceROM.SourceROM != null)
            {
                // Generate actual ROM
                byte[] trackingROM;

                if (saveWithHeader)
                {
                    // Merge header with ROM
                    trackingROM = new byte[sourceROM.SourceROMSMCHeader.Length + sourceROM.SourceROM.Length];

                    Buffer.BlockCopy(sourceROM.SourceROMSMCHeader, 0, trackingROM, 0, sourceROM.SourceROMSMCHeader.Length);
                    Buffer.BlockCopy(sourceROM.SourceROM, 0, trackingROM, sourceROM.SourceROMSMCHeader.Length, sourceROM.SourceROM.Length);
                }

                else
                {
                    trackingROM = new byte[sourceROM.SourceROM.Length];
                    Buffer.BlockCopy(sourceROM.SourceROM, 0, trackingROM, 0, sourceROM.SourceROM.Length);
                }

                Crc32 trackingROMCRC32 = new Crc32();
                string trackingROMHash = null;

                foreach (byte singleByte in trackingROMCRC32.ComputeHash(trackingROM))
                {
                    trackingROMHash += singleByte.ToString("X2").ToUpper();
                }

                if (savedFileHash != trackingROMHash)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to save your progress before closing?", "Attention!", MessageBoxButtons.YesNoCancel);

                    if (dialogResult == DialogResult.Yes)
                    {
                        // Generate ROM, write to file and store copy for dirty tracking
                        Save(sourceROM.ROMFullPath, "ROM file has successfully been saved to: " + sourceROM.ROMFullPath, saveWithHeader);
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

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}