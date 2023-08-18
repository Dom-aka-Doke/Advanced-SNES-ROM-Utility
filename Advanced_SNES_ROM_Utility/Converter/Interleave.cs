using System;
using System.Text;
using System.Windows.Forms;
using Advanced_SNES_ROM_Utility.Lists;

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
                    intpattern = SNESROMList.DeIntPattern20;
                }

                else if (!ufo && sourceROM.IntCalcFileSize == 24)
                {
                    // Array with interleaving pattern for 24 MBit ROMs which were NOT dumped with Super UFO
                    intpattern = SNESROMList.DeIntPattern24;
                }

                else if (gd && sourceROM.IntCalcFileSize == 48)
                {
                    // Array with interleaving pattern for 48 MBit ROMs which were dumped WITH Super Game Doctor
                    intpattern = SNESROMList.DeIntPattern48;
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