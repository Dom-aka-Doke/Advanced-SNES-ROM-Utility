using System;
using System.Collections.Generic;
using System.IO;

namespace Advanced_SNES_ROM_Utility.Converter
{
    public static partial class SNESROMConvert
    {
        public static void SwapBin(this SNESROM sourceROM)
        {
            // Check if ROM is multiple of 8 MBit, otherwise swapping is not possible
            if (sourceROM.SourceROM.Length % 1048576 == 0)
            {
                int romChunks = sourceROM.SourceROM.Length / 1048576;

                if (romChunks > 1)
                {
                    // Define size for single ROM file (8 Mbit)
                    int chunkSize = 1048576;

                    for (int index = 0; index < romChunks; index++)
                    {
                        string romChunkName = sourceROM.ROMName + "_[" + index + "]";
                        byte[] sourceROMChunk = new byte[chunkSize];

                        Buffer.BlockCopy(sourceROM.SourceROM, index * chunkSize, sourceROMChunk, 0, chunkSize);

                        SwapBinChunk(sourceROMChunk, sourceROM.ROMFolder, romChunkName);
                    }
                }

                else
                {
                    SwapBinChunk(sourceROM.SourceROM, sourceROM.ROMFolder, sourceROM.ROMName);
                }
            }

            else
            {
                return;
            }
        }

        private static void SwapBinChunk(byte[] sourceROM, string romFolder, string romName)
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
            File.WriteAllBytes(romFolder + @"\" + romName + "_[swapped]" + ".bin", swappedSourceROM);
        }
    }
}