using System.Drawing;
using System.Reflection;

namespace Milkshake.Configuration;

public class MilkshakeOptions
{
    public string BasePath { get; set; } = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)}/Milkshake";
    public bool MultipleInstances { get; set; } = false;
    public Size MaxSize { get; set; } = new (1024, 1024);
    public TimeSpan CacheLifetime { get; set; } = TimeSpan.Zero;

    public bool SerializeMilkshakes { get; set; } = true;
}