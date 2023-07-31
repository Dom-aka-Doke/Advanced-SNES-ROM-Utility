using System;

namespace Advanced_SNES_ROM_Utility.Converter
{
    public static partial class SNESROMConvert
    {
        public static void Interleave(this SNESROM sourceROM)
        {
            byte[] interleavedROM = new byte[sourceROM.SourceROM.Length];   // Empty byte array for deinterleaved ROM

            int chunkSize = 32768;  // Number of bytes for each chunk
            int chunkItems = sourceROM.SourceROM.Length / chunkSize;    // Number of chunks

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

            sourceROM.SourceROM = interleavedROM;
            sourceROM.Initialize();
        }
    }
}