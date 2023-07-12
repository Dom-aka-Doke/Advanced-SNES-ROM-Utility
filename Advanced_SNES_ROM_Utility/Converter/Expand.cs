using System;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        public void Expand(int sizeExpandedROM, bool mirror)
        {
            byte[] expandedROM = new byte[sizeExpandedROM * 131072];

            foreach (byte singleByte in expandedROM) { expandedROM[singleByte] = 0x00; }

            // Mirror ROM if option is selected (only up to 32 Mbit possible)
            if (mirror && sizeExpandedROM <= 32)
            {
                byte[] mirroredROM = Mirror(SourceROM);
                int i = 0;

                while (i < expandedROM.Length)
                {
                    Buffer.BlockCopy(mirroredROM, 0, expandedROM, i, mirroredROM.Length);
                    i += mirroredROM.Length;
                }
            }

            else
            {
                Buffer.BlockCopy(SourceROM, 0, expandedROM, 0, SourceROM.Length);
            }

            // If expanding a non ExROM to ExROM
            if (sizeExpandedROM >= 48 && IntCalcFileSize <= 32 && !mirror)
            {
                if (StringMapMode.StartsWith("LoROM"))
                {
                    //if (sourceROM.ByteMapMode >= 0x30)
                    //{

                    if (ByteMapMode >= 0x30)          // <-- remove this if block when using uncommented way!
                    {
                        byte[] newMapMode = new byte[1] { 0x32 };
                        Buffer.BlockCopy(newMapMode, 0, expandedROM, (int)UIntROMHeaderOffset + 0x25, 1);
                    }

                    Buffer.BlockCopy(expandedROM, 0, expandedROM, 0x400000, 0x8000);

                    //}
                    /*
                    else
                    {
                        int size = 0x400000;
                        if (sizeExpandedROM == 48) { size = 0x200000; }
                        Buffer.BlockCopy(expandedROM, 0, expandedROM, 0x400000, size);

                        int fillZeroLength = 0x3F8000;
                        if (sizeExpandedROM == 48) { fillZeroLength = 0x1F8000; }
                        byte[] fillZeroByte = new byte[fillZeroLength];
                        foreach (byte singleByte in fillZeroByte) { fillZeroByte[singleByte] = 0x00; }

                        Buffer.BlockCopy(fillZeroByte, 0, expandedROM, 0x8000, fillZeroByte.Length);
                    }*/
                }

                else if (StringMapMode.StartsWith("HiROM"))
                {
                    byte[] newMapMode = new byte[1];
                    newMapMode[0] = (byte)(ByteROMSpeed | 0x05);
                    Buffer.BlockCopy(newMapMode, 0, expandedROM, (int)UIntROMHeaderOffset + 0x25, 1);
                    Buffer.BlockCopy(expandedROM, 0x8000, expandedROM, 0x408000, 0x8000);
                }
            }

            // If expanding a non ExROM to ExROM using mirroring
            if (sizeExpandedROM >= 48 && IntCalcFileSize <= 32 && mirror)
            {
                if (StringMapMode.Contains("LoROM"))
                {
                    if (ByteMapMode >= 0x30)
                    {
                        byte[] newMapMode = new byte[1] { 0x32 };
                        Buffer.BlockCopy(newMapMode, 0, expandedROM, (int)UIntROMHeaderOffset + 0x25, 1);
                    }

                    int size = 0x400000;
                    if (sizeExpandedROM == 48) { size = 0x200000; }
                    Buffer.BlockCopy(expandedROM, 0, expandedROM, 0x400000, size);
                }

                else if (StringMapMode.Contains("HiROM"))
                {
                    byte[] newMapMode = new byte[1];
                    newMapMode[0] = (byte)(ByteROMSpeed | 0x05);
                    Buffer.BlockCopy(newMapMode, 0, expandedROM, (int)UIntROMHeaderOffset + 0x25, 1);

                    int offset = 0x408000;

                    while (offset < expandedROM.Length)
                    {
                        int firstHalfLocation = offset - 0x400000;
                        Buffer.BlockCopy(expandedROM, firstHalfLocation, expandedROM, offset, 0x8000);
                        offset += 0x10000;
                    }
                }
            }

            SourceROM = expandedROM;

            Initialize();
        }
    }
}