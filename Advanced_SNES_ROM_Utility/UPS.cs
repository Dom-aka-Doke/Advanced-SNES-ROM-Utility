using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        public byte[] ApplyUPSPatch(string upsFilePath)
        {
            byte[] byteArrayUPSPatch = File.ReadAllBytes(upsFilePath);
            ulong offset = 0;

            // UPS patches muste be at least 19 bytes
            if (byteArrayUPSPatch.Length < 19) { return null; }

            // Check if UPS patch starts with magic number
            byte[] magicNumber = Encoding.ASCII.GetBytes("UPS1");
            byte[] checkMagicNumber = new byte[magicNumber.Length];
            Array.Copy(byteArrayUPSPatch, (int)offset, checkMagicNumber, 0, magicNumber.Length);
            if (!checkMagicNumber.SequenceEqual(magicNumber)) { return null; }
            offset += 4;

            // Initialize variables for validations
            byte[] crc32PatchFile = new byte[4];
            byte[] crc32SourceFile = new byte[4];
            byte[] crc32DestinationFile = new byte[4];

            // Verify CRC32 of patch file
            Array.Copy(byteArrayUPSPatch, byteArrayUPSPatch.Length - crc32PatchFile.Length, crc32PatchFile, 0, crc32PatchFile.Length);
            if (!BitConverter.IsLittleEndian) { Array.Reverse(crc32PatchFile); }
            string internalHashPatchFile = BitConverter.ToString(crc32PatchFile).Replace("-", "");

            Crc32 calcCRC32PatchFile = new Crc32();
            string calcHashPatchFile = null;

            foreach (byte singleByte in calcCRC32PatchFile.ComputeHash(byteArrayUPSPatch, 0, byteArrayUPSPatch.Length - crc32PatchFile.Length))
            {
                calcHashPatchFile += singleByte.ToString("X2").ToUpper();
            }

            if (internalHashPatchFile != calcHashPatchFile) { return null; }

            // Verify size of source file
            ulong vwiSourceFileLength = GetVWI(byteArrayUPSPatch, ref offset);
            if ((ulong)SourceROM.Length != vwiSourceFileLength) { return null; }

            // Verify CRC32 of source file
            Array.Copy(byteArrayUPSPatch, byteArrayUPSPatch.Length - (crc32SourceFile.Length + crc32DestinationFile.Length + crc32PatchFile.Length), crc32SourceFile, 0, crc32SourceFile.Length);
            if (!BitConverter.IsLittleEndian) { Array.Reverse(crc32SourceFile); }
            string internalHashSourceFile = BitConverter.ToString(crc32PatchFile).Replace("-", "");

            if (internalHashSourceFile != CRC32Hash) { return null; }

            // Create destination file for patching
            ulong vwiDestinationFileLength = GetVWI(byteArrayUPSPatch, ref offset);
            byte[] patchedSourceROM = new byte[vwiDestinationFileLength];
            foreach (byte b in patchedSourceROM) { patchedSourceROM[b] = 0x00; }

            // Verfiy destination file size
            Array.Copy(byteArrayUPSPatch, byteArrayUPSPatch.Length - (crc32DestinationFile.Length + crc32PatchFile.Length), crc32DestinationFile, 0, crc32DestinationFile.Length);

            // Verfiy CRC32 of destination file
            return null;
        }

        private ulong GetVWI(byte[] byteArrayUPSArray, ref ulong offset)
        {
            ulong value = 0;
            int shift = 1;
            offset++;
            byte x = byteArrayUPSArray[offset];
            value += (ulong)((x & 0x7F) * shift);

            while ((x & 0x80) == 0)
            {
                shift <<= 7;
                value += (ulong)shift;
                offset++;
                x = byteArrayUPSArray[offset];
                value += (ulong)((x & 0x7F) * shift);
            }

            offset++;
            
            return value;
        }
    }
}