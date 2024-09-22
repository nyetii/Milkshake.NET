﻿using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Milkshake.Configuration;

public static class MilkshakeConfigurationExtensions
{
    public static IServiceCollection AddMilkshake(this IServiceCollection services) =>
        AddMilkshake(services, new MilkshakeOptions());

    public static IServiceCollection AddMilkshake(this IServiceCollection services, MilkshakeOptions options)
    {
        services.AddSingleton<MilkshakeService>();
        services.AddSingleton<GenerationService>();
        services.AddScoped<MilkshakeInstance>();

        return services;
    }
}