using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Milkshake.Models.Interfaces;

namespace Milkshake.Models
{
    internal class Source : IMedia
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ImageTags Tags { get; set; } = ImageTags.Any;
    }
}
