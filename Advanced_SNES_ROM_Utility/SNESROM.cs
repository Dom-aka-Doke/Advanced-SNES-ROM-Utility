using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        public string ROMName;
        public string ROMFullPath;
        public string ROMFolder;

        public byte[] SourceROM;
        public byte[] SourceROMHeader;
        public byte[] SourceROMSMCHeader;

        public byte[] ByteArrayChecksum;
        public byte[] ByteArrayInvChecksum;
        public byte[] ByteArrayCalcChecksum;
        public byte[] ByteArrayCalcInvChecksum;

        public uint UIntROMHeaderOffset;
        public uint UIntSMCHeader;
        public int IntROMSize;
        public int IntCalcFileSize;
        public string CRC32Hash;

        public bool IsNewHeader;
        public bool IsBSROM;
        public bool IsInterleaved;

        public byte ByteROMType;
        public byte ByteMapMode;
        public byte ByteROMSpeed;
        public byte ByteSRAMSize;
        public byte ByteExRAMSize;
        public byte ByteVersion;
        public byte ByteROMSize;
        public byte[] ByteArrayTitle;
        public byte ByteCountry;
        public int IntCompany;
        public byte[] ByteArrayGameCode;

        public string StringROMType;
        public string StringMapMode;
        public string StringROMSpeed;
        public string StringRAMSize;
        public string StringVersion;
        public string StringROMSize;
        public string StringTitle;
        public string StringCountry;
        public string StringCompany;
        public string StringRegion;
        public string StringSMCHeader;
        public string StringGameCode;

        public SNESROM(string @romPath)
        {
            // Read ROM
            if (File.Exists(@romPath))
            {
                ROMFullPath = @romPath;

                try
                {
                    ROMName = Path.GetFileNameWithoutExtension(ROMFullPath);
                    ROMFolder = Path.GetDirectoryName(ROMFullPath);
                    FileInfo romFileInfo = new FileInfo(ROMFullPath);

                    if (romFileInfo.Length >= 131072 && romFileInfo.Length <= 16777728)                 // Min is 1 Mbit, max is 128 Mbit incl. 512 byte header
                    {
                        SourceROM = File.ReadAllBytes(ROMFullPath);
                        Initialize();
                    }

                    else
                    {
                        MessageBox.Show("ROMs cannot be smaller than 128 kByte (1 Mbit) or larger than 16 MByte (128 Mbit).");
                    }
                }

                catch
                {
                    return;
                }
            }
        }

        public void Initialize()
        {
            // Initialize ROM
            GetSMCHeader();
            GetROMHeader();
            GetTitle();
            GetMapMode();
            GetROMSpeed();
            GetCompany();
            GetROMSize();
            GetROMType();
            CheckIsNewHeader();
            GetSRAMSize();
            GetExRAMSize();
            GetCountry();
            GetVersion();
            GetGameCode();
            GetChecksum();
            GetInverseChecksum();
            CalculateFileSize();
            CalculateChecksum();
            CalculateInverseChecksum();
            CalculateCrc32Hash();
        }

        private void GetSMCHeader()
        {
            // Calculate size of header
            uint existingSMCHeader = 0;

            if (SourceROMSMCHeader != null)
            {
                existingSMCHeader = (uint)SourceROMSMCHeader.Length;
            }

            UIntSMCHeader = (uint)(SourceROM.Length + existingSMCHeader) % 1024;

            if (UIntSMCHeader == 0)
            {
                StringSMCHeader = "No";
            }

            else if (UIntSMCHeader > 0)
            {
                StringSMCHeader = "Malformed";

                if (SourceROMSMCHeader == null)
                {
                    SourceROMSMCHeader = new byte[UIntSMCHeader];
                    byte[] newSourceROM = new byte[SourceROM.Length - UIntSMCHeader];

                    Buffer.BlockCopy(SourceROM, 0, SourceROMSMCHeader, 0, (int)UIntSMCHeader);
                    Buffer.BlockCopy(SourceROM, (int)UIntSMCHeader, newSourceROM, 0, newSourceROM.Length);

                    SourceROM = newSourceROM;
                }
            }

            if (UIntSMCHeader == 512)
            {
                StringSMCHeader = "Yes";
            }
        }

        private void GetROMHeader()
        {
            // Initialize with most likely values
            UIntROMHeaderOffset = 0x7FB0;
            IsBSROM = false;

            int mapModeScoreLoROM = GetMapModeScore(SourceROM, 0x7FB0, false);
            int mapModeScoreBSLoROM = GetMapModeScore(SourceROM, 0x7FB0, true);
            int mapModeScoreHiROM = GetMapModeScore(SourceROM, 0xFFB0, false);
            int mapModeScoreBSHiROM = GetMapModeScore(SourceROM, 0xFFB0, true);
            int mapModeScoreExLoROM = GetMapModeScore(SourceROM, 0x407FB0, false);
            int mapModeScoreExHiROM = GetMapModeScore(SourceROM, 0x40FFB0, false);

            if (mapModeScoreLoROM >= mapModeScoreHiROM && mapModeScoreLoROM >= mapModeScoreExLoROM && mapModeScoreLoROM >= mapModeScoreExHiROM)
            {
                UIntROMHeaderOffset = 0x7FB0;

                if (mapModeScoreBSLoROM > mapModeScoreLoROM)
                {
                    IsBSROM = true;
                }
            }

            else if (mapModeScoreHiROM >= mapModeScoreExLoROM && mapModeScoreHiROM >= mapModeScoreExHiROM)
            {
                UIntROMHeaderOffset = 0xFFB0;

                if (mapModeScoreBSHiROM > mapModeScoreHiROM)
                {
                    IsBSROM = true;
                }
            }

            else if (mapModeScoreExLoROM >= mapModeScoreExHiROM)
            {
                UIntROMHeaderOffset = 0x407FB0;
            }

            else
            {
                UIntROMHeaderOffset = 0x40FFB0;
            }

            // Load header
            SourceROMHeader = new byte[80];
            Buffer.BlockCopy(SourceROM, (int)UIntROMHeaderOffset, SourceROMHeader, 0, 80);
        }

        public static int GetMapModeScore(byte[] sourceROM, uint offset, bool isBSROM)
        {
            // Check if ROM has normal or extended size
            if (sourceROM.Length < offset + 0x50)
            {
                return -100;
            }

            // Define score variable
            int score = 0;

            // Get all necessary values from ROM
            byte[] resetVector = new byte[2];
            Buffer.BlockCopy(sourceROM, (int)offset + 0x4C, resetVector, 0, 2);

            byte[] checksum = new byte[2];
            Buffer.BlockCopy(sourceROM, (int)offset + 0x2E, checksum, 0, 2);

            byte[] checksumInverse = new byte[2];
            Buffer.BlockCopy(sourceROM, (int)offset + 0x2C, checksumInverse, 0, 2);

            byte[] mapMode = new byte[1];
            if (isBSROM) { Buffer.BlockCopy(sourceROM, (int)offset + 0x28, mapMode, 0, 1); } else { Buffer.BlockCopy(sourceROM, (int)offset + 0x25, mapMode, 0, 1); }
            
            // Bitmask this byte, because non relevant bits are not clearly defined
            mapMode[0] &= 0x37;

            byte[] romSize = new byte[1];
            Buffer.BlockCopy(sourceROM, (int)offset + 0x27, romSize, 0, 1);

            byte[] destinationCode = new byte[1];
            Buffer.BlockCopy(sourceROM, (int)offset + 0x29, destinationCode, 0, 1);

            byte[] fixedValue33 = new byte[1];
            Buffer.BlockCopy(sourceROM, (int)offset + 0x2A, fixedValue33, 0, 1);

            byte[] fixedValue00 = new byte[7];
            Buffer.BlockCopy(sourceROM, (int)offset + 0x06, fixedValue00, 0, 7);

            byte[] opCode = new byte[1];
            Buffer.BlockCopy(sourceROM, BitConverter.ToUInt16(resetVector, 0), opCode, 0, 1);

            // Check if reset vector is pointing to an address greater than or equal to 0x8000
            if (BitConverter.ToUInt16(resetVector, 0) < 0x8000)
            {
                return -100;
            }

            // Some values must be in big endian
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(resetVector);
                Array.Reverse(opCode);
                Array.Reverse(checksum);
                Array.Reverse(checksumInverse);
            }

            // Check opcodes

            // Most likely opcodes
            if (opCode[0] == 0x78  //sei
            || opCode[0] == 0x18  //clc (clc; xce)
            || opCode[0] == 0x38  //sec (sec; xce)
            || opCode[0] == 0x9c  //stz $nnnn (stz $4200)
            || opCode[0] == 0x4c  //jmp $nnnn
            || opCode[0] == 0x5c  //jml $nnnnnn
            )
            {
                score += 8;
            }

            // Plausible opcodes
            if (opCode[0] == 0xc2  //rep #$nn
            || opCode[0] == 0xe2  //sep #$nn
            || opCode[0] == 0xad  //lda $nnnn
            || opCode[0] == 0xae  //ldx $nnnn
            || opCode[0] == 0xac  //ldy $nnnn
            || opCode[0] == 0xaf  //lda $nnnnnn
            || opCode[0] == 0xa9  //lda #$nn
            || opCode[0] == 0xa2  //ldx #$nn
            || opCode[0] == 0xa0  //ldy #$nn
            || opCode[0] == 0x20  //jsr $nnnn
            || opCode[0] == 0x22  //jsl $nnnnnn
            )
            {
                score += 4;
            }

            // Implausible opcodes
            if (opCode[0] == 0x40  //rti
            || opCode[0] == 0x60  //rts
            || opCode[0] == 0x6b  //rtl
            || opCode[0] == 0xcd  //cmp $nnnn
            || opCode[0] == 0xec  //cpx $nnnn
            || opCode[0] == 0xcc  //cpy $nnnn
            )
            {
                score -= 4;
            }

            // Least likely opcodes
            if (opCode[0] == 0x00  //brk #$nn
            || opCode[0] == 0x02  //cop #$nn
            || opCode[0] == 0xdb  //stp
            || opCode[0] == 0x42  //wdm
            || opCode[0] == 0xff  //sbc $nnnnnn,x
            )
            {
                score -= 8;
            }

            // Check if checksums add up to 0xFFFF
            if (BitConverter.ToUInt16(checksum, 0) + BitConverter.ToUInt16(checksumInverse, 0) == 0xFFFF)
            {
                score += 4;
            }

            // Check if internal ROM type is valid
            // LoROM
            if (offset == 0x7FB0 && (mapMode[0] == 0x20 || mapMode[0] == 0x30))
            {
                score += 2;
            }
            // HiROM
            if (offset == 0xFFB0 && (mapMode[0] == 0x21 || mapMode[0] == 0x31))
            {
                score += 2;
            }
            // ExLoROM - f.e. Star Ocean
            if (offset == 0x407FB0 && mapMode[0] == 0x32)
            {
                score += 4;
            }
            // ExHiROM - f.e. Tales Of Phantasia
            if (offset == 0x40FFB0 && mapMode[0] == 0x35)
            {
                score += 4;
            }

            // Check if internal ROM size is vaild
            if (romSize[0] >= 0x07 && romSize[0] <= 0x0D)
            {
                score += 2;
            }

            // Check if destination code size is vaild
            if (destinationCode[0] <= 0x14)
            {
                score += 2;
            }

            // Check for some fixed ROM values
            if (fixedValue33[0] == 0x33)
            {
                score += 4;
            }

            if (fixedValue00[0] == 0x00 &&
                fixedValue00[1] == 0x00 &&
                fixedValue00[2] == 0x00 &&
                fixedValue00[3] == 0x00 &&
                fixedValue00[4] == 0x00 &&
                fixedValue00[5] == 0x00 &&
                fixedValue00[6] == 0x00)
            {
                score += 2;
            }

            // If game is tested for being BS-X
            if (isBSROM)
            {
                // If title starts with 'BS' like most of them do
                byte[] titleBS = new byte[2];
                Buffer.BlockCopy(sourceROM, (int)offset + 0x10, titleBS, 0, 2);

                if (titleBS[0] == 0x42 && titleBS[1] == 0x53)
                {
                    score += 2;
                }
            }

            // Return score
            return score;
        }

        private void GetTitle()
        {
            byte[] title = new byte[21];

            if (IsBSROM) { title = new byte[16]; Buffer.BlockCopy(SourceROMHeader, 0x10, title, 0, 16); } else { Buffer.BlockCopy(SourceROMHeader, 0x10, title, 0, 21); if (title[20] == 0x00) { title = new byte[20]; Buffer.BlockCopy(SourceROMHeader, 0x10, title, 0, 20); } }

            // Return title as little endian byte[]
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(title);
            }

            ByteArrayTitle = title;

            StringTitle = Encoding.GetEncoding(932).GetString(ByteArrayTitle);
        }

        public void GetMapMode()
        {
            byte[] mapMode = new byte[1];
            if (IsBSROM) { Buffer.BlockCopy(SourceROMHeader, 0x28, mapMode, 0, 1); } else { Buffer.BlockCopy(SourceROMHeader, 0x25, mapMode, 0, 1); }
            
            // SPC7110 games have an odd value in their header but actually are HiROM
            if (mapMode[0] == 0x3A)
            {
                mapMode[0] = 0x21;
            }

            // Bitmask this byte, because non relevant bits are not clearly defined
            mapMode[0] &= 0x37;

            ByteMapMode = mapMode[0];

            // Initialize with false
            IsInterleaved = false;

            switch (ByteMapMode)
            {
                case 0x20: StringMapMode = "LoROM"; break;
                case 0x21: StringMapMode = "HiROM"; if (UIntROMHeaderOffset == 0x7FB0) { IsInterleaved = true; }; break;
                case 0x22: StringMapMode = "LoROM (SDD-1)"; break;
                case 0x23: StringMapMode = "LoROM (SA-1)"; break;
                case 0x25: StringMapMode = "ExHiROM"; if (UIntROMHeaderOffset == 0x7FB0) { IsInterleaved = true; }; break;
                case 0x30: StringMapMode = "LoROM"; break;
                case 0x31: StringMapMode = "HiROM"; if (UIntROMHeaderOffset == 0x7FB0) { IsInterleaved = true; }; break;
                case 0x32: StringMapMode = "ExLoROM"; break;
                case 0x33: StringMapMode = "LoROM (SA-1)"; break;
                case 0x35: StringMapMode = "ExHiROM"; if (UIntROMHeaderOffset == 0x7FB0) { IsInterleaved = true; }; break;
                default: StringMapMode = "Unknown"; break;
            }

            // ROM that contains oversized title which overwrites the map mode byte, but actually is LoROM
            if (StringTitle.Equals("YUYU NO QUIZ DE GO!GO"))
            {
                StringMapMode = "LoROM";

                // If this ROM is not interleaved set it as not interleaved
                if (UIntROMHeaderOffset == 0x7FB0)
                {
                    IsInterleaved = false;
                }
            }
        }

        private void GetROMType()
        {
            byte[] type = new byte[1];
            if (IsBSROM) { Buffer.BlockCopy(SourceROMHeader, 0x29, type, 0, 1); } else { Buffer.BlockCopy(SourceROMHeader, 0x26, type, 0, 1); }

            ByteROMType = type[0];

            string[] falseDSP1Games = { "Ballz                ",    // Games that are detected as DSP-4 but actually are DSP-1
                                        "Lock On              ",
                                        "INDY CAR CHALLENGE   ",
                                        "SUPER AIR DIVER      ",
                                        "SUPER AIRDIVER2      ",
                                        "SUZUKA 8Hours        ",
                                        "ｴｰｽｦﾈﾗｴ!             ",    // Ace o Nerae!
                                        "ｿｳｺｳｷﾍｲ ﾎﾞﾄﾑｽﾞ       " };  // Soukou Kihei Votoms - The Battling Road

            string[] falseGSU2Games = { "STARFOX2             " };  // Games that are detected as GSU-1 but actually are GSU-2

            switch (ByteROMType)
            {
                case 0x00: StringROMType = "ROM Only"; if (IsBSROM) { StringROMType = "BS-X+FLASH+SoundLink"; }; break;
                case 0x01: StringROMType = "ROM+RAM"; break;
                case 0x02: StringROMType = "ROM+RAM+Battery"; break;
                case 0x03: if (ByteROMSpeed == 0x30 && !falseDSP1Games.Contains(StringTitle)) { StringROMType = "ROM+DSP4"; } else { StringROMType = "ROM+DSP1"; }; break;
                case 0x04: StringROMType = "ROM+DSP1+RAM"; break;
                case 0x05: if (ByteROMSpeed == 0x20) { StringROMType = "ROM+DSP2+RAM+Battery"; } else if (ByteROMSpeed == 0x30 && IntCompany == 0x018E) { StringROMType = "ROM+DSP3+RAM+Battery"; } else { StringROMType = "ROM+DSP1+RAM+Battery"; }; break;
                case 0x10: if (IsBSROM) { StringROMType = "BS-X+FLASH"; }; break;
                case 0x13: StringROMType = "ROM+MarioChip1+RAM"; break;
                case 0x14: StringROMType = "ROM+GSU1+RAM"; if (ByteROMSize > 0x0A) { StringROMType = "ROM+GSU2+RAM"; }; break;
                case 0x15: StringROMType = "ROM+GSU2+RAM+Battery"; if (ByteROMSize <= 0x0A && !falseGSU2Games.Contains(StringTitle)) { StringROMType = "ROM+GSU1+RAM+Battery"; }; break;
                case 0x1A: StringROMType = "ROM+GSU1+RAM+Battery"; break;
                case 0x20: if (IsBSROM) { StringROMType = "BS-X+PSRAM+SoundLink"; }; break;
                case 0x25: StringROMType = "ROM+OBC1+RAM+Battery"; break;
                case 0x30: if (IsBSROM) { StringROMType = "BS-X+PSRAM"; }; break;
                case 0x34: StringROMType = "ROM+SA1+RAM"; break;
                case 0x35: StringROMType = "ROM+SA1+RAM+Battery"; break;
                case 0x36: StringROMType = "ROM+SA1"; break;
                case 0x43: StringROMType = "ROM+S-DD1"; break;
                case 0x45: StringROMType = "ROM+S-DD1+RAM+Battery"; break;
                case 0x55: StringROMType = "ROM+S-RTC+RAM+Battery"; break;
                case 0xA0: if (IsBSROM) { StringROMType = "BS-X+SoundNovel"; }; break;
                case 0xE3: StringROMType = "ROM+SGB"; break;
                case 0xE5: StringROMType = "ROM+BS-X"; break;
                case 0xF3: StringROMType = "ROM+CX-4"; break;
                case 0xF5: if (StringMapMode.Contains("HiROM")) { StringROMType = "ROM+SPC-7110+RAM+Battery"; } else if (StringMapMode.Contains("LoROM")) { StringROMType = "ROM+ST-018+RAM+Battery"; }; break;
                case 0xF6: if (ByteROMSize == 0x0A) { StringROMType = "ROM+ST-010"; } else { StringROMType = "ROM+ST-011"; }; break;
                case 0xF9: StringROMType = "ROM+SPC-7110+RTC+RAM+Battery"; break;
                default: StringROMType = "Unknown"; break;
            }
        }

        private void GetROMSpeed()
        {
            // Bitmask first nibble, because only this information is needed
            byte speed = (byte)(ByteMapMode & 0xF0);

            ByteROMSpeed = speed;

            switch (ByteROMSpeed)
            {
                // If bitmasked map mode is 0x30 ROM is FastROM, if it is 0x20 then it is SlowROM
                case 32: StringROMSpeed = "SlowROM (200 ns)"; break;
                case 48: StringROMSpeed = "FastROM (120 ns)"; break;
                default: StringROMSpeed = "Unknown"; break;
            }
        }

        private void GetROMSize()
        {
            byte[] size = new byte[1];

            if (IsBSROM)
            {
                size = new byte[4];
                Buffer.BlockCopy(SourceROMHeader, 0x20, size, 0, 4);
                uint byteSize = BitConverter.ToUInt32(size, 0);

                if (byteSize > 0 && byteSize <= 4294967295)
                {
                    IntROMSize = 1;
                    while (byteSize > 1)
                    {
                        byteSize >>= 1;
                        IntROMSize++;
                    }

                    ByteROMSize = Convert.ToByte(IntROMSize);
                    StringROMSize = IntROMSize + " Mbit";
                }

                else
                {
                    IntROMSize = 0;
                    ByteROMSize = 0;
                    StringROMSize = "Unknown";
                }
            }
            
            else
            { 
                Buffer.BlockCopy(SourceROMHeader, 0x27, size, 0, 1);
                ByteROMSize = size[0];

                if (ByteROMSize >= 0x07 && ByteROMSize <= 0x0E)
                {
                    IntROMSize = 1;

                    int count = 7;
                    while (count < ByteROMSize)
                    {
                        IntROMSize *= 2;
                        count++;
                    }

                    StringROMSize = IntROMSize + " Mbit";
                }

                else
                {
                    IntROMSize = 0;
                    ByteROMSize = 0;
                    StringROMSize = "Unknown";
                }
            }
        }

        private void CheckIsNewHeader()
        {
            bool isNewHeader = false;

            byte[] value = new byte[1];
            Buffer.BlockCopy(SourceROMHeader, 0x2A, value, 0, 1);

            if(value[0] == 0x33)
            {
                isNewHeader = true;
            }

            IsNewHeader = isNewHeader;
        }

        private void GetSRAMSize()
        {
            byte[] sramsize = new byte[1];
            if (IsBSROM) { sramsize[0] = 0x00; } else { Buffer.BlockCopy(SourceROMHeader, 0x28, sramsize, 0, 1); }

            ByteSRAMSize = sramsize[0];
        }

        private void GetExRAMSize()
        {
            byte[] exramsize = new byte[1];
            exramsize[0] = 0x00;

            if (!IsBSROM)
            {
                if(IsNewHeader)
                {
                    Buffer.BlockCopy(SourceROMHeader, 0x0D, exramsize, 0, 1);
                }

                // Star Fox/Star Wing RAM fix
                else if(StringTitle.Contains("STAR FOX") || StringTitle.Contains("STAR WING"))
                {
                    exramsize[0] = 0x06;
                }
            }

            ByteExRAMSize = exramsize[0];
            StringRAMSize = "No";

            if (ByteSRAMSize > 0x00)
            {
                StringRAMSize = "Yes (" + Math.Pow(2, ByteSRAMSize) + " kByte)";
            }

            else if (ByteExRAMSize > 0x00)
            {
                StringRAMSize = "Yes (" + Math.Pow(2, ByteExRAMSize) + " kByte)";
            }
        }

        private void CalculateFileSize()
        {
            int filesize = (SourceROM.Length * 8) / 1048576;
            IntCalcFileSize = filesize;
        }

        private void GetCountry()
        {
            byte[] country = new byte[1];

            if (IsBSROM) { country[0] = 0x00; } else { Buffer.BlockCopy(SourceROMHeader, 0x29, country, 0, 1); }

            ByteCountry = country[0];

            switch (ByteCountry)
            {
                case 0: StringCountry = "Japan"; StringRegion = "NTSC"; break;
                case 1: StringCountry = "USA"; StringRegion = "NTSC"; break;
                case 2: StringCountry = "Europe/Oceania/Asia"; StringRegion = "PAL"; break;
                case 3: StringCountry = "Sweden/Scandinavia"; StringRegion = "PAL"; break;
                case 4: StringCountry = "Finland"; StringRegion = "PAL"; break;
                case 5: StringCountry = "Denmark"; StringRegion = "PAL"; break;
                case 6: StringCountry = "France"; StringRegion = "SECAM (PAL-like, 50 Hz)"; break;
                case 7: StringCountry = "Netherlands"; StringRegion = "PAL"; break;
                case 8: StringCountry = "Spain"; StringRegion = "PAL"; break;
                case 9: StringCountry = "Germany/Austria/Switzerland"; StringRegion = "PAL"; break;
                case 10: StringCountry = "China/Hong Kong"; StringRegion = "PAL"; break;
                case 11: StringCountry = "Indonesia"; StringRegion = "PAL"; break;
                case 12: StringCountry = "South Korea"; StringRegion = "NTSC"; break;
                case 13: StringCountry = "Common (?)"; StringRegion = "?"; break;
                case 14: StringCountry = "Canada"; StringRegion = "NTSC"; break;
                case 15: StringCountry = "Brazil"; StringRegion = "PAL-M (NTSC-like, 60 Hz)"; break;
                case 16: StringCountry = "Australia"; StringRegion = "PAL"; break;
                case 17: StringCountry = "Other variation"; StringRegion = "?"; break;
                case 18: StringCountry = "Other variation"; StringRegion = "?"; break;
                case 19: StringCountry = "Other variation"; StringRegion = "?"; break;
                default: StringCountry = "Unknown"; StringRegion = "?"; break;
            }
        }

        private void GetCompany()
        {
            byte[] company = new byte[1];
            int companyCode = -1;

            Buffer.BlockCopy(SourceROMHeader, 0x2A, company, 0, 1);

            if (company[0] != 0x33)
            {
                companyCode = ((company[0] >> 4) & 0x0F) * 36 + (company[0] & 0x0F);
                IntCompany = companyCode;
            }

            else
            {
                company = new byte[2];

                Buffer.BlockCopy(SourceROMHeader, 0x00, company, 0, 2);

                if(company[0] > 0x39)
                {
                    company[0] -= 0x37;
                }

                else
                {
                    company[0] -= 0x30;
                }

                if (company[1] > 0x39)
                {
                    company[1] -= 0x37;
                }

                else
                {
                    company[1] -= 0x30;
                }

                if (company[0] >= 0 && company[1] >= 0)
                {
                    companyCode = company[0] * 36 + company[1];
                }

                IntCompany = companyCode;
            }

            string companyString = "0x" + IntCompany.ToString("X4");

            switch (companyString)
            {
                case "0x0001": StringCompany = "Nintendo"; break;
                case "0x0002": StringCompany = "Rocket Games/Ajinomoto"; break;
                case "0x0003": StringCompany = "Imagineer-Zoom"; break;
                case "0x0004": StringCompany = "Gray Matter"; break;
                case "0x0005": StringCompany = "Zamuse"; break;
                case "0x0006": StringCompany = "Falcom"; break;
                case "0x0008": StringCompany = "Capcom"; break;
                case "0x0009": StringCompany = "Hot B Co."; break;
                case "0x000A": StringCompany = "Jaleco"; break;
                case "0x000B": StringCompany = "Coconuts Japan"; break;
                case "0x000C": StringCompany = "Coconuts Japan/G.X.Media"; break;
                case "0x000D": StringCompany = "Micronet"; break;
                case "0x000E": StringCompany = "Technos"; break;
                case "0x000F": StringCompany = "Mebio Software"; break;
                case "0x0010": StringCompany = "Shouei System"; break;
                case "0x0011": StringCompany = "Starfish"; break;
                case "0x0013": StringCompany = "Mitsui Fudosan/Dentsu"; break;
                case "0x0015": StringCompany = "Warashi Inc."; break;
                case "0x0017": StringCompany = "Nowpro"; break;
                case "0x0019": StringCompany = "Game Village"; break;
                case "0x001A": StringCompany = "IE Institute"; break;
                case "0x0024": StringCompany = "Banarex"; break;
                case "0x0025": StringCompany = "Starfish"; break;
                case "0x0026": StringCompany = "Infocom"; break;
                case "0x0027": StringCompany = "Electronic Arts Japan"; break;
                case "0x0029": StringCompany = "Cobra Team"; break;
                case "0x002A": StringCompany = "Human/Field"; break;
                case "0x002B": StringCompany = "KOEI"; break;
                case "0x002C": StringCompany = "Hudson Soft"; break;
                case "0x002D": StringCompany = "S.C.P./Game Village"; break;
                case "0x002E": StringCompany = "Yanoman"; break;
                case "0x0030": StringCompany = "Tecmo Products"; break;
                case "0x0031": StringCompany = "Japan Glary Business"; break;
                case "0x0032": StringCompany = "Forum/OpenSystem"; break;
                case "0x0033": StringCompany = "Virgin Games (Japan)"; break;
                case "0x0034": StringCompany = "SMDE"; break;
                case "0x0035": StringCompany = "Yojigen"; break;
                case "0x0037": StringCompany = "Daikokudenki"; break;
                case "0x003D": StringCompany = "Creatures Inc."; break;
                case "0x003E": StringCompany = "TDK Deep Impresion"; break;
                case "0x0048": StringCompany = "Destination Software/KSS"; break;
                case "0x0049": StringCompany = "Sunsoft/Tokai Engineering"; break;
                case "0x004A": StringCompany = "POW (Planning Office Wada)/VR 1 Japan"; break;
                case "0x004B": StringCompany = "Micro World"; break;
                case "0x004D": StringCompany = "San-X"; break;
                case "0x004E": StringCompany = "Enix"; break;
                case "0x004F": StringCompany = "Loriciel/Electro Brain"; break;
                case "0x0050": StringCompany = "Kemco Japan"; break;
                case "0x0051": StringCompany = "Seta Co.,Ltd."; break;
                case "0x0052": StringCompany = "Culture Brain"; break;
                case "0x0053": StringCompany = "Irem Corp."; break;
                case "0x0054": StringCompany = "Palsoft"; break;
                case "0x0055": StringCompany = "Visit Co., Ltd."; break;
                case "0x0056": StringCompany = "Intec"; break;
                case "0x0057": StringCompany = "System Sacom"; break;
                case "0x0058": StringCompany = "Poppo"; break;
                case "0x0059": StringCompany = "Ubisoft Japan"; break;
                case "0x005B": StringCompany = "Media Works"; break;
                case "0x005C": StringCompany = "NEC InterChannel"; break;
                case "0x005D": StringCompany = "Tam"; break;
                case "0x005E": StringCompany = "Gajin/Jordan"; break;
                case "0x005F": StringCompany = "Smilesoft"; break;
                case "0x0062": StringCompany = "Mediakite"; break;
                case "0x006C": StringCompany = "Viacom"; break;
                case "0x006D": StringCompany = "Carrozzeria"; break;
                case "0x006E": StringCompany = "Dynamic"; break;
                case "0x0070": StringCompany = "Magifact"; break;
                case "0x0071": StringCompany = "Hect"; break;
                case "0x0072": StringCompany = "Codemasters"; break;
                case "0x0073": StringCompany = "Taito/GAGA Communications"; break;
                case "0x0074": StringCompany = "Laguna"; break;
                case "0x0075": StringCompany = "Telstar Fun & Games/Event/Taito"; break;
                case "0x0077": StringCompany = "Arcade Zone Ltd."; break;
                case "0x0078": StringCompany = "Entertainment International/Empire Software"; break;
                case "0x0079": StringCompany = "Loriciel"; break;
                case "0x007A": StringCompany = "Gremlin Graphics"; break;
                case "0x0090": StringCompany = "Seika Corp."; break;
                case "0x0091": StringCompany = "UBI SOFT Entertainment Software"; break;
                case "0x0092": StringCompany = "Sunsoft US"; break;
                case "0x0094": StringCompany = "Life Fitness"; break;
                case "0x0096": StringCompany = "System 3"; break;
                case "0x0097": StringCompany = "Spectrum Holobyte"; break;
                case "0x0099": StringCompany = "Irem"; break;
                case "0x009B": StringCompany = "Raya Systems"; break;
                case "0x009C": StringCompany = "Renovation Products"; break;
                case "0x009D": StringCompany = "Malibu Games"; break;
                case "0x009F": StringCompany = "Eidos/U.S. Gold"; break;
                case "0x00A0": StringCompany = "Playmates Interactive"; break;
                case "0x00A3": StringCompany = "Fox Interactive"; break;
                case "0x00A4": StringCompany = "Time Warner Interactive"; break;
                case "0x00AA": StringCompany = "Disney Interactive"; break;
                case "0x00AC": StringCompany = "Black Pearl"; break;
                case "0x00AE": StringCompany = "Advanced Productions"; break;
                case "0x00B1": StringCompany = "GT Interactive"; break;
                case "0x00B2": StringCompany = "RARE"; break;
                case "0x00B3": StringCompany = "Crave Entertainment"; break;
                case "0x00B4": StringCompany = "Absolute Entertainment"; break;
                case "0x00B5": StringCompany = "Acclaim"; break;
                case "0x00B6": StringCompany = "Activision"; break;
                case "0x00B7": StringCompany = "American Sammy"; break;
                case "0x00B8": StringCompany = "Take 2/GameTek"; break;
                case "0x00B9": StringCompany = "Hi Tech"; break;
                case "0x00BA": StringCompany = "LJN Ltd."; break;
                case "0x00BC": StringCompany = "Mattel"; break;
                case "0x00BE": StringCompany = "Mindscape/Red Orb Entertainment"; break;
                case "0x00BF": StringCompany = "Romstar"; break;
                case "0x00C0": StringCompany = "Taxan"; break;
                case "0x00C1": StringCompany = "Midway/Tradewest"; break;
                case "0x00C3": StringCompany = "American Softworks Corp."; break;
                case "0x00C4": StringCompany = "Majesco Sales Inc."; break;
                case "0x00C5": StringCompany = "3DO"; break;
                case "0x00C8": StringCompany = "Hasbro"; break;
                case "0x00C9": StringCompany = "NewKidCo"; break;
                case "0x00CA": StringCompany = "Telegames"; break;
                case "0x00CB": StringCompany = "Metro3D"; break;
                case "0x00CD": StringCompany = "Vatical Entertainment"; break;
                case "0x00CE": StringCompany = "LEGO Media"; break;
                case "0x00D0": StringCompany = "Xicat Interactive"; break;
                case "0x00D1": StringCompany = "Cryo Interactive"; break;
                case "0x00D4": StringCompany = "Red Storm Entertainment"; break;
                case "0x00D5": StringCompany = "Microids"; break;
                case "0x00D7": StringCompany = "Conspiracy/Swing"; break;
                case "0x00D8": StringCompany = "Titus"; break;
                case "0x00D9": StringCompany = "Virgin Interactive"; break;
                case "0x00DA": StringCompany = "Maxis"; break;
                case "0x00DC": StringCompany = "LucasArts Entertainment"; break;
                case "0x00DF": StringCompany = "Ocean"; break;
                case "0x00E1": StringCompany = "Electronic Arts"; break;
                case "0x00E3": StringCompany = "Laser Beam"; break;
                case "0x00E6": StringCompany = "Elite Systems"; break;
                case "0x00E7": StringCompany = "Electro Brain"; break;
                case "0x00E8": StringCompany = "The Learning Company"; break;
                case "0x00E9": StringCompany = "BBC"; break;
                case "0x00EB": StringCompany = "Software 2000"; break;
                case "0x00ED": StringCompany = "BAM! Entertainment"; break;
                case "0x00EE": StringCompany = "Studio 3"; break;
                case "0x00F2": StringCompany = "Classified Games"; break;
                case "0x00F4": StringCompany = "TDK Mediactive"; break;
                case "0x00F6": StringCompany = "DreamCatcher"; break;
                case "0x00F7": StringCompany = "JoWood Produtions"; break;
                case "0x00F8": StringCompany = "SEGA"; break;
                case "0x00F9": StringCompany = "Wannado Edition"; break;
                case "0x00FA": StringCompany = "LSP (Light & Shadow Prod.)"; break;
                case "0x00FB": StringCompany = "ITE Media"; break;
                case "0x00FC": StringCompany = "Infogrames"; break;
                case "0x00FD": StringCompany = "Interplay"; break;
                case "0x00FE": StringCompany = "JVC (US)"; break;
                case "0x00FF": StringCompany = "Parker Brothers"; break;
                case "0x0101": StringCompany = "SCI (Sales Curve Interactive)/Storm"; break;
                case "0x0104": StringCompany = "THQ Software"; break;
                case "0x0105": StringCompany = "Accolade Inc."; break;
                case "0x0106": StringCompany = "Triffix Entertainment"; break;
                case "0x0108": StringCompany = "Microprose Software"; break;
                case "0x0109": StringCompany = "Universal Interactive/Sierra/Simon & Schuster"; break;
                case "0x010B": StringCompany = "Kemco"; break;
                case "0x010C": StringCompany = "Rage Software"; break;
                case "0x010D": StringCompany = "Encore"; break;
                case "0x010F": StringCompany = "Zoo"; break;
                case "0x0110": StringCompany = "Kiddinx"; break;
                case "0x0111": StringCompany = "Simon & Schuster Interactive"; break;
                case "0x0112": StringCompany = "Asmik Ace Entertainment Inc./AIA"; break;
                case "0x0113": StringCompany = "Empire Interactive"; break;
                case "0x0116": StringCompany = "Jester Interactive"; break;
                case "0x0118": StringCompany = "Rockstar Games"; break;
                case "0x0119": StringCompany = "Scholastic"; break;
                case "0x011A": StringCompany = "Ignition Entertainment"; break;
                case "0x011B": StringCompany = "Summitsoft"; break;
                case "0x011C": StringCompany = "Stadlbauer"; break;
                case "0x0120": StringCompany = "Misawa"; break;
                case "0x0121": StringCompany = "Teichiku"; break;
                case "0x0122": StringCompany = "Namco Ltd."; break;
                case "0x0123": StringCompany = "LOZC"; break;
                case "0x0124": StringCompany = "KOEI"; break;
                case "0x0126": StringCompany = "Tokuma Shoten Intermedia"; break;
                case "0x0127": StringCompany = "Tsukuda Original"; break;
                case "0x0128": StringCompany = "DATAM-Polystar"; break;
                case "0x012B": StringCompany = "Bullet-Proof Software"; break;
                case "0x012C": StringCompany = "Vic Tokai Inc."; break;
                case "0x012E": StringCompany = "Character Soft"; break;
                case "0x012F": StringCompany = "I'Max"; break;
                case "0x0130": StringCompany = "Saurus"; break;
                case "0x0133": StringCompany = "General Entertainment"; break;
                case "0x0136": StringCompany = "I'Max"; break;
                case "0x0137": StringCompany = "Success"; break;
                case "0x0139": StringCompany = "SEGA Japan"; break;
                case "0x0144": StringCompany = "Takara"; break;
                case "0x0145": StringCompany = "Chun Soft"; break;
                case "0x0146": StringCompany = "Video System Co., Ltd./McO'River"; break;
                case "0x0147": StringCompany = "BEC"; break;
                case "0x0149": StringCompany = "Varie"; break;
                case "0x014A": StringCompany = "Yonezawa/S'pal"; break;
                case "0x014B": StringCompany = "Kaneko"; break;
                case "0x014D": StringCompany = "Victor Interactive Software/Pack-in-Video"; break;
                case "0x014E": StringCompany = "Nichibutsu/Nihon Bussan"; break;
                case "0x014F": StringCompany = "Tecmo"; break;
                case "0x0150": StringCompany = "Imagineer"; break;
                case "0x0153": StringCompany = "Nova"; break;
                case "0x0154": StringCompany = "Den'Z"; break;
                case "0x0155": StringCompany = "Bottom Up"; break;
                case "0x0157": StringCompany = "TGL (Technical Group Laboratory)"; break;
                case "0x0159": StringCompany = "Hasbro Japan"; break;
                case "0x015B": StringCompany = "Marvelous Entertainment"; break;
                case "0x015D": StringCompany = "Keynet Inc."; break;
                case "0x015E": StringCompany = "Hands-On Entertainment"; break;
                case "0x0168": StringCompany = "Telenet"; break;
                case "0x0169": StringCompany = "Hori"; break;
                case "0x016C": StringCompany = "Konami"; break;
                case "0x016D": StringCompany = "K.Amusement Leasing Co."; break;
                case "0x016E": StringCompany = "Kawada"; break;
                case "0x016F": StringCompany = "Takara"; break;
                case "0x0171": StringCompany = "Technos Japan Corp."; break;
                case "0x0172": StringCompany = "JVC (Europe/Japan)/Victor Musical Industries"; break;
                case "0x0174": StringCompany = "Toei Animation"; break;
                case "0x0175": StringCompany = "Toho"; break;
                case "0x0177": StringCompany = "Namco"; break;
                case "0x0178": StringCompany = "Media Rings Corp."; break;
                case "0x0179": StringCompany = "J-Wing"; break;
                case "0x017B": StringCompany = "Pioneer LDC"; break;
                case "0x017C": StringCompany = "KID"; break;
                case "0x017D": StringCompany = "Mediafactory"; break;
                case "0x0181": StringCompany = "Infogrames Hudson"; break;
                case "0x018C": StringCompany = "Acclaim Japan"; break;
                case "0x018D": StringCompany = "ASCII Co./Nexoft"; break;
                case "0x018E": StringCompany = "Bandai"; break;
                case "0x0190": StringCompany = "Enix"; break;
                case "0x0192": StringCompany = "HAL Laboratory/Halken"; break;
                case "0x0193": StringCompany = "SNK"; break;
                case "0x0195": StringCompany = "Pony Canyon Hanbai"; break;
                case "0x0196": StringCompany = "Culture Brain"; break;
                case "0x0197": StringCompany = "Sunsoft"; break;
                case "0x0198": StringCompany = "Toshiba EMI"; break;
                case "0x0199": StringCompany = "Sony Imagesoft"; break;
                case "0x019B": StringCompany = "Sammy"; break;
                case "0x019C": StringCompany = "Magical"; break;
                case "0x019D": StringCompany = "Visco"; break;
                case "0x019F": StringCompany = "Compile"; break;
                case "0x01A1": StringCompany = "MTO Inc."; break;
                case "0x01A3": StringCompany = "Sunrise Interactive"; break;
                case "0x01A5": StringCompany = "Global A Entertainment"; break;
                case "0x01A6": StringCompany = "Fuuki"; break;
                case "0x01B0": StringCompany = "Taito"; break;
                case "0x01B2": StringCompany = "Kemco"; break;
                case "0x01B3": StringCompany = "Square"; break;
                case "0x01B4": StringCompany = "Tokuma Shoten"; break;
                case "0x01B5": StringCompany = "Data East"; break;
                case "0x01B6": StringCompany = "Tonkin House"; break;
                case "0x01B8": StringCompany = "KOEI"; break;
                case "0x01BA": StringCompany = "Konami/Ultra/Palcom"; break;
                case "0x01BB": StringCompany = "NTVIC/VAP"; break;
                case "0x01BC": StringCompany = "Use Co., Ltd."; break;
                case "0x01BD": StringCompany = "Meldac"; break;
                case "0x01BE": StringCompany = "Pony Canyon (Japan)/FCI (US)"; break;
                case "0x01BF": StringCompany = "Angel/Sotsu Agency/Sunrise"; break;
                case "0x01C0": StringCompany = "Yumedia/Aroma Co., Ltd."; break;
                case "0x01C3": StringCompany = "Boss"; break;
                case "0x01C4": StringCompany = "Axela/Crea-Tech"; break;
                case "0x01C5": StringCompany = "Sekaibunka-Sha/Sumire kobo/Marigul Management Inc."; break;
                case "0x01C6": StringCompany = "Konami Computer Entertainment Osaka"; break;
                case "0x01C9": StringCompany = "Enterbrain"; break;
                case "0x01D4": StringCompany = "Taito/Disco"; break;
                case "0x01D5": StringCompany = "Sofel"; break;
                case "0x01D6": StringCompany = "Quest Corp."; break;
                case "0x01D7": StringCompany = "Sigma"; break;
                case "0x01D8": StringCompany = "Ask Kodansha"; break;
                case "0x01DA": StringCompany = "Naxat"; break;
                case "0x01DB": StringCompany = "Copya System"; break;
                case "0x01DC": StringCompany = "Capcom Co., Ltd."; break;
                case "0x01DD": StringCompany = "Banpresto"; break;
                case "0x01DE": StringCompany = "TOMY"; break;
                case "0x01DF": StringCompany = "Acclaim/LJN Japan"; break;
                case "0x01E1": StringCompany = "NCS"; break;
                case "0x01E2": StringCompany = "Human Entertainment"; break;
                case "0x01E3": StringCompany = "Altron"; break;
                case "0x01E4": StringCompany = "Jaleco"; break;
                case "0x01E5": StringCompany = "Gaps Inc."; break;
                case "0x01EB": StringCompany = "Elf"; break;
                case "0x01F8": StringCompany = "Jaleco"; break;
                case "0x01FA": StringCompany = "Yutaka"; break;
                case "0x01FB": StringCompany = "Varie"; break;
                case "0x01FC": StringCompany = "T&ESoft"; break;
                case "0x01FD": StringCompany = "Epoch Co., Ltd."; break;
                case "0x01FF": StringCompany = "Athena"; break;
                case "0x0200": StringCompany = "Asmik"; break;
                case "0x0201": StringCompany = "Natsume"; break;
                case "0x0202": StringCompany = "King Records"; break;
                case "0x0203": StringCompany = "Atlus"; break;
                case "0x0204": StringCompany = "Epic/Sony Records (Japan)"; break;
                case "0x0206": StringCompany = "IGS (Information Global Service)"; break;
                case "0x0208": StringCompany = "Chatnoir"; break;
                case "0x0209": StringCompany = "Right Stuff"; break;
                case "0x020B": StringCompany = "NTT COMWARE"; break;
                case "0x020D": StringCompany = "Spike"; break;
                case "0x020E": StringCompany = "Konami Computer Entertainment Tokyo"; break;
                case "0x020F": StringCompany = "Alphadream Corp."; break;
                case "0x0211": StringCompany = "Sting"; break;
                case "0x021C": StringCompany = "A Wave"; break;
                case "0x021D": StringCompany = "Motown Software"; break;
                case "0x021E": StringCompany = "Left Field Entertainment"; break;
                case "0x021F": StringCompany = "Extreme Entertainment Group"; break;
                case "0x0220": StringCompany = "TecMagik"; break;
                case "0x0225": StringCompany = "Cybersoft"; break;
                case "0x0227": StringCompany = "Psygnosis"; break;
                case "0x022A": StringCompany = "Davidson/Western Tech."; break;
                case "0x022B": StringCompany = "Unlicensed"; break;
                case "0x0230": StringCompany = "The Game Factory Europe"; break;
                case "0x0231": StringCompany = "Hip Games"; break;
                case "0x0232": StringCompany = "Aspyr"; break;
                case "0x0235": StringCompany = "Mastiff"; break;
                case "0x0236": StringCompany = "iQue"; break;
                case "0x0237": StringCompany = "Digital Tainment Pool"; break;
                case "0x0238": StringCompany = "XS Games"; break;
                case "0x0239": StringCompany = "Daiwon"; break;
                case "0x0241": StringCompany = "PCCW Japan"; break;
                case "0x0244": StringCompany = "KiKi Co. Ltd."; break;
                case "0x0245": StringCompany = "Open Sesame Inc."; break;
                case "0x0246": StringCompany = "Sims"; break;
                case "0x0247": StringCompany = "Broccoli"; break;
                case "0x0248": StringCompany = "Avex"; break;
                case "0x0249": StringCompany = "D3 Publisher"; break;
                case "0x024B": StringCompany = "Konami Computer Entertainment Japan"; break;
                case "0x024D": StringCompany = "Square-Enix"; break;
                case "0x024E": StringCompany = "KSG"; break;
                case "0x024F": StringCompany = "Micott & Basara Inc."; break;
                case "0x0251": StringCompany = "Orbital Media"; break;
                case "0x0262": StringCompany = "The Game Factory USA"; break;
                case "0x0265": StringCompany = "Treasure"; break;
                case "0x0266": StringCompany = "Aruze"; break;
                case "0x0267": StringCompany = "Ertain"; break;
                case "0x0268": StringCompany = "SNK Playmore"; break;
                case "0x0299": StringCompany = "Yojigen"; break;
                default: StringCompany = "Unknown"; break;
            }
        }

        private void CalculateChecksum()
        {
            uint offsetChksm = UIntROMHeaderOffset + 0x2C;
            ulong checksum = 0;
            uint multiplier = 1;
            byte[] byteChksm = new byte[2];

            // For clearing out checksum - checksum = 0x0000 , inverse = 0xFFFF
            byte[] clearSum = new byte[4] { 0xFF, 0xFF, 0x00, 0x00 };

            // Make a copy with clean checksum of the ROM for independent calculation
            byte[] noChecksumSourceROM = new byte[SourceROM.Length];
            Buffer.BlockCopy(SourceROM, 0, noChecksumSourceROM, 0, SourceROM.Length);
            Buffer.BlockCopy(clearSum, 0, noChecksumSourceROM, (int)offsetChksm, clearSum.Length);

            // For BS-X ROMs fill header with 0x00
            if (IsBSROM)
            {
                byte[] clearBSHeader = new byte [48] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                       0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                       0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                Buffer.BlockCopy(clearBSHeader, 0, noChecksumSourceROM, (int)UIntROMHeaderOffset, clearBSHeader.Length);
            }

            // Mirror ROM if neccessary | not working for Momotarou Dentetsu Happy and Tengai Makyou Zero / Tengai Makyou Zero - Shounen Jump no Shou (3 MByte ROMs with Special Chip + RAM + SRAM)
            if ((IntROMSize > IntCalcFileSize) && ByteROMType != 0xF5 && ByteROMType != 0xF9)
            {
                // Get mirrored ROM
                noChecksumSourceROM = Mirror(noChecksumSourceROM);
            }

            // Momotarou Dentetsu Happy fix | This didn't work with expanded ROM: else if (calcFileSize == 24 && romType == 0xF5)
            else if (IntROMSize == 32 && ByteROMType == 0xF5)
            {
                multiplier = 2;
            }

            // Calculate checksum
            foreach(byte singleByte in noChecksumSourceROM)
            {
                checksum += singleByte;
            }

            // Because of Momotarou Dentetsu Happy... again...
            checksum *= multiplier;

            // Take only first 16 bit from checksum
            checksum &= 0xFFFF;
            UInt16 checksum16 = (UInt16)checksum;

            // Return checksum as big endian byte[]
            byteChksm = BitConverter.GetBytes(checksum16);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteChksm);
            }

            ByteArrayCalcChecksum = byteChksm;
        }

        private void CalculateInverseChecksum()
        {
            UInt16 calcInvChksm = 0;
            byte[] byteInvChksm = new byte[2];

            // Calculate inverse checksum (XOR with 0xFFFF)
            calcInvChksm = BitConverter.ToUInt16(ByteArrayCalcChecksum, 0);
            calcInvChksm ^= 0xFFFF;

            // Return checksum as byte[]
            byteInvChksm = BitConverter.GetBytes(calcInvChksm);

            ByteArrayCalcInvChecksum = byteInvChksm;
        }

        private void GetChecksum()
        {
            byte[] checksum = new byte[2];

            Buffer.BlockCopy(SourceROMHeader, 0x2E, checksum, 0, 2);

            // Return checksum as big endian byte[]
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(checksum);
            }

            ByteArrayChecksum = checksum;
        }

        private void GetInverseChecksum()
        {
            byte[] checksum = new byte[2];

            Buffer.BlockCopy(SourceROMHeader, 0x2C, checksum, 0, 2);

            // Return checksum as big endian byte[]
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(checksum);
            }

            ByteArrayInvChecksum = checksum;
        }

        private void GetVersion()
        {
            byte[] version = new byte[1];

            Buffer.BlockCopy(SourceROMHeader, 0x2B, version, 0, 1);

            ByteVersion = version[0];

            StringVersion = "1." + ByteVersion;
        }

        private void GetGameCode()
        {
            if (IsNewHeader && !IsBSROM)
            {
                byte[] gamecode = new byte[4];
                Buffer.BlockCopy(SourceROMHeader, 0x02, gamecode, 0, 4);
                ByteArrayGameCode = gamecode;
                StringGameCode = Encoding.GetEncoding(932).GetString(ByteArrayGameCode);
            }

            else
            {
                StringGameCode = "N/A";
            }
        }

        private void CalculateCrc32Hash()
        {
            Crc32 crc32 = new Crc32();
            string hash = null;
            byte[] CRC32SourceROM = new byte[SourceROM.Length + UIntSMCHeader];

            if (SourceROMSMCHeader != null && UIntSMCHeader > 0)
            {
                // Merge header with ROM if header exists
                Buffer.BlockCopy(SourceROMSMCHeader, 0, CRC32SourceROM, 0, SourceROMSMCHeader.Length);
                Buffer.BlockCopy(SourceROM, 0, CRC32SourceROM, SourceROMSMCHeader.Length, SourceROM.Length);
            }

            else
            {
                Buffer.BlockCopy(SourceROM, 0, CRC32SourceROM, 0, SourceROM.Length);
            }

            foreach (byte singleByte in crc32.ComputeHash(CRC32SourceROM))
            {
                hash += singleByte.ToString("X2").ToUpper();
            }

            CRC32Hash = hash;
        }
    }
}