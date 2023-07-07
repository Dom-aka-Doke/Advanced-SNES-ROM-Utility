using System;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        public byte[] Mirror(byte[] sourceROM)
        {
            byte[] mirroredROM = new byte[(IntROMSize * 131072)];

            int ctr = 0;
            int romSize1 = IntROMSize;

            // Get size of ROM #1
            do
            {
                ctr = romSize1 / IntCalcFileSize;
                romSize1 /= 2;
            }

            while (ctr > 1);

            // Get size of ROM #2 and its multiplier
            int romSize2 = IntCalcFileSize - romSize1;
            int romRest = IntROMSize - romSize1;
            int romSize2Multiplicator = romRest / romSize2;

            // Mirror ROM
            Buffer.BlockCopy(sourceROM, 0, mirroredROM, 0, romSize1 * 131072);

            for (int i = 0; i < romSize2Multiplicator; i++)
            {
                Buffer.BlockCopy(sourceROM, romSize1 * 131072, mirroredROM, (romSize1 * 131072) + (i * romSize2 * 131072), romSize2 * 131072);
            }

            return mirroredROM;
        }
    }
}