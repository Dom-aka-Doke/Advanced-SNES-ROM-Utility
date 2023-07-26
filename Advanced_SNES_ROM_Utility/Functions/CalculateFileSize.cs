namespace Advanced_SNES_ROM_Utility.Functions
{
    public static partial class SNESROMFunction
    {
        public static int CalculateFileSize(byte[] sourceROM)
        {
            return (sourceROM.Length * 8) / 1048576;
        }
    }
}