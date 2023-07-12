using System;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        public static int CalculateMapModeScore(byte[] sourceROM, uint offset, bool isBSROM)
        {
            // Check if ROM has normal or extended size
            if (sourceROM.Length < offset + 0x50)
            {
                return -100;
            }

            // Define score variable
            int score = 0;

            // Get all necessary values from ROM
            byte[] resetVector = new byte[2];
            Buffer.BlockCopy(sourceROM, (int)offset + 0x4C, resetVector, 0, 2);

            byte[] checksum = new byte[2];
            Buffer.BlockCopy(sourceROM, (int)offset + 0x2E, checksum, 0, 2);

            byte[] checksumInverse = new byte[2];
            Buffer.BlockCopy(sourceROM, (int)offset + 0x2C, checksumInverse, 0, 2);

            byte[] mapMode = new byte[1];
            if (isBSROM) { Buffer.BlockCopy(sourceROM, (int)offset + 0x28, mapMode, 0, 1); } else { Buffer.BlockCopy(sourceROM, (int)offset + 0x25, mapMode, 0, 1); }

            // Bitmask this byte, because non relevant bits are not clearly defined
            mapMode[0] &= 0x37;

            byte[] romSize = new byte[1];
            Buffer.BlockCopy(sourceROM, (int)offset + 0x27, romSize, 0, 1);

            byte[] destinationCode = new byte[1];
            Buffer.BlockCopy(sourceROM, (int)offset + 0x29, destinationCode, 0, 1);

            byte[] fixedValue33 = new byte[1];
            Buffer.BlockCopy(sourceROM, (int)offset + 0x2A, fixedValue33, 0, 1);

            byte[] fixedValue00 = new byte[7];
            Buffer.BlockCopy(sourceROM, (int)offset + 0x06, fixedValue00, 0, 7);

            byte[] opCode = new byte[1];
            Buffer.BlockCopy(sourceROM, BitConverter.ToUInt16(resetVector, 0), opCode, 0, 1);

            // Check if reset vector is pointing to an address greater than or equal to 0x8000
            if (BitConverter.ToUInt16(resetVector, 0) < 0x8000)
            {
                return -100;
            }

            // Some values must be in big endian
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(resetVector);
                Array.Reverse(opCode);
                Array.Reverse(checksum);
                Array.Reverse(checksumInverse);
            }

            // Check opcodes

            // Most likely opcodes
            if (opCode[0] == 0x78  //sei
            || opCode[0] == 0x18  //clc (clc; xce)
            || opCode[0] == 0x38  //sec (sec; xce)
            || opCode[0] == 0x9c  //stz $nnnn (stz $4200)
            || opCode[0] == 0x4c  //jmp $nnnn
            || opCode[0] == 0x5c  //jml $nnnnnn
            )
            {
                score += 8;
            }

            // Plausible opcodes
            if (opCode[0] == 0xc2  //rep #$nn
            || opCode[0] == 0xe2  //sep #$nn
            || opCode[0] == 0xad  //lda $nnnn
            || opCode[0] == 0xae  //ldx $nnnn
            || opCode[0] == 0xac  //ldy $nnnn
            || opCode[0] == 0xaf  //lda $nnnnnn
            || opCode[0] == 0xa9  //lda #$nn
            || opCode[0] == 0xa2  //ldx #$nn
            || opCode[0] == 0xa0  //ldy #$nn
            || opCode[0] == 0x20  //jsr $nnnn
            || opCode[0] == 0x22  //jsl $nnnnnn
            )
            {
                score += 4;
            }

            // Implausible opcodes
            if (opCode[0] == 0x40  //rti
            || opCode[0] == 0x60  //rts
            || opCode[0] == 0x6b  //rtl
            || opCode[0] == 0xcd  //cmp $nnnn
            || opCode[0] == 0xec  //cpx $nnnn
            || opCode[0] == 0xcc  //cpy $nnnn
            )
            {
                score -= 4;
            }

            // Least likely opcodes
            if (opCode[0] == 0x00  //brk #$nn
            || opCode[0] == 0x02  //cop #$nn
            || opCode[0] == 0xdb  //stp
            || opCode[0] == 0x42  //wdm
            || opCode[0] == 0xff  //sbc $nnnnnn,x
            )
            {
                score -= 8;
            }

            // Check if checksums add up to 0xFFFF
            if (BitConverter.ToUInt16(checksum, 0) + BitConverter.ToUInt16(checksumInverse, 0) == 0xFFFF)
            {
                score += 4;
            }

            // Check if internal ROM type is valid
            // LoROM
            if (offset == (int)HeaderOffset.lorom && (mapMode[0] == (int)MapMode.lorom_1 || mapMode[0] == (int)MapMode.lorom_2))
            {
                score += 2;
            }
            // HiROM
            if (offset == (int)HeaderOffset.hirom && (mapMode[0] == (int)MapMode.hirom_1 || mapMode[0] == (int)MapMode.hirom_2 || mapMode[0] == (int)MapMode.hirom_spc7110))
            {
                score += 2;
            }
            // ExLoROM - f.e. Star Ocean
            if (offset == (int)HeaderOffset.exlorom && mapMode[0] == (int)MapMode.exlorom)
            {
                score += 4;
            }
            // ExHiROM - f.e. Tales Of Phantasia
            if (offset == (int)HeaderOffset.exhirom && (mapMode[0] == (int)MapMode.exhirom_1 || mapMode[0] == (int)MapMode.exhirom_2))
            {
                score += 4;
            }

            // Check if internal ROM size is vaild
            if (romSize[0] >= 0x07 && romSize[0] <= 0x0D)
            {
                score += 2;
            }

            // Check if destination code size is vaild
            if (destinationCode[0] <= 0x14)
            {
                score += 2;
            }

            // Check for some fixed ROM values
            if (fixedValue33[0] == 0x33)
            {
                score += 4;
            }

            if (fixedValue00[0] == 0x00 &&
                fixedValue00[1] == 0x00 &&
                fixedValue00[2] == 0x00 &&
                fixedValue00[3] == 0x00 &&
                fixedValue00[4] == 0x00 &&
                fixedValue00[5] == 0x00 &&
                fixedValue00[6] == 0x00)
            {
                score += 2;
            }

            // If game is tested for being BS-X
            if (isBSROM)
            {
                // If title starts with 'BS' like most of them do
                byte[] titleBS = new byte[2];
                Buffer.BlockCopy(sourceROM, (int)offset + 0x10, titleBS, 0, 2);

                if (titleBS[0] == 0x42 && titleBS[1] == 0x53)
                {
                    score += 2;
                }
            }

            // Return score
            return score;
        }
    }
}