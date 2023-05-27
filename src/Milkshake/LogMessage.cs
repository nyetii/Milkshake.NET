using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake
{
    public class LogMessage
    {
        public string Message { get; }
        public Exception? Exception { get; }
        public Severity Severity { get; }
        public string? Source { get; } = Assembly.GetExecutingAssembly().GetName().Name;

        public LogMessage(string message, Severity severity, Exception? exception = null)
        {
            Message = message;
            Severity = severity;
            Exception = exception;
        }
    }

    public enum Severity
    {
        Verbose,
        Debug,
        Information,
        Warning,
        Error,
        Fatal
    }
}
