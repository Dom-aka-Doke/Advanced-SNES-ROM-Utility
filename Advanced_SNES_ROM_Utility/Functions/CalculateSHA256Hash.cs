using System;
using System.Security.Cryptography;
using System.Linq;

namespace Advanced_SNES_ROM_Utility.Functions
{
    public static partial class SNESROMFunction
    {
        public static string CalculateSHA256Hash(byte[] sourceROM, byte[] sourceROMSMCHeader, uint uIntSMCHeader)
        {
            string hash = null;
            byte[] SHA256SourceROM = new byte[sourceROM.Length + uIntSMCHeader];

            if (sourceROMSMCHeader != null && uIntSMCHeader > 0)
            {
                // Merge header with ROM if header exists
                Buffer.BlockCopy(sourceROMSMCHeader, 0, SHA256SourceROM, 0, sourceROMSMCHeader.Length);
                Buffer.BlockCopy(sourceROM, 0, SHA256SourceROM, sourceROMSMCHeader.Length, sourceROM.Length);
            }

            else
            {
                Buffer.BlockCopy(sourceROM, 0, SHA256SourceROM, 0, sourceROM.Length);
            }

            using (var sha256 = SHA256.Create())
            {
                hash = string.Concat(sha256.ComputeHash(SHA256SourceROM).Select(b => b.ToString("x2")));
            }

            return hash;
        }
    }
}