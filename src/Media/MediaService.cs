using System.Linq.Expressions;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;

namespace Milkshake.Media;

// TODO: Remove the Load, Save, Delete, etc. methods from the IMedia interface and implement them here instead
public class MediaService : IMediaService
{
    private readonly IMilkshakeService _service;
    private readonly IMilkshakeInstance _instance;

    public MediaService(IMilkshakeService service, IMilkshakeInstance instance)
    {
        _service = service;
        _instance = instance;
    }

    public IMilkshake Create<T>() where T : IMilkshake, IMedia, new()
    {
        var milkshake = new T
        {
            Id = Guid.NewGuid(),
            FileName = "",
            Stream = new MemoryStream()
        };

        return milkshake;
    }

    // TODO: Maybe turn this method virtual so it can be overriden in case of a database handling?
    // Probably database handling from here is not necessary, and we can handle it somewhere else with the dictionary.
    public T Find<T>(Guid guid) where T : class, IMilkshake, IMedia, new()
    {
        var milkshake = new T();

        milkshake = milkshake switch
        {
            Source => _instance.Sources[guid] as T,
            Template => throw new NotImplementedException(),
            _ => throw new NotImplementedException()
        };

        if (milkshake is null)
            throw new Exception($"Milkshake of ID \"{guid}\" could not be found.");

        return milkshake;
    }

    public async Task<T> LoadAsync<T>() where T : class, IMilkshake, IMedia, new()
    {
        var files = new DirectoryInfo(_service.GetDirectory<T>(_instance.Name))
            .GetFiles()
            .Where(x => x.Extension is not ".json")
            .ToArray();

        if (files.Length is 0)
            throw new Exception($"There are no valid images on the {typeof(T).Name.ToLower()} directory.");

        var selectedFile = files[Random.Shared.Next(files.Length - 1)];
        var name = selectedFile.Name.Replace(selectedFile.Extension, string.Empty);

        if (!Guid.TryParse(name, out var guid))
            throw new Exception("File contains invalid name, it must be a GUID.");

        var milkshake = Find<T>(guid);

        milkshake.FileName = selectedFile.Name;
        milkshake.Stream = new MemoryStream();

        var file = selectedFile.Open(FileMode.Open);
        await file.CopyToAsync(milkshake.Stream);

        return milkshake;
    }

    // This one assumes the milkshake has been fetched by a database or something, so it doesn't load from the dictionary.
    public async Task<T> LoadAsync<T>(T milkshake) where T : class, IMilkshake, IMedia
    {
        var fileInfo = new FileInfo(_service.GetDirectory<T>(_instance.Name, milkshake.FileName));

        milkshake.Stream = new MemoryStream();

        var file = fileInfo.Open(FileMode.Open);
        await file.CopyToAsync(milkshake.Stream);

        return milkshake;
    }

    public async Task<bool> SaveAsync<T>(IMilkshake milkshake, CancellationToken cancellationToken = default) where T : Media, IMilkshake, new()
    {
        var dictionary = GetDictionary<T>();

        if (milkshake is not T metadata)
            return false;

        if (!dictionary.TryAdd(milkshake.Id, metadata))
            return false;

        var directory = new DirectoryInfo(_service.GetDirectory<T>(_instance.Name));

        metadata.FileName = $"{milkshake.Id}.webp";

        if (_service.Options.SerializeMilkshakes)
            await UpdateMetadataFileAsync(directory, dictionary, cancellationToken);
        
        // TODO: Replace this with ImageMagick later on.
        await using var image = new FileStream($"{directory}/{metadata.FileName}", FileMode.Create);

        await metadata.Stream.CopyToAsync(image, cancellationToken);

        return true;
    }

    private async Task UpdateMetadataFileAsync<T>(DirectoryInfo directory, Dictionary<Guid, T> dictionary, CancellationToken cancellationToken = default) where T : Media, IMilkshake, new()
    {
        var metadata = directory.GetFiles().FirstOrDefault(x => x.Name is "metadata.json");

        if (metadata is null)
            throw new Exception($"Could not find metadata file for {typeof(T).Name}.");

        await using var stream = metadata.OpenWrite();
        if (stream is null)
            throw new Exception();
        
        await JsonSerializer.SerializeAsync(stream, dictionary, _service.SerializerOptions, cancellationToken);

        //await stream.CopyToAsync(file, cancellationToken);
    }

    private Dictionary<Guid, T> GetDictionary<T>() where T : Media, IMilkshake, new()
    {
        var t = new T();

        return t switch
        {
            Source => _instance.Sources as Dictionary<Guid, T> ?? throw new Exception("Source Dictionary is null."),
            Template => throw new NotImplementedException(),
            _ => throw new Exception("Unsupported type."),
        };
    }
}