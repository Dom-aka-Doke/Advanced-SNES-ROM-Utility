using System.Drawing;
using System.Windows.Forms;

namespace Advanced_SNES_ROM_Utility
{
    public partial class FormSlowROMFix : Form
    {
        public FormSlowROMFix()
        {
            InitializeComponent();

            pictureBoxInfo.Image = SystemIcons.Information.ToBitmap();
        }

        private void CheckBoxDontShowAgain_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkBoxDontShowAgain.Checked)
            {
                Properties.Settings.Default.SlowROMFixMessage = true;
            }

            else if (!checkBoxDontShowAgain.Checked)
            {
                Properties.Settings.Default.SlowROMFixMessage = false;
            }

            Properties.Settings.Default.Save();
        }
    }
}