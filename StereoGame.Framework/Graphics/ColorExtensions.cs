namespace StereoGame.Framework.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class ColorExtensions
    {
        private const double tolerance = 0.000000000000001;

        public static Color ColorFromHSV(double hue, double saturation, double value, int a = 255)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(a, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(a, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(a, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(a, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(a, t, p, v);
            else
                return Color.FromArgb(a, v, p, q);
        }
        public static Color HSLToRGB(double h, double s, double l, int a = 255)
        {
            h = Math.Max(0, Math.Min(360, h));
            s = Math.Max(0, Math.Min(1, s));
            l = Math.Max(0, Math.Min(1, l));
            a = Math.Max(0, Math.Min(255, a));

            if (Math.Abs(s) < tolerance)
            {
                return Color.FromArgb(
                        a,
                        Math.Max(0, Math.Min(255, (int)(l * 255))),
                        Math.Max(0, Math.Min(255, (int)(l * 255))),
                        Math.Max(0, Math.Min(255, (int)(l * 255))));
            }
            double q = l < .5D
                                ? l * (1D + s)
                                : (l + s) - (l * s);
            double p = (2D * l) - q;

            double hk = h / 360D;
            double[] T = new double[3];
            T[0] = hk + (1D / 3D); // Tr
            T[1] = hk; // Tb
            T[2] = hk - (1D / 3D); // Tg

            for (int i = 0; i < 3; i++)
            {
                if (T[i] < 0D)
                    T[i] += 1D;
                if (T[i] > 1D)
                    T[i] -= 1D;

                if ((T[i] * 6D) < 1D)
                    T[i] = p + ((q - p) * 6D * T[i]);
                else if ((T[i] * 2D) < 1)
                    T[i] = q;
                else if ((T[i] * 3D) < 2)
                    T[i] = p + ((q - p) * ((2D / 3D) - T[i]) * 6D);
                else
                    T[i] = p;
            }

            return Color.FromArgb(
                    a,
                    Math.Max(0, Math.Min(255, (int)(T[0] * 255))),
                    Math.Max(0, Math.Min(255, (int)(T[1] * 255))),
                    Math.Max(0, Math.Min(255, (int)(T[2] * 255))));
        }
    }
}
