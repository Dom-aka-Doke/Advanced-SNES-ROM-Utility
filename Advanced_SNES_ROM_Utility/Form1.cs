using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Advanced_SNES_ROM_Utility
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            buttonAddHeader.Enabled = false;
            buttonRemoveHeader.Enabled = false;
            buttonSwapBinROM.Enabled = false;
            buttonSplitROM.Enabled = false;
            buttonExpandROM.Enabled = false;
            buttonDeinterleave.Enabled = false;
            buttonFixChksm.Enabled = false;
            buttonFixRegion.Enabled = false;
            comboBoxSplitROM.Enabled = false;
            comboBoxExpandROM.Enabled = false;
            buttonEdit.Enabled = false;
            textBoxROMName.Text = "Select a ROM File!";

            // Get the right settings for option menu
            if (Properties.Settings.Default.AutoFixIntROMSize) { toolStripMenuItemAutoFixROM.Checked = true; } else { toolStripMenuItemAutoFixROM.Checked = false; }
        }

        // Get option settings
        bool autoFixROMSize = Properties.Settings.Default.AutoFixIntROMSize;

        // Create empty ROM
        ROM sourceROM;

        private void buttonSelectROM_Click(object sender, EventArgs e)
        {
            // Select ROM file dialogue
            OpenFileDialog selectROMDialog = new OpenFileDialog();

            selectROMDialog.Filter = "SNES/SFC ROMs (*.smc;*.swc;*;*.sfc;*.fig)|*.smc;*.swc*;*.sfc;*.fig|" +
                                     "All Files (*.*)|*.*";

            // If successfully selected a ROM file...
            if (selectROMDialog.ShowDialog() == DialogResult.OK)
            {
                // Reset labels
                labelSRAM.Text = "(S)RAM";

                // Clear content of combo boxes and disable
                comboBoxExpandROM.DataSource = null;
                comboBoxSplitROM.DataSource = null;
                comboBoxExpandROM.Items.Clear();
                comboBoxSplitROM.Items.Clear();
                comboBoxExpandROM.Enabled = false;
                comboBoxSplitROM.Enabled = false;

                // Disable all buttons on new load
                buttonAddHeader.Enabled = false;
                buttonRemoveHeader.Enabled = false;
                buttonExpandROM.Enabled = false;
                buttonSplitROM.Enabled = false;
                buttonDeinterleave.Enabled = false;
                buttonFixChksm.Enabled = false;
                buttonFixRegion.Enabled = false;
                buttonSwapBinROM.Enabled = false;
                buttonEdit.Enabled = true;

                // Create new ROM
                sourceROM = new ROM(@selectROMDialog.FileName);

                // Set path into label
                textBoxROMName.Text = @selectROMDialog.FileName;

                // Enable / disable button for adding header
                if (sourceROM.SMCHeader == 0) { buttonAddHeader.Enabled = true; } else { buttonRemoveHeader.Enabled = true; }

                // If option auto fix ROM size is enabled
                if (autoFixROMSize)
                {
                    // Some Hacks may have an odd size in their header, so we should fix that by taking the right value
                    if ((sourceROM.IntROMSize < sourceROM.CalculatedFileSize) && !sourceROM.IsBSROM)
                    {
                        sourceROM.IntROMSize = 1;

                        while (sourceROM.IntROMSize < sourceROM.CalculatedFileSize)
                        {
                            sourceROM.IntROMSize *= 2;
                        }

                        byte byteROMSizeValue = Convert.ToByte(sourceROM.IntROMSize);
                        byte[] byteArrayROMSizeValue = new byte[1];
                        byteArrayROMSizeValue[0] = byteROMSizeValue;

                        switch (byteArrayROMSizeValue[0])
                        {
                            case 0x01: byteArrayROMSizeValue[0] = 0x07; break;
                            case 0x02: byteArrayROMSizeValue[0] = 0x08; break;
                            case 0x04: byteArrayROMSizeValue[0] = 0x09; break;
                            case 0x08: byteArrayROMSizeValue[0] = 0x0A; break;
                            case 0x10: byteArrayROMSizeValue[0] = 0x0B; break;
                            case 0x20: byteArrayROMSizeValue[0] = 0x0C; break;
                            case 0x40: byteArrayROMSizeValue[0] = 0x0D; break;
                        }

                        // Set new ROM size value
                        Buffer.BlockCopy(byteArrayROMSizeValue, 0, sourceROM.SourceROM, (int)sourceROM.ROMHeaderOffset + 0x27, 1);
                    }
                }

                // Set (S)RAM information
                if (sourceROM.ByteSRAMSize > 0x00)
                {
                    labelSRAM.Text = "SRAM";
                }

                else if (sourceROM.ByteExRAMSize > 0x00)
                {
                    labelSRAM.Text = "RAM";
                }

                // Check if ROM is swappable
                if (sourceROM.SourceROM.Length % 1048576 == 0)
                {
                    buttonSwapBinROM.Enabled = true;
                }

                // Check if ROM can be expanded
                if (sourceROM.CalculatedFileSize < 32)
                {
                    List<comboBoxExpandROMList> list = new List<comboBoxExpandROMList>();

                    if (sourceROM.CalculatedFileSize < 1) { list.Add(new comboBoxExpandROMList { Id = 1, Name = "1 Mbit (128 kByte) | 27C1001" }); };
                    if (sourceROM.CalculatedFileSize < 2) { list.Add(new comboBoxExpandROMList { Id = 2, Name = "2 Mbit (256 kByte) | 27C2001" }); };
                    if (sourceROM.CalculatedFileSize < 4) { list.Add(new comboBoxExpandROMList { Id = 4, Name = "4 Mbit (512 kByte) | 274001" }); };
                    if (sourceROM.CalculatedFileSize < 8) { list.Add(new comboBoxExpandROMList { Id = 8, Name = "8 Mbit (1 MByte) | 27C801" }); };
                    //if (sourceROM.CalcFileSize < 12) { list.Add(new comboBoxExpandROMList { Id = 12, Name = "12 Mbit (1,5 MByte)" }); };
                    if (sourceROM.CalculatedFileSize < 16) { list.Add(new comboBoxExpandROMList { Id = 16, Name = "16 Mbit (2 MByte) | 27C160" }); };
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

                // Check if ROM can be splittet
                if (sourceROM.CalculatedFileSize > 1)
                {
                    List<comboBoxSplitROMList> list = new List<comboBoxSplitROMList>();

                    if (sourceROM.CalculatedFileSize % 32 == 0 && sourceROM.CalculatedFileSize > 32) { list.Add(new comboBoxSplitROMList { Id = 32, Name = "32 Mbit (4 MByte) | 27C322" }); };
                    if (sourceROM.CalculatedFileSize % 16 == 0 && sourceROM.CalculatedFileSize > 16) { list.Add(new comboBoxSplitROMList { Id = 16, Name = "16 Mbit (2 MByte) | 27C160" }); };
                    if (sourceROM.CalculatedFileSize % 8 == 0 && sourceROM.CalculatedFileSize > 8) { list.Add(new comboBoxSplitROMList { Id = 8, Name = "8 Mbit (1 MByte) | 27C801" }); };
                    if (sourceROM.CalculatedFileSize % 4 == 0 && sourceROM.CalculatedFileSize > 4) { list.Add(new comboBoxSplitROMList { Id = 4, Name = "4 Mbit (512 kByte) | 27C4001" }); };
                    if (sourceROM.CalculatedFileSize % 2 == 0 && sourceROM.CalculatedFileSize > 2) { list.Add(new comboBoxSplitROMList { Id = 2, Name = "2 Mbit (256 kByte) | 27C2001" }); };
                    list.Add(new comboBoxSplitROMList { Id = 1, Name = "1 Mbit (128 kByte) | 27C1001" });

                    comboBoxSplitROM.DataSource = list;
                    comboBoxSplitROM.DisplayMember = "Name";
                    comboBoxSplitROM.ValueMember = "Id";

                    buttonSplitROM.Enabled = true;
                    comboBoxSplitROM.Enabled = true;
                }

                // Check if checksum can be fixed
                if (!sourceROM.ReadChksm.SequenceEqual(sourceROM.CalcChksm))
                {
                    buttonFixChksm.Enabled = true;
                }

                // Check if ROM contains region locks
                if(sourceROM.UnlockRegion(sourceROM.SourceROM, sourceROM.SourceROMSMCHeader, false, sourceROM.ROMSavePath, sourceROM.ROMName, sourceROM.StringRegion))
                {
                    buttonFixRegion.Enabled = true;
                }

                // Check if ROM is interleaved
                if (sourceROM.IsInterleaved)
                {
                    // Disable not needed buttons
                    buttonAddHeader.Enabled = false;
                    buttonSwapBinROM.Enabled = false;
                    buttonDeinterleave.Enabled = true;
                    buttonFixChksm.Enabled = false;
                }

                RefreshLabels();
            }
        }

        private void ButtonAddHeader_Click(object sender, EventArgs e)
        {
            // Just add 512 bytes of 0x00 to the beginning of the file
            byte[] romHeader = new byte[512];
            byte[] sourceROMHeadered = new byte[sourceROM.SourceROM.Length + romHeader.Length];

            foreach(byte singleByte in romHeader)
            {
                romHeader[singleByte] = 0x00;
            }

            Buffer.BlockCopy(romHeader, 0, sourceROMHeadered, 0, romHeader.Length);
            Buffer.BlockCopy(sourceROM.SourceROM, 0, sourceROMHeadered, romHeader.Length, sourceROM.SourceROM.Length);

            // Save file with header
            File.WriteAllBytes(@sourceROM.ROMSavePath + @"\" + sourceROM.ROMName + "_[headered]" + ".smc", sourceROMHeadered);
            buttonAddHeader.Enabled = false;

            MessageBox.Show("Header successfully added!\n\nFile saved to: '" + @sourceROM.ROMSavePath + @"\" + sourceROM.ROMName + "_[headered]" + ".smc'");
        }

        private void ButtonRemoveHeader_Click(object sender, EventArgs e)
        {
            // Save file without header
            File.WriteAllBytes(@sourceROM.ROMSavePath + @"\" + sourceROM.ROMName + "_[no_header]" + ".sfc", sourceROM.SourceROM);
            buttonRemoveHeader.Enabled = false;

            MessageBox.Show("Header successfully removed!\n\nFile saved to: '" + @sourceROM.ROMSavePath + @"\" + sourceROM.ROMName + "_[no_header]" + ".sfc'");
        }

        private void ButtonSwapBinROM_Click(object sender, EventArgs e)
        {
            // Swap ROM - If ROM size is multiple of 8 Mbit then additionally split into chunks
            int romChunks = sourceROM.SourceROM.Length / 1048576;

            if(romChunks > 1)
            {
                // Define size for single ROM file (8 Mbit)
                int chunkSize = 1048576;

                for (int index = 0; index < romChunks; index++)
                {
                    string romChunkName = sourceROM.ROMName + "_[" + index + "]";
                    byte[] sourceROMChunk = new byte[chunkSize];

                    Buffer.BlockCopy(sourceROM.SourceROM, index * chunkSize, sourceROMChunk, 0, chunkSize);

                    sourceROM.SwapBin(sourceROMChunk, sourceROM.ROMSavePath, romChunkName);
                }
            }

            else
            {
                sourceROM.SwapBin(sourceROM.SourceROM, sourceROM.ROMSavePath, sourceROM.ROMName);
            }
            
            buttonSwapBinROM.Enabled = false;
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
            File.WriteAllBytes(@sourceROM.ROMSavePath + @"\" + sourceROM.ROMName + "_[expanded]" + ".bin", expandedROM);

            buttonExpandROM.Enabled = false;
            comboBoxExpandROM.Enabled = false;

            MessageBox.Show("ROM successfully expanded!\n\nFile saved to: '" + @sourceROM.ROMSavePath + @"\" + sourceROM.ROMName + "_[expanded]" + ".bin'\n\nIn case there was a header, it has been removed!");
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
                File.WriteAllBytes(@sourceROM.ROMSavePath + @"\" + romChunkName + "_[split]" + ".bin", splitROM);
                MessageBox.Show("ROM successfully splittet!\n\nFile saved to: '" + @sourceROM.ROMSavePath + @"\" + romChunkName + "_[split]" + ".bin'\n\nIn case there was a header, it has been removed!");
            }

            buttonSplitROM.Enabled = false;
            comboBoxSplitROM.Enabled = false;
        }

        private void ButtonFixChksm_Click(object sender, EventArgs e)
        {
            sourceROM.FixChecksum();
            sourceROM.Initialize();
            RefreshLabels();
            buttonFixChksm.Enabled = false;

            /*
            // Save checksum fixed file with header
            if (sourceROM.SourceROMSMCHeader != null)
            {
                byte[] checksumFixedHeaderedROM = new byte[sourceROM.SourceROMSMCHeader.Length + checksumFixedROM.Length];

                Buffer.BlockCopy(sourceROM.SourceROMSMCHeader, 0, checksumFixedHeaderedROM, 0, sourceROM.SourceROMSMCHeader.Length);
                Buffer.BlockCopy(checksumFixedROM, 0, checksumFixedHeaderedROM, sourceROM.SourceROMSMCHeader.Length, checksumFixedROM.Length);

                File.WriteAllBytes(@sourceROM.ROMSavePath + @"\" + sourceROM.ROMName + "_[checksum_fixed]" + ".sfc", checksumFixedHeaderedROM);
                MessageBox.Show("Checksum successfully fixed!\n\nFile saved to: '" + @sourceROM.ROMSavePath + @"\" + sourceROM.ROMName + "_[checksum_fixed]" + ".sfc'");
            }

            else
            {
                // Save checksum fixed file without header
                File.WriteAllBytes(@sourceROM.ROMSavePath + @"\" + sourceROM.ROMName + "_[checksum_fixed]" + ".sfc", checksumFixedROM);
                MessageBox.Show("Checksum successfully fixed!\n\nFile saved to: '" + @sourceROM.ROMSavePath + @"\" + sourceROM.ROMName + "_[checksum_fixed]" + ".sfc'");
            }
            */
        }

        private void buttonFixRegion_Click(object sender, EventArgs e)
        {
            sourceROM.UnlockRegion(sourceROM.SourceROM, sourceROM.SourceROMSMCHeader, true, sourceROM.ROMSavePath, sourceROM.ROMName, sourceROM.StringRegion);
            buttonFixRegion.Enabled = false;
        }

        private void buttonDeinterleave_Click(object sender, EventArgs e)
        {
            sourceROM.Deinterlave(sourceROM.SourceROM, sourceROM.SourceROMSMCHeader, sourceROM.CalculatedFileSize, sourceROM.ROMSavePath, sourceROM.ROMName);
            buttonDeinterleave.Enabled = false;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            Form3 editROMInformation = new Form3(sourceROM.SourceROM, sourceROM.SourceROMSMCHeader, sourceROM.ROMHeaderOffset, sourceROM.StringTitle, sourceROM.ByteArrayTitle, sourceROM.StringVersion, sourceROM.ByteCountry, sourceROM.StringCompany, sourceROM.IntROMSize, sourceROM.CalculatedFileSize, sourceROM.ROMSavePath, sourceROM.ROMName, sourceROM.IsBSROM);
            editROMInformation.Show();
        }

        private void RefreshLabels()
        {
            // Set labels
            labelGetTitle.Text = sourceROM.StringTitle;
            labelGetMapMode.Text = sourceROM.StringMapMode;
            labelGetROMType.Text = sourceROM.StringROMType;
            labelGetROMSize.Text = sourceROM.StringROMSize;
            labelGetSRAM.Text = sourceROM.StringRAMSize;
            labelGetFileSize.Text = sourceROM.CalculatedFileSize.ToString() + " Mbit (" + ((float)sourceROM.CalculatedFileSize / 8) + " MByte)";
            labelGetSMCHeader.Text = sourceROM.StringSMCHeader;
            labelGetROMSpeed.Text = sourceROM.StringROMSpeed;
            labelGetCountry.Text = sourceROM.StringCountry;
            labelGetRegion.Text = sourceROM.StringRegion;
            labelGetCompany.Text = sourceROM.StringCompany;
            labelGetIntChksm.Text = BitConverter.ToString(sourceROM.ReadChksm).Replace("-", "");
            labelGetIntInvChksm.Text = BitConverter.ToString(sourceROM.ReadInvChksm).Replace("-", "");
            labelGetCalcChksm.Text = BitConverter.ToString(sourceROM.CalcChksm).Replace("-", "");
            labelGetCalcInvChksm.Text = BitConverter.ToString(sourceROM.CalcInvChksm).Replace("-", "");
            labelGetVersion.Text = sourceROM.StringVersion;
            labelGetCRC32Chksm.Text = sourceROM.CRC32Hash;
        }

        private void toolStripMenuItemAutoFixROM_Click(object sender, EventArgs e)
        {
            // Change to false
            if (autoFixROMSize == true)
            {
                autoFixROMSize = false;
                toolStripMenuItemAutoFixROM.Checked = false;

                // Save in settings
                Properties.Settings.Default.AutoFixIntROMSize = false;
            }

            // Change to true
            else if (autoFixROMSize == false)
            {
                autoFixROMSize = true;
                toolStripMenuItemAutoFixROM.Checked = true;

                // Save in settings
                Properties.Settings.Default.AutoFixIntROMSize = true;
            }

            // Save settings
            Properties.Settings.Default.Save();

            // Restart Application
            DialogResult dialogRestart = MessageBox.Show("Change only takes effect after a restart!\nRestart now?", "Restart Application", MessageBoxButtons.YesNo);

            if (dialogRestart == DialogResult.Yes)
            {
                Application.Restart();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 openAboutBox = new Form4();
            openAboutBox.Show();
        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 openManualBox = new Form5();
            openManualBox.Show();
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


    }
}