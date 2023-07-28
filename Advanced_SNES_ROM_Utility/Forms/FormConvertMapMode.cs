using System.Drawing;
using System.Windows.Forms;

namespace Advanced_SNES_ROM_Utility
{
    public partial class FormConvertMapMode : Form
    {
        public FormConvertMapMode()
        {
            InitializeComponent();

            pictureBoxInfo.Image = SystemIcons.Information.ToBitmap();
        }

        private void CheckBoxDontShowAgain_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkBoxDontShowAgain.Checked)
            {
                Properties.Settings.Default.ConvertMapModeMessage = true;
            }

            else if (!checkBoxDontShowAgain.Checked)
            {
                Properties.Settings.Default.ConvertMapModeMessage = false;
            }

            Properties.Settings.Default.Save();
        }
    }
}