using Milkshake.Models.Interfaces;

namespace Milkshake;

public class MediaService
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