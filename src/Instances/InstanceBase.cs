using Microsoft.Extensions.DependencyInjection;
using Milkshake.Media;

namespace Milkshake;

public abstract class InstanceBase
{
    protected InstanceBase(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Service = scope.ServiceProvider.GetRequiredService<IMilkshakeService>();
        Generation = scope.ServiceProvider.GetRequiredService<IGenerationService>();

        Instance = scope.ServiceProvider.GetRequiredService<IMilkshakeInstance>();
        Media = scope.ServiceProvider.GetRequiredService<IMediaService>();

        if (Service.Options.MultipleInstances && Instance.Name is "default")
        {
            var attribute = InstanceAttribute.GetValue(this);

            if(attribute is not null)
                InstanceName = attribute.Name;

            Instance.Name = attribute?.Name ?? InstanceName;
        }
        else if (Instance.Name != InstanceName)
        {
            throw new Exception("Instance cannot be accessed.");
        }

        Instance.Initialize();
    }

    protected InstanceBase(IServiceProvider serviceProvider, string instanceName)
        : this(serviceProvider)
    {
        InstanceName = instanceName;
    }

    internal string InstanceName { get; } = "default";

    protected readonly IMilkshakeService Service;
    protected readonly IMilkshakeInstance Instance;
    protected readonly IMediaService Media;
    protected readonly IGenerationService Generation;
}