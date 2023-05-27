using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Models
{
    /// <summary>
    /// Represents which layer of the generated image a Milkshake should be.
    /// </summary>
    public enum Layer
    {
        Background = -1,
        Base = 0,
        Foreground = 1
    }
}
