using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Milkshake.Generation;
using Milkshake.Media.Models;

namespace Milkshake.Instances;

public class MilkshakeInstance : IMilkshakeInstance
{
    private readonly ILogger<MilkshakeInstance> _logger;

    private readonly IMilkshakeService _service;

    // TODO: Rename to Name, because InstanceName is fugly.
    [Required(AllowEmptyStrings = false)]
    public string InstanceName { get; set; } = "default";

    public string BaseDirectory => _service.GetDirectory(InstanceName);

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

    private readonly Lazy<Dictionary<Guid, Source>> _sources;
    public Dictionary<Guid, Source> Sources => _sources.Value;

    public MilkshakeInstance(IMilkshakeService service, ILogger<MilkshakeInstance> logger)
    {
        _service = service;
        _logger = logger;

        _sources = new Lazy<Dictionary<Guid, Source>>(LoadMetadata<Source>);
    }

    public void Initialize()
    {
        VerifyDirectories();
    }

    // TODO: Add a check if the serialized files are enabled, case they're not, probably return an empty Dictionary.
    // That's for database handling, in that case the developer would opt the best way to handle it.
    private Dictionary<Guid, T> LoadMetadata<T>() where T : class, IMilkshake
    {
        _logger.LogDebug("Loading {name}s of {instanceName}", typeof(T).Name, InstanceName);

        var directory = _service.GetDirectory<Source>(InstanceName, "metadata.json");

        var json = JsonSerializer.Deserialize<Dictionary<Guid, T>>(directory);

        if (json is null)
            throw new Exception($"metadata.json of {typeof(T).Name} could not be loaded.");

        return json;
    }

    private void VerifyDirectories()
    {
        _logger.LogDebug("Verifying directories of {instanceName}", InstanceName);
        
        var baseDirectory = new DirectoryInfo(BaseDirectory);

        if (baseDirectory is null)
            throw new Exception($"Could not find the directory of the instance \"{InstanceName}\".");

        if (!baseDirectory.Exists)
            baseDirectory.Create();

        var subDirectories = baseDirectory.EnumerateDirectories().ToList();

        if (!subDirectories.Any(x => x.Name is "source"))
            subDirectories.Add(baseDirectory.CreateSubdirectory("source"));

        if (!subDirectories.Any(x => x.Name is "template"))
            subDirectories.Add(baseDirectory.CreateSubdirectory("template"));

        if (!subDirectories.Any(x => x.Name is "generation"))
            subDirectories.Add(baseDirectory.CreateSubdirectory("generation"));

        foreach (var subDirectory in subDirectories)
        {
            // TODO: Maybe a check if the milkshakes can be Serialized or not.
            if (subDirectory.Name is not ("source" or "template" or "generation")
                || subDirectory.GetFiles("metadata.json", SearchOption.TopDirectoryOnly).Length is not 0) 
                continue;

            var dict = new Dictionary<Guid, object>();
            File.WriteAllText($"{subDirectory.FullName}/metadata.json", JsonSerializer.Serialize(dict));

            _logger.LogDebug("Created metadata.json of {name}", subDirectory.Name);
        }

        _logger.LogDebug("The directories of {instanceName} are good to go", InstanceName);
    }

    private int GetMilkshakeCount<T>() where T : class, IMilkshake, IMedia
    {
        return Directory.EnumerateFiles($"{BaseDirectory}/{typeof(T).Name.ToLower()}").Count();
    }

    public IGeneration CreateGeneration() => new Generation.Generation(this);
}