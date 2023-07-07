using System;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
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