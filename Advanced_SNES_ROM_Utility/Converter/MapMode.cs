using System;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        public byte[] ConvertMapMode(SNESROM sourceROM)
        {
            byte[] convertedSourceROM = null;

            // LoROM -> HiROM
            // https://github.com/danielburgess/LoHiROM

            if (!sourceROM.IsInterleaved && (sourceROM.ByteMapMode == 0x20 || sourceROM.ByteMapMode == 0x30))
            {
                try
                {
                    convertedSourceROM = new byte[sourceROM.SourceROM.Length * 2];

                    int chunkSize = 32768;
                    int chunks = (sourceROM.SourceROM.Length / chunkSize);

                    int newPos = 0;
                    int sourcePos = 0;

                    for (int chunkCtr = 0; chunkCtr < chunks; chunkCtr++)
                    {
                        for (int chunkByteCtr = 0; chunkByteCtr < chunkSize; chunkByteCtr++)
                        {
                            sourcePos = chunkByteCtr + (chunkCtr * chunkSize);
                            newPos = chunkByteCtr + (chunkCtr * 0x10000);
                            convertedSourceROM[newPos] = ((chunkCtr == 0) ? (byte)0xFF : sourceROM.SourceROM[sourcePos]);
                            convertedSourceROM[newPos + chunkSize] = sourceROM.SourceROM[sourcePos];
                        }
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }

            // HiROM -> LoROM
            else if (!sourceROM.IsInterleaved && (sourceROM.ByteMapMode == 0x21 || sourceROM.ByteMapMode == 0x31))
            {

            }

            return convertedSourceROM;
        }
    }
}