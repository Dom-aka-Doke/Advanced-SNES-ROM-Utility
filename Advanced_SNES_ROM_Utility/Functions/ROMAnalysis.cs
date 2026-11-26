using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Advanced_SNES_ROM_Utility.Converter;

namespace Advanced_SNES_ROM_Utility.Functions
{
    /// <summary>Result of the overdump detection</summary>
    public class OverdumpInfo
    {
        public bool Supported = true;
        public bool Overdump;
        /// <summary>"mirror" or "padding"</summary>
        public string Type;
        public int RealSize;
        public int FileSize;
        public int InternalSize;
        public byte Fill;
        public string Reason;
    }

    /// <summary>One ROM bank of the heatmap</summary>
    public class BankInfo
    {
        public int Index;
        public int Offset;
        public int Length;
        public string SNESAddress;
        /// <summary>0x00 / 0xFF if the bank only contains this value, otherwise -1</summary>
        public int Fill = -1;
        public double Entropy;
        public bool Header;
        /// <summary>Index of the first identical bank, -1 if this bank is unique or the original</summary>
        public int DuplicateOf = -1;
        /// <summary>Group of identical banks, -1 if none</summary>
        public int Group = -1;
    }

    /// <summary>Result of the bank analysis</summary>
    public class BankAnalysis
    {
        public int BankSize;
        public int ROMSize;
        public List<BankInfo> Banks = new List<BankInfo>();
        public List<List<int>> Groups = new List<List<int>>();
        public int Unique;
        public int Duplicates;
        public int Copies;
        public int Empty;
    }

    public static partial class SNESROMFunction
    {
        private const int AnalysisMbit = 0x20000;   // 1 Mbit = 128 kByte
        private const int AnalysisChunk = 0x8000;   // granularity for mirror compares, smallest ROM size

        private static bool SameBytes(byte[] data, int a, int b, int length)
        {
            for (int i = 0; i < length; i++)
            {
                if (data[a + i] != data[b + i]) { return false; }
            }
            return true;
        }

        /// <summary>True if everything from `size` on is a mirror of the first `size` bytes</summary>
        private static bool IsMirrorBeyond(byte[] rom, int size)
        {
            for (int offset = size; offset < rom.Length; offset += AnalysisChunk)
            {
                int length = Math.Min(AnalysisChunk, rom.Length - offset);
                if (!SameBytes(rom, offset, SNESROMConvert.MirrorAddress(offset, size), length)) { return false; }
            }
            return true;
        }

        /// <summary>Returns 0x00 / 0xFF if the range only contains that value, otherwise -1</summary>
        private static int FillValue(byte[] rom, int start, int end)
        {
            if (start >= end) { return -1; }

            byte value = rom[start];
            if (value != 0x00 && value != 0xFF) { return -1; }

            for (int i = start + 1; i < end; i++)
            {
                if (rom[i] != value) { return -1; }
            }

            return value;
        }

        /// <summary>Sizes a ROM can really have: 32 / 64 kByte for tiny ROMs, then every whole Mbit</summary>
        private static IEnumerable<int> CandidateSizes(int minSize, int fileSize)
        {
            for (int size = minSize; size < AnalysisMbit && size < fileSize; size *= 2) { yield return size; }

            int first = Math.Max(AnalysisMbit, (minSize + AnalysisMbit - 1) / AnalysisMbit * AnalysisMbit);
            for (int size = first; size < fileSize; size += AnalysisMbit) { yield return size; }
        }

