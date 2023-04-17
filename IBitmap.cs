using System.Drawing;

namespace Speedy_Bitmap
{
    public interface IBitmap : IDisposable
    {
        Bitmap map { get; }
        float Horizontalresolution => map.HorizontalResolution;
        float Verticalresolution => map.VerticalResolution;
        public Rectangle Border
        {
            get
            {
                GraphicsUnit pixel = GraphicsUnit.Pixel;
                RectangleF rect = map.GetBounds(ref pixel);
                return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
            }
        }
        Point PixelUnit
        {
            get
            {
                GraphicsUnit pixel = GraphicsUnit.Pixel;
                RectangleF rect = map.GetBounds(ref pixel);
                return new Point((int)rect.X, (int)rect.Y);
            }
        }
        int Width => map.Width;
        int Height => map.Height;
        void Save(string file, ImageFormat format);
        void Lock(); 
        void Unlock();  
    }
}