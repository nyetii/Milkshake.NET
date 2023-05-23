using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Exceptions
{
    public class InvalidMilkshakeException : Exception
    {
        public InvalidMilkshakeException() { }

        public InvalidMilkshakeException(string message) : base(message) { }

        public InvalidMilkshakeException(string message, Exception inner) : base(message, inner) { }
    }
}
