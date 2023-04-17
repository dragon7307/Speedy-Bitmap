using System.Drawing;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using static Speedy_Bitmap.ImageFormat;
  
namespace Speedy_Bitmap;

public sealed unsafe class BitmapRGB : BitmapBase<PixelRGB>
{
    public BitmapRGB(Bitmap bmp) : base(bmp)
    {
    }

    protected override PixelFormat format => PixelFormat.Format24bppRgb;

    public static BitmapRGB Load(string file)
    {
        if (!File.Exists(file))
        {
            throw new FileNotFoundException("Couldn't load the bitmap", file);
        }
        Bitmap bmp = new Bitmap(file);
        return new BitmapRGB(bmp);
    }

    public override void SetPixels(PixelRGB pixel)
    {
        Bitmap b = new Bitmap(map);
        BitmapData bData = b.LockBits(new Rectangle(0, 0, map.Width, map.Height), ImageLockMode.ReadWrite, b.PixelFormat);
        byte bitsPerPixel = 32;
        byte* scan0 = (byte*)bData.Scan0.ToPointer();

        for (int x = 0; x < bData.Height; ++x)
        {
            for (int y = 0; y < bData.Width; ++y)
            {
                byte* data = scan0 + x * bData.Stride + y * bitsPerPixel / 8;

                data[0] = pixel.B;
                data[1] = pixel.G;
                data[2] = pixel.R;
            }
        }

        b.UnlockBits(bData);
        map = new Bitmap(b);
    }
    public override void SetPixels(SetPixelAtPosDelegate<PixelRGB> eval)
    {
        Bitmap b = new Bitmap(map);
        BitmapData bData = b.LockBits(new Rectangle(0, 0, map.Width, map.Height), ImageLockMode.ReadWrite, b.PixelFormat);
        byte bitsPerPixel = 32;
        byte* scan0 = (byte*)bData.Scan0.ToPointer();

        for (int x = 0; x < bData.Height; ++x)
        {
            for (int y = 0; y < bData.Width; ++y)
            {
                byte* data = scan0 + x * bData.Stride + y * bitsPerPixel / 8;
                PixelRGB current = (data[0], data[1], data[2]);
                PixelRGB result = eval(x, y, current);
                data[0] = result.B;
                data[1] = result.G;
                data[2] = result.R;
            }
        }

        b.UnlockBits(bData);
        map = new Bitmap(b);
    }
}
public sealed unsafe class BitmapRGBA : BitmapBase<PixelRGBA>
{
    public BitmapRGBA(Bitmap bmp) : base(bmp)
    {
    }

    protected override PixelFormat format => PixelFormat.Format32bppArgb;

    public static BitmapRGBA Load(string file)
    {
        if (!File.Exists(file))
        {
            throw new FileNotFoundException("Couldn't load the bitmap", file);
        }
        Bitmap bmp = new Bitmap(file);
        Bitmap temp = new Bitmap("C:/Users/tclemens/Downloads/output-onlinepngtools.png");
        BitmapRGBA bitmap = new BitmapRGBA(temp);
        bitmap.SetPixels((x, y, c) => bmp.GetPixel(x, y) );
        //PropertyItem item = temp.GetPropertyItem(2) ?? throw new Exception();
        //  bmp.SetPropertyItem(item);
        return bitmap;
    }

    public override void SetPixels(PixelRGBA pixel)
    {
        Bitmap b = new Bitmap(map);
        BitmapData bData = b.LockBits(new Rectangle(0, 0, map.Width, map.Height), ImageLockMode.ReadWrite, b.PixelFormat);
        byte bitsPerPixel = 32;
        byte* scan0 = (byte*)bData.Scan0.ToPointer();

        for (int x = 0; x < bData.Height; ++x)
        {
            for (int y = 0; y < bData.Width; ++y)
            {
                byte* data = scan0 + x * bData.Stride + y * bitsPerPixel / 8;

                data[0] = pixel.B;
                data[1] = pixel.G;
                data[2] = pixel.R;
            }
        }

        b.UnlockBits(bData);
        map = new Bitmap(b);
    }
    public override void SetPixels(SetPixelAtPosDelegate<PixelRGBA> eval)
    {
        Bitmap b = new Bitmap(map);
        BitmapData bData = b.LockBits(new Rectangle(0, 0, map.Width, map.Height), ImageLockMode.ReadWrite, b.PixelFormat);
        byte bitsPerPixel = 32;
        byte* scan0 = (byte*)bData.Scan0.ToPointer();

        for (int x = 0; x < bData.Height; ++x)
        {
            for (int y = 0; y < bData.Width; ++y)
            {
                byte* data = scan0 + x * bData.Stride + y * bitsPerPixel / 8;
                PixelRGBA current = (data[0], data[1], data[2], data[3]);
                PixelRGBA result = eval(x, y, current);
                data[0] = result.B;
                data[1] = result.G;
                data[2] = result.R;
            }
        }

        b.UnlockBits(bData);
        map = new Bitmap(b);
    }
}