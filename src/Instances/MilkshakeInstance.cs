using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Milkshake.Models;
using Milkshake.Models.Interfaces;

namespace Milkshake.Instances;

public class MilkshakeInstance
{
    private readonly MilkshakeService _service;


    public readonly GenerationService Generation;


    [Required(AllowEmptyStrings = false)]
    public string InstanceName { get; internal set; } = "default";

    public int TemplateCount => GetMilkshakeCount<Template>();
    public int SourceCount { get; set; }

    public int GenerationCount { get; internal set; }

    public MilkshakeInstance(MilkshakeService service, GenerationService generation)
    {
        _service = service;
        Generation = generation;
    }

    public MilkshakeInstance(MilkshakeService service, GenerationService generation, string instanceName)
    {
        _service = service;
        Generation = generation;
        InstanceName = instanceName;
    }

    private int GetMilkshakeCount<T>() where T : IMilkshake, IMedia
    {
        return Directory.EnumerateFiles($"{_service.Options.BasePath}/{typeof(T).Name.ToLower()}").Count();
    }

    public IGeneration CreateGeneration() => new Generation(this);
}