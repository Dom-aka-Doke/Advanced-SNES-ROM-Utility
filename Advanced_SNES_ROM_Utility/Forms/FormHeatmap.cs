using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Advanced_SNES_ROM_Utility.Functions;

namespace Advanced_SNES_ROM_Utility
{
    /// <summary>
    /// ROM Heatmap: shows the ROM split into banks (32 kByte LoROM / 64 kByte HiROM) and highlights
    /// identical (duplicated) and empty banks, or the entropy of each bank.
    /// </summary>
    public class FormHeatmap : Form
    {
        private static readonly Color ColorUnique = Color.FromArgb(42, 120, 214);
        private static readonly Color ColorDuplicate = Color.FromArgb(235, 120, 40);
        private static readonly Color ColorEmpty = Color.FromArgb(200, 200, 205);
        private static readonly Color ColorHeader = Color.FromArgb(30, 30, 30);

        // Sequential ramp (light -> dark blue) for the entropy view
        private static readonly Color[] Ramp =
        {
            Color.FromArgb(205, 226, 251), Color.FromArgb(183, 211, 246), Color.FromArgb(158, 197, 244), Color.FromArgb(134, 182, 239),
            Color.FromArgb(109, 167, 236), Color.FromArgb(85, 152, 231), Color.FromArgb(57, 135, 229), Color.FromArgb(42, 120, 214),
            Color.FromArgb(37, 106, 191), Color.FromArgb(28, 92, 171), Color.FromArgb(24, 79, 149), Color.FromArgb(16, 66, 129),
            Color.FromArgb(13, 54, 107)
        };

        private const int BanksPerRow = 16;
        private const int RowLabelWidth = 64;
        private const int CellHeight = 26;
        private const int CellGap = 3;

        private SNESROM _sourceROM;
        private BankAnalysis _analysis;
        private OverdumpInfo _overdump;
        private bool _entropyView;
        private int _hoverBank = -1;

        private readonly Label _labelName = new Label();
        private readonly Label _labelInfo = new Label();
        private readonly ComboBox _comboBankSize = new ComboBox();
        private readonly RadioButton _radioDuplicates = new RadioButton();
        private readonly RadioButton _radioEntropy = new RadioButton();
        private readonly Label _labelSummary = new Label();
        private readonly Label _labelLegend = new Label();
        private readonly DoubleBufferedPanel _panelGrid = new DoubleBufferedPanel();
        private readonly ListBox _listGroups = new ListBox();
        private readonly Label _labelGroups = new Label();
        private readonly Button _buttonClose = new Button();
        private readonly ToolTip _toolTip = new ToolTip();

