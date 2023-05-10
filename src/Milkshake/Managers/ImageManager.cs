using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Milkshake.Models.Interfaces;

namespace Milkshake.Managers
{
    public static class ImageManager
    {
        private static HttpClient? _httpClient;

        public static void Download<T>(this T value, string url) where T : class, IMedia, new()
        {
            HandleDownload(value, url).Start();
        }

        private static async Task HandleDownload<T>(T milkshake, string url) where T : class, IMedia
        {
            var extension = url.Split('.').Last().ToLowerInvariant();

            var extensions = new[] { "png", "jpeg", "jpg" };

            if (!extensions.Contains(extension))
                throw new NotSupportedException("Invalid file type.");

            var path = $"{milkshake.Path}/{milkshake.Name}-{milkshake.Id}.{extension}";

            var socketsHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromSeconds(30)
            };

            _httpClient = new HttpClient(socketsHandler);

            var attachment = await _httpClient.GetAsync(url);
            attachment.EnsureSuccessStatusCode();

            await using var filestream = new FileStream(path, FileMode.CreateNew);
            await attachment.Content.CopyToAsync(filestream);
        }
    }
}
