using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        public void ApplyIPSPatch(string ipsFilePath, bool saveWithHeader)
        {
            byte[] byteArrayIPSArray = File.ReadAllBytes(ipsFilePath);

            // IPS patches muste be at least 14 bytes
            if (byteArrayIPSArray.Length < 14) { return; }

            // Check if IPS patch starts with magic number and contains end-of-file marker
            byte[] magicNumber = Encoding.ASCII.GetBytes("PATCH");
            byte[] checkMagicNumber = new byte[magicNumber.Length];
            Array.Copy(byteArrayIPSArray, 0, checkMagicNumber, 0, magicNumber.Length);
            if (!checkMagicNumber.SequenceEqual(magicNumber)) { return; }

            string eofMarker = "EOF";
            byte[] checkEOFMarker = new byte[eofMarker.Length + 3];
            Array.Copy(byteArrayIPSArray, byteArrayIPSArray.Length - (eofMarker.Length + 3), checkEOFMarker, 0, eofMarker.Length + 3);
            string stringCheckEOFMarker = Encoding.ASCII.GetString(checkEOFMarker);
            if (!stringCheckEOFMarker.StartsWith(eofMarker) && !stringCheckEOFMarker.EndsWith(eofMarker)) { return; }

            int endingOffset = byteArrayIPSArray.Length - eofMarker.Length;

            // Prepare ROM for patching
            byte[] patchedSourceROM = null;

            if (saveWithHeader)
            {
                // Merge header with ROM if header exists
                patchedSourceROM = new byte[SourceROMSMCHeader.Length + SourceROM.Length];

                Buffer.BlockCopy(SourceROMSMCHeader, 0, patchedSourceROM, 0, SourceROMSMCHeader.Length);
                Buffer.BlockCopy(SourceROM, 0, patchedSourceROM, SourceROMSMCHeader.Length, SourceROM.Length);
            }

            else
            {
                // Just copy source ROM if no header exists
                patchedSourceROM = new byte[SourceROM.Length];

                Buffer.BlockCopy(SourceROM, 0, patchedSourceROM, 0, SourceROM.Length);
            }

            // Expand or truncate if file size infromation is included
            if (stringCheckEOFMarker.StartsWith(eofMarker))
            {
                MessageBox.Show("Cannot handle IPS file size information yet, be patient ...");

                return;

                endingOffset -= 3;

                byte[] newSize = new byte[3] { checkEOFMarker[3], checkEOFMarker[4], checkEOFMarker[5] };
                if (BitConverter.IsLittleEndian) { Array.Reverse(newSize); }
                byte[] uint32NewSize = new byte[4] { newSize[0], newSize[1], newSize[2], 0x00 };
                uint uintNewSize = BitConverter.ToUInt32(uint32NewSize, 0);

                patchedSourceROM = new byte[uintNewSize];
            }

            // Start patching
            for (int i = magicNumber.Length; i < endingOffset; i++)
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

                    if (BitConverter.IsLittleEndian) { Array.Reverse(payload); }
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
                Buffer.BlockCopy(payload, 0, patchedSourceROM, (int)uintOffset, payloadLength);

                // Fix loop increment
                i--;
            }

            SourceROM = patchedSourceROM;
        }
    }
}