        public FormHeatmap(SNESROM sourceROM)
        {
            Text = "ROM Heatmap - " + Application.ProductName + " v" + ProductVersionShort();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            ClientSize = new Size(760, 600);
            MinimumSize = new Size(560, 420);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Microsoft Sans Serif", 8.25F);
            KeyPreview = true;

            _labelName.Font = new Font(Font, FontStyle.Bold);
            _labelName.AutoSize = true;
            _labelName.Location = new Point(12, 12);

            _labelInfo.AutoSize = true;
            _labelInfo.Location = new Point(12, 30);

            Label labelBankSize = new Label { Text = "Bank size", AutoSize = true, Location = new Point(400, 15) };
            _comboBankSize.DropDownStyle = ComboBoxStyle.DropDownList;
            _comboBankSize.Items.AddRange(new object[] { "Auto", "32 kByte", "64 kByte" });
            _comboBankSize.SelectedIndex = 0;
            _comboBankSize.Location = new Point(460, 11);
            _comboBankSize.Size = new Size(100, 21);
            _comboBankSize.SelectedIndexChanged += (s, e) => RefreshData();

            _radioDuplicates.Text = "Duplicates";
            _radioDuplicates.Checked = true;
            _radioDuplicates.AutoSize = true;
            _radioDuplicates.Location = new Point(580, 13);
            _radioDuplicates.CheckedChanged += (s, e) => { _entropyView = _radioEntropy.Checked; UpdateLegend(); _panelGrid.Invalidate(); };

            _radioEntropy.Text = "Entropy";
            _radioEntropy.AutoSize = true;
            _radioEntropy.Location = new Point(670, 13);

            _labelSummary.AutoSize = false;
            _labelSummary.Location = new Point(12, 52);
            _labelSummary.Size = new Size(736, 18);
            _labelSummary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            _labelLegend.AutoSize = false;
            _labelLegend.Location = new Point(12, 72);
            _labelLegend.Size = new Size(736, 22);
            _labelLegend.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _labelLegend.Paint += LabelLegend_Paint;

            _panelGrid.Location = new Point(12, 98);
            _panelGrid.Size = new Size(736, 360);
            _panelGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _panelGrid.BorderStyle = BorderStyle.FixedSingle;
            _panelGrid.BackColor = Color.White;
            _panelGrid.AutoScroll = true;
            _panelGrid.Paint += PanelGrid_Paint;
            _panelGrid.MouseMove += PanelGrid_MouseMove;
            _panelGrid.MouseLeave += (s, e) => { _hoverBank = -1; _toolTip.Hide(_panelGrid); _panelGrid.Invalidate(); };
            _panelGrid.Resize += (s, e) => UpdateScrollSize();
            _panelGrid.Scroll += (s, e) => _panelGrid.Invalidate();

            _labelGroups.Text = "Identical banks";
            _labelGroups.Font = new Font(Font, FontStyle.Bold);
            _labelGroups.AutoSize = true;
            _labelGroups.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            _labelGroups.Location = new Point(12, 466);

            _listGroups.Location = new Point(12, 484);
            _listGroups.Size = new Size(736, 82);
            _listGroups.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _listGroups.IntegralHeight = false;

            _buttonClose.Text = "Close";
            _buttonClose.Size = new Size(80, 23);
            _buttonClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _buttonClose.Location = new Point(668, 571);
            _buttonClose.Click += (s, e) => Close();
            CancelButton = _buttonClose;

            Controls.AddRange(new Control[] { _labelName, _labelInfo, labelBankSize, _comboBankSize, _radioDuplicates, _radioEntropy,
                                              _labelSummary, _labelLegend, _panelGrid, _labelGroups, _listGroups, _buttonClose });

            Layout += (s, e) => LayoutHeaderControls(labelBankSize);

            _sourceROM = sourceROM;
            UpdateLegend();
            RefreshData();
        }

        private static string ProductVersionShort()
        {
            Version version = new Version(Application.ProductVersion);
            return version.Major + "." + version.Minor;
        }

        /// <summary>Shows a new or changed ROM</summary>
        public void UpdateROM(SNESROM sourceROM)
        {
            _sourceROM = sourceROM;
            RefreshData();
        }

        private void RefreshData()
        {
            if (_sourceROM == null || _sourceROM.SourceROM == null) { return; }

            Cursor = Cursors.WaitCursor;

            int bankSize = 0;
            if (_comboBankSize.SelectedIndex == 1) { bankSize = 0x8000; }
            else if (_comboBankSize.SelectedIndex == 2) { bankSize = 0x10000; }

            _analysis = _sourceROM.AnalyzeBanks(bankSize);
            _overdump = _sourceROM.DetectOverdump();

            _labelName.Text = System.IO.Path.GetFileName(_sourceROM.ROMFullPath);
            _labelInfo.Text = _sourceROM.StringMapMode + " · " + SNESROMFunction.FormatBytes(_analysis.ROMSize) + " · " +
                              _analysis.Banks.Count + (_analysis.Banks.Count == 1 ? " bank" : " banks") + " of " + (_analysis.BankSize / 1024) + " kByte" +
                              (_sourceROM.IsInterleaved ? " · interleaved" : string.Empty);

            string overdumpText = !_overdump.Supported ? "Overdump: " + _overdump.Reason :
                                  _overdump.Overdump ? "Overdump: " + (_overdump.Type == "mirror" ? "mirrored" : "padded") + " data, real size " + SNESROMFunction.FormatBytes(_overdump.RealSize) :
                                  "Overdump: none found";

            _labelSummary.Text = "Banks: " + _analysis.Banks.Count + "     Unique: " + _analysis.Unique + "     In duplicate groups: " + _analysis.Duplicates +
                                 " (" + _analysis.Copies + " redundant)     Empty: " + _analysis.Empty + "     " + overdumpText;

            _listGroups.Items.Clear();

            if (_analysis.Groups.Count == 0)
            {
                _listGroups.Items.Add("No identical banks found.");
            }

            else
            {
                foreach (List<int> group in _analysis.Groups)
                {
                    _listGroups.Items.Add("Banks " + string.Join(", ", group.Select(b => b.ToString("X2") + " (" + BankAddress(_analysis.Banks[b]) + ")")) + " are identical");
                }
            }

            UpdateScrollSize();
            _panelGrid.Invalidate();
            Cursor = Cursors.Default;
        }

