using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Advanced_SNES_ROM_Utility
{
    /// <summary>Small round status light (green = OK, red = not OK) shown next to the "Checksums" caption</summary>
    public class CheckLight : Control
    {
        private static readonly Color ColorOK = Color.FromArgb(46, 160, 67);
        private static readonly Color ColorBad = Color.FromArgb(207, 34, 46);

        private bool _ok;

        public CheckLight()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Size = new Size(10, 10);
            TabStop = false;
        }

        [DefaultValue(false)]
        public bool OK
        {
            get { return _ok; }
            set { _ok = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle circle = new Rectangle(1, 1, Width - 3, Height - 3);

            using (Brush brush = new SolidBrush(_ok ? ColorOK : ColorBad))
            using (Pen pen = new Pen(Color.FromArgb(70, 0, 0, 0)))
            {
                e.Graphics.FillEllipse(brush, circle);
                e.Graphics.DrawEllipse(pen, circle);
            }
        }
    }
}
