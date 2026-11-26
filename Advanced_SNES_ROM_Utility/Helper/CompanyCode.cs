using System;

namespace Advanced_SNES_ROM_Utility.Helper
{
    /// <summary>
    /// Transformation between the internal company code (key of SNESROMList.Company) and the bytes
    /// stored in the ROM header.
    ///
    /// Old header ($FFDA != 0x33): one byte at $FFDA, code = high nibble * 36 + low nibble
    ///                             (the byte's hex notation read as base 36, f.e. 0xA4 -> "A4" -> 10 * 36 + 4)
    /// New header ($FFDA == 0x33): two ASCII characters ("maker code") at $FFB0, code = c0 * 36 + c1
    ///                             with '0'-'9' = 0-9 and 'A'-'Z' = 10-35
    ///
    /// Decoding works like SNESROM.GetCompany() (byte arithmetic wraps around at 256), so homebrew
    /// maker codes with unusual characters keep their codes.
    /// </summary>
    public static class CompanyCode
    {
        public const byte NewHeaderMarker = 0x33;

        /// <summary>Decodes one maker code character like SNESROM.GetCompany()</summary>
        public static int DecodeChar(byte b)
        {
            return (b > 0x39 ? b - 0x37 : b - 0x30) & 0xFF;
        }

        /// <summary>Inverse of DecodeChar(); returns -1 if no byte produces this digit value</summary>
        public static int EncodeChar(int d)
        {
            if (d < 0 || d > 0xFF) { return -1; }
            if (d <= 9) { return 0x30 + d; }        // '0'-'9'
            if (d <= 0xC8) { return d + 0x37; }     // 'A'-'Z' and beyond
            if (d >= 0xD0) { return d - 0xD0; }     // wrap around (bytes 0x00-0x2F)
            return -1;                              // 0xC9-0xCF cannot be produced
        }

        public static int DecodeOld(byte b)
        {
            return ((b >> 4) & 0x0F) * 36 + (b & 0x0F);
        }

        public static int DecodeNew(byte b0, byte b1)
        {
            return DecodeChar(b0) * 36 + DecodeChar(b1);
        }

        /// <summary>
        /// Old header: code -> single byte. Only codes whose base 36 digits are 0-15 are possible,
        /// and 0x33 is reserved as marker for the extended header. Returns -1 if not possible.
        /// </summary>
        public static int EncodeOld(int code)
        {
            if (code < 0) { return -1; }

            int hi = code / 36;
            int lo = code % 36;

            if (hi > 0x0F || lo > 0x0F) { return -1; }

            int value = (hi << 4) | lo;
            return value == NewHeaderMarker ? -1 : value;
        }

        /// <summary>
        /// New header: code -> two maker code bytes. Prefers the regular notation ('0'-'9', 'A'-'Z').
        /// Returns null if not possible.
        /// </summary>
        public static byte[] EncodeNew(int code)
        {
            if (code < 0) { return null; }

            // Regular split first (second digit 0-35), then alternatives with a larger second digit
            for (int c0 = code / 36; c0 >= 0; c0--)
            {
                int c1 = code - c0 * 36;
                if (c1 > 0xFF) { break; }

                int b0 = EncodeChar(c0);
                int b1 = EncodeChar(c1);

                if (b0 >= 0 && b1 >= 0 && c0 <= 0xFF)
                {
                    return new byte[] { (byte)b0, (byte)b1 };
                }
            }

            return null;
        }

        /// <summary>Text shown for the code as stored in the header, f.e. "A4"</summary>
        public static string HeaderNotation(int code, bool isNewHeader)
        {
            if (isNewHeader)
            {
                byte[] bytes = EncodeNew(code);
                if (bytes == null) { return "??"; }

                string text = string.Empty;
                foreach (byte b in bytes)
                {
                    text += (b >= 0x20 && b < 0x7F) ? ((char)b).ToString() : "\\x" + b.ToString("x2");
                }
                return text;
            }

            int value = EncodeOld(code);
            return value < 0 ? "??" : value.ToString("X2");
        }

        /// <summary>Key of SNESROMList.Company, f.e. 0x016C -> "0x016C"</summary>
        public static string CodeKey(int code)
        {
            return "0x" + code.ToString("X4");
        }
    }
}
