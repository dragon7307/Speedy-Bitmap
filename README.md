# Speedy-Bitmap
The default c# Bitmap class is slow!
This package helps you gain a 100x performance boost with your image editing by leveraging the power of carefully tested unsafe code.

### Structure 
-----

- The library exposes a public interface IBitmap which allows to save bitmaps.
- Derived from this interface is an abstract class classed BitmapBase. Most methods will accept one of this as it provides all the required functionality. 
- A Bitmap can be created or loaded though by Calling the static Load method on either the BitmapRGB or the BitmapARGB class,
depending on wether you want to support transpareny (ARGB) or not.<br>
Keep in mind though that adding transparency support comes with a natural overhead and thus BitmapRGB should be preffered if possible. 

### Usage 
-----

##### Individual pixels:
under the hood, the bitmaps work by utilising a method called locking, therefore it is required to call bitmap.Lock() before using the bitmap object.
After locking, it is possible to set or get pixels by calling bitmap.GetPixel(x, y) or bitmap.SetPixel(x, y, Pixel) responsively.
The type that is returned by GetPixel and passed to SetPixel depends on the Type used. The BitmapRGB accepts the type PixelRGB

##### Transform all pixels:
