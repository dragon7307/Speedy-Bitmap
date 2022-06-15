using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static Speedy_Bitmap.ImageFormat;





#pragma warning disable CA1416
namespace Speedy_Bitmap;


public unsafe class FastBitmap : IDisposable
{



    int _width;
    public Bitmap bmp { get; internal set; }




    byte* cur;
    BitmapData? dat;

    public int Width
    {

        get
        {
            return bmp.Width;
        }
    }




    public Point PixelUnit
    {

        get
        {

            GraphicsUnit pixel = GraphicsUnit.Pixel;
            RectangleF rect = bmp.GetBounds(ref pixel);
            return new Point((int)rect.X, (int)rect.Y);
        }
    }



    public Rectangle Border
    {
        get
        {
            GraphicsUnit pixel = GraphicsUnit.Pixel;
            RectangleF rect = bmp.GetBounds(ref pixel);
            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }
    }
    public int Height
    {

        get
        {
            return bmp.Height;
        }
    }









    public void Save(string file, ImageFormat format = ImageFormat.Jpeg)
    {
        using (FileStream t = new(file, FileMode.Create))
            bmp.Save(t, formattoimg(format));
    }



    public static FastBitmap Load(string p)
    {
        return new FastBitmap(new Bitmap(p));
    }
    public FastBitmap(int width, int height) : this(new Bitmap(width, height)) { }
    public FastBitmap(int width) : this(width, width) { }
    static System.Drawing.Imaging.ImageFormat formattoimg(ImageFormat t) => t switch
    {
        Png => System.Drawing.Imaging.ImageFormat.Png,
        Bmp => System.Drawing.Imaging.ImageFormat.Bmp,
        Jpeg => System.Drawing.Imaging.ImageFormat.Jpeg,
        Jpg => System.Drawing.Imaging.ImageFormat.Jpeg,
        Gif => System.Drawing.Imaging.ImageFormat.Gif,
        _ => System.Drawing.Imaging.ImageFormat.Png,
    };
    public void MakeTransparent() => bmp.MakeTransparent();

    public void MakeTransparent(Color col) => bmp.MakeTransparent(col);

    public float Horizontalresolution => bmp.HorizontalResolution;

    public float Verticalresolution => bmp.VerticalResolution;
    public void Lock()
    {
        _width = Border.Width * sizeof(pixel);
        if (_width % 4 != 0)
        {
            _width = 4 * (_width / 4 + 1);
        }
        dat = bmp.LockBits(Border, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
        cur = (byte*)dat.Scan0.ToPointer();
    }


    public void Unlock()
    {
        bmp.UnlockBits(dat!);
        dat = null;
        cur = null;
    }


    public void Dispose()
    {
        if (dat != null) Unlock();
        bmp.Dispose();
        bmp = null!;
    }
    public pixel* At(int x, int y) => (pixel*)(cur + y * _width + x * sizeof(pixel));
    public FastBitmap(Bitmap map) { bmp = new Bitmap(map); }
    public pixel Getpixel(int x, int y)
    {

        return *At(x, y);

    }


    public void SetPixel(int x, int y, pixel c)
    {


        pixel* p = At(x, y);
        *p = c;

    }




    public unsafe void SetPixels(pixel p)
    {
        Bitmap b = new Bitmap(bmp);

        BitmapData bData = b.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, b.PixelFormat);

        byte bitsPerPixel = 32;

        byte* scan0 = (byte*)bData.Scan0.ToPointer();

        for (int i = 0; i < bData.Height; ++i)
        {
            for (int j = 0; j < bData.Width; ++j)
            {
                byte* data = scan0 + i * bData.Stride + j * bitsPerPixel / 8;



                data[0] = p.r;

                data[1] = p.g;

                data[2] = p.b;
                //   byte[]  brt    =  new  byte[] {    0,0,255, 255 };



            }
        }




        b.UnlockBits(bData);
        bmp = new Bitmap(b);
    }



    public unsafe void SetPixels(Func<int, int, pixel, pixel> rett)
    {
        Bitmap b = new Bitmap(bmp);

        BitmapData bData = b.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, b.PixelFormat);

        byte bitsPerPixel = 32;

        byte* scan0 = (byte*)bData.Scan0.ToPointer();

        for (int i = 0; i < bData.Height; ++i)
        {
            for (int j = 0; j < bData.Width; ++j)
            {
                byte* data = scan0 + i * bData.Stride + j * bitsPerPixel / 8;






                var val = rett(i, j, new pixel(data[0], data[1], data[2]));
                (data[0], data[1], data[2]) = (val.r, val.g, val.b);
                //   byte[]  brt    =  new  byte[] {    0,0,255, 255 };



            }
        }




        b.UnlockBits(bData);
        bmp = new Bitmap(b);
    }
}


public readonly struct pixel
{



    public readonly byte r;


    public readonly byte g;

    public readonly byte b;

    public pixel(byte r, byte g, byte b)
    {
        this.b = r;
        this.g = g;
        this.r = b;
    }

    public static explicit operator Color(pixel p) => Color.FromArgb(p.r, p.g, p.b);



    public static implicit operator pixel(Color p) => new pixel(p.R, p.G, p.B);
}





public enum ImageFormat
{

    Png,
    Jpg,
    Jpeg,
    Gif,
    Bmp,
    img
}
#pragma warning restore CA1416