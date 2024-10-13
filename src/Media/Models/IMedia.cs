namespace Milkshake.Media.Models;

public interface IMedia<T> where T : class, IMilkshake
{
    public string FileName { get; set; }

    // TODO: Remove Load, Save, Rename, Delete methods.
    // TODO: Add MemoryStream property.
    // TODO: Maybe add a type of T property? A IMedia will always be a IMilkshake but a IMilkshake not necessarily is an IMedia

    public Task<T> LoadAsync();
    public Task<T> LoadAsync(Guid guid);
    public Task<bool> SaveAsync(MemoryStream stream);
    public Task<bool> RenameAsync(string newName);
    public Task<bool> DeleteAsync();
}