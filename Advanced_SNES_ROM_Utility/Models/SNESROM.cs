using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Advanced_SNES_ROM_Utility.Functions;
using Advanced_SNES_ROM_Utility.Helper;
using Advanced_SNES_ROM_Utility.Lists;

namespace Advanced_SNES_ROM_Utility
{
    public class SNESROM
    {
        public string ROMName { get; set; }
        public string ROMFullPath { get; set; }
        public string ROMFolder { get; set; }

        public byte[] SourceROM { get; set; }
        public byte[] SourceROMHeader { get; set; }
        public byte[] SourceROMSMCHeader { get; set; }

        public byte[] ByteArrayChecksum { get; set; }
        public byte[] ByteArrayInvChecksum { get; set; }
        public byte[] ByteArrayCalcChecksum { get; set; }
        public byte[] ByteArrayCalcInvChecksum { get; set; }

        public uint UIntROMHeaderOffset { get; set; }
        public uint UIntSMCHeader { get; set; }
        public int IntROMSize { get; set; }
        public int IntCalcFileSize { get; set; }
        public string CRC32Hash { get; set; }

        public bool IsNewHeader { get; set; }
        public bool IsBSROM { get; set; }
        public bool IsInterleaved { get; set; }

        public byte ByteROMType { get; set; }
        public byte ByteMapMode { get; set; }
        public byte ByteROMSpeed { get; set; }
        public byte ByteSRAMSize { get; set; }
        public byte ByteExRAMSize { get; set; }
        public byte ByteVersion { get; set; }
        public byte ByteROMSize { get; set; }
        public byte[] ByteArrayTitle { get; set; }
        public byte ByteCountry { get; set; }
        public int IntCompany { get; set; }
        public byte[] ByteArrayGameCode { get; set; }

        public string StringROMType { get; set; }
        public string StringMapMode { get; set; }
        public string StringROMSpeed { get; set; }
        public string StringRAMSize { get; set; }
        public string StringVersion { get; set; }
        public string StringROMSize { get; set; }
        public string StringTitle { get; set; }
        public string StringCountry { get; set; }
        public string StringCompany { get; set; }
        public string StringRegion { get; set; }
        public string StringSMCHeader { get; set; }
        public string StringGameCode { get; set; }

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
            IntCalcFileSize = SNESROMFunction.CalculateFileSize(SourceROM);
            ByteArrayCalcChecksum = SNESROMFunction.CalculateChecksum(SourceROM, UIntROMHeaderOffset, IsBSROM, IntROMSize, IntCalcFileSize, ByteROMType);
            ByteArrayCalcInvChecksum = SNESROMFunction.CalculateInverseChecksum(ByteArrayCalcChecksum);
            CRC32Hash = SNESROMFunction.CalculateCrc32Hash(SourceROM, SourceROMSMCHeader, UIntSMCHeader);
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
            UIntROMHeaderOffset = (int)HeaderOffset.lorom;
            IsBSROM = false;

            int mapModeScoreLoROM = SNESROMFunction.CalculateMapModeScore(SourceROM, (int)HeaderOffset.lorom, false);
            int mapModeScoreBSLoROM = SNESROMFunction.CalculateMapModeScore(SourceROM, (int)HeaderOffset.lorom, true);
            int mapModeScoreHiROM = SNESROMFunction.CalculateMapModeScore(SourceROM, (int)HeaderOffset.hirom, false);
            int mapModeScoreBSHiROM = SNESROMFunction.CalculateMapModeScore(SourceROM, (int)HeaderOffset.hirom, true);
            int mapModeScoreExLoROM = SNESROMFunction.CalculateMapModeScore(SourceROM, (int)HeaderOffset.exlorom, false);
            int mapModeScoreExHiROM = SNESROMFunction.CalculateMapModeScore(SourceROM, (int)HeaderOffset.exhirom, false);

            if (mapModeScoreLoROM >= mapModeScoreHiROM && mapModeScoreLoROM >= mapModeScoreExLoROM && mapModeScoreLoROM >= mapModeScoreExHiROM)
            {
                UIntROMHeaderOffset = (int)HeaderOffset.lorom;

                if (mapModeScoreBSLoROM > mapModeScoreLoROM)
                {
                    IsBSROM = true;
                }
            }

            else if (mapModeScoreHiROM >= mapModeScoreExLoROM && mapModeScoreHiROM >= mapModeScoreExHiROM)
            {
                UIntROMHeaderOffset = (int)HeaderOffset.hirom;

                if (mapModeScoreBSHiROM > mapModeScoreHiROM)
                {
                    IsBSROM = true;
                }
            }

