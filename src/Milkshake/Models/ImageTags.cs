using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Models
{
    [Flags]
    public enum ImageTags
    {
        Any = 0, // Generic Tag
        Person = 1,
        Symbol = 2,
        Object = 4,
        Shitpost = 8,
        Picture = 16, // Any real life photo or screenshot which doesn't fit in the former Tags.
        Post = 32, // Social media screenshots.
        Text = 64  // Billboards, signs, etc.
    }
}
