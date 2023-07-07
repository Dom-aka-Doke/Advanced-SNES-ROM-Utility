namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        public void RemoveHeader()
        {
            // Remove existing header
            SourceROMSMCHeader = null;

            Initialize();
        }
    }
}