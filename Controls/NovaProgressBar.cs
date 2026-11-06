using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;

namespace NovaUI.WinForms
{
    /// <summary>What text (if any) is drawn on top of the progress bar.</summary>
    public enum NovaProgressTextDisplay
    {
        None = 0,
        Percent = 1,
        Value = 2
    }

    /// <summary>
    /// Rounded progress bar with optional marquee mode and optional caption.
    /// Painted entirely with GDI+, no system progress primitives involved.
    /// </summary>
    [DefaultProperty("Value")]
    public class NovaProgressBar : Control
    {
        private readonly Timer _marqueeTimer;
        private float _marqueeOffset;
        private int _minimum;
        private int _maximum = 100;
        private int _value;
        private int _step = 10;
        private bool _marquee;
        private int _marqueeSpeed = 30;
        private float _marqueeWidth = 0.35f;
        private int _cornerRadius = 8;
        private Color _fillColor = NovaTheme.Accent;
        private Color _trackColor = NovaTheme.SurfaceColor;
        private Color _textColor = Color.White;
        private NovaProgressTextDisplay _textDisplay = NovaProgressTextDisplay.None;

        public NovaProgressBar()
        {
            SetStyle(Ui.PaintStyles, true);
            UpdateStyles();
            Font = new Font("Segoe UI", 8.25f, FontStyle.Bold);
            Size = new Size(240, 18);
            _marqueeTimer = new Timer();
            _marqueeTimer.Interval = _marqueeSpeed;
            _marqueeTimer.Tick += OnMarqueeTick;
        }

