using System;

namespace Advanced_SNES_ROM_Utility.Converter
{
    /// <summary>How new space is filled when expanding a ROM</summary>
    public enum ExpandFill
    {
        /// <summary>0x00 - free space for ROM hacks (default)</summary>
        Empty00,
        /// <summary>0xFF - like an erased (E)EPROM / flash chip</summary>
        EmptyFF,
        /// <summary>Mirrored data, like the SNES sees a smaller game on a bigger chip</summary>
        Mirror
    }

    public static partial class SNESROMConvert
    {
        // ExLoROM / ExHiROM: data beyond 4 MByte is seen in banks $00-$7D
        private const int ExROMStart = 0x400000;

        /// <summary>Converts the stored setting ("00", "FF", "mirror") into ExpandFill</summary>
        public static ExpandFill ParseExpandFill(string value)
        {
            switch ((value ?? string.Empty).ToLowerInvariant())
            {
                case "ff": return ExpandFill.EmptyFF;
                case "mirror": return ExpandFill.Mirror;
                default: return ExpandFill.Empty00;
            }
        }

        public static string ExpandFillToSetting(ExpandFill fill)
        {
            switch (fill)
            {
                case ExpandFill.EmptyFF: return "FF";
                case ExpandFill.Mirror: return "mirror";
                default: return "00";
            }
        }

        /// <summary>
        /// ROMs that are converted to ExLoROM / ExHiROM when expanding beyond 32 Mbit (no enhancement
        /// chip with its own mapping). Other ROMs can still be expanded beyond 32 Mbit (free space for
        /// ROM hacks), but their data and header stay untouched.
        /// </summary>
        public static bool CanExpandToExROM(this SNESROM sourceROM)
        {
            if (sourceROM.IsBSROM || sourceROM.IsInterleaved || sourceROM.IsSuperFX) { return false; }

            bool plainLoROM = sourceROM.ByteMapMode == (byte)MapMode.lorom_1 || sourceROM.ByteMapMode == (byte)MapMode.lorom_2;
            bool plainHiROM = sourceROM.ByteMapMode == (byte)MapMode.hirom_1 || sourceROM.ByteMapMode == (byte)MapMode.hirom_2;

            return plainLoROM || plainHiROM;
        }

        /// <summary>Expands the ROM to sizeExpandedROM Mbit (0.5 = 512 Kbit)</summary>
        public static void Expand(this SNESROM sourceROM, double sizeExpandedROM, ExpandFill fill)
        {
            int targetLength = (int)(sizeExpandedROM * 131072);
            byte fillByte = fill == ExpandFill.EmptyFF ? (byte)0xFF : (byte)0x00;
            bool toExROM = targetLength > ExROMStart && sourceROM.SourceROM.Length <= ExROMStart && sourceROM.CanExpandToExROM();
            int mapModePos = (int)sourceROM.UIntROMHeaderOffset + (int)HeaderValue.mapmode;

            // Area that keeps the game's data: everything, or the first 4 MByte when converting to ExROM
            int lowerLength = toExROM ? ExROMStart : targetLength;
            byte[] expandedROM = new byte[targetLength];

            if (fillByte != 0x00)
            {
                for (int i = 0; i < expandedROM.Length; i++) { expandedROM[i] = fillByte; }
            }

            if (fill == ExpandFill.Mirror)
            {
                // Fill exactly like the SNES mirrors the real data (real data size, not the header size)
                byte[] mirrored = MirrorTo(sourceROM.SourceROM, lowerLength);
                Buffer.BlockCopy(mirrored, 0, expandedROM, 0, mirrored.Length);
            }

            else
            {
                Buffer.BlockCopy(sourceROM.SourceROM, 0, expandedROM, 0, Math.Min(sourceROM.SourceROM.Length, targetLength));
            }

            if (toExROM)
            {
                // In ExLoROM / ExHiROM, banks $80-$FF still show the first 4 MByte, but banks $00-$7D show
                // the new area beyond 4 MByte. Games that run code via $00-$3F would crash, so the parts the
                // CPU reaches there are copied. Everything else in the new area stays free space.
                int dataLength = fill == ExpandFill.Mirror ? lowerLength : sourceROM.SourceROM.Length;

                if (sourceROM.StringMapMode.StartsWith("LoROM"))
                {
                    if (sourceROM.ByteMapMode >= 0x30)
                    {
                        expandedROM[mapModePos] = (byte)MapMode.exlorom;
                    }

                    // $00-$7D:8000-FFFF -> ROM $400000+ in 32 kByte banks: one to one copy of the LoROM data
                    int length = Math.Min(dataLength, targetLength - ExROMStart);
                    Buffer.BlockCopy(expandedROM, 0, expandedROM, ExROMStart, length);
                }

                else if (sourceROM.StringMapMode.StartsWith("HiROM"))
                {
                    expandedROM[mapModePos] = (byte)(sourceROM.ByteROMSpeed | 0x05);

                    // $00-$3F:8000-FFFF -> upper halves of the 64 kByte banks at $400000+
                    for (int bank = 0; bank * 0x10000 < dataLength; bank++)
                    {
                        int destination = ExROMStart + bank * 0x10000 + 0x8000;
                        if (destination + 0x8000 > targetLength) { break; }
                        Buffer.BlockCopy(expandedROM, bank * 0x10000 + 0x8000, expandedROM, destination, 0x8000);
                    }
                }
            }

            sourceROM.SourceROM = expandedROM;
            sourceROM.Initialize(new string[] { "romheader", "mapmode", "romsize", "checksum", "filesize" });
        }
    }
}
