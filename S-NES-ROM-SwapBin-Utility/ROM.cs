using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Advanced_SNES_ROM_Utility
{
    class ROM
    {
        public static uint CheckSMCHeader(byte[] sourceROM)
        {
            // Calculate size of header
            int header = sourceROM.Length % 1024;

            return (uint)header;
        }
        public static int GetMapModeScore(byte[] sourceROM, uint offset, bool isBSROM)
        {
            // Check if ROM has normal or extended size
            if (sourceROM.Length < offset + 0x50)
            {
                return -100;
            }

            /* For Debugging only
            string cOpCode = "";
            string cChecksum = "";
            string cMapMode = "";
            string cFixedValue33 = "";
            string cFixedValue00 = "";
            string cROMSize = "";
            string cDestinationcode = "";
            string cBSTitle = "";
            */

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

                // Debugging
                //cOpCode = "✓ (+8)";
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

                // Debugging
                //cOpCode = "✓ (+4)";
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

                // Debugging
                //cOpCode = "❌ (-4)";
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

                // Debugging
                //cOpCode = "❌ (-8)";
            }

            // Check if checksums add up to 0xFFFF
            if (BitConverter.ToUInt16(checksum, 0) + BitConverter.ToUInt16(checksumInverse, 0) == 0xFFFF)
            {
                score += 4;

                // Debugging
                //cChecksum = "✓ (+4)";
            }

            // Check if internal ROM type is valid
            // LoROM
            if (offset == 0x7FB0 && (mapMode[0] == 0x20 || mapMode[0] == 0x30))
            {
                score += 2;

                // Debugging
                //cMapMode = "✓ (+2)";
            }
            // HiROM
            if (offset == 0xFFB0 && (mapMode[0] == 0x21 || mapMode[0] == 0x31))
            {
                score += 2;

                // Debugging
                //cMapMode = "✓ (+2)";
            }
            // ExLoROM - f.e. Star Ocean
            if (offset == 0x407FB0 && mapMode[0] == 0x32)
            {
                score += 4;

                // Debugging
                //cMapMode = "✓ (+4)";
            }
            // ExHiROM - f.e. Tales Of Phantasia
            if (offset == 0x40FFB0 && mapMode[0] == 0x35)
            {
                score += 4;

                // Debugging
                //cMapMode = "✓ (+4)";
            }

            // Check if internal ROM size is vaild
            if (romSize[0] >= 0x07 && romSize[0] <= 0x0D)
            {
                score += 2;

                // Debugging
                //cROMSize = "✓ (+2)";
            }

            // Check if destination code size is vaild
            if (destinationCode[0] <= 0x14)
            {
                score += 2;

                // Debugging
                //cDestinationcode = "✓ (+2)";
            }

            // Check for some fixed ROM values
            if (fixedValue33[0] == 0x33)
            {
                score += 4;

                // Debugging
                //cFixedValue33 = "✓ (+4)";
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

                // Debugging
                //cFixedValue00 = "✓ (+2)";
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

                    // Debugging
                    //cBSTitle = "✓ (+2)";
                }
            }

            /* Debugging
            string sOffset = offset.ToString("X4");
            string sChecksum = BitConverter.ToString(checksum);
            string sChecksumInverse = BitConverter.ToString(checksumInverse);
            string sMapMode = BitConverter.ToString(mapMode);
            string sResetVector = BitConverter.ToString(resetVector);
            string sOPCode = BitConverter.ToString(opCode);
            
            MessageBox.Show("Offset: " + sOffset + "\nReset Vector: " + sResetVector + "\nOpcode: " + sOPCode + " " + cOpCode + "\nChecksum: " + sChecksum + "\nInverse Checksum: " + sChecksumInverse + "\nChesksum addition: " + (BitConverter.ToUInt16(checksum, 0) + BitConverter.ToUInt16(checksumInverse, 0)).ToString("X4") + " " + cChecksum + "\nMap Mode: " + sMapMode + " " + cMapMode + "\nROM Size " + cROMSize + "\nDestinationcode " + cDestinationcode + "\nFixed Value (0x33) " + cFixedValue33 + "\nFixed Value (0x00) " + cFixedValue00 + "\nBS Title " + cBSTitle + "\nScore: " + score);
            */

            // Return score
            return score;
        }

        public static byte[] ReadTitle(byte[] sourceROMHeader, bool isBSROM)
        {
            byte[] title = new byte[21];

            if (isBSROM) { title = new byte[16]; Buffer.BlockCopy(sourceROMHeader, 0x10, title, 0, 16); } else { Buffer.BlockCopy(sourceROMHeader, 0x10, title, 0, 21); }

            // Return title as little endian byte[]
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(title);
            }

            return title;
        }

        public static byte[] ReadMapMode(byte[] sourceROMHeader, bool isBSROM)
        {
            byte[] mapMode = new byte[1];
            if (isBSROM) { Buffer.BlockCopy(sourceROMHeader, 0x28, mapMode, 0, 1); } else { Buffer.BlockCopy(sourceROMHeader, 0x25, mapMode, 0, 1); }
            
            // SPC7110 games have an odd value in their header but actually are HiROM
            if (mapMode[0] == 0x3A)
            {
                mapMode[0] = 0x21;
            }

            // Bitmask this byte, because non relevant bits are not clearly defined
            mapMode[0] &= 0x37;

            return mapMode;
        }

        public static byte[] ReadROMType(byte[] sourceROMHeader, bool isBSROM)
        {
            byte[] type = new byte[1];
            if (isBSROM) { Buffer.BlockCopy(sourceROMHeader, 0x29, type, 0, 1); } else { Buffer.BlockCopy(sourceROMHeader, 0x26, type, 0, 1); }

            return type;
        }
        public static byte ReadROMSpeed(byte mapMode)
        {
            byte speed = mapMode;
            // Bitmask first nibble, because only this information is needed
            speed &= 0xF0;

            return speed;
        }

        public static byte[] ReadROMSize(byte[] sourceROMHeader, bool isBSROM)
        {
            byte[] size = new byte[1];

            if (isBSROM)
            {
                size = new byte[4];
                Buffer.BlockCopy(sourceROMHeader, 0x20, size, 0, 4);
                int intSize = BitConverter.ToInt32(size, 0);

                int count = 0;
                while (intSize > 0)
                {
                    count += intSize & 1;
                    intSize >>= 1;
                }

                size = new byte[1];
                size[0] = Convert.ToByte(count);

                switch (size[0])
                {
                    case 0x01: size[0] = 0x07; break;
                    case 0x02: size[0] = 0x08; break;
                    case 0x04: size[0] = 0x09; break;
                    case 0x08: size[0] = 0x0A; break;
                    case 0x10: size[0] = 0x0B; break;
                    case 0x20: size[0] = 0x0C; break;
                    case 0x40: size[0] = 0x0D; break;
                    default: size[0] = 0x00; break;
                }
            } 
            
            else
            { 
                Buffer.BlockCopy(sourceROMHeader, 0x27, size, 0, 1);
            }

            return size;
        }

        public static bool CheckIsNewHeader(byte[] sourceROMHeader)
        {
            bool isNewHeader = false;

            byte[] value = new byte[1];
            Buffer.BlockCopy(sourceROMHeader, 0x2A, value, 0, 1);

            if(value[0] == 0x33)
            {
                isNewHeader = true;
            }

            return isNewHeader;
        }

        public static byte[] ReadSRAMSize(byte[] sourceROMHeader, bool isBSROM)
        {
            byte[] sramsize = new byte[1];
            if (isBSROM) { sramsize[0] = 0x00; } else { Buffer.BlockCopy(sourceROMHeader, 0x28, sramsize, 0, 1); }

            return sramsize;
        }

        public static byte[] ReadExRAMSize(byte[] sourceROMHeader, bool isNewHeader, string stringTitle, bool isBSROM)
        {
            byte[] exramsize = new byte[1];

            exramsize[0] = 0x00;

            if (!isBSROM)
            {
                if(isNewHeader)
                {
                    Buffer.BlockCopy(sourceROMHeader, 0x0D, exramsize, 0, 1);
                }

                // Star Fox/Star Wing RAM fix
                else if(stringTitle.Contains("STAR FOX") || stringTitle.Contains("STAR WING"))
                {
                    exramsize[0] = 0x06;
                }
            }

            return exramsize;
        }

        public static int CalculateFileSize(byte[] sourceROM)
        {
            int filesize = (sourceROM.Length * 8) / 1048576;
            return filesize;
        }

        public static byte[] ReadCountryCode(byte[] sourceROMHeader, bool isBSROM)
        {
            byte[] country = new byte[1];

            if (isBSROM) { country[0] = 0x00; } else { Buffer.BlockCopy(sourceROMHeader, 0x29, country, 0, 1); }

            return country;
        }

        public static int ReadCompanyCode(byte[] sourceROMHeader)
        {
            byte[] company = new byte[1];
            int companyCode = -1;

            Buffer.BlockCopy(sourceROMHeader, 0x2A, company, 0, 1);

            if (company[0] != 0x33)
            {
                companyCode = ((company[0] >> 4) & 0x0F) * 36 + (company[0] & 0x0F);
                return companyCode;
            }

            else
            {
                company = new byte[2];

                Buffer.BlockCopy(sourceROMHeader, 0x00, company, 0, 2);

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

                return companyCode;
            }
        }

        public static byte[] CalculateChecksum(byte[] sourceROM, uint offset, int intROMSize, int calcFileSize, byte romType, bool isBSROM)
        {
            uint offsetChksm = offset + 0x2C;
            ulong checksum = 0;
            uint multiplier = 1;
            byte[] byteChksm = new byte[2];

            // For clearing out checksum - checksum = 0x0000 , inverse = 0xFFFF
            byte[] clearSum = new byte[4];
            clearSum[0] = 0xFF;
            clearSum[1] = 0xFF;
            clearSum[2] = 0x00;
            clearSum[3] = 0x00;

            // Make a copy with clean checksum of the ROM for independent calculation
            byte[] noChecksumSourceROM = new byte[sourceROM.Length];
            Buffer.BlockCopy(sourceROM, 0, noChecksumSourceROM, 0, sourceROM.Length);
            Buffer.BlockCopy(clearSum, 0, noChecksumSourceROM, (int)offsetChksm, clearSum.Length);

            // For BS-X ROMs fill header with 0x00
            if (isBSROM)
            {
                byte[] clearBSHeader = new byte [48] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                       0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                       0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                Buffer.BlockCopy(clearBSHeader, 0, noChecksumSourceROM, (int)offset, clearBSHeader.Length);
            }

            // Some Hacks may have an odd size in their header, so we should fix that by taking the right value
            if (intROMSize < calcFileSize)
            {
                intROMSize = 1;

                while (intROMSize < calcFileSize)
                {
                    intROMSize *= 2;
                }
            }

            // Mirror ROM if neccessary | not working for Momotarou Dentetsu Happy and Tengai Makyou Zero / Tengai Makyou Zero - Shounen Jump no Shou (3 MByte ROMs with Special Chip + RAM + SRAM)
            if ((intROMSize > calcFileSize) && romType != 0xF5 && romType != 0xF9)
            {
                try
                {
                    // Create new ROM for mirroring
                    byte[] mirroredROM = new byte[(intROMSize * 131072)];

                    int ctr = 0;
                    int romSize1 = intROMSize;

                    // Get size of ROM #1
                    do
                    {
                        ctr = romSize1 / calcFileSize;
                        romSize1 /= 2;
                    }

                    while (ctr > 1);

                    // Get size of ROM #2 and its multiplier
                    int romSize2 = calcFileSize - romSize1;
                    int romRest = intROMSize - romSize1;
                    int romSize2Multiplicator = romRest / romSize2;
                
                    //Debugging
                    //MessageBox.Show("ROM 1: " + romSize1 + "\nROM 2: " + romSize2 + "\nMultiplier ROM 2: " + romSize2Multiplicator);

                    // Mirror ROM
                    Buffer.BlockCopy(noChecksumSourceROM, 0, mirroredROM, 0, romSize1 * 131072);
                    
                    for (int i=0; i < romSize2Multiplicator; i++)
                    {
                        Buffer.BlockCopy(noChecksumSourceROM, romSize1 * 131072, mirroredROM, (romSize1 * 131072) + (i * romSize2 * 131072), romSize2 * 131072);
                    }

                    // Put mirrored ROM into noChecksumSourceROM for calculation
                    noChecksumSourceROM = mirroredROM;

                    // Debugging
                    //File.WriteAllBytes(@"C:\Temp\Test.bin", noChecksumSourceROM);
                }

                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            // Momotarou Dentetsu Happy fix | This didn't work with expanded ROM: else if (calcFileSize == 24 && romType == 0xF5)
            else if (intROMSize == 32 && romType == 0xF5)
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

            return byteChksm;
        }
        public static byte[] CalculateInverseChecksum(byte[] calcChksm)
        {
            UInt16 calcInvChksm = 0;
            byte[] byteInvChksm = new byte[2];

            // Calculate inverse checksum (XOR with 0xFFFF)
            calcInvChksm = BitConverter.ToUInt16(calcChksm, 0);
            calcInvChksm ^= 0xFFFF;

            // Return checksum as byte[]
            byteInvChksm = BitConverter.GetBytes(calcInvChksm);

            return byteInvChksm;
        }
        public static byte[] ReadChecksum(byte[] sourceROMHeader)
        {
            byte[] checksum = new byte[2];

            Buffer.BlockCopy(sourceROMHeader, 0x2E, checksum, 0, 2);

            // Return checksum as big endian byte[]
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(checksum);
            }

            return checksum;
        }
        public static byte[] ReadInverseChecksum(byte[] sourceROMHeader)
        {
            byte[] checksum = new byte[2];

            Buffer.BlockCopy(sourceROMHeader, 0x2C, checksum, 0, 2);

            // Return checksum as big endian byte[]
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(checksum);
            }

            return checksum;
        }
        public static byte[] ReadVersion(byte[] sourceROMHeader)
        {
            byte[] version = new byte[1];

            Buffer.BlockCopy(sourceROMHeader, 0x2B, version, 0, 1);

            return version;
        }
        public static string CalculateCrc32Hash(byte[] sourceROM)
        {
            Crc32 crc32 = new Crc32();
            string hash = null;

            foreach (byte singleByte in crc32.ComputeHash(sourceROM))
            {
                hash += singleByte.ToString("X2").ToUpper();
            }

            return hash;
        }
        public static void SwapBin(byte[] sourceROM, String romSavePath, String romName)
        {
            // Make a copy of the ROM for swapping
            byte[] swappedSourceROM = new byte[sourceROM.Length];
            Buffer.BlockCopy(sourceROM, 0, swappedSourceROM, 0, sourceROM.Length);

            // List of adresses to be swapped
            List<int> swapSrcAdds = new List<int> { 65536, 131072, 262144, 524288, 393216, 196608, 786432, 589824, 458752, 851968, 720896, 917504 };
            List<int> swapDstAdds = new List<int> { 262144, 524288, 65536, 131072, 589824, 786432, 196608, 393216, 851968, 458752, 917504, 720896 };

            // Number of bytes to be swapped on each address - (64Kb [8Mbit = 64Kb x 16])
            int swapSize = 65536;

            // Do the swap thing
            int swapDstAddCtr = 0;

            foreach (int swapSrcAdd in swapSrcAdds)
            {
                Buffer.BlockCopy(sourceROM, swapSrcAdd, swappedSourceROM, swapDstAdds[swapDstAddCtr], swapSize);
                swapDstAddCtr++;
            }

            // Save swapped file
            File.WriteAllBytes(@romSavePath + @"\" + romName + "_[swapped]" + ".bin", swappedSourceROM);
            MessageBox.Show("File successfully swapped!\n\nFile saved to: '" + @romSavePath + @"\" + romName + "_[swapped]" + ".bin'\n\nIn case there was a header, it has been removed!");
        }
        public static void Deinterlave(byte[] sourceROM, byte[] sourceROMSMCHeader, int calcFileSize, String romSavePath, String romName)
        {
            byte[] ufoTitle = new byte[8];
            byte[] gdTitle = new byte[14];

            bool ufo = false;   // Super UFO
            bool gd = false;    // Game Doctor SF

            // Analyze SMC header
            if (sourceROMSMCHeader != null)
            {
                Buffer.BlockCopy(sourceROMSMCHeader, 8, ufoTitle, 0, 8);
                Buffer.BlockCopy(sourceROMSMCHeader, 0, gdTitle, 0, 14);

                if (Encoding.ASCII.GetString(ufoTitle).Equals("SUPERUFO"))
                {
                    ufo = true;
                }

                if (Encoding.ASCII.GetString(gdTitle).Equals("GAME DOCTOR SF"))
                {
                    gd = true;
                }
            }
           
            // If there is no header or header is not Super UFO or Game Doctor, ask user if he wants to proceed
            else if ((!ufo && !gd) || sourceROMSMCHeader == null)
            {
                DialogResult dialogResult = new DialogResult();
                Form2 chooseCopier = new Form2();
                dialogResult = chooseCopier.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    // Set Game Doctor SF as copier
                    gd = true;
                }

                else if (dialogResult == DialogResult.Yes)
                {
                    // Set Super UFO as copier
                    ufo = true;
                }

                else if (dialogResult == DialogResult.Cancel)
                {
                    // If user don't wants to proceed, stop this function
                    return;
                }
            }

            byte[] deinterleavedROM = new byte[sourceROM.Length];   // Empty byte array for deinterleaved ROM

            int chunkSize = 32768;  // Number of bytes for each chunk
            int chunkItems = sourceROM.Length / chunkSize;    // Number of chunks

            // In some special cases of 20, 24 or 48 MBit ROMs, deinterleaving must be done by following one of these pattern
            int[] deintpattern = null;

            if (!ufo && calcFileSize == 20)
            {
                // Array with deinterleaving pattern for 20 MBit ROMs which were NOT dumped with Super UFO
                deintpattern = new int[] { 1,   3,   5,   7,   9,  11,  13,  15,  17,  19,  21,  23,  25,  27,  29,
                                          31,  33,  35,  37,  39,  41,  43,  45,  47,  49,  51,  53,  55,  57,  59,
                                          61,  63,  65,  67,  69,  71,  73,  75,  77,  79,  64,  66,  68,  70,  72,
                                          74,  76,  78,  32,  34,  36,  38,  40,  42,  44,  46,  48,  50,  52,  54,
                                          56,  58,  60,  62,   0,   2,   4,   6,   8,  10,  12,  14,  16,  18,  20,
                                          22,  24,  26,  28,  30 };
            }

            else if (!ufo && calcFileSize == 24)
            {
                // Array with deinterleaving pattern for 24 MBit ROMs which were NOT dumped with Super UFO
                deintpattern = new int[] { 1,   3,   5,   7,   9,  11,  13,  15,  17,  19,  21,  23,  25,  27,  29,
                                          31,  33,  35,  37,  39,  41,  43,  45,  47,  49,  51,  53,  55,  57,  59,
                                          61,  63,  65,  67,  69,  71,  73,  75,  77,  79,  81,  83,  85,  87,  89,
                                          91,  93,  95,  64,  66,  68,  70,  72,  74,  76,  78,  80,  82,  84,  86,
                                          88,  90,  92,  94,   0,   2,   4,   6,   8,  10,  12,  14,  16,  18,  20,
                                          22,  24,  26,  28,  30,  32,  34,  36,  38,  40,  42,  44,  46,  48,  50,
                                          52,  54,  56,  58,  60,  62 };
            }

            else if (gd && calcFileSize == 48)
            {
                // Array with deinterleaving pattern for 48 MBit ROMs which were dumped WITH Super Game Doctor
                deintpattern = new int[] { 129, 131, 133, 135, 137, 139, 141, 143, 145, 147, 149, 151, 153, 155, 157,
                                           159, 161, 163, 165, 167, 169, 171, 173, 175, 177, 179, 181, 183, 185, 187,
                                           189, 191, 128, 130, 132, 134, 136, 138, 140, 142, 144, 146, 148, 150, 152,
                                           154, 156, 158, 160, 162, 164, 166, 168, 170, 172, 174, 176, 178, 180, 182,
                                           184, 186, 188, 190,   1,   3,   5,   7,   9,  11,  13,  15,  17,  19,  21,
                                            23,  25,  27,  29,  31,  33,  35,  37,  39,  41,  43,  45,  47,  49,  51,
                                            53,  55,  57,  59,  61,  63,  65,  67,  69,  71,  73,  75,  77,  79,  81,
                                            83,  85,  87,  89,  91,  93,  95,  97,  99, 101, 103, 105, 107, 109, 111,
                                           113, 115, 117, 119, 121, 123, 125, 127,   0,   2,   4,   6,   8,  10,  12,
                                            14,  16,  18,  20,  22,  24,  26,  28,  30,  32,  34,  36,  38,  40,  42,
                                            44,  46,  48,  50,  52,  54,  56,  58,  60,  62,  64,  66,  68,  70,  72,
                                            74,  76,  78,  80,  82,  84,  86,  88,  90,  92,  94,  96,  98, 100, 102,
                                           104, 106, 108, 110, 112, 114, 116, 118, 120, 122, 124, 126 };
            }

            if (deintpattern != null)
            {
                for (int chunkCtr = 0; chunkCtr < chunkItems; chunkCtr++)
                {
                    Buffer.BlockCopy(sourceROM, chunkCtr * chunkSize, deinterleavedROM, deintpattern[chunkCtr] * chunkSize, chunkSize);
                }
            }

            // If no special case is detectetd, take the standard deinterleaving algorithm
            else
            {
                int even = 0;
                int odd = 1;

                for (int chunkCtr = 0; chunkCtr < chunkItems; chunkCtr++)
                {
                    int sourcePos = chunkCtr * chunkSize;
                    int destPos = 0;

                    if ((chunkCtr * chunkSize) < (calcFileSize * 131072) / 2)
                    {
                        destPos = odd * chunkSize;

                        Buffer.BlockCopy(sourceROM, sourcePos, deinterleavedROM, destPos, chunkSize);

                        odd += 2;
                    }

                    else
                    {
                        destPos = even * chunkSize;

                        Buffer.BlockCopy(sourceROM, sourcePos, deinterleavedROM, destPos, chunkSize);

                        even += 2;
                    }
                }
            }

            // Save deinterleaved file
            string identifier = "_[deinterleaved]";

            if (gd)
            {
                identifier = "_[GD_deinterleaved]";
            }

            else if (ufo)
            {
                identifier = "_[UFO_deinterleaved]";
            }

            File.WriteAllBytes(@romSavePath + @"\" + romName + identifier + ".sfc", deinterleavedROM);
            MessageBox.Show("File successfully deinterleaved!\n\nFile saved to: '" + @romSavePath + @"\" + romName + identifier + ".sfc'\n\nIn case there was a header, it has been removed!");
        }
        public static bool UnlockRegion(byte[] sourceROM, byte[] sourceROMSMCHeader, bool unlock, string romSavePath, string romName, string region)
        {
            List<byte[]> lockingCodes = new List<byte[]>();
            List<byte[]> unlockingCodes = new List<byte[]>();
            List<bool[]> lockingCodePattern = new List<bool[]>();

            byte[] regionFixedSourceROM = null;

            if(unlock)
            {
                regionFixedSourceROM = new byte[sourceROM.Length];
                Buffer.BlockCopy(sourceROM, 0, regionFixedSourceROM, 0, sourceROM.Length);
            }


            // Load bad codes into list
            if (region.Equals("PAL"))
            {
                // Bad codes in PAL games
                byte[] lockingCode01 = { 0xad, 0x3f, 0x21, 0x29, 0x10, 0x00, 0xd0 }; lockingCodes.Add(lockingCode01);
                byte[] lockingCode02 = { 0xad, 0x3f, 0x21, 0x89, 0x10, 0x00, 0xd0 }; lockingCodes.Add(lockingCode02);
                byte[] lockingCode03 = { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xcf, 0xbd, 0xff, 0x00, 0xf0 }; lockingCodes.Add(lockingCode03);
                byte[] lockingCode04 = { 0xad, 0x3f, 0x21, 0x89, 0x10, 0xd0 }; lockingCodes.Add(lockingCode04);
                byte[] lockingCode05 = { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0xd0 }; lockingCodes.Add(lockingCode05);
                byte[] lockingCode06 = { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0x00, 0xd0 }; lockingCodes.Add(lockingCode06);
                byte[] lockingCode07 = { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x00, 0xc9, 0x00, 0xf0 }; lockingCodes.Add(lockingCode07);
                byte[] lockingCode08 = { 0xa2, 0x18, 0x01, 0xbd, 0x27, 0x20, 0x89, 0x10, 0x00, 0xf0, 0x01 }; lockingCodes.Add(lockingCode08);
                byte[] lockingCode09 = { 0xaf, 0x3f, 0x21, 0x00, 0x89, 0x10, 0x00 }; lockingCodes.Add(lockingCode09);                           // Asterix & Obelix
                byte[] lockingCode10 = { 0xaf, 0x3f, 0x21, 0x00, 0xc2, 0x20, 0x29, 0x10, 0x00 }; lockingCodes.Add(lockingCode10);               // 90 Minutes - European Prime Goal
                byte[] lockingCode11 = { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0xc2 }; lockingCodes.Add(lockingCode11);                           // California Games 2
                byte[] lockingCode12 = { 0xad, 0x3f, 0x21, 0x29, 0x10, 0x85, 0x62, 0xc2 }; lockingCodes.Add(lockingCode12);                     // Dirt Racer

                // Good codes for PAL games
                byte[] unlockingCode01 = { 0xad, 0x3f, 0x21, 0x29, 0x10, 0x00, 0x80 }; unlockingCodes.Add(unlockingCode01);
                byte[] unlockingCode02 = { 0xa9, 0x10, 0x00, 0x89, 0x10, 0x00, 0xd0 }; unlockingCodes.Add(unlockingCode02);
                byte[] unlockingCode03 = { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xcf, 0xbd, 0xff, 0x00, 0x80 }; unlockingCodes.Add(unlockingCode03);
                byte[] unlockingCode04 = { 0xad, 0x3f, 0x21, 0x89, 0x10, 0x80 }; unlockingCodes.Add(unlockingCode04);
                byte[] unlockingCode05 = { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0x80 }; unlockingCodes.Add(unlockingCode05);
                byte[] unlockingCode06 = { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0x00, 0xea, 0xea }; unlockingCodes.Add(unlockingCode06);
                byte[] unlockingCode07 = { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x00, 0xc9, 0x00, 0x80 }; unlockingCodes.Add(unlockingCode07);
                byte[] unlockingCode08 = { 0xa2, 0x18, 0x01, 0xbd, 0x27, 0x20, 0x89, 0x10, 0x00, 0xea, 0xea }; unlockingCodes.Add(unlockingCode08);
                byte[] unlockingCode09 = { 0xaf, 0x3f, 0x21, 0x00, 0xA9, 0x10, 0x00 }; unlockingCodes.Add(unlockingCode09);
                byte[] unlockingCode10 = { 0xaf, 0x3f, 0x21, 0x00, 0xc2, 0x20, 0xa9, 0x10, 0x00 }; unlockingCodes.Add(unlockingCode10);
                byte[] unlockingCode11 = { 0xaf, 0x3f, 0x21, 0x00, 0xa9, 0x10, 0xc2 }; unlockingCodes.Add(unlockingCode11);
                byte[] unlockingCode12 = { 0xad, 0x3f, 0x21, 0xa9, 0x10, 0x85, 0x62, 0xc2 }; unlockingCodes.Add(unlockingCode12);

                // Pattern for PAL games
                bool[] lockingCodePattern01 = { false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern01);
                bool[] lockingCodePattern02 = { false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern02);
                bool[] lockingCodePattern03 = { false, false, false, false, false, false, false, false, true, false }; lockingCodePattern.Add(lockingCodePattern03);
                bool[] lockingCodePattern04 = { false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern04);
                bool[] lockingCodePattern05 = { false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern05);
                bool[] lockingCodePattern06 = { false, false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern06);
                bool[] lockingCodePattern07 = { false, false, false, false, false, true, false, true, false }; lockingCodePattern.Add(lockingCodePattern07);
                bool[] lockingCodePattern08 = { false, false, false, false, false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern08);
                bool[] lockingCodePattern09 = { false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern09);
                bool[] lockingCodePattern10 = { false, false, false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern10);
                bool[] lockingCodePattern11 = { false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern11);
                bool[] lockingCodePattern12 = { false, false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern12);
            }

            else
            {
                // Bad codes in NTSC games
                byte[] lockingCode20 = { 0x3f, 0x21, 0x29, 0x10, 0xf0 }; lockingCodes.Add(lockingCode20);
                byte[] lockingCode21 = { 0x3f, 0x21, 0x89, 0x10, 0xf0 }; lockingCodes.Add(lockingCode21);
                byte[] lockingCode22 = { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xd0 }; lockingCodes.Add(lockingCode22);
                byte[] lockingCode23 = { 0xad, 0x3f, 0x21, 0x89, 0x10, 0xd0 }; lockingCodes.Add(lockingCode23);
                byte[] lockingCode24 = { 0x3f, 0x21, 0x29, 0x10, 0x00, 0xf0 }; lockingCodes.Add(lockingCode24);
                byte[] lockingCode25 = { 0x3f, 0x21, 0x89, 0x10, 0x00, 0xf0 }; lockingCodes.Add(lockingCode25);
                byte[] lockingCode26 = { 0x3f, 0x21, 0x29, 0x10, 0x00, 0xd0 }; lockingCodes.Add(lockingCode26);
                byte[] lockingCode27 = { 0x3f, 0x21, 0x89, 0x10, 0x00, 0xd0 }; lockingCodes.Add(lockingCode27);
                byte[] lockingCode28 = { 0x3f, 0x21, 0x89, 0x10, 0xc2, 0x00, 0xf0 }; lockingCodes.Add(lockingCode28);
                byte[] lockingCode29 = { 0x3f, 0x21, 0x89, 0x10, 0xc2, 0x00, 0xd0 }; lockingCodes.Add(lockingCode29);
                byte[] lockingCode30 = { 0x3f, 0x21, 0x29, 0x10, 0xc9, 0x10, 0xf0 }; lockingCodes.Add(lockingCode30);
                byte[] lockingCode31 = { 0x3f, 0x21, 0x89, 0x10, 0xc9, 0x10, 0xf0 }; lockingCodes.Add(lockingCode31);
                byte[] lockingCode32 = { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xc9, 0x00, 0xf0 }; lockingCodes.Add(lockingCode32);
                byte[] lockingCode33 = { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xc9, 0x00, 0xd0 }; lockingCodes.Add(lockingCode33);
                byte[] lockingCode34 = { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xc9, 0x10, 0xd0 }; lockingCodes.Add(lockingCode34);
                byte[] lockingCode35 = { 0x3f, 0x21, 0x29, 0x10, 0xcf, 0x00, 0x00, 0x80, 0xf0 }; lockingCodes.Add(lockingCode35);
                byte[] lockingCode36 = { 0xad, 0x3f, 0x21, 0x8d, 0x00, 0x00, 0x29, 0x10, 0x8d }; lockingCodes.Add(lockingCode36);
                byte[] lockingCode37 = { 0x3f, 0x21, 0x00, 0x29, 0x10, 0xf0 }; lockingCodes.Add(lockingCode37);
                byte[] lockingCode38 = { 0x3f, 0x21, 0x00, 0x89, 0x10, 0xf0 }; lockingCodes.Add(lockingCode38);
                byte[] lockingCode39 = { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0xd0 }; lockingCodes.Add(lockingCode39);
                byte[] lockingCode40 = { 0xaf, 0x3f, 0x21, 0x00, 0x89, 0x10, 0xd0 }; lockingCodes.Add(lockingCode40);
                byte[] lockingCode41 = { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0x00, 0xf0 }; lockingCodes.Add(lockingCode41);
                byte[] lockingCode42 = { 0xaf, 0x3f, 0x21, 0x00, 0x89, 0x10, 0x00, 0xf0 }; lockingCodes.Add(lockingCode42);
                byte[] lockingCode43 = { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x00, 0xc9, 0x00, 0xf0 }; lockingCodes.Add(lockingCode43);
                byte[] lockingCode44 = { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0x80, 0x2d, 0x00, 0x1b }; lockingCodes.Add(lockingCode44);
                byte[] lockingCode45 = { 0x3f, 0x21, 0x00, 0x89, 0x10, 0xc2, 0x00, 0xf0 }; lockingCodes.Add(lockingCode45);
                byte[] lockingCode46 = { 0xaf, 0x3f, 0x21, 0x00, 0x00, 0x00, 0x29, 0x10, 0x00, 0xd0 }; lockingCodes.Add(lockingCode46);
                byte[] lockingCode47 = { 0x3f, 0x21, 0xc2, 0x00, 0x29, 0x10, 0x00, 0xf0 }; lockingCodes.Add(lockingCode47);
                byte[] lockingCode48 = { 0x3f, 0x21, 0xc2, 0x00, 0x29, 0x10, 0x00, 0xd0 }; lockingCodes.Add(lockingCode48);
                byte[] lockingCode49 = { 0xaf, 0x3f, 0x21, 0xea, 0x89, 0x10, 0x00, 0xd0 }; lockingCodes.Add(lockingCode49);
                byte[] lockingCode50 = { 0xa2, 0x18, 0x01, 0xbd, 0x27, 0x20, 0x89, 0x10, 0x00, 0xd0, 0x01 }; lockingCodes.Add(lockingCode50);
                byte[] lockingCode51 = { 0x29, 0x10, 0x00, 0xa2, 0x00, 0x00, 0xc9, 0x10, 0x00, 0xd0 }; lockingCodes.Add(lockingCode51);
                byte[] lockingCode52 = { 0xad, 0x3f, 0x21, 0x29, 0x0f }; lockingCodes.Add(lockingCode52);       // Dezaemon - Kaite Tsukutte Asoberu
                byte[] lockingCode53 = { 0xad, 0x39, 0xb5, 0xd0, 0x1a, 0x22 }; lockingCodes.Add(lockingCode53);     // Earthbound
                byte[] lockingCode54 = { 0x1a, 0x8f, 0xf0, 0x7f, 0x31, 0xcf, 0xf0 }; lockingCodes.Add(lockingCode54);   // Earthbound
                byte[] lockingCode55 = { 0x1a, 0x8f, 0xf0, 0x7f, 0x32, 0xcf, 0xf0 }; lockingCodes.Add(lockingCode55);   // Earthbound
                byte[] lockingCode56 = { 0x1a, 0x8f, 0xf0, 0x7f, 0x31, 0xcf, 0xf0 }; lockingCodes.Add(lockingCode56);   // Earthbound
                byte[] lockingCode57 = { 0xa1, 0xc0, 0xca, 0x10, 0xf8, 0x38, 0xef, 0xf2, 0xfd, 0xc3, 0xf0 }; lockingCodes.Add(lockingCode57);     // Earthbound
                byte[] lockingCode58 = { 0xa1, 0xc0, 0xca, 0x10, 0xf8, 0xef, 0x38, 0xf2, 0xfd, 0xc3, 0xf0 }; lockingCodes.Add(lockingCode58);   // Earthbound
                byte[] lockingCode59 = { 0xa1, 0xc0, 0xca, 0x10, 0xf8, 0x38, 0xef, 0xef, 0xff, 0xc1 }; lockingCodes.Add(lockingCode59);     // Earthbound

                // Good codes for NTSC games
                byte[] unlockingCode20 = { 0x3f, 0x21, 0x29, 0x10, 0x80 }; unlockingCodes.Add(unlockingCode20);
                byte[] unlockingCode21 = { 0x3f, 0x21, 0x89, 0x10, 0x80 }; unlockingCodes.Add(unlockingCode21);
                byte[] unlockingCode22 = { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xea, 0xea }; unlockingCodes.Add(unlockingCode22);
                byte[] unlockingCode23 = { 0xad, 0x3f, 0x21, 0x89, 0x10, 0xea, 0xea }; unlockingCodes.Add(unlockingCode23);         // Could also be 0xad, 0x3f, 0x21, 0x89, 0x10, 0x80 but uCON64 uses (ea ea)
                byte[] unlockingCode24 = { 0x3f, 0x21, 0x29, 0x10, 0x00, 0x80 }; unlockingCodes.Add(unlockingCode24);
                byte[] unlockingCode25 = { 0x3f, 0x21, 0x89, 0x10, 0x00, 0x80 }; unlockingCodes.Add(unlockingCode25);
                byte[] unlockingCode26 = { 0x3f, 0x21, 0x29, 0x10, 0x00, 0xea, 0xea }; unlockingCodes.Add(unlockingCode26);
                byte[] unlockingCode27 = { 0x3f, 0x21, 0x89, 0x10, 0x00, 0xea, 0xea }; unlockingCodes.Add(unlockingCode27);
                byte[] unlockingCode28 = { 0x3f, 0x21, 0x89, 0x10, 0xc2, 0x00, 0x80 }; unlockingCodes.Add(unlockingCode28);
                byte[] unlockingCode29 = { 0x3f, 0x21, 0x89, 0x10, 0xc2, 0x00, 0xea, 0xea }; unlockingCodes.Add(unlockingCode29);
                byte[] unlockingCode30 = { 0x3f, 0x21, 0x29, 0x10, 0xc9, 0x10, 0x80 }; unlockingCodes.Add(unlockingCode30);
                byte[] unlockingCode31 = { 0x3f, 0x21, 0x89, 0x10, 0xc9, 0x10, 0x80 }; unlockingCodes.Add(unlockingCode31);
                byte[] unlockingCode32 = { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xc9, 0x00, 0xea, 0xea }; unlockingCodes.Add(unlockingCode32);
                byte[] unlockingCode33 = { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xc9, 0x00, 0x80 }; unlockingCodes.Add(unlockingCode33);
                byte[] unlockingCode34 = { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xc9, 0x10, 0xea, 0xea }; unlockingCodes.Add(unlockingCode34);
                byte[] unlockingCode35 = { 0x3f, 0x21, 0x29, 0x10, 0xcf, 0x00, 0x00, 0x80, 0x80 }; unlockingCodes.Add(unlockingCode35);
                byte[] unlockingCode36 = { 0xad, 0x3f, 0x21, 0x8d, 0x00, 0x00, 0x29, 0x00, 0x8d }; unlockingCodes.Add(unlockingCode36);
                byte[] unlockingCode37 = { 0x3f, 0x21, 0x00, 0x29, 0x10, 0x80 }; unlockingCodes.Add(unlockingCode37);
                byte[] unlockingCode38 = { 0x3f, 0x21, 0x00, 0x89, 0x10, 0x80 }; unlockingCodes.Add(unlockingCode38);
                byte[] unlockingCode39 = { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0xea, 0xea }; unlockingCodes.Add(unlockingCode39);
                byte[] unlockingCode40 = { 0xaf, 0x3f, 0x21, 0x00, 0x89, 0x10, 0xea, 0xea }; unlockingCodes.Add(unlockingCode40);
                byte[] unlockingCode41 = { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0x00, 0x80 }; unlockingCodes.Add(unlockingCode41);
                byte[] unlockingCode42 = { 0xaf, 0x3f, 0x21, 0x00, 0x89, 0x10, 0x00, 0x80 }; unlockingCodes.Add(unlockingCode42);
                byte[] unlockingCode43 = { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x00, 0xc9, 0x00, 0x80 }; unlockingCodes.Add(unlockingCode43);
                byte[] unlockingCode44 = { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x00, 0x80, 0x2d, 0x00, 0x1b }; unlockingCodes.Add(unlockingCode44);
                byte[] unlockingCode45 = { 0x3f, 0x21, 0x00, 0x89, 0x10, 0xc2, 0x00, 0x80 }; unlockingCodes.Add(unlockingCode45);
                byte[] unlockingCode46 = { 0xaf, 0x3f, 0x21, 0x00, 0x00, 0x00, 0x29, 0x10, 0x00, 0xea, 0xea }; unlockingCodes.Add(unlockingCode46);
                byte[] unlockingCode47 = { 0x3f, 0x21, 0xc2, 0x00, 0x29, 0x10, 0x00, 0x80 }; unlockingCodes.Add(unlockingCode47);
                byte[] unlockingCode48 = { 0x3f, 0x21, 0xc2, 0x00, 0x29, 0x10, 0x00, 0xea, 0xea }; unlockingCodes.Add(unlockingCode48);
                byte[] unlockingCode49 = { 0xa9, 0x00, 0x00, 0xea, 0x89, 0x10, 0x00, 0xd0 }; unlockingCodes.Add(unlockingCode49);
                byte[] unlockingCode50 = { 0xa2, 0x18, 0x01, 0xbd, 0x27, 0x20, 0x89, 0x10, 0x00, 0xea, 0xea }; unlockingCodes.Add(unlockingCode50);
                byte[] unlockingCode51 = { 0x29, 0x10, 0x00, 0xa2, 0x00, 0x00, 0xc9, 0x10, 0x00, 0x80 }; unlockingCodes.Add(unlockingCode51);
                byte[] unlockingCode52 = { 0xad, 0x3f, 0x21, 0x29, 0xff }; unlockingCodes.Add(unlockingCode52);       // Dezaemon - Kaite Tsukutte Asoberu
                byte[] unlockingCode53 = { 0xad, 0x39, 0xb5, 0xea, 0xea, 0x22 }; unlockingCodes.Add(unlockingCode53);       // Earthbound
                byte[] unlockingCode54 = { 0x1a, 0x8f, 0xf0, 0x7f, 0x30, 0xcf, 0xf0 }; unlockingCodes.Add(unlockingCode54);     // Earthbound
                byte[] unlockingCode55 = { 0x1a, 0x8f, 0xf0, 0x7f, 0x30, 0xcf, 0xf0 }; unlockingCodes.Add(unlockingCode55);     // Earthbound
                byte[] unlockingCode56 = { 0x1a, 0x8f, 0xf0, 0x7f, 0x30, 0xcf, 0xf0 }; unlockingCodes.Add(unlockingCode56);     // Earthbound
                byte[] unlockingCode57 = { 0xa1, 0xc0, 0xca, 0x10, 0xf8, 0x38, 0xea, 0xa9, 0x00, 0x00, 0x80 }; unlockingCodes.Add(unlockingCode57);       // Earthbound
                byte[] unlockingCode58 = { 0xa1, 0xc0, 0xca, 0x10, 0xf8, 0x38, 0xea, 0xa9, 0x00, 0x00, 0x80 }; unlockingCodes.Add(unlockingCode58);     // Earthbound
                byte[] unlockingCode59 = { 0xa1, 0xc0, 0xca, 0x10, 0xf8, 0x38, 0xea, 0xa9, 0x00, 0x00 }; unlockingCodes.Add(unlockingCode59);       // Earthbound


                // Pattern for NTSC games
                bool[] lockingCodePattern20 = { false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern20);
                bool[] lockingCodePattern21 = { false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern21);
                bool[] lockingCodePattern22 = { false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern22);
                bool[] lockingCodePattern23 = { false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern23);
                bool[] lockingCodePattern24 = { false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern24);
                bool[] lockingCodePattern25 = { false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern25);
                bool[] lockingCodePattern26 = { false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern26);
                bool[] lockingCodePattern27 = { false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern27);
                bool[] lockingCodePattern28 = { false, false, false, false, false, true, false }; lockingCodePattern.Add(lockingCodePattern28);
                bool[] lockingCodePattern29 = { false, false, false, false, false, true, false }; lockingCodePattern.Add(lockingCodePattern29);
                bool[] lockingCodePattern30 = { false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern30);
                bool[] lockingCodePattern31 = { false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern31);
                bool[] lockingCodePattern32 = { false, false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern32);
                bool[] lockingCodePattern33 = { false, false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern33);
                bool[] lockingCodePattern34 = { false, false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern34);
                bool[] lockingCodePattern35 = { false, false, false, false, false, true, true, false, false }; lockingCodePattern.Add(lockingCodePattern35);
                bool[] lockingCodePattern36 = { false, false, false, false, true, true, false, false, false }; lockingCodePattern.Add(lockingCodePattern36);
                bool[] lockingCodePattern37 = { false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern37);
                bool[] lockingCodePattern38 = { false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern38);
                bool[] lockingCodePattern39 = { false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern39);
                bool[] lockingCodePattern40 = { false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern40);
                bool[] lockingCodePattern41 = { false, false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern41);
                bool[] lockingCodePattern42 = { false, false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern42);
                bool[] lockingCodePattern43 = { false, false, false, false, false, true, false, true, false }; lockingCodePattern.Add(lockingCodePattern43);
                bool[] lockingCodePattern44 = { false, false, false, false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern44);
                bool[] lockingCodePattern45 = { false, false, false, false, false, false, true, false }; lockingCodePattern.Add(lockingCodePattern45);
                bool[] lockingCodePattern46 = { false, false, false, false, true, true, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern46);
                bool[] lockingCodePattern47 = { false, false, false, true, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern47);
                bool[] lockingCodePattern48 = { false, false, false, true, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern48);
                bool[] lockingCodePattern49 = { false, false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern49);
                bool[] lockingCodePattern50 = { false, false, false, false, false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern50);
                bool[] lockingCodePattern51 = { false, false, false, false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern51);
                bool[] lockingCodePattern52 = { false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern52);
                bool[] lockingCodePattern53 = { false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern53);
                bool[] lockingCodePattern54 = { false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern54);
                bool[] lockingCodePattern55 = { false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern55);
                bool[] lockingCodePattern56 = { false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern56);
                bool[] lockingCodePattern57 = { false, false, false, false, false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern57);
                bool[] lockingCodePattern58 = { false, false, false, false, false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern58);
                bool[] lockingCodePattern59 = { false, false, false, false, false, false, false, false, false, false }; lockingCodePattern.Add(lockingCodePattern59);
            }

            bool foundBadCode = false;
            int index = 0;

            foreach (byte[] lockingCode in lockingCodes)
            {
                byte[] unlockingCode = unlockingCodes[index];
                bool[] lockingPattern = lockingCodePattern[index];

                int offset = -1;
                int c = sourceROM.Length - lockingCode.Length + 1;
                int j;

                for (int i = 0; i < c; i++)
                {
                    if (sourceROM[i] != lockingCode[0])
                    {
                        continue;
                    }

                    for (j = lockingCode.Length - 1; j >= 1 && sourceROM[i + j] == lockingCode[j] || lockingPattern[j]; j--)
                    {
                        if (lockingPattern[j])
                        {
                            unlockingCode[j] = sourceROM[i + j];
                        }
                    }
                    
                    if (j == 0)
                    {
                        offset = i;
                        foundBadCode = true;

                        // If bad code was found replace it with good code
                        if (foundBadCode && unlock)
                        {
                            Buffer.BlockCopy(unlockingCode, 0, regionFixedSourceROM, offset, unlockingCode.Length);
                        }

                        // If only check is needed, tell that bad code was found in ROM and leave
                        else if (foundBadCode && !unlock)
                        {
                            return true;
                        }
                    }
                }

                index++;
            }

            // Save file if found bad codes and unlocking is enabled
            if (foundBadCode && unlock)
            {
                // Save region free file with header
                if (sourceROMSMCHeader != null)
                {
                    byte[] regionFixedHeaderedROM = new byte[sourceROMSMCHeader.Length + regionFixedSourceROM.Length];

                    Buffer.BlockCopy(sourceROMSMCHeader, 0, regionFixedHeaderedROM, 0, sourceROMSMCHeader.Length);
                    Buffer.BlockCopy(regionFixedSourceROM, 0, regionFixedHeaderedROM, sourceROMSMCHeader.Length, regionFixedSourceROM.Length);

                    File.WriteAllBytes(@romSavePath + @"\" + romName + "_[region_free]" + ".sfc", regionFixedHeaderedROM);
                }

                else
                {
                    // Save region free file without header
                    File.WriteAllBytes(@romSavePath + @"\" + romName + "_[region_free]" + ".sfc", regionFixedSourceROM);
                }

                MessageBox.Show("Region lock successfully removed!\n\nFile saved to: '" + @romSavePath + @"\" + romName + "_[region_free]" + ".sfc'");

                return true;
            }

            return false;
        }
    }
}