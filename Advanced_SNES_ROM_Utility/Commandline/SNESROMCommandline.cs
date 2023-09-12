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
                    "-recursive",
                    "-overwrite",
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
                    CLIHandleReturnCode(CLIProcessFile(sourceROM, args), sourceROM, args[1], inputParameters);
                }

                else if (args.Length > 1 && Directory.Exists(args[1]))
                {
                    List<string> files = inputParameters.Contains("-recursive") ?
                        Directory.GetFiles(args[1], "*.*", SearchOption.AllDirectories).Where(file => _cliFileExtensions.Contains(Path.GetExtension(file))).ToList() : 
                        Directory.GetFiles(args[1]).Where(file => _cliFileExtensions.Any(file.ToLower().EndsWith)).ToList();

                    foreach (string file in files)
                    {
                        SNESROM sourceROM = new SNESROM(file);
                        CLIHandleReturnCode(CLIProcessFile(sourceROM, args), sourceROM, file, inputParameters);

                        if (returnCode == -1)
                        {
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
            int returnCode = 0;

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

                            if (returnCode == 0)
                            {
                                returnCode = 1;
                            }

                            argPos++;
                            break;

                        case "-fixchecksum":
                            sourceROM.FixChecksum();
                            
                            if (returnCode == 0)
                            {
                                returnCode = 1;
                            }

                            break;

                        case "-fixromsize":
                            sourceROM.FixInternalROMSize();

                            if (returnCode == 0)
                            {
                                returnCode = 1;
                            }

                            break;

                        case "-removeregion":
                            sourceROM.RemoveRegionChecks();

                            if (returnCode == 0)
                            {
                                returnCode = 1;
                            }

                            break;

                        case "-removesram":
                            sourceROM.RemoveSRAMChecks();

                            if (returnCode == 0)
                            {
                                returnCode = 1;
                            }

                            break;

                        case "-removeslowrom":
                            sourceROM.RemoveSlowROMChecks();

                            if (returnCode == 0)
                            {
                                returnCode = 1;
                            }

                            break;

                        case "-deinterleave":
                            if (sourceROM.IsInterleaved)
                            {
                                sourceROM.Deinterleave();

                                if (returnCode == 0)
                                {
                                    returnCode = 1;
                                }
                            }
                            break;

                        case "-interleave":
                            if (!sourceROM.IsInterleaved && (sourceROM.UIntROMHeaderOffset == (uint)HeaderOffset.hirom || sourceROM.UIntROMHeaderOffset == (uint)HeaderOffset.exhirom))
                            {
                                sourceROM.Interleave();

                                if (returnCode == 0)
                                {
                                    returnCode = 1;
                                }

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

                                    if (returnCode == 0)
                                    {
                                        returnCode = 1;
                                    }
                                }

                                else
                                {
                                    returnCode = -2;
                                }
                            }

                            argPos++;
                            break;

                        default:
                            break;
                    }
                }
            }

            return returnCode;
        }

        private static void CLIHandleReturnCode(int returnCode, SNESROM sourceROM, string savePath, List<string> inputParameters)
        {
            /* Return codes
             *  1 = successful
             *  0 = noting happened
             * -1 = error -> execution stopped
             * -2 = failed to execute operation -> skip function for this file
             */

            switch (returnCode)
            {
                case 1:
                    CLISave(sourceROM, savePath, inputParameters.Contains("-overwrite") ? true : false);
                    break;
                case 0:
                    Console.WriteLine("\nNo executable operation was found.");
                    break;
                case -1:
                    Console.WriteLine("\nExecution stopped!\nPlease check your arguments and parameters and try again.");
                    break;
                case -2:
                    CLISave(sourceROM, savePath, inputParameters.Contains("-overwrite") ? true : false);
                    Console.WriteLine("Info: Failed to execute an operation for this file.\n");
                    break;
                default:
                    Console.WriteLine("\nHuh! Something unexpected happened!");
                    break;
            }
        }

        private static void CLISave(SNESROM sourceROM, string path, bool overwrite)
        {
            string nameExtension = overwrite ? string.Empty : "-outfile";
            byte[] mergedSourceROM = CLIMergeROM(sourceROM);
            string savePath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + nameExtension + Path.GetExtension(path));
            File.WriteAllBytes(savePath, mergedSourceROM);
            Console.WriteLine($"\nProcessing file {sourceROM.ROMFullPath} was successful!" +
                              $"\nSaved to: {savePath}");
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
                                     "\t\t-recursive\t\t\tDetermines whether subdirectories should be included\n" +
                                     "\t\t-overwrite\t\t\tDetermines whether overwriting files or saving with an extension\n" +
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