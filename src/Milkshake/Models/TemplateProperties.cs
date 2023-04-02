using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;

namespace Milkshake.Models
{
    internal class TemplateProperties
    {
        public string PropertyName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Layer Layer { get; set; }
        public bool? IsText { get; set; }
        public string? Color { get; set; }
        public Gravity? Orientation { get; set; }
        public string? Font { get; set; }
        public string? StrokeColor { get; set; }
        public int? StrokeWidth { get; set; }

    }

    internal enum Layer
    {
        Background = -1,
        Base = 0,
        Foreground = 1
    }
}
