using System;
using System.Windows.Forms;
using Advanced_SNES_ROM_Utility.Commandline;

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
            // A single file (f.e. "Open with" or a ROM dropped onto the program) opens the window
            bool openFile = args.Length == 1 && !args[0].StartsWith("-") && System.IO.File.Exists(args[0]);

            if (args.Length > 0 && !openFile)
            {
                SNESROMCommandline.Run(args);
            }

            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain(openFile ? args[0] : null));
            }
        }
    }
}