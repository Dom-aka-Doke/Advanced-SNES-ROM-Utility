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
            if (args.Length > 0)
            {
                SNESROMCommandline.Run(args);
            }

            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }
        }
    }
}