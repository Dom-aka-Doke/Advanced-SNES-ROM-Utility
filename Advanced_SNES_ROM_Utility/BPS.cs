using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Advanced_SNES_ROM_Utility
{
    class BPSPatch
    {
        public static byte[] Apply(byte[] mergedSourceROM, string crc32Hash, string bpsFilePath)
        {
            byte[] byteArrayBPSPatch = File.ReadAllBytes(bpsFilePath);
            ulong offsetBPSPatch = 0;
            ulong offsetSourceFile = 0;
            ulong offsetDestinationFile = 0;

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
            if (internalHashSourceFile != crc32Hash) { return null; }

            // Get VWI information
            ulong vwiSourceFileLength = GetVWI(ref byteArrayBPSPatch, ref offsetBPSPatch);
            ulong vwiDestinationFileLength = GetVWI(ref byteArrayBPSPatch, ref offsetBPSPatch);
            ulong vwiMetadataLength = GetVWI(ref byteArrayBPSPatch, ref offsetBPSPatch);

            // Validate length of source file
            if ((ulong)(mergedSourceROM.Length) != vwiSourceFileLength) { return null; }

            // Create destination file for patching
            byte[] patchedSourceROM = new byte[vwiDestinationFileLength];

            // Generate patched file using VWI information
            while (offsetBPSPatch < (ulong)byteArrayBPSPatch.Length - 12)
            {
                ulong data = GetVWI(ref byteArrayBPSPatch, ref offsetBPSPatch);
                ulong command = data & 3;
                ulong length = (data >> 2) + 1;

                try
                {
                    switch (command)
                    {
                        case 0: SourceRead(ref mergedSourceROM, ref patchedSourceROM, ref offsetSourceFile, ref offsetDestinationFile, length); break;
                        case 1: TargetRead(ref byteArrayBPSPatch, ref patchedSourceROM, ref offsetBPSPatch, ref offsetDestinationFile, length); break;
                        case 2: SourceCopy(ref byteArrayBPSPatch, ref mergedSourceROM, ref patchedSourceROM, ref offsetBPSPatch, ref offsetSourceFile, ref offsetDestinationFile); break;
                        case 3: TargetCopy(ref byteArrayBPSPatch, ref patchedSourceROM, ref offsetBPSPatch, ref offsetSourceFile, ref offsetDestinationFile); break;
                        default: return null;
                    }
                }

                catch (Exception ex)
                {
                    File.WriteAllBytes(@"C:\Temp\test.smc", patchedSourceROM);
                    return null;
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

        private static void SourceRead(ref byte[] mergedSourceROM, ref byte[] patchedSourceROM, ref ulong offsetSourceFile, ref ulong offsetDestinationFile, ulong length)
        {
            Array.Copy(mergedSourceROM, (int)offsetSourceFile, patchedSourceROM, (int)offsetDestinationFile, (int)length);
            offsetSourceFile += length;
            offsetDestinationFile += length;
        }

        private static void TargetRead(ref byte[] byteArrayBPSPatch, ref byte[] patchedSourceROM, ref ulong offsetBPSPatch, ref ulong offsetDestinationFile, ulong length)
        {
            Array.Copy(byteArrayBPSPatch, (int)offsetBPSPatch, patchedSourceROM, (int)offsetDestinationFile, (int)length);
            offsetBPSPatch += length;
            offsetDestinationFile += length;
        }

        private static void SourceCopy(ref byte[] byteArrayBPSPatch, ref byte[] mergedSourceROM, ref byte[] patchedSourceROM, ref ulong offsetBPSPatch, ref ulong offsetSourceFile, ref ulong offsetDestinationFile)
        {
            long data = (long)GetVWI(ref byteArrayBPSPatch, ref offsetBPSPatch);
            long sourceRelativeLength = (data >> 2) + 1;
            long dataAndOne = data & 1;
            int minusOrPlusOne = 1;
            if (dataAndOne == 1) { minusOrPlusOne = -1; }
            long sourceRelativeOffset = (int)offsetSourceFile + (minusOrPlusOne * (data >> 1));
            
            Array.Copy(mergedSourceROM, (int)sourceRelativeOffset, patchedSourceROM, (int)offsetDestinationFile, (int)sourceRelativeLength);
            offsetDestinationFile += (ulong)sourceRelativeLength;
            offsetSourceFile += (ulong)sourceRelativeLength;
        }

        private static void TargetCopy(ref byte[] byteArrayBPSPatch, ref byte[] patchedSourceROM, ref ulong offsetBPSPatch, ref ulong offsetSourceFile, ref ulong offsetDestinationFile)
        {
            long data = (long)GetVWI(ref byteArrayBPSPatch, ref offsetBPSPatch);
            long targetRelativeLength = (data >> 2) + 1;
            long dataAndOne = data & 1;
            int minusOrPlusOne = 1;
            if (dataAndOne == 1) { minusOrPlusOne = -1; }
            long targetRelativeOffset = (int)offsetDestinationFile + (minusOrPlusOne * (data >> 1));

            Array.Copy(patchedSourceROM, (int)targetRelativeOffset, patchedSourceROM, (int)offsetDestinationFile, (int)targetRelativeLength);
            offsetDestinationFile += (ulong)targetRelativeLength;
            offsetSourceFile += (ulong)targetRelativeLength;
        }

        private static ulong GetVWI(ref byte[] byteArrayBPSPatch, ref ulong offsetBPSPatch)
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
    }
}