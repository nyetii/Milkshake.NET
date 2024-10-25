namespace Milkshake.Media;

public interface IMediaService
{
    public IMilkshake Create<T>() where T : IMilkshake, IMedia, new();
    public T Find<T>(Guid guid) where T : class, IMilkshake, IMedia, new();
    public Task<T> LoadAsync<T>() where T : class, IMilkshake, IMedia, new();
    public Task<T> LoadAsync<T>(T milkshake) where T : class, IMilkshake, IMedia;

    public Task<bool> SaveAsync<T>(IMilkshake milkshake, CancellationToken cancellationToken = default)
        where T : Media, IMilkshake, new();
}