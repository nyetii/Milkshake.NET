using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
using Milkshake.Exceptions;
using Milkshake.Managers;
using Milkshake.Models;
using Milkshake.Models.Interfaces;
using Instance = Milkshake.Models.Instance;

namespace Milkshake.Builders
{
    public class SourceBuilder
    {
        private readonly Source _source = new();

        private string _extension = "png";
        private string _url = string.Empty;

        //private readonly ISource _source = new T();

        private readonly MilkshakeService _service;
        

        public SourceBuilder(MilkshakeService service, ContextData context)
        {
            _service = service;
            _source.MilkshakeContextId = context.ContextId;
        }

        public SourceBuilder WithName(string name)
        {
            _source.Name = name;
            return this;
        }

        public SourceBuilder WithDescription(string? description)
        {
            _source.Description = description ?? "No description provided.";
            return this;
        }

        public SourceBuilder WithTags(ImageTags tags)
        {
            _source.Tags = tags;
            return this;
        }

        private void SetDimensions()
        {
            using var image =
                new MagickImage(_source.Path);

            _source.Width = image.Width;
            _source.Height = image.Height;

            //_source.Width = width;
            //_source.Height = height;
            //return this;
        }

        public SourceBuilder WithUrl(string url)
        {
            _url = url;
            _extension = _url.Split('.').Last().ToLowerInvariant();

            var extensions = new[] { "png", "jpeg", "jpg", "webp" };

            if (!extensions.Contains(_extension))
                throw new InvalidMilkshakeException("Invalid file type.");

            _source.Path = _service.Options.MultipleInstances
                ? $"{_service.Options.BasePath}/{_source.MilkshakeContextId}/source/{_source.Name}-{_source.Id}.webp"
                : $"{_service.Options.BasePath}/source/{_source.Name}-{_source.Id}.webp";
            
            //_source.Path = $"{_service.Options.BasePath}/{_source.MilkshakeContextId}/source/{_source.Name}-{_source.Id}.webp";

            _source.Download(url, _source.Path, _service.Options.MaxDimensions);

            return this;
        }

        public SourceBuilder WithStats(string creator)
        {
            _source.Creator = creator;
            return this;
        }

        public Source Build()
        {
            //_source.Path = $"{_service.Options.BasePath}/{_source.Name}-{_source.MilkshakeContextId}.{_extension}";
            SetDimensions();

            return _source;
        }
    }
}
