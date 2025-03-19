using System;
using System.Linq;

namespace Advanced_SNES_ROM_Utility.Functions
{
    public static partial class SNESROMFunction
    {
        public static void FixChecksum(this SNESROM sourceROM)
        {
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

            Buffer.BlockCopy(newChksmSequence, 0, sourceROM.SourceROM, (int)(sourceROM.UIntROMHeaderOffset + (int)HeaderValue.inverse_checksum), newChksmSequence.Length);

            if (sourceROM.IsExROMHeaderCopied)
            {
                byte[] defaultChecksum = new byte[4] { 0xFF, 0xFF, 0x00, 0x00 };
                byte[] chksmHeaderCopy = new byte[4];
                Buffer.BlockCopy(sourceROM.SourceROM, (int)(sourceROM.UIntROMHeaderCopyOffset + (int)HeaderValue.inverse_checksum), chksmHeaderCopy, 0, chksmHeaderCopy.Length);

                // Some games like Tales of Phantasia might have a default checksum in their copied header, so we don't overwrite in this case
                if (!chksmHeaderCopy.SequenceEqual(defaultChecksum))
                {
                    Buffer.BlockCopy(newChksmSequence, 0, sourceROM.SourceROM, (int)(sourceROM.UIntROMHeaderCopyOffset + (int)HeaderValue.inverse_checksum), newChksmSequence.Length);
                }
            }

            sourceROM.Initialize(new string[] { "romheader", "checksum" });
        }
    }
}