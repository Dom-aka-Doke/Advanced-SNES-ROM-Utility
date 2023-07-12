using System;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
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

            if (UIntROMHeaderOffset == (int)HeaderOffset.exlorom || UIntROMHeaderOffset == (int)HeaderOffset.exhirom)
            {
                Buffer.BlockCopy(newChksmSequence, 0, SourceROM, (int)offset - 0x400000, newChksmSequence.Length);
            }

            Initialize();
        }
    }
}