using System;
using System.IO;
using System.Windows.Forms;
using deltaq;

namespace Advanced_SNES_ROM_Utility
{
    class BDFPatch
    {
        public static byte[] Apply(byte[] mergedSourceROM, string bdfFilePath)
        {
            try
            {
                byte[] byteArrayBDFPatch = File.ReadAllBytes(bdfFilePath);
                MemoryStream patchedSourceROMStream = new MemoryStream();
                BsPatch.Apply(mergedSourceROM, byteArrayBDFPatch, patchedSourceROMStream);
                return patchedSourceROMStream.ToArray();
            }

            catch
            {
                return null;
            }
        }
    }
}