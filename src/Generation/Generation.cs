using Milkshake.Instances;

namespace Milkshake.Generation;

internal class Generation : IGeneration
{
    public MilkshakeInstance Instance { get; init; }
    public event Action<IGeneration>? Ready;
    public void OnReady() => Ready?.Invoke(this);

    internal Generation(MilkshakeInstance instance)
    {
        Instance = instance;
    }
}