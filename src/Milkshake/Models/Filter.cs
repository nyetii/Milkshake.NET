using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Models
{
    [Flags]
    public enum Filter
    {
        None = 0,
        Grayscale = 1,
        Vignette = 2,
        Blur = 4,
        Sepia = 8,
        Charcoal = 16,
        Emboss = 32,
        OilPaint = 64,
        Negate = 128
    }
}
