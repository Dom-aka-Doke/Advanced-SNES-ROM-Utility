using System;
using System.Windows.Forms;

namespace Advanced_SNES_ROM_Utility
{
    public partial class FormHelp : Form
    {
        public FormHelp()
        {
            InitializeComponent();
        }

        private void buttonManualClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
