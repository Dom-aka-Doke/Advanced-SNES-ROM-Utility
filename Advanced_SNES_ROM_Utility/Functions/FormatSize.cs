using System;
using System.Globalization;

namespace Advanced_SNES_ROM_Utility.Functions
{
    public static partial class SNESROMFunction
    {
        /// <summary>Formats a size in Mbit: "8 Mbit", below 1 Mbit in Kbit ("256 Kbit")</summary>
        public static string FormatMbit(double mbit)
        {
            if (mbit >= 1)
            {
                return mbit.ToString("0.##", CultureInfo.InvariantCulture) + " Mbit";
            }

            return Math.Round(mbit * 1024) + " Kbit";
        }

        /// <summary>Formats a size in Mbit as bytes: "1 MByte" / "1,5 MByte", below 1 MByte in kByte ("32 kByte")</summary>
        public static string FormatMByte(double mbit)
        {
            if (mbit >= 8)
            {
                return (mbit / 8).ToString("0.###", CultureInfo.InvariantCulture).Replace('.', ',') + " MByte";
            }

            return Math.Round(mbit * 128) + " kByte";
        }

        /// <summary>Formats a size in bytes: "8 Mbit" / "512 Kbit" / "1.50 Mbit"</summary>
        public static string FormatBytes(long bytes)
        {
            double mbit = bytes / 131072.0;

            if (mbit < 1)
            {
                return Math.Round(mbit * 1024) + " Kbit";
            }

            return (mbit == Math.Floor(mbit) ? mbit.ToString("0", CultureInfo.InvariantCulture) : mbit.ToString("0.00", CultureInfo.InvariantCulture)) + " Mbit";
        }

        /// <summary>True if the internal ROM size does not match the file size</summary>
        public static bool NeedsInternalROMSizeFix(this SNESROM sourceROM)
        {
            return sourceROM.IntROMSize < sourceROM.IntCalcFileSize || sourceROM.IntCalcFileSize <= sourceROM.IntROMSize / 2;
        }
    }
}
