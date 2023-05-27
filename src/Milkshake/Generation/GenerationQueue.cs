using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Milkshake.Exceptions;
using Milkshake.Models;

namespace Milkshake.Generation
{
    public class GenerationQueue
    {
        private readonly MilkshakeService _service;
        private Queue<Generation> _queue = new();

        public GenerationQueue(MilkshakeService service)
        {
            _service = service;
        }

        /// <summary>
        /// The client's <see cref="Generation"/> object is passed to the library and is added to the queue.
        /// </summary>
        /// <param name="prompt"></param>
        /// <exception cref="InvalidMilkshakeException"></exception>
        public void Enqueue(Generation prompt)
        {

            if (prompt.Template is null)
                throw new InvalidMilkshakeException("The provided template is null.");

            if (prompt.Template.Properties is null || prompt.Template.Properties.Count < 1)
                throw new InvalidMilkshakeException("No properties for the template were found.");

            var properties = prompt.Template.Properties.ToArray();
            

            _queue.Enqueue(prompt);
        }

        /// <summary>
        /// Generates the next Milkshake on the queue.
        /// </summary>
        /// <remarks>The Generated data is sent to <seealso cref="ImageGenerated"/> event.</remarks>
        /// <returns><see cref="Task"/></returns>
        public async Task Generate()
        {
            if (_queue.Count is 0) 
                return;
            var prompt = _queue.Dequeue();

            using var canvas = new MagickImage(prompt.Template!.Path, new MagickReadSettings{BackgroundColor = MagickColors.Transparent});
            
            
            
            using var overCanvas = new MagickImage(width: prompt.Template.Width, height: prompt.Template.Height, color: new MagickColor(MagickColors.Transparent));

            using var underCanvas = new MagickImage(width: prompt.Template.Width, height: prompt.Template.Height, color: new MagickColor(MagickColors.Transparent));

            var properties = PopulateSources(prompt);
            var layers = new List<MagickImage>();

            foreach (var property in properties)
            {
                MagickReadSettings settings;
                if(!property.properties.IsText)
                {
                    settings = new MagickReadSettings()
                    {
                        Width = property.properties.Width,
                        Height = property.properties.Height,
                        BackgroundColor = MagickColors.Transparent
                    };

                    var source = new MagickImage(property.source.Path, settings);

                    switch (property.properties.Layer)
                    {
                        case Layer.Background:
                            source.Distort(DistortMethod.Resize, property.properties.Width, property.properties.Height);
                            underCanvas.Composite(source, property.properties.X, property.properties.Y, CompositeOperator.Over);
                            break;
                        case Layer.Base:
                            source.Distort(DistortMethod.Resize, property.properties.Width, property.properties.Height);
                            canvas.Composite(source, property.properties.X, property.properties.Y, CompositeOperator.Over);
                            break;
                        case Layer.Foreground:
                            source.Distort(DistortMethod.Resize, property.properties.Width, property.properties.Height);
                            overCanvas.Composite(source, property.properties.X, property.properties.Y, CompositeOperator.Over);
                            break;
                        default:
                            break;
                    }
                    
                    source.Dispose();
                }
                else
                {

                    settings = new MagickReadSettings()
                    {
                        Font = property.properties.Font,
                        FillColor = new MagickColor(property.properties.Color),
                        TextGravity = property.properties.Orientation,
                        Width = property.properties.Width,
                        Height = property.properties.Height,
                        BackgroundColor = MagickColors.Transparent
                    };

                    if (property.properties.StrokeWidth > 0)
                    {
                        settings.StrokeColor = new MagickColor(property.properties.StrokeColor);
                        settings.StrokeWidth = property.properties.StrokeWidth;
                        settings.StrokeAntiAlias = true;
                    }

                    using var source = new MagickImage($"caption: {property.source.Name}", settings);
                    canvas.Composite(source, property.properties.X, property.properties.Y, CompositeOperator.Over);
                    source.Dispose();
                    
                }
            }

            

            using var image = new MagickImageCollection();

            image.Add(underCanvas);
            image.Add(canvas);
            image.Add(overCanvas);

            var output = image.Mosaic();

            output.Format = MagickFormat.Png;
            
            await using var stream = new FileStream(path: $"{_service.Options.BasePath}/cache/{prompt.Id}.png", FileMode.OpenOrCreate);

            await output.WriteAsync(stream);
            
            await OnImageGenerated(prompt, new GeneratedEventArgs() {Id = prompt.Id, FilePath = stream.Name});
        }

        /// <summary>
        /// Fires when a Milkshake generation is ready.
        /// </summary>
        public event GeneratedHandler? ImageGenerated;

        public delegate Task GeneratedHandler(object sender, GeneratedEventArgs args);

        private async Task OnImageGenerated(object sender, GeneratedEventArgs args)
        {
            ImageGenerated?.Invoke(sender, args);

            await Task.CompletedTask;
        }

        /// <summary>
        /// Populates each <see cref="TemplateProperties"/> of a <see cref="Template"/> with a <see cref="Source"/>.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns><see cref="List{T}"/></returns>
        private List<(Source source, TemplateProperties properties)> PopulateSources(Generation prompt)
        {
            var properties = prompt.Template.Properties.ToArray();
            var sources = prompt.Sources.ToArray();
            

            var populatedSources = new List<(Source source, TemplateProperties properties)>();
            var duplicate = new List<string>();

            for (int i = 0; i < properties.Length && i < sources.Length; i++)
            {
                if (!duplicate.Any(x => x.Equals(properties[i].Name)))
                {
                    var propertyGroup = properties.Where(x => x.Name == properties[i].Name).ToArray();

                    if (propertyGroup.Length > 1)
                        duplicate.Add(propertyGroup!.FirstOrDefault()!.Name);

                    var filteredSources = sources.Where(x => x.Tags.HasFlag(properties[i].Tags)).ToArray();

                    if (filteredSources.Length is 0)
                    {
                        filteredSources = sources;
                    }

                    var src = new Random().Next(0, filteredSources.Length);

                    foreach(var item in propertyGroup)
                    {
                        prompt.Properties.Add((item.Name, filteredSources[src]));
                        populatedSources.Add((filteredSources[src], item));
                    }
                }
            }

            return populatedSources;
        }
    }
}
