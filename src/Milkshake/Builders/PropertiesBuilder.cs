using Milkshake.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milkshake.Models;

namespace Milkshake.Builders
{
    public class PropertiesBuilder
    {
        private readonly MilkshakeService _service;
        private readonly Template _template;

        private protected TemplateProperties _properties = new();

        public PropertiesBuilder(MilkshakeService service, Template template)
        {
            _service = service;
            _template = template;
            _properties.TemplateId = template.Id;
        }

        public PropertiesBuilder WithName(string name)
        {
            _properties.Name = name;
            return this;
        }

        public PropertiesBuilder WithDescription(string description)
        {
            _properties.Description = description;
            return this;
        }

        public PropertiesBuilder WithTags(ImageTags tags)
        {
            _properties.Tags = tags;
            return this;
        }

        public PropertiesBuilder WithAnchors(int x, int y)
        {
            _properties.X = x;
            _properties.Y = y;
            return this;
        }

        public PropertiesBuilder WithDimensions(int width, int height)
        {
            _properties.Width = width;
            _properties.Height = height;
            return this;
        }

        public PropertiesBuilder WithLayer(Layer layer)
        {
            _properties.Layer = layer;
            return this;
        }

        public PropertiesBuilder WithText(Action<PropertiesTextBuilder> textProperties)
        {
            var props = new PropertiesTextBuilder();
            textProperties(props);

            _properties.IsText = true;

            _properties.Color = props.Color ?? _properties.Color;
            _properties.Orientation = props.Orientation ?? _properties.Orientation;
            _properties.Font = props.Font ?? _properties.Font;
            _properties.StrokeColor = props.StrokeColor ?? _properties.StrokeColor;
            _properties.StrokeWidth = props.StrokeWidth ?? _properties.StrokeWidth;

            return this;
        }

        public TemplateProperties Build()
        {
            return _properties;
        }
    }
}
