using System;
using System.Text;
using System.Windows.Forms;

namespace Advanced_SNES_ROM_Utility.Converter
{
    public static partial class SNESROMConvert
    {
        public static void Interleave(this SNESROM sourceROM)
        {
            // In some special cases of 20, 24 or 48 MBit ROMs, interleaving must be done by following a pattern
            int[] intpattern = null;

            if (sourceROM.IntCalcFileSize == 20 || sourceROM.IntCalcFileSize == 24 || sourceROM.IntCalcFileSize == 48)
            {
                byte[] ufoTitle = new byte[8];
                byte[] gdTitle = new byte[14];

                bool ufo = false;   // Super UFO
                bool gd = false;    // Game Doctor SF

                // Analyze SMC header
                if (sourceROM.SourceROMSMCHeader != null)
                {
                    Buffer.BlockCopy(sourceROM.SourceROMSMCHeader, 8, ufoTitle, 0, 8);
                    Buffer.BlockCopy(sourceROM.SourceROMSMCHeader, 0, gdTitle, 0, 14);

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
                else if ((!ufo && !gd) || sourceROM.SourceROMSMCHeader == null)
                {
                    string chooseCopierText = "ROM file has no or an unknown copier header and a file size of 20, 24 or 48 MBit.\n\n" +
                                              "In this case, interleaving requires the specification of a target format.\n\n" +
                                              "You now can go on by selecting a supported copier format or use standard interleaving.\n\n" +
                                              "Do this at your own risk! Don't proceed if you don't know exactly what you're doing!\n\n" +
                                              "Always verify your ROM by loading it in an emulator!";

                    DialogResult dialogResult = new DialogResult();
                    FormChooseCopier chooseCopier = new FormChooseCopier(chooseCopierText);
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

                if (!ufo && sourceROM.IntCalcFileSize == 20)
                {
                    // Array with interleaving pattern for 20 MBit ROMs which were NOT dumped with Super UFO
                    intpattern = new int[] { 1,   3,   5,   7,   9,  11,  13,  15,  17,  19,  21,  23,  25,  27,  29,
                                          31,  33,  35,  37,  39,  41,  43,  45,  47,  49,  51,  53,  55,  57,  59,
                                          61,  63,  65,  67,  69,  71,  73,  75,  77,  79,  64,  66,  68,  70,  72,
                                          74,  76,  78,  32,  34,  36,  38,  40,  42,  44,  46,  48,  50,  52,  54,
                                          56,  58,  60,  62,   0,   2,   4,   6,   8,  10,  12,  14,  16,  18,  20,
                                          22,  24,  26,  28,  30 };
                }

                else if (!ufo && sourceROM.IntCalcFileSize == 24)
                {
                    // Array with interleaving pattern for 24 MBit ROMs which were NOT dumped with Super UFO
                    intpattern = new int[] { 1,   3,   5,   7,   9,  11,  13,  15,  17,  19,  21,  23,  25,  27,  29,
                                          31,  33,  35,  37,  39,  41,  43,  45,  47,  49,  51,  53,  55,  57,  59,
                                          61,  63,  65,  67,  69,  71,  73,  75,  77,  79,  81,  83,  85,  87,  89,
                                          91,  93,  95,  64,  66,  68,  70,  72,  74,  76,  78,  80,  82,  84,  86,
                                          88,  90,  92,  94,   0,   2,   4,   6,   8,  10,  12,  14,  16,  18,  20,
                                          22,  24,  26,  28,  30,  32,  34,  36,  38,  40,  42,  44,  46,  48,  50,
                                          52,  54,  56,  58,  60,  62 };
                }

                else if (gd && sourceROM.IntCalcFileSize == 48)
                {
                    // Array with interleaving pattern for 48 MBit ROMs which were dumped WITH Super Game Doctor
                    intpattern = new int[] { 129, 131, 133, 135, 137, 139, 141, 143, 145, 147, 149, 151, 153, 155, 157,
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
            }

            byte[] interleavedROM = new byte[sourceROM.SourceROM.Length];   // Empty byte array for interleaved ROM

            int chunkSize = 32768;  // Number of bytes for each chunk
            int chunkItems = sourceROM.SourceROM.Length / chunkSize;    // Number of chunks

            if (intpattern != null)
            {
                for (int chunkCtr = 0; chunkCtr < chunkItems; chunkCtr++)
                {
                    Buffer.BlockCopy(sourceROM.SourceROM, intpattern[chunkCtr] * chunkSize, interleavedROM, chunkCtr * chunkSize, chunkSize);
                }
            }

            // If no special case is detectetd, take the standard interleaving algorithm
            else
            {
                for (int chunkCtr = 0; chunkCtr < chunkItems; chunkCtr++)
                {
                    int sourcePos = chunkCtr * chunkSize;
                    int destPos = (chunkCtr / 2) * chunkSize;

                    if (chunkCtr % 2 == 0)
                    {
                        destPos += ((sourceROM.IntCalcFileSize * 131072) / 2);
                    }

                    Buffer.BlockCopy(sourceROM.SourceROM, sourcePos, interleavedROM, destPos, chunkSize);
                }
            }

            sourceROM.SourceROM = interleavedROM;
            sourceROM.Initialize();
        }
    }
}