namespace Milkshake.Models.Interfaces;

public interface IMedia<T> where T : class, IMilkshake
{
    public string FileName { get; set; }

    public Task<T> LoadAsync();
    public Task<T> LoadAsync(Guid guid);
    public Task<bool> SaveAsync(MemoryStream stream);
    public Task<bool> RenameAsync(string newName);
    public Task<bool> DeleteAsync();
}