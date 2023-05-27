using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Configuration
{
    public class MilkshakeOptions
    {
        public string BasePath { get; set; } = $"{Environment.CurrentDirectory}/Milkshake";
        public bool MultipleInstances { get; set; } = true;
        public (int width, int height) MaxDimensions { get; set; } = (1024, 1024);
    }
}
