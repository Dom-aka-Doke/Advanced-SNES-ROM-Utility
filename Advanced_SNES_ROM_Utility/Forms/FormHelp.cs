using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Advanced_SNES_ROM_Utility
{
    public partial class FormHelp : Form
    {
        public FormHelp()
        {
            InitializeComponent();

            Assembly assembly = Assembly.GetExecutingAssembly();
            StreamReader reader = new StreamReader(assembly.GetManifestResourceStream("Advanced_SNES_ROM_Utility.Documents.Manual.html"));
            webBrowserHelp.DocumentText = reader.ReadToEnd();
        }

        private void buttonManualClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}