        /// <summary>
        /// Detects overdumps:
        /// - "mirror":  the end of the file is a mirror of a smaller ROM (f.e. a 4 Mbit game dumped as 8 Mbit,
        ///              or a 12 Mbit game dumped as 16 Mbit with the last 4 Mbit repeated)
        /// - "padding": the file is larger than the internal ROM size and the excess only contains 0x00/0xFF
        /// </summary>
        public static OverdumpInfo DetectOverdump(this SNESROM sourceROM)
        {
            byte[] data = sourceROM.SourceROM;
            OverdumpInfo result = new OverdumpInfo
            {
                FileSize = data.Length,
                InternalSize = (int)(sourceROM.IntROMSize * AnalysisMbit)
            };

            if (sourceROM.IsInterleaved)
            {
                result.Supported = false;
                result.Reason = "Interleaved ROMs have to be deinterleaved first.";
                return result;
            }

            // The (copied) internal header must stay inside the ROM
            int headerEnd = (int)sourceROM.UIntROMHeaderOffset + 0x50;
            if (sourceROM.IsExROMHeaderCopied) { headerEnd = Math.Max(headerEnd, (int)sourceROM.UIntROMHeaderCopyOffset + 0x50); }
            int minSize = Math.Max(AnalysisChunk, (headerEnd + AnalysisChunk - 1) / AnalysisChunk * AnalysisChunk);

            // 1) Padding beyond the internal ROM size (0x00/0xFF). Padding is never trimmed below the
            //    internal size, because games may contain unused (filled) space inside their real ROM size.
            if (result.InternalSize >= minSize && data.Length > result.InternalSize)
            {
                int fill = FillValue(data, result.InternalSize, data.Length);

                if (fill >= 0)
                {
                    result.Overdump = true;
                    result.Type = "padding";
                    result.Fill = (byte)fill;
                    result.RealSize = result.InternalSize;
                    return result;
                }
            }

            // 2) Mirrored data: smallest ROM size whose mirror explains the whole file.
            //    Sizes where the rest of the file is only 0x00/0xFF are skipped - a filled area
            //    "mirrors" itself and would otherwise be mistaken for real data.
            foreach (int size in CandidateSizes(minSize, data.Length))
            {
                if (FillValue(data, size, data.Length) >= 0) { continue; }

                if (IsMirrorBeyond(data, size))
                {
                    result.Overdump = true;
                    result.Type = "mirror";
                    result.RealSize = size;
                    return result;
                }
            }

            return result;
        }

        /// <summary>Removes a detected overdump. Returns the applied detection result or null.</summary>
        public static OverdumpInfo RemoveOverdump(this SNESROM sourceROM)
        {
            OverdumpInfo info = sourceROM.DetectOverdump();
            if (!info.Supported || !info.Overdump) { return null; }

            byte[] trimmed = new byte[info.RealSize];
            Buffer.BlockCopy(sourceROM.SourceROM, 0, trimmed, 0, info.RealSize);

            sourceROM.SourceROM = trimmed;
            sourceROM.Initialize(new string[] { "romheader", "mapmode", "romsize", "checksum", "filesize" });
            return info;
        }

        /// <summary>Default bank size for the ROM's map mode: 32 kByte for LoROM, 64 kByte for HiROM</summary>
        public static int DefaultBankSize(this SNESROM sourceROM)
        {
            bool hi = sourceROM.UIntROMHeaderOffset == (uint)HeaderOffset.hirom || sourceROM.UIntROMHeaderOffset == (uint)HeaderOffset.exhirom;
            return hi && !sourceROM.IsInterleaved ? 0x10000 : 0x8000;
        }

        /// <summary>SNES CPU address of a file offset (start of a bank)</summary>
        private static string BankSNESAddress(SNESROM sourceROM, int offset, int bankSize)
        {
            uint headerOffset = sourceROM.UIntROMHeaderOffset;
            bool isHi = headerOffset == (uint)HeaderOffset.hirom || headerOffset == (uint)HeaderOffset.exhirom;

            if (isHi && bankSize == 0x10000)
            {
                int bank = offset >> 16;

                // ExHiROM: first 4 MByte at $C0-$FF, rest at $40-$7D
                if (headerOffset == (uint)HeaderOffset.exhirom)
                {
                    return bank < 0x40 ? "$" + (0xC0 + bank).ToString("X2") + ":0000" : "$" + bank.ToString("X2") + ":0000";
                }

                return "$" + (0xC0 + (bank & 0x3F)).ToString("X2") + ":0000";
            }

            if (!isHi && bankSize == 0x8000)
            {
                int bank = offset >> 15;

                // ExLoROM: first 4 MByte at $80-$FF, rest at $00-$7D
                if (headerOffset == (uint)HeaderOffset.exlorom)
                {
                    return bank < 0x80 ? "$" + (0x80 + bank).ToString("X2") + ":8000" : "$" + (bank - 0x80).ToString("X2") + ":8000";
                }

                return "$" + (bank & 0x7F).ToString("X2") + ":8000";
            }

            return string.Empty;
        }

