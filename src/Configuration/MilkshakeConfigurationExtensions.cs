using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Milkshake.Generation;
using Milkshake.Instances;
using Milkshake.Media;

namespace Milkshake.Configuration;

public static class MilkshakeConfigurationExtensions
{
    public static IServiceCollection AddMilkshake(this IServiceCollection services, Action<MilkshakeOptions> options)
    {
        services.AddOptions<MilkshakeOptions>()
            .Configure(options);

        return services.AddMilkshake();
    }

    public static IServiceCollection AddMilkshake(this IServiceCollection services, ConfigurationManager configuration, string name = "Milkshake")
    {
        services.AddOptions<MilkshakeOptions>()
            .Configure(options => 
                configuration.GetSection(name)
                    .Bind(options));

        return services.AddMilkshake();
    }

    internal static IServiceCollection AddMilkshake(this IServiceCollection services)
    {
        services.AddSingleton<IMilkshakeService, MilkshakeService>();
        services.AddSingleton<IGenerationService, GenerationService>();

        services.AddScoped<IMilkshakeInstance, MilkshakeInstance>();

        services.AddTransient<IMediaService, MediaService>();

        return services;
    }
}