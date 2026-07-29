using System;
using System.Security.Cryptography;
using System.Linq;

namespace Advanced_SNES_ROM_Utility.Functions
{
    public static partial class SNESROMFunction
    {
        public static string CalculateSHA1Hash(byte[] sourceROM, byte[] sourceROMSMCHeader, uint uIntSMCHeader)
        {
            string hash = null;
            byte[] SHA1SourceROM = new byte[sourceROM.Length + uIntSMCHeader];

            if (sourceROMSMCHeader != null && uIntSMCHeader > 0)
            {
                // Merge header with ROM if header exists
                Buffer.BlockCopy(sourceROMSMCHeader, 0, SHA1SourceROM, 0, sourceROMSMCHeader.Length);
                Buffer.BlockCopy(sourceROM, 0, SHA1SourceROM, sourceROMSMCHeader.Length, sourceROM.Length);
            }

            else
            {
                Buffer.BlockCopy(sourceROM, 0, SHA1SourceROM, 0, sourceROM.Length);
            }

            using (var sha1 = SHA1.Create())
            {
                hash = string.Concat(sha1.ComputeHash(SHA1SourceROM).Select(b => b.ToString("x2")));
            }

            return hash;
        }
    }
}