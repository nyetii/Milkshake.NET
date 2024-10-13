using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Milkshake.Generation;
using Milkshake.Media.Models;

namespace Milkshake.Instances;

public class MilkshakeInstance : IMilkshakeInstance
{
    private readonly IMilkshakeService _service;

    [Required(AllowEmptyStrings = false)]
    public string InstanceName { get; set; } = "default";

    public int TemplateCount => GetMilkshakeCount<Template>();
    public int SourceCount => GetMilkshakeCount<Source>();

    public int GenerationCount { get; set; }

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

    public MilkshakeInstance(IMilkshakeService service)
    {
        _service = service;
    }

    private int GetMilkshakeCount<T>() where T : class, IMilkshake, IMedia<T>
    {
        return Directory.EnumerateFiles($"{_service.Options.BasePath}/{typeof(T).Name.ToLower()}").Count();
    }

    public IGeneration CreateGeneration() => new Generation.Generation(this);
}