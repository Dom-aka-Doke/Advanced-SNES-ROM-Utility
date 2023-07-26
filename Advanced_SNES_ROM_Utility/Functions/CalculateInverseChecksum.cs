using System;

namespace Advanced_SNES_ROM_Utility.Functions
{
    public static partial class SNESROMFunction
    {
        public static byte[] CalculateInverseChecksum(byte[] byteArrayCalcChecksum)
        {
            UInt16 calcInvChksm = 0;
            byte[] byteInvChksm = new byte[2];

            // Calculate inverse checksum (XOR with 0xFFFF)
            calcInvChksm = BitConverter.ToUInt16(byteArrayCalcChecksum, 0);
            calcInvChksm ^= 0xFFFF;

            // Return checksum as byte[]
            byteInvChksm = BitConverter.GetBytes(calcInvChksm);

            return byteInvChksm;
        }
    }
}