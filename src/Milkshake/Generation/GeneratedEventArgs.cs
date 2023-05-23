using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Generation
{
    public class GeneratedEventArgs : EventArgs
    {
        public Guid Id { get; set; }
        public string FilePath { get; set; }
    }
}
