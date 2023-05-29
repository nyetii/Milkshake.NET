using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Models.Interfaces
{
    public interface IMilkshake : IMilkshakeBase
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        ImageTags Tags { get; set; }
    }
}
