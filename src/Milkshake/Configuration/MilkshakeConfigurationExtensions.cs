using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Milkshake.Attributes;
using Milkshake.Builders;
using Milkshake.Crud;
using Milkshake.Generation;
using Milkshake.Managers;
using Milkshake.Models.Interfaces;

namespace Milkshake.Configuration
{
    public static class MilkshakeConfigurationExtensions
    {
        public static IHostBuilder ConfigureMilkshake(this IHostBuilder hostBuilder, Action<IMilkshakeConfiguration> configureMilkshake)
        {
            return hostBuilder.ConfigureServices((context, collection) => collection.AddMilkshake(configureMilkshake));
        }

        public static IHostBuilder ConfigureMilkshake(this IHostBuilder hostBuilder, Assembly assembly, Action <IMilkshakeConfiguration> configureMilkshake)
        {
            return hostBuilder.ConfigureServices((context, collection) => collection.AddMilkshake(configureMilkshake, assembly));
        }

        public static IHostBuilder ConfigureMilkshake<T>(this IHostBuilder hostBuilder, Action<IMilkshakeConfiguration> configureMilkshake)
        {
            Assembly assembly = typeof(T).Assembly;
            return hostBuilder.ConfigureServices((context, collection) => collection.AddMilkshake(configureMilkshake, assembly));
        }

        public static IServiceCollection AddMilkshake(this IServiceCollection services, Action<IMilkshakeConfiguration> configure,
            Assembly? assembly = null)
        {
            services.AddOptions();
            services.AddSingleton<MilkshakeService>();
            services.AddSingleton<SourceBuilder>();
            services.AddSingleton<GenerationQueue>();
            services.AddTransient<ContextData>();

            var provider = services.BuildServiceProvider();

            configure(new MilkshakeConfiguration(services, assembly, provider.GetRequiredService<MilkshakeService>()));

            var ms = provider.GetRequiredService<MilkshakeService>();
            ms.Log($"Milkshake v{ms.Version.ToString(3)}");

            return services;
        }

        public static IMilkshakeConfiguration AddInstanceManager(this IMilkshakeConfiguration milkshake)
        {
            Type genericClass = typeof(InstanceBuilder<>);
            Type attribute = InstanceAttribute.InstanceType(milkshake.Assembly);
            Type instanceBuilder = genericClass.MakeGenericType(attribute);

            Type instanceManager = typeof(InstanceManager<>).MakeGenericType(attribute);

            milkshake.Services.AddScoped(instanceBuilder);
            milkshake.Services.AddScoped(instanceManager);

            return milkshake;
        }

        public static IMilkshakeConfiguration AddCrud(this IMilkshakeConfiguration milkshake, Assembly? assembly = null)
        {
            assembly ??= milkshake.Assembly;

            //var a = CrudAttribute.GetType(assembly);

            //var crud = typeof(ICrud<>);
            //var generic = crud.MakeGenericType(crud, a);
            //var activated = Activator.CreateInstance(generic);

            var types = new List<Type>();

            foreach (var assemblyType in assembly.GetTypes())
            {
                var attribute = Attribute.GetCustomAttributes(assemblyType);

                foreach (var item in attribute)
                    if (item is HandleAttribute handle)
                    {
                        types.Add(handle.Type);
                        break;
                    }
            }

            //Type? self = assembly!.GetTypes().FirstOrDefault(assemblyType => assemblyType.IsAssignableTo(typeof(ICrud<>)));

            if (types is null)
            {
                milkshake.Milkshake.Log("No best CRUD type found.", Severity.Warning);
                return milkshake;
            }

            foreach (var item in types)
            {

                if (item.IsAssignableTo(typeof(IMilkshake)) || item.IsAssignableTo(typeof(IInstanceBase)))
                {
                    var genericClass = typeof(ICrud<>);
                    var i = genericClass.MakeGenericType(item);

                    //Type? self = assembly!.GetTypes().FirstOrDefault(assemblyType => assemblyType.IsAssignableTo(i));

                    Type? self = null;

                    foreach (var type in assembly.GetTypes())
                    {
                        var attribute = Attribute.GetCustomAttributes(type);

                        foreach (var subitem in attribute)
                        {
                            if (subitem is CrudAttribute crud)
                            {
                                self = type.MakeGenericType(item);
                                

                                milkshake.Services.AddTransient(i, self);
                                milkshake.HandlerNames.Add(self.ShortDisplayName());
                                break;
                            }
                            
                        }
                    }
                }
                else
                {
                    milkshake.Milkshake.Log($"Type \"{item.ShortDisplayName()}\" is invalid.\n" +
                                            $"Expected implementation of \"{typeof(IMilkshake).ShortDisplayName()}\".",
                        Severity.Warning);
                }
            }

            //Type genericClass = typeof(ICrud);
            //Type iMilkshakeHandler = genericClass.MakeGenericType(self);


            //milkshake.Services.AddTransient(iMilkshakeHandler, item.self);

            //milkshake.Services.AddTransient(generic, a);
            return milkshake;
        }

        public static IMilkshakeConfiguration AddCrud<T>(this IMilkshakeConfiguration milkshake) //where T : ICrud<T>, new()
        {
            //milkshake.Services.AddTransient<ICrud, T>();
            return milkshake;
        }

