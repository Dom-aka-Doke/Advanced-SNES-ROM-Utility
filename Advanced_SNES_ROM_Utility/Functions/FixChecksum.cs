using System;

namespace Advanced_SNES_ROM_Utility.Functions
{
    public static partial class SNESROMFunction
    {
        public static void FixChecksum(this SNESROM sourceROM)
        {
            uint offset = sourceROM.UIntROMHeaderOffset + 0x2C;
            byte[] newChksm = new byte[2];
            byte[] newInvChksm = new byte[2];
            byte[] newChksmSequence = new byte[4];

            newChksm = sourceROM.ByteArrayCalcChecksum;
            newInvChksm = sourceROM.ByteArrayCalcInvChecksum;

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

            Buffer.BlockCopy(newChksmSequence, 0, sourceROM.SourceROM, (int)offset, newChksmSequence.Length);

            if (sourceROM.UIntROMHeaderOffset == (int)HeaderOffset.exlorom || sourceROM.UIntROMHeaderOffset == (int)HeaderOffset.exhirom)
            {
                Buffer.BlockCopy(newChksmSequence, 0, sourceROM.SourceROM, (int)offset - 0x400000, newChksmSequence.Length);
            }

            sourceROM.Initialize();
        }
    }
}