using System;
using System.IO;
using System.Windows.Forms;
using xdelta3.net;

namespace Advanced_SNES_ROM_Utility
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