        public static IMilkshakeConfiguration AddMilkshakeHandler<T>(this IMilkshakeConfiguration milkshake) where T : class, IMilkshake
        {
            Assembly assembly = typeof(T).Assembly;

            Type self = null!;

            Type milkshakeType = null!;

            foreach (var assemblyType in assembly.GetTypes())
            {
                var attribute = Attribute.GetCustomAttributes(assemblyType);

                

                foreach(var item in attribute)
                    if (item is HandleAttribute handle)
                    {
                        milkshakeType = handle.Type;
                        self = assemblyType;
                        break;
                    }

            }
            
            Type genericClass = typeof(IMilkshakeHandler<,>);
            Type iMilkshakeHandler = genericClass.MakeGenericType(milkshakeType, self);

            if (self.IsAssignableTo(iMilkshakeHandler))
            {
                milkshake.Services.AddTransient(iMilkshakeHandler, self);
                milkshake.HandlerNames.Add(self.ShortDisplayName());
            }
            else
                milkshake.Milkshake.Log($"Type \"{self.ShortDisplayName()}\" is invalid.\n" +
                                        $"Expected implementation of \"{iMilkshakeHandler.ShortDisplayName()}\".",
                    Severity.Warning);

            return milkshake;
        }

        public static IMilkshakeConfiguration AddMilkshakeHandler(this IMilkshakeConfiguration milkshake, Assembly? assembly = null)
        {
            assembly ??= milkshake.Assembly;

            var types = new List<(Type milkshakeType, Type self)>(4);

            foreach (var assemblyType in assembly.GetTypes())
            {
                var attribute = Attribute.GetCustomAttributes(assemblyType);

                foreach (var item in attribute)
                    if (item is HandleAttribute handle)
                    {
                        if (types.Any(x => handle.Type == x.milkshakeType)) continue;
                        types.Add((handle.Type, assemblyType));
                        break;
                    }
            }

            var invalidTypes = new List<(Type, Type)>();

            foreach (var item in types)
            {
                var type = item.milkshakeType;
                var self = item.self;

                var genericClass = typeof(IMilkshakeHandler<,>);
                var iMilkshakeHandler = genericClass.MakeGenericType(type, self);

                //var a = Activator.CreateInstance(item.self);

                if(self.IsAssignableTo(iMilkshakeHandler))
                {
                    milkshake.Services.AddScoped(iMilkshakeHandler, self);
                    milkshake.HandlerNames.Add(self.ShortDisplayName());
                }
                else
                {
                    milkshake.Milkshake.Log($"Type \"{item.self.ShortDisplayName()}\" is invalid.\n" +
                                            $"Expected implementation of \"{iMilkshakeHandler.ShortDisplayName()}\".",
                        Severity.Warning);
                    invalidTypes.Add(item);
                }
            }

            var names = new List<string>(milkshake.HandlerNames.Count);
            names.Add("");
            foreach (var item in milkshake.HandlerNames)
            {
                var name = "        " + item;
                names.Add(name);
            }
            names.Add("");
            names.Insert(0, "Handler implementations found:");


            milkshake.Milkshake.Log(string.Join('\n', names), Severity.Debug);

            if (types.Count != 0 && types.Count - invalidTypes.Count != 0) 
                return milkshake;

            milkshake.Milkshake.Log("No best Milkshake types found.", Severity.Warning);
            return milkshake;

        }
        

        public static IMilkshakeConfiguration AddOptions(this IMilkshakeConfiguration milkshake, Action<MilkshakeOptions> options)
        {
            milkshake.Services.Configure(options);
            return milkshake;
        }

        //public static IServiceCollection AddMilkshake(this IServiceCollection services,
        //    Action<MilkshakeConfiguration> configureOptions) => services;

        //public static IServiceCollection AddMilkshake<T, TTemp, THand>
        //    (this IServiceCollection services,
        //        Action<MilkshakeOptions> configureOptions)
        //where T : AbstractCrud
        //where TTemp : class, IMilkshake, new()
        //where THand : class
        //{
        //    services.Configure(configureOptions);

        //    services.AddSingleton<MilkshakeService>();
        //    services.AddTransient<ICrud, T>();

        //    Assembly assembly = Assembly.GetEntryAssembly()!;
        //    var a = assembly.ExportedTypes.ToList();

        //    Type properties;
        //    Type template;
        //    Type handler;

        //    foreach (var type in a)
        //    {
        //        if (!type.IsAssignableTo(typeof(IProperties))) continue;
        //        properties = type;
        //        break;
        //    }

        //    foreach (var type in a)
        //    {
        //        if (!type.IsAssignableTo(typeof(ITemplate<>))) continue;
        //        template = type;
        //        break;
        //    }

        //    foreach (var type in a)
        //    {
        //        if (!type.IsAssignableTo(typeof(IMilkshakeHandler<,>))) continue;
        //        handler = type;
        //        break;
        //    }



        //    services.AddTransient<IMilkshakeHandler<TTemp, THand>>();

        //    return services;
        //}

        //static T Test<T>(T test) where T : class, IMilkshake
        //{
        //    return test;
        //}
    }
}