        private static double Entropy(byte[] data, int start, int length)
        {
            int[] counts = new int[256];
            for (int i = start; i < start + length; i++) { counts[data[i]]++; }

            double entropy = 0;
            for (int i = 0; i < 256; i++)
            {
                if (counts[i] > 0)
                {
                    double p = (double)counts[i] / length;
                    entropy -= p * Math.Log(p, 2);
                }
            }

            return entropy;     // 0 - 8 bits per byte
        }

        /// <summary>Splits the ROM into banks and finds identical and empty banks</summary>
        /// <param name="bankSize">0x8000 or 0x10000; 0 = depending on the map mode</param>
        public static BankAnalysis AnalyzeBanks(this SNESROM sourceROM, int bankSize = 0)
        {
            int size = bankSize > 0 ? bankSize : sourceROM.DefaultBankSize();
            byte[] data = sourceROM.SourceROM;
            int count = (data.Length + size - 1) / size;

            List<int> headerOffsets = new List<int> { (int)sourceROM.UIntROMHeaderOffset };
            if (sourceROM.IsExROMHeaderCopied) { headerOffsets.Add((int)sourceROM.UIntROMHeaderCopyOffset); }

            BankAnalysis analysis = new BankAnalysis { BankSize = size, ROMSize = data.Length };
            Dictionary<string, List<int>> byHash = new Dictionary<string, List<int>>();
            List<string> order = new List<string>();

            using (SHA1 sha1 = SHA1.Create())
            {
                for (int i = 0; i < count; i++)
                {
                    int start = i * size;
                    int end = Math.Min(start + size, data.Length);

                    BankInfo bank = new BankInfo
                    {
                        Index = i,
                        Offset = start,
                        Length = end - start,
                        SNESAddress = BankSNESAddress(sourceROM, start, size),
                        Fill = FillValue(data, start, end),
                        Entropy = Math.Round(Entropy(data, start, end - start), 2),
                        Header = headerOffsets.Any(h => h >= start && h < end)
                    };

                    string hash = BitConverter.ToString(sha1.ComputeHash(data, start, end - start));

                    if (!byHash.ContainsKey(hash))
                    {
                        byHash[hash] = new List<int>();
                        order.Add(hash);
                    }

                    byHash[hash].Add(i);
                    analysis.Banks.Add(bank);
                }
            }

            // Groups of identical banks (filled banks are reported as empty, not as duplicates)
            foreach (string hash in order)
            {
                List<int> members = byHash[hash];
                if (members.Count < 2 || analysis.Banks[members[0]].Fill >= 0) { continue; }

                int id = analysis.Groups.Count;
                analysis.Groups.Add(members);

                foreach (int member in members)
                {
                    analysis.Banks[member].Group = id;
                    analysis.Banks[member].DuplicateOf = member == members[0] ? -1 : members[0];
                }
            }

            analysis.Duplicates = analysis.Banks.Count(b => b.Group >= 0);
            analysis.Copies = analysis.Banks.Count(b => b.DuplicateOf >= 0);
            analysis.Empty = analysis.Banks.Count(b => b.Fill >= 0);
            analysis.Unique = count - analysis.Duplicates - analysis.Empty;

            return analysis;
        }
    }
}
