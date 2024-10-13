using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Milkshake.Models;
using Milkshake.Models.Interfaces;

namespace Milkshake.Instances;

public class MilkshakeInstance : IInstance
{
    private readonly MilkshakeService _service;

    [Required(AllowEmptyStrings = false)]
    public string InstanceName { get; internal set; } = "default";

    public int TemplateCount => GetMilkshakeCount<Template>();
    public int SourceCount => GetMilkshakeCount<Source>();

    public int GenerationCount { get; internal set; }

    //public MilkshakeInstance(MilkshakeService service, GenerationService generation)
    //{
    //    _service = service;
    //    Generation = generation;
    //}

    //public MilkshakeInstance(MilkshakeService service, GenerationService generation, string instanceName)
    //{
    //    _service = service;
    //    Generation = generation;
    //    InstanceName = instanceName;
    //}

    public MilkshakeInstance(MilkshakeService service)
    {
        _service = service;
    }

    private int GetMilkshakeCount<T>() where T : class, IMilkshake, IMedia<T>
    {
        return Directory.EnumerateFiles($"{_service.Options.BasePath}/{typeof(T).Name.ToLower()}").Count();
    }

    public IGeneration CreateGeneration() => new Generation(this);
}