namespace Milkshake.Models.Interfaces;

public interface IMedia
{
    public string FileName { get; set; }

    public Task<bool> SaveAsync(MemoryStream stream);
    public Task<bool> RenameAsync(string newName);
    public Task<bool> DeleteAsync();
}