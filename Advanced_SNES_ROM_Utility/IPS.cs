using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        public byte[] ApplyIPSPatch(string ipsFilePath)
        {
            byte[] byteArrayIPSPatch = File.ReadAllBytes(ipsFilePath);

            // IPS patches muste be at least 14 bytes
            if (byteArrayIPSPatch.Length < 14) { return null; }

            // Check if IPS patch starts with magic number and contains end-of-file marker
            byte[] magicNumber = Encoding.ASCII.GetBytes("PATCH");
            byte[] checkMagicNumber = new byte[magicNumber.Length];
            Array.Copy(byteArrayIPSPatch, 0, checkMagicNumber, 0, magicNumber.Length);
            if (!checkMagicNumber.SequenceEqual(magicNumber)) { return null; }

            string eofMarker = "EOF";
            byte[] checkEOFMarker = new byte[eofMarker.Length + 3];
            Array.Copy(byteArrayIPSPatch, byteArrayIPSPatch.Length - (eofMarker.Length + 3), checkEOFMarker, 0, eofMarker.Length + 3);
            string stringCheckEOFMarker = Encoding.ASCII.GetString(checkEOFMarker);
            if (!stringCheckEOFMarker.StartsWith(eofMarker) && !stringCheckEOFMarker.EndsWith(eofMarker)) { return null; }

            int ipsFileEndingOffset = byteArrayIPSPatch.Length - eofMarker.Length;
            int ipsFileExpansionSize = 0;
            int ipsFileTruncationSize = 0;

            // Get size to truncate file to, if infromation is included
            if (stringCheckEOFMarker.StartsWith(eofMarker))
            {
                byte[] newSize24 = new byte[3] { checkEOFMarker[3], checkEOFMarker[4], checkEOFMarker[5] };
                if (BitConverter.IsLittleEndian) { Array.Reverse(newSize24); }
                byte[] newSize32 = new byte[4] { newSize24[0], newSize24[1], newSize24[2], 0x00 };
                ipsFileTruncationSize = BitConverter.ToInt32(newSize32, 0);

                ipsFileEndingOffset -= 3;
            }

            // Prepare ROM for patching
            byte[] patchedSourceROM = null;

            ipsFileExpansionSize = Patch(magicNumber, ipsFileEndingOffset, byteArrayIPSPatch, patchedSourceROM, false);

            if (ipsFileExpansionSize > (SourceROM.Length + UIntSMCHeader))
            {
                patchedSourceROM = new byte[ipsFileExpansionSize];
            }

            else
            {
                patchedSourceROM = new byte[SourceROM.Length + UIntSMCHeader];
            }

            // Copy source ROM data over to ROM for patching
            if (SourceROMSMCHeader != null && UIntSMCHeader > 0)
            {
                // Merge header with ROM if header exists
                Buffer.BlockCopy(SourceROMSMCHeader, 0, patchedSourceROM, 0, SourceROMSMCHeader.Length);
                Buffer.BlockCopy(SourceROM, 0, patchedSourceROM, SourceROMSMCHeader.Length, SourceROM.Length);
            }

            else
            {
                // Just copy source ROM if no header exists
                Buffer.BlockCopy(SourceROM, 0, patchedSourceROM, 0, SourceROM.Length);
            }

            // Patch file
            Patch(magicNumber, ipsFileEndingOffset, byteArrayIPSPatch, patchedSourceROM, true);

            // Truncate file, if necessary
            if (ipsFileTruncationSize > 0 && ipsFileTruncationSize < patchedSourceROM.Length)
            {
                byte[] tempPatchedSourceROM = patchedSourceROM;
                patchedSourceROM = new byte[ipsFileTruncationSize];
                Buffer.BlockCopy(tempPatchedSourceROM, 0, patchedSourceROM, 0, ipsFileTruncationSize);
            }

            return patchedSourceROM;
        }

        private int Patch(byte[] magicNumber, int ipsFileEndingOffset, byte[] byteArrayIPSArray, byte[] patchedSourceROM, bool patch)
        {
            int effectivePatchSize = 0;

            // Start patching
            for (int i = magicNumber.Length; i < ipsFileEndingOffset; i++)
            {
                byte[] offset = new byte[3];
                byte[] length = new byte[2];

                // Get offset to insert
                Array.Copy(byteArrayIPSArray, i, offset, 0, offset.Length);
                if (BitConverter.IsLittleEndian) { Array.Reverse(offset); }
                byte[] uint32Offset = new byte[4] { offset[0], offset[1], offset[2], 0x00 };
                uint uintOffset = BitConverter.ToUInt32(uint32Offset, 0);
                i += offset.Length;

                // Get length of payload
                Array.Copy(byteArrayIPSArray, i, length, 0, length.Length);
                if (BitConverter.IsLittleEndian) { Array.Reverse(length); }
                int payloadLength = BitConverter.ToUInt16(length, 0);
                i += length.Length;

                // Initialize payload
                byte[] payload = null;

                // Check for RLE hunk
                if (payloadLength == 0)
                {
                    // Get length of RLE run
                    Array.Copy(byteArrayIPSArray, i, length, 0, length.Length);
                    if (BitConverter.IsLittleEndian) { Array.Reverse(length); }
                    payloadLength = BitConverter.ToUInt16(length, 0);
                    i += length.Length;

                    // Get byte for RLE payload
                    byte rlePayloadByte = byteArrayIPSArray[i];
                    i++;

                    // Generate RLE payload
                    payload = new byte[payloadLength];

                    for (int y = 0; y < payloadLength; y++)
                    {
                        payload[y] = rlePayloadByte;
                    }
                }

                // If no RLE hunk was detected, read payload
                else
                {
                    payload = new byte[payloadLength];
                    if (BitConverter.IsLittleEndian) { Array.Reverse(payload); }
                    Array.Copy(byteArrayIPSArray, i, payload, 0, payloadLength);
                    i += payloadLength;
                }

                // Insert payload into SourceROM
                if (patch)
                {
                    Buffer.BlockCopy(payload, 0, patchedSourceROM, (int)uintOffset, payloadLength);
                }

                // Fix loop increment
                i--;

                // Calculate size on last loop operation
                if (i == (ipsFileEndingOffset - 1))
                {
                    effectivePatchSize = (int)uintOffset + payloadLength;
                }
            }

            return effectivePatchSize;
        }
    }
}