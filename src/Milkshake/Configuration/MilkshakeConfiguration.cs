using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Milkshake.Configuration
{
    public interface IMilkshakeConfiguration
    {
        MilkshakeService Milkshake { get; }
        List<string> HandlerNames { get; internal set; }
        IServiceCollection Services { get; }
        Assembly? Assembly { get; }
    }
    public class MilkshakeConfiguration : IMilkshakeConfiguration
    {
        public MilkshakeService Milkshake { get; }
        public List<string> HandlerNames { get; set; } = new List<string>();

        public IServiceCollection Services { get; }
        public Assembly? Assembly { get; }

        public MilkshakeConfiguration(IServiceCollection services, Assembly? assembly, MilkshakeService milkshake)
        {
            Services = services;
            Assembly = assembly ?? Assembly.GetEntryAssembly();
            Milkshake = milkshake;
        }
        //public IMilkshakeConfiguration AddCrud<T>() where T : AbstractCrud
        //{
        //    //_services.Configure(configureOptions);

        //    _services.AddSingleton<MilkshakeService>();
        //    _services.AddTransient<ICrud, T>();
        //    return this;
        //}
        //public IConfigurationProvider Build(IConfigurationBuilder builder)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
