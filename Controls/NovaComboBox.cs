using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;

namespace NovaUI.WinForms
{
    /// <summary>
    /// Flat, rounded owner-drawn combo box. Works best with
    /// DropDownStyle = DropDownList (the default here). The control repaints
    /// its whole face on WM_PAINT so the stock flat border and arrow never
    /// show through.
    /// </summary>
    [DefaultEvent("SelectedIndexChanged")]
    public class NovaComboBox : ComboBox
    {
        private bool _hover;
        private int _cornerRadius = 8;
        private Color _fillColor = NovaTheme.BackColor;
        private Color _borderColor = NovaTheme.BorderColor;
        private Color _focusBorderColor = NovaTheme.Accent;
        private Color _itemHoverColor = Color.FromArgb(238, 240, 252);
        private Color _arrowColor = NovaTheme.MutedColor;

        private const int WmPaint = 0x000F;

        public NovaComboBox()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            DrawMode = DrawMode.OwnerDrawFixed;
            FlatStyle = FlatStyle.Flat;
            DropDownStyle = ComboBoxStyle.DropDownList;
            ItemHeight = 24;
            Font = new Font("Segoe UI", 9.75f);
            Size = new Size(200, 34);
            BackColor = _fillColor;
            ForeColor = NovaTheme.TextColor;
            Cursor = Cursors.Hand;
        }

        /// <summary>Corner radius in pixels. 0 = square.</summary>
        [Category("Nova"), DefaultValue(8)]
        public int CornerRadius
        {
            get { return _cornerRadius; }
            set { _cornerRadius = value < 0 ? 0 : value; Invalidate(); }
        }

        /// <summary>Background of the closed control and of list rows.</summary>
        [Category("Nova")]
        public Color FillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; BackColor = value; Invalidate(); }
        }

        /// <summary>Border color when the control is not focused.</summary>
        [Category("Nova")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; Invalidate(); }
        }

        /// <summary>Border color when focused or dropped down.</summary>
        [Category("Nova")]
        public Color FocusBorderColor
        {
            get { return _focusBorderColor; }
            set { _focusBorderColor = value; Invalidate(); }
        }

        /// <summary>Highlight color of the row under the pointer in the list.</summary>
        [Category("Nova")]
        public Color ItemHoverColor
        {
            get { return _itemHoverColor; }
            set { _itemHoverColor = value; Invalidate(); }
        }

        /// <summary>Color of the dropdown chevron.</summary>
        [Category("Nova")]
        public Color ArrowColor
        {
            get { return _arrowColor; }
            set { _arrowColor = value; Invalidate(); }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _hover = true; Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false; Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            Invalidate();
            base.OnDropDownClosed(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            Invalidate();
            base.OnSelectedIndexChanged(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WmPaint)
            {
                using (Graphics g = CreateGraphics())
                {
                    DrawFace(g);
                }
            }
        }

        private void DrawFace(Graphics g)
        {
            Color parentBack = Parent != null && Parent.BackgroundImage == null ? Parent.BackColor : BackColor;
            g.Clear(parentBack);

            Rectangle r = new Rectangle(0, 0, Width, Height);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF rect = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);

            using (GraphicsPath path = Ui.RoundRect(rect, _cornerRadius))
            {
                using (SolidBrush b = new SolidBrush(_fillColor)) g.FillPath(b, path);
                Color border = Focused || DroppedDown ? _focusBorderColor : (_hover ? Ui.Blend(_borderColor, NovaTheme.TextColor, 0.25f) : _borderColor);
                using (Pen p = new Pen(border, Focused || DroppedDown ? 1.6f : 1f)) g.DrawPath(p, path);
            }

            if (DropDownStyle == ComboBoxStyle.DropDownList)
            {
                string label = SelectedIndex >= 0 && Items.Count > 0 ? GetItemText(SelectedItem) : Text;
                Rectangle textRect = new Rectangle(12, 0, Width - 12 - 30, Height);
                TextRenderer.DrawText(g, label, Font, textRect, ForeColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }

            DrawChevron(g);
        }

        private void DrawChevron(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float cx = Width - 16f;
            float cy = Height / 2f;
            using (Pen p = new Pen(_arrowColor, 1.8f))
            {
                p.StartCap = LineCap.Round;
                p.EndCap = LineCap.Round;
                g.DrawLines(p, new[]
                {
                    new PointF(cx - 4.5f, cy - 2.5f),
                    new PointF(cx, cy + 2.5f),
                    new PointF(cx + 4.5f, cy - 2.5f)
                });
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) { base.OnDrawItem(e); return; }
            Graphics g = e.Graphics;
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            using (SolidBrush b = new SolidBrush(selected ? _itemHoverColor : _fillColor))
            {
                g.FillRectangle(b, e.Bounds);
            }
            Rectangle textRect = new Rectangle(e.Bounds.X + 10, e.Bounds.Y, e.Bounds.Width - 20, e.Bounds.Height);
            TextRenderer.DrawText(g, GetItemText(Items[e.Index]), Font, textRect, ForeColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }
}
