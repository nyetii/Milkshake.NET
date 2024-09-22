using Milkshake.Models.Interfaces;

namespace Milkshake.Services.Interfaces;

public interface IImageService
{
    public Task<MemoryStream> DownloadAsync(string url);
    
}