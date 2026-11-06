using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;

namespace NovaUI.WinForms
{
    /// <summary>
    /// Custom-drawn label with crisp text, optional accent dot prefix and
    /// correct AutoSize support.
    /// </summary>
    [DefaultProperty("Text")]
    public class NovaLabel : Control
    {
        private bool _showDot;
        private Color _dotColor = NovaTheme.Accent;
        private bool _autoEllipsis = true;

        public NovaLabel()
        {
            SetStyle(Ui.PaintStyles, true);
            UpdateStyles();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            ForeColor = NovaTheme.TextColor;
            Font = new Font("Segoe UI", 9.75f);
            Size = new Size(120, 22);
        }

        /// <summary>Show a small accent-colored dot before the text.</summary>
        [Category("Nova"), DefaultValue(false)]
        public bool ShowDot
        {
            get { return _showDot; }
            set { _showDot = value; Invalidate(); }
        }

        /// <summary>Color of the accent dot.</summary>
        [Category("Nova")]
        public Color DotColor
        {
            get { return _dotColor; }
            set { _dotColor = value; Invalidate(); }
        }

        /// <summary>Truncate overflowing text with an ellipsis.</summary>
        [Category("Nova"), DefaultValue(true)]
        public bool AutoEllipsis
        {
            get { return _autoEllipsis; }
            set { _autoEllipsis = value; Invalidate(); }
        }

        /// <summary>Text alignment inside the label.</summary>
        [Category("Nova"), DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment TextAlign { get; set; } = ContentAlignment.MiddleLeft;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; Invalidate(); }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size s = TextRenderer.MeasureText(Text, Font);
            int extra = Padding.Horizontal + (_showDot ? 14 : 0);
            return new Size(s.Width + extra + 2, s.Height + Padding.Vertical);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int textX = Padding.Left;
            if (_showDot)
            {
                int cy = Height / 2;
                using (SolidBrush b = new SolidBrush(_dotColor))
                {
                    g.FillEllipse(b, Padding.Left + 1, cy - 4, 8, 8);
                }
                textX += 14;
            }

            TextFormatFlags flags = Ui.TextFlags(TextAlign);
            if (_autoEllipsis) flags |= TextFormatFlags.EndEllipsis;
            Rectangle r = new Rectangle(textX, Padding.Top, Width - textX - Padding.Right, Height - Padding.Vertical);
            TextRenderer.DrawText(g, Text, Font, r, ForeColor, flags);
        }
    }
}
