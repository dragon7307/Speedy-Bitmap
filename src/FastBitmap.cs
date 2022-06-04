using System.Drawing;
using System.Drawing.Imaging;




#pragma warning disable CA1416  
namespace Speedy_Bitmap; 

 
    public unsafe class FastBitmap 
    {

    

        int _width;

        int _height;


        public Bitmap bmp { get; internal set; }   


     
     
        byte* cur;
        BitmapData?  dat;

        public int   Width 
        {

            get
            {
                return _width;
            }
        }




        public Point  PixelUnit 
        {

            get
            {

                GraphicsUnit  pixel  = GraphicsUnit.Pixel;
                RectangleF rect = bmp.GetBounds(ref  pixel  );
                return new Point((int)rect.X, (int)rect.Y);
            }
        }



        public  Rectangle   Border
        {
            get
            {
                GraphicsUnit pixel  = GraphicsUnit.Pixel;
                RectangleF rect = bmp.GetBounds(ref pixel);
                return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
            }
        }
        public int   Height 
        {

            get
            {
                return _height;
            }
        }








    public void MakeTransparent() =>  bmp.MakeTransparent();

    public void MakeTransparent(   Color    col  ) => bmp.MakeTransparent(   col   );

    public   float  Horizontalresolution => bmp.HorizontalResolution;   
     
    public   float     Verticalresolution => bmp. VerticalResolution ;

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
        bmp.UnlockBits(dat !  );
        dat = null; 
        cur = null;
    }


    public void Dispose()
    {
        if (dat != null) Unlock(); 
        bmp.Dispose(); 
        bmp = null! ;  
    }
        public pixel* At(int x, int y) => (pixel*)(cur + y *  _width + x * sizeof(pixel));
        public FastBitmap(Bitmap map) => bmp = new Bitmap  (map);
        public pixel Getpixel(int x, int y)
        {

            return *At(x, y);

        }


        public void SetPixel(int x, int y, pixel c)
        {


            pixel* p = At(x, y);
            *p = c;

        }

    
    }


public readonly struct pixel
{



    public readonly byte r;


    public readonly byte g;

    public readonly byte b;

    public pixel(byte r, byte g, byte b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }

    public static explicit operator Color(pixel p) => Color.FromArgb(p.r, p.g, p.b);



    public static implicit operator pixel(Color p) => new pixel(p.R, p.G, p.B);
}



#pragma warning  restore  CA1416  