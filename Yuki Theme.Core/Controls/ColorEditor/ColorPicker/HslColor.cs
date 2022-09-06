using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MechanikaDesign.WinForms.UI.ColorPicker
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HslColor
    {
        public static readonly HslColor Empty;
        private double hue;
        private double saturation;
        private double luminance;
        private int alpha;

        public HslColor(int a, double h, double s, double l)
        {
            alpha = a;
            hue = h;
            saturation = s;
            luminance = l;
            A = a;
            H = hue;
            S = saturation;
            L = luminance;
        }

        public HslColor(double h, double s, double l)
        {
            alpha = 0xff;
            hue = h;
            saturation = s;
            luminance = l;
        }

        public HslColor(Color color)
        {
            alpha = color.A;
            hue = 0.0;
            saturation = 0.0;
            luminance = 0.0;
            RGBtoHSL(color);
        }

        public static HslColor FromArgb(int a, int r, int g, int b)
        {
            return new HslColor(Color.FromArgb(a, r, g, b));
        }

        public static HslColor FromColor(Color color)
        {
            return new HslColor(color);
        }

        public static HslColor FromAhsl(int a)
        {
            return new HslColor(a, 0.0, 0.0, 0.0);
        }

        public static HslColor FromAhsl(int a, HslColor hsl)
        {
            return new HslColor(a, hsl.hue, hsl.saturation, hsl.luminance);
        }

        public static HslColor FromAhsl(double h, double s, double l)
        {
            return new HslColor(0xff, h, s, l);
        }

        public static HslColor FromAhsl(int a, double h, double s, double l)
        {
            return new HslColor(a, h, s, l);
        }

        public static bool operator ==(HslColor left, HslColor right)
        {
            return (((left.A == right.A) && (left.H == right.H)) && ((left.S == right.S) && (left.L == right.L)));
        }

        public static bool operator !=(HslColor left, HslColor right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is HslColor)
            {
                HslColor color = (HslColor)obj;
                if (((A == color.A) && (H == color.H)) && ((S == color.S) && (L == color.L)))
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (((alpha.GetHashCode() ^ hue.GetHashCode()) ^ saturation.GetHashCode()) ^ luminance.GetHashCode());
        }

        [DefaultValue((double)0.0), Category("Appearance"), Description("H Channel value")]
        public double H
        {
            get
            {
                return hue;
            }
            set
            {
                hue = value;
                hue = (hue > 1.0) ? 1.0 : ((hue < 0.0) ? 0.0 : hue);
            }
        }
        [Category("Appearance"), Description("S Channel value"), DefaultValue((double)0.0)]
        public double S
        {
            get
            {
                return saturation;
            }
            set
            {
                saturation = value;
                saturation = (saturation > 1.0) ? 1.0 : ((saturation < 0.0) ? 0.0 : saturation);
            }
        }
        [Category("Appearance"), Description("L Channel value"), DefaultValue((double)0.0)]
        public double L
        {
            get
            {
                return luminance;
            }
            set
            {
                luminance = value;
                luminance = (luminance > 1.0) ? 1.0 : ((luminance < 0.0) ? 0.0 : luminance);
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color RgbValue
        {
            get
            {
                return HSLtoRGB();
            }
            set
            {
                RGBtoHSL(value);
            }
        }
        public int A
        {
            get
            {
                return alpha;
            }
            set
            {
                alpha = (value > 0xff) ? 0xff : ((value < 0) ? 0 : value);
            }
        }
        public bool IsEmpty
        {
            get
            {
                return ((((alpha == 0) && (H == 0.0)) && (S == 0.0)) && (L == 0.0));
            }
        }

        public Color ToRgbColor()
        {
            return ToRgbColor(A);
        }

        public Color ToRgbColor(int alpha)
        {
            double q;
            if (L < 0.5)
            {
                q = L * (1 + S);
            }
            else
            {
                q = L + S - (L * S);
            }
            double p = 2 * L - q;
            double hk = H / 360;

            // r,g,b colors
            double[] tc = new[]
                    {
                      hk + (1d / 3d), hk, hk - (1d / 3d)
                    };
            double[] colors = new[]
                        {
                          0.0, 0.0, 0.0
                        };

            for (int color = 0; color < colors.Length; color++)
            {
                if (tc[color] < 0)
                {
                    tc[color] += 1;
                }
                if (tc[color] > 1)
                {
                    tc[color] -= 1;
                }

                if (tc[color] < (1d / 6d))
                {
                    colors[color] = p + ((q - p) * 6 * tc[color]);
                }
                else if (tc[color] >= (1d / 6d) && tc[color] < (1d / 2d))
                {
                    colors[color] = q;
                }
                else if (tc[color] >= (1d / 2d) && tc[color] < (2d / 3d))
                {
                    colors[color] = p + ((q - p) * 6 * (2d / 3d - tc[color]));
                }
                else
                {
                    colors[color] = p;
                }

                colors[color] *= 255;
            }
	//		try {
				int r=(int)colors[0];
				if(r>254)
				{
					r=254;
				}
			if(r<0)
			{
				r=0;
			}
				int g=(int)colors[1];
				if(g>254)
				{
					g=254;
				}

			if(g<0)
			{
				g=0;
			}
				int b=(int)colors[2];
				if(b>254)
				{
					b=254;
			}
			if(b<0)
			{
				b=0;
			}
			Color res=Color.FromArgb(alpha, r, g, b);
			return res;
	//		}
	//		catch (Exception ex) {
//				Console.WriteLine (ex.Message);
	//			return Color.FromArgb (0, 0, 0);
	//		}
        }

        private Color HSLtoRGB()
        {
            int num2;
            int red = Round(luminance * 255.0);
            int blue = Round(((1.0 - saturation) * (luminance / 1.0)) * 255.0);
            double num4 = ((double)(red - blue)) / 255.0;
            if ((hue >= 0.0) && (hue <= 0.16666666666666666))
            {
                num2 = Round((((hue - 0.0) * num4) * 1530.0) + blue);
                return Color.FromArgb(alpha, red, num2, blue);
            }
            if (hue <= 0.33333333333333331)
            {
                num2 = Round((-((hue - 0.16666666666666666) * num4) * 1530.0) + red);
                return Color.FromArgb(alpha, num2, red, blue);
            }
            if (hue <= 0.5)
            {
                num2 = Round((((hue - 0.33333333333333331) * num4) * 1530.0) + blue);
                return Color.FromArgb(alpha, blue, red, num2);
            }
            if (hue <= 0.66666666666666663)
            {
                num2 = Round((-((hue - 0.5) * num4) * 1530.0) + red);
                return Color.FromArgb(alpha, blue, num2, red);
            }
            if (hue <= 0.83333333333333337)
            {
                num2 = Round((((hue - 0.66666666666666663) * num4) * 1530.0) + blue);
                return Color.FromArgb(alpha, num2, blue, red);
            }
            if (hue <= 1.0)
            {
                num2 = Round((-((hue - 0.83333333333333337) * num4) * 1530.0) + red);
                return Color.FromArgb(alpha, red, blue, num2);
            }
            return Color.FromArgb(alpha, 0, 0, 0);
        }

        private void RGBtoHSL(Color color)
        {
            int r;
            int g;
            double num4;
            alpha = color.A;
            if (color.R > color.G)
            {
                r = color.R;
                g = color.G;
            }
            else
            {
                r = color.G;
                g = color.R;
            }
            if (color.B > r)
            {
                r = color.B;
            }
            else if (color.B < g)
            {
                g = color.B;
            }
            int num3 = r - g;
            luminance = ((double)r) / 255.0;
            if (r == 0)
            {
                saturation = 0.0;
            }
            else
            {
                saturation = ((double)num3) / ((double)r);
            }
            if (num3 == 0)
            {
                num4 = 0.0;
            }
            else
            {
                num4 = 60.0 / ((double)num3);
            }
            if (r == color.R)
            {
                if (color.G < color.B)
                {
                    hue = (360.0 + (num4 * (color.G - color.B))) / 360.0;
                }
                else
                {
                    hue = (num4 * (color.G - color.B)) / 360.0;
                }
            }
            else if (r == color.G)
            {
                hue = (120.0 + (num4 * (color.B - color.R))) / 360.0;
            }
            else if (r == color.B)
            {
                hue = (240.0 + (num4 * (color.R - color.G))) / 360.0;
            }
            else
            {
                hue = 0.0;
            }
        }

        private int Round(double val)
        {
            return (int)(val + 0.5);
        }

        static HslColor()
        {
            Empty = new HslColor();
        }
    }
}
