using System;
using Advanced_SNES_ROM_Utility.Helper;

namespace Advanced_SNES_ROM_Utility.Functions
{
    public static partial class SNESROMFunction
    {
        public static string CalculateCrc32Hash(byte[] sourceROM, byte[] sourceROMSMCHeader, uint uIntSMCHeader)
        {
            Crc32 crc32 = new Crc32();
            string hash = null;
            byte[] CRC32SourceROM = new byte[sourceROM.Length + uIntSMCHeader];

            if (sourceROMSMCHeader != null && uIntSMCHeader > 0)
            {
                // Merge header with ROM if header exists
                Buffer.BlockCopy(sourceROMSMCHeader, 0, CRC32SourceROM, 0, sourceROMSMCHeader.Length);
                Buffer.BlockCopy(sourceROM, 0, CRC32SourceROM, sourceROMSMCHeader.Length, sourceROM.Length);
            }

            else
            {
                Buffer.BlockCopy(sourceROM, 0, CRC32SourceROM, 0, sourceROM.Length);
            }

            foreach (byte singleByte in crc32.ComputeHash(CRC32SourceROM))
            {
                hash += singleByte.ToString("X2").ToUpper();
            }

            return hash;
        }
    }
}