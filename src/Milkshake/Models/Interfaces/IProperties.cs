using ImageMagick;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Models.Interfaces
{
    public interface IProperties : IMilkshake
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Layer Layer { get; set; }
        public bool IsText { get; set; }
        public string Color { get; set; }
        public Gravity Orientation { get; set; }
        public string Font { get; set; }
        public string StrokeColor { get; set; }
        public int StrokeWidth { get; set; }

        public Guid TemplateId { get; set; }
        public int Index { get; set; }
    }
}
