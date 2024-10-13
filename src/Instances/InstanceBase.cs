using Microsoft.Extensions.DependencyInjection;

namespace Milkshake.Instances;

public abstract class InstanceBase
{
    protected InstanceBase(MilkshakeService service, GenerationService generation, IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Service = service;
        Generation = generation;

        Instance = scope.ServiceProvider.GetRequiredService<MilkshakeInstance>();

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
    }

    protected InstanceBase(MilkshakeService service, GenerationService generation, IServiceProvider serviceProvider, string instanceName)
        : this(service, generation, serviceProvider)
    {
        InstanceName = instanceName;
    }

    internal string InstanceName { get; } = "default";

    protected readonly MilkshakeService Service;
    protected readonly MilkshakeInstance Instance;
    protected readonly GenerationService Generation;
}