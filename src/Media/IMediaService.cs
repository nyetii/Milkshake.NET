namespace Milkshake.Media;

public interface IMediaService
{
    public T Find<T>(Guid guid) where T : class, IMilkshake, IMedia, new();
    public Task<T> LoadAsync<T>() where T : class, IMilkshake, IMedia, new();
    public Task<T> LoadAsync<T>(T milkshake) where T : class, IMilkshake, IMedia;
}