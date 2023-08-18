using System;
using System.IO;
using System.Windows.Forms;
using Advanced_SNES_ROM_Utility.Functions;
using Advanced_SNES_ROM_Utility.Converter;

namespace Advanced_SNES_ROM_Utility
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                int returnCode = 0;

                if (args[0].ToLower().Equals("-path"))
                {
                    if (File.Exists(args[1]))
                    {
                        SNESROM sourceROM = new SNESROM(args[1]);
                        returnCode = ProcessFile(sourceROM, args);
                    }

                    else if (Directory.Exists(args[1]))
                    {
                        string[] files = Directory.GetFiles(args[1]);

                        foreach (string file in files)
                        {
                            SNESROM sourceROM = new SNESROM(file);
                            returnCode = ProcessFile(sourceROM, args);
                        }
                    }
                }
            }

            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }
        }

        private static int ProcessFile(this SNESROM sourceROM, string[] args)
        {
            for (int argPos = 2; argPos < args.Length; argPos++)
            {
                switch (args[argPos].ToLower())
                {
                    case "-header":
                        if (args[argPos + 1].ToLower().Equals("add") && sourceROM.SourceROMSMCHeader == null)
                        {
                            sourceROM.AddHeader();
                        }

                        else if (args[argPos + 1].ToLower().Equals("remove") && sourceROM.SourceROMSMCHeader != null)
                        {
                            sourceROM.RemoveHeader();
                        }

                        else
                        {
                            return -1;
                        }

                        argPos++;
                        break;

                    case "-fixchecksum":
                        sourceROM.FixChecksum();
                        break;

                    case "-fixinternalromsize":
                        sourceROM.FixInternalROMSize();
                        break;

                    case "-removeregionchecks":
                        sourceROM.RemoveRegionChecks();
                        break;

                    case "-removesramchecks":
                        sourceROM.RemoveSRAMChecks();
                        break;

                    case "-removeslowromchecks":
                        sourceROM.RemoveSlowROMChecks();
                        break;

                    case "-deinterleave":
                        if (sourceROM.IsInterleaved)
                        {
                            sourceROM.Deinterleave();
                        }
                        break;

                    case "-interleave":
                        if (!sourceROM.IsInterleaved && (sourceROM.UIntROMHeaderOffset == (uint)HeaderOffset.hirom || sourceROM.UIntROMHeaderOffset == (uint)HeaderOffset.exhirom))
                        {
                            sourceROM.Interleave();
                        }
                        break;

                    case "-patch":
                        if (File.Exists(args[argPos + 1]))
                        {
                            // Here comes some code...
                        }
                        break;

                    case "-version":
                        return -2;

                    case "-help":
                        return -1;
                }
            }

            return 0;
        }

        private static void PrintHelp()
        {
            return;
        }
    }
}