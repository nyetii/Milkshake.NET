﻿using System.Reflection.Metadata;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Milkshake.Configuration;

namespace Milkshake;

public class MilkshakeService : IMilkshakeService
{
    internal Dictionary<string, MilkshakeInstance> Instances { get; init; } = [];

    public MilkshakeOptions Options { get; init; }

    private readonly IGenerationService _generation;

    private readonly ILogger<MilkshakeService> _logger;

    public MilkshakeService(IOptions<MilkshakeOptions> options, ILogger<MilkshakeService> logger, IGenerationService generation)
    {
        _logger = logger;
        _generation = generation;
        Options = options.Value;
    }

    public string GetDirectory(string instanceName) => Path.Combine(Options.BasePath, instanceName);
    public string GetDirectory<T>() => Path.Combine(Options.BasePath, typeof(T).Name.ToLower());
    public string GetDirectory<T>(string instanceName, string fileName = "") => Path.Combine(Options.BasePath, instanceName, typeof(T).Name.ToLower(), fileName);

    //public MilkshakeInstance CreateInstance<T>(T caller) where T : class
    //{
    //    var attribute = InstanceAttribute.GetValue(caller);

    //    if (Options.MultipleInstances) 
    //        return new MilkshakeInstance(this, _generation, attribute.Name);

    //    _logger.LogWarning("Multiple Instances are disabled, " +
    //                       "{Name} cannot be accessed and the default instance will be opened instead.",
    //        attribute.Name);

    //    return new MilkshakeInstance(this, _generation);

    //}

    //public MilkshakeInstance CreateInstance(string instanceName)
    //{
    //    var instance = new MilkshakeInstance(this, _generation)
    //    {
    //        Name = instanceName
    //    };
    //    return instance;
    //}

    //public MilkshakeInstance? GetInstance(string instanceName)
    //{
    //    if (!Options.MultipleInstances)
    //    {
    //        _logger.LogWarning("Multiple Instances are disabled, " +
    //                           "{instanceName} cannot be accessed and the default instance will be opened instead.",
    //            instanceName);

    //        instanceName = "default";
    //    }

    //    //if (!Directory.Exists($"{Options.BasePath}/{instanceName}"))
    //    //    return null;

    //    var instance = new MilkshakeInstance(this, _generation)
    //    {
    //        Name = instanceName
    //    };
    //    return instance;
    //}
}