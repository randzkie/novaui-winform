using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;

namespace NovaUI.WinForms
{
    /// <summary>
    /// Flat rounded button with hover/press states, painted entirely with GDI+.
    /// Two looks: filled (default) and outline. All colors default from NovaTheme.
    /// </summary>
    public class NovaButton : Control
    {
        private bool _hover;
        private bool _pressed;
        private int _cornerRadius = 10;
        private Color _accent = NovaTheme.Accent;
        private Color _accentHover = NovaTheme.AccentHover;
        private Color _textColor = Color.White;
        private Color _borderColor = NovaTheme.Accent;
        private float _borderWidth = 1.5f;
        private bool _outline;

        public NovaButton()
        {
            SetStyle(Ui.PaintStyles, true);
            UpdateStyles();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 9.75f, FontStyle.Bold);
            Size = new Size(140, 38);
            Cursor = Cursors.Hand;
        }

        /// <summary>Corner radius in pixels. 0 = square.</summary>
        [Category("Nova"), DefaultValue(10)]
        public int CornerRadius
        {
            get { return _cornerRadius; }
            set { _cornerRadius = value < 0 ? 0 : value; Invalidate(); }
        }

        /// <summary>Fill color in filled mode / border+text color in outline mode.</summary>
        [Category("Nova")]
        public Color AccentColor
        {
            get { return _accent; }
            set { _accent = value; Invalidate(); }
        }

        /// <summary>Fill color while the pointer is over the button (filled mode).</summary>
        [Category("Nova")]
        public Color HoverColor
        {
            get { return _accentHover; }
            set { _accentHover = value; Invalidate(); }
        }

        /// <summary>Text color in filled mode.</summary>
        [Category("Nova")]
        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; Invalidate(); }
        }

        /// <summary>Border color in outline mode.</summary>
        [Category("Nova")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; Invalidate(); }
        }

        /// <summary>Border thickness in outline mode (pixels).</summary>
        [Category("Nova"), DefaultValue(1.5f)]
        public float BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value <= 0f ? 1f : value; Invalidate(); }
        }

        /// <summary>True = ghost/outline look (transparent fill, colored border and text).</summary>
        [Category("Nova"), DefaultValue(false)]
        public bool Outline
        {
            get { return _outline; }
            set { _outline = value; Invalidate(); }
        }

        /// <summary>Text alignment inside the button.</summary>
        [Category("Nova"), DefaultValue(ContentAlignment.MiddleCenter)]
        public ContentAlignment TextAlign { get; set; } = ContentAlignment.MiddleCenter;

        /// <summary>Text the button displays (forwarded to Control.Text).</summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; Invalidate(); }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _hover = true; Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false; _pressed = false; Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) _pressed = true;
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _pressed = false; Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF rect = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);

            Color face;
            Color label;
            if (!Enabled)
            {
                face = _outline ? Color.Transparent : Ui.Blend(_accent, Color.Gray, 0.55f);
                label = Ui.Blend(_outline ? _borderColor : _textColor, Color.Gray, 0.5f);
            }
            else if (_pressed)
            {
                face = _outline ? Ui.Blend(_accent, Color.White, 0.85f) : Ui.Darken(_accent, 0.18f);
                label = _outline ? _accent : _textColor;
            }
            else if (_hover)
            {
                face = _outline ? Ui.Blend(_accent, Color.White, 0.92f) : _accentHover;
                label = _outline ? _accent : _textColor;
            }
            else
            {
                face = _outline ? Color.Transparent : _accent;
                label = _outline ? _borderColor : _textColor;
            }

            using (GraphicsPath path = Ui.RoundRect(rect, _cornerRadius))
            {
                if (face != Color.Transparent && face.A > 0)
                {
                    using (SolidBrush b = new SolidBrush(face)) g.FillPath(b, path);
                }
                if (_outline)
                {
                    using (Pen p = new Pen(!Enabled ? Ui.Blend(_borderColor, Color.Gray, 0.5f) : _borderColor, _borderWidth))
                        g.DrawPath(p, path);
                }
            }

            TextRenderer.DrawText(g, Text, Font, new Rectangle(0, 0, Width, Height),
                label, Ui.TextFlags(TextAlign));
        }
    }
}
