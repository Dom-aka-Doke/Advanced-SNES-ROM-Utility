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
            ulong offsetUPSPatch = 0;
            ulong offsetSourceFile = 0;

            // UPS patches muste be at least 19 bytes
            if (byteArrayUPSPatch.Length < 19) { return null; }

            // Check if UPS patch starts with magic number
            byte[] magicNumber = Encoding.ASCII.GetBytes("UPS1");
            byte[] checkMagicNumber = new byte[magicNumber.Length];
            Array.Copy(byteArrayUPSPatch, (int)offsetUPSPatch, checkMagicNumber, 0, magicNumber.Length);
            if (!checkMagicNumber.SequenceEqual(magicNumber)) { return null; }
            offsetUPSPatch += 4;

            // Initialize variables for validations
            byte[] crc32PatchFile = new byte[4];
            byte[] crc32SourceFile = new byte[4];
            byte[] crc32DestinationFile = new byte[4];

            // Verify CRC32 of patch file
            Array.Copy(byteArrayUPSPatch, byteArrayUPSPatch.Length - crc32PatchFile.Length, crc32PatchFile, 0, crc32PatchFile.Length);
            if (BitConverter.IsLittleEndian) { Array.Reverse(crc32PatchFile); }
            string internalHashPatchFile = BitConverter.ToString(crc32PatchFile).Replace("-", "");

            Crc32 calcCRC32PatchFile = new Crc32();
            string calcHashPatchFile = null;

            foreach (byte singleByte in calcCRC32PatchFile.ComputeHash(byteArrayUPSPatch, 0, byteArrayUPSPatch.Length - crc32PatchFile.Length))
            {
                calcHashPatchFile += singleByte.ToString("X2").ToUpper();
            }

            if (internalHashPatchFile != calcHashPatchFile) { return null; }

            // Verify size of source file
            ulong vwiSourceFileLength = GetVWI(byteArrayUPSPatch, ref offsetUPSPatch);
            ulong vwiDestinationFileLength = GetVWI(byteArrayUPSPatch, ref offsetUPSPatch);
            if ((ulong)(SourceROM.Length + UIntSMCHeader) != vwiSourceFileLength && (ulong)(SourceROM.Length + UIntSMCHeader) != vwiDestinationFileLength) { return null; }

            // Verify CRC32 of source file
            Array.Copy(byteArrayUPSPatch, byteArrayUPSPatch.Length - (crc32SourceFile.Length + crc32DestinationFile.Length + crc32PatchFile.Length), crc32SourceFile, 0, crc32SourceFile.Length);
            Array.Copy(byteArrayUPSPatch, byteArrayUPSPatch.Length - (crc32DestinationFile.Length + crc32PatchFile.Length), crc32DestinationFile, 0, crc32DestinationFile.Length);
            if (BitConverter.IsLittleEndian) { Array.Reverse(crc32SourceFile); Array.Reverse(crc32DestinationFile); }
            string internalHashSourceFile = BitConverter.ToString(crc32SourceFile).Replace("-", "");
            string internalHashDestinationFile = BitConverter.ToString(crc32DestinationFile).Replace("-", "");

            if (internalHashSourceFile != CRC32Hash && internalHashDestinationFile != CRC32Hash) { return null; }

            // Create destination file for patching
            byte[] patchedSourceROM = null;
            
            if (internalHashSourceFile == CRC32Hash) { patchedSourceROM = new byte[vwiDestinationFileLength]; }
            else if (internalHashDestinationFile == CRC32Hash) { patchedSourceROM = new byte[vwiSourceFileLength]; }

            foreach (byte b in patchedSourceROM) { patchedSourceROM[b] = 0x00; }

            if (SourceROMSMCHeader != null && UIntSMCHeader > 0) { Array.Copy(SourceROMSMCHeader, 0, patchedSourceROM, 0, SourceROMSMCHeader.Length); }
            if (patchedSourceROM.Length <= SourceROM.Length + UIntSMCHeader) { Array.Copy(SourceROM, 0, patchedSourceROM, UIntSMCHeader, patchedSourceROM.Length - UIntSMCHeader); }
            else if (patchedSourceROM.Length > SourceROM.Length + UIntSMCHeader) { Array.Copy(SourceROM, 0, patchedSourceROM, UIntSMCHeader, SourceROM.Length); }

            // Generate patched file using VWI information
            while (offsetUPSPatch < (ulong)(byteArrayUPSPatch.Length - (crc32SourceFile.Length + crc32DestinationFile.Length + crc32PatchFile.Length)))
            {
                ulong bytesToSkip = GetVWI(byteArrayUPSPatch, ref offsetUPSPatch);
                offsetSourceFile += bytesToSkip;

                while (byteArrayUPSPatch[offsetUPSPatch] != 0x00 && offsetSourceFile < (ulong)patchedSourceROM.Length)
                {
                    patchedSourceROM[offsetSourceFile] = (byte)(byteArrayUPSPatch[offsetUPSPatch] ^ patchedSourceROM[offsetSourceFile]);
                    offsetSourceFile++;
                    offsetUPSPatch++;
                }

                offsetSourceFile++;
                offsetUPSPatch++;
            }

            // Verify size of destination file
            if ((ulong)patchedSourceROM.Length != vwiDestinationFileLength && (ulong)patchedSourceROM.Length != vwiSourceFileLength) { return null; }

            // Verfiy CRC32 of destination file
            Crc32 calcCRC32DestinationFile = new Crc32();
            string calcHashDestinationFile = null;

            foreach (byte singleByte in calcCRC32DestinationFile.ComputeHash(patchedSourceROM))
            {
                calcHashDestinationFile += singleByte.ToString("X2").ToUpper();
            }

            if (internalHashDestinationFile != calcHashDestinationFile && internalHashDestinationFile != CRC32Hash) { return null; }

            return patchedSourceROM;
        }

        private ulong GetVWI(byte[] byteArrayUPSPatch, ref ulong offsetUPSPatch)
        {
            ulong data = 0;
            int shift = 1;
            byte x = byteArrayUPSPatch[offsetUPSPatch];
            data += (ulong)((x & 0x7F) * shift);

            while ((x & 0x80) == 0)
            {
                shift <<= 7;
                data += (ulong)shift;
                offsetUPSPatch++;
                x = byteArrayUPSPatch[offsetUPSPatch];
                data += (ulong)((x & 0x7F) * shift);
            }

            offsetUPSPatch++;
            return data;
        }
    }
}