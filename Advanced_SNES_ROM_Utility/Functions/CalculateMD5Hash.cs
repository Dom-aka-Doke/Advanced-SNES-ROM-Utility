using System;
using System.Security.Cryptography;
using System.Linq;

namespace Advanced_SNES_ROM_Utility.Functions
{
    public static partial class SNESROMFunction
    {
        public static string CalculateMD5Hash(byte[] sourceROM, byte[] sourceROMSMCHeader, uint uIntSMCHeader)
        {
            string hash = null;
            byte[] MD5SourceROM = new byte[sourceROM.Length + uIntSMCHeader];

            if (sourceROMSMCHeader != null && uIntSMCHeader > 0)
            {
                // Merge header with ROM if header exists
                Buffer.BlockCopy(sourceROMSMCHeader, 0, MD5SourceROM, 0, sourceROMSMCHeader.Length);
                Buffer.BlockCopy(sourceROM, 0, MD5SourceROM, sourceROMSMCHeader.Length, sourceROM.Length);
            }

            else
            {
                Buffer.BlockCopy(sourceROM, 0, MD5SourceROM, 0, sourceROM.Length);
            }

            using (var md5 = MD5.Create())
            {
                hash = string.Concat(md5.ComputeHash(MD5SourceROM).Select(b => b.ToString("x2")));
            }

            return hash;
        }
    }
}