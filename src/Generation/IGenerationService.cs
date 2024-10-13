namespace Milkshake.Generation;

public interface IGenerationService
{
    public int QueueLength { get; }

    public void Enqueue(IGeneration prompt);
}