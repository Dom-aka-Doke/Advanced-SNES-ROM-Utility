using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        public void ApplyIPSPatch(string ipsFilePath)
        {
            byte[] byteArrayIPSArray = File.ReadAllBytes(ipsFilePath);

            // IPS patches muste be at least 14 bytes
            if (byteArrayIPSArray.Length < 14) { return; }

            // Check if IPS patch starts with magic number and contains end-of-file marker
            byte[] magicNumber = Encoding.ASCII.GetBytes("PATCH");
            byte[] eofMarker = Encoding.ASCII.GetBytes("EOF");

            byte[] checkMagicNumber = new byte[magicNumber.Length];
            byte[] checkEOFMarker = new byte[eofMarker.Length];

            Array.Copy(byteArrayIPSArray, 0, checkMagicNumber, 0, magicNumber.Length);
            Array.Copy(byteArrayIPSArray, byteArrayIPSArray.Length - eofMarker.Length, checkEOFMarker, 0, eofMarker.Length);

            if (!checkMagicNumber.SequenceEqual(magicNumber)) { return; }
            if (!checkEOFMarker.SequenceEqual(eofMarker)) { return; }

            // Start patching
            for (int i = magicNumber.Length; i < (byteArrayIPSArray.Length - eofMarker.Length); i++)
            {
                byte[] offset = new byte[3];
                byte[] length = new byte[2];

                // Get offset to insert
                Array.Copy(byteArrayIPSArray, i, offset, 0, offset.Length);
                if (BitConverter.IsLittleEndian) { Array.Reverse(offset); }
                int intOffset = BitConverter.ToInt32(offset, 0);
                i += offset.Length;

                // Get length of payload
                Array.Copy(byteArrayIPSArray, i, length, 0, length.Length);
                if (BitConverter.IsLittleEndian) { Array.Reverse(length); }
                i += length.Length;

                // Get payload
                int payloadLength = BitConverter.ToInt32(length, 0);
                byte[] payload = new byte[payloadLength];
                if (BitConverter.IsLittleEndian) { Array.Reverse(payload); }
                Array.Copy(byteArrayIPSArray, i, payload, 0, payloadLength);
                i += payloadLength;

                // Insert payload into SourceROM
                Buffer.BlockCopy(payload, 0, SourceROM, intOffset, payloadLength);
            }
        }
    }
}
