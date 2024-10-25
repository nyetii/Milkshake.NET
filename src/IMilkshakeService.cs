using System.Text.Json;
using Milkshake.Configuration;

namespace Milkshake;

public interface IMilkshakeService
{
    public MilkshakeOptions Options { get; init; }
    internal JsonSerializerOptions SerializerOptions { get; }

    internal string GetDirectory(string instanceName);
    internal string GetDirectory<T>();
    internal string GetDirectory<T>(string instanceName, string fileName = "");
}