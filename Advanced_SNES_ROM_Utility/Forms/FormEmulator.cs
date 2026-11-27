using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Advanced_SNES_ROM_Utility.Functions;

namespace Advanced_SNES_ROM_Utility
{
    /// <summary>Dialog for choosing the emulator used by "Run in Emulator"</summary>
    public class FormEmulator : Form
    {
        private readonly TextBox _textBoxPath = new TextBox();
        private readonly TextBox _textBoxArguments = new TextBox();
        private readonly Button _buttonBrowse = new Button();
        private readonly Button _buttonRemove = new Button();
        private readonly Button _buttonOK = new Button();
        private readonly Button _buttonCancel = new Button();

        public string EmulatorPath { get { return _textBoxPath.Text.Trim().Trim('"'); } }

        public string EmulatorArguments
        {
            get { return string.IsNullOrWhiteSpace(_textBoxArguments.Text) ? EmulatorLauncher.DefaultArguments : _textBoxArguments.Text.Trim(); }
        }

        public FormEmulator(string emulatorPath, string arguments)
        {
            Text = "Set Emulator";
            Font = new Font("Microsoft Sans Serif", 8.25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScaleDimensions = new SizeF(6F, 13F);
            ClientSize = new Size(460, 178);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;

            Label labelPath = new Label { Text = "Emulator (program file)", AutoSize = true, Location = new Point(12, 12) };
            _textBoxPath.Location = new Point(12, 30);
            _textBoxPath.Size = new Size(355, 20);
            _textBoxPath.Text = emulatorPath ?? "";

            _buttonBrowse.Text = "Browse...";
            _buttonBrowse.Location = new Point(373, 28);
            _buttonBrowse.Size = new Size(75, 23);
            _buttonBrowse.Click += ButtonBrowse_Click;

            Label labelArguments = new Label { Text = "Arguments", AutoSize = true, Location = new Point(12, 62) };
            _textBoxArguments.Location = new Point(12, 80);
            _textBoxArguments.Size = new Size(436, 20);
            _textBoxArguments.Text = string.IsNullOrWhiteSpace(arguments) ? EmulatorLauncher.DefaultArguments : arguments;

            Label labelHint = new Label
            {
                Text = "%ROM% is replaced by the path of the ROM. Example for RetroArch:\n-L cores\\snes9x_libretro.dll \"%ROM%\"",
                AutoSize = true,
                ForeColor = SystemColors.GrayText,
                Location = new Point(12, 106)
            };

            _buttonRemove.Text = "Remove";
            _buttonRemove.Location = new Point(12, 143);
            _buttonRemove.Size = new Size(75, 23);
            _buttonRemove.Click += (s, e) => { _textBoxPath.Text = ""; _textBoxArguments.Text = EmulatorLauncher.DefaultArguments; };

            _buttonOK.Text = "OK";
            _buttonOK.Location = new Point(292, 143);
            _buttonOK.Size = new Size(75, 23);
            _buttonOK.Click += ButtonOK_Click;

            _buttonCancel.Text = "Cancel";
            _buttonCancel.Location = new Point(373, 143);
            _buttonCancel.Size = new Size(75, 23);
            _buttonCancel.DialogResult = DialogResult.Cancel;

            AcceptButton = _buttonOK;
            CancelButton = _buttonCancel;

            Controls.AddRange(new Control[] { labelPath, _textBoxPath, _buttonBrowse, labelArguments, _textBoxArguments, labelHint, _buttonRemove, _buttonOK, _buttonCancel });
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Select emulator";
                dialog.Filter = "Programs (*.exe)|*.exe|All Files (*.*)|*.*";

                try
                {
                    if (EmulatorPath.Length > 0 && File.Exists(EmulatorPath)) { dialog.InitialDirectory = Path.GetDirectoryName(EmulatorPath); }
                }

                catch
                {
                    // invalid path in the text box
                }

                if (dialog.ShowDialog(this) == DialogResult.OK) { _textBoxPath.Text = dialog.FileName; }
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (EmulatorPath.Length > 0 && !File.Exists(EmulatorPath))
            {
                MessageBox.Show(this, "The emulator could not be found:\n" + EmulatorPath, "Attention!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
