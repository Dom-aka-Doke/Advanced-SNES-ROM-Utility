using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Advanced_SNES_ROM_Utility
{
    class BPSPatch
    {
        public static byte[] Apply(SNESROM sourceROM, string bpsFilePath)
        {
            byte[] byteArrayBPSPatch = File.ReadAllBytes(bpsFilePath);
            ulong offsetBPSPatch = 0;
            ulong offsetSourceFile = 0;             // sourceRelativeOffset
            ulong offsetDestinationFile = 0;        // outputOffset

            // Check if BPS patch starts with magic number
            byte[] magicNumber = Encoding.ASCII.GetBytes("BPS1");
            byte[] checkMagicNumber = new byte[magicNumber.Length];
            Array.Copy(byteArrayBPSPatch, (int)offsetBPSPatch, checkMagicNumber, 0, magicNumber.Length);
            if (!checkMagicNumber.SequenceEqual(magicNumber)) { return null; }
            offsetBPSPatch += 4;

            // Get CRC32 checksums for validation
            byte[] crc32PatchFile = new byte[4];
            byte[] crc32SourceFile = new byte[4];
            byte[] crc32DestinationFile = new byte[4];

            Array.Copy(byteArrayBPSPatch, byteArrayBPSPatch.Length - crc32PatchFile.Length, crc32PatchFile, 0, crc32PatchFile.Length);
            Array.Copy(byteArrayBPSPatch, byteArrayBPSPatch.Length - (crc32SourceFile.Length + crc32DestinationFile.Length + crc32PatchFile.Length), crc32SourceFile, 0, crc32SourceFile.Length);
            Array.Copy(byteArrayBPSPatch, byteArrayBPSPatch.Length - (crc32DestinationFile.Length + crc32PatchFile.Length), crc32DestinationFile, 0, crc32DestinationFile.Length);
            if (BitConverter.IsLittleEndian) { Array.Reverse(crc32PatchFile); Array.Reverse(crc32SourceFile); Array.Reverse(crc32DestinationFile); }

            string internalHashPatchFile = BitConverter.ToString(crc32PatchFile).Replace("-", "");
            string internalHashSourceFile = BitConverter.ToString(crc32SourceFile).Replace("-", "");
            string internalHashDestinationFile = BitConverter.ToString(crc32DestinationFile).Replace("-", "");

            // Validate CRC32 of patch file
            Crc32 calcCRC32PatchFile = new Crc32();
            string calcHashPatchFile = null;

            foreach (byte singleByte in calcCRC32PatchFile.ComputeHash(byteArrayBPSPatch, 0, byteArrayBPSPatch.Length - crc32PatchFile.Length))
            {
                calcHashPatchFile += singleByte.ToString("X2").ToUpper();
            }

            if (internalHashPatchFile != calcHashPatchFile) { return null; }

            // Validate CRC32 of source file
            if (internalHashSourceFile != sourceROM.CRC32Hash) { return null; }

            // Get VWI information
            ulong vwiSourceFileLength = GetVWI(byteArrayBPSPatch, ref offsetBPSPatch);
            ulong vwiDestinationFileLength = GetVWI(byteArrayBPSPatch, ref offsetBPSPatch);
            ulong vwiMetadataLength = GetVWI(byteArrayBPSPatch, ref offsetBPSPatch);

            // Validate length of source file
            if ((ulong)(sourceROM.SourceROM.Length + sourceROM.UIntSMCHeader) != vwiSourceFileLength) { return null; }

            // Merge ROM and header, if header exists
            byte[] byteArraySourceROM = new byte[sourceROM.SourceROM.Length + sourceROM.UIntSMCHeader];

            if (sourceROM.SourceROMSMCHeader != null && sourceROM.UIntSMCHeader > 0)
            {
                Array.Copy(sourceROM.SourceROMSMCHeader, 0, byteArraySourceROM, 0, sourceROM.UIntSMCHeader);
                Array.Copy(sourceROM.SourceROM, 0, byteArraySourceROM, sourceROM.UIntSMCHeader, sourceROM.SourceROM.Length);
            }

            else
            {
                Array.Copy(sourceROM.SourceROMSMCHeader, 0, byteArraySourceROM, 0, sourceROM.SourceROM.Length);
            }

            // Create destination file for patching
            byte[] patchedSourceROM = new byte[vwiDestinationFileLength];
            foreach (byte b in patchedSourceROM) { patchedSourceROM[b] = 0x00; }

            // Generate patched file using VWI information
            while (offsetBPSPatch < (ulong)byteArrayBPSPatch.Length - 12)
            {
                ulong data = GetVWI(byteArrayBPSPatch, ref offsetBPSPatch);
                ulong command = data & 3;
                ulong length = (data >> 2) + 1;

                switch (command)
                {
                    case 0: SourceRead(ref patchedSourceROM, ref byteArraySourceROM, ref offsetDestinationFile, length); break;
/*                  case 1: TargetRead(ref patchedSourceROM, ref byteArraySourceROM, ref offsetDestinationFile, length); break;
                    case 2: SourceCopy(ref patchedSourceROM, ref byteArraySourceROM, ref offsetDestinationFile, length); break;
                    case 3: TargetCopy(ref patchedSourceROM, ref byteArraySourceROM, ref offsetDestinationFile, length); break;  */
                }
            }

            // Validate length of destination file
            if ((ulong)patchedSourceROM.Length != vwiDestinationFileLength) { return null; }

            // Verfiy CRC32 of destination file
            Crc32 calcCRC32DestinationFile = new Crc32();
            string calcHashDestinationFile = null;

            foreach (byte singleByte in calcCRC32DestinationFile.ComputeHash(patchedSourceROM))
            {
                calcHashDestinationFile += singleByte.ToString("X2").ToUpper();
            }

            if (internalHashDestinationFile != calcHashDestinationFile) { return null; }

            return patchedSourceROM;
        }

        private static ulong GetVWI(byte[] byteArrayBPSPatch, ref ulong offsetBPSPatch)
        {
            ulong data = 0;
            int shift = 1;
            byte x = byteArrayBPSPatch[offsetBPSPatch];
            data += (ulong)((x & 0x7F) * shift);

            while ((x & 0x80) == 0)
            {
                shift <<= 7;
                data += (ulong)shift;
                offsetBPSPatch++;
                x = byteArrayBPSPatch[offsetBPSPatch];
                data += (ulong)((x & 0x7F) * shift);
            }

            offsetBPSPatch++;
            return data;
        }

        private static void SourceRead(ref byte[] patchedSourceROM, ref byte[] byteArraySourceROM, ref ulong offsetDestinationFile, ulong length)
        {
            while (length-- >= 0)
            {
                patchedSourceROM[offsetDestinationFile] = byteArraySourceROM[offsetDestinationFile];
                offsetDestinationFile++;
            }
        }
        /*
        private static void TargetRead()
        {
            while (length--)
            {
                target[outputOffset++] = read();
            }
        }

        private static void SourceCopy()
        {
            uint64 data = decode();
            sourceRelativeOffset += (data & 1 ? -1 : +1) * (data >> 1);
            while (length--)
            {
                target[outputOffset++] = source[sourceRelativeOffset++];
            }
        }

        private static void TargetCopy()
        {
            uint64 data = decode();
            targetRelativeOffset += (data & 1 ? -1 : +1) * (data >> 1);
            while (length--)
            {
                target[outputOffset++] = target[targetRelativeOffset++];
            }
        }   */
    }
}