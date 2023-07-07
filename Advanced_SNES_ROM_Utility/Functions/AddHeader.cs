namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        public void AddHeader()
        {
            // Create empty header
            SourceROMSMCHeader = new byte[512];

            foreach (byte singleByte in SourceROMSMCHeader)
            {
                SourceROMSMCHeader[singleByte] = 0x00;
            }

            Initialize();
        }
    }
}