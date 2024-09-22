using System.ComponentModel.DataAnnotations;
using Milkshake.Models;
using Milkshake.Models.Interfaces;

namespace Milkshake;

public class MilkshakeInstance
{
    private readonly MilkshakeService _service;


    public readonly GenerationService Generation;


    [Required(AllowEmptyStrings = false)] 
    public string InstanceName { get; set; } = null!;

    public int TemplateCount => GetMilkshakeCount<Template>();
    public int SourceCount { get; set; }

    public int GenerationCount { get; set; }

    public MilkshakeInstance(MilkshakeService service, GenerationService generation)
    {
        _service = service;
        Generation = generation;
    }

    private int GetMilkshakeCount<T>() where T : IMilkshake, IMedia
    {
        return Directory.EnumerateFiles($"{_service.Options.BasePath}/{typeof(T).Name.ToLower()}").Count();
    }

    public IGeneration CreateGeneration() => new Generation(this);
}