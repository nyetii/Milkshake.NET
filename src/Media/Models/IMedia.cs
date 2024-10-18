namespace Milkshake.Media.Models;

public interface IMedia
{
    public string FileName { get; internal set; }

    public MemoryStream Stream { get; internal set; }

    public long Size { get; }

    // TODO: Remove Load, Save, Rename, Delete methods.
    // TODO: Add MemoryStream property.
    // TODO: Maybe add a type of T property? A IMedia will always be a IMilkshake but a IMilkshake not necessarily is an IMedia.
    // TODO: Maybe create an abstract class out of IMedia? Instead of objects implementing IMedia, they would extend Media.
}