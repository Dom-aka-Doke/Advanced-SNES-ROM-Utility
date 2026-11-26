using System;

namespace Advanced_SNES_ROM_Utility.Functions
{
    public static partial class SNESROMFunction
    {
        public static void FixInternalROMSize(this SNESROM sourceROM)
        {
            if (!sourceROM.NeedsInternalROMSizeFix()) { return; }

            if (!sourceROM.IsBSROM)
            {
                // Smallest size value: 0x05 = 32 kByte (256 Kbit), size = 2^n kByte
                byte[] byteArrayROMSizeValue = { 0x05 };
                sourceROM.IntROMSize = 0.25;

                while (sourceROM.IntROMSize < sourceROM.IntCalcFileSize)
                {
                    sourceROM.IntROMSize *= 2;
                    byteArrayROMSizeValue[0]++;
                }

                Buffer.BlockCopy(byteArrayROMSizeValue, 0, sourceROM.SourceROM, (int)sourceROM.UIntROMHeaderOffset + (int)HeaderValue.size, 1);

                if (sourceROM.IsExROMHeaderCopied)
                {
                    Buffer.BlockCopy(byteArrayROMSizeValue, 0, sourceROM.SourceROM, (int)(sourceROM.UIntROMHeaderCopyOffset + (int)HeaderValue.size), 1);
                }
            }

            else
            {
                sourceROM.IntROMSize = sourceROM.IntCalcFileSize;

                uint size = 1;
                int sizeCtr = (int)sourceROM.IntROMSize;

                while (sizeCtr > 1)
                {
                    size <<= 1;
                    size |= 1;
                    sizeCtr--;
                }

                byte[] byteArrayROMSizeValue = BitConverter.GetBytes(size);
                Buffer.BlockCopy(byteArrayROMSizeValue, 0, sourceROM.SourceROM, (int)sourceROM.UIntROMHeaderOffset + 0x20, 4);
            }

            sourceROM.Initialize(new string[] { "romheader", "romsize", "filesize", "checksum" });
        }
    }
}
