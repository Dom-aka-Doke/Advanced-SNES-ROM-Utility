using System;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        public void FixInternalROMSize()
        {
            if (IntROMSize < IntCalcFileSize)
            {
                IntROMSize = 1;

                if (!IsBSROM)
                {
                    byte[] byteArrayROMSizeValue = { 0x07 };

                    while (IntROMSize < IntCalcFileSize)
                    {
                        IntROMSize *= 2;
                        byteArrayROMSizeValue[0]++;
                    }

                    Buffer.BlockCopy(byteArrayROMSizeValue, 0, SourceROM, (int)UIntROMHeaderOffset + 0x27, 1);

                    if (UIntROMHeaderOffset == (int)HeaderOffset.exlorom || UIntROMHeaderOffset == (int)HeaderOffset.exhirom)
                    {
                        Buffer.BlockCopy(byteArrayROMSizeValue, 0, SourceROM, (int)(UIntROMHeaderOffset + 0x27 - 0x400000), 1);
                    }
                }

                else
                {
                    IntROMSize = IntCalcFileSize;

                    uint size = 1;
                    int sizeCtr = IntROMSize;

                    while (sizeCtr > 1)
                    {
                        size <<= 1;
                        size |= 1;
                        sizeCtr--;
                    }

                    byte[] byteArrayROMSizeValue = BitConverter.GetBytes(size);
                    Buffer.BlockCopy(byteArrayROMSizeValue, 0, SourceROM, (int)UIntROMHeaderOffset + 0x20, 4);
                }

                Initialize();
            }
        }
    }
}