        private static string BankAddress(BankInfo bank)
        {
            string offset = "$" + bank.Offset.ToString("X6");
            return string.IsNullOrEmpty(bank.SNESAddress) ? offset : offset + " / " + bank.SNESAddress;
        }

        // Place bank size and view selection right aligned, measured at the current DPI
        private void LayoutHeaderControls(Label labelBankSize)
        {
            int right = ClientSize.Width - 12;
            Size entropy = _radioEntropy.GetPreferredSize(Size.Empty);
            Size duplicates = _radioDuplicates.GetPreferredSize(Size.Empty);
            _radioEntropy.Location = new Point(right - entropy.Width, 13);
            _radioDuplicates.Location = new Point(_radioEntropy.Left - 12 - duplicates.Width, 13);
            _comboBankSize.Location = new Point(_radioDuplicates.Left - 20 - _comboBankSize.Width, 11);
            labelBankSize.Location = new Point(_comboBankSize.Left - 6 - labelBankSize.GetPreferredSize(Size.Empty).Width, 15);
        }

        private int CellWidth()
        {
            int width = _panelGrid.ClientSize.Width - RowLabelWidth - 8;
            return Math.Max(24, (width - (BanksPerRow - 1) * CellGap) / BanksPerRow);
        }

        private void UpdateScrollSize()
        {
            if (_analysis == null) { return; }
            int rows = (_analysis.Banks.Count + BanksPerRow - 1) / BanksPerRow;
            _panelGrid.AutoScrollMinSize = new Size(0, 8 + rows * (CellHeight + CellGap));
        }

        private Rectangle CellRectangle(int index)
        {
            int cellWidth = CellWidth();
            int row = index / BanksPerRow;
            int column = index % BanksPerRow;
            return new Rectangle(RowLabelWidth + column * (cellWidth + CellGap), 4 + row * (CellHeight + CellGap) + _panelGrid.AutoScrollPosition.Y, cellWidth, CellHeight);
        }

        private Color CellColor(BankInfo bank, out bool darkBackground)
        {
            if (_entropyView)
            {
                int i = (int)Math.Round(bank.Entropy / 8 * (Ramp.Length - 1));
                i = Math.Max(0, Math.Min(Ramp.Length - 1, i));
                darkBackground = i >= 7;
                return Ramp[i];
            }

            if (bank.Fill >= 0) { darkBackground = false; return ColorEmpty; }
            if (bank.Group >= 0) { darkBackground = false; return ColorDuplicate; }
            darkBackground = true;
            return ColorUnique;
        }

