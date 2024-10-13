using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Milkshake.Generation;
using Milkshake.Instances;

namespace Milkshake.Configuration;

public static class MilkshakeConfigurationExtensions
{
    public static IServiceCollection AddMilkshake(this IServiceCollection services) =>
        AddMilkshake(services, new MilkshakeOptions());

    public static IServiceCollection AddMilkshake(this IServiceCollection services, MilkshakeOptions options)
    {
        services.AddOptions<MilkshakeOptions>();
        services.AddSingleton<IMilkshakeService, MilkshakeService>();
        services.AddSingleton<IGenerationService, GenerationService>();
        services.AddScoped<IMilkshakeInstance, MilkshakeInstance>();

        return services;
    }
}