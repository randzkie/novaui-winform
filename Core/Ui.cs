using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;

namespace NovaUI.WinForms
{
    /// <summary>
    /// Shared low-level painting helpers used by every NovaUI control.
    /// Kept internal on purpose - the public surface is the controls themselves.
    /// </summary>
    internal static class Ui
    {
        /// <summary>True when the control lives on a WinForms designer surface.</summary>
        internal static bool DesignMode(Control c)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return true;
            Control walk = c;
            while (walk != null)
            {
                if (walk.Site != null && walk.Site.DesignMode) return true;
                walk = walk.Parent;
            }
            return false;
        }

        /// <summary>Builds a rounded-rectangle path from four arcs.</summary>
        internal static GraphicsPath RoundRect(RectangleF r, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            if (r.Width <= 0f || r.Height <= 0f)
            {
                path.AddRectangle(new RectangleF(r.X, r.Y, Math.Max(0.1f, r.Width), Math.Max(0.1f, r.Height)));
                return path;
            }
            float d = radius * 2f;
            if (d <= 0f) { path.AddRectangle(r); return path; }
            if (d > r.Width) d = r.Width;
            if (d > r.Height) d = r.Height;
            path.AddArc(r.X, r.Y, d, d, 180f, 90f);
            path.AddArc(r.Right - d, r.Y, d, d, 270f, 90f);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0f, 90f);
            path.AddArc(r.X, r.Bottom - d, d, d, 90f, 90f);
            path.CloseFigure();
            return path;
        }

        /// <summary>Linear blend of two colors. t = 0 returns a, t = 1 returns b.</summary>
        internal static Color Blend(Color a, Color b, float t)
        {
            if (t < 0f) t = 0f; else if (t > 1f) t = 1f;
            int r = a.R + (int)((b.R - a.R) * t);
            int g = a.G + (int)((b.G - a.G) * t);
            int bl = a.B + (int)((b.B - a.B) * t);
            return Color.FromArgb(a.A, r, g, bl);
        }

        /// <summary>Darkens a color by t (0..1).</summary>
        internal static Color Darken(Color c, float t) { return Blend(c, Color.Black, t); }

        internal static TextFormatFlags TextFlags(ContentAlignment alignment)
        {
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;
            switch (alignment)
            {
                case ContentAlignment.TopLeft: flags = TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.EndEllipsis; break;
                case ContentAlignment.MiddleLeft: flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis; break;
                case ContentAlignment.BottomLeft: flags = TextFormatFlags.Left | TextFormatFlags.Bottom | TextFormatFlags.EndEllipsis; break;
                case ContentAlignment.TopCenter: flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.Top | TextFormatFlags.EndEllipsis; break;
                case ContentAlignment.MiddleCenter: break;
                case ContentAlignment.BottomCenter: flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.Bottom | TextFormatFlags.EndEllipsis; break;
                case ContentAlignment.TopRight: flags = TextFormatFlags.Right | TextFormatFlags.Top | TextFormatFlags.EndEllipsis; break;
                case ContentAlignment.MiddleRight: flags = TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis; break;
                case ContentAlignment.BottomRight: flags = TextFormatFlags.Right | TextFormatFlags.Bottom | TextFormatFlags.EndEllipsis; break;
            }
            return flags;
        }

        /// <summary>The standard double-buffered owner-paint style set.</summary>
        internal const ControlStyles PaintStyles =
            ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw | ControlStyles.UserPaint;
    }
}
