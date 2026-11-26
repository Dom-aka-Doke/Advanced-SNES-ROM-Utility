namespace Advanced_SNES_ROM_Utility.Functions
{
    public static partial class SNESROMFunction
    {
        /// <summary>
        /// File size in Mbit. From 1 Mbit on whole Mbit (as before), below 1 Mbit the exact
        /// fraction (f.e. 0.25 for 32 kByte homebrew / test ROMs).
        /// </summary>
        public static double CalculateFileSize(byte[] sourceROM)
        {
            double mbit = (sourceROM.Length * 8.0) / 1048576;
            return mbit >= 1 ? System.Math.Floor(mbit) : mbit;
        }
    }
}
