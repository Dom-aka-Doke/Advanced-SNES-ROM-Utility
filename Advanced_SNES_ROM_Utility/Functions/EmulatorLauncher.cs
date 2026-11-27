using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Advanced_SNES_ROM_Utility.Functions
{
    /// <summary>Starts the current state of a ROM in an emulator, using a temporary copy</summary>
    public static class EmulatorLauncher
    {
        /// <summary>Placeholder for the ROM path in the emulator arguments</summary>
        public const string RomPlaceholder = "%ROM%";

        public const string DefaultArguments = "\"" + RomPlaceholder + "\"";

        // Temporary ROM copies of this session (deleted on exit; save files of the emulator stay)
        private static readonly List<string> _createdFiles = new List<string>();

        public static string TempFolder
        {
            get { return Path.Combine(Path.GetTempPath(), "Advanced SNES ROM Utility"); }
        }

        /// <summary>Builds the argument string: replaces %ROM% or appends the quoted ROM path</summary>
        public static string BuildArguments(string arguments, string romPath)
        {
            if (string.IsNullOrWhiteSpace(arguments)) { arguments = DefaultArguments; }

            if (arguments.IndexOf(RomPlaceholder, StringComparison.OrdinalIgnoreCase) < 0)
            {
                return arguments.Trim() + " \"" + romPath + "\"";
            }

            return ReplaceIgnoreCase(arguments, RomPlaceholder, romPath);
        }

        /// <summary>Writes the ROM data to a temporary file and starts the emulator with it</summary>
        public static void Run(byte[] romData, string romFileName, string emulatorPath, string arguments)
        {
            if (string.IsNullOrEmpty(emulatorPath) || !File.Exists(emulatorPath))
            {
                throw new FileNotFoundException("The emulator could not be found:\n" + emulatorPath);
            }

            Directory.CreateDirectory(TempFolder);

            // Same file name as the original, so save files of the emulator get the same name each time
            string tempROM = Path.Combine(TempFolder, Path.GetFileName(romFileName));

            try
            {
                File.WriteAllBytes(tempROM, romData);
            }

            catch (IOException)
            {
                // File still opened by a running emulator - use another name
                tempROM = Path.Combine(TempFolder, Path.GetFileNameWithoutExtension(romFileName) + " (" + DateTime.Now.ToString("HHmmss") + ")" + Path.GetExtension(romFileName));
                File.WriteAllBytes(tempROM, romData);
            }

            if (!_createdFiles.Contains(tempROM)) { _createdFiles.Add(tempROM); }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = emulatorPath,
                Arguments = BuildArguments(arguments, tempROM),
                WorkingDirectory = Path.GetDirectoryName(emulatorPath),
                UseShellExecute = false
            };

            Process.Start(startInfo);
        }

        /// <summary>Deletes the temporary ROM copies of this session (files still in use are skipped)</summary>
        public static void Cleanup()
        {
            foreach (string file in _createdFiles)
            {
                try
                {
                    if (File.Exists(file)) { File.Delete(file); }
                }

                catch
                {
                    // still opened by the emulator
                }
            }

            _createdFiles.Clear();
        }

        private static string ReplaceIgnoreCase(string text, string search, string replacement)
        {
            int index = text.IndexOf(search, StringComparison.OrdinalIgnoreCase);

            while (index >= 0)
            {
                text = text.Substring(0, index) + replacement + text.Substring(index + search.Length);
                index = text.IndexOf(search, index + replacement.Length, StringComparison.OrdinalIgnoreCase);
            }

            return text;
        }
    }
}
