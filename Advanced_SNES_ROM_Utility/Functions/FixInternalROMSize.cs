using System;

namespace Advanced_SNES_ROM_Utility.Functions
{
    public static partial class SNESROMFunction
    {
        public static void FixInternalROMSize(this SNESROM sourceROM)
        {   
            if (sourceROM.IntROMSize < sourceROM.IntCalcFileSize || sourceROM.IntCalcFileSize <= (sourceROM.IntROMSize / 2))
            {
                sourceROM.IntROMSize = 1;

                if (!sourceROM.IsBSROM)
                {
                    byte[] byteArrayROMSizeValue = { 0x07 };

                    while (sourceROM.IntROMSize < sourceROM.IntCalcFileSize)
                    {
                        sourceROM.IntROMSize *= 2;
                        byteArrayROMSizeValue[0]++;
                    }

                    Buffer.BlockCopy(byteArrayROMSizeValue, 0, sourceROM.SourceROM, (int)sourceROM.UIntROMHeaderOffset + 0x27, 1);

                    if (sourceROM.UIntROMHeaderOffset == (int)HeaderOffset.exlorom || sourceROM.UIntROMHeaderOffset == (int)HeaderOffset.exhirom)
                    {
                        Buffer.BlockCopy(byteArrayROMSizeValue, 0, sourceROM.SourceROM, (int)(sourceROM.UIntROMHeaderOffset + 0x27 - 0x400000), 1);
                    }
                }

                else
                {
                    sourceROM.IntROMSize = sourceROM.IntCalcFileSize;

                    uint size = 1;
                    int sizeCtr = sourceROM.IntROMSize;

                    while (sizeCtr > 1)
                    {
                        size <<= 1;
                        size |= 1;
                        sizeCtr--;
                    }

                    byte[] byteArrayROMSizeValue = BitConverter.GetBytes(size);
                    Buffer.BlockCopy(byteArrayROMSizeValue, 0, sourceROM.SourceROM, (int)sourceROM.UIntROMHeaderOffset + 0x20, 4);
                }

                sourceROM.Initialize();
            }
        }
    }
}