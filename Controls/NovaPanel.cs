using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;

namespace NovaUI.WinForms
{
    /// <summary>
    /// Rounded, optionally titled panel (card). Clipping is done through a
    /// Region so child controls never paint over the rounded corners.
    /// </summary>
    public class NovaPanel : Panel
    {
        private int _cornerRadius = 12;
        private Color _fillColor = NovaTheme.SurfaceColor;
        private Color _borderColor = NovaTheme.BorderColor;
        private bool _showBorder;
        private string _title = "";
        private Color _titleColor = NovaTheme.TextColor;
        private bool _showTitle;

        public NovaPanel()
        {
            SetStyle(Ui.PaintStyles, true);
            UpdateStyles();
            DoubleBuffered = true;
            Padding = new Padding(16);
        }

        /// <summary>Corner radius in pixels. 0 = square.</summary>
        [Category("Nova"), DefaultValue(12)]
        public int CornerRadius
        {
            get { return _cornerRadius; }
            set { _cornerRadius = value < 0 ? 0 : value; RefreshRegion(); Invalidate(); }
        }

        /// <summary>Card background color.</summary>
        [Category("Nova")]
        public Color FillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; Invalidate(); }
        }

        /// <summary>Border color (only drawn when ShowBorder is true).</summary>
        [Category("Nova")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; Invalidate(); }
        }

        /// <summary>Draw a 1px border around the card.</summary>
        [Category("Nova"), DefaultValue(false)]
        public bool ShowBorder
        {
            get { return _showBorder; }
            set { _showBorder = value; Invalidate(); }
        }

        /// <summary>Card title text shown top-left (when ShowTitle is true).</summary>
        [Category("Nova"), DefaultValue("")]
        public string Title
        {
            get { return _title; }
            set { _title = value ?? ""; Invalidate(); }
        }

        /// <summary>Title text color.</summary>
        [Category("Nova")]
        public Color TitleColor
        {
            get { return _titleColor; }
            set { _titleColor = value; Invalidate(); }
        }

        /// <summary>Show the Title above the content area.</summary>
        [Category("Nova"), DefaultValue(false)]
        public bool ShowTitle
        {
            get { return _showTitle; }
            set { _showTitle = value; Invalidate(); }
        }

        private void RefreshRegion()
        {
            Region old = Region;
            Region = new Region(Ui.RoundRect(new RectangleF(0f, 0f, Width, Height), _cornerRadius));
            if (old != null) old.Dispose();
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            RefreshRegion();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF rect = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);

            using (GraphicsPath path = Ui.RoundRect(rect, _cornerRadius))
            {
                using (SolidBrush b = new SolidBrush(_fillColor)) g.FillPath(b, path);
                if (_showBorder)
                {
                    using (Pen p = new Pen(_borderColor, 1f)) g.DrawPath(p, path);
                }
            }

            if (_showTitle && _title.Length > 0)
            {
                Rectangle titleRect = new Rectangle(Padding.Left, 8, Width - Padding.Horizontal, Font.Height + 4);
                TextRenderer.DrawText(g, _title, Font, titleRect, _titleColor,
                    TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            }

            base.OnPaint(e);
        }
    }
}
