using Milkshake.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milkshake.Models;

namespace Milkshake.Builders
{
    public class ToppingBuilder
    {
        private readonly MilkshakeService _service;
        private readonly Template _template;

        private readonly Topping _topping = new();

        public ToppingBuilder(MilkshakeService service, Template template)
        {
            _service = service;
            _template = template;
            _topping.TemplateId = template.Id;
        }

        public ToppingBuilder WithName(string name)
        {
            _topping.Name = name;
            return this;
        }

        public ToppingBuilder WithDescription(string description)
        {
            _topping.Description = description;
            return this;
        }

        public ToppingBuilder WithTags(ImageTags tags)
        {
            _topping.Tags = tags;
            return this;
        }

        public ToppingBuilder WithAnchors(int x, int y)
        {
            _topping.X = x;
            _topping.Y = y;
            return this;
        }

        public ToppingBuilder WithDimensions(int width, int height)
        {
            _topping.Width = width;
            _topping.Height = height;
            return this;
        }

        public ToppingBuilder WithLayer(Layer layer)
        {
            _topping.Layer = layer;
            return this;
        }

        public ToppingBuilder WithText(Action<TextPropertiesBuilder> textProperties)
        {
            var props = new TextPropertiesBuilder();
            textProperties(props);

            _topping.IsText = true;

            _topping.Color = props.Color ?? _topping.Color;
            _topping.Orientation = props.Orientation ?? _topping.Orientation;
            _topping.Font = props.Font ?? _topping.Font;
            _topping.StrokeColor = props.StrokeColor ?? _topping.StrokeColor;
            _topping.StrokeWidth = props.StrokeWidth ?? _topping.StrokeWidth;

            return this;
        }

        public Topping Build()
        {
            return _topping;
        }
    }
}
