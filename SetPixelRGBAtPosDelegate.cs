using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speedy_Bitmap;

public delegate T SetPixelAtPosDelegate<T>(int x, int y, T pixel)  where T : struct;