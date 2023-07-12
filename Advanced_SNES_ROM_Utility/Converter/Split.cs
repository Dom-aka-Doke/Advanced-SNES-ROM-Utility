using System;
using System.IO;
using System.Windows.Forms;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        public void Split(int splitROMSize)
        {
            int romChunks = SourceROM.Length / (splitROMSize * 131072);

            for (int index = 0; index < romChunks; index++)
            {
                string romChunkName = ROMName + "_[" + index + "]";
                byte[] splitROM = new byte[splitROMSize * 131072];

                Buffer.BlockCopy(SourceROM, index * (splitROMSize * 131072), splitROM, 0, splitROMSize * 131072);

                // Save file split
                File.WriteAllBytes(ROMFolder + @"\" + romChunkName + "_[split]" + ".bin", splitROM);
                MessageBox.Show("ROM successfully splittet!\n\nFile saved to: '" + ROMFolder + @"\" + romChunkName + "_[split]" + ".bin'\n\nIn case there was a header, it has been removed!");
            }
        }
    }
}