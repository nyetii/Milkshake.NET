using System.Text.Json.Serialization;

namespace Milkshake.Media;

public abstract class Media : IMedia, IDisposable, IAsyncDisposable
{
    public string FileName { get; set; } = null!;
    [JsonIgnore]
    public MemoryStream Stream { get; set; } = new();

    public long Length => Stream.Length;

    public void Dispose()
    {
        Stream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await Stream.DisposeAsync();
    }
}