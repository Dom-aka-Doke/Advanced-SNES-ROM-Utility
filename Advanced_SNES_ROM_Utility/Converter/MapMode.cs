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

            if (!sourceROM.IsInterleaved && (sourceROM.ByteMapMode == (byte)MapMode.lorom_1 || sourceROM.ByteMapMode == (byte)MapMode.lorom_2))
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

                    byte[] newByteMapMode = new byte[] { (byte)MapMode.hirom_1 };
                    int newHeaderMapModePosition = (int)HeaderOffset.hirom + (int)HeaderValue.mapmode;
                    Buffer.BlockCopy(newByteMapMode, 0, convertedSourceROM, newHeaderMapModePosition, 1);
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }

            // HiROM -> LoROM
            else if (!sourceROM.IsInterleaved && (sourceROM.ByteMapMode == (byte)MapMode.hirom_1 || sourceROM.ByteMapMode == (byte)MapMode.hirom_2))
            {
                try
                {
                    convertedSourceROM = new byte[sourceROM.SourceROM.Length / 2];

                    int chunkSize = 65536;
                    int chunks = (sourceROM.SourceROM.Length / chunkSize);

                    int newPos = 0;
                    int sourcePos = 0;

                    /*
                     Here comes code to convert HiROM to LoROM
                     */

                    byte[] newByteMapMode = new byte[] { (byte)MapMode.lorom_1 };
                    int newHeaderMapModePosition = (int)HeaderOffset.lorom + (int)HeaderValue.mapmode;
                    Buffer.BlockCopy(newByteMapMode, 0, convertedSourceROM, newHeaderMapModePosition, 1);
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return convertedSourceROM;
        }
    }
}