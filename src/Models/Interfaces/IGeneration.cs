using Milkshake.Instances;

namespace Milkshake.Models.Interfaces;

public interface IGeneration
{
    public MilkshakeInstance Instance { get; init; }

    public event Action<IGeneration> Ready;

    internal void OnReady();
}