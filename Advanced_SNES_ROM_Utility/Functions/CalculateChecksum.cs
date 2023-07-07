using System;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        private void CalculateChecksum()
        {
            uint offsetChksm = UIntROMHeaderOffset + 0x2C;
            ulong checksum = 0;
            uint multiplier = 1;
            byte[] byteChksm = new byte[2];

            // For clearing out checksum - checksum = 0x0000 , inverse = 0xFFFF
            byte[] clearSum = new byte[4] { 0xFF, 0xFF, 0x00, 0x00 };

            // Make a copy with clean checksum of the ROM for independent calculation
            byte[] noChecksumSourceROM = new byte[SourceROM.Length];
            Buffer.BlockCopy(SourceROM, 0, noChecksumSourceROM, 0, SourceROM.Length);
            Buffer.BlockCopy(clearSum, 0, noChecksumSourceROM, (int)offsetChksm, clearSum.Length);

            // For BS-X ROMs fill header with 0x00
            if (IsBSROM)
            {
                byte[] clearBSHeader = new byte[48] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                       0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                       0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                Buffer.BlockCopy(clearBSHeader, 0, noChecksumSourceROM, (int)UIntROMHeaderOffset, clearBSHeader.Length);
            }

            // Mirror ROM if neccessary | not working for Momotarou Dentetsu Happy and Tengai Makyou Zero / Tengai Makyou Zero - Shounen Jump no Shou (3 MByte ROMs with Special Chip + RAM + SRAM)
            if ((IntROMSize > IntCalcFileSize) && ByteROMType != 0xF5 && ByteROMType != 0xF9)
            {
                // Get mirrored ROM
                noChecksumSourceROM = Mirror(noChecksumSourceROM);
            }

            // Momotarou Dentetsu Happy fix | This didn't work with expanded ROM: else if (calcFileSize == 24 && romType == 0xF5)
            else if (IntROMSize == 32 && ByteROMType == 0xF5)
            {
                multiplier = 2;
            }

            // Calculate checksum
            foreach (byte singleByte in noChecksumSourceROM)
            {
                checksum += singleByte;
            }

            // Because of Momotarou Dentetsu Happy... again...
            checksum *= multiplier;

            // Take only first 16 bit from checksum
            checksum &= 0xFFFF;
            UInt16 checksum16 = (UInt16)checksum;

            // Return checksum as big endian byte[]
            byteChksm = BitConverter.GetBytes(checksum16);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteChksm);
            }

            ByteArrayCalcChecksum = byteChksm;
        }
    }
}