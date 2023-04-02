using System.Security.Cryptography.X509Certificates;

namespace Milkshake
{
    public class MilkshakeService
    {
        public event Logging? Log;
        public delegate Task Logging(LogMessage log);
        internal async Task LogAsync(string message, Severity severity, Exception? exception = null)
        {
            if(Log != null)
                await Log.Invoke(new LogMessage(message, severity, exception));
        }
    }
}