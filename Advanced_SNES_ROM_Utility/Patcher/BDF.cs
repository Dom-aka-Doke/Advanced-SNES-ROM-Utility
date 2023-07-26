using System.IO;
using deltaq;

namespace Advanced_SNES_ROM_Utility.Patcher
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