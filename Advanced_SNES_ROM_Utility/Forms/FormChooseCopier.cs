﻿using System.Windows.Forms;

namespace Advanced_SNES_ROM_Utility
{
    public partial class FormChooseCopier : Form
    {
        public FormChooseCopier(string chooseCopierText)
        {
            InitializeComponent();

            labelCopierText.Text = chooseCopierText;
        }
    }
}