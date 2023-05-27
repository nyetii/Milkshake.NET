using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;
using Milkshake.Configuration;

namespace Milkshake
{
    public class MilkshakeService
    {
        public Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version!;

        public event Logging? Logger;
        public delegate Task Logging(LogMessage log);
        internal async Task LogAsync(string message, Severity severity = Severity.Information, Exception? exception = null)
        {
            if(Logger != null)
                await Logger.Invoke(new LogMessage(message, severity, exception));
        }

        internal void Log(string message, Severity severity = Severity.Information, Exception? exception = null)
        {
            Logger?.Invoke(new LogMessage(message, severity, exception));
        }

        public readonly MilkshakeOptions Options;
        
        public MilkshakeService(IOptions<MilkshakeOptions> options)
        {
            Options = options.Value;
        }
    }
}