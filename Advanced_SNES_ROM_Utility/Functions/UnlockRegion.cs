using System.Collections.Generic;
using Advanced_SNES_ROM_Utility.Helper;

namespace Advanced_SNES_ROM_Utility.Functions
{
    public static partial class SNESROMFunction
    {
        public static bool UnlockRegion(this SNESROM sourceROM, bool unlock)
        {
            IDictionary<string, string> lockingCodeDictionary = new Dictionary<string, string>();

            if (sourceROM.StringRegion.Equals("PAL"))
            {
                lockingCodeDictionary.Add(@"(AD3F21291000)(D0)", "$1 80");
                lockingCodeDictionary.Add(@"(AD3F21)(891000D0)", "A9 10 00 $2");
                lockingCodeDictionary.Add(@"(AD3F212910CFBDFF)(\w{2})(F0)", "$1 $2 80");
                lockingCodeDictionary.Add(@"(AD3F218910)(D0)", "$1 80");
                lockingCodeDictionary.Add(@"(AD3F218910)(F0)", "$1 EA EA");
                lockingCodeDictionary.Add(@"(AF3F21002910)(D0)", "$1 80");
                lockingCodeDictionary.Add(@"(AF3F2100291000)(D0)", "$1 EA EA");
                lockingCodeDictionary.Add(@"(AF3F210029)(\w{2})(C9)(\w{2})(F0)", "$1 $2 $3 $4 80");
                lockingCodeDictionary.Add(@"(A21801BD2720891000)(F001)", "$1 EA EA");
                lockingCodeDictionary.Add(@"(AF3F2100)(89)(1000)", "$1 A9 $3");                         // Asterix & Obelix
                lockingCodeDictionary.Add(@"(AF3F2100C220)(29)(1000)", "$1 A9 $3");                     // 90 Minutes - European Prime Goal
                lockingCodeDictionary.Add(@"(AF3F2100)(29)(10C2)", "$1 A9 $3");                         // California Games 2
                lockingCodeDictionary.Add(@"(AD3F21)(29)(108562C2)", "$1 A9 $3");                       // Dirt Racer
            }

            else
            {
                lockingCodeDictionary.Add(@"(3F21)(29|89)(10)(F0)", "$1 $2 $3 80");
                lockingCodeDictionary.Add(@"(AD3F21)(29|89)(10)(D0)", "$1 $2 $3 EA EA");
                lockingCodeDictionary.Add(@"(3F21)(29|89)(1000)(F0)", "$1  $2 $3 80");
                lockingCodeDictionary.Add(@"(3F21)(29|89)(1000)(D0)", "$1 $2 $3 EA EA");
                lockingCodeDictionary.Add(@"(3F218910C2)(\w{2})(F0)", "$1 $2 80");
                lockingCodeDictionary.Add(@"(3F218910C2)(\w{2})(D0)", "$1 $2 EA EA");
                lockingCodeDictionary.Add(@"(3F21)(29|89)(10C910)(F0)", "$1  $2 $3 80");
                lockingCodeDictionary.Add(@"(AD3F212910C900)(F0)", "$1 EA EA");
                lockingCodeDictionary.Add(@"(AD3F212910C900)(D0)", "$1 80");
                lockingCodeDictionary.Add(@"(AD3F212910C910)(D0)", "$1 EA EA");
                lockingCodeDictionary.Add(@"(3F212910CF)(\w{4})(80)(F0)", "$1 $2 $3 80");
                lockingCodeDictionary.Add(@"(AD3F218D)(\w{4})(29)(10)(8D)", "$1 $2 $3 00 $5");
                lockingCodeDictionary.Add(@"(3F2100)(29|89)(10)(F0)", "$1 $2 $3 80");
                lockingCodeDictionary.Add(@"(AF3F2100)(29|89)(10)(D0)", "$1 $2 $3 EA EA");
                lockingCodeDictionary.Add(@"(AF3F2100)(29|89)(1000)(F0)", "$1 $2 $3 80");
                lockingCodeDictionary.Add(@"(AF3F210029)(\w{2})(C9)(\w{2})(F0)", "$1 $2 $3 $4 80");
                lockingCodeDictionary.Add(@"(AF3F210029)(10)(802D001B)", "$1 00 $3");
                lockingCodeDictionary.Add(@"(3F21008910C2)(\w{2})(F0)", "$1 $2 80");
                lockingCodeDictionary.Add(@"(AF3F2100)(\w{4})(291000)(D0)", "$1 $2 $3 EA EA");
                lockingCodeDictionary.Add(@"(3F21C2)(\w{2})(291000)(F0)", "$1 $2 $3 80");
                lockingCodeDictionary.Add(@"(3F21C2)(\w{2})(291000)(D0)", "$1 $2 $3 EA EA");
                lockingCodeDictionary.Add(@"(AF3F21)(EA891000D0)", "A9 00 00 $2");
                lockingCodeDictionary.Add(@"(A21801BD2720891000)(D001)", "$1 EA EA");
                lockingCodeDictionary.Add(@"(291000A20000C91000)(D0)", "$1 80");
                lockingCodeDictionary.Add(@"(AD3F2129)(0F)", "$1 FF");   // Dezaemon - Kaite Tsukutte Asoberu
                lockingCodeDictionary.Add(@"(ADFF1FD006)(22645FC08004)(22008A0F)(C230AB2B7AFA6840)", "EA EA EA EA EA $2 EA EA EA EA $4");   // Cooly Skunk
                // Earthbound
                lockingCodeDictionary.Add(@"(AD39B5)(D01A)(22)", "$1 EA EA $3");
                lockingCodeDictionary.Add(@"(A1C0CA10F838)(EFF2FDC3F0)", "$1 EA A9 00 00 80");
                lockingCodeDictionary.Add(@"(A1C0CA10F8)(EF38F2FDC3F0)", "$1 38 EA A9 00 00 80");
                lockingCodeDictionary.Add(@"(A1C0CA10F838)(EFEFFFC1)", "$1 EA A9 00 00");
            }

            return SNESROMHelper.FindAndReplaceByRegEx(sourceROM, lockingCodeDictionary, unlock);
        }
    }
}