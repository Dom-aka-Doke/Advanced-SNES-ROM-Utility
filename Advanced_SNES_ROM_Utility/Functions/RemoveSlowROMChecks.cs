using System.Collections.Generic;
using Advanced_SNES_ROM_Utility.Helper;

namespace Advanced_SNES_ROM_Utility.Functions
{
    public static partial class SNESROMFunction
    {
        public static bool RemoveSlowROMChecks(this SNESROM sourceROM, bool unlock = true)
        {
            IDictionary<string, string> lockingCodeDictionary = new Dictionary<string, string>();

            lockingCodeDictionary.Add(@"(A2|A9)(01)(8D|8E)(0D42)", "$1 00 $3 $4");
            lockingCodeDictionary.Add(@"(A9)(01)(008D0D42)", "$1 00 $3");
            lockingCodeDictionary.Add(@"(A9)(01)(8F0D4200)", "$1 00 $3");

            return SNESROMHelper.FindAndReplaceByRegEx(sourceROM, lockingCodeDictionary, unlock);
        }
    }
}