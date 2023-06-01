using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using ImageMagick;
using Milkshake.Models;
using Milkshake.Models.Interfaces;

namespace Milkshake.Managers
{
    public static class ImageManager
    {
        private static HttpClient? _httpClient;
        
        internal static void Download<T>(this T value, string url, string path, (int width, int height) limit) where T : class, IMedia
        {
            var stream = HandleDownload(url).Result;
            ToWebp(stream, limit, path).Wait();
        }

        private static async Task<MemoryStream> HandleDownload(string url) 
        {
            var extension = url.Split('.').Last().ToLowerInvariant();

            var extensions = new[] { "png", "jpeg", "jpg", "webp" };

            if (!extensions.Contains(extension))
                throw new NotSupportedException("Invalid file type.");
            

            var socketsHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromSeconds(30)
            };

            _httpClient = new HttpClient(socketsHandler);

            var attachment = await _httpClient.GetAsync(url);
            attachment.EnsureSuccessStatusCode();

            var memoryStream = new MemoryStream();

            await attachment.Content.CopyToAsync(memoryStream);

            return memoryStream;
        }
        
        private static async Task ToWebp(MemoryStream stream, (int width, int height) limit, string path)
        {
            stream.Position = 0;
            using var image = new MagickImage(stream);

            if (image.Width > limit.width || image.Height > limit.height)
            {
                var resize = new MagickGeometry();
                resize.Width = limit.width;
                resize.Height = limit.height;
                //resize.FillArea = true;

                resize.IgnoreAspectRatio = false;

                image.AdaptiveResize(resize);
            }

            image.Format = MagickFormat.WebP;

            stream.Position = 0;
            await image.WriteAsync(stream);
            

            stream.Position = 0;
            using var output = new MagickImage(stream);
            stream.Position = 0;
            await using var filestream = new FileStream($"{path}", FileMode.CreateNew);
            await output.WriteAsync(filestream);

            await stream.DisposeAsync();
        }

        /// <summary>
        /// Renames the Milkshake file based on the <paramref name="name"/> parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <returns>The new file name in order to be update <see cref="IMedia.Path"/>.</returns>
        public static string RenameFile<T>(this T value, string name) where T : class, IMedia
        {
            var filename = Path.GetFileNameWithoutExtension(value.Path);
            var extension = Path.GetExtension(value.Path);

            var path = value.Path.Replace(filename, name);

            var fileInfo = new FileInfo(value.Path);

            fileInfo.MoveTo(path);

            return path;
        }

        /// <summary>
        /// Returns the Milkshake file name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns>The Milkshake file name.</returns>
        public static string GetFileName<T>(this T value) where T : class, IMedia
        {
            return Path.GetFileName(value.Path);
        }

        /// <summary>
        /// Returns the Milkshake filtered file name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns>A file name without special characters for URLs.</returns>
        public static string GetFilteredFileName<T>(this T value) where T : class, IMedia
        {
            var filename = Path.GetFileName(value.Path);

            filename = Regex.Replace(filename, "\\s+", "_");
            filename = new string(filename.Where(x => !char.IsSymbol(x) && !char.IsPunctuation(x) || x is '.' or '_' or '-').ToArray());

            return filename;
        }

        /// <summary>
        /// Deletes the Milkshake file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static void Delete<T>(this T value) where T : class, IMedia
        {
            File.Delete(value.Path);
        }
    }
}
