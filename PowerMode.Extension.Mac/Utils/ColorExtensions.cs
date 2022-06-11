﻿namespace PowerMode.Extension.Mac.Utils
{
    using System;
    using System.Drawing;
    using Color2 = System.Windows.Media.Color;

    public static class ColorExtensions
    {
        public static Color SpinColor(this Color color, int hueAngle)
        {
            double hue;
            double saturation;
            double value;
            color.ColorToHSV(out hue, out saturation, out value);

            var newHue = (hue + hueAngle) % 360;

            return ColorFromHSV(newHue, saturation, value);
        }


        public static void ColorToHSV(this Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(v, t, p);
            else if (hi == 1)
                return Color.FromArgb(q, v, p);
            else if (hi == 2)
                return Color.FromArgb(p, v, t);
            else if (hi == 3)
                return Color.FromArgb(p, q, v);
            else if (hi == 4)
                return Color.FromArgb(t, p, v);
            else
                return Color.FromArgb(v, p, q);
        }

        public static Color2 ToMediaColor(this Color color)
        {
            return Color2.FromRgb(color.R, color.G, color.B);
        }

        public static Color ToDrawingColor(this Color2 color)
        {
            return Color.FromArgb(color.R, color.G, color.B);
        }

        public static String ToHexString(this Color color)
        {
            return $"#{color.R.ToString("X2")}{color.G.ToString("X2")}{color.B.ToString("X2")}";
        }

        public static String ToRGBString(this Color color)
        {
            return string.Join(", ", color.R.ToString());
        }
    }
}
