using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milkshake.Models;
using Milkshake.Models.Interfaces;

namespace Milkshake.Generation
{
    public class Generation
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Caller { get; set; } = string.Empty;

        public Template? Template { get; set; }

        public List<Source> Sources { get; set; } = new();
        public List<(string Name, Source Source)> Properties { get; set; } = new();
    }
}
