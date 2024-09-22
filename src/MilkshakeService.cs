using System.Runtime.InteropServices.Marshalling;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Milkshake.Configuration;

namespace Milkshake;

public class MilkshakeService
{
    public readonly MilkshakeOptions Options;

    private readonly GenerationService _generation;

    private readonly ILogger<MilkshakeService> _logger;

    public MilkshakeService(IOptions<MilkshakeOptions> options, ILogger<MilkshakeService> logger, GenerationService generation)
    {
        _logger = logger;
        _generation = generation;
        Options = options.Value;
    }

    public MilkshakeInstance CreateInstance(string instanceName)
    {
        var instance = new MilkshakeInstance(this, _generation)
        {
            InstanceName = instanceName
        };
        return instance;
    }

    public MilkshakeInstance? GetInstance(string instanceName)
    {
        if (!Options.MultipleInstances)
        {
            _logger.LogWarning("Multiple Instances are disabled, " +
                               "{instanceName} cannot be accessed and the default instance will be opened instead.",
                instanceName);

            instanceName = "default";
        }

        //if (!Directory.Exists($"{Options.BasePath}/{instanceName}"))
        //    return null;

        var instance = new MilkshakeInstance(this, _generation)
        {
            InstanceName = instanceName
        };
        return instance;
    }
}