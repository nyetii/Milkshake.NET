using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
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

            if (prompt.Template.Toppings is null || prompt.Template.Toppings.Count < 1)
                throw new InvalidMilkshakeException("No toppings for the template were found.");

            var toppings = prompt.Template.Toppings.ToArray();
            

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

            var milkshakes = PopulateSources(prompt);
            var layers = new List<MagickImage>();

            foreach (var milkshake in milkshakes)
            {
                MagickReadSettings settings;
                if(!milkshake.topping.IsText)
                {
                    settings = new MagickReadSettings()
                    {
                        Width = milkshake.topping.Width,
                        Height = milkshake.topping.Height,
                        BackgroundColor = MagickColors.Transparent
                    };

                    var source = new MagickImage(milkshake.source.Path, settings);

                    var filters = Enum.GetValues(typeof(Filter));
                    
                    if(milkshake.topping.Filter != 0)
                        foreach (Filter filter in filters)
                        {
                            if (milkshake.topping.Filter.HasFlag(filter))
                                switch (filter)
                                {
                                    case Filter.Grayscale:
                                        source.Grayscale();
                                        break;
                                    case Filter.Vignette:
                                        source.Vignette();
                                        break;
                                    case Filter.Blur:
                                        source.Blur(Channels.All);
                                        break;
                                    case Filter.Sepia:
                                        source.SepiaTone();
                                        break;
                                    case Filter.Charcoal:
                                        source.Charcoal();
                                        break;
                                    case Filter.Emboss:
                                        source.Emboss();
                                        break;
                                    case Filter.OilPaint:
                                        source.OilPaint();
                                        break;
                                    case Filter.Negate:
                                        source.Negate(Channels.CMYK);
                                        break;
                                    case Filter.None:
                                    default:
                                        break;

                                }
                        }
                    
                    switch (milkshake.topping.Layer)
                    {
                        case Layer.Background:
                            source.Distort(DistortMethod.Resize, milkshake.topping.Width, milkshake.topping.Height);
                            underCanvas.Composite(source, milkshake.topping.X, milkshake.topping.Y, CompositeOperator.Over);
                            break;
                        case Layer.Base:
                            source.Distort(DistortMethod.Resize, milkshake.topping.Width, milkshake.topping.Height);
                            canvas.Composite(source, milkshake.topping.X, milkshake.topping.Y, CompositeOperator.Over);
                            break;
                        case Layer.Foreground:
                            source.Distort(DistortMethod.Resize, milkshake.topping.Width, milkshake.topping.Height);
                            overCanvas.Composite(source, milkshake.topping.X, milkshake.topping.Y, CompositeOperator.Over);
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
                        Font = milkshake.topping.Font,
                        FillColor = new MagickColor(milkshake.topping.Color),
                        TextGravity = milkshake.topping.Orientation,
                        Width = milkshake.topping.Width,
                        Height = milkshake.topping.Height,
                        BackgroundColor = MagickColors.Transparent
                    };

                    if (milkshake.topping.StrokeWidth > 0)
                    {
                        settings.StrokeColor = new MagickColor(milkshake.topping.StrokeColor);
                        settings.StrokeWidth = milkshake.topping.StrokeWidth;
                        settings.StrokeAntiAlias = true;
                    }

                    using var source = new MagickImage($"caption: {milkshake.source.Name}", settings);
                    canvas.Composite(source, milkshake.topping.X, milkshake.topping.Y, CompositeOperator.Over);
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
        /// Populates each <see cref="Topping"/> of a <see cref="Template"/> with a <see cref="Source"/>.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns><see cref="List{T}"/></returns>
        private List<(Source source, Topping topping)> PopulateSources(Generation prompt)
        {
            var toppings = prompt.Template.Toppings.ToArray();
            var sources = prompt.Sources.ToArray();
            

            var populatedMilkshakes = new List<(Source source, Topping topping)>();
            var duplicate = new List<string>();

            for (int i = 0; i < toppings.Length && i < sources.Length; i++)
            {
                if (!duplicate.Any(x => x.Equals(toppings[i].Name)))
                {
                    var propertyGroup = toppings.Where(x => x.Name == toppings[i].Name).ToArray();

                    if (propertyGroup.Length > 1)
                        duplicate.Add(propertyGroup!.FirstOrDefault()!.Name);

                    var filteredSources = sources.Where(x => x.Tags.HasFlag(toppings[i].Tags)).ToArray();

                    if (filteredSources.Length is 0)
                    {
                        filteredSources = sources;
                    }

                    var src = new Random().Next(0, filteredSources.Length);

                    foreach(var item in propertyGroup)
                    {
                        prompt.Properties.Add((item.Name, filteredSources[src]));
                        populatedMilkshakes.Add((filteredSources[src], item));
                    }
                }
            }

            return populatedMilkshakes;
        }
    }
}
