using System.IO;
using xdelta3.net;

namespace Advanced_SNES_ROM_Utility.Patcher
{
    class XDELTAPatch
    {
        public static byte[] Apply(byte[] mergedSourceROM, string xdeltaFilePath)
        {
            try
            {
                byte[] byteArrayBDFPatch = File.ReadAllBytes(xdeltaFilePath);
                return Xdelta3Lib.Decode(source: mergedSourceROM, delta: byteArrayBDFPatch).ToArray();
            }

            catch
            {
                return null;
            }
        }
    }
}