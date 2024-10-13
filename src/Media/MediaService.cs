using Milkshake.Media.Models;

namespace Milkshake.Media;

// TODO: Remove the Load, Save, Delete, etc. methods from the IMedia interface and implement them here instead
public class MediaService : IMediaService
{
    private readonly MilkshakeService _service;

    public MediaService(MilkshakeService service)
    {
        _service = service;
    }

    public virtual T LoadAsync<T>() where T : class, IMilkshake, IMedia<T>
    {
        if (_service.Options.SerializeMilkshakes)
        {

        }

        throw new NotImplementedException();
    }

    public virtual T LoadAsync<T>(Guid guid) where T : class, IMilkshake, IMedia<T>
    {
        throw new NotImplementedException();
    }
}