using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace Speedy_Bitmap;

public readonly struct PixelRGB
{
    [SetsRequiredMembers]   
    public PixelRGB(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }

    public required byte R { get; init; }
    public required byte G { get; init; }
    public required byte B { get; init; }

    public static explicit operator Color(PixelRGB pixel) => Color.FromArgb(pixel.R, pixel.G, pixel.B);
    public static implicit operator (byte R, byte G, byte B)(PixelRGB pixel) => (pixel.R, pixel.G, pixel.B);
    public static implicit operator PixelRGB(Color color) => new PixelRGB(color.R, color.G, color.B);
    public static implicit operator PixelRGB((byte R, byte G, byte B) tuple) => new PixelRGB(tuple.R, tuple.G, tuple.B);
}