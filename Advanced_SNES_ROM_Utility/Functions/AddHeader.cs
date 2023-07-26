namespace Advanced_SNES_ROM_Utility.Functions
{
    public static partial class SNESROMFunction
    {
        public static void AddHeader(this SNESROM sourceROM)
        {
            // Create empty header
            sourceROM.SourceROMSMCHeader = new byte[512];

            foreach (byte singleByte in sourceROM.SourceROMSMCHeader)
            {
                sourceROM.SourceROMSMCHeader[singleByte] = 0x00;
            }

            sourceROM.Initialize();
        }
    }
}