using System.IO;

namespace Advanced_SNES_ROM_Utility
{
    public partial class SNESROM
    {
        public void ApplyIPSPatch(string ipsFilePath)
        {
            byte[] byteArrayIPSArray = File.ReadAllBytes(ipsFilePath);

            // IPS patches muste be at least 14 bytes
            if (byteArrayIPSArray.Length >= 14)
            {

            }
        }
    }
}
