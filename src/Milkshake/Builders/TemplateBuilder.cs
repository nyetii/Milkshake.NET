using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.Extensions.Hosting;
using Milkshake.Exceptions;
using Milkshake.Managers;
using Milkshake.Models;
using Milkshake.Models.Interfaces;
using Instance = Milkshake.Models.Instance;

namespace Milkshake.Builders
{
    public class TemplateBuilder
    {
        private readonly Template _template = new();

        private string _extension = "png";
        private string _url = string.Empty;
        
        private readonly MilkshakeService _service;


        public TemplateBuilder(MilkshakeService service, ContextData context)
        {
            _service = service;
            _template.MilkshakeContextId = context.ContextId;
        }

        public TemplateBuilder WithName(string name)
        {
            _template.Name = name;
            return this;
        }

        public TemplateBuilder WithDescription(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
                description = "No description provided.";

            _template.Description = description;
            return this;
        }

        public TemplateBuilder WithTags(ImageTags tags)
        {
            _template.Tags = tags;
            return this;
        }

        private void SetDimensions()
        {
            using var image =
                new MagickImage(_template.Path);

            _template.Width = image.Width;
            _template.Height = image.Height;
        }

        public TemplateBuilder WithUrl(string url)
        {
            _url = url;
            _extension = _url.Split('.').Last().ToLowerInvariant();

            var extensions = new[] { "png", "jpeg", "jpg", "webp" };

            if (!extensions.Contains(_extension))
                throw new InvalidMilkshakeException("Invalid file type.");

            _template.Path = _service.Options.MultipleInstances
                ? $"{_service.Options.BasePath}/{_template.MilkshakeContextId}/template/{_template.Name}-{_template.Id}.webp"
                : $"{_service.Options.BasePath}/template/{_template.Name}-{_template.Id}.webp";
            

            _template.Download(url, _template.Path, _service.Options.MaxDimensions);
            
            return this;
        }

        public TemplateBuilder WithStats(string creator)
        {
            _template.Creator = creator;
            return this;
        }

        public Template Build()
        {
            SetDimensions();

            return _template;
        }

    }
}
