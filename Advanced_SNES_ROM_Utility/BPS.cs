using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        public byte[] ApplyBPSPatch(string bpsFilePath)
        {
            byte[] byteArrayBPSPatch = File.ReadAllBytes(bpsFilePath);
            ulong offsetBPSPatch = 0;
            ulong offsetSourceFile = 0;

            // Check if BPS patch starts with magic number
            byte[] magicNumber = Encoding.ASCII.GetBytes("BPS1");
            byte[] checkMagicNumber = new byte[magicNumber.Length];
            Array.Copy(byteArrayBPSPatch, (int)offsetBPSPatch, checkMagicNumber, 0, magicNumber.Length);
            if (!checkMagicNumber.SequenceEqual(magicNumber)) { return null; }
            offsetBPSPatch += 4;

            // Get VWI information
            ulong vwiSourceFileLength = GetVWI(byteArrayBPSPatch, ref offsetBPSPatch);
            ulong vwiDestinationFileLength = GetVWI(byteArrayBPSPatch, ref offsetBPSPatch);
            ulong vwiMetadataLength = GetVWI(byteArrayBPSPatch, ref offsetBPSPatch);

            // Generate patched file using VWI information
            while (offsetBPSPatch < (ulong)byteArrayBPSPatch.Length - 12)
            {
                ulong data = GetVWI(byteArrayBPSPatch, ref offsetBPSPatch);
                ulong command = data & 3;
                ulong length = (data >> 2) + 1;
            }

            return null;
        }
        /*
        void sourceRead()
        {
            while (length--)
            {
                target[outputOffset] = source[outputOffset];
                outputOffset++;
            }
        }

        void targetRead()
        {
            while (length--)
            {
                target[outputOffset++] = read();
            }
        }

        void sourceCopy()
        {
            uint64 data = decode();
            sourceRelativeOffset += (data & 1 ? -1 : +1) * (data >> 1);
            while (length--)
            {
                target[outputOffset++] = source[sourceRelativeOffset++];
            }
        }

        void targetCopy()
        {
            uint64 data = decode();
            targetRelativeOffset += (data & 1 ? -1 : +1) * (data >> 1);
            while (length--)
            {
                target[outputOffset++] = target[targetRelativeOffset++];
            }
        }
        */
    }
}
