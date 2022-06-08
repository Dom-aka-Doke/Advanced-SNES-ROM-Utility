using System;
using System.Windows.Forms;

namespace Advanced_SNES_ROM_Utility
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        private void buttonAboutClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabelAboutCommunityLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                linkLabelAboutCommunityLink.LinkVisited = true;
                System.Diagnostics.Process.Start("https://snes-projects.de");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link!\n\n" + ex.ToString());
            }
        }

        private void linkLabelAboutLogo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                linkLabelAboutLogo.LinkVisited = true;
                System.Diagnostics.Process.Start("http://www.iconarchive.com/artist/chrisbanks2.html");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link!\n\n" + ex.ToString());
            }
        }
    }
}