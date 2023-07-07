namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        private void CalculateFileSize()
        {
            int filesize = (SourceROM.Length * 8) / 1048576;
            IntCalcFileSize = filesize;
        }
    }
}