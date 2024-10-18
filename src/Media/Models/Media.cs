namespace Milkshake.Media.Models;

public abstract class Media : IMedia, IDisposable, IAsyncDisposable
{
    public string FileName { get; set; } = null!;
    public MemoryStream Stream { get; set; } = null!;

    public long Size => Stream.Length;

    public void Dispose()
    {
        Stream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await Stream.DisposeAsync();
    }
}