            else if (mapModeScoreExLoROM >= mapModeScoreExHiROM)
            {
                UIntROMHeaderOffset = (int)HeaderOffset.exlorom;
            }

            else
            {
                UIntROMHeaderOffset = (int)HeaderOffset.exhirom;
            }

            // Load header
            SourceROMHeader = new byte[80];
            Buffer.BlockCopy(SourceROM, (int)UIntROMHeaderOffset, SourceROMHeader, 0, 80);
        }

        private void GetTitle()
        {
            byte[] title = new byte[21];

            if (IsBSROM) { title = new byte[16]; Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.title, title, 0, 16); } else { Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.title, title, 0, 21); if (title[20] == 0x00) { title = new byte[20]; Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.title, title, 0, 20); } }

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
            IsInterleaved = false;

            if (IsBSROM) { Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.bs_mapmode, mapMode, 0, 1); } else { Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.mapmode, mapMode, 0, 1); }
            
            // SPC7110 games have an odd value in their header and don't have to be bitmasked
            if (mapMode[0] != (byte)MapMode.hirom_spc7110)
            {
                // Bitmask this byte, because non relevant bits are not clearly defined
                mapMode[0] &= (byte)MapMode.bitmask;
            }

            ByteMapMode = mapMode[0];
            StringMapMode = string.IsNullOrEmpty(SNESROMHelper.GetEnumDescription((MapMode)ByteMapMode)) ? "Unknown" : SNESROMHelper.GetEnumDescription((MapMode)ByteMapMode);

            if (StringMapMode.Contains(SNESROMHelper.GetEnumDescription((MapMode)0x21)) && (UIntROMHeaderOffset == (uint)HeaderOffset.lorom || UIntROMHeaderOffset == (uint)HeaderOffset.exlorom))
            {
                IsInterleaved = true;
            }

            // ROM that contains oversized title which overwrites the map mode byte, but actually is LoROM
            if (StringTitle.Equals("YUYU NO QUIZ DE GO!GO"))
            {
                StringMapMode = SNESROMHelper.GetEnumDescription((MapMode)0x20);

                // If this ROM is not interleaved set it as not interleaved
                if (UIntROMHeaderOffset == (uint)HeaderOffset.lorom)
                {
                    IsInterleaved = false;
                }
            }
        }

        private void GetROMType()
        {
            byte[] type = new byte[1];
            if (IsBSROM) { Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.bs_type, type, 0, 1); } else { Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.type, type, 0, 1); }

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
                case 0x05: if (ByteROMSpeed == (byte)Speed.slow) { StringROMType = "ROM+DSP2+RAM+Battery"; } else if (ByteROMSpeed == (byte)Speed.fast && IntCompany == 0x018E) { StringROMType = "ROM+DSP3+RAM+Battery"; } else { StringROMType = "ROM+DSP1+RAM+Battery"; }; break;
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
                case 0xF5: if (StringMapMode.Contains(SNESROMHelper.GetEnumDescription((MapMode)0x21))) { StringROMType = "ROM+SPC-7110+RAM+Battery"; } else if (StringMapMode.Contains(SNESROMHelper.GetEnumDescription((MapMode)0x20))) { StringROMType = "ROM+ST-018+RAM+Battery"; }; break;
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
            StringROMSpeed = string.IsNullOrEmpty(SNESROMHelper.GetEnumDescription((Speed)ByteROMSpeed)) ? "Unknown" : SNESROMHelper.GetEnumDescription((Speed)ByteROMSpeed);
        }

        private void GetROMSize()
        {
            byte[] size = new byte[1];

            if (IsBSROM)
            {
                size = new byte[4];
                Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.bs_size, size, 0, 4);
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
                Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.size, size, 0, 1);
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
            Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.company, value, 0, 1);

            if(value[0] == 0x33)
            {
                isNewHeader = true;
            }

            IsNewHeader = isNewHeader;
        }

        private void GetSRAMSize()
        {
            byte[] sramsize = new byte[1];
            if (IsBSROM) { sramsize[0] = 0x00; } else { Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.sram, sramsize, 0, 1); }

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
                    Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.exram, exramsize, 0, 1);
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

        private void GetCountry()
        {
            byte[] country = new byte[1];

            if (IsBSROM) { country[0] = 0x00; } else { Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.country, country, 0, 1); }

            ByteCountry = country[0];

            try
            {
                StringCountry = SNESROMList.CountryRegion[ByteCountry, 0];
                StringRegion = SNESROMList.CountryRegion[ByteCountry, 1];
            }

            catch
            {
                StringCountry = "Unknown";
                StringRegion = "?";
            }
        }

        private void GetCompany()
        {
            byte[] company = new byte[1];
            int companyCode = -1;

            Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.company, company, 0, 1);

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

            try
            {
                StringCompany = SNESROMList.Company["0x" + IntCompany.ToString("X4")];
            }

            catch
            {
                StringCompany = "Unknown";
            }
            
        }

        private void GetChecksum()
        {
            byte[] checksum = new byte[2];

            Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.checksum, checksum, 0, 2);

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

            Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.inverse_checksum, checksum, 0, 2);

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

            Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.version, version, 0, 1);

            ByteVersion = version[0];

            StringVersion = "1." + ByteVersion;
        }

        private void GetGameCode()
        {
            if (IsNewHeader && !IsBSROM)
            {
                byte[] gamecode = new byte[4];
                Buffer.BlockCopy(SourceROMHeader, (int)HeaderValue.gamecode, gamecode, 0, 4);
                ByteArrayGameCode = gamecode;
                StringGameCode = Encoding.GetEncoding(932).GetString(ByteArrayGameCode);
            }

            else
            {
                StringGameCode = "N/A";
            }
        }

        public void SetTitle(string newTitle, int maxLength)
        {
            Encoding newEncodedTitle = Encoding.GetEncoding(932);
            byte[] newByteTitle = newEncodedTitle.GetBytes(newTitle.Trim());

            byte[] byteArrayTitle = new byte[maxLength];

            for (int i = 0; i < maxLength; i++)
            {
                byteArrayTitle[i] = 0x20;
            }

            int newByteTitleTempLenght = newByteTitle.Length;

            if (newByteTitle.Length > byteArrayTitle.Length) { newByteTitleTempLenght = byteArrayTitle.Length; }

            Buffer.BlockCopy(newByteTitle, 0, byteArrayTitle, 0, newByteTitleTempLenght);

            Buffer.BlockCopy(byteArrayTitle, 0, SourceROM, (int)UIntROMHeaderOffset + (int)HeaderValue.title, byteArrayTitle.Length);

            if (UIntROMHeaderOffset == (uint)HeaderOffset.exlorom || UIntROMHeaderOffset == (uint)HeaderOffset.exhirom)
            {
                Buffer.BlockCopy(byteArrayTitle, 0, SourceROM, (int)UIntROMHeaderOffset + (int)HeaderValue.title - 0x400000, byteArrayTitle.Length);
            }

            Initialize();
        }

        public void SetVersion(byte newVersion)
        {
            byte[] byteArrayVersion = { newVersion };
            Buffer.BlockCopy(byteArrayVersion, 0, SourceROM, (int)UIntROMHeaderOffset + (int)HeaderValue.version, 1);

            if (UIntROMHeaderOffset == (uint)HeaderOffset.exlorom || UIntROMHeaderOffset == (uint)HeaderOffset.exhirom)
            {
                Buffer.BlockCopy(byteArrayVersion, 0, SourceROM, (int)UIntROMHeaderOffset + (int)HeaderValue.version - 0x400000, 1);
            }

            Initialize();
        }

        public void SetCountryRegion(byte newCountryRegion)
        {
            byte[] byteArrayCountryRegion = { newCountryRegion };
            Buffer.BlockCopy(byteArrayCountryRegion, 0, SourceROM, (int)UIntROMHeaderOffset + (int)HeaderValue.country, 1);

            if (UIntROMHeaderOffset == (uint)HeaderOffset.exlorom || UIntROMHeaderOffset == (uint)HeaderOffset.exhirom)
            {
                Buffer.BlockCopy(byteArrayCountryRegion, 0, SourceROM, (int)UIntROMHeaderOffset + (int)HeaderValue.country - 0x400000, 1);
            }

            Initialize();
        }

        public void SetGameCode(string newGameCode)
        {
            Encoding newEncodedGameCode = Encoding.GetEncoding(932);
            byte[] newByteGameCode = newEncodedGameCode.GetBytes(newGameCode.Trim());

            byte[] byteArrayGameCode = new byte[4] { 0x20, 0x20, 0x20, 0x20 };

            Buffer.BlockCopy(newByteGameCode, 0, byteArrayGameCode, 0, newByteGameCode.Length);

            Buffer.BlockCopy(byteArrayGameCode, 0, SourceROM, (int)UIntROMHeaderOffset + (int)HeaderValue.gamecode, 4);

            if (UIntROMHeaderOffset == (uint)HeaderOffset.exlorom || UIntROMHeaderOffset == (uint)HeaderOffset.exhirom)
            {
                Buffer.BlockCopy(byteArrayGameCode, 0, SourceROM, (int)UIntROMHeaderOffset + (int)HeaderValue.gamecode - 0x400000, 4);
            }

            Initialize();
        }
    }
}