using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Milkshake.Generation;
using Milkshake.Instances;
using Milkshake.Media;

namespace Milkshake.Configuration;

public static class MilkshakeConfigurationExtensions
{
    public static IServiceCollection AddMilkshake(this IServiceCollection services) =>
        AddMilkshake(services, new MilkshakeOptions());

    public static IServiceCollection AddMilkshake(this IServiceCollection services, MilkshakeOptions options)
    {
        // TODO: Check Options, its value is not persisting, apparently.
        services.AddOptions<MilkshakeOptions>();
        services.AddSingleton<IMilkshakeService, MilkshakeService>();
        services.AddSingleton<IGenerationService, GenerationService>();

        services.AddScoped<IMilkshakeInstance, MilkshakeInstance>();

        services.AddTransient<IMediaService, MediaService>();

        return services;
    }
}