using System;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        private void CalculateInverseChecksum()
        {
            UInt16 calcInvChksm = 0;
            byte[] byteInvChksm = new byte[2];

            // Calculate inverse checksum (XOR with 0xFFFF)
            calcInvChksm = BitConverter.ToUInt16(ByteArrayCalcChecksum, 0);
            calcInvChksm ^= 0xFFFF;

            // Return checksum as byte[]
            byteInvChksm = BitConverter.GetBytes(calcInvChksm);

            ByteArrayCalcInvChecksum = byteInvChksm;
        }
    }
}