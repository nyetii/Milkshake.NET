using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Managers
{
    public class ContextData
    {
        public Guid ContextId { get; init; }
        public string Vips { get; set; } = string.Empty;
    }
}
