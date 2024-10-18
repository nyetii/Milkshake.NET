namespace Milkshake;

public interface IGeneration
{
    public MilkshakeInstance Instance { get; init; }

    public event Action<IGeneration> Ready;

    internal void OnReady();

    // TODO: Add a IMedia property here, probably with a IMedia<Milkshake> signature where Milkshake is a concrete type.
}