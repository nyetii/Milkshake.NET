using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Exceptions
{
    internal class PermissionDeniedException : Exception
    {
        //public PermissionDeniedException() { }

        public PermissionDeniedException(string message = "The caller id is not included in the permission list") : base(message) { }

        public PermissionDeniedException(string message, Exception inner) : base(message, inner) { }
    }
}