        private void PanelGrid_Paint(object sender, PaintEventArgs e)
        {
            if (_analysis == null) { return; }

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            StringFormat center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            using (Font cellFont = new Font("Consolas", 8F))
            using (Brush labelBrush = new SolidBrush(Color.FromArgb(90, 90, 100)))
            {
                for (int i = 0; i < _analysis.Banks.Count; i++)
                {
                    BankInfo bank = _analysis.Banks[i];
                    Rectangle rect = CellRectangle(i);

                    if (rect.Bottom < 0 || rect.Top > _panelGrid.ClientSize.Height) { continue; }

                    // Offset of the row
                    if (i % BanksPerRow == 0)
                    {
                        g.DrawString("$" + bank.Offset.ToString("X6"), cellFont, labelBrush, new RectangleF(0, rect.Top, RowLabelWidth - 4, rect.Height), new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
                    }

                    bool dark;
                    Color color = CellColor(bank, out dark);

                    using (GraphicsPath path = RoundedRectangle(rect, 3))
                    using (Brush brush = new SolidBrush(color))
                    {
                        g.FillPath(brush, path);

                        if (bank.Header)
                        {
                            using (Pen pen = new Pen(ColorHeader, 2)) { g.DrawPath(pen, path); }
                        }

                        if (i == _hoverBank)
                        {
                            using (Pen pen = new Pen(Color.FromArgb(160, 0, 0, 0), 1)) { g.DrawPath(pen, path); }
                        }
                    }

                    using (Brush textBrush = new SolidBrush(dark ? Color.White : Color.FromArgb(40, 40, 50)))
                    {
                        g.DrawString(i.ToString("X2"), cellFont, textBrush, rect, center);
                    }
                }
            }
        }

        private static GraphicsPath RoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void PanelGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (_analysis == null) { return; }

            int found = -1;
            for (int i = 0; i < _analysis.Banks.Count; i++)
            {
                if (CellRectangle(i).Contains(e.Location)) { found = i; break; }
            }

            if (found == _hoverBank) { return; }

            _hoverBank = found;
            _panelGrid.Invalidate();

            if (found < 0) { _toolTip.Hide(_panelGrid); return; }

            BankInfo bank = _analysis.Banks[found];
            string status;

            if (bank.Fill >= 0) { status = "Empty (filled with 0x" + bank.Fill.ToString("X2") + ")"; }
            else if (bank.Group >= 0 && bank.DuplicateOf < 0) { status = "Original - identical to bank(s) " + string.Join(", ", _analysis.Groups[bank.Group].Where(b => b != found).Select(b => b.ToString("X2"))); }
            else if (bank.Group >= 0) { status = "Duplicate of bank " + bank.DuplicateOf.ToString("X2"); }
            else { status = "Unique data"; }

            string text = "Bank " + found.ToString("X2") + "\n" +
                          "File offset: $" + bank.Offset.ToString("X6") + (string.IsNullOrEmpty(bank.SNESAddress) ? string.Empty : "\nSNES address: " + bank.SNESAddress) + "\n" +
                          status + "\n" +
                          "Entropy: " + bank.Entropy.ToString("0.00") + " bits per byte" +
                          (bank.Header ? "\nContains the internal header" : string.Empty);

            Rectangle rect = CellRectangle(found);
            _toolTip.Show(text, _panelGrid, rect.Left, rect.Bottom + 4, 8000);
        }

        private void UpdateLegend()
        {
            _labelLegend.Invalidate();
        }

        private void LabelLegend_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int x = 0;

            Action<Color, bool, string> item = (color, ring, text) =>
            {
                Rectangle swatch = new Rectangle(x, 4, 14, 14);
                using (GraphicsPath path = RoundedRectangle(swatch, 2))
                {
                    using (Brush brush = new SolidBrush(color)) { g.FillPath(brush, path); }
                    if (ring) { using (Pen pen = new Pen(ColorHeader, 2)) { g.DrawPath(pen, path); } }
                }
                x += 18;
                TextRenderer.DrawText(g, text, Font, new Point(x, 4), ForeColor);
                x += TextRenderer.MeasureText(text, Font).Width + 14;
            };

            if (!_entropyView)
            {
                item(ColorUnique, false, "Unique");
                item(ColorDuplicate, false, "Duplicate (identical bank)");
                item(ColorEmpty, false, "Empty (only 0x00 or 0xFF)");
                item(Color.White, true, "Internal header");
            }

            else
            {
                TextRenderer.DrawText(g, "Entropy (bits per byte):  0", Font, new Point(x, 4), ForeColor);
                x += TextRenderer.MeasureText("Entropy (bits per byte):  0", Font).Width + 2;

                Rectangle ramp = new Rectangle(x, 4, 140, 14);
                using (LinearGradientBrush brush = new LinearGradientBrush(ramp, Ramp[0], Ramp[Ramp.Length - 1], LinearGradientMode.Horizontal))
                {
                    ColorBlend blend = new ColorBlend
                    {
                        Colors = Ramp,
                        Positions = Enumerable.Range(0, Ramp.Length).Select(i => (float)i / (Ramp.Length - 1)).ToArray()
                    };
                    brush.InterpolationColors = blend;
                    g.FillRectangle(brush, ramp);
                }

                x += 144;
                TextRenderer.DrawText(g, "8     Low = empty / repetitive, high = compressed / random data", Font, new Point(x, 4), ForeColor);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { Close(); }
            base.OnKeyDown(e);
        }

        /// <summary>Panel without flickering</summary>
        private class DoubleBufferedPanel : Panel
        {
            public DoubleBufferedPanel()
            {
                DoubleBuffered = true;
                ResizeRedraw = true;
            }
        }
    }
}
