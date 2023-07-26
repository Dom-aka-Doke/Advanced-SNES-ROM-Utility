using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Advanced_SNES_ROM_Utility.Helper
{
    public static partial class SNESROMHelper
    {
        public static bool FindAndReplaceByRegEx(this SNESROM sourceROM, IDictionary<string, string> lockingCodeDictionary, bool unlock)
        {
            string sourceROMString = BitConverter.ToString(sourceROM.SourceROM).Replace("-", "");
            int ulcCtr = 0;
            int matchCtr = 0;

            foreach (KeyValuePair<string, string> lockingCode in lockingCodeDictionary)
            {
                Regex r = new Regex(lockingCode.Key);

                foreach (Match match in r.Matches(sourceROMString))
                {
                    if (match.Index % 2 == 0)
                    {
                        if (unlock)
                        {
                            string newHexString = Regex.Replace(match.Value, lockingCode.Key, lockingCode.Value).Replace(" ", "");
                            sourceROMString = sourceROMString.Remove(match.Index, newHexString.Length).Insert(match.Index, newHexString);
                            matchCtr++;
                        }

                        else
                        {
                            return true;
                        }
                    }
                }

                ulcCtr++;
            }

            if (matchCtr > 0)
            {
                sourceROM.SourceROM = StringToByteArray(sourceROMString);
                sourceROM.Initialize();
                return true;
            }

            return false;
        }

        public static byte[] StringToByteArray(string hexString)
        {
            int numberChars = hexString.Length;
            byte[] bytes = new byte[numberChars / 2];

            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}