        /// <summary>Lower bound of the range.</summary>
        [Category("Nova"), DefaultValue(0)]
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                if (value > _maximum) value = _maximum;
                _minimum = value;
                if (_value < _minimum) _value = _minimum;
                Invalidate();
            }
        }

        /// <summary>Upper bound of the range.</summary>
        [Category("Nova"), DefaultValue(100)]
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                if (value < _minimum) value = _minimum;
                _maximum = value;
                if (_value > _maximum) _value = _maximum;
                Invalidate();
            }
        }

        /// <summary>Current position between Minimum and Maximum.</summary>
        [Category("Nova"), DefaultValue(0)]
        public int Value
        {
            get { return _value; }
            set
            {
                int clamped = value;
                if (clamped < _minimum) clamped = _minimum;
                if (clamped > _maximum) clamped = _maximum;
                if (clamped == _value) return;
                _value = clamped;
                Invalidate();
                OnValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>Amount added by PerformStep().</summary>
        [Category("Nova"), DefaultValue(10)]
        public int Step
        {
            get { return _step; }
            set { _step = value; }
        }

        /// <summary>True = indeterminate sliding segment instead of Value fill.</summary>
        [Category("Nova"), DefaultValue(false)]
        public bool Marquee
        {
            get { return _marquee; }
            set
            {
                _marquee = value;
                UpdateMarqueeTimer();
                Invalidate();
            }
        }

        /// <summary>Marquee animation interval in milliseconds.</summary>
        [Category("Nova"), DefaultValue(30)]
        public int MarqueeSpeed
        {
            get { return _marqueeSpeed; }
            set
            {
                _marqueeSpeed = value < 10 ? 10 : value;
                _marqueeTimer.Interval = _marqueeSpeed;
            }
        }

        /// <summary>Marquee segment length as a fraction of the track (0.05 .. 1).</summary>
        [Category("Nova"), DefaultValue(0.35f)]
        public float MarqueeWidth
        {
            get { return _marqueeWidth; }
            set
            {
                if (value < 0.05f) value = 0.05f; else if (value > 1f) value = 1f;
                _marqueeWidth = value;
                Invalidate();
            }
        }

        /// <summary>Corner radius in pixels. 0 = square.</summary>
        [Category("Nova"), DefaultValue(8)]
        public int CornerRadius
        {
            get { return _cornerRadius; }
            set { _cornerRadius = value < 0 ? 0 : value; Invalidate(); }
        }

        /// <summary>Fill color of the progress segment.</summary>
        [Category("Nova")]
        public Color FillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; Invalidate(); }
        }

        /// <summary>Color of the unfilled track.</summary>
        [Category("Nova")]
        public Color TrackColor
        {
            get { return _trackColor; }
            set { _trackColor = value; Invalidate(); }
        }

        /// <summary>Caption color when TextDisplay is not None.</summary>
        [Category("Nova")]
        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; Invalidate(); }
        }

        /// <summary>Which caption (if any) is drawn over the bar.</summary>
        [Category("Nova"), DefaultValue(NovaProgressTextDisplay.None)]
        public NovaProgressTextDisplay TextDisplay
        {
            get { return _textDisplay; }
            set { _textDisplay = value; Invalidate(); }
        }

        /// <summary>Raised after Value changes.</summary>
        [Category("Nova")]
        public event EventHandler ValueChanged;

        /// <summary>Adds Step to Value.</summary>
        public void PerformStep()
        {
            Value = _value + _step;
        }

        /// <summary>Adds amount to Value (clamped).</summary>
        public void Increment(int amount)
        {
            Value = _value + amount;
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            EventHandler handler = ValueChanged;
            if (handler != null) handler(this, e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            UpdateMarqueeTimer();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            _marqueeTimer.Enabled = false;
            base.OnHandleDestroyed(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _marqueeTimer != null) _marqueeTimer.Dispose();
            base.Dispose(disposing);
        }

        private void UpdateMarqueeTimer()
        {
            _marqueeTimer.Enabled = _marquee && Visible && !Ui.DesignMode(this);
        }

        private void OnMarqueeTick(object sender, EventArgs e)
        {
            _marqueeOffset += 0.03f;
            if (_marqueeOffset > 1f + _marqueeWidth) _marqueeOffset = -_marqueeWidth;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color parentBack = Parent != null && Parent.BackgroundImage == null ? Parent.BackColor : BackColor;
            g.Clear(parentBack);

            RectangleF rect = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            using (GraphicsPath track = Ui.RoundRect(rect, _cornerRadius))
            {
                using (SolidBrush tb = new SolidBrush(_trackColor)) g.FillPath(tb, track);

                RectangleF fillRect;
                if (_marquee)
                {
                    float w = rect.Width * _marqueeWidth;
                    float x = rect.X + (rect.Width + w) * _marqueeOffset - w;
                    fillRect = new RectangleF(x, rect.Y, w, rect.Height);
                }
                else
                {
                    float frac = _maximum > _minimum ? (float)(_value - _minimum) / (_maximum - _minimum) : 0f;
                    fillRect = new RectangleF(rect.X, rect.Y, rect.Width * frac, rect.Height);
                }

                if (fillRect.Width >= 2f)
                {
                    if (fillRect.Width < rect.Width)
                    {
                        fillRect.Height -= 1f;
                        fillRect.Y += 0.5f;
                    }
                    using (GraphicsPath fillPath = Ui.RoundRect(fillRect, _cornerRadius))
                    using (SolidBrush fb = new SolidBrush(_fillColor))
                    {
                        g.FillPath(fb, fillPath);
                        g.SetClip(track);
                        g.FillPath(fb, fillPath);
                        g.ResetClip();
                    }
                }
            }

            if (_textDisplay != NovaProgressTextDisplay.None)
            {
                string label = _textDisplay == NovaProgressTextDisplay.Percent
                    ? (Maximum > Minimum ? string.Format("{0}%", (int)Math.Round(100f * (_value - _minimum) / (float)(_maximum - _minimum))) : "0%")
                    : string.Format("{0} / {1}", _value, _maximum);
                TextRenderer.DrawText(g, label, Font, new Rectangle(0, 0, Width, Height),
                    _textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }
    }
}
