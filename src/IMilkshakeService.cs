using Milkshake.Configuration;

namespace Milkshake;

public interface IMilkshakeService
{
    public MilkshakeOptions Options { get; init; }
}