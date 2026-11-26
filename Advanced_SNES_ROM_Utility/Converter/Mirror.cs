using System;

namespace Advanced_SNES_ROM_Utility.Converter
{
    public static partial class SNESROMConvert
    {
        // A cartridge only decodes as many address lines as its ROM needs. Addresses beyond the real
        // ROM size therefore show data that already exists ("mirror"). For a ROM size that is not a
        // power of two (f.e. 12 Mbit = 8 + 4 Mbit) the remainder is mirrored until the next power
        // of two is filled: 8 + 4 + 4. This is applied recursively (14 = 8 + 4 + 2 -> 8 + 4 + 2 + 2).
        //
        // The previous algorithm was only correct for remainders that are a power of two and for
        // files matching their internal ROM size.

        private const int MirrorChunk = 0x8000;

        /// <summary>
        /// Returns the address inside a ROM of `size` bytes that `addr` is mirrored from
        /// (same algorithm as bsnes' Bus::mirror).
        /// </summary>
        public static int MirrorAddress(int addr, int size)
        {
            if (size == 0) { return 0; }

            int baseAddr = 0;
            int mask = 1 << 30;

            while (addr >= size)
            {
                while ((addr & mask) == 0) { mask >>= 1; }
                addr -= mask;

                if (size > mask)
                {
                    size -= mask;
                    baseAddr += mask;
                }

                mask >>= 1;
            }

            return baseAddr + addr;
        }

        /// <summary>
        /// Builds the image the SNES sees when `data` is mapped into `targetLength` bytes of address space.
        /// If the target is smaller than the data, the data is cut (like a smaller ROM chip would do).
        /// </summary>
        public static byte[] MirrorTo(byte[] data, int targetLength)
        {
            byte[] output = new byte[targetLength];
            int size = data.Length;

            if (size == 0) { return output; }

            Buffer.BlockCopy(data, 0, output, 0, Math.Min(size, targetLength));

            if (targetLength <= size) { return output; }

            if (size % MirrorChunk == 0)
            {
                // Chunk aligned sizes map whole chunks contiguously
                for (int offset = size; offset < targetLength; offset += MirrorChunk)
                {
                    int length = Math.Min(MirrorChunk, targetLength - offset);
                    Buffer.BlockCopy(data, MirrorAddress(offset, size), output, offset, length);
                }
            }

            else
            {
                for (int offset = size; offset < targetLength; offset++)
                {
                    output[offset] = data[MirrorAddress(offset, size)];
                }
            }

            return output;
        }

        /// <summary>
        /// Mirrors the ROM data into an address space of `intROMSize` Mbit like the SNES does.
        /// The real data size is taken from the data itself.
        /// </summary>
        public static byte[] Mirror(byte[] sourceROM, double intROMSize, double intCalcFileSize)
        {
            return MirrorTo(sourceROM, (int)(intROMSize * 131072));
        }
    }
}
