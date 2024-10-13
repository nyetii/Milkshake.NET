using System.Drawing;

namespace Milkshake.Configuration;

public class MilkshakeOptions
{
    public string BasePath { get; set; } = $"{Environment.CurrentDirectory}/Milkshake";
    public bool MultipleInstances { get; set; } = true;
    public Size MaxSize { get; set; } = new (1024, 1024);
    public TimeSpan CacheLifetime { get; set; } = TimeSpan.Zero;

    public bool SerializeMilkshakes { get; set; } = true;
}