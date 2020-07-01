using System;
using System.Windows.Forms;
using System.IO;
using System.Text;
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

        byte[] sourceROM = null;
        byte[] sourceROMHeader = new byte[80];
        byte[] sourceROMSMCHeader = null;
        byte[] readROMSize = new byte[1];
        byte[] readTitle = null;
        string stringTitle = null;
        string stringVersion = null;
        uint smcHeader = 0;
        int intROMSize = 0;
        int calcFileSize = 0;
        string romPath = "";
        string romName = "";
        string romSavePath = "";
        byte[] country = new byte[1];
        string company = null;
        string stringRegion = "";
        byte[] calcChksm = new byte[2];
        byte[] calcInvChksm = new byte[2];
        byte[] readChksm = new byte[2];
        byte[] readInvChksm = new byte[2];
        uint romHeaderOffset = 0x7FB0;      // default is LoROM, because most known ROMs use this
        bool isInterleaved = false;
        bool isBSROM = false;

        // Get option settings
        bool autoFixROMSize = Properties.Settings.Default.AutoFixIntROMSize;

        private void buttonSelectROM_Click(object sender, EventArgs e)
        {
            // Select ROM file dialogue
            OpenFileDialog selectROMDialog = new OpenFileDialog();

            selectROMDialog.Filter = "SNES/SFC ROMs (*.smc;*.swc;*;*.sfc;*.fig)|*.smc;*.swc*;*.sfc;*.fig|" +
                                     "All Files (*.*)|*.*";

            // If successfully selected a ROM file...
            if (selectROMDialog.ShowDialog() == DialogResult.OK)
            {
                // Reset gloablly used variables
                sourceROMHeader = new byte[80];
                sourceROMSMCHeader = null;
                readTitle = new byte[21];
                stringTitle = "";
                readROMSize = new byte[1];
                intROMSize = 0;
                smcHeader = 0;
                calcFileSize = 0;
                calcChksm = new byte[2];
                calcInvChksm = new byte[2];
                readChksm = new byte[2];
                readInvChksm = new byte[2];
                country = new byte[1];
                company = "";
                stringRegion = "";
                romHeaderOffset = 0x7FB0;      // default is LoROM, because most known ROMs use this
                isInterleaved = false;
                isBSROM = false;

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

                // Define some variables
                romPath = @selectROMDialog.FileName;
                romName = Path.GetFileNameWithoutExtension(romPath);
                romSavePath = Path.GetDirectoryName(romPath);
                textBoxROMName.Text = romPath;
                sourceROM = File.ReadAllBytes(romPath);

                string stringSMCHeader = null;
                string stringMapMode = null;
                string stringROMType = null;
                string stringROMSpeed = null;
                string stringROMSize = null;
                string stringRAMSize = "No";
                string stringCountry = null;
                stringVersion = null;

                // Check for header and ignore it for further actions
                smcHeader = ROM.CheckSMCHeader(sourceROM);

                if (smcHeader == 0)
                {
                    stringSMCHeader = "No";

                    buttonAddHeader.Enabled = true;
                }

                else if (smcHeader > 0)
                {
                    stringSMCHeader = "Malformed";

                    sourceROMSMCHeader = new byte[smcHeader];
                    byte[] newSourceROM = new byte[sourceROM.Length - smcHeader];

                    Buffer.BlockCopy(sourceROM, 0, sourceROMSMCHeader, 0, (int)smcHeader);
                    Buffer.BlockCopy(sourceROM, (int)smcHeader, newSourceROM, 0, newSourceROM.Length);

                    sourceROM = newSourceROM;

                    buttonRemoveHeader.Enabled = true;
                }

                if (smcHeader == 512)
                {
                    stringSMCHeader = "Yes";
                }

                // Get Map Mode
                int mapModeScoreLoROM = ROM.GetMapModeScore(sourceROM, 0x7FB0, false);
                int mapModeScoreBSLoROM = ROM.GetMapModeScore(sourceROM, 0x7FB0, true);
                int mapModeScoreHiROM = ROM.GetMapModeScore(sourceROM, 0xFFB0, false);
                int mapModeScoreBSHiROM = ROM.GetMapModeScore(sourceROM, 0xFFB0, true);
                int mapModeScoreExLoROM = ROM.GetMapModeScore(sourceROM, 0x407FB0, false);
                int mapModeScoreExHiROM = ROM.GetMapModeScore(sourceROM, 0x40FFB0, false);

                if (mapModeScoreLoROM >= mapModeScoreHiROM && mapModeScoreLoROM >= mapModeScoreExLoROM && mapModeScoreLoROM >= mapModeScoreExHiROM)
                {
                    romHeaderOffset = 0x7FB0;

                    if (mapModeScoreBSLoROM > mapModeScoreLoROM)
                    {
                        isBSROM = true;
                    }
                }

                else if (mapModeScoreHiROM >= mapModeScoreExLoROM && mapModeScoreHiROM >= mapModeScoreExHiROM)
                {
                    romHeaderOffset = 0xFFB0;

                    if (mapModeScoreBSHiROM > mapModeScoreHiROM)
                    {
                        isBSROM = true;
                    }
                }

                else if (mapModeScoreExLoROM >= mapModeScoreExHiROM)
                {
                    romHeaderOffset = 0x407FB0;
                }

                else
                {
                    romHeaderOffset = 0x40FFB0;
                }

                // Load header
                Buffer.BlockCopy(sourceROM, (int)romHeaderOffset, sourceROMHeader, 0, 80);

                // Get values
                readTitle = ROM.ReadTitle(sourceROMHeader, isBSROM);
                stringTitle = Encoding.GetEncoding(932).GetString(readTitle);
                byte[] readROMType = ROM.ReadROMType(sourceROMHeader, isBSROM);
                byte[] readMapMode = ROM.ReadMapMode(sourceROMHeader, isBSROM);
                byte readROMSpeed = ROM.ReadROMSpeed(readMapMode[0]);
                readROMSize = ROM.ReadROMSize(sourceROMHeader, isBSROM);
                bool isNewHeader = ROM.CheckIsNewHeader(sourceROMHeader);
                byte[] readSRAMSize = ROM.ReadSRAMSize(sourceROMHeader, isBSROM);
                byte[] readExRAMSize = ROM.ReadExRAMSize(sourceROMHeader, isNewHeader, stringTitle, isBSROM);
                country = ROM.ReadCountryCode(sourceROMHeader, isBSROM);
                company = "0x" + ROM.ReadCompanyCode(sourceROMHeader).ToString("X4");
                readChksm = ROM.ReadChecksum(sourceROMHeader);
                readInvChksm = ROM.ReadInverseChecksum(sourceROMHeader);
                byte[] readVersion = ROM.ReadVersion(sourceROMHeader);
                calcFileSize = ROM.CalculateFileSize(sourceROM);
                string crc32Hash = ROM.CalculateCrc32Hash(sourceROM);

                // --- Do some conversion ---

                // Get map mode
                switch (readMapMode[0])
                {
                    case 0x20: stringMapMode = "LoROM"; break;
                    case 0x21: stringMapMode = "HiROM"; if (romHeaderOffset == 0x7FB0) { isInterleaved = true; }; break;
                    case 0x22: stringMapMode = "LoROM (SDD-1)"; break;
                    case 0x23: stringMapMode = "LoROM (SA-1)"; break;
                    case 0x25: stringMapMode = "ExHiROM"; if (romHeaderOffset == 0x7FB0) { isInterleaved = true; }; break;
                    case 0x30: stringMapMode = "LoROM"; break;
                    case 0x31: stringMapMode = "HiROM"; if (romHeaderOffset == 0x7FB0) { isInterleaved = true; }; break;
                    case 0x32: stringMapMode = "ExLoROM"; break;
                    case 0x33: stringMapMode = "LoROM (SA-1)"; break;
                    case 0x35: stringMapMode = "ExHiROM"; if (romHeaderOffset == 0x7FB0) { isInterleaved = true; }; break;
                    default: stringMapMode = "Unknown"; break;
                }

                // ROM that contains oversized title which overwrites the map mode byte, but actually is LoROM
                if (stringTitle.Equals("YUYU NO QUIZ DE GO!GO"))
                {
                    stringMapMode = "LoROM";

                    // If this ROM is not interleaved set it as not interleaved
                    if (romHeaderOffset == 0x7FB0)
                    {
                        isInterleaved = false;
                    }
                }

                // Get ROM size
                switch (readROMSize[0])
                {
                    case 0x07: stringROMSize = "1 Mbit"; intROMSize = 1; break;
                    case 0x08: stringROMSize = "2 Mbit"; intROMSize = 2; break;
                    case 0x09: stringROMSize = "4 Mbit"; intROMSize = 4; break;
                    case 0x0A: stringROMSize = "8 Mbit"; intROMSize = 8; break;
                    case 0x0B: stringROMSize = "16 Mbit"; intROMSize = 16; break;
                    case 0x0C: stringROMSize = "32 Mbit"; intROMSize = 32; break;
                    case 0x0D: stringROMSize = "64 Mbit"; intROMSize = 64; break;
                    default: stringROMSize = "Unknown"; break;
                }

                // If option auto fix ROM size is enabled
                if (autoFixROMSize)
                {
                    // Some Hacks may have an odd size in their header, so we should fix that by taking the right value
                    if ((intROMSize < calcFileSize) && !isBSROM)
                    {
                        intROMSize = 1;

                        while (intROMSize < calcFileSize)
                        {
                            intROMSize *= 2;
                        }

                        byte byteROMSizeValue = Convert.ToByte(intROMSize);
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

                        Buffer.BlockCopy(byteArrayROMSizeValue, 0, sourceROM, (int)romHeaderOffset + 0x27, 1);              // Set new ROM size value
                    }
                }

                // If there is SRAM detected, calculate the size
                if (readSRAMSize[0] > 0x00)
                {
                    labelSRAM.Text = "SRAM";
                    stringRAMSize = "Yes (" + Math.Pow(2, readSRAMSize[0]) + " kByte)";
                }

                else if (readExRAMSize[0] > 0x00)
                {
                    labelSRAM.Text = "RAM";
                    stringRAMSize = "Yes (" + Math.Pow(2, readExRAMSize[0]) + " kByte)";
                }

                // Get ROM speed
                switch (readROMSpeed)
                {
                    // If bitmasked map mode is 0x30 ROM is FastROM, if it is 0x20 then it is SlowROM
                    case 32: stringROMSpeed = "SlowROM (200 ns)"; break;
                    case 48: stringROMSpeed = "FastROM (120 ns)"; break;
                    default: stringROMSpeed = "Unknown"; break;
                }

                // Get country and video mode
                switch (country[0])
                {
                    case 0: stringCountry = "Japan"; stringRegion = "NTSC"; break;
                    case 1: stringCountry = "USA"; stringRegion = "NTSC"; break;
                    case 2: stringCountry = "Europe/Oceania/Asia"; stringRegion = "PAL"; break;
                    case 3: stringCountry = "Sweden/Scandinavia"; stringRegion = "PAL"; break;
                    case 4: stringCountry = "Finland"; stringRegion = "PAL"; break;
                    case 5: stringCountry = "Denmark"; stringRegion = "PAL"; break;
                    case 6: stringCountry = "France"; stringRegion = "SECAM (PAL-like, 50 Hz)"; break;
                    case 7: stringCountry = "Netherlands"; stringRegion = "PAL"; break;
                    case 8: stringCountry = "Spain"; stringRegion = "PAL"; break;
                    case 9: stringCountry = "Germany/Austria/Switzerland"; stringRegion = "PAL"; break;
                    case 10: stringCountry = "China/Hong Kong"; stringRegion = "PAL"; break;
                    case 11: stringCountry = "Indonesia"; stringRegion = "PAL"; break;
                    case 12: stringCountry = "South Korea"; stringRegion = "NTSC"; break;
                    case 13: stringCountry = "Common (?)"; stringRegion = "?"; break;
                    case 14: stringCountry = "Canada"; stringRegion = "NTSC"; break;
                    case 15: stringCountry = "Brazil"; stringRegion = "PAL-M (NTSC-like, 60 Hz)"; break;
                    case 16: stringCountry = "Australia"; stringRegion = "PAL"; break;
                    case 17: stringCountry = "Other variation"; stringRegion = "?"; break;
                    case 18: stringCountry = "Other variation"; stringRegion = "?"; break;
                    case 19: stringCountry = "Other variation"; stringRegion = "?"; break;
                    default: stringCountry = "Unknown"; stringRegion = "?"; break;
                }

                // Get version
                stringVersion = "1." + readVersion[0];

                // Convert romtype to string
                string[] falseDSP1Games = { "Ballz                ",    // Games that are detected as DSP-4 but actually are DSP-1
                                            "Lock On              ",
                                            "INDY CAR CHALLENGE   ",
                                            "SUPER AIR DIVER      ",
                                            "SUPER AIRDIVER2      ",
                                            "SUZUKA 8Hours        ",
                                            "ｴｰｽｦﾈﾗｴ!             ",    // Ace o Nerae!
                                            "ｿｳｺｳｷﾍｲ ﾎﾞﾄﾑｽﾞ       " };  // Soukou Kihei Votoms - The Battling Road

                string[] falseGSU2Games = { "STARFOX2             " };  // Games that are detected as GSU-1 but actually are GSU-2

                switch (readROMType[0])
                {
                    case 0x00: stringROMType = "ROM Only"; if (isBSROM) { stringROMType = "BS-X+FLASH+SoundLink"; }; break;
                    case 0x01: stringROMType = "ROM+RAM"; break;
                    case 0x02: stringROMType = "ROM+RAM+Battery"; break;
                    case 0x03: if (readROMSpeed == 0x30 && !falseDSP1Games.Contains(stringTitle)) { stringROMType = "ROM+DSP4"; } else { stringROMType = "ROM+DSP1"; }; break;
                    case 0x04: stringROMType = "ROM+DSP1+RAM"; break;
                    case 0x05: if (readROMSpeed == 0x20) { stringROMType = "ROM+DSP2+RAM+Battery"; } else if (readROMSpeed == 0x30 && company.Equals("0x018E")) { stringROMType = "ROM+DSP3+RAM+Battery"; } else { stringROMType = "ROM+DSP1+RAM+Battery"; }; break;
                    case 0x10: if (isBSROM) { stringROMType = "BS-X+FLASH"; }; break;
                    case 0x13: stringROMType = "ROM+MarioChip1+RAM"; break;
                    case 0x14: stringROMType = "ROM+GSU1+RAM"; if (readROMSize[0] > 0x0A) { stringROMType = "ROM+GSU2+RAM"; }; break;
                    case 0x15: stringROMType = "ROM+GSU2+RAM+Battery"; if (readROMSize[0] <= 0x0A && !falseGSU2Games.Contains(stringTitle)) { stringROMType = "ROM+GSU1+RAM+Battery"; }; break;
                    case 0x1A: stringROMType = "ROM+GSU1+RAM+Battery"; break;
                    case 0x20: if (isBSROM) { stringROMType = "BS-X+PSRAM+SoundLink"; }; break;
                    case 0x25: stringROMType = "ROM+OBC1+RAM+Battery"; break;
                    case 0x30: if (isBSROM) { stringROMType = "BS-X+PSRAM"; }; break;
                    case 0x34: stringROMType = "ROM+SA1+RAM"; break;
                    case 0x35: stringROMType = "ROM+SA1+RAM+Battery"; break;
                    case 0x36: stringROMType = "ROM+SA1"; break;
                    case 0x43: stringROMType = "ROM+S-DD1"; break;
                    case 0x45: stringROMType = "ROM+S-DD1+RAM+Battery"; break;
                    case 0x55: stringROMType = "ROM+S-RTC+RAM+Battery"; break;
                    case 0xA0: if (isBSROM) { stringROMType = "BS-X+SoundNovel"; }; break;
                    case 0xE3: stringROMType = "ROM+SGB"; break;
                    case 0xE5: stringROMType = "ROM+BS-X"; break;
                    case 0xF3: stringROMType = "ROM+CX-4"; break;
                    case 0xF5: if (stringMapMode.Contains("HiROM")) { stringROMType = "ROM+SPC-7110+RAM+Battery"; } else if (stringMapMode.Contains("LoROM")) { stringROMType = "ROM+ST-018+RAM+Battery"; }; break;
                    case 0xF6: if (readROMSize[0] == 0x0A) { stringROMType = "ROM+ST-010"; } else { stringROMType = "ROM+ST-011"; }; break;
                    case 0xF9: stringROMType = "ROM+SPC-7110+RTC+RAM+Battery"; break;
                    default: stringROMType = "Unknown"; break;
                }

                // Convert company code to string
                string stringCompany = "";

                switch (company)
                {
                    case "0x0001": stringCompany = "Nintendo"; break;
                    case "0x0002": stringCompany = "Rocket Games/Ajinomoto"; break;
                    case "0x0003": stringCompany = "Imagineer-Zoom"; break;
                    case "0x0004": stringCompany = "Gray Matter"; break;
                    case "0x0005": stringCompany = "Zamuse"; break;
                    case "0x0006": stringCompany = "Falcom"; break;
                    case "0x0008": stringCompany = "Capcom"; break;
                    case "0x0009": stringCompany = "Hot B Co."; break;
                    case "0x000A": stringCompany = "Jaleco"; break;
                    case "0x000B": stringCompany = "Coconuts Japan"; break;
                    case "0x000C": stringCompany = "Coconuts Japan/G.X.Media"; break;
                    case "0x000D": stringCompany = "Micronet"; break;
                    case "0x000E": stringCompany = "Technos"; break;
                    case "0x000F": stringCompany = "Mebio Software"; break;
                    case "0x0010": stringCompany = "Shouei System"; break;
                    case "0x0011": stringCompany = "Starfish"; break;
                    case "0x0013": stringCompany = "Mitsui Fudosan/Dentsu"; break;
                    case "0x0015": stringCompany = "Warashi Inc."; break;
                    case "0x0017": stringCompany = "Nowpro"; break;
                    case "0x0019": stringCompany = "Game Village"; break;
                    case "0x001A": stringCompany = "IE Institute"; break;
                    case "0x0024": stringCompany = "Banarex"; break;
                    case "0x0025": stringCompany = "Starfish"; break;
                    case "0x0026": stringCompany = "Infocom"; break;
                    case "0x0027": stringCompany = "Electronic Arts Japan"; break;
                    case "0x0029": stringCompany = "Cobra Team"; break;
                    case "0x002A": stringCompany = "Human/Field"; break;
                    case "0x002B": stringCompany = "KOEI"; break;
                    case "0x002C": stringCompany = "Hudson Soft"; break;
                    case "0x002D": stringCompany = "S.C.P./Game Village"; break;
                    case "0x002E": stringCompany = "Yanoman"; break;
                    case "0x0030": stringCompany = "Tecmo Products"; break;
                    case "0x0031": stringCompany = "Japan Glary Business"; break;
                    case "0x0032": stringCompany = "Forum/OpenSystem"; break;
                    case "0x0033": stringCompany = "Virgin Games (Japan)"; break;
                    case "0x0034": stringCompany = "SMDE"; break;
                    case "0x0035": stringCompany = "Yojigen"; break;
                    case "0x0037": stringCompany = "Daikokudenki"; break;
                    case "0x003D": stringCompany = "Creatures Inc."; break;
                    case "0x003E": stringCompany = "TDK Deep Impresion"; break;
                    case "0x0048": stringCompany = "Destination Software/KSS"; break;
                    case "0x0049": stringCompany = "Sunsoft/Tokai Engineering"; break;
                    case "0x004A": stringCompany = "POW (Planning Office Wada)/VR 1 Japan"; break;
                    case "0x004B": stringCompany = "Micro World"; break;
                    case "0x004D": stringCompany = "San-X"; break;
                    case "0x004E": stringCompany = "Enix"; break;
                    case "0x004F": stringCompany = "Loriciel/Electro Brain"; break;
                    case "0x0050": stringCompany = "Kemco Japan"; break;
                    case "0x0051": stringCompany = "Seta Co.,Ltd."; break;
                    case "0x0052": stringCompany = "Culture Brain"; break;
                    case "0x0053": stringCompany = "Irem Corp."; break;
                    case "0x0054": stringCompany = "Palsoft"; break;
                    case "0x0055": stringCompany = "Visit Co., Ltd."; break;
                    case "0x0056": stringCompany = "Intec"; break;
                    case "0x0057": stringCompany = "System Sacom"; break;
                    case "0x0058": stringCompany = "Poppo"; break;
                    case "0x0059": stringCompany = "Ubisoft Japan"; break;
                    case "0x005B": stringCompany = "Media Works"; break;
                    case "0x005C": stringCompany = "NEC InterChannel"; break;
                    case "0x005D": stringCompany = "Tam"; break;
                    case "0x005E": stringCompany = "Gajin/Jordan"; break;
                    case "0x005F": stringCompany = "Smilesoft"; break;
                    case "0x0062": stringCompany = "Mediakite"; break;
                    case "0x006C": stringCompany = "Viacom"; break;
                    case "0x006D": stringCompany = "Carrozzeria"; break;
                    case "0x006E": stringCompany = "Dynamic"; break;
                    case "0x0070": stringCompany = "Magifact"; break;
                    case "0x0071": stringCompany = "Hect"; break;
                    case "0x0072": stringCompany = "Codemasters"; break;
                    case "0x0073": stringCompany = "Taito/GAGA Communications"; break;
                    case "0x0074": stringCompany = "Laguna"; break;
                    case "0x0075": stringCompany = "Telstar Fun & Games/Event/Taito"; break;
                    case "0x0077": stringCompany = "Arcade Zone Ltd."; break;
                    case "0x0078": stringCompany = "Entertainment International/Empire Software"; break;
                    case "0x0079": stringCompany = "Loriciel"; break;
                    case "0x007A": stringCompany = "Gremlin Graphics"; break;
                    case "0x0090": stringCompany = "Seika Corp."; break;
                    case "0x0091": stringCompany = "UBI SOFT Entertainment Software"; break;
                    case "0x0092": stringCompany = "Sunsoft US"; break;
                    case "0x0094": stringCompany = "Life Fitness"; break;
                    case "0x0096": stringCompany = "System 3"; break;
                    case "0x0097": stringCompany = "Spectrum Holobyte"; break;
                    case "0x0099": stringCompany = "Irem"; break;
                    case "0x009B": stringCompany = "Raya Systems"; break;
                    case "0x009C": stringCompany = "Renovation Products"; break;
                    case "0x009D": stringCompany = "Malibu Games"; break;
                    case "0x009F": stringCompany = "Eidos/U.S. Gold"; break;
                    case "0x00A0": stringCompany = "Playmates Interactive"; break;
                    case "0x00A3": stringCompany = "Fox Interactive"; break;
                    case "0x00A4": stringCompany = "Time Warner Interactive"; break;
                    case "0x00AA": stringCompany = "Disney Interactive"; break;
                    case "0x00AC": stringCompany = "Black Pearl"; break;
                    case "0x00AE": stringCompany = "Advanced Productions"; break;
                    case "0x00B1": stringCompany = "GT Interactive"; break;
                    case "0x00B2": stringCompany = "RARE"; break;
                    case "0x00B3": stringCompany = "Crave Entertainment"; break;
                    case "0x00B4": stringCompany = "Absolute Entertainment"; break;
                    case "0x00B5": stringCompany = "Acclaim"; break;
                    case "0x00B6": stringCompany = "Activision"; break;
                    case "0x00B7": stringCompany = "American Sammy"; break;
                    case "0x00B8": stringCompany = "Take 2/GameTek"; break;
                    case "0x00B9": stringCompany = "Hi Tech"; break;
                    case "0x00BA": stringCompany = "LJN Ltd."; break;
                    case "0x00BC": stringCompany = "Mattel"; break;
                    case "0x00BE": stringCompany = "Mindscape/Red Orb Entertainment"; break;
                    case "0x00BF": stringCompany = "Romstar"; break;
                    case "0x00C0": stringCompany = "Taxan"; break;
                    case "0x00C1": stringCompany = "Midway/Tradewest"; break;
                    case "0x00C3": stringCompany = "American Softworks Corp."; break;
                    case "0x00C4": stringCompany = "Majesco Sales Inc."; break;
                    case "0x00C5": stringCompany = "3DO"; break;
                    case "0x00C8": stringCompany = "Hasbro"; break;
                    case "0x00C9": stringCompany = "NewKidCo"; break;
                    case "0x00CA": stringCompany = "Telegames"; break;
                    case "0x00CB": stringCompany = "Metro3D"; break;
                    case "0x00CD": stringCompany = "Vatical Entertainment"; break;
                    case "0x00CE": stringCompany = "LEGO Media"; break;
                    case "0x00D0": stringCompany = "Xicat Interactive"; break;
                    case "0x00D1": stringCompany = "Cryo Interactive"; break;
                    case "0x00D4": stringCompany = "Red Storm Entertainment"; break;
                    case "0x00D5": stringCompany = "Microids"; break;
                    case "0x00D7": stringCompany = "Conspiracy/Swing"; break;
                    case "0x00D8": stringCompany = "Titus"; break;
                    case "0x00D9": stringCompany = "Virgin Interactive"; break;
                    case "0x00DA": stringCompany = "Maxis"; break;
                    case "0x00DC": stringCompany = "LucasArts Entertainment"; break;
                    case "0x00DF": stringCompany = "Ocean"; break;
                    case "0x00E1": stringCompany = "Electronic Arts"; break;
                    case "0x00E3": stringCompany = "Laser Beam"; break;
                    case "0x00E6": stringCompany = "Elite Systems"; break;
                    case "0x00E7": stringCompany = "Electro Brain"; break;
                    case "0x00E8": stringCompany = "The Learning Company"; break;
                    case "0x00E9": stringCompany = "BBC"; break;
                    case "0x00EB": stringCompany = "Software 2000"; break;
                    case "0x00ED": stringCompany = "BAM! Entertainment"; break;
                    case "0x00EE": stringCompany = "Studio 3"; break;
                    case "0x00F2": stringCompany = "Classified Games"; break;
                    case "0x00F4": stringCompany = "TDK Mediactive"; break;
                    case "0x00F6": stringCompany = "DreamCatcher"; break;
                    case "0x00F7": stringCompany = "JoWood Produtions"; break;
                    case "0x00F8": stringCompany = "SEGA"; break;
                    case "0x00F9": stringCompany = "Wannado Edition"; break;
                    case "0x00FA": stringCompany = "LSP (Light & Shadow Prod.)"; break;
                    case "0x00FB": stringCompany = "ITE Media"; break;
                    case "0x00FC": stringCompany = "Infogrames"; break;
                    case "0x00FD": stringCompany = "Interplay"; break;
                    case "0x00FE": stringCompany = "JVC (US)"; break;
                    case "0x00FF": stringCompany = "Parker Brothers"; break;
                    case "0x0101": stringCompany = "SCI (Sales Curve Interactive)/Storm"; break;
                    case "0x0104": stringCompany = "THQ Software"; break;
                    case "0x0105": stringCompany = "Accolade Inc."; break;
                    case "0x0106": stringCompany = "Triffix Entertainment"; break;
                    case "0x0108": stringCompany = "Microprose Software"; break;
                    case "0x0109": stringCompany = "Universal Interactive/Sierra/Simon & Schuster"; break;
                    case "0x010B": stringCompany = "Kemco"; break;
                    case "0x010C": stringCompany = "Rage Software"; break;
                    case "0x010D": stringCompany = "Encore"; break;
                    case "0x010F": stringCompany = "Zoo"; break;
                    case "0x0110": stringCompany = "Kiddinx"; break;
                    case "0x0111": stringCompany = "Simon & Schuster Interactive"; break;
                    case "0x0112": stringCompany = "Asmik Ace Entertainment Inc./AIA"; break;
                    case "0x0113": stringCompany = "Empire Interactive"; break;
                    case "0x0116": stringCompany = "Jester Interactive"; break;
                    case "0x0118": stringCompany = "Rockstar Games"; break;
                    case "0x0119": stringCompany = "Scholastic"; break;
                    case "0x011A": stringCompany = "Ignition Entertainment"; break;
                    case "0x011B": stringCompany = "Summitsoft"; break;
                    case "0x011C": stringCompany = "Stadlbauer"; break;
                    case "0x0120": stringCompany = "Misawa"; break;
                    case "0x0121": stringCompany = "Teichiku"; break;
                    case "0x0122": stringCompany = "Namco Ltd."; break;
                    case "0x0123": stringCompany = "LOZC"; break;
                    case "0x0124": stringCompany = "KOEI"; break;
                    case "0x0126": stringCompany = "Tokuma Shoten Intermedia"; break;
                    case "0x0127": stringCompany = "Tsukuda Original"; break;
                    case "0x0128": stringCompany = "DATAM-Polystar"; break;
                    case "0x012B": stringCompany = "Bullet-Proof Software"; break;
                    case "0x012C": stringCompany = "Vic Tokai Inc."; break;
                    case "0x012E": stringCompany = "Character Soft"; break;
                    case "0x012F": stringCompany = "I'Max"; break;
                    case "0x0130": stringCompany = "Saurus"; break;
                    case "0x0133": stringCompany = "General Entertainment"; break;
                    case "0x0136": stringCompany = "I'Max"; break;
                    case "0x0137": stringCompany = "Success"; break;
                    case "0x0139": stringCompany = "SEGA Japan"; break;
                    case "0x0144": stringCompany = "Takara"; break;
                    case "0x0145": stringCompany = "Chun Soft"; break;
                    case "0x0146": stringCompany = "Video System Co., Ltd./McO'River"; break;
                    case "0x0147": stringCompany = "BEC"; break;
                    case "0x0149": stringCompany = "Varie"; break;
                    case "0x014A": stringCompany = "Yonezawa/S'pal"; break;
                    case "0x014B": stringCompany = "Kaneko"; break;
                    case "0x014D": stringCompany = "Victor Interactive Software/Pack-in-Video"; break;
                    case "0x014E": stringCompany = "Nichibutsu/Nihon Bussan"; break;
                    case "0x014F": stringCompany = "Tecmo"; break;
                    case "0x0150": stringCompany = "Imagineer"; break;
                    case "0x0153": stringCompany = "Nova"; break;
                    case "0x0154": stringCompany = "Den'Z"; break;
                    case "0x0155": stringCompany = "Bottom Up"; break;
                    case "0x0157": stringCompany = "TGL (Technical Group Laboratory)"; break;
                    case "0x0159": stringCompany = "Hasbro Japan"; break;
                    case "0x015B": stringCompany = "Marvelous Entertainment"; break;
                    case "0x015D": stringCompany = "Keynet Inc."; break;
                    case "0x015E": stringCompany = "Hands-On Entertainment"; break;
                    case "0x0168": stringCompany = "Telenet"; break;
                    case "0x0169": stringCompany = "Hori"; break;
                    case "0x016C": stringCompany = "Konami"; break;
                    case "0x016D": stringCompany = "K.Amusement Leasing Co."; break;
                    case "0x016E": stringCompany = "Kawada"; break;
                    case "0x016F": stringCompany = "Takara"; break;
                    case "0x0171": stringCompany = "Technos Japan Corp."; break;
                    case "0x0172": stringCompany = "JVC (Europe/Japan)/Victor Musical Industries"; break;
                    case "0x0174": stringCompany = "Toei Animation"; break;
                    case "0x0175": stringCompany = "Toho"; break;
                    case "0x0177": stringCompany = "Namco"; break;
                    case "0x0178": stringCompany = "Media Rings Corp."; break;
                    case "0x0179": stringCompany = "J-Wing"; break;
                    case "0x017B": stringCompany = "Pioneer LDC"; break;
                    case "0x017C": stringCompany = "KID"; break;
                    case "0x017D": stringCompany = "Mediafactory"; break;
                    case "0x0181": stringCompany = "Infogrames Hudson"; break;
                    case "0x018C": stringCompany = "Acclaim Japan"; break;
                    case "0x018D": stringCompany = "ASCII Co./Nexoft"; break;
                    case "0x018E": stringCompany = "Bandai"; break;
                    case "0x0190": stringCompany = "Enix"; break;
                    case "0x0192": stringCompany = "HAL Laboratory/Halken"; break;
                    case "0x0193": stringCompany = "SNK"; break;
                    case "0x0195": stringCompany = "Pony Canyon Hanbai"; break;
                    case "0x0196": stringCompany = "Culture Brain"; break;
                    case "0x0197": stringCompany = "Sunsoft"; break;
                    case "0x0198": stringCompany = "Toshiba EMI"; break;
                    case "0x0199": stringCompany = "Sony Imagesoft"; break;
                    case "0x019B": stringCompany = "Sammy"; break;
                    case "0x019C": stringCompany = "Magical"; break;
                    case "0x019D": stringCompany = "Visco"; break;
                    case "0x019F": stringCompany = "Compile"; break;
                    case "0x01A1": stringCompany = "MTO Inc."; break;
                    case "0x01A3": stringCompany = "Sunrise Interactive"; break;
                    case "0x01A5": stringCompany = "Global A Entertainment"; break;
                    case "0x01A6": stringCompany = "Fuuki"; break;
                    case "0x01B0": stringCompany = "Taito"; break;
                    case "0x01B2": stringCompany = "Kemco"; break;
                    case "0x01B3": stringCompany = "Square"; break;
                    case "0x01B4": stringCompany = "Tokuma Shoten"; break;
                    case "0x01B5": stringCompany = "Data East"; break;
                    case "0x01B6": stringCompany = "Tonkin House"; break;
                    case "0x01B8": stringCompany = "KOEI"; break;
                    case "0x01BA": stringCompany = "Konami/Ultra/Palcom"; break;
                    case "0x01BB": stringCompany = "NTVIC/VAP"; break;
                    case "0x01BC": stringCompany = "Use Co., Ltd."; break;
                    case "0x01BD": stringCompany = "Meldac"; break;
                    case "0x01BE": stringCompany = "Pony Canyon (Japan)/FCI (US)"; break;
                    case "0x01BF": stringCompany = "Angel/Sotsu Agency/Sunrise"; break;
                    case "0x01C0": stringCompany = "Yumedia/Aroma Co., Ltd."; break;
                    case "0x01C3": stringCompany = "Boss"; break;
                    case "0x01C4": stringCompany = "Axela/Crea-Tech"; break;
                    case "0x01C5": stringCompany = "Sekaibunka-Sha/Sumire kobo/Marigul Management Inc."; break;
                    case "0x01C6": stringCompany = "Konami Computer Entertainment Osaka"; break;
                    case "0x01C9": stringCompany = "Enterbrain"; break;
                    case "0x01D4": stringCompany = "Taito/Disco"; break;
                    case "0x01D5": stringCompany = "Sofel"; break;
                    case "0x01D6": stringCompany = "Quest Corp."; break;
                    case "0x01D7": stringCompany = "Sigma"; break;
                    case "0x01D8": stringCompany = "Ask Kodansha"; break;
                    case "0x01DA": stringCompany = "Naxat"; break;
                    case "0x01DB": stringCompany = "Copya System"; break;
                    case "0x01DC": stringCompany = "Capcom Co., Ltd."; break;
                    case "0x01DD": stringCompany = "Banpresto"; break;
                    case "0x01DE": stringCompany = "TOMY"; break;
                    case "0x01DF": stringCompany = "Acclaim/LJN Japan"; break;
                    case "0x01E1": stringCompany = "NCS"; break;
                    case "0x01E2": stringCompany = "Human Entertainment"; break;
                    case "0x01E3": stringCompany = "Altron"; break;
                    case "0x01E4": stringCompany = "Jaleco"; break;
                    case "0x01E5": stringCompany = "Gaps Inc."; break;
                    case "0x01EB": stringCompany = "Elf"; break;
                    case "0x01F8": stringCompany = "Jaleco"; break;
                    case "0x01FA": stringCompany = "Yutaka"; break;
                    case "0x01FB": stringCompany = "Varie"; break;
                    case "0x01FC": stringCompany = "T&ESoft"; break;
                    case "0x01FD": stringCompany = "Epoch Co., Ltd."; break;
                    case "0x01FF": stringCompany = "Athena"; break;
                    case "0x0200": stringCompany = "Asmik"; break;
                    case "0x0201": stringCompany = "Natsume"; break;
                    case "0x0202": stringCompany = "King Records"; break;
                    case "0x0203": stringCompany = "Atlus"; break;
                    case "0x0204": stringCompany = "Epic/Sony Records (Japan)"; break;
                    case "0x0206": stringCompany = "IGS (Information Global Service)"; break;
                    case "0x0208": stringCompany = "Chatnoir"; break;
                    case "0x0209": stringCompany = "Right Stuff"; break;
                    case "0x020B": stringCompany = "NTT COMWARE"; break;
                    case "0x020D": stringCompany = "Spike"; break;
                    case "0x020E": stringCompany = "Konami Computer Entertainment Tokyo"; break;
                    case "0x020F": stringCompany = "Alphadream Corp."; break;
                    case "0x0211": stringCompany = "Sting"; break;
                    case "0x021C": stringCompany = "A Wave"; break;
                    case "0x021D": stringCompany = "Motown Software"; break;
                    case "0x021E": stringCompany = "Left Field Entertainment"; break;
                    case "0x021F": stringCompany = "Extreme Entertainment Group"; break;
                    case "0x0220": stringCompany = "TecMagik"; break;
                    case "0x0225": stringCompany = "Cybersoft"; break;
                    case "0x0227": stringCompany = "Psygnosis"; break;
                    case "0x022A": stringCompany = "Davidson/Western Tech."; break;
                    case "0x022B": stringCompany = "Unlicensed"; break;
                    case "0x0230": stringCompany = "The Game Factory Europe"; break;
                    case "0x0231": stringCompany = "Hip Games"; break;
                    case "0x0232": stringCompany = "Aspyr"; break;
                    case "0x0235": stringCompany = "Mastiff"; break;
                    case "0x0236": stringCompany = "iQue"; break;
                    case "0x0237": stringCompany = "Digital Tainment Pool"; break;
                    case "0x0238": stringCompany = "XS Games"; break;
                    case "0x0239": stringCompany = "Daiwon"; break;
                    case "0x0241": stringCompany = "PCCW Japan"; break;
                    case "0x0244": stringCompany = "KiKi Co. Ltd."; break;
                    case "0x0245": stringCompany = "Open Sesame Inc."; break;
                    case "0x0246": stringCompany = "Sims"; break;
                    case "0x0247": stringCompany = "Broccoli"; break;
                    case "0x0248": stringCompany = "Avex"; break;
                    case "0x0249": stringCompany = "D3 Publisher"; break;
                    case "0x024B": stringCompany = "Konami Computer Entertainment Japan"; break;
                    case "0x024D": stringCompany = "Square-Enix"; break;
                    case "0x024E": stringCompany = "KSG"; break;
                    case "0x024F": stringCompany = "Micott & Basara Inc."; break;
                    case "0x0251": stringCompany = "Orbital Media"; break;
                    case "0x0262": stringCompany = "The Game Factory USA"; break;
                    case "0x0265": stringCompany = "Treasure"; break;
                    case "0x0266": stringCompany = "Aruze"; break;
                    case "0x0267": stringCompany = "Ertain"; break;
                    case "0x0268": stringCompany = "SNK Playmore"; break;
                    case "0x0299": stringCompany = "Yojigen"; break;
                    default: stringCompany = "Unknown"; break;
                }

                // Calculate checksums
                calcChksm = ROM.CalculateChecksum(sourceROM, romHeaderOffset, intROMSize, calcFileSize, readROMType[0], isBSROM);
                calcInvChksm = ROM.CalculateInverseChecksum(calcChksm);

                // Check if ROM is swappable
                if (sourceROM.Length % 1048576 == 0)
                {
                    buttonSwapBinROM.Enabled = true;
                }

                // Check if ROM can be expanded
                if (calcFileSize < 32)
                {
                    List<comboBoxExpandROMList> list = new List<comboBoxExpandROMList>();

                    if (calcFileSize < 1) { list.Add(new comboBoxExpandROMList { Id = 1, Name = "1 Mbit (128 kByte) | 27C1001" }); };
                    if (calcFileSize < 2) { list.Add(new comboBoxExpandROMList { Id = 2, Name = "2 Mbit (256 kByte) | 27C2001" }); };
                    if (calcFileSize < 4) { list.Add(new comboBoxExpandROMList { Id = 4, Name = "4 Mbit (512 kByte) | 274001" }); };
                    if (calcFileSize < 8) { list.Add(new comboBoxExpandROMList { Id = 8, Name = "8 Mbit (1 MByte) | 27C801" }); };
                    //if (calcFileSize < 12) { list.Add(new comboBoxExpandROMList { Id = 12, Name = "12 Mbit (1,5 MByte)" }); };
                    if (calcFileSize < 16) { list.Add(new comboBoxExpandROMList { Id = 16, Name = "16 Mbit (2 MByte) | 27C160" }); };
                    //if (calcFileSize < 20) { list.Add(new comboBoxExpandROMList { Id = 20, Name = "20 Mbit (2,5 MByte)" }); };
                    //if (calcFileSize < 24) { list.Add(new comboBoxExpandROMList { Id = 24, Name = "24 Mbit (3 MByte)" }); };
                    //if (calcFileSize < 28) { list.Add(new comboBoxExpandROMList { Id = 28, Name = "28 Mbit (3,5 MByte)" }); };
                    list.Add(new comboBoxExpandROMList { Id = 32, Name = "32 Mbit (4 MByte) | 27C322" });

                    comboBoxExpandROM.DataSource = list;
                    comboBoxExpandROM.DisplayMember = "Name";
                    comboBoxExpandROM.ValueMember = "Id";

                    buttonExpandROM.Enabled = true;
                    comboBoxExpandROM.Enabled = true;
                }

                // Check if ROM can be splittet
                if (calcFileSize > 1)
                {
                    List<comboBoxSplitROMList> list = new List<comboBoxSplitROMList>();

                    if (calcFileSize % 32 == 0 && calcFileSize > 32) { list.Add(new comboBoxSplitROMList { Id = 32, Name = "32 Mbit (4 MByte) | 27C322" }); };
                    if (calcFileSize % 16 == 0 && calcFileSize > 16) { list.Add(new comboBoxSplitROMList { Id = 16, Name = "16 Mbit (2 MByte) | 27C160" }); };
                    if (calcFileSize % 8 == 0 && calcFileSize > 8) { list.Add(new comboBoxSplitROMList { Id = 8, Name = "8 Mbit (1 MByte) | 27C801" }); };
                    if (calcFileSize % 4 == 0 && calcFileSize > 4) { list.Add(new comboBoxSplitROMList { Id = 4, Name = "4 Mbit (512 kByte) | 27C4001" }); };
                    if (calcFileSize % 2 == 0 && calcFileSize > 2) { list.Add(new comboBoxSplitROMList { Id = 2, Name = "2 Mbit (256 kByte) | 27C2001" }); };
                    list.Add(new comboBoxSplitROMList { Id = 1, Name = "1 Mbit (128 kByte) | 27C1001" });

                    comboBoxSplitROM.DataSource = list;
                    comboBoxSplitROM.DisplayMember = "Name";
                    comboBoxSplitROM.ValueMember = "Id";

                    buttonSplitROM.Enabled = true;
                    comboBoxSplitROM.Enabled = true;
                }

                // Check if checksum can be fixed
                if (!readChksm.SequenceEqual(calcChksm))
                {
                    buttonFixChksm.Enabled = true;
                }

                // Check if ROM contains region locks
                if(ROM.UnlockRegion(sourceROM, sourceROMSMCHeader, false, romSavePath, romName, stringRegion))
                {
                    buttonFixRegion.Enabled = true;
                }

                // Check if ROM is interleaved
                if (isInterleaved)
                {
                    // Disable not needed buttons
                    buttonAddHeader.Enabled = false;
                    buttonSwapBinROM.Enabled = false;
                    buttonDeinterleave.Enabled = true;
                    buttonFixChksm.Enabled = false;
                }

                // Set labels
                labelGetTitle.Text = stringTitle; //Encoding.ASCII.GetString(readTitle);
                labelGetMapMode.Text = stringMapMode;
                labelGetROMType.Text = stringROMType; //+ " (" + BitConverter.ToString(readROMType) + ")";
                labelGetROMSize.Text = stringROMSize;
                labelGetSRAM.Text = stringRAMSize;
                labelGetFileSize.Text = calcFileSize.ToString() + " Mbit (" + ((float)calcFileSize / 8) + " MByte)";
                labelGetSMCHeader.Text = stringSMCHeader;
                labelGetROMSpeed.Text = stringROMSpeed;
                labelGetCountry.Text = stringCountry;
                labelGetRegion.Text = stringRegion;
                labelGetCompany.Text = stringCompany;
                labelGetIntChksm.Text = BitConverter.ToString(readChksm).Replace("-", "");
                labelGetIntInvChksm.Text = BitConverter.ToString(readInvChksm).Replace("-", "");
                labelGetCalcChksm.Text = BitConverter.ToString(calcChksm).Replace("-","");
                labelGetCalcInvChksm.Text = BitConverter.ToString(calcInvChksm).Replace("-", "");
                labelGetVersion.Text = stringVersion;
                labelGetCRC32Chksm.Text = crc32Hash;
            }
        }

        private void ButtonAddHeader_Click(object sender, EventArgs e)
        {
            // Just add 512 bytes of 0x00 to the beginning of the file
            byte[] romHeader = new byte[512];
            byte[] sourceROMHeadered = new byte[sourceROM.Length + romHeader.Length];

            foreach(byte singleByte in romHeader)
            {
                romHeader[singleByte] = 0x00;
            }

            Buffer.BlockCopy(romHeader, 0, sourceROMHeadered, 0, romHeader.Length);
            Buffer.BlockCopy(sourceROM, 0, sourceROMHeadered, romHeader.Length, sourceROM.Length);

            // Save file with header
            File.WriteAllBytes(@romSavePath + @"\" + romName + "_[headered]" + ".smc", sourceROMHeadered);
            buttonAddHeader.Enabled = false;

            MessageBox.Show("Header successfully added!\n\nFile saved to: '" + @romSavePath + @"\" + romName + "_[headered]" + ".smc'");
        }

        private void ButtonRemoveHeader_Click(object sender, EventArgs e)
        {
            // Save file without header
            File.WriteAllBytes(@romSavePath + @"\" + romName + "_[no_header]" + ".sfc", sourceROM);
            buttonRemoveHeader.Enabled = false;

            MessageBox.Show("Header successfully removed!\n\nFile saved to: '" + @romSavePath + @"\" + romName + "_[no_header]" + ".sfc'");
        }

        private void ButtonSwapBinROM_Click(object sender, EventArgs e)
        {
            // Swap ROM - If ROM size is multiple of 8 Mbit then additionally split into chunks
            int romChunks = sourceROM.Length / 1048576;

            if(romChunks > 1)
            {
                // Define size for single ROM file (8 Mbit)
                int chunkSize = 1048576;

                for (int index = 0; index < romChunks; index++)
                {
                    string romChunkName = romName + "_[" + index + "]";
                    byte[] sourceROMChunk = new byte[chunkSize];

                    Buffer.BlockCopy(sourceROM, index * chunkSize, sourceROMChunk, 0, chunkSize);

                    ROM.SwapBin(sourceROMChunk, romSavePath, romChunkName);
                }
            }

            else
            {
                ROM.SwapBin(sourceROM, romSavePath, romName);
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

            Buffer.BlockCopy(sourceROM, 0, expandedROM, 0, sourceROM.Length);

            // Save file without header
            File.WriteAllBytes(@romSavePath + @"\" + romName + "_[expanded]" + ".bin", expandedROM);

            buttonExpandROM.Enabled = false;
            comboBoxExpandROM.Enabled = false;

            MessageBox.Show("ROM successfully expanded!\n\nFile saved to: '" + @romSavePath + @"\" + romName + "_[expanded]" + ".bin'\n\nIn case there was a header, it has been removed!");
        }

        private void ButtonSplitROM_Click(object sender, EventArgs e)
        {
            int splitROMSize = (int)comboBoxSplitROM.SelectedValue;
            int romChunks = sourceROM.Length / (splitROMSize * 131072);

            for (int index = 0; index < romChunks; index++)
            {
                string romChunkName = romName + "_[" + index + "]";
                byte[] splitROM = new byte[splitROMSize * 131072];

                Buffer.BlockCopy(sourceROM, index * (splitROMSize * 131072), splitROM, 0, splitROMSize * 131072);

                // Save file split
                File.WriteAllBytes(@romSavePath + @"\" + romChunkName + "_[split]" + ".bin", splitROM);
                MessageBox.Show("ROM successfully splittet!\n\nFile saved to: '" + @romSavePath + @"\" + romChunkName + "_[split]" + ".bin'\n\nIn case there was a header, it has been removed!");
            }

            buttonSplitROM.Enabled = false;
            comboBoxSplitROM.Enabled = false;
        }

        private void ButtonFixChksm_Click(object sender, EventArgs e)
        {
            byte[] checksumFixedROM = new byte[sourceROM.Length];
            byte[] newChksm = new byte[2];
            byte[] newInvChksm = new byte[2];
            byte[] newChksmSequence = new byte[4];
            uint offset = romHeaderOffset + 0x2C;

            newChksm = calcChksm;
            newInvChksm = calcInvChksm;

            // Reverse checksum for inserting
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(newChksm);
                Array.Reverse(newInvChksm);
            }

            newChksmSequence[0] = newInvChksm[0];
            newChksmSequence[1] = newInvChksm[1];
            newChksmSequence[2] = newChksm[0];
            newChksmSequence[3] = newChksm[1];

            Buffer.BlockCopy(sourceROM, 0, checksumFixedROM, 0, sourceROM.Length);
            Buffer.BlockCopy(newChksmSequence, 0, checksumFixedROM, (int)offset, newChksmSequence.Length);

            buttonFixChksm.Enabled = false;

            // Save checksum fixed file with header
            if (sourceROMSMCHeader != null)
            {
                byte[] checksumFixedHeaderedROM = new byte[sourceROMSMCHeader.Length + checksumFixedROM.Length];
                
                Buffer.BlockCopy(sourceROMSMCHeader, 0, checksumFixedHeaderedROM, 0, sourceROMSMCHeader.Length);
                Buffer.BlockCopy(checksumFixedROM, 0, checksumFixedHeaderedROM, sourceROMSMCHeader.Length, checksumFixedROM.Length);

                File.WriteAllBytes(@romSavePath + @"\" + romName + "_[checksum_fixed]" + ".sfc", checksumFixedHeaderedROM);
                MessageBox.Show("Checksum successfully fixed!\n\nFile saved to: '" + @romSavePath + @"\" + romName + "_[checksum_fixed]" + ".sfc'");
            }

            else
            {
                // Save checksum fixed file without header
                File.WriteAllBytes(@romSavePath + @"\" + romName + "_[checksum_fixed]" + ".sfc", checksumFixedROM);
                MessageBox.Show("Checksum successfully fixed!\n\nFile saved to: '" + @romSavePath + @"\" + romName + "_[checksum_fixed]" + ".sfc'");
            }
        }

        private void buttonFixRegion_Click(object sender, EventArgs e)
        {
            ROM.UnlockRegion(sourceROM, sourceROMSMCHeader, true, romSavePath, romName, stringRegion);
            buttonFixRegion.Enabled = false;
        }

        private void buttonDeinterleave_Click(object sender, EventArgs e)
        {
            ROM.Deinterlave(sourceROM, sourceROMSMCHeader, calcFileSize, romSavePath, romName);
            buttonDeinterleave.Enabled = false;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            Form3 editROMInformation = new Form3(sourceROM, sourceROMSMCHeader, romHeaderOffset, stringTitle, readTitle, stringVersion, country, company, intROMSize, calcFileSize, romSavePath, romName, isBSROM);
            editROMInformation.Show();
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