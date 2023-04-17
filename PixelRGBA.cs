using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace Speedy_Bitmap;

public readonly struct PixelRGBA
{
    [SetsRequiredMembers]
    public PixelRGBA(byte r, byte g, byte b, byte a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public required byte R { get; init; }
    public required byte G { get; init; }
    public required byte B { get; init; }
    public required byte A { get; init; }

    public static explicit operator Color(PixelRGBA pixel) => Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);
    public static implicit operator (byte R, byte G, byte B, byte A)(PixelRGBA pixel) => (pixel.R, pixel.G, pixel.B, pixel.A);  
    public static implicit operator PixelRGBA(Color color) => new PixelRGBA(color.R, color.G, color.B, color.A);
    public static implicit operator PixelRGBA((byte R, byte G, byte B, byte A) tuple) => new PixelRGBA(tuple.R, tuple.G, tuple.B, tuple.A);   
}
