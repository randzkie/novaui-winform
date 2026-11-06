using System;
using System.Drawing;

namespace NovaUI.WinForms
{
    /// <summary>
    /// Central NovaUI palette. Every control reads its defaults from here, so
    /// changing NovaTheme.Accent recolors the whole app at once. Individual
    /// controls can still override any color through their own properties.
    /// </summary>
    public static class NovaTheme
    {
        /// <summary>Primary accent (blurple).</summary>
        public static Color Accent { get; set; } = Color.FromArgb(88, 101, 242);

        /// <summary>Accent used on hover states.</summary>
        public static Color AccentHover { get; set; } = Color.FromArgb(71, 82, 196);

        /// <summary>Standard window background.</summary>
        public static Color BackColor { get; set; } = Color.White;

        /// <summary>Card / panel surface.</summary>
        public static Color SurfaceColor { get; set; } = Color.FromArgb(246, 247, 249);

        /// <summary>Primary text.</summary>
        public static Color TextColor { get; set; } = Color.FromArgb(23, 26, 33);

        /// <summary>Secondary / placeholder text.</summary>
        public static Color MutedColor { get; set; } = Color.FromArgb(122, 128, 140);

        /// <summary>Default 1px border.</summary>
        public static Color BorderColor { get; set; } = Color.FromArgb(214, 219, 228);

        /// <summary>Success / positive states.</summary>
        public static Color SuccessColor { get; set; } = Color.FromArgb(53, 178, 115);

        /// <summary>Danger / destructive states.</summary>
        public static Color DangerColor { get; set; } = Color.FromArgb(235, 69, 94);

        /// <summary>Warning states.</summary>
        public static Color WarningColor { get; set; } = Color.FromArgb(240, 166, 44);

        /// <summary>Switch the whole palette to a dark scheme.</summary>
        public static void ApplyDark()
        {
            BackColor = Color.FromArgb(32, 34, 39);
            SurfaceColor = Color.FromArgb(44, 47, 54);
            TextColor = Color.FromArgb(235, 237, 242);
            MutedColor = Color.FromArgb(130, 136, 148);
            BorderColor = Color.FromArgb(62, 66, 76);
        }

        /// <summary>Switch the whole palette back to the light scheme.</summary>
        public static void ApplyLight()
        {
            BackColor = Color.White;
            SurfaceColor = Color.FromArgb(246, 247, 249);
            TextColor = Color.FromArgb(23, 26, 33);
            MutedColor = Color.FromArgb(122, 128, 140);
            BorderColor = Color.FromArgb(214, 219, 228);
        }
    }
}
