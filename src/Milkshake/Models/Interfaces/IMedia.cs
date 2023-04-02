using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Models.Interfaces
{
    public interface IMedia
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
