using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
using Milkshake.Managers;
using Milkshake.Models;
using Milkshake.Models.Interfaces;

namespace Milkshake.Builders
{
    public class SourceBuilder
    {
        private string _extension = "png";
        private string _url = string.Empty;
        private string _filePath = string.Empty;

        private string _name = string.Empty;
        private string _description = string.Empty;
        private ImageTags _tags;
        private int _width = 0;
        private int _height = 0;

        //private readonly ISource _source = new T();

        private readonly MilkshakeService _service;

        public SourceBuilder(MilkshakeService service)
        {
            _service = service;
        }

        public SourceBuilder<T> WithName(string name)
        {
            _name = name;
            return this;
        }

        public SourceBuilder<T> WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public SourceBuilder<T> WithTags(ImageTags tags)
        {
            _tags = tags;
            return this;
        }

        private void SetDimensions()
        {
            using var image =
                new MagickImage(_source.Path);

            _width = image.Width;
            _height = image.Height;
            //_source.Width = width;
            //_source.Height = height;
            //return this;
        }

        public SourceBuilder<T> WithUrl(string url)
        {
            _url = url;
            _extension = _url.Split('.').Last().ToLowerInvariant();

            var extensions = new[] { "png", "jpeg", "jpg" };

            if (!extensions.Contains(_extension))
                throw new NotSupportedException("Invalid file type.");

            _filePath = $"{_service.Options.BasePath}/{_source.MilkshakeContextId}/Source";

            _source.Download(url);

            return this;
        }

        public ISource Build()
        {
            _source.Path = $"{_service.Options.BasePath}/{_source.Name}-{_source.MilkshakeContextId}.{_extension}";
            SetDimensions();

            return new Source();
        }

        private class Source : ISource
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public ImageTags Tags { get; set; }
            public DateTime CreationDateTime { get; set; }
            public string Path { get; set; }
            public Guid MilkshakeContextId { get; set; }
        }
    }
}
