using System.Drawing;
using System.Drawing.Imaging;
using static Speedy_Bitmap.ImageFormat;

namespace Speedy_Bitmap
{
    public abstract unsafe class BitmapBase<T> : IBitmap where T : struct
    {
        Bitmap bmp;
        byte* cur;
        BitmapData? dat;
        readonly IBitmap ithis;
        int width;
        protected abstract PixelFormat format { get; }   
        public Bitmap map
        {
            get
            {
                return bmp;
            }
            internal set
            {
                bmp = value;
            }
        }
        public BitmapBase(Bitmap bmp)  
        {
            this.bmp = bmp;
            ithis = this;
        }
        public BitmapBase(int width, int height) : this(new Bitmap(width, height)) { }
        static System.Drawing.Imaging.ImageFormat ConvertFormat(ImageFormat t) => t switch
        {
            Png => System.Drawing.Imaging.ImageFormat.Png,
            Bmp => System.Drawing.Imaging.ImageFormat.Bmp,
            Jpeg => System.Drawing.Imaging.ImageFormat.Jpeg,
            Jpg => System.Drawing.Imaging.ImageFormat.Jpeg,
            Gif => System.Drawing.Imaging.ImageFormat.Gif,
            _ => System.Drawing.Imaging.ImageFormat.Png,
        };
        public T* At(int x, int y) => (T*)(cur + y * width + x * sizeof(T));
        public T GetPixel(int x, int y)
        {
            return *At(x, y);
        }
        public void Lock()
        {
            width = ithis.Border.Width * sizeof(T);
            if (width % 4 != 0)
            {
                width = 4 * (width / 4 + 1);
            }
            dat = map.LockBits(ithis.Border, ImageLockMode.ReadWrite, format);
            cur = (byte*)dat.Scan0.ToPointer();
        }
        public void Save(string file, ImageFormat format)
        {
            map.Save(file, ConvertFormat(format));
        }
        public void SetPixel(int x, int y, T pixel)
        {
            *At(x, y) = pixel;
        }
        /// <summary>
        /// Sets all pixels to a specific color (PixelRGB)   
        /// </summary>
        /// <param name="pixel">The RGBColor to which all pixels are to be set </param>  
        public abstract void SetPixels(T pixel);
        /// <summary>
        /// Sets all pixels to a the return value of a specific function (SetPixelRGBAtPosDelegate)   
        /// </summary>
        /// <param name="eval">The Delegate determining the color of a pixel at a given coordinate!</param>
        public abstract void SetPixels(SetPixelAtPosDelegate<T> eval); 

        public void Unlock()
        {
            if (dat is null)
            {
                return;
            }
            map.UnlockBits(dat);
            dat = null;
            cur = null;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this); 
            if (dat is null)  
            {
                Unlock();
            }
            map.Dispose();
        }
    }
}