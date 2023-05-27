using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
using Milkshake.Models;

namespace Milkshake.Builders
{
    public class TextPropertiesBuilder
    {
        public string? Color { get; set; }
        public Gravity? Orientation { get; set; }
        public string? Font { get; set; }
        public string? StrokeColor { get; set; }
        public int? StrokeWidth { get; set; }
        
    }
}
