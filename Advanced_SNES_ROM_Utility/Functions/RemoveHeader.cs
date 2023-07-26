namespace Advanced_SNES_ROM_Utility.Functions
{
    public static partial class SNESROMFunction
    {
        public static void RemoveHeader(this SNESROM sourceROM)
        {
            // Remove existing header
            sourceROM.SourceROMSMCHeader = null;
            
            sourceROM.Initialize();
        }
    }
}