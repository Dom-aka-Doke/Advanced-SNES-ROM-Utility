using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        public void AddHeader()
        {
            // Create empty header
            SourceROMSMCHeader = new byte[512];

            foreach (byte singleByte in SourceROMSMCHeader)
            {
                SourceROMSMCHeader[singleByte] = 0x00;
            }

            Initialize();
        }

        public void RemoveHeader()
        {
            // Remove existing header
            SourceROMSMCHeader = null;

            Initialize();
        }

        public void FixChecksum()
        {
            uint offset = UIntROMHeaderOffset + 0x2C;
            byte[] newChksm = new byte[2];
            byte[] newInvChksm = new byte[2];
            byte[] newChksmSequence = new byte[4];

            newChksm = ByteArrayCalcChecksum;
            newInvChksm = ByteArrayCalcInvChecksum;

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

            Buffer.BlockCopy(newChksmSequence, 0, SourceROM, (int)offset, newChksmSequence.Length);

            if (UIntROMHeaderOffset == 0x407FB0 || UIntROMHeaderOffset == 0x40FFB0)
            {
                Buffer.BlockCopy(newChksmSequence, 0, SourceROM, (int)offset - 0x400000, newChksmSequence.Length);
            }

            Initialize();
        }

        public void FixInternalROMSize()
        {
            if (IntROMSize < IntCalcFileSize)
            {
                IntROMSize = 1;

                if (!IsBSROM)
                {
                    byte[] byteArrayROMSizeValue = { 0x07 };

                    while (IntROMSize < IntCalcFileSize)
                    {
                        IntROMSize *= 2;
                        byteArrayROMSizeValue[0]++;
                    }

                    Buffer.BlockCopy(byteArrayROMSizeValue, 0, SourceROM, (int)UIntROMHeaderOffset + 0x27, 1);

                    if (UIntROMHeaderOffset == 0x407FB0 || UIntROMHeaderOffset == 0x40FFB0)
                    {
                        Buffer.BlockCopy(byteArrayROMSizeValue, 0, SourceROM, (int)(UIntROMHeaderOffset + 0x27 - 0x400000), 1);
                    }
                }

                else
                {
                    IntROMSize = IntCalcFileSize;

                    uint size = 1;
                    int sizeCtr = IntROMSize;

                    while (sizeCtr > 1)
                    {
                        size <<= 1;
                        size |= 1;
                        sizeCtr--;
                    }

                    byte[] byteArrayROMSizeValue = BitConverter.GetBytes(size);
                    Buffer.BlockCopy(byteArrayROMSizeValue, 0, SourceROM, (int)UIntROMHeaderOffset + 0x20, 4);
                }

                Initialize();
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

            Buffer.BlockCopy(byteArrayTitle, 0, SourceROM, (int)UIntROMHeaderOffset + 0x10, byteArrayTitle.Length);

            if (UIntROMHeaderOffset == 0x407FB0 || UIntROMHeaderOffset == 0x40FFB0)
            {
                Buffer.BlockCopy(byteArrayTitle, 0, SourceROM, (int)UIntROMHeaderOffset + 0x10 - 0x400000, byteArrayTitle.Length);
            }

            Initialize();
        }

        public void SetVersion(byte newVersion)
        {
            byte[] byteArrayVersion = { newVersion };
            Buffer.BlockCopy(byteArrayVersion, 0, SourceROM, (int)UIntROMHeaderOffset + 0x2B, 1);

            if (UIntROMHeaderOffset == 0x407FB0 || UIntROMHeaderOffset == 0x40FFB0)
            {
                Buffer.BlockCopy(byteArrayVersion, 0, SourceROM, (int)UIntROMHeaderOffset + 0x2B - 0x400000, 1);
            }

            Initialize();
        }

        public void SetCountryRegion(byte newCountryRegion)
        {
            byte[] byteArrayCountryRegion = { newCountryRegion };
            Buffer.BlockCopy(byteArrayCountryRegion, 0, SourceROM, (int)UIntROMHeaderOffset + 0x29, 1);

            if (UIntROMHeaderOffset == 0x407FB0 || UIntROMHeaderOffset == 0x40FFB0)
            {
                Buffer.BlockCopy(byteArrayCountryRegion, 0, SourceROM, (int)UIntROMHeaderOffset + 0x29 - 0x400000, 1);
            }

            Initialize();
        }

        public void SwapBin()
        {
            // Check if ROM is multiple of 8 MBit, otherwise swapping is not possible
            if (SourceROM.Length % 1048576 == 0)
            {
                int romChunks = SourceROM.Length / 1048576;

                if (romChunks > 1)
                {
                    // Define size for single ROM file (8 Mbit)
                    int chunkSize = 1048576;

                    for (int index = 0; index < romChunks; index++)
                    {
                        string romChunkName = ROMName + "_[" + index + "]";
                        byte[] sourceROMChunk = new byte[chunkSize];

                        Buffer.BlockCopy(SourceROM, index * chunkSize, sourceROMChunk, 0, chunkSize);

                        SwapBinChunk(sourceROMChunk, romChunkName);
                    }
                }

                else
                {
                    SwapBinChunk(SourceROM, ROMName);
                }
            }

            else
            {
                return;
            }
        }

        private void SwapBinChunk(byte[] sourceROM, string romName)
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
            File.WriteAllBytes(ROMFolder + @"\" + romName + "_[swapped]" + ".bin", swappedSourceROM);
        }

        public void Deinterlave()
        {
            byte[] ufoTitle = new byte[8];
            byte[] gdTitle = new byte[14];

            bool ufo = false;   // Super UFO
            bool gd = false;    // Game Doctor SF

            // Analyze SMC header
            if (SourceROMSMCHeader != null)
            {
                Buffer.BlockCopy(SourceROMSMCHeader, 8, ufoTitle, 0, 8);
                Buffer.BlockCopy(SourceROMSMCHeader, 0, gdTitle, 0, 14);

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
            else if ((!ufo && !gd) || SourceROMSMCHeader == null)
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

            byte[] deinterleavedROM = new byte[SourceROM.Length];   // Empty byte array for deinterleaved ROM

            int chunkSize = 32768;  // Number of bytes for each chunk
            int chunkItems = SourceROM.Length / chunkSize;    // Number of chunks

            // In some special cases of 20, 24 or 48 MBit ROMs, deinterleaving must be done by following one of these pattern
            int[] deintpattern = null;

            if (!ufo && IntCalcFileSize == 20)
            {
                // Array with deinterleaving pattern for 20 MBit ROMs which were NOT dumped with Super UFO
                deintpattern = new int[] { 1,   3,   5,   7,   9,  11,  13,  15,  17,  19,  21,  23,  25,  27,  29,
                                          31,  33,  35,  37,  39,  41,  43,  45,  47,  49,  51,  53,  55,  57,  59,
                                          61,  63,  65,  67,  69,  71,  73,  75,  77,  79,  64,  66,  68,  70,  72,
                                          74,  76,  78,  32,  34,  36,  38,  40,  42,  44,  46,  48,  50,  52,  54,
                                          56,  58,  60,  62,   0,   2,   4,   6,   8,  10,  12,  14,  16,  18,  20,
                                          22,  24,  26,  28,  30 };
            }

            else if (!ufo && IntCalcFileSize == 24)
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

            else if (gd && IntCalcFileSize == 48)
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
                    Buffer.BlockCopy(SourceROM, chunkCtr * chunkSize, deinterleavedROM, deintpattern[chunkCtr] * chunkSize, chunkSize);
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

                    if ((chunkCtr * chunkSize) < (IntCalcFileSize * 131072) / 2)
                    {
                        destPos = odd * chunkSize;

                        Buffer.BlockCopy(SourceROM, sourcePos, deinterleavedROM, destPos, chunkSize);

                        odd += 2;
                    }

                    else
                    {
                        destPos = even * chunkSize;

                        Buffer.BlockCopy(SourceROM, sourcePos, deinterleavedROM, destPos, chunkSize);

                        even += 2;
                    }
                }
            }

            SourceROM = deinterleavedROM;
            Initialize();
        }

        public bool UnlockRegion(bool unlock)
        {
            List<byte[]> lockingCodes = new List<byte[]>();
            List<byte[]> unlockingCodes = new List<byte[]>();
            List<bool[]> lockingCodePattern = new List<bool[]>();

            // Load bad codes into list
            if (StringRegion.Equals("PAL"))
            {
                // Bad codes in PAL games
                lockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x29, 0x10, 0x00, 0xd0 });
                lockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x89, 0x10, 0x00, 0xd0 });
                lockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xcf, 0xbd, 0xff, 0x00, 0xf0 });
                lockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x89, 0x10, 0xd0 });
                lockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0xd0 });
                lockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0x00, 0xd0 });
                lockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x00, 0xc9, 0x00, 0xf0 });
                lockingCodes.Add(new byte[] { 0xa2, 0x18, 0x01, 0xbd, 0x27, 0x20, 0x89, 0x10, 0x00, 0xf0, 0x01 });
                lockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x89, 0x10, 0x00 });                           // Asterix & Obelix
                lockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0xc2, 0x20, 0x29, 0x10, 0x00 });               // 90 Minutes - European Prime Goal
                lockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0xc2 });                           // California Games 2
                lockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x29, 0x10, 0x85, 0x62, 0xc2 });                     // Dirt Racer

                // Good codes for PAL games
                unlockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x29, 0x10, 0x00, 0x80 });
                unlockingCodes.Add(new byte[] { 0xa9, 0x10, 0x00, 0x89, 0x10, 0x00, 0xd0 });
                unlockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xcf, 0xbd, 0xff, 0x00, 0x80 });
                unlockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x89, 0x10, 0x80 });
                unlockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0x80 });
                unlockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0x00, 0xea, 0xea });
                unlockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x00, 0xc9, 0x00, 0x80 });
                unlockingCodes.Add(new byte[] { 0xa2, 0x18, 0x01, 0xbd, 0x27, 0x20, 0x89, 0x10, 0x00, 0xea, 0xea });
                unlockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0xA9, 0x10, 0x00 });
                unlockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0xc2, 0x20, 0xa9, 0x10, 0x00 });
                unlockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0xa9, 0x10, 0xc2 });
                unlockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0xa9, 0x10, 0x85, 0x62, 0xc2 });

                // Pattern for PAL games
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false, true, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, true, false, true, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false });
            }

            else
            {
                // Bad codes in NTSC games
                lockingCodes.Add(new byte[] { 0x3f, 0x21, 0x29, 0x10, 0xf0 });
                lockingCodes.Add(new byte[] { 0x3f, 0x21, 0x89, 0x10, 0xf0 });
                lockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xd0 });
                lockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x89, 0x10, 0xd0 });
                lockingCodes.Add(new byte[] { 0x3f, 0x21, 0x29, 0x10, 0x00, 0xf0 });
                lockingCodes.Add(new byte[] { 0x3f, 0x21, 0x89, 0x10, 0x00, 0xf0 });
                lockingCodes.Add(new byte[] { 0x3f, 0x21, 0x29, 0x10, 0x00, 0xd0 });
                lockingCodes.Add(new byte[] { 0x3f, 0x21, 0x89, 0x10, 0x00, 0xd0 });
                lockingCodes.Add(new byte[] { 0x3f, 0x21, 0x89, 0x10, 0xc2, 0x00, 0xf0 });
                lockingCodes.Add(new byte[] { 0x3f, 0x21, 0x89, 0x10, 0xc2, 0x00, 0xd0 });
                lockingCodes.Add(new byte[] { 0x3f, 0x21, 0x29, 0x10, 0xc9, 0x10, 0xf0 });
                lockingCodes.Add(new byte[] { 0x3f, 0x21, 0x89, 0x10, 0xc9, 0x10, 0xf0 });
                lockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xc9, 0x00, 0xf0 });
                lockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xc9, 0x00, 0xd0 });
                lockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xc9, 0x10, 0xd0 });
                lockingCodes.Add(new byte[] { 0x3f, 0x21, 0x29, 0x10, 0xcf, 0x00, 0x00, 0x80, 0xf0 });
                lockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x8d, 0x00, 0x00, 0x29, 0x10, 0x8d });
                lockingCodes.Add(new byte[] { 0x3f, 0x21, 0x00, 0x29, 0x10, 0xf0 });
                lockingCodes.Add(new byte[] { 0x3f, 0x21, 0x00, 0x89, 0x10, 0xf0 });
                lockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0xd0 });
                lockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x89, 0x10, 0xd0 });
                lockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0x00, 0xf0 });
                lockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x89, 0x10, 0x00, 0xf0 });
                lockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x00, 0xc9, 0x00, 0xf0 });
                lockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0x80, 0x2d, 0x00, 0x1b });
                lockingCodes.Add(new byte[] { 0x3f, 0x21, 0x00, 0x89, 0x10, 0xc2, 0x00, 0xf0 });
                lockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x00, 0x00, 0x29, 0x10, 0x00, 0xd0 });
                lockingCodes.Add(new byte[] { 0x3f, 0x21, 0xc2, 0x00, 0x29, 0x10, 0x00, 0xf0 });
                lockingCodes.Add(new byte[] { 0x3f, 0x21, 0xc2, 0x00, 0x29, 0x10, 0x00, 0xd0 });
                lockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0xea, 0x89, 0x10, 0x00, 0xd0 });
                lockingCodes.Add(new byte[] { 0xa2, 0x18, 0x01, 0xbd, 0x27, 0x20, 0x89, 0x10, 0x00, 0xd0, 0x01 });
                lockingCodes.Add(new byte[] { 0x29, 0x10, 0x00, 0xa2, 0x00, 0x00, 0xc9, 0x10, 0x00, 0xd0 });
                lockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x29, 0x0f });       // Dezaemon - Kaite Tsukutte Asoberu
                lockingCodes.Add(new byte[] { 0xad, 0x39, 0xb5, 0xd0, 0x1a, 0x22 });     // Earthbound
                lockingCodes.Add(new byte[] { 0x1a, 0x8f, 0xf0, 0x7f, 0x31, 0xcf, 0xf0 });   // Earthbound
                lockingCodes.Add(new byte[] { 0x1a, 0x8f, 0xf0, 0x7f, 0x32, 0xcf, 0xf0 });   // Earthbound
                lockingCodes.Add(new byte[] { 0x1a, 0x8f, 0xf0, 0x7f, 0x31, 0xcf, 0xf0 });   // Earthbound
                lockingCodes.Add(new byte[] { 0xa1, 0xc0, 0xca, 0x10, 0xf8, 0x38, 0xef, 0xf2, 0xfd, 0xc3, 0xf0 });     // Earthbound
                lockingCodes.Add(new byte[] { 0xa1, 0xc0, 0xca, 0x10, 0xf8, 0xef, 0x38, 0xf2, 0xfd, 0xc3, 0xf0 });   // Earthbound
                lockingCodes.Add(new byte[] { 0xa1, 0xc0, 0xca, 0x10, 0xf8, 0x38, 0xef, 0xef, 0xff, 0xc1 });     // Earthbound
                lockingCodes.Add(new byte[] { 0xad, 0xff, 0x1f, 0xd0, 0x06, 0x22, 0x64, 0x5f, 0xc0, 0x80, 0x04, 0x22, 0x00, 0x8a, 0x0f, 0xc2, 0x30, 0xab, 0x2b, 0x7a, 0xfa, 0x68, 0x40 });     // Cooly Skunk

                // Good codes for NTSC games
                unlockingCodes.Add(new byte[] { 0x3f, 0x21, 0x29, 0x10, 0x80 });
                unlockingCodes.Add(new byte[] { 0x3f, 0x21, 0x89, 0x10, 0x80 });
                unlockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xea, 0xea });
                unlockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x89, 0x10, 0xea, 0xea });         // Could also be 0xad, 0x3f, 0x21, 0x89, 0x10, 0x80 but uCON64 uses (ea ea)
                unlockingCodes.Add(new byte[] { 0x3f, 0x21, 0x29, 0x10, 0x00, 0x80 });
                unlockingCodes.Add(new byte[] { 0x3f, 0x21, 0x89, 0x10, 0x00, 0x80 });
                unlockingCodes.Add(new byte[] { 0x3f, 0x21, 0x29, 0x10, 0x00, 0xea, 0xea });
                unlockingCodes.Add(new byte[] { 0x3f, 0x21, 0x89, 0x10, 0x00, 0xea, 0xea });
                unlockingCodes.Add(new byte[] { 0x3f, 0x21, 0x89, 0x10, 0xc2, 0x00, 0x80 });
                unlockingCodes.Add(new byte[] { 0x3f, 0x21, 0x89, 0x10, 0xc2, 0x00, 0xea, 0xea });
                unlockingCodes.Add(new byte[] { 0x3f, 0x21, 0x29, 0x10, 0xc9, 0x10, 0x80 });
                unlockingCodes.Add(new byte[] { 0x3f, 0x21, 0x89, 0x10, 0xc9, 0x10, 0x80 });
                unlockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xc9, 0x00, 0xea, 0xea });
                unlockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xc9, 0x00, 0x80 });
                unlockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x29, 0x10, 0xc9, 0x10, 0xea, 0xea });
                unlockingCodes.Add(new byte[] { 0x3f, 0x21, 0x29, 0x10, 0xcf, 0x00, 0x00, 0x80, 0x80 });
                unlockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x8d, 0x00, 0x00, 0x29, 0x00, 0x8d });
                unlockingCodes.Add(new byte[] { 0x3f, 0x21, 0x00, 0x29, 0x10, 0x80 });
                unlockingCodes.Add(new byte[] { 0x3f, 0x21, 0x00, 0x89, 0x10, 0x80 });
                unlockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0xea, 0xea });
                unlockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x89, 0x10, 0xea, 0xea });
                unlockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x10, 0x00, 0x80 });
                unlockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x89, 0x10, 0x00, 0x80 });
                unlockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x00, 0xc9, 0x00, 0x80 });
                unlockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x29, 0x00, 0x80, 0x2d, 0x00, 0x1b });
                unlockingCodes.Add(new byte[] { 0x3f, 0x21, 0x00, 0x89, 0x10, 0xc2, 0x00, 0x80 });
                unlockingCodes.Add(new byte[] { 0xaf, 0x3f, 0x21, 0x00, 0x00, 0x00, 0x29, 0x10, 0x00, 0xea, 0xea });
                unlockingCodes.Add(new byte[] { 0x3f, 0x21, 0xc2, 0x00, 0x29, 0x10, 0x00, 0x80 });
                unlockingCodes.Add(new byte[] { 0x3f, 0x21, 0xc2, 0x00, 0x29, 0x10, 0x00, 0xea, 0xea });
                unlockingCodes.Add(new byte[] { 0xa9, 0x00, 0x00, 0xea, 0x89, 0x10, 0x00, 0xd0 });
                unlockingCodes.Add(new byte[] { 0xa2, 0x18, 0x01, 0xbd, 0x27, 0x20, 0x89, 0x10, 0x00, 0xea, 0xea });
                unlockingCodes.Add(new byte[] { 0x29, 0x10, 0x00, 0xa2, 0x00, 0x00, 0xc9, 0x10, 0x00, 0x80 });
                unlockingCodes.Add(new byte[] { 0xad, 0x3f, 0x21, 0x29, 0xff });       // Dezaemon - Kaite Tsukutte Asoberu
                unlockingCodes.Add(new byte[] { 0xad, 0x39, 0xb5, 0xea, 0xea, 0x22 });       // Earthbound
                unlockingCodes.Add(new byte[] { 0x1a, 0x8f, 0xf0, 0x7f, 0x30, 0xcf, 0xf0 });     // Earthbound
                unlockingCodes.Add(new byte[] { 0x1a, 0x8f, 0xf0, 0x7f, 0x30, 0xcf, 0xf0 });     // Earthbound
                unlockingCodes.Add(new byte[] { 0x1a, 0x8f, 0xf0, 0x7f, 0x30, 0xcf, 0xf0 });     // Earthbound
                unlockingCodes.Add(new byte[] { 0xa1, 0xc0, 0xca, 0x10, 0xf8, 0x38, 0xea, 0xa9, 0x00, 0x00, 0x80 });       // Earthbound
                unlockingCodes.Add(new byte[] { 0xa1, 0xc0, 0xca, 0x10, 0xf8, 0x38, 0xea, 0xa9, 0x00, 0x00, 0x80 });     // Earthbound
                unlockingCodes.Add(new byte[] { 0xa1, 0xc0, 0xca, 0x10, 0xf8, 0x38, 0xea, 0xa9, 0x00, 0x00 });       // Earthbound
                unlockingCodes.Add(new byte[] { 0xea, 0xea, 0xea, 0xea, 0xea, 0x22, 0x64, 0x5f, 0xc0, 0x80, 0x04, 0xea, 0xea, 0xea, 0xea, 0xc2, 0x30, 0xab, 0x2b, 0x7a, 0xfa, 0x68, 0x40 });       //  Cooly Skunk


                // Pattern for NTSC games
                lockingCodePattern.Add(new bool[] { false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, true, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, true, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, true, true, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, true, true, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, true, false, true, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, true, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, true, true, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, true, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, true, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false, false, false });
                lockingCodePattern.Add(new bool[] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false });
            }

            return FindAndReplace(lockingCodes, unlockingCodes, lockingCodePattern, unlock);
        }

        public bool RemoveSlowROMChecks(bool unlock)
        {
            IDictionary<string, string> lockingCodeDictionary = new Dictionary<string, string>();

            lockingCodeDictionary.Add(@"(A2|A9)(01)(8D|8E)(0D42)", "$1 00 $3 $4");
            lockingCodeDictionary.Add(@"(A9)(01)(008D0D42)", "$1 00 $3");
            lockingCodeDictionary.Add(@"(A9)(01)(8F0D4200)", "$1 00 $3");

            return FindAndReplaceByRegEx(lockingCodeDictionary, unlock);
        }

        public bool RemoveSRAMChecks(bool unlock)
        {
            IDictionary<string, string> lockingCodeDictionary = new Dictionary<string, string>();

            if (ByteSRAMSize > 0x00)
            {
                if (StringMapMode.Contains("LoROM"))
                {
                    List<string> excludedTitles = new List<string>{ "OHCHAN NO LOGIC" };

                    if (ByteSRAMSize == 0x03 || excludedTitles.Contains(StringTitle.Trim()))
                    {
                        lockingCodeDictionary.Add(@"(8F|9F)(\w{4})(70)(CF|DF)(\w{4})(70)(D0)", "$1 $2 $3 $4 $5 $6 EA EA");
                        lockingCodeDictionary.Add(@"(8F|9F)(\w{4})(70)(CF|DF)(\w{4})(70)(F0)", "$1 $2 $3 $4 $5 $6 80");
                    }
                    
                    else
                    {
                        lockingCodeDictionary.Add(@"(8F|9F)(\w{4})(70)(CF|DF)(\w{4})(70)(D0)", "$1 $2 $3 $4 $5 $6 80");
                        lockingCodeDictionary.Add(@"(8F|9F)(\w{4})(70)(CF|DF)(\w{4})(70)(F0)", "$1 $2 $3 $4 $5 $6 EA EA");
                    }

                    lockingCodeDictionary.Add(@"(A90000A2FE1FDF000070)(D0)", "$1 EA EA");                                                   // Super Metroid
                    lockingCodeDictionary.Add(@"(8F)(\w{4})(77E2)(\w{2})(AF)(\w{4})(77C9)(\w{2})(F0)", "$1 $2 $3 $4 $5 $6 $7 $8 80");       // Uniracers / Unirally
                    lockingCodeDictionary.Add(@"(CA10F838EF1A8081)(8D)", "$1 9C");                                                          // Kirby's Dream Course
                    lockingCodeDictionary.Add(@"(81CA10F8CF398087)(F0)", "$1 80");                                                          // Kirby's Dream Course

                    return FindAndReplaceByRegEx(lockingCodeDictionary, unlock);
                }

                else if (StringMapMode.Contains("HiROM"))
                {
                    if (StringTitle.Contains("DONKEY KONG COUNTRY") || StringTitle.Contains("SUPER DONKEY KONG")) { lockingCodeDictionary.Add(@"(8F|9F)(57|59)(60|68)(30|31|32|33)(CF|DF)(57|59)(60)(30|31|32|33)(D0)", "$1 $2 $3 $4 $5 $6 $7 $8 EA EA"); }    // Donkey Kong Country
                    
                    if (StringTitle.Contains("DONKEY KONG COUNTRY")) { lockingCodeDictionary.Add(@"(8F|9F)(\w{4})(30|31|32|33)(CF|DF)(\w{4})(30|31|32|33)(D0)", "$1 $2 $3 $4 $5 $6 EA EA"); }
                    else { lockingCodeDictionary.Add(@"(8F|9F)(\w{4})(30|31|32|33)(CF|DF)(\w{4})(30|31|32|33)(D0)", "$1 $2 $3 $4 $5 $6 80"); }

                    lockingCodeDictionary.Add(@"(8F|9F)(\w{4})(30|31|32|33)(CF|DF)(\w{4})(30|31|32|33)(F0)", "$1 $2 $3 $4 $5 $6 EA EA");
                    lockingCodeDictionary.Add(@"(8F|9F)(\w{4})(30|31|32|33)(AF)(\w{4})(30|31|32|33)(C9)(\w{4})(D0)", "$1 $2 $3 $4 $5 $6 $7 $8 80");

                    lockingCodeDictionary.Add(@"(AF|BF)(\w{2})(FF)(80|C0)(CF|DF)(\w{2})(FF40)(F0)", "$1 $2 $3 $4 $5 $6 $7 80");                    // Breath of Fire II
                    lockingCodeDictionary.Add(@"(AF|BF)(\w{4})(30|31|32|33)(CF|DF)(\w{4})(30|31|32|33)(F0)", "$1 $2 $3 $4 $5 $6 80");              // Breath of Fire II
                    lockingCodeDictionary.Add(@"(8F|AF)(\w{4})(B0CF)(\w{4})(B1)(D0)", "$1 $2 $3 $4 $5 EA EA");                                     // Mario no Super Picross
                    lockingCodeDictionary.Add(@"(2638E94812C9AF71)(F0)", "$1 80");                                                                 // Donkey Kong Country 2 - Diddy's Kong Quest
                    lockingCodeDictionary.Add(@"(A05C2F7732E9C704)(F0)", "$1 80");                                                                 // Donkey Kong Country 2 - Diddy's Kong Quest
                    lockingCodeDictionary.Add(@"(4B4F4E4700F8)(F7)", "$1 F8");                                                                     // Donkey Kong Country 2 - Diddy's Kong Quest
                    lockingCodeDictionary.Add(@"(A9C3)(80DD)(FFFF)(F06C)", "$1 F0 CC $3 80 7D");                                                   // Donkey Kong Country 3 - Dixie Kong's Double Trouble
                    lockingCodeDictionary.Add(@"(D0F4ABCFAEFF00D0)(01)", "$1 00");                                                                 // Front Mission - Gun Hazard

                    return FindAndReplaceByRegEx(lockingCodeDictionary, unlock);
                }
            }

            else
            {
                if (StringMapMode.Contains("LoROM"))
                {
                    lockingCodeDictionary.Add(@"(8F|9F)(\w{4})(70)(CF|DF)(\w{4})(70)(F0)", "$1 $2 $3 $4 $5 $6 EA EA");                                                          // Mega Man X
                    lockingCodeDictionary.Add(@"(AF|BF)(\w{2})(8000)(CF|DF)(\w{2})(8040)(F0)", "$1 $2 $3 $4 $5 $6 80");                                                         // Mega Man X
                    lockingCodeDictionary.Add(@"(AF|BF)(\w{2})(FF)(80|C0)(CF|DF)(\w{2})(FF40)(F0)", "$1 $2 $3 $4 $5 $6 $7 80");                                                 // Demon's Crest
                    lockingCodeDictionary.Add(@"(8F)(\w{4})(70AF)(\w{4})(70C9)(\w{4})(D0)", "$1 $2 $3 $4 $5 $6 80");                                                            // Tetris Attack
                    lockingCodeDictionary.Add(@"(C230)(ADCF1F)(C95044D0)", "$1 4C D1 80 $3");                                                                                   // Tetris Attack
                    lockingCodeDictionary.Add(@"(AF481F00F00CC220B9081C49FFFF1A99081C6BDA5A8D0002)(78F8)(AD220238ED00028D2202)(D858)(22629600)", "$1 80 00 $3 80 00 $5");       // Beavis & Butthead

                    return FindAndReplaceByRegEx(lockingCodeDictionary, unlock);
                }

                else if (StringMapMode.Contains("HiROM"))
                {
                    lockingCodeDictionary.Add(@"(5C7FD08318FB78C230)", "EAEAEAEAEAEAEAEAEA");           // Killer Instinct
                    lockingCodeDictionary.Add(@"(22085C10B028)", "EAEAEAEAEAEA");                       // BS The Legend of Zelda Remix
                    lockingCodeDictionary.Add(@"(DAE230C9)(01)(F018C9)(02)", "$1 09 $3 07");            // BS The Legend of Zelda Remix
                    lockingCodeDictionary.Add(@"(29FF00C9)(07)(009016)", "$1 00 $3");                   // BS The Legend of Zelda Remix

                    return FindAndReplaceByRegEx(lockingCodeDictionary, unlock);
                }
            }

            return false;
        }

        public bool FindAndReplace(List<byte[]> lockingCodes, List<byte[]> unlockingCodes, List<bool[]> lockingCodePattern, bool unlock)
        {
            bool foundBadCode = false;
            int index = 0;

            byte[] regionFixedSourceROM = null;

            if (unlock)
            {
                regionFixedSourceROM = new byte[SourceROM.Length];
                Buffer.BlockCopy(SourceROM, 0, regionFixedSourceROM, 0, SourceROM.Length);
            }

            foreach (byte[] lockingCode in lockingCodes)
            {
                byte[] unlockingCode = unlockingCodes[index];
                bool[] lockingPattern = lockingCodePattern[index];

                int offset = -1;
                int c = SourceROM.Length - lockingCode.Length + 1;
                int j;

                for (int i = 0; i < c; i++)
                {
                    if (SourceROM[i] != lockingCode[0])
                    {
                        continue;
                    }

                    for (j = lockingCode.Length - 1; j >= 1 && SourceROM[i + j] == lockingCode[j] || lockingPattern[j]; j--)
                    {
                        if (lockingPattern[j])
                        {
                            unlockingCode[j] = SourceROM[i + j];
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
                SourceROM = regionFixedSourceROM;
                Initialize();
                return true;
            }

            return false;
        }

        public bool FindAndReplaceByRegEx(IDictionary<string, string> lockingCodeDictionary, bool unlock)
        {
            string sourceROMString = BitConverter.ToString(SourceROM).Replace("-", "");
            int ulcCtr = 0;
            int matchCtr = 0;

            foreach (KeyValuePair<string, string> lockingCode in lockingCodeDictionary)
            {
                Regex r = new Regex(lockingCode.Key);

                foreach (Match match in r.Matches(sourceROMString))
                {
                    if (match.Index % 2 == 0)
                    {
                        if (unlock)
                        {
                            string newHexString = Regex.Replace(match.Value, lockingCode.Key, lockingCode.Value).Replace(" ", "");
                            sourceROMString = sourceROMString.Remove(match.Index, newHexString.Length).Insert(match.Index, newHexString);
                            matchCtr++;
                        }

                        else
                        {
                            return true;
                        }
                    }
                }

                ulcCtr++;
            }

            if (matchCtr > 0)
            {
                SourceROM = StringToByteArray(sourceROMString);
                Initialize();
                return true;
            }

            return false;
        }

        public static byte[] StringToByteArray(string hexString)
        {
            int numberChars = hexString.Length;
            byte[] bytes = new byte[numberChars/2];

            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i/2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}