using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using Milkshake.Generation;
using Milkshake.Media;

namespace Milkshake.Instances;

public abstract class InstanceBase
{
    protected InstanceBase(IMilkshakeService service, IGenerationService generation, IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Service = service;
        Generation = generation;

        Instance = scope.ServiceProvider.GetRequiredService<IMilkshakeInstance>();
        Media = scope.ServiceProvider.GetRequiredService<IMediaService>();

        if (Service.Options.MultipleInstances && Instance.InstanceName is "default")
        {
            var attribute = InstanceAttribute.GetValue(this);

            if(attribute is not null)
                InstanceName = attribute.Name;

            Instance.InstanceName = attribute?.Name ?? InstanceName;
        }
        else if (Instance.InstanceName != InstanceName)
        {
            throw new Exception("Instance cannot be accessed.");
        }

        Instance.Initialize();
    }

    protected InstanceBase(IMilkshakeService service, IGenerationService generation, IServiceProvider serviceProvider, string instanceName)
        : this(service, generation, serviceProvider)
    {
        InstanceName = instanceName;
    }

    internal string InstanceName { get; } = "default";

    protected readonly IMilkshakeService Service;
    protected readonly IMilkshakeInstance Instance;
    protected readonly IMediaService Media;
    protected readonly IGenerationService Generation;
}