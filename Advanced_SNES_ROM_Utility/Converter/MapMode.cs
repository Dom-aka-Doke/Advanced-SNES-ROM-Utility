using System;

namespace Advanced_SNES_ROM_Utility.Converter
{
    public static partial class SNESROMConvert
    {
        public static void ConvertMapMode(this SNESROM sourceROM)
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
                            newPos = chunkByteCtr + (chunkCtr * 65536);
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
            // https://github.com/pimaster/LoHiROM/blob/Allow-High-to-Low-conversion

            else if (!sourceROM.IsInterleaved && (sourceROM.ByteMapMode == (byte)MapMode.hirom_1 || sourceROM.ByteMapMode == (byte)MapMode.hirom_2))
            {
                try
                {
                    convertedSourceROM = new byte[sourceROM.SourceROM.Length / 2];

                    int chunkSize = 65536;
                    int chunks = (sourceROM.SourceROM.Length / chunkSize);

                    int newPos = 0;
                    int sourcePos = 0;

                    for (int chunkCtr = 0; chunkCtr < chunks; chunkCtr++)
                    {
                        for (int chunkByteCtr = 0; chunkByteCtr < 32768; chunkByteCtr++)
                        {
                            sourcePos = ((chunkCtr == 0) ? chunkByteCtr + (chunkCtr * chunkSize) + 32768 : chunkByteCtr + (chunkCtr * chunkSize));
                            newPos = chunkByteCtr + (chunkCtr * 32768);
                            convertedSourceROM[newPos] = sourceROM.SourceROM[sourcePos];
                        }
                    }

                    byte[] newByteMapMode = new byte[] { (byte)MapMode.lorom_1 };
                    int newHeaderMapModePosition = (int)HeaderOffset.lorom + (int)HeaderValue.mapmode;
                    Buffer.BlockCopy(newByteMapMode, 0, convertedSourceROM, newHeaderMapModePosition, 1);
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }

            sourceROM.SourceROM = convertedSourceROM;
            sourceROM.Initialize();
        }
    }
}