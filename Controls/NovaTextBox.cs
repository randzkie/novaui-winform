using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;

namespace NovaUI.WinForms
{
    /// <summary>
    /// Rounded single-line text box with placeholder text and an accent border
    /// on focus. Hosts a real TextBox child, so IME, clipboard and context
    /// menu behavior are the native ones.
    /// </summary>
    [DefaultEvent("TextChanged")]
    public class NovaTextBox : UserControl
    {
        private readonly TextBox _inner;
        private bool _focused;
        private int _cornerRadius = 8;
        private Color _borderColor = NovaTheme.BorderColor;
        private Color _focusColor = NovaTheme.Accent;
        private Color _fillColor = NovaTheme.BackColor;
        private string _placeholder = "";
        private bool _readOnly;

        public NovaTextBox()
        {
            SetStyle(Ui.PaintStyles, true);
            UpdateStyles();
            _inner = new TextBox();
            _inner.BorderStyle = BorderStyle.None;
            _inner.Font = new Font("Segoe UI", 9.75f);
            _inner.ForeColor = NovaTheme.TextColor;
            _inner.BackColor = _fillColor;
            _inner.GotFocus += InnerOnGotFocus;
            _inner.LostFocus += InnerOnLostFocus;
            _inner.TextChanged += InnerOnTextChanged;
            _inner.KeyDown += InnerOnKeyDown;
            _inner.KeyPress += InnerOnKeyPress;
            Controls.Add(_inner);
            Size = new Size(220, 34);
            BackColor = _fillColor;
            Cursor = Cursors.IBeam;
        }

        /// <summary>Corner radius in pixels. 0 = square.</summary>
        [Category("Nova"), DefaultValue(8)]
        public int CornerRadius
        {
            get { return _cornerRadius; }
            set { _cornerRadius = value < 0 ? 0 : value; Invalidate(); }
        }

        /// <summary>Gray text shown while the box is empty and unfocused.</summary>
        [Category("Nova"), DefaultValue("")]
        public string Placeholder
        {
            get { return _placeholder; }
            set { _placeholder = value ?? ""; Invalidate(); }
        }

        /// <summary>Border color when the box is not focused.</summary>
        [Category("Nova")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; Invalidate(); }
        }

        /// <summary>Border color when the box has focus.</summary>
        [Category("Nova")]
        public Color FocusColor
        {
            get { return _focusColor; }
            set { _focusColor = value; Invalidate(); }
        }

        /// <summary>Background fill of the box.</summary>
        [Category("Nova")]
        public Color FillColor
        {
            get { return _fillColor; }
            set
            {
                _fillColor = value;
                BackColor = value;
                _inner.BackColor = value;
                Invalidate();
            }
        }

        /// <summary>Current text.</summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return _inner.Text; }
            set { _inner.Text = value; }
        }

        /// <summary>Prevent editing (text can still be selected and copied).</summary>
        [Category("Behavior"), DefaultValue(false)]
        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                _readOnly = value;
                _inner.ReadOnly = value;
                _inner.BackColor = value ? Ui.Blend(_fillColor, Color.Gray, 0.08f) : _fillColor;
            }
        }

        /// <summary>Maximum number of characters.</summary>
        [Category("Behavior"), DefaultValue(32767)]
        public int MaxLength
        {
            get { return _inner.MaxLength; }
            set { _inner.MaxLength = value; }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new event EventHandler TextChanged;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new event KeyEventHandler KeyDown;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new event KeyPressEventHandler KeyPress;

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            _inner.BackColor = _fillColor;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            _inner.Font = Font;
            LayoutInner();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            _inner.ForeColor = ForeColor;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            LayoutInner();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF rect = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            Color border = _focused ? _focusColor : _borderColor;

            using (GraphicsPath path = Ui.RoundRect(rect, _cornerRadius))
            {
                using (SolidBrush b = new SolidBrush(_fillColor)) g.FillPath(b, path);
                using (Pen p = new Pen(border, _focused ? 1.6f : 1f)) g.DrawPath(p, path);
            }

            if (!_focused && _inner.TextLength == 0 && _placeholder.Length > 0)
            {
                Rectangle pr = new Rectangle(10, 0, Width - 20, Height);
                TextRenderer.DrawText(g, _placeholder, Font, pr,
                    Ui.Blend(_borderColor, NovaTheme.MutedColor, 0.5f),
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }
        }

        private void LayoutInner()
        {
            _inner.SetBounds(10, (Height - _inner.Height) / 2, Width - 20, _inner.Height);
        }

        private void InnerOnGotFocus(object sender, EventArgs e)
        {
            _focused = true; Invalidate();
        }

        private void InnerOnLostFocus(object sender, EventArgs e)
        {
            _focused = false; Invalidate();
        }

        private void InnerOnTextChanged(object sender, EventArgs e)
        {
            Invalidate();
            EventHandler handler = TextChanged;
            if (handler != null) handler(this, e);
        }

        private void InnerOnKeyDown(object sender, KeyEventArgs e)
        {
            KeyEventHandler handler = KeyDown;
            if (handler != null) handler(this, e);
        }

        private void InnerOnKeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPressEventHandler handler = KeyPress;
            if (handler != null) handler(this, e);
        }

        /// <summary>Move keyboard focus into the inner text box.</summary>
        public void FocusInput()
        {
            _inner.Focus();
        }
    }
}
