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

        private static bool _cliLog = false;
        private static string _cliLogFilePath = string.Empty;

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
                    "-log",
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

            // Set logging options
            _cliLog = inputParameters.Contains("-log") ? true : false;
            _cliLogFilePath = _cliLog ? Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}_CLI.log") : string.Empty;

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
                        if (CLIHandleReturnCode(CLIProcessFile(sourceROM, args), sourceROM, file, inputParameters) == -1) { break; }
                    }
                }

                else
                {
                    Console.WriteLine("\nFile or folder not found");
                    if (_cliLog) { CLIWriteLog(_cliLogFilePath, "File or folder not found"); }
                }
            }
        }

        private static int CLIProcessFile(this SNESROM sourceROM, string[] args)
        {
            int returnCode = 0;

            if (args.Length > 2)
            {
                Console.WriteLine($"\nProcessing file {sourceROM.ROMFullPath}");
                if (_cliLog) { CLIWriteLog(_cliLogFilePath, $"Processing file {sourceROM.ROMFullPath}", true); }

                for (int argPos = 2; argPos < args.Length; argPos++)
                {
                    switch (args[argPos].ToLower())
                    {
                        case "-header":
                            if (args[argPos + 1].ToLower().Equals("add"))
                            {
                                if (sourceROM.SourceROMSMCHeader == null)
                                {
                                    sourceROM.AddHeader();
                                    Console.WriteLine("-header: added header");
                                    if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-header: added header"); }
                                    returnCode = 1;
                                }

                                else
                                {
                                    Console.WriteLine("-header: header already existing");
                                    if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-header: header already existing"); }
                                }
                            }

                            else if (args[argPos + 1].ToLower().Equals("remove"))
                            {
                                if (sourceROM.SourceROMSMCHeader != null)
                                {
                                    sourceROM.RemoveHeader();
                                    Console.WriteLine("-header: removed header");
                                    if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-header: removed header"); }
                                    returnCode = 1;
                                }

                                else
                                {
                                    Console.WriteLine("-header: no header found to remove");
                                    if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-header: no header found to remove"); }
                                }
                            }

                            else
                            {
                                return -1;
                            }

                            argPos++;
                            break;

                        case "-fixchecksum":
                            if (!sourceROM.ByteArrayChecksum.SequenceEqual(sourceROM.ByteArrayCalcChecksum))
                            {
                                sourceROM.FixChecksum();
                                Console.WriteLine("-fixchecksum: fixed bad checksum");
                                if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-fixchecksum: fixed bad checksum"); }
                                returnCode = 1;
                            }
                            
                            else
                            {
                                Console.WriteLine("-fixchecksum: checksum already OK");
                                if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-fixchecksum: checksum already OK"); }
                            }
                            
                            break;

                        case "-fixromsize":
                            if (sourceROM.IntROMSize < sourceROM.IntCalcFileSize || sourceROM.IntCalcFileSize <= (sourceROM.IntROMSize / 2))
                            {
                                sourceROM.FixInternalROMSize();
                                Console.WriteLine("-fixromsize: fixed wrong internal ROM size");
                                if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-fixromsize: fixed wrong internal ROM size"); }
                                returnCode = 1;
                            }

                            else
                            {
                                Console.WriteLine("-fixromsize: internal ROM size already OK");
                                if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-fixromsize: internal ROM size already OK"); }
                            }

                            break;

                        case "-removeregion":
                            if (sourceROM.RemoveRegionChecks(false))
                            {
                                sourceROM.RemoveRegionChecks();
                                Console.WriteLine("-removeregion: removed region checks");
                                if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-removeregion: removed region checks"); }
                                returnCode = 1;
                            }
                            
                            else
                            {
                                Console.WriteLine("-removeregion: no region checks found");
                                if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-removeregion: no region checks found"); }
                            }

                            break;

                        case "-removesram":
                            if (sourceROM.RemoveSRAMChecks(false))
                            {
                                sourceROM.RemoveSRAMChecks();
                                Console.WriteLine("-removesram: removed SRAM checks");
                                if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-removesram: removed SRAM checks"); }
                                returnCode = 1;
                            }

                            else
                            {
                                Console.WriteLine("-removesram: no SRAM checks found");
                                if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-removesram: no SRAM checks found"); }
                            }

                            break;

                        case "-removeslowrom":
                            if (sourceROM.ByteROMSpeed == (byte)Speed.fast && sourceROM.RemoveSlowROMChecks(false))
                            {
                                sourceROM.RemoveSlowROMChecks();
                                Console.WriteLine("-removeslowrom: removed SlowROM checks");
                                if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-removeslowrom: removed SlowROM checks"); }
                                returnCode = 1;
                            }
                            
                            else
                            {
                                Console.WriteLine("-removeslowrom: no SlowROM checks found");
                                if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-removeslowrom: no SlowROM checks found"); }
                            }

                            break;

                        case "-deinterleave":
                            if (sourceROM.IsInterleaved)
                            {
                                sourceROM.Deinterleave();
                                Console.WriteLine("-deinterleave: deinterleaved successfully");
                                if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-deinterleave: deinterleaved successfully"); }
                                returnCode = 1;
                            }

                            else
                            {
                                Console.WriteLine("-deinterleave: ROM is already deinterleaved");
                                if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-deinterleave: ROM is already deinterleaved"); }
                            }

                            break;

                        case "-interleave":
                            if (!sourceROM.IsInterleaved && (sourceROM.UIntROMHeaderOffset == (uint)HeaderOffset.hirom || sourceROM.UIntROMHeaderOffset == (uint)HeaderOffset.exhirom))
                            {
                                sourceROM.Interleave();
                                Console.WriteLine("-interleave: interleaved successfully");
                                if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-interleave: interleaved successfully"); }
                                returnCode = 1;
                            }

                            else
                            {
                                if (sourceROM.UIntROMHeaderOffset == (uint)HeaderOffset.lorom || sourceROM.UIntROMHeaderOffset == (uint)HeaderOffset.exlorom)
                                {
                                    Console.WriteLine("-interleave: interleaving LoROM is not possible");
                                    if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-interleave: interleaving LoROM is not possible"); }
                                }

                                else
                                {
                                    Console.WriteLine("-interleave: ROM is already interleaved");
                                    if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-interleave: ROM is already interleaved"); }
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

                                    Console.WriteLine("-patch: ROM successfully patched");
                                    if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-patch: ROM successfully patched"); }
                                    returnCode = 1;
                                }

                                else
                                {
                                    Console.WriteLine("-patch: failed to patch ROM");
                                    if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-patch: failed to patch ROM"); }
                                }
                            }

                            else
                            {
                                Console.WriteLine("-patch: patch file not found");
                                if (_cliLog) { CLIWriteLog(_cliLogFilePath, "-patch: patch file not found"); }

                                return -1;
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

        private static int CLIHandleReturnCode(int returnCode, SNESROM sourceROM, string savePath, List<string> inputParameters)
        {
            /* Return codes
             *  1 = success
             *  0 = nothing
             * -1 = error
             */

            switch (returnCode)
            {
                case 1:
                    CLISave(sourceROM, savePath, inputParameters.Contains("-overwrite") ? true : false);
                    return 1;
                case 0:
                    Console.WriteLine("No executable operation was found -> skipped file");
                    if (_cliLog) { CLIWriteLog(_cliLogFilePath, "No executable operation was found -> skipped file"); }
                    return 0;
                case -1:
                    Console.WriteLine("Execution stopped! Please check your arguments and parameters and try again");
                    if (_cliLog) { CLIWriteLog(_cliLogFilePath, "Execution stopped! Please check your arguments and parameters and try again"); }
                    return -1;
                default:
                    Console.WriteLine("An unexpected error occurred!");
                    if (_cliLog) { CLIWriteLog(_cliLogFilePath, "An unexpected error occurred!"); }
                    return -1;
            }
        }

        private static void CLISave(SNESROM sourceROM, string path, bool overwrite)
        {
            string nameExtension = overwrite ? string.Empty : "-outfile";
            byte[] mergedSourceROM = CLIMergeROM(sourceROM);
            string savePath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + nameExtension + Path.GetExtension(path));
            File.WriteAllBytes(savePath, mergedSourceROM);
            Console.WriteLine($"Saved file to {savePath}");
            if (_cliLog) { CLIWriteLog(_cliLogFilePath, $"Saved file to {savePath}"); }
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
                                     "\t\t-fixromsize\t\t\tDetermines whether a wrong internal ROM size should be fixed\n" +
                                     "\t\t-removeregion\t\t\tDetermines whether existing region checks should be removed\n" +
                                     "\t\t-removesram\t\t\tDetermines whether existing sram checks should be removed\n" +
                                     "\t\t-removeslowrom\t\t\tDetermines whether existing slow ROM checks should be removed\n" +
                                     "\t\t-deinterleave\t\t\tDetermines whether an interleaved ROM should be deinterleaved\n" +
                                     "\t\t-interleave\t\t\tDetermines whether a deinterleaved ROM should be interleaved\n" +
                                     "\t\t-patch <FILEPATH>\t\tDetermines whether ROM(s) should be patched with a specific patch\n" +
                                     "\t\t-log\t\t\t\tDetermines whether output should be written to a logfile\n" +
                                     "\t\t-version\t\t\tShow version information\n" +
                                     "\t\t-help\t\t\t\tShow help and usage information\n"
                              );
        }

        private static void CLIPrintVersion()
        {
            Console.WriteLine("\n" + Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        private static void CLIWriteLog(string cliLogFilePath, string cliLogMessage, bool emptyLine = false)
        {
            try
            {
                FileStream cliLogFileStream = new FileStream(cliLogFilePath, FileMode.Append, FileAccess.Write);
                StreamWriter cliLogStreamWriter = new StreamWriter(cliLogFileStream);
                if (emptyLine) { cliLogStreamWriter.WriteLine(); }
                cliLogStreamWriter.WriteLine($"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff")}] {cliLogMessage}");
                cliLogStreamWriter.Close();
                cliLogFileStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}