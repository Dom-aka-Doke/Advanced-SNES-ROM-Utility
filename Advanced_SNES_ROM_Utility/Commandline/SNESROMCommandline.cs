using System;
using System.Reflection;
using Advanced_SNES_ROM_Utility.Functions;
using Advanced_SNES_ROM_Utility.Converter;
using Advanced_SNES_ROM_Utility.Patcher;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace Advanced_SNES_ROM_Utility.Commandline
{
    public static class SNESROMCommandline
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        private static List<string> _cliParameters = new List<string>()
                {
                    "-path",
                    "-header",
                    "-fixchecksum",
                    "-fixromsize",
                    "-removeregion",
                    "-removesram",
                    "-removeslowrom",
                    "-deinterleave",
                    "-interleave",
                    "-patch",
                    "-version",
                    "-help"
                };

        private static string[] _cliFileExtensions = { ".smc", ".sfc", ".swc", ".fig" };

        public static void Run(string[] args)
        {
            // Attach to console
            AttachConsole(ATTACH_PARENT_PROCESS);
            
            // Collect arguments
            List<string> inputParameters = args.Where(arg => arg.StartsWith("-")).ToList();

            // Return codes
            /*  0 = noting happened
             *  1 = successful
             * -1 = error -> execution stopped
             * -2 = failed to execute operation on file (skip)
             */
            int returnCode = 0;

            // Print help if invalid argument was found or user wants to display help
            if (inputParameters.Except(_cliParameters).Any() || args[0].ToLower().Equals("-help"))
            {
                CLIPrintHelp();
            }

            // Print version number, if user wants to see version number
            else if (args[0].ToLower().Equals("-version"))
            {
                CLIPrintVersion();
            }

            // Process file(s)
            else if (args[0].ToLower().Equals("-path"))
            {
                if (args.Length > 1 && File.Exists(args[1]) && _cliFileExtensions.Contains(Path.GetExtension(args[1])))
                {
                    SNESROM sourceROM = new SNESROM(args[1]);
                    returnCode = CLIProcessFile(sourceROM, args);

                    switch (returnCode)
                    {
                        case 0:
                            Console.WriteLine("\nCould not find any operation to execute");
                            break;
                        case 1:
                            byte[] mergedSourceROM = CLIMergeROM(sourceROM);
                            File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(args[1]), Path.GetFileNameWithoutExtension(args[1]) + "-outfile" + Path.GetExtension(args[1])), mergedSourceROM);
                            break;
                        case -1:
                            Console.WriteLine("\nExecution stopped!\nPlease check your arguments and parameters and try again.");
                            break;
                        case 2:
                            Console.WriteLine("\nFailed to execute operation -> skip");
                            break;
                        default:
                            Console.WriteLine("\nHuh! Something unexpected happened!");
                            break;
                    }
                }

                else if (args.Length > 1 && Directory.Exists(args[1]))
                {
                    List<string> files = Directory.GetFiles(args[1]).Where(file => _cliFileExtensions.Any(file.ToLower().EndsWith)).ToList();

                    foreach (string file in files)
                    {
                        SNESROM sourceROM = new SNESROM(file);
                        returnCode = CLIProcessFile(sourceROM, args);

                        switch (returnCode)
                        {
                            case 0: Console.WriteLine("\nCould not find any operation to execute");
                                break;
                            case 1:
                                byte[] mergedSourceROM = CLIMergeROM(sourceROM);
                                File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(args[1]), Path.GetFileNameWithoutExtension(args[1]) + "-outfile" + Path.GetExtension(args[1])), mergedSourceROM);
                                break;
                            case -1: Console.WriteLine("\nExecution stopped!\nPlease check your arguments and parameters and try again.");
                                break;
                            case 2: Console.WriteLine("\nFailed to execute operation -> skip");
                                break;
                            default: Console.WriteLine("\nHuh! Something unexpected happened!");
                                break;
                        }
                    }
                }

                else
                {
                    Console.WriteLine("\nFile or folder not found");
                }
            }
        }

        private static int CLIProcessFile(this SNESROM sourceROM, string[] args)
        {
            if (args.Length > 2)
            {
                for (int argPos = 2; argPos < args.Length; argPos++)
                {
                    switch (args[argPos].ToLower())
                    {
                        case "-header":
                            if (args[argPos + 1].ToLower().Equals("add"))
                            {
                                sourceROM.AddHeader();
                            }

                            else if (args[argPos + 1].ToLower().Equals("remove"))
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

                        case "-fixromsize":
                            sourceROM.FixInternalROMSize();
                            break;

                        case "-removeregion":
                            sourceROM.RemoveRegionChecks();
                            break;

                        case "-removesram":
                            sourceROM.RemoveSRAMChecks();
                            break;

                        case "-removeslowrom":
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
                                byte[] patchedSourceROM = null;
                                byte[] mergedSourceROM = CLIMergeROM(sourceROM);

                                switch (Path.GetExtension(args[argPos + 1]))
                                {
                                    case ".ips": patchedSourceROM = IPSPatch.Apply(mergedSourceROM, args[argPos + 1]); break;
                                    case ".ups": patchedSourceROM = UPSPatch.Apply(mergedSourceROM, sourceROM.CRC32Hash, args[argPos + 1]); break;
                                    case ".bps": patchedSourceROM = BPSPatch.Apply(mergedSourceROM, sourceROM.CRC32Hash, args[argPos + 1]); break;
                                    case ".bdf": patchedSourceROM = BDFPatch.Apply(mergedSourceROM, args[argPos + 1]); break;
                                    case ".xdelta": patchedSourceROM = XDELTAPatch.Apply(mergedSourceROM, args[argPos + 1]); break;
                                }

                                if (patchedSourceROM != null)
                                {
                                    sourceROM.SourceROM = patchedSourceROM;
                                    sourceROM.UIntSMCHeader = 0;
                                    sourceROM.SourceROMSMCHeader = null;
                                    sourceROM.Initialize();
                                }

                                else
                                {
                                    return -2;
                                }
                            }

                            argPos++;
                            break;
                    }
                }

                return 1;
            }

            else
            {
                return 0;
            }
        }

        private static byte[] CLIMergeROM(SNESROM sourceROM)
        {
            byte[] mergedSourceROM = new byte[sourceROM.SourceROM.Length + sourceROM.UIntSMCHeader];

            if (sourceROM.SourceROMSMCHeader != null && sourceROM.UIntSMCHeader > 0)
            {
                // Merge header with ROM if header exists
                Buffer.BlockCopy(sourceROM.SourceROMSMCHeader, 0, mergedSourceROM, 0, sourceROM.SourceROMSMCHeader.Length);
                Buffer.BlockCopy(sourceROM.SourceROM, 0, mergedSourceROM, sourceROM.SourceROMSMCHeader.Length, sourceROM.SourceROM.Length);
            }

            else
            {
                // Just copy source ROM if no header exists
                Buffer.BlockCopy(sourceROM.SourceROM, 0, mergedSourceROM, 0, sourceROM.SourceROM.Length);
            }

            return mergedSourceROM;
        }

        private static void CLIPrintHelp()
        {
            Console.WriteLine("\n" +
                              "Usage:\n" +
                                  "\tAdvanced_SNES_ROM_Utility [options]\n" +
                              "\n" +
                                  "\tOptions:\n" +
                                     "\t\t-path <FILEPATH, FOLDER>\tThe path or folder to ROM file(s)\n" +
                                     "\t\t-header <add, remove>\t\tDetermines whether a header should be added or removed\n" +
                                     "\t\t-fixchecksum\t\t\tDetermines whether a broken checksum should be fixed\n" +
                                     "\t\t-fixromsize\t\t\tDetermines whether a wrong internal ROM size information should be fixed\n" +
                                     "\t\t-removeregion\t\t\tDetermines whether existing region checks should be removed\n" +
                                     "\t\t-removesram\t\t\tDetermines whether existing sram checks should be removed\n" +
                                     "\t\t-removeslowrom\t\t\tDetermines whether existing slow ROM checks should be removed\n" +
                                     "\t\t-deinterleave\t\t\tDetermines whether an interleaved ROM should be deinterleaved\n" +
                                     "\t\t-interleave\t\t\tDetermines whether a deinterleaved ROM should be interleaved\n" +
                                     "\t\t-patch <FILEPATH>\t\tDetermines whether ROM(s) should be patched with a specific patch\n" +
                                     "\t\t-version\t\t\tShow version information\n" +
                                     "\t\t-help\t\t\t\tShow help and usage information\n"
                              );
        }

        private static void CLIPrintVersion()
        {
            Console.WriteLine("\n" + Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